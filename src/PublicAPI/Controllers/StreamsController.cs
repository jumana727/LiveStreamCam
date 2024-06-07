using ApplicationCore.Entities.VideoStreamAggregate;

namespace PublicAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VideoStreamController(IReadRepository<VideoStream> VideoStreamRepository, VideoStreamService VideoStreamService) :
    ControllerBase
{
    private readonly IReadRepository<VideoStream> _VideoStreamRepository = VideoStreamRepository;
    private readonly VideoStreamService _VideoStreamService = VideoStreamService;

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<VideoStream>>> GetAll()
    {
        var VideoStreamList = await _VideoStreamRepository.ListAsync();

        if (VideoStreamList is null) return NotFound();

        return Ok(VideoStreamList);
    }


    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<VideoStream>> Create([FromBody] string uri)
    {
        var newVideoStream = await _VideoStreamService.Create(uri);

        // Handle client bad request and server side errors

        if (newVideoStream is null) return BadRequest();

        // return CreatedAtAction(nameof(Get), new Dictionary<string, object>() { {"VideoStreamId", newVideoStream.Id} }, newVideoStream);
        return CreatedAtAction(nameof(Get), new { newVideoStream.Id }, newVideoStream);
    }

    [HttpGet("{Id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<VideoStream>> Get(Guid Id)
    {
        OneByIdSpec<VideoStream> VideoStreamSpec = new(Id);
        var VideoStream = await _VideoStreamRepository.FirstOrDefaultAsync(VideoStreamSpec);

        if (VideoStream is null) return NotFound();

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

    [HttpPut]
    public async Task Update(Guid VideoStreamId, string VideoStreamUri)
    {
        await _VideoStreamService.Update(VideoStreamId, VideoStreamUri);
    }

    [HttpGet("start/{VideoStreamId}/{streamName}")]
    public async Task<ActionResult> Start(Guid VideoStreamId, string streamName)
    {
        var response = await _VideoStreamService.Start(VideoStreamId, streamName);

        if (response.IsSuccessStatusCode)
        {
            return Ok();
        }

        return BadRequest();
    }

    [HttpGet("stop/{streamName}")]
    public async Task<ActionResult> Stop(string streamName)
    {
        var response = await _VideoStreamService.Stop(streamName);

        if (response.IsSuccessStatusCode)
        {
            return Ok();
        }

        return BadRequest();
    }

    public static string GetRtspStreamPath(string url)
    {
        if (string.IsNullOrEmpty(url))
        {
            return null;
        }

        // Split URL by protocol and path
        var parts = url.Split("//", 2);
        if (parts.Length < 2)
        {
            return null;
        }

        // Extract path after port (if any)
        var pathAndPort = parts[1].Split(':');
        if (pathAndPort.Length < 2)
        {
            return pathAndPort[0]; // No port specified, return entire path
        }

        return pathAndPort[1];
    }

}
