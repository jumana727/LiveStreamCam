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

    public async Task Update(Guid VideoStreamId, string VideoStreamUri)
    {
        var videoStream = await _VideoStreamRepository.GetByIdAsync(VideoStreamId) ?? throw new Exception("VideoStream does not exist!");

        videoStream.Uri = VideoStreamUri;

        await _VideoStreamRepository.UpdateAsync(videoStream);
    }

}
