using System.ComponentModel.DataAnnotations;

namespace UVCRMS.Models
{
    public class Designation
    {
        public int Id { get; set; }

        [Required, StringLength(50)]
        public string DesignationName { get; set; }
    }
}
