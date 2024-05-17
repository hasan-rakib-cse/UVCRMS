using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace UVCRMS.Models
{
    public class Course
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please give the Course Code")]
        [Remote("IsCourseCodeExist", "Course", ErrorMessage = "Course Code already exist")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Code must be at least five (5) characters long.")]
        public string CourseCode { get; set; }

        [Required(ErrorMessage = "Please give the Course name")]
        [StringLength(50)]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Name cannot be a number")]
        [Remote("IsCourseNameExist", "Course", ErrorMessage = "Name  already exist")]
        public string CourseName { get; set; }

        [Required(ErrorMessage = "Please give the Course Credit")]
        [Range(0.5, 5, ErrorMessage = "Course Credit must be between 0.5 and 5.0")]
        public double CourseCredit { get; set; }

        [Required(ErrorMessage = "Please give the Course Description")]
        [DataType(DataType.MultilineText)]
        [StringLength(300)]
        public string CourseDescription { get; set; }

        [Required(ErrorMessage = "Please Select Department Name"), Display(Name = "Select Department")]
        public int DepartmentId { get; set; }

        [Required(ErrorMessage = "Please Select Semester Name"), Display(Name = "Select Semester")]
        public int SemesterId { get; set; }


        public Department? Department { get; set; }
        public Semester? Semester { get; set; }



        public List<ClassRoomAllocation>? ClassRoomAllocations { get; set; }
        public List<CourseAssignToTeacher>? CourseAssignToTeachers { get; set; }
        public List<EnrollInACourse>? EnrollInACourses { get; set; }
        public List<SaveStudentResult>? SaveStudentResults { get; set; }
        public List<Teacher>? Teachers { get; set; }
    }
}
