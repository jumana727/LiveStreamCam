using System.Diagnostics.CodeAnalysis;

namespace ApplicationCore.Entities.VideoStreamAggregate;

public class VideoStream : EntityBase<Guid>, IEqualityComparer<VideoStream>, IAggregateRoot
{
    public string Uri {get; set;}

    public List<AnalyticsSettings> AnalyticsSettings {get;} = [];

    public VideoStream(string uri)
    {
        Uri = uri;
    }


#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private VideoStream() {}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.


    public bool Equals(VideoStream? x, VideoStream? y) => x?.Id == y?.Id;

    public int GetHashCode([DisallowNull] VideoStream obj) => obj.Id.GetHashCode();

}
