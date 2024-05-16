using System.ComponentModel.DataAnnotations;

namespace UVCRMS.Models
{
    public class CourseAssignToTeacher
    {
        public int Id { get; set; }

        [Required, Display(Name = "Department")]
        public int DepartmentId { get; set; }

        [Required(ErrorMessage = "Please Give the Teacher Name"), Display(Name = "Teacher")]
        public int TeacherId { get; set; }

        [Required(ErrorMessage = "Please Give the Course Code"), Display(Name = "Course")]
        public int CourseId { get; set; }

        [StringLength(50)]
        public string? Status { get; set; }


        public Department? Department { get; set; }
        public Teacher? Teacher { get; set; }
        public Course? Course { get; set; }

    }
}
