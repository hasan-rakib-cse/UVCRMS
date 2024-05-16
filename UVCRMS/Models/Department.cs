using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace UVCRMS.Models
{
    public class Department
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Department Code Must Be Fill Up")]
        [StringLength(7, MinimumLength = 2, ErrorMessage = "Code must be between 2 to 7 character")]
        [Remote("IsCodeExist", "Department", ErrorMessage = "Code already exist")]
        public string DepartmentCode { get; set; }

        [Required(ErrorMessage = "Please Enter Department Name "), StringLength(50)]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Department Name cannot be a number")]
        [Remote("IsNameExist", "Department", ErrorMessage = "Name already exist")]
        public string DepartmentName { get; set; }
    }
}
