using ApplicationCore.Models;

namespace ApplicationCore;

public interface IAnalyticsService
{
    public IEnumerable<AnalyticsResult> StartAnalytics(string uri, CancellationToken cancellationToken);
}
