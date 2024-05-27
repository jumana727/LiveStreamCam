using ApplicationCore;
using ApplicationCore.Entities.VideoStreamAggregate;
using Ardalis.GuardClauses;
using Microsoft.AspNetCore.SignalR;
using NuGet.Protocol;
using PublicAPI.Hubs;

namespace PublicAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnalyticsController : ControllerBase
{
    private static readonly object threadLock = new();
    private static readonly Dictionary<string, CancellationTokenSource> activeThreads = [];

    private readonly IReadRepository<VideoStream> _VideoStreamRepository;
    private readonly IHubContext<AnalyticsResultsHub> _hubContext;
    private readonly ILogger<AnalyticsController> _logger;
    private readonly AnalyticsSettingsService _analyticsSettingsService;

    public AnalyticsController(IReadRepository<VideoStream> VideoStreamRepository,
        ILogger<AnalyticsController> logger,
        IHubContext<AnalyticsResultsHub> hubContext,
        AnalyticsSettingsService analyticsSettingsService)
    {
        _VideoStreamRepository = VideoStreamRepository;
        _logger = logger;
        _hubContext = hubContext;
        _analyticsSettingsService = analyticsSettingsService;
    }

    [HttpGet("StartAnalytics")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> StartAnalytics(Guid videoStreamId, Guid analyticsSettingsId, IServiceProvider serviceProvider)
    {
        _logger.LogDebug("AnalyticsController | StartAnalytics ({VideoStreamId}:{AnalyticsSettingsId}).", videoStreamId, analyticsSettingsId);


        // Dummy AnalyticsSetting
        var dummyAnalytics = await _analyticsSettingsService.Create(100, ProcessorType.Gpu.ToString(), AnalyticsType.FaceDetection.ToString());

        var groupId = (videoStreamId.ToString() + analyticsSettingsId.ToString()).ToLower();
        if (activeThreads.ContainsKey(groupId))  return Ok(new {Message = $"Already running."});

        VideoStreamWithAnalyticsSettingsSpec VideoStreamSpec = new(videoStreamId);
        var videoStream = await _VideoStreamRepository.FirstOrDefaultAsync(VideoStreamSpec);
        Guard.Against.Null(videoStream, nameof(videoStream));

        // var analyticsSettings = videoStream.AnalyticsSettings.Where(a => a.Id.ToString() == dummyAnalytics.Id.ToString()).FirstOrDefault();
        // Guard.Against.Null(analyticsSettings, nameof(analyticsSettings));
        var analyticsSettings = dummyAnalytics;

        var scope = serviceProvider.CreateScope();
        var scopedServiceProvider = scope.ServiceProvider;
        var analyticsService = (IAnalyticsService)scopedServiceProvider.GetRequiredKeyedService(typeof(IAnalyticsService), analyticsSettings.AnalyticsType);

        CancellationTokenSource cancellationTokenSource = new();
        Thread analyticsThread = new(() =>
        {
            try
            {
                foreach (var result in analyticsService.StartAnalytics(videoStream.Uri, cancellationTokenSource.Token))
                {
                    _hubContext.Clients.Group(groupId).SendAsync("result", result.ToJson().ToString());
                    Console.WriteLine("signalr sending");
                }
            }
            finally
            {
                // Ensure cleanup
                scope.Dispose();
            }
        });

        lock (threadLock)
        {
            var addResult = activeThreads.TryAdd(groupId, cancellationTokenSource);
            
            if (addResult)
            {
                analyticsThread.Start();
                return Ok();
            }
            else    return StatusCode(StatusCodes.Status500InternalServerError); 
        }
    }

    [HttpGet("StopAnalytics")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult StopAnalytics(Guid videoStreamId, Guid analyticsSettingsId)
    {
        var groupId = (videoStreamId.ToString() + analyticsSettingsId.ToString()).ToLower();

        lock (threadLock)
        {
            var removeResult = activeThreads.Remove(groupId, out var cancellationTokenSource);

            if (removeResult)
            {
                cancellationTokenSource?.Cancel();
                _hubContext.Clients.Group(groupId).SendAsync("stop");
                return Ok();
            }
            else return Ok(new { Message = $"StartAnalytics is already not running for (VideoStreamId={videoStreamId}:analyticsSettingsId={analyticsSettingsId})." });
        }
    }

}
