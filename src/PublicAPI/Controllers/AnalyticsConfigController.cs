using ApplicationCore.Entities.AnalyticsSettingsAggregate;
using ApplicationCore.Entities.VideoStreamAggregate;

namespace PublicAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnalyticsConfigController(IReadRepository<VideoStream> VideoStreamRepository,
    IReadRepository<AnalyticsSettings> analyticsSettingsRepository,
    AnalyticsConfigService analyticsConfigService)
        : ControllerBase
{
    private readonly IReadRepository<VideoStream> _VideoStreamRepository = VideoStreamRepository;
    private readonly IReadRepository<AnalyticsSettings> _analyticsSettingsRepository = analyticsSettingsRepository;
    private readonly AnalyticsConfigService _analyticsConfigService = analyticsConfigService;

    [HttpPost("{VideoStreamId}/analyticsSettings/")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<VideoStream>> ApplySettingstoVideoStream(Guid VideoStreamId, List<Guid> analyticsSettingsIds)
    {
        var VideoStream = await _analyticsConfigService.ApplySettingstoVideoStream(VideoStreamId, analyticsSettingsIds);
        return CreatedAtAction(nameof(GetVideoStream), new { VideoStreamId = VideoStream.Id }, VideoStream);
    }

    // [HttpPost("{analyticsSettingsId}/VideoStream/")]
    // [ProducesResponseType(StatusCodes.Status201Created)]
    // public async Task<ActionResult<VideoStream>> ApplySettingtoVideoStreams(Guid analyticsSettingsId, List<Guid> VideoStreamIds)
    // {
    //     var analyticsSettings = await _analyticsConfigService.ApplySettingToVideoStreams(analyticsSettingsId, VideoStreamIds);
    //     return CreatedAtAction(nameof(GetAnalyticsSettings), new { analyticsSettings.Id }, analyticsSettings);
    // }

    [HttpDelete("{VideoStreamId}/analyticsSettings/")]
    public async Task<IActionResult> RemoveSettingsFromVideoStream(Guid VideoStreamId, List<Guid> analyticsSettingsIds)
    {
        await _analyticsConfigService.RemoveSettingsFromVideoStream(VideoStreamId, analyticsSettingsIds);
        return NoContent();
    }

    // [HttpDelete("{analyticsSettingsId}/VideoStream/")]
    // public async Task<IActionResult> RemoveSettingFromVideoStreams(Guid analyticsSettingsId, List<Guid> VideoStreamIds)
    // {
    //     await _analyticsConfigService.RemoveSettingFromVideoStreams(analyticsSettingsId, VideoStreamIds);
    //     return NoContent();
    // }

    [HttpGet("VideoStream/{VideoStreamId}")]
    public async Task<ActionResult<VideoStream>> GetVideoStream(Guid VideoStreamId)
    {
        VideoStreamWithAnalyticsSettingsSpec VideoStreamSpec = new(VideoStreamId);
        var VideoStream = await _VideoStreamRepository.ListAsync(VideoStreamSpec);
        return Ok(VideoStream);
    }

    // [HttpGet("analyticsSettings/{analyticsSettingsId}")]
    // public async Task<ActionResult<AnalyticsSettings>> GetAnalyticsSettings(Guid analyticsSettingsId)
    // {
    //     AnalyticsSettingsWithVideoStreamsSpec analyticsSettingsSpec = new(analyticsSettingsId);
    //     var analyticsSettings = await _analyticsSettingsRepository.ListAsync(analyticsSettingsSpec);
    //     return Ok(analyticsSettings);
    // }

}
