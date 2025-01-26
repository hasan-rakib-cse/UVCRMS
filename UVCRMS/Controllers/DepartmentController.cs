using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UVCRMS.Data;
using UVCRMS.Models;

namespace UVCRMS.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly ApplicationDbContext db;
        public DepartmentController(ApplicationDbContext dbContext)
        {
            db = dbContext;
        }

        public IActionResult Index()
        {
            return View(db.Departments.ToList());
        }

        public IActionResult Details(int id)
        {
            return View(db.Departments.Where(x => x.Id == id).FirstOrDefault());
        }

        public IActionResult Edit(int id)
        {
            return View(db.Departments.Where(x => x.Id == id).FirstOrDefault());
        }

        [HttpPost]
        public IActionResult Edit(Department dept)
        {
            try
            {
                db.Departments.Add(dept);
                db.SaveChanges();
                TempData["update_success_msg"] = "Data Updated Successfully.";
                return RedirectToAction("Index", "Department");
            }
            catch
            {
                return View();
            }
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || db.Departments == null)
            {
                return NotFound();
            }

            var department = db.Departments.FirstOrDefault(x => x.Id == id);
            if (department == null)
            {
                return NotFound();
            }
            return View(department);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int? id)
        {
            var dept = db.Departments.FirstOrDefault(x => x.Id == id);
            if (dept == null)
            {
                return NotFound();
            }

            db.Departments.Remove(dept); // Efficient way to delete
            db.SaveChanges();

            TempData["delete_success_msg"] = "Data Deleted Successfully.";
            return RedirectToAction("Index", "Department");
        }


        public JsonResult IsCodeExist(string DepartmentCode)
        {
            var dept = db.Departments.ToList();
            if (!dept.Any(x => x.DepartmentCode.ToLower() == DepartmentCode.ToLower()))
            {
                return Json(true);
            }
            return Json(false);
        }

        public JsonResult IsNameExist(string DepartmentName)
        {
            var dept = db.Departments.ToList();
            if (!dept.Any(x => x.DepartmentName.ToLower() == DepartmentName.ToLower()))
            {
                return Json(true);
            }
            return Json(false);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Department dept)
        {
            db.Departments.Add(dept);
            db.SaveChanges();
            TempData["SuccessMessage"] = "Successfully saved the department.";
            return RedirectToAction("Create", "Department");
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
