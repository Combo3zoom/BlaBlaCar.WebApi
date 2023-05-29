using System.ComponentModel.DataAnnotations;

namespace BusinnesLayer.Models.Entities.User;

public class UpdateUserBody
{
    [Required]
    public string? Name { get; set; }
}