using System.Net.Http.Json;
using System.Text.Json;

namespace ApplicationCore.Services;

public class VideoStreamService(IRepository<VideoStream> VideoStreamRepository, ILogger<VideoStreamService> logger, HttpClient httpClient)
{
    private const string mediaMtxControlAPIEndpoint = "http://localhost:9997/v3/config/paths";  
    private readonly IRepository<VideoStream> _VideoStreamRepository = VideoStreamRepository;
    private readonly ILogger<VideoStreamService> _logger = logger;
    private readonly HttpClient _httpClient = httpClient;

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

    public async Task<HttpResponseMessage> Start(Guid videoStreamId, string streamName)
    {
        var videoStream = await _VideoStreamRepository.GetByIdAsync(videoStreamId) ?? throw new Exception("VideoStream does not exist!");

        var requestData = new
        {
            name = streamName,
            source = videoStream.Uri
        };

        var content = new StringContent(JsonSerializer.Serialize(requestData));

        return await _httpClient.PostAsync($"{mediaMtxControlAPIEndpoint}/add/{streamName}", content);
    }

    public async Task<HttpResponseMessage> Stop(string streamName)
    {
        return await _httpClient.DeleteAsync($"{mediaMtxControlAPIEndpoint}/delete/{streamName}");
    }

}
