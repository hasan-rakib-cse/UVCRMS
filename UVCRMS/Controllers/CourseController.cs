using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UVCRMS.Data;
using UVCRMS.Models;

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
            ViewBag.Departments = new SelectList(db.Departments.ToList(), "Id", "DepartmentName");
            ViewBag.Semesters = new SelectList(db.Semesters.ToList(), "Id", "SemesterName");
            return View();
        }


        [HttpPost]
        public IActionResult Create(Course course)
        {
            if (ModelState.IsValid)
            {
                db.Entry(course).State = EntityState.Added;
                db.SaveChanges();
                return RedirectToAction("Create", "Course").WithNotice("Successfully Course Saved");
            }
            ModelState.Clear();
            return RedirectToAction("Create", "Course").WithError("Not Saved");
        }


        public JsonResult IsCourseCodeExist(string courseCode)
        {
            var course = db.Courses.ToList();
            if (!course.Any(x => x.CourseCode.ToLower() == courseCode.ToLower()))
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }


        public JsonResult IsCourseNameExist(string coursName)
        {
            var course = db.Courses.ToList();
            if (!course.Any(x => x.CourseName.ToLower() == coursName.ToLower()))
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }


        public IActionResult Edit(int id)
        {
            ViewBag.Departments = new SelectList(db.Departments.ToList(), "Id", "DepartmentName");
            ViewBag.Semesters = new SelectList(db.Semesters.ToList(), "Id", "SemesterName");
            return View(db.Courses.Where(x => x.Id == id).FirstOrDefault());
        }


        [HttpPost]
        public IActionResult Edit(Course course)
        {
            db.Entry(course).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index", "Course");
        }


        public IActionResult Details(int id)
        {
            return View(db.Courses.Where(x => x.Id == id).FirstOrDefault());
        }


        public IActionResult Delete(int id)
        {
            return View(db.Courses.Where(x => x.Id == id).FirstOrDefault());
        }


        [HttpPost]
        public IActionResult Delete(int id, FormCollection collection)
        {
            Course course = db.Courses.Where(x => x.Id == id).FirstOrDefault();
            db.Entry(course).State = EntityState.Deleted;
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
