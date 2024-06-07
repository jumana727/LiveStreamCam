using ApplicationCore;
using FaceDetectionSDK;
using ApplicationCore.Models;
using OpenCvSharp;

namespace PublicAPI.Services;

public class FaceDetectionService : IAnalyticsService, IDisposable
{

    private readonly ulong _sdkInstance;
    private readonly ILogger<FaceDetectionService> _logger;

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

    public IEnumerable<AnalyticsResult> StartAnalytics(string uri, CancellationToken cancellationToken)
    {
        using VideoCapture capture = new(uri);
        if (!capture.IsOpened())
            throw new Exception($"Failed to open the video uri.");// for Camera( ID={cameraId}, Url={cameraInfo.CameraUrl} ).");

        while (!cancellationToken.IsCancellationRequested)
        {
            using Mat frame = new();
            bool readResult = capture.Read(frame);
            if (!readResult) break;

            var tempImageName = $"{Path.GetTempFileName()}.jpeg";

            frame.SaveImage(tempImageName);

            var fdResult = DetectFace(tempImageName);

            var detectedFrame = DrawBoundingBox(frame, fdResult);

            if (!detectedFrame.SaveImage(tempImageName))
                _logger.LogError("Cannot save result image");

            // File.Delete(tempImageName);

            yield return fdResult;
        }

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
