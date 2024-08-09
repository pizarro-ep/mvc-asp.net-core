using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Models{
    public class CreateRoleViewModel{
        [Required]
        [Display(Name = "Nombre del rol")]
        public string RoleName { get; set; } = string.Empty;

        public string? Description { get; set; } = string.Empty;
    }
}