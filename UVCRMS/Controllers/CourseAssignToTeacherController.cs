using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UVCRMS.Data;
using UVCRMS.Gateway;
using UVCRMS.Models;

namespace UVCRMS.Controllers
{
    public class CourseAssignToTeacherController : Controller
    {
        private readonly ApplicationDbContext db;
        public CourseAssignToTeacherController(ApplicationDbContext dbContext)
        {
            db = dbContext;
        }


        public IActionResult Index(CourseAssignToTeacher courseAssign)
        {

            return View(db.CourseAssignToTeachers.ToList());
        }


        public IActionResult CourseAssignToTeacher()
        {
            ViewBag.Departments = new SelectList(db.Departments, "Id", "DepartmentCode");
            return View();
        }


        [HttpPost]
        public IActionResult CourseAssignToTeacher(CourseAssignToTeacher courseAssign)
        {
            var course = db.Courses.FirstOrDefault(x => x.Id == courseAssign.CourseId);
            var teacher = db.Teachers.FirstOrDefault(x => x.Id == courseAssign.TeacherId);

            if (IsCourseCodeExist(course.CourseCode.ToString()) && IsTeacherNameExist(teacher.TeacherName.ToString()))
            {
                teacher.TeacherRemainingCredit = teacher.TeacherRemainingCredit - course.CourseCredit;
                db.Entry(teacher).State = EntityState.Modified;
                db.SaveChanges();

                courseAssign.Status = "Assigned";

                db.Entry(courseAssign).State = EntityState.Added;
                db.SaveChanges();

                return RedirectToAction("CourseAssignToTeacher", "CourseAssignToTeacher").WithNotice("Course Assign To Teacher Successfully Saved");
            }
            return RedirectToAction("CourseAssignToTeacher", "CourseAssignToTeacher").WithNotice("Not Saved Bcoz Teacher Name Or Course Code Already Exist");
        }


        public bool IsCourseCodeExist(string courseCode)
        {
            var course = db.CourseAssignToTeachers.ToList();
            if (!course.Any(x => x.Course.CourseCode.ToLower() == courseCode.ToLower()))
            {
                return true;
            }
            return false;
        }


        public bool IsTeacherNameExist(string teacherName)
        {
            var teacher = db.CourseAssignToTeachers.ToList();
            if (!teacher.Any(x => x.Teacher.TeacherName.ToLower() == teacherName.ToLower()))
            {
                return true;
            }
            return false;
        }


        public JsonResult GetAllAssignCourses(int deptId)
        {
            var courseStatics = new CourseAssignTeacherGateway().GetAllAssignCourses(deptId);
            return Json(courseStatics, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetTeacherByDeptId(int departmentId)
        {
            var teachers = db.Teachers.Where(x => x.DepartmentId == departmentId).ToList();
            return Json(teachers);
        }

        public JsonResult GetCourseByDeptId(int departmentId)
        {
            var courses = db.Courses.Where(x => x.DepartmentId == departmentId).ToList();
            return Json(courses);
        }


        public JsonResult GetCreditToBeTakenById(int teacherId)
        {
            var teacher = db.Teachers.FirstOrDefault(x => x.Id == teacherId);
            return Json(teacher);
        }


        public JsonResult GetCourseCodeById(int courseId)
        {
            var course = db.Courses.FirstOrDefault(x => x.Id == courseId);
            return Json(course);
        }


        public IActionResult CreateCourseStaticsByDeptId()
        {
            ViewBag.DepartmentId = new SelectList(db.Departments.ToList(), "Id", "DepartmentCode");
            return View();
        }


        public IActionResult UnassignCourse()
        {
            ViewData["Message"] = "";
            return View();
        }


        [HttpPost]
        public IActionResult UnassignCourse(string unAssignCourse)
        {
            foreach (var item in db.Courses)
            {
                db.Entry(item).State = EntityState.Modified;
            }

            db.SaveChanges();
            ViewData["Message"] = "Successfully UnAssign";

            return View();
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
