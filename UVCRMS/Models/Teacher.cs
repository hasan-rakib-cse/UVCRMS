using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace UVCRMS.Models
{
    public class Teacher
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please Give the Name"), StringLength(100)]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Name cannot be a number")]
        public string TeacherName { get; set; }

        [StringLength(300)]
        public string? TeacherAddress { get; set; }

        [Required(ErrorMessage = "Please Give the  Email"), DataType(DataType.EmailAddress), StringLength(100)]
        [Remote("IsEmailExist", "Teacher", ErrorMessage = "Email already exist")]
        public string TeacherEmail { get; set; }

        [Required(ErrorMessage = "Please Give the Contact No"), StringLength(20)]
        [RegularExpression(@"^\(?[+. ]?([0-9]{2})\)?[-. ]?([0-9]{11})$", ErrorMessage = "Invalid Phone number(e.+8801XXXXXXXXX)")]
        public string TeacherContactNo { get; set; }

        [Required(ErrorMessage = "Please Select  the  Designation"), Display(Name = "Select Designation")]
        public int DesignationId { get; set; }

        [Required(ErrorMessage = "Please Select  the  Department"), Display(Name = "Select Department")]
        public int DepartmentId { get; set; }

        [Required(ErrorMessage = "Please Select  the  Course"), Display(Name = "Select Course")]
        public int CourseId { get; set; }

        [Required(ErrorMessage = "Please Give the Credit")]
        [Range(+.5, Double.MaxValue, ErrorMessage = "Credit Can not be negetive")]
        public double CreditToBeTaken { get; set; }

        public double? TeacherRemainingCredit { get; set; }



        public Department? Department { get; set; }
        public Designation? Designation { get; set; }
    }
}
