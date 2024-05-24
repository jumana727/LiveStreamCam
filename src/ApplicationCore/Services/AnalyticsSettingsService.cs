using static ApplicationCore.Helpers.EnumHelper;

namespace ApplicationCore.Services;

public class AnalyticsSettingsService(IRepository<AnalyticsSettings> analyticsSettingsRepository,
    ILogger<AnalyticsSettingsService> logger)
{
    private readonly IRepository<AnalyticsSettings> _analyticsSettingsRepository = analyticsSettingsRepository;
    private readonly ILogger<AnalyticsSettingsService> _logger = logger;


    public async Task<AnalyticsSettings> Create(uint skipFrames, string processorType, string analyticsType)
    {
        AnalyticsSettings newAnalyticsSettings = new(
            skipFrames,
            GetEnum<ProcessorType>(processorType),
            GetEnum<AnalyticsType>(analyticsType)
        );
        newAnalyticsSettings = await _analyticsSettingsRepository.AddAsync(newAnalyticsSettings);
        return newAnalyticsSettings;
    }

    public async Task Delete(Guid id)
    {
        OneByIdSpec<AnalyticsSettings> analyticsSettingsSpec = new(id);
        await _analyticsSettingsRepository.DeleteRangeAsync(analyticsSettingsSpec);
    }

    public async Task<AnalyticsSettings> UpdateSkipFrames(Guid id, uint newSkipFrames)
    {
        OneByIdSpec<AnalyticsSettings> analyticsSettingsSpec = new(id);
        var analyticsSettings = await _analyticsSettingsRepository.FirstOrDefaultAsync(analyticsSettingsSpec);
        Guard.Against.Null(analyticsSettings, nameof(analyticsSettings));

        analyticsSettings.UpdateSkipFrames(newSkipFrames);
        await _analyticsSettingsRepository.UpdateAsync(analyticsSettings);
        return analyticsSettings;
    }

    public async Task<AnalyticsSettings> UpdateProcessorType(Guid id, string processorType)
    {
        OneByIdSpec<AnalyticsSettings> analyticsSettingsSpec = new(id);
        var analyticsSettings = await _analyticsSettingsRepository.FirstOrDefaultAsync(analyticsSettingsSpec);
        Guard.Against.Null(analyticsSettings, nameof(analyticsSettings));

        analyticsSettings.UpdateProcessorType(GetEnum<ProcessorType>(processorType));
        await _analyticsSettingsRepository.UpdateAsync(analyticsSettings);
        return analyticsSettings;
    }

}
