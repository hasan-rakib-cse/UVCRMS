using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UVCRMS.Data;
using UVCRMS.Models;

namespace UVCRMS.Controllers
{
    public class EnrollCourseController : Controller
    {
        private readonly ApplicationDbContext db;
        public EnrollCourseController(ApplicationDbContext dbContext)
        {
            db = dbContext;
        }

        public IActionResult Index()
        {
            return View(db.EnrollInACourses.ToList());
        }


        public IActionResult CreateEnrollCourse()
        {
            ViewBag.Students = new SelectList(db.Students.ToList(), "Id", "StudentRegNo");
            ViewBag.Courses = new SelectList(db.Courses.ToList(), "Id", "CoursName");
            ViewBag.Departments = new SelectList(db.Departments.ToList(), "Id", "DepartmentCode");

            return View();
        }


        [HttpPost]
        public IActionResult CreateEnrollCourse(EnrollInACourse enrollInACourse)
        {
            enrollInACourse.Status = "Enroll";

            db.Entry(enrollInACourse).State = EntityState.Added;
            db.SaveChanges();

            return RedirectToAction("CreateEnrollCourse", "EnrollCourse").WithNotice("Enroll Successfully");
        }


        public JsonResult GetStudentNameEmailDeptByRegNo(int studentId)
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
