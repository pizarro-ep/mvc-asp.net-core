using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Models{
    public class EditRoleViewModel{
        [Required]
        public string Id { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre del rol es requerido")]
        [Display(Name = "Nombre del rol")]
        public string RoleName { get; set; } = string.Empty;

        public string? Description { get; set; } = string.Empty;

        public List<string>? Users { get; set; } = new List<string>();
    }
}