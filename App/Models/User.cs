using System.ComponentModel.DataAnnotations;

namespace App.Models
{
    public class User{
        public int Id {get; set;}

        [Required]
        [StringLength(20, MinimumLength = 4)]
        public required string Username {get; set;}

        [DataType(DataType.EmailAddress)]
        [MaxLength(50)]
        public string? Email {get; set;}

        [DataType(DataType.Password)]
        [StringLength(16, MinimumLength = 8)]
        public required string Password {get; set;}

    }
}