using Microsoft.AspNetCore.Identity;

namespace DataLayer.Models;

public class ApplicationRole:IdentityRole<Guid>
{
    public ApplicationRole(string name)
    {
        Name = name;
    }
}