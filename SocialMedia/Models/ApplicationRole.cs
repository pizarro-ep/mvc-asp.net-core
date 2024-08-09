using Microsoft.AspNetCore.Identity;

namespace SocialMedia.Models{
    public class ApplicationRole : IdentityRole {
        public string? Description { get; set; }
    }
}