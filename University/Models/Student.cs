using System;
using System.Collections.Generic;

namespace University.Models {
    public class Student{
        public int Id {get;set;}
        public required string LastName {get;set;}
        public required string Surname {get;set;}
        public DateTime? EnrollmentDate {get;set;}
        public ICollection<Enrollment>? Enrollments {get;set;}
    }
}