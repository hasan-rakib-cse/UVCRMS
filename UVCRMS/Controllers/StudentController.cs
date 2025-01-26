using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UVCRMS.Data;
using UVCRMS.Models;

namespace UVCRMS.Controllers
{
    public class StudentController : Controller
    {
        private readonly ApplicationDbContext db;
        public StudentController(ApplicationDbContext dbContext)
        {
            db = dbContext;
        }

        public IActionResult Index()
        {
            return View(db.Students.ToList());
        }

        public IActionResult CreateStudentRegistration()
        {
            ViewBag.Departments = new SelectList(db.Departments.ToList(), "Id", "DepartmentCode");
            return View();
        }

        //[HttpPost]

        //public IActionResult CreateStudentRegistration([Bind(Include = "Id,StudentName,StudentEmail,StudentContactNo,Date,StudentAddress,StudentRegNo,DepartmentId")] Student student)
        //{
        //    var course = db.Departments.FirstOrDefault(x => x.Id == student.DepartmentId);
        //    string tempDeptCode = course.DepartmentCode;

        //    string tempDate = student.Date.Substring(6, 4);
        //    var lastRecord = (from s in db.Students orderby s.Id descending select s).First();

        //    if (lastRecord != null)
        //    {
        //        string lastRecordString = Reverse(lastRecord.StudentRegNo.ToString());
        //        string lastNumberOfStudentRegNo = lastRecordString.Substring(0, 3);
        //        string tempLastNumberOfStudentRegNo = Reverse(lastNumberOfStudentRegNo);
        //        int count = 0;
        //        for (int i = 0; i < tempLastNumberOfStudentRegNo.Length; i++)
        //        {
        //            count++;
        //            if (tempLastNumberOfStudentRegNo[i] != '0')
        //            {
        //                count--;
        //                break;
        //            }
        //        }
        //        string NumOfStudentRegNo;
        //        if (count == 2)
        //        {
        //            string tempRegNo = tempLastNumberOfStudentRegNo.Substring(2, 1);
        //            int num = Convert.ToInt32(tempRegNo) + 1;
        //            NumOfStudentRegNo = "00" + num.ToString();
        //        }
        //        else if (count == 1)
        //        {
        //            string tempRegNo = tempLastNumberOfStudentRegNo.Substring(1, 2);
        //            int num = Convert.ToInt32(tempRegNo) + 1;
        //            NumOfStudentRegNo = "0" + num.ToString();
        //        }
        //        else
        //        {
        //            string tempRegNo = tempLastNumberOfStudentRegNo.Substring(0, 3);
        //            int num = Convert.ToInt32(tempRegNo) + 1;
        //            NumOfStudentRegNo = num.ToString();
        //        }

        //        student.StudentRegNo = tempDeptCode + "-" + tempDate + "-" + NumOfStudentRegNo;
        //    }
        //    else
        //    {
        //        student.StudentRegNo = tempDeptCode + "-" + tempDate + "-" + "001";
        //    }

        //    var regNo = db.Students.ToList();
        //    if (!regNo.Any(x => x.StudentEmail.ToLower() == student.StudentEmail.ToLower()))
        //    {
        //        db.Entry(student).State = EntityState.Added;
        //        db.SaveChanges();

        //        return RedirectToAction("CreateStudentRegistration", "Student").WithNotice("Reg No: " + student.StudentRegNo + " Name: " + student.StudentName + " Email: " + student.StudentEmail + " Contact: " + student.StudentContactNo + " Saved Successfully");
        //    }
        //    ModelState.Clear();

        //    return RedirectToAction("CreateStudentRegistration", "Student").WithNotice("Not Create Reg No Because Already Exist");
        //}


        //public static string Reverse(string s)
        //{
        //    char[] charArray = s.ToCharArray();
        //    Array.Reverse(charArray);
        //    return new string(charArray);
        //}

        [HttpPost]
        public async Task<IActionResult> CreateStudentRegistration([FromBody] Student student)
        {
            if (student == null)
            {
                return BadRequest("Invalid student data.");
            }

            // Fetch the department by ID
            var course = await db.Departments.FirstOrDefaultAsync(x => x.Id == student.DepartmentId);
            //if (course == null)
            //{
            //    return NotFound("Department not found.");
            //}

            string tempDeptCode = course.DepartmentCode;
            string tempDate = student.Date.Substring(6, 4); // Assuming Date is a string, format validation required

            // Fetch the last student record
            var lastRecord = await db.Students.OrderByDescending(s => s.Id).FirstOrDefaultAsync();

            string newStudentRegNo;
            if (lastRecord is not null)
            {
                string lastRegNo = Reverse(lastRecord.StudentRegNo);
                string lastNumberSegment = Reverse(lastRegNo.Substring(0, 3));

                int lastNumber = int.Parse(lastNumberSegment.TrimStart('0'));
                string nextNumberSegment = (lastNumber + 1).ToString("D3"); // Always format with 3 digits

                newStudentRegNo = $"{tempDeptCode}-{tempDate}-{nextNumberSegment}";
            }
            else
            {
                // First registration
                newStudentRegNo = $"{tempDeptCode}-{tempDate}-001";
            }

            student.StudentRegNo = newStudentRegNo;

            // Check for duplicate email
            bool emailExists = await db.Students.AnyAsync(x => x.StudentEmail.ToLower() == student.StudentEmail.ToLower());
            if (emailExists)
            {
                return Conflict("A student with the same email already exists.");
            }

            // Save the student
            db.Students.Add(student);
            await db.SaveChangesAsync();

            return CreatedAtAction(nameof(CreateStudentRegistration), new
            {
                RegNo = student.StudentRegNo,
                Name = student.StudentName,
                Email = student.StudentEmail,
                Contact = student.StudentContactNo
            }, "Student registration created successfully.");
        }

        //private string Reverse(string input)
        //{
        //    return new string(input.Reverse().ToArray());
        //}

        public static string Reverse(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }


        public JsonResult IsEmailExist(string studentEmail)
        {
            var email = db.Students.ToList();
            if (!email.Any(x => x.StudentEmail.ToLower() == studentEmail.ToLower()))
            {
                return Json(true);
            }

            return Json(false);
        }

        //public async Task<IActionResult> IsEmailExist(string studentEmail)
        //{
        //    if (string.IsNullOrEmpty(studentEmail))
        //    {
        //        return BadRequest("Email cannot be null or empty.");
        //    }

        //    // Check if the email exists in the database (case-insensitive)
        //    bool emailExists = await db.Students.AnyAsync(x => x.StudentEmail.ToLower() == studentEmail.ToLower());

        //    // Return JSON response
        //    return Json(new { exists = emailExists });
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}
