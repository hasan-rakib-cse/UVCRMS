
//using Microsoft.Data.SqlClient;
//using Microsoft.EntityFrameworkCore;
//using UVCRMS.Data;
//using UVCRMS.Models;
//using Microsoft.Extensions.Configuration;

//namespace UVCRMS.Gateway
//{
//    public class CourseAssignTeacherGateway
//    {
//        private readonly IConfiguration _configuration;
//        public CourseAssignTeacherGateway(IConfiguration configuration)
//        {
//            _configuration = configuration;
//        }

//        private string conString = WebConfigurationManager.ConnectionStrings["ApplicationDbContext"].ConnectionString;
//        //private string conString = _context.Database.GetDbConnection();

//        //private string conString = _configuration["ConnectionStrings:DefaultConnection"].

//        public List<ViewAssignCourse> GetAllAssignCourses(int deptId)
//        {
//            //var conString = _context.Database.GetDbConnection();

//            string query = "SELECT Courses.CourseCode, Courses.CoursName,Semesters.SemesterName,Teachers.TeacherName"
//                            + " FROM CourseAssignToTeachers"
//                            + " Right JOIN Courses ON Courses.Id=CourseAssignToTeachers.CourseId"
//                            + " LEFT JOIN Teachers ON Teachers.Id=CourseAssignToTeachers.TeacherId"
//                            + " Inner JOIN Semesters ON Courses.SemesterId=Semesters.Id"
//                            + " where Courses.DepartmentId='" + deptId + "' order by CourseCode";

//            SqlConnection connection = new SqlConnection(conString);
//            SqlCommand command = new SqlCommand(query, connection);
//            connection.Open();
//            SqlDataReader reader = command.ExecuteReader();
//            List<ViewAssignCourse> listOfItems = new List<ViewAssignCourse>();

//            while (reader.Read())
//            {
//                ViewAssignCourse viewAssignCourse = new ViewAssignCourse();

//                viewAssignCourse.CourseCode = reader["CourseCode"].ToString();
//                viewAssignCourse.CoursName = reader["CoursName"].ToString();
//                viewAssignCourse.CourseSemester = reader["SemesterName"].ToString();
//                viewAssignCourse.AssignTeacherName = reader["TeacherName"].ToString();

//                if (viewAssignCourse.AssignTeacherName == "")
//                {
//                    viewAssignCourse.AssignTeacherName = "Not Assigned Yet";
//                }

//                listOfItems.Add(viewAssignCourse);
//            }

//            connection.Close();
//            return listOfItems;
//        }


//        public bool UnAllocateClassRoom()
//        {
//            SqlConnection connection = new SqlConnection(conString);
//            string query = "UPDATE ClassRoomAllocations SET Status = " + "1" + " ";
//            SqlCommand command = new SqlCommand(query, connection);
//            connection.Open();
//            int rowAffect = command.ExecuteNonQuery();
//            connection.Close();

//            if (rowAffect > 0)
//            {
//                return true;
//            }

//            return false;
//        }

//    }
//}
