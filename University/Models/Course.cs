using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace University.Models {
    public class Course {
        [DatabaseGenerated(DatabaseGeneratedOption.None)] // Permite especificar la clave principal
        [Display(Name = "Number")]
        public int CourseID {get;set;}
        
        [StringLength(50, MinimumLength = 3)]
        public required string Title {get;set;}

        [Range(0, 5)]
        public required int Credits {get;set;}

        public int DepartmentID {get; set;} // un curso se asigna a un solo departamento

        public Department? Department {get; set;} // un curso se asigna a un solo departamento

        public ICollection<Enrollment>? Enrollment {get;set;} // un curso puede tener varios alumnos inscritos

        public ICollection<CourseAssignment>? CourseAssignment {get;set;} // el curso puede ser inpartido por varios instructuros
    }
}