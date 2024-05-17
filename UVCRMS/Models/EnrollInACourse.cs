using System.ComponentModel.DataAnnotations;

namespace UVCRMS.Models
{
    public class EnrollInACourse
    {
        public int Id { get; set; }

        [Required, Display(Name = "Select Student")]
        public int StudentId { get; set; }

        [Required, Display(Name = "Select Course")]
        public int CourseId { get; set; }

        [StringLength(50)]
        public string? Status { get; set; }

        public string? Date { get; set; }


        public Student? Student { get; set; }
        public Course? Course { get; set; }
    }
}
