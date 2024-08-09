using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "El nombre es requerido.")]
        [Display(Name = "Nombres")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Los apellidos son requerido.")]
        [Display(Name = "Apellidos")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "El correo es requerido.")]
        [EmailAddress(ErrorMessage = "Correo inválido.")]
        [Remote(action: "IsEmailAvailable", controller: "Account")]
        [Display(Name = "Correo")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es requerida.")]
        //[StringLength(25, MinimumLength = 8, ErrorMessage = "La contraseña debe tener como mínimo 8 caracteres y máximo 20 caracteres")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "La contraseña y la contraseña de confirmación no coinciden.")]
        [Display(Name = "Confirmar contraseña")]
        public string ConfirmPassword {get; set;} = string.Empty;
    }
}