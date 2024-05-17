using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace UVCRMS.Models
{
    public class Designation
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please Enter Designation Name "), StringLength(50)]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Designation Name cannot be a number")]
        [Remote("IsDesignationNameExist", "Designation", ErrorMessage = "Name already exist")]
        public string DesignationName { get; set; }



        public List<Teacher>? Teachers { get; set; }
    }
}
