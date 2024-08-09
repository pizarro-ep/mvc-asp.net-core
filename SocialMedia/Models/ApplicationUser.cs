using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Models{
    public class ApplicationUser : IdentityUser{
        public string? Name {get; set;}
        public string? LastName {get; set;}
    }
}