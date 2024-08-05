using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BooksManagment.Models{
    public class Book {
        public int Id {get; set;}
        
        [Required(ErrorMessage = "El título del libro es requerido")]
        [StringLength(255)]
        [Display(Name = "Título")]
        public string Title {get;set;}
        
        [StringLength(1000)]
        [Display(Name = "Descipción")]
        public string? Description {get; set;}

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fecha de publicación")]
        public DateTime PublicationDate {get; set;}


        [Required]
        public int GenreID {get; set;}                      // FK Genre

        [Required]
        public int AuthorID {get; set;}                     // FK Author


        public Genre? Genre {get; set;}                     // Book(1) ---> Genre(1)

        public Author? Author {get; set;}                   // Book(1) ---> Author(1)

        public ICollection<Review> Reviews {get; set;}      // Book(1) ---> Review(n)


        public Book(){
            Title = string.Empty;
            Reviews = new HashSet<Review>();
        }
    }
}