namespace ApplicationCore.Services;

public class AnalyticsConfigService(ILogger<AnalyticsConfigService> logger,
        IRepository<VideoStream> VideoStreamRepository, 
        IRepository<AnalyticsSettings> analyticsSettingsRepository)
{
    private readonly IRepository<VideoStream> _VideoStreamRepository = VideoStreamRepository;
    private readonly IRepository<AnalyticsSettings> _analyticsSettingsRepository = analyticsSettingsRepository;
    private readonly ILogger<AnalyticsConfigService> _logger = logger;

    public async Task<VideoStream> ApplySettingstoVideoStream(Guid VideoStreamId, List<Guid> analyticsSettingsIdList)
    {
        VideoStreamWithAnalyticsSettingsSpec VideoStreamSpec = new(VideoStreamId);
        var VideoStream = await _VideoStreamRepository.FirstOrDefaultAsync(VideoStreamSpec);
        Guard.Against.Null(VideoStream);

        ListByIdSpec<AnalyticsSettings> analyticsSettingsSpec = new(analyticsSettingsIdList);
        var analyticsSettingsList = await _analyticsSettingsRepository.ListAsync(analyticsSettingsSpec);
        Guard.Against.NullOrEmpty(analyticsSettingsList);

        VideoStream.AnalyticsSettings.AddRangeUnique(analyticsSettingsList);
        await _VideoStreamRepository.UpdateAsync(VideoStream);
        return VideoStream;
    }

    public async Task RemoveSettingsFromVideoStream(Guid VideoStreamId, List<Guid> analyticsSettingsIdList)
    {
        VideoStreamWithAnalyticsSettingsSpec VideoStreamSpec = new(VideoStreamId);
        var VideoStream = await _VideoStreamRepository.FirstOrDefaultAsync(VideoStreamSpec);
        Guard.Against.Null(VideoStream);

        VideoStream.AnalyticsSettings.RemoveAll(a => analyticsSettingsIdList.Contains(a.Id));

        await _VideoStreamRepository.UpdateAsync(VideoStream);
    }

    // public async Task<AnalyticsSettings> ApplySettingToVideoStreams(Guid analyticsSettingsId, List<Guid> VideoStreamIdList)
    // {
    //     AnalyticsSettingsWithVideoStreamsSpec analyticsSettingsSpec = new(analyticsSettingsId);
    //     var analyticsSettings = await _analyticsSettingsRepository.FirstOrDefaultAsync(analyticsSettingsSpec);
    //     Guard.Against.Null(analyticsSettings);

    //     ListByIdSpec<VideoStream> VideoStreamListSpec = new(VideoStreamIdList);
    //     var VideoStreamList = await _VideoStreamRepository.ListAsync(VideoStreamListSpec);
    //     Guard.Against.NullOrEmpty(VideoStreamList);

    //     analyticsSettings.VideoStreams.AddRangeUnique(VideoStreamList);
    //     await _analyticsSettingsRepository.UpdateAsync(analyticsSettings);
    //     return analyticsSettings;
    // }

    // public async Task RemoveSettingFromVideoStreams(Guid analyticsSettingsId, List<Guid> VideoStreamIdList)
    // {
    //     AnalyticsSettingsWithVideoStreamsSpec VideoStreamSpec = new(analyticsSettingsId);
    //     var analyticsSettings = await _analyticsSettingsRepository.FirstOrDefaultAsync(VideoStreamSpec);
    //     Guard.Against.Null(analyticsSettings);

    //     analyticsSettings.VideoStreams.RemoveAll(c => VideoStreamIdList.Contains(c.Id));

    //     await _analyticsSettingsRepository.UpdateAsync(analyticsSettings);
    // }

}
