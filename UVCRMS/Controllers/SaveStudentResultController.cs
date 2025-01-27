using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UVCRMS.Data;
using UVCRMS.Models;

namespace UVCRMS.Controllers
{
    public class SaveStudentResultController : Controller
    {
        private readonly ApplicationDbContext db;

        // Updated Version
        public SaveStudentResultController(ApplicationDbContext dbContext) => db = dbContext;
        //public SaveStudentResultController(ApplicationDbContext dbContext)
        //{
        //    db = dbContext;
        //}

        public IActionResult Index()
        {
            return View(db.SaveStudentResults.ToList());
        }

        public IActionResult ViewResult()
        {
            ViewBag.Students = new SelectList(db.Students.ToList(), "Id", "StudentRegNo");
            ViewBag.Courses = new SelectList(db.Courses.ToList(), "Id", "CoursName");
            ViewBag.Departments = new SelectList(db.Departments.ToList(), "Id", "DepartmentCode");
            ViewBag.Grades = new SelectList(db.Grades.ToList(), "Id", "GradeLetter");

            return View();
        }

        public IActionResult CreateEnrollCourse()
        {
            ViewBag.Students = new SelectList(db.Students.ToList(), "Id", "StudentRegNo");
            ViewBag.Courses = new SelectList(db.Courses.ToList(), "Id", "CoursName");
            ViewBag.Departments = new SelectList(db.Departments.ToList(), "Id", "DepartmentCode");
            ViewBag.Grades = new SelectList(db.Grades.ToList(), "Id", "GradeLetter");

            return View();
        }

        [HttpPost]
        public IActionResult CreateEnrollCourse(SaveStudentResult saveStudentResult)
        {
            saveStudentResult.Status = "Enroll";

            db.SaveStudentResults.Add(saveStudentResult);
            db.SaveChanges();

            TempData["student_result_successfully"] = "Student Result Successfully";
            return RedirectToAction("CreateEnrollCourse", "EnrollCourse");
        }

        //public IActionResult PrintAll()
        //{
        //    var q = new ActionAsPdf("ViewResult");
        //    return q;
        //}

        public JsonResult GetStudentNameEmailDeptByRegNo(int studentId)
        {
            var students = db.SaveStudentResults.Where(x => x.StudentId == studentId).ToList();

            return Json(students);
        }

        public JsonResult GetStudentNameEmailDeptByStId(int studentId)
        {
            var student = db.Students.FirstOrDefault(x => x.Id == studentId);

            return Json(student);
        }

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
