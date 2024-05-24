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


        //public JsonResult IsEmailExist(string studentEmail)
        //{
        //    var email = db.Students.ToList();
        //    if (!email.Any(x => x.StudentEmail.ToLower() == studentEmail.ToLower()))
        //    {
        //        return Json(true, JsonRequestBehavior.AllowGet);
        //    }

        //    return Json(false, JsonRequestBehavior.AllowGet);
        //}


        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}

    }
}
