namespace BusinnesLayer.Models.Entities.Trip;

public class UpdateTripBody
{
    public Guid Id { get; set; }
    public string StartRoute { get; set; }
    public string EndRoute { get; set; }
    public DateTime DepartureAt { get; set; }
}