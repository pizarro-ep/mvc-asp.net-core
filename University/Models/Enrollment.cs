using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace University.Models {
    public enum Grade {A, B, C, D, E, F}

    public class Enrollment{
        public int Id {get;set;}
        
        public required int CourseID {get;set;} // la inscripcion es para solo un curso
        
        public required int StudentID {get;set;} // // la inscripcion es para solo un estudiante

        [DisplayFormat(NullDisplayText = "N° Grado")]
        public Grade? Grade {get;set;}
        
        public Course? Course {get;set;} // la inscripcion es para solo un estudiante
        
        public Student? Student {get;set;} // // la inscripcion es para solo un estudiante
    }
}