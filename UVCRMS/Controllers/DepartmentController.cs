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

        public IActionResult PrintAll()
        {
            var q = new ActionAsPdf("Index");
            return q;
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
                db.Entry(dept).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index", "Department");
            }
            catch
            {
                return View();
            }
        }

        public IActionResult Delete(int id)
        {
            return View(db.Departments.Where(x => x.Id == id).FirstOrDefault());
        }


        [HttpPost]
        public IActionResult Delete(int id, FormCollection collection)
        {
            Department dept = db.Departments.Where(x => x.Id == id).FirstOrDefault();
            db.Entry(dept).State = EntityState.Deleted;
            db.SaveChanges();
            return RedirectToAction("Index", "Department");
        }


        public JsonResult IsCodeExist(string DepartmentCode)
        {
            var dept = db.Departments.ToList();
            if (!dept.Any(x => x.DepartmentCode.ToLower() == DepartmentCode.ToLower()))
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }


        public JsonResult IsNameExist(string DepartmentName)
        {
            var dept = db.Departments.ToList();
            if (!dept.Any(x => x.DepartmentName.ToLower() == DepartmentName.ToLower()))
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }


        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Create(Department dept)
        {
            db.Entry(dept).State = EntityState.Added;
            db.SaveChanges();

            return RedirectToAction("Create", "Department").WithNotice("Succesfully Department Saved");
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
