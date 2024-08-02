using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace University.Models
{
    public class Instructor
    {
        public int ID { get; set; }

        [Required]
        [Display(Name = "Nombre")]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        //[Column("Apellidos")]
        [Display(Name = "Apellidos")]
        [StringLength(50)]
        public string Surname { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fecha de contratación")]
        public DateTime HireDate { get; set; }

        [Display(Name = "Nombre completo")]
        public string FullName { get { return LastName + " " + Surname; }}

        public ICollection<CourseAssignment>? CourseAssignments { get; set; } // Instructir puede inpartir varios cursos
        
        public OfficeAssignment? OfficeAssignment { get; set; }      // Solo puede tener una oficina
    }
}