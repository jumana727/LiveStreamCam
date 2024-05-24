namespace ApplicationCore.Services;

public class VideoStreamService(IRepository<VideoStream> VideoStreamRepository, ILogger<VideoStreamService> logger)
{
    private readonly IRepository<VideoStream> _VideoStreamRepository = VideoStreamRepository;
    private readonly ILogger<VideoStreamService> _logger = logger;

    public async Task<VideoStream> Create(string rtspUrl)
    {
        VideoStream newVideoStream = new(rtspUrl);
        newVideoStream = await _VideoStreamRepository.AddAsync(newVideoStream);
        return newVideoStream;
    }

    public async Task Delete(Guid id)
    {
        OneByIdSpec<VideoStream> VideoStreamSpec = new(id);
        await _VideoStreamRepository.DeleteRangeAsync(VideoStreamSpec);
    }

}
