using System.ComponentModel.DataAnnotations;

namespace UVCRMS.Models
{
    public class Grade
    {
        public int Id { get; set; }

        [StringLength(15)]
        public string GradeLetter { get; set; }
    }
}
