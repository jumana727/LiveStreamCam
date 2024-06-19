using System.Diagnostics;

namespace PublicAPI;

public static class FFmpegService
{
    public static Process StartFFmpegProcess(int height, int width)
    {
        string rtspUrlOutput = "rtsp://mediamtx:8554/sample-out";
        int fps = 25;

        // FFmpeg command
        var ffmpegArgs = $"-y -f rawvideo -vcodec rawvideo -pix_fmt bgr24 -s {width}x{height} -r {fps} -i - " +
                         $"-c:v libx264 -preset ultrafast -f rtsp {rtspUrlOutput}";

        // var ffmpegArgs = $"-y -f rawvideo -vcodec rawvideo -pix_fmt bgr24 -s {width}x{height} -r {fps} -i - " +
        //                  $"-c:v libx264 -preset slow -f rtsp {rtspUrlOutput}";


        var ffmpegProcess = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "ffmpeg",
                Arguments = ffmpegArgs,
                UseShellExecute = false,
                RedirectStandardInput = true,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            }
        };

        ffmpegProcess.Start();

        // Handle ffmpeg errors
        _ = Task.Run(() =>
        {
            // while (!ffmpegProcess.StandardError.EndOfStream)
            // {
            //     string line = ffmpegProcess.Stan:dardError.ReadLine();
            //     Console.WriteLine(line);
            // }
        });

        return ffmpegProcess;
    }
}
