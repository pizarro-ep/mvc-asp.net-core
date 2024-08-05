using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BooksManagment.Models {
    public class Review {
        public int Id {get; set;}

        [Required(ErrorMessage = "El ranting es requerido")]
        [Range(1, 5)]
        public int Rating {get; set;}

        [StringLength(1000)]
        [Display(Name = "Comentario")]
        public string? Comment {get; set;}

        [Required(ErrorMessage = "La fecha de reseña es requerido")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fecha de reseña")]
        public DateTime ReviewDate {get; set;}


        [Required]
        public int BookID {get; set;}           // FK Book

        [Required]
        public int UserID {get; set;}           // FK User

        // Navigation properties
        public Book? Book {get; set;}            // Review(1) ---> Book(1)

        public User? User {get; set;}            // Review(1) ---> User(1)



        public Review(){}
    }
}