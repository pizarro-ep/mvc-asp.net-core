using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BooksManagment.Models{
    public class Role {
        public int Id {get; set;}

        [Required(ErrorMessage = "El rol es requerido")]
        [StringLength(20), Display(Name = "Rol"), RegularExpression(@"^[A-Z]+[a-zA-Z]*$)]")]
        public string Name {get; set;}


        public ICollection<User> Users {get; set;}       // Role(1) ---> User(n)



        public Role(){
            Name = string.Empty;
            Users = new HashSet<User>();
        }
    }
}