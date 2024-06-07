using System.Diagnostics.CodeAnalysis;
using static ApplicationCore.Constants.AnalyticsSettingsConstants;

namespace ApplicationCore.Entities.AnalyticsSettingsAggregate;

public class AnalyticsSettings : EntityBase<Guid>, IEqualityComparer<AnalyticsSettings>, IAggregateRoot
{
    public AnalyticsType AnalyticsType { get; init; }
    public ProcessorType ProcessorType { get; private set; }
    private uint _skipFrames;
    public uint SkipFrames
    {
        private set
        {
            _skipFrames = value >= maxSkipFrames ? maxSkipFrames : value;
        }
        get
        {
            return _skipFrames;
        }
    }

    public AnalyticsSettings(uint skipFrames, ProcessorType processorType, AnalyticsType analyticsType) =>
        (SkipFrames, ProcessorType, AnalyticsType) = (skipFrames, processorType, analyticsType);

    public void UpdateProcessorType(ProcessorType processorType)
    {
        ProcessorType = processorType;
    }

    public void UpdateSkipFrames(uint skipFrames)
    {
        SkipFrames = skipFrames;
    }

    public bool Equals(AnalyticsSettings? x, AnalyticsSettings? y) => x?.Id == y?.Id;

    public int GetHashCode([DisallowNull] AnalyticsSettings obj) => obj.Id.GetHashCode();

}
