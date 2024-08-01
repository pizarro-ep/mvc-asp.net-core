using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace University.Models {
    public class Course {
        [DatabaseGenerated(DatabaseGeneratedOption.None)] // Permite especificar la clave principal
        public int CourseID {get;set;}
        public required string Title {get;set;}
        public required int Credits {get;set;}

        public ICollection<Enrollment>? Enrollment {get;set;}
    }
}