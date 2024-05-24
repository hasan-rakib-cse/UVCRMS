using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UVCRMS.Data;
using UVCRMS.Gateway;
using UVCRMS.Models;

namespace UVCRMS.Controllers
{
    public class AllocateClassRoomController : Controller
    {
        private readonly ApplicationDbContext db;
        public AllocateClassRoomController(ApplicationDbContext dbContext)
        {
            db = dbContext;
        }


        public IActionResult Index(CourseAssignToTeacher courseAssign)
        {
            return View(db.CourseAssignToTeachers.ToList());
        }

        public IActionResult Create()
        {
            ViewBag.Departments = new SelectList(db.Departments, "Id", "DepartmentCode");

            ViewBag.Courses = new SelectList(db.Courses, "Id", "CourseCode");
            ViewBag.Rooms = new SelectList(db.Rooms, "Id", "RoomNo");
            ViewBag.SevenDayWeeks = new SelectList(db.SevenDayWeeks, "Id", "DayCode");

            return View();
        }


        [HttpPost]
        public IActionResult Create(ClassRoomAllocation classRoomAllocation)
        {
            classRoomAllocation.Status = "Allocate";
            classRoomAllocation.TimeFrom = classRoomAllocation.TimeFrom.Remove(2, 1);
            classRoomAllocation.TimeTo = classRoomAllocation.TimeTo.Remove(2, 1);

            var course = db.Courses.FirstOrDefault(x => x.Id == classRoomAllocation.CourseId);

            if (Convert.ToInt32(classRoomAllocation.TimeFrom) <= Convert.ToInt32(classRoomAllocation.TimeTo))
            {
                if (IsDayExist(classRoomAllocation.CourseId, classRoomAllocation.SevenDayWeekId))
                {
                    db.Entry(classRoomAllocation).State = EntityState.Added;
                    db.SaveChanges();

                    return RedirectToAction("Create", "AllocateClassRoom").WithNotice("Class Allocated Successfully");

                }
                else
                {
                    return RedirectToAction("Create", "AllocateClassRoom").WithError("Class Not Allocated Becouse Day Already Assigned to particular course.");
                }

            }

            ModelState.Clear();
            return RedirectToAction("Create", "AllocateClassRoom").WithError("Time Formate Not Right");

        }
        

        public bool IsDayExist(int courseId, int dayId)
        {
            var classAllocation = db.ClassRoomAllocations.ToList();
            if (!classAllocation.Any(x => x.SevenDayWeekId == dayId && x.CourseId == courseId))
            {
                return true;
            }

            return false;
        }


        public IActionResult ViewClassScheduleAndRoomAllocation()
        {
            ViewBag.Departments = new SelectList(db.Departments, "Id", "DepartmentCode");
            return View();
        }


        public JsonResult GetClassScheduleAndRoomAllocationByDeptId(int deptId)
        {
            var getAllClassScheduleViews = GetAllClassScheduleViews(deptId);
            return Json(getAllClassScheduleViews);
        }


        public JsonResult GetCourseByDeptId(int deptId)
        {
            var courses = db.Courses.Where(x => x.DepartmentId == deptId).ToList();
            return Json(courses);
        }


        public List<ClassScheduleView> GetAllClassScheduleViews(int departmentId)
        {
            ViewClassSchedule gateway = new ViewClassSchedule();
            List<ClassScheduleView> list = gateway.GetAllClassScheduleViewsByDeptId(departmentId);
            List<ClassScheduleView> myList = new List<ClassScheduleView>();

            for (var i = 0; i < list.Count;)
            {
                ClassScheduleView classView = list[i];
                ClassScheduleView temp = new ClassScheduleView();
                temp.CourseCode = classView.CourseCode;
                temp.CourseName = classView.CourseName;
                temp.ScheduleInfo = ("R. No : " + classView.RoomName + ", " + classView.DayShortName + ", " + classView.TimeFrom + " - " + classView.TimeTo) + "</br>";
                int ck = 0;
                i++;

                for (var j = i; j < list.Count; j++)
                {
                    ck = 1;
                    ClassScheduleView classView2 = list[j - 1];
                    ClassScheduleView classView3 = list[j];

                    if (classView2.CourseCode == classView3.CourseCode)
                    {
                        i++;
                        temp.ScheduleInfo += ("R. No : " + classView3.RoomName + ", " + classView3.DayShortName + ", " +
                                                         classView3.TimeFrom + " - " + classView3.TimeTo + "</br>");

                        //myList.Add(temp);
                    }
                    else
                    {
                        if (classView.RoomName == "")
                        {
                            temp.ScheduleInfo = "Not Scheduled Yet";
                        }
                        myList.Add(temp);
                        break;
                    }
                }
                if (ck == 0)
                {
                    if (classView.RoomName == "")
                    {
                        temp.ScheduleInfo = "Not Scheduled Yet";
                    }
                    myList.Add(temp);
                }
            }
            return myList;
        }


        [HttpGet]
        public IActionResult UnallocatedClassRooms()
        {
            ViewData["Message"] = "";
            return View();
        }


        [HttpPost]
        public IActionResult UnallocatedClassRooms(string unallocatedClassRoom)
        {
            ViewClassSchedule gateway = new ViewClassSchedule();
            if (gateway.UnAllocateClassRoom())
            {
                ViewData["Message"] = "UnAllocate Successfully";
            }
            else
            {
                ViewData["Message"] = "UnAllocate Failed";
            }
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
