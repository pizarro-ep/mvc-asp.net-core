using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BooksManagment.Models{
    public class User {
        public int Id {get; set;}

        [Required(ErrorMessage = "El nombre de usuario es requerido")]        
        [StringLength(50, MinimumLength = 4, ErrorMessage = "El nombre de usuario debe tener entre 4 y 50 caracteres")]
        [RegularExpression(@"^[a-zA-Z0-9]*$")]
        [Display(Name = "Nombre de usuario")]
        public string Username {get; set;}

        [Required(ErrorMessage = "La contraseña es requerida")]
        [StringLength(25, MinimumLength = 8, ErrorMessage = "La contraseña debe tener como minimo de 8 caracteres y máximo de 20 caracteres")]
        [Display(Name = "Contraseña")]
        public string Password {get; set;}
        
        [Required(ErrorMessage = "El correo es requerido.")]
        [EmailAddress(ErrorMessage = "Correo inválido.")]
        [Display(Name = "Correo")]
        public string Email {get; set;}


        [Required]
        public int RoleID {get; set;}                       // FK Role
        
        public Role? Role {get; set;}                        // User(1) ---> Role(1)

        public ICollection<Review> Reviews {get; set;}      // User(1) ---> Review(n)


        
        public User(){
            Username = string.Empty;
            Password = string.Empty;
            Email = string.Empty;
            Reviews = new HashSet<Review>();
        }
    }
}