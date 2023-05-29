namespace BusinnesLayer.Models.Entities.Trip;

public class FoundTripBody
{
    public string startRoute { get; set; }
    public string endRoute { get; set; }

    public FoundTripBody(string startRoute, string endRoute)
    {
        this.startRoute = startRoute;
        this.endRoute = endRoute;
    }

    public FoundTripBody()
    {
        
    }
}