namespace University.Models {
    public enum Grade {A, B, C, D, E, F}

    public class Enrollment{
        public int Id {get;set;}
        public required int CourseID {get;set;}
        public required int StudentID {get;set;}
        public Grade? Grade {get;set;}
        
        public Course? Course {get;set;}
        public Student? Student {get;set;}
    }
}