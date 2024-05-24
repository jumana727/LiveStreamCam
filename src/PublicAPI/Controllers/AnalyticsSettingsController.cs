using ApplicationCore.Entities.AnalyticsSettingsAggregate;

namespace PublicAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnalyticsSettingsController(IReadRepository<AnalyticsSettings> analyticsSettingsRepository,
    AnalyticsSettingsService analyticsSettingsService)
        : ControllerBase
{
    private readonly IReadRepository<AnalyticsSettings> _analyticsSettingsRepository = analyticsSettingsRepository;
    private readonly AnalyticsSettingsService _analyticsSettingsService = analyticsSettingsService;

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<AnalyticsSettings>> Create(uint SkipFrames, string ProcessorType, string AnalyticsType)
    {
        var newAnalyticsSettings =
            await _analyticsSettingsService.Create(SkipFrames, ProcessorType, AnalyticsType);
        return CreatedAtAction(nameof(Get), new { newAnalyticsSettings.Id }, newAnalyticsSettings);
    }

    [HttpDelete("{analyticsId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(Guid analyticsId)
    {
        await _analyticsSettingsService.Delete(analyticsId);
        return NoContent();
    }

    [HttpPut("{analyticsId}/skipFrames")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateSkipFrames(Guid analyticsId, uint skipFrames)
    {
        var newAnalyticsSettings = await _analyticsSettingsService.UpdateSkipFrames(analyticsId, skipFrames);
        return Ok(newAnalyticsSettings);
    }

    [HttpPut("{analyticsId}/processorType")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateProcessorType(Guid analyticsId, string processorType)
    {
        var newAnalyticsSettings = await _analyticsSettingsService.UpdateProcessorType(analyticsId, processorType);
        return Ok(newAnalyticsSettings);
    }

    [HttpGet("{Id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AnalyticsSettings>> Get(Guid Id)
    {
        OneByIdSpec<AnalyticsSettings> analyticsSettingsSpec = new(Id);
        var analyticsSettings = await _analyticsSettingsRepository.FirstOrDefaultAsync(analyticsSettingsSpec);

        if (analyticsSettings is null)    return NotFound();

        return Ok(analyticsSettings);
    }

}
