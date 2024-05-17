using System.ComponentModel.DataAnnotations;

namespace UVCRMS.Models
{
    public class Grade
    {
        public int Id { get; set; }

        [Required, StringLength(15), Display(Name = "Select Grade")]
        public string GradeLetter { get; set; }



        public List<SaveStudentResult>? SaveStudentResults { get; set; }
    }
}
