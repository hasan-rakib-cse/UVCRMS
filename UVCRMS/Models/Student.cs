using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace UVCRMS.Models
{
    public class Student
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please Give the  Name"), StringLength(100)]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Name cannot be a number")]
        public string StudentName { get; set; }

        [Required(ErrorMessage = "Please Give the Email"), StringLength(100)]
        [DataType(DataType.EmailAddress)]
        [Remote("IsEmailExist", "Student", ErrorMessage = "Email already exist")]
        public string StudentEmail { get; set; }

        [Required(ErrorMessage = "Please Give the Contact Number"), StringLength(20)]
        public string StudentContactNo { get; set; }

        [DataType(DataType.Date), Display(Name = "Please Select  Date")]
        public string? Date { get; set; }

        [DataType(DataType.MultilineText), StringLength(300)]
        public string? StudentAddress { get; set; }

        [Required, Display(Name = "Student Registration No"), StringLength(20)]
        public string StudentRegNo { get; set; }

        [Required(ErrorMessage = "Please Select A Department"), Display(Name = "Select Department")]
        public int DepartmentId { get; set; }



        public Department? Department { get; set; }


        public List<EnrollInACourse>? EnrollInACourses { get; set; }
        public List<SaveStudentResult>? SaveStudentResults { get; set; }
    }
}
