namespace SocialMedia.Models
{
    public class UserRolesViewModel
    {
        public string RoleId { get; set; } = string.Empty;
        public string RoleName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsSelected { get; set; }
    }
}