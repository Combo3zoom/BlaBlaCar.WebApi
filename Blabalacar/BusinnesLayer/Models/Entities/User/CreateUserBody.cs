using System.ComponentModel.DataAnnotations;

namespace BusinnesLayer.Models.Entities.User;

public class CreateUserBody
{
    [Required]
    [NameValidation("^[A-z]{1}[a-z]{3,16}$",ErrorMessage = "Name is incorrect")]
    public string Name { get; set; }
    public bool IsVerification { get; set; }
}