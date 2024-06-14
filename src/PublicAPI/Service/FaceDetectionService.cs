using ApplicationCore;
using FaceDetectionSDK;
using ApplicationCore.Models;
using OpenCvSharp;
using System.Diagnostics;

namespace PublicAPI.Services;

public class FaceDetectionService : IAnalyticsService, IDisposable
{

    private readonly ulong _sdkInstance;
    private readonly ILogger<FaceDetectionService> _logger;

    private int skipCount;

    private const int SKIP_FRAMES = 5;

    private AnalyticsResult previousResult;

    public FaceDetectionService(ILogger<FaceDetectionService> logger)
    {
        _logger = logger;

        string modelDir = "/app/publish/libForSDKs";
        int status = FaceDetector.initializeLibraryInstance(out ulong libInstance, modelDir);
        logger.LogDebug("Library Initialization Status: {status}", status);

        string imgPath = "/app/publish/libForSDKs/init.jpeg";
        var sdkInitializationStatus = FaceDetector.initializeSdkInstance(imgPath, 0, in libInstance, out _sdkInstance);
        _logger.LogDebug("Sdk Initialization Status : {status}", sdkInitializationStatus);

        status = FaceDetector.releaseLibraryInstance(in libInstance);
        _logger.LogDebug("Library Release Status: {status}", status);
    }

    public IEnumerable<AnalyticsResult>? StartAnalyticsAsync(string uri, CancellationToken cancellationToken)
    {
        using VideoCapture capture = new(uri);
        if (!capture.IsOpened())
            throw new Exception($"Failed to open the video uri.");// for Camera( ID={cameraId}, Url={cameraInfo.CameraUrl} ).");


        //set skipCount to zero
        skipCount = 0;

        Process ffmpegProcess;

        try
        {
            using Mat testFrame = new();
            if (capture.Read(testFrame))
            {
                ffmpegProcess = FFmpegService.StartFFmpegProcess(testFrame.Height, testFrame.Width);
            }
            else
            {
                throw new Exception("Test Frame cannot be read!!");
            }
        }
        catch (Exception e)
        {
            _logger.LogError("Error in ffmpeg process");
            throw new Exception(e.Message);
        }


        while (!cancellationToken.IsCancellationRequested)
        {
            // saving video for debugging
            // VideoWriter videoWriter = new VideoWriter(videoName, FourCC.XVID, fps, new OpenCvSharp.Size(width, height), true);


            using Mat frame = new();
            bool readResult = capture.Read(frame);
            if (!readResult) break;

            var tempImageName = $"{Path.GetTempFileName()}.jpeg";

            frame.SaveImage(tempImageName);

            if (skipCount <= SKIP_FRAMES)
            {
                skipCount++;

                //send to ffmpeg process
                byte[] buffer = new byte[frame.Width * frame.Height * frame.ElemSize()];

                var detectedFrame = DrawBoundingBox(frame, previousResult ?? new AnalyticsResult(0, 0, 0, 0, 0, 0, DateTime.Now));

                buffer = MatToByteArray(detectedFrame);

                ffmpegProcess.StandardInput.BaseStream.WriteAsync(buffer, 0, buffer.Length);

                yield return null;
            }

            else
            {
                var fdResult = DetectFace(tempImageName);

                var detectedFrame = DrawBoundingBox(frame, fdResult);

                int width = detectedFrame.Width;
                int height = detectedFrame.Height;

                Console.WriteLine($"height and width {height} {width}");

                // Send to ffmpeg process through pipe
                byte[] buffer = new byte[width * height * detectedFrame.ElemSize()];

                buffer = MatToByteArray(detectedFrame);

                ffmpegProcess.StandardInput.BaseStream.WriteAsync(buffer, 0, buffer.Length);

                if (!detectedFrame.SaveImage(tempImageName))
                    _logger.LogError("Cannot save result image");

                skipCount = 0;
                previousResult = fdResult;

                yield return fdResult;
            }
            // File.Delete(tempImageName);
        }

    }

    private static byte[] MatToByteArray(Mat mat)
    {
        int size = mat.Rows * mat.Cols * mat.ElemSize();
        byte[] data = new byte[size];
        System.Runtime.InteropServices.Marshal.Copy(mat.Data, data, 0, size);
        return data;
    }


    private AnalyticsResult DetectFace(string imgPath)
    {
        int benchmark = 0;

        int result = FaceDetector.detectFace(out float a, out float b, out float c, out float d, out float e, imgPath, benchmark, in _sdkInstance);
        _logger.LogDebug($"{a} {b} {c} {d} {e} {result}");

        if (result != 0)
        {
            _logger.LogError("Failed to detect face.");
        }

        var fdResult = new AnalyticsResult(a, b, c, d, e, result, DateTime.Now);

        return fdResult;
    }

    private static Mat DrawBoundingBox(Mat imageFrame, AnalyticsResult fDResults)
    {
        var resultRectangle = new Rect((int)fDResults.X, (int)fDResults.Y, (int)fDResults.Height, (int)fDResults.Width);
        Cv2.Rectangle(imageFrame, resultRectangle, Scalar.Green, 2);

        return imageFrame;
    }

    // ~FaceDetectionService()
    // {
    //     Dispose();
    // }

    public void Dispose()
    {
        var sdkReleaseStatus = FaceDetector.releaseSdkInstance(in _sdkInstance);
        _logger.LogDebug("Sdk Release Status: {status}", sdkReleaseStatus);

        GC.SuppressFinalize(this);
    }
}
