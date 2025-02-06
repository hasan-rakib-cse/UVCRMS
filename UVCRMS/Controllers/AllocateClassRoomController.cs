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
        private readonly ViewClassSchedule gateway;
        public AllocateClassRoomController(ApplicationDbContext dbContext, ViewClassSchedule viewClassSchedule)
        {
            db = dbContext;
            gateway = viewClassSchedule;
        }

        public IActionResult Index(CourseAssignToTeacher courseAssign)
        {
            return View(db.CourseAssignToTeachers.ToList());
        }

        public IActionResult Create()
        {
            ViewBag.Departments = db.Departments.Select(d => new SelectListItem
            {
                Value = d.Id.ToString(),
                Text = d.DepartmentCode
            }).ToList();

            ViewBag.Courses = db.Courses.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.CourseCode
            }).ToList();
            ViewBag.Rooms = db.Rooms.Select(r => new SelectListItem
            {
                Value = r.Id.ToString(),
                Text = r.RoomNo
            }).ToList();
            ViewBag.SevenDayWeeks = new SelectList(db.SevenDayWeeks, "Id", "DayCode");
            //ViewBag.Courses = db.Courses.Select(c => new { c.Id, c.CourseCode }).ToList();
            //ViewBag.Rooms = db.Rooms.Select(r => new { r.Id, r.RoomNo }).ToList();
            //ViewBag.SevenDayWeeks = new SelectList(db.SevenDayWeeks, "Id", "DayCode");

            return View();
        }

        //[HttpPost]
        //public IActionResult Create(ClassRoomAllocation classRoomAllocation)
        //{
        //    classRoomAllocation.Status = "Allocate";
        //    classRoomAllocation.TimeFrom = classRoomAllocation.TimeFrom.Remove(2, 1);
        //    classRoomAllocation.TimeTo = classRoomAllocation.TimeTo.Remove(2, 1);

        //    var course = db.Courses.FirstOrDefault(x => x.Id == classRoomAllocation.CourseId);

        //    if (Convert.ToInt32(classRoomAllocation.TimeFrom) <= Convert.ToInt32(classRoomAllocation.TimeTo))
        //    {
        //        if (IsDayExist(classRoomAllocation.CourseId, classRoomAllocation.SevenDayWeekId))
        //        {
        //            db.Entry(classRoomAllocation).State = EntityState.Added;
        //            db.SaveChanges();

        //            return RedirectToAction("Create", "AllocateClassRoom").WithNotice("Class Allocated Successfully");

        //        }
        //        else
        //        {
        //            return RedirectToAction("Create", "AllocateClassRoom").WithError("Class Not Allocated Becouse Day Already Assigned to particular course.");
        //        }

        //    }

        //    ModelState.Clear();
        //    return RedirectToAction("Create", "AllocateClassRoom").WithError("Time Formate Not Right");

        //}

        //[HttpPost]
        //public async Task<IActionResult> Create([FromBody] ClassRoomAllocation classRoomAllocation)
        //{
        //    if (classRoomAllocation == null)
        //    {
        //        return BadRequest("Invalid classroom allocation data.");
        //    }

        //    // Assign default status
        //    classRoomAllocation.Status = "Allocate";

        //    // Format and validate time
        //    if (classRoomAllocation.TimeFrom.Length != 5 || classRoomAllocation.TimeTo.Length != 5)
        //    {
        //        return BadRequest("Invalid time format. Expected HH:mm.");
        //    }

        //    classRoomAllocation.TimeFrom = classRoomAllocation.TimeFrom.Replace(":", "");
        //    classRoomAllocation.TimeTo = classRoomAllocation.TimeTo.Replace(":", "");

        //    if (Convert.ToInt32(classRoomAllocation.TimeFrom) > Convert.ToInt32(classRoomAllocation.TimeTo))
        //    {
        //        return BadRequest("Start time must be earlier than or equal to end time.");
        //    }

        //    // Validate course existence
        //    var course = await db.Courses.FirstOrDefaultAsync(x => x.Id == classRoomAllocation.CourseId);
        //    if (course == null)
        //    {
        //        return NotFound("Course not found.");
        //    }

        //    // Check if the day is already allocated
        //    if (await IsDayExistAsync(classRoomAllocation.CourseId, classRoomAllocation.SevenDayWeekId))
        //    {
        //        return Conflict("The day is already assigned to the particular course.");
        //    }

        //    // Add classroom allocation
        //    db.ClassRoomAllocations.Add(classRoomAllocation);
        //    await db.SaveChangesAsync();

        //    return Ok(new
        //    {
        //        Message = "Class allocated successfully.",
        //        Allocation = classRoomAllocation
        //    });
        //}

        [HttpPost]
        public IActionResult Create(ClassRoomAllocation classRoomAllocation)
        {
            if (classRoomAllocation == null)
            {
                return BadRequest("Invalid classroom allocation data.");
            }

            // Assign default status
            classRoomAllocation.Status = "Allocate";
            classRoomAllocation.TimeFrom = classRoomAllocation.TimeFrom.Remove(2, 1);
            classRoomAllocation.TimeTo = classRoomAllocation.TimeTo.Remove(2, 1);

            if (Convert.ToInt32(classRoomAllocation.TimeFrom) <= Convert.ToInt32(classRoomAllocation.TimeTo))
            {
                if (IsDayExist(classRoomAllocation.CourseId, classRoomAllocation.SevenDayWeekId))
                {
                    db.ClassRoomAllocations.Add(classRoomAllocation);
                    db.SaveChanges();

                    TempData["SuccessMessage"] = "Class Allocated Successfully";
                    return RedirectToAction("Create", "AllocateClassRoom");
                }
                else
                {
                    TempData["ErrorMessage"] = "Class Not Allocated Because the Day is Already Assigned to the Particular Course.";
                    return RedirectToAction("Create", "AllocateClassRoom");
                }
            }

            TempData["ErrorMessage"] = "Time Format is Not Correct.";
            return RedirectToAction("Create", "AllocateClassRoom");
        }

        //public async Task<bool> IsDayExistAsync(int courseId, int sevenDayWeekId)
        //{
        //    return await db.ClassRoomAllocations.AnyAsync(x => x.CourseId == courseId &&
        //                                                  x.SevenDayWeekId == sevenDayWeekId && 
        //                                                  x.Status == "Allocate");
        //}

        public bool IsDayExist(int courseId, int dayId)
        {
            var classAllocation = db.ClassRoomAllocations.ToList();
            if (!classAllocation.Any(c => c.SevenDayWeekId == dayId && c.CourseId == courseId))
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
            //ViewClassSchedule gateway = new ViewClassSchedule();
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

        // LINQ Query for above function.
        //public async Task<List<ClassScheduleView>> GetAllClassScheduleViewsAsync(int departmentId)
        //{
        //    // Fetch the class schedule list from the gateway
        //    var gateway = new ViewClassSchedule();
        //    var classScheduleList = await gateway.GetAllClassScheduleViewsByDeptIdAsync(departmentId);
        //    if (classScheduleList == null || !classScheduleList.Any())
        //    {
        //        return new List<ClassScheduleView>();
        //    }

        //    // Group schedules by CourseCode
        //    var groupedSchedules = classScheduleList
        //        .GroupBy(schedule => schedule.CourseCode)
        //        .Select(group =>
        //        {
        //            var first = group.First();
        //            return new ClassScheduleView
        //            {
        //                CourseCode = first.CourseCode,
        //                CourseName = first.CourseName,
        //                ScheduleInfo = group.Any(x => string.IsNullOrEmpty(x.RoomName))
        //                    ? "Not Scheduled Yet"
        //                    : string.Join("</br>", group.Select(schedule =>
        //                        $"R. No : {schedule.RoomName}, {schedule.DayShortName}, {schedule.TimeFrom} - {schedule.TimeTo}"))
        //            };
        //        }).ToList();

        //    return groupedSchedules;
        //}

        [HttpGet]
        public IActionResult UnallocatedClassRooms()
        {
            ViewData["Message"] = "";
            return View();
        }

        [HttpPost]
        public IActionResult UnallocatedClassRooms(string unallocatedClassRoom)
        {
            //ViewClassSchedule gateway = new ViewClassSchedule();
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
