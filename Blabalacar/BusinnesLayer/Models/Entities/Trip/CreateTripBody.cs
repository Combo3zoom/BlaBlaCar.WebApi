namespace BusinnesLayer.Models.Entities.Trip;

public class CreateTripBody
{
    public string StartRoute { get; set; }
    public string EndRoute { get; set; }
    public DateTime DepartureAt { get; set; }
}