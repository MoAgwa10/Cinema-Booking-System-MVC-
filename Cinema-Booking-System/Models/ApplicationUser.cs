using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

public class ApplicationUser : IdentityUser
{
    [Display(Name = "Full name")]
    public string FirstName { get; set; }

    public string LastName { get; set; }
}
