using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace University.Models {
    public class Student{
        public int Id {get;set;}

        [Required]
        [StringLength(50)]
        [RegularExpression(@"^[A-Z]+[a-zA-Z]*$")]
        public required string LastName {get;set;}
        
        [Required]
        [StringLength(50)]
        //[Column("Apellidos")]
        [Display(Name = "Apellidos")]
        [RegularExpression(@"^[A-Z]+[a-zA-Z]*$")]
        public required string Surname {get;set;}
        
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fecha de incripción")]
        public DateTime? EnrollmentDate {get;set;}

        [Display(Name = "Nombres completos")]
        public string FullName {get {return LastName +" "+ Surname;} }
        
        public ICollection<Enrollment>? Enrollments {get;set;}
    }
}