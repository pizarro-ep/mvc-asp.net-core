using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace University.Models
{
    public class Department
    {
        public int DepartmentID { get; set; }

        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        public decimal Budget { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fecha de inicio")]
        public DateTime StartDate { get; set; }

        public int? InstructorID { get; set; } // un departamento puede tener o no un istructor

        [Timestamp]
        public byte[]? RowVersion {get;set;}

        public Instructor? Administrator { get; set; } // Contiene al instructor pero lo denominamos como Administrador
        public ICollection<Course>? Courses { get; set; } // un departamento puede tener varios cursos
    }
}