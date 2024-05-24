namespace ApplicationCore.Specifications;

public class VideoStreamWithAnalyticsSettingsSpec : Specification<VideoStream>
{
    public VideoStreamWithAnalyticsSettingsSpec(Guid id)
    {
        Query.Where(element => element.Id == id)
            .Include(c => c.AnalyticsSettings);
    }
}
