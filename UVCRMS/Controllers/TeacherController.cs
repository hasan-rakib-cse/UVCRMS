﻿using Microsoft.AspNetCore.Mvc;
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
                db.Entry(teacher).State = EntityState.Added;
                db.SaveChanges();
                return RedirectToAction("Create", "Teacher").WithNotice("Successfully Teacher Saved");
            }

            ModelState.Clear();
            return View().WithFlash("Not Saved");
        }


        public JsonResult IsEmailExist(string TeacherEmail)
        {
            var email = db.Teachers.ToList();
            if (!email.Any(x => x.TeacherEmail.ToLower() == TeacherEmail.ToLower()))
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            return Json(false, JsonRequestBehavior.AllowGet);
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
