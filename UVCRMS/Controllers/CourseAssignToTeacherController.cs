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
        private readonly CourseAssignTeacherGateway courseStatics;
        public CourseAssignToTeacherController(ApplicationDbContext dbContext, CourseAssignTeacherGateway courseAssignTeacherGateway)
        {
            db = dbContext;
            courseStatics = courseAssignTeacherGateway;
        }


        public IActionResult Index(CourseAssignToTeacher courseAssign)
        {

            return View(db.CourseAssignToTeachers.ToList());
        }

        // This code will work also.
        //public IActionResult CourseAssignToTeacher()
        //{
        //    ViewBag.Departments = new SelectList(db.Departments, "Id", "DepartmentCode");
        //    return View();
        //}

        public IActionResult CourseAssignToTeacher()
        {
            var departments = db.Departments.Select(d => new
            {
                d.Id,
                d.DepartmentCode
            }).ToList();
            ViewBag.Departments = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(departments, "Id", "DepartmentCode");
            return View();
        }


        //[HttpPost]
        //public IActionResult CourseAssignToTeacher(CourseAssignToTeacher courseAssign)
        //{
        //    var course = db.Courses.FirstOrDefault(x => x.Id == courseAssign.CourseId);
        //    var teacher = db.Teachers.FirstOrDefault(x => x.Id == courseAssign.TeacherId);

        //    if (IsCourseCodeExist(course.CourseCode.ToString()) && IsTeacherNameExist(teacher.TeacherName.ToString()))
        //    {
        //        teacher.TeacherRemainingCredit = teacher.TeacherRemainingCredit - course.CourseCredit;
        //        db.Entry(teacher).State = EntityState.Modified;
        //        db.SaveChanges();

        //        courseAssign.Status = "Assigned";

        //        db.Entry(courseAssign).State = EntityState.Added;
        //        db.SaveChanges();

        //        return RedirectToAction("CourseAssignToTeacher", "CourseAssignToTeacher").WithNotice("Course Assign To Teacher Successfully Saved");
        //    }
        //    return RedirectToAction("CourseAssignToTeacher", "CourseAssignToTeacher").WithNotice("Not Saved Bcoz Teacher Name Or Course Code Already Exist");
        //}

        [HttpPost]
        public IActionResult CourseAssignToTeacher(CourseAssignToTeacher courseAssign)
        {
            // Check if the input is valid
            if (courseAssign == null || courseAssign.CourseId <= 0 || courseAssign.TeacherId <= 0)
            {
                TempData["invalid_input"] = "Invalid input. Please provide valid course and teacher details.";
                return RedirectToAction("CourseAssignToTeacher");
            }

            var course = db.Courses.FirstOrDefault(x => x.Id == courseAssign.CourseId);
            var teacher = db.Teachers.FirstOrDefault(x => x.Id == courseAssign.TeacherId);

            // Ensure course and teacher exist
            if (course == null)
            {
                TempData["course_not_found"] = "Course not found.";
                return RedirectToAction("CourseAssignToTeacher");
            }

            if (teacher == null)
            {
                TempData["teacher_not_found"] = "Teacher not found.";
                return RedirectToAction("CourseAssignToTeacher");
            }

            // Check if the course code and teacher name exist in the system
            if (IsCourseCodeExist(course.CourseCode) && IsTeacherNameExist(teacher.TeacherName))
            {
                if (teacher.TeacherRemainingCredit < course.CourseCredit)
                {
                    TempData["not_enough_credit"] = "Teacher does not have enough remaining credits to assign this course.";
                    return RedirectToAction("CourseAssignToTeacher");
                }

                // Update the remaining credit of the teacher
                teacher.TeacherRemainingCredit = teacher.TeacherRemainingCredit - course.CourseCredit;
                //db.Entry(teacher).State = EntityState.Modified;
                db.Teachers.Update(teacher);
                db.SaveChanges();

                // Mark the course as assigned to the teacher
                courseAssign.Status = "Assigned";

                //db.Entry(courseAssign).State = EntityState.Added;
                db.CourseAssignToTeachers.Add(courseAssign);
                db.SaveChanges();

                TempData["SuccessMessage"] = "Course assigned to teacher successfully.";
                return RedirectToAction("CourseAssignToTeacher", "CourseAssignToTeacher");
            }

            // If course code or teacher name already exists in the assignment
            TempData["SuccessMessage"] = "Not saved because the teacher name or course code already exists.";
            return RedirectToAction("CourseAssignToTeacher");
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

        [HttpGet]
        public JsonResult GetAllAssignCourses(int deptId)
        {
            var assignedCourses = courseStatics.GetAllAssignCourses(deptId);
            return Json(assignedCourses);
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
