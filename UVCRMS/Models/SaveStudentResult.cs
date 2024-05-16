using System.ComponentModel.DataAnnotations;

namespace UVCRMS.Models
{
    public class SaveStudentResult
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please Select Course"), Display(Name = "Student Reg. No.")]
        public int StudentId { get; set;}

        [Required(ErrorMessage = "Please Select Course"), Display(Name = "Select Course")]
        public int CourseId { get; set; }

        [Required(ErrorMessage = "Please Select Grade"), Display(Name = "Select Grade")]
        public int GradeId { get; set; }

        [StringLength(50)]
        public string? Status { get; set; }
        public string? Date { get; set; }


        public Student? Student { get; set; }
        public Course? Course { get; set; }
        public Grade? Grade { get; set;}


    }
}
