using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BooksManagment.Models {
    public class Author {
        public int Id {get; set;}

        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(100)]
        [RegularExpression(@"^[A-Z]+[a-zA-Z\s]*$")]
        public string Names {get; set;}

        [Required(ErrorMessage = "Los apellidos son requeridos")]
        [StringLength(100)]
        [RegularExpression(@"^[A-Z]+[a-zA-Z\s]*$")]
        public string Surnames {get; set;}

        [StringLength(1000)]
        public string? Biography {get; set;}

        [Required(ErrorMessage = "La fecha de nacimiento es requerido")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fecha de nacimiento")] 
        public DateTime BirthDate {get; set;}


        public ICollection<Book> Books {get; set;}      // Author(1) ---> Book(n)

        // 
        public Author(){
            Names = string.Empty;
            Surnames = string.Empty;
            Books = new HashSet<Book>();                // Initialize
        }
    }
}