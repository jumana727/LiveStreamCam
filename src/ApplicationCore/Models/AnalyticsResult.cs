namespace ApplicationCore.Models;

public class AnalyticsResult
{
    public Guid Id { get; set; }

    public float X;
    public float Y;
    public float Width;
    public float Height;
    public float Score;
    public int status;
    public DateTime dateTime;

    public AnalyticsResult(float X, float Y, float Width, float Height, float Score, int status, DateTime dateTime)
    {
        this.X = X;
        this.Y = Y;
        this.Width = Width;
        this.Height = Height;
        this.Score = Score;
        this.status = status;
        this.dateTime = dateTime;
    }
}
