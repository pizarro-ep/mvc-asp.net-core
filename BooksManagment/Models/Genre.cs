using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BooksManagment.Models{
    public class Genre {
        public int Id {get; set;}

        [Required(ErrorMessage = "El nombre del género es requerido")]
        [RegularExpression(@"^[A-Z]+[a-zA-Z]*$")]
        [Display(Name = "Género")]
        public string Name {get; set;}


        public ICollection<Book> Books {get; set;}      // Genre(1) ---> Book(n)



        public Genre(){
            Name = string.Empty;
            Books = new HashSet<Book>();
        }
    }
}