using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace UVCRMS.Models
{
    public class Semester
    {
        public int Id { get; set; }

        [Required, StringLength(50)]
        public string SemesterName { get; set; }
    }
}
