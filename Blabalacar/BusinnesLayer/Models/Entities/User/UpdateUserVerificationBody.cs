namespace BusinnesLayer.Models.Entities.User;

public class UpdateUserVerificationBody
{
    public Guid Id { get; set; }
    public bool isVerificated { get; set; }
}