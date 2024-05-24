// using ApplicationCore.Entities.AnalyticsSettingsAggregate;
// using ApplicationCore.Entities.VideoStreamAggregate;
// using ApplicationCore.Entities.VideoStreamGroupAggregate;
// using ApplicationCore.Enums;

// namespace Infrastructure.Data;

// public static class AppDbContextSeed
// {

//     public static void Seed(AppDbContext context)
//     {
//         if (context.VideoStream.Any())   return;

//         var VideoStreams = GetVideoStreams().ToArray();
//         context.VideoStream.AddRange(VideoStreams);
//         context.SaveChanges();

//         var VideoStreamGroups = GetVideoStreamGroups().ToArray();
//         context.VideoStreamGroup.AddRange(VideoStreamGroups);
//         context.SaveChanges();

//         var analyticsSettings = GetAnalyticsSettings().ToArray();
//         context.AnalyticsSettings.AddRange(analyticsSettings);
//         context.SaveChanges();

//         GroupVideoStreams(VideoStreams, VideoStreamGroups);
//         context.SaveChanges();

//         ApplySettings(VideoStreams, analyticsSettings);
//         context.SaveChanges();
//     }

//     public static async Task SeedAsync(AppDbContext context)
//     {
//         if (await context.VideoStream.AnyAsync())   return;

//         var VideoStreams = GetVideoStreams().ToArray();
//         await context.VideoStream.AddRangeAsync(VideoStreams);
//         await context.SaveChangesAsync();

//         var VideoStreamGroups = GetVideoStreamGroups().ToArray();
//         await context.VideoStreamGroup.AddRangeAsync(VideoStreamGroups);
//         await context.SaveChangesAsync();

//         var analyticsSettings = GetAnalyticsSettings().ToArray();
//         await context.AnalyticsSettings.AddRangeAsync(analyticsSettings);
//         await context.SaveChangesAsync();

//         GroupVideoStreams(VideoStreams, VideoStreamGroups);
//         await context.SaveChangesAsync();

//         ApplySettings(VideoStreams, analyticsSettings);
//         await context.SaveChangesAsync();
//     }


//     static IEnumerable<VideoStream> GetVideoStreams()
//         => [
//             new("rtsp://dummy1"),
//             new("rtsp://dummy2"), 
//             new("rtsp://dummy3"), 
//             new("rtsp://dummy4"), 
//             new("rtsp://dummy5"), 
//         ];

//     static IEnumerable<VideoStreamGroup> GetVideoStreamGroups()
//         => [
//             new("Group1"),
//             new("Group2"),
//             new("Group3"),
//             new("Group4"),
//             new("Group5"),
//         ];

//     static IEnumerable<AnalyticsSettings> GetAnalyticsSettings()
//         => [
//             new(0, ProcessorType.Gpu, AnalyticsType.FaceDetection),
//             new(0, ProcessorType.Gpu, AnalyticsType.ObjectDetection),
//             new(0, ProcessorType.Gpu, AnalyticsType.NumberPlateDetection),
//             new(2, ProcessorType.Gpu, AnalyticsType.FaceDetection),
//             new(1, ProcessorType.Cpu, AnalyticsType.FaceDetection),
//             new(1, ProcessorType.Any, AnalyticsType.FaceDetection),
//         ];

//     static void GroupVideoStreams(VideoStream[] VideoStreams, VideoStreamGroup[] VideoStreamGroups)
//     {
//         VideoStreamGroups[0].VideoStreams.AddRange(VideoStreams[..3]);
//         VideoStreamGroups[1].VideoStreams.AddRange(VideoStreams[3..5]);
//     }

//     static void ApplySettings(VideoStream[] VideoStreams, AnalyticsSettings[] analyticsSettings)
//     {
//         VideoStreams[0].AnalyticsSettings.AddRange(analyticsSettings[0..3]);
//         analyticsSettings[1].VideoStreams.AddRange(VideoStreams[1..3]);
//     }

// }
