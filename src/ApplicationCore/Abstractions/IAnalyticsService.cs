using ApplicationCore.Models;

namespace ApplicationCore;

public interface IAnalyticsService
{
    public IEnumerable<AnalyticsResult> StartAnalyticsAsync(string uri, CancellationToken cancellationToken);
}
