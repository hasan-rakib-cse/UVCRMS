using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UVCRMS.Data;
using UVCRMS.Models;

namespace UVCRMS.Controllers
{
    public class TeacherController : Controller
    {
        private readonly ApplicationDbContext db;
        public TeacherController(ApplicationDbContext dbContext)
        {
            db = dbContext;
        }

        public IActionResult Index()
        {
            return View(db.Teachers.ToList());
        }

        public IActionResult Create()
        {
            ViewBag.Departments = new SelectList(db.Departments.ToList(), "Id", "DepartmentName");
            ViewBag.Designations = new SelectList(db.Designations.ToList(), "Id", "DesignationName");
            return View();
        }

        [HttpPost]
        public IActionResult Create(Teacher teacher)
        {
            var course = db.Courses.FirstOrDefault(x => x.DepartmentId == teacher.DepartmentId);

            if (ModelState.IsValid)
            {
                teacher.TeacherRemainingCredit = teacher.CreditToBeTaken;
                db.Teachers.Add(teacher);
                db.SaveChanges();
                TempData["SuccessMessage"] = "Successfully Teacher Saved";
                return RedirectToAction("Create", "Teacher");
            }

            TempData["ErrorMessage"] = "Not Saved";
            return View();
        }

        [HttpGet]
        public JsonResult IsEmailExist(string TeacherEmail)
        {
            var email = db.Teachers.ToList();
            if (!email.Any(x => x.TeacherEmail.ToLower() == TeacherEmail.ToLower()))
            {
                return Json(true);
            }
            return Json(false);
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
