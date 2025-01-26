using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UVCRMS.Data;
using UVCRMS.Models;
using static UVCRMS.Models.SemesterEnum;

namespace UVCRMS.Controllers
{
    public class CourseController : Controller
    {
        private readonly ApplicationDbContext db;
        public CourseController(ApplicationDbContext dbContext)
        {
            db = dbContext;
        }

        public IActionResult Index()
        {
            return View(db.Courses.ToList());
        }

        public IActionResult Create()
        {
            var departments = db.Departments.Select(d => new
            {
                Id = d.Id,
                DepartmentName = d.DepartmentName
            }).ToList();

            var semesters = db.Semesters.Select(s => new
            {
                Id = s.Id,
                SemesterName = s.SemesterName
            }).ToList();

            ViewBag.Departments = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(departments, "Id", "DepartmentName");
            ViewBag.Semesters = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(semesters, "Id", "SemesterName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Course course)
        {
            if (ModelState.IsValid)
            {
                db.Courses.Add(course);
                db.SaveChanges();
                TempData["insert_success_msg"] = "Data Inserted Successfully.";
            }
            return RedirectToAction("Create", "Course");
        }

        [HttpGet]
        public IActionResult IsCourseCodeExist(string courseCode)
        {
            var course = db.Courses.ToList();
            if (!course.Any(x => x.CourseCode.ToLower() == courseCode.ToLower()))
            {
                return Json(true);
            }
            return Json(false);
        }

        [HttpGet]
        public IActionResult IsCourseNameExist(string coursName)
        {
            var course = db.Courses.ToList();
            if (!course.Any(x => x.CourseName.ToLower() == coursName.ToLower()))
            {
                return Json(true);
            }
            return Json(false);
        }

        public IActionResult Edit(int id)
        {
            var departments = db.Departments.Select (d => new 
            {
               Id = d.Id,
               DepartmentName = d.DepartmentName
            }).ToList();

            var semesters = db.Semesters.Select(s => new
            {
                Id = s.Id,
                SemesterName = s.SemesterName
            }).ToList();

            ViewBag.Departments = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(departments, "Id", "DepartmentName");
            ViewBag.Semesters = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(semesters, "Id", "SemesterName");

            var course = db.Courses.FirstOrDefault(x => x.Id == id);
            if(course == null)
            {
                return NotFound();
            }
            return View(course);
        }

        [HttpPost]
        public IActionResult Edit(Course course)
        {
            if(course is not null)
            {
                db.Courses.Add(course);
                db.SaveChanges();
            }
            return RedirectToAction("Index", "Course");
        }

        public IActionResult Details(int id)
        {
            return View(db.Courses.Where(x => x.Id == id).FirstOrDefault());
        }

        public IActionResult Delete(int? id)
        {
            if(id == null || db.Courses == null) 
            { 
                return NotFound();
            }
            var course = db.Courses.FirstOrDefault(c => c.Id == id);
            return View(course);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int? id)
        {
            var course = db.Courses.FirstOrDefault(c => c.Id == id);
            if(course == null )
            {
                return NotFound();
            }
            db.Courses.Remove(course);
            db.SaveChanges();

            return RedirectToAction("Index", "Course");
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
