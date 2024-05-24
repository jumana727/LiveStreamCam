using ApplicationCore.Entities.VideoStreamAggregate;

namespace PublicAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VideoStreamController(IReadRepository<VideoStream> VideoStreamRepository, VideoStreamService VideoStreamService) :
    ControllerBase
{
    private readonly IReadRepository<VideoStream> _VideoStreamRepository = VideoStreamRepository;
    private readonly VideoStreamService _VideoStreamService = VideoStreamService;

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<VideoStream>> Create(string uri)
    {
        var newVideoStream = await _VideoStreamService.Create(uri);

        // Handle client bad request and server side errors

        if (newVideoStream is null) return BadRequest();

        // return CreatedAtAction(nameof(Get), new Dictionary<string, object>() { {"VideoStreamId", newVideoStream.Id} }, newVideoStream);
        return CreatedAtAction(nameof(Get),  new {newVideoStream.Id} , newVideoStream);
    }

    [HttpGet("{Id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<VideoStream>> Get(Guid Id)
    {
        OneByIdSpec<VideoStream> VideoStreamSpec = new(Id);
        var VideoStream = await _VideoStreamRepository.FirstOrDefaultAsync(VideoStreamSpec);

        if (VideoStream is null)    return NotFound();

        return Ok(VideoStream);
    }

    [HttpDelete("{VideoStreamId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(Guid VideoStreamId)
    {
        await _VideoStreamService.Delete(VideoStreamId);

        //Handle client bad request and server side errors

        return NoContent();
    }

}
