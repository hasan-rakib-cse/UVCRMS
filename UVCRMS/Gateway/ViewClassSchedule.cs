using Microsoft.Data.SqlClient;
using System.Data;
using UVCRMS.Models;

namespace UVCRMS.Gateway
{
    public class ViewClassSchedule
    {
        private readonly string conString;

        public ViewClassSchedule (IConfiguration configuration)
        {
            conString = configuration.GetConnectionString("ApplicationDbContext");
        }

        //private string conString = WebConfigurationManager.ConnectionStrings["ApplicationDbContext"].ConnectionString;

        public List<ClassScheduleView> GetAllClassScheduleViewsByDeptId(int deptId)
        {
            SqlConnection connection = new SqlConnection(conString);

            string query =
                "SELECT Courses.CourseCode, Courses.CoursName,SevenDayWeeks.DayCode,ClassRoomAllocations.TimeFrom," +
                " ClassRoomAllocations.TimeTo,ClassRoomAllocations.Status,Rooms.RoomNo" +
                " FROM  ClassRoomAllocations" +
                " Left join SevenDayWeeks ON SevenDayWeeks.Id=ClassRoomAllocations.SevenDayWeekId" +
                " Inner join Rooms ON Rooms.Id=ClassRoomAllocations.RoomId" +
                " Right JOIN Courses ON Courses.Id=ClassRoomAllocations.CourseId" +
                " where Courses.DepartmentId='" + deptId + "' order by CourseCode";

            SqlCommand command = new SqlCommand(query, connection);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            List<ClassScheduleView> listOfItems = new List<ClassScheduleView>();

            while (reader.Read())
            {
                if (reader["Status"].ToString() == "1")
                {
                    continue;
                }

                ClassScheduleView item = new ClassScheduleView();
                item.CourseCode = reader["CourseCode"].ToString();
                item.CourseName = reader["CoursName"].ToString();
                item.DayShortName = reader["DayCode"].ToString();
                item.TimeFrom = reader["TimeFrom"].ToString();

                if (item.TimeFrom != "" && Convert.ToInt32(item.TimeFrom) >= 1200)
                {
                    int temp1 = Convert.ToInt32(item.TimeFrom) - 1200;
                    string timeFrom = "";
                    if (temp1.ToString().Length == 1)
                    {
                        timeFrom = "12:00" + " PM";
                    }
                    else if (temp1.ToString().Length == 2)
                    {
                        timeFrom = "12:" + temp1 + " PM";
                    }
                    else if (temp1.ToString().Length == 3)
                    {
                        string temp = temp1.ToString();
                        char[] array = temp.ToCharArray();
                        timeFrom = "0" + array[0] + ":" + array[1] + array[2] + " PM";
                    }
                    else
                    {
                        string temp = temp1.ToString();
                        char[] array = temp.ToCharArray();
                        timeFrom = "" + array[0] + array[1] + ":" + array[2] + array[3] + " PM";
                    }

                    item.TimeFrom = timeFrom;
                }
                else if (item.TimeFrom != "" && Convert.ToInt32(item.TimeFrom) < 1200)
                {
                    string temp = item.TimeFrom.ToString();
                    char[] array = temp.ToCharArray();
                    item.TimeFrom = "" + array[0] + array[1] + ":" + array[2] + array[3] + " AM";
                }

                item.TimeTo = reader["TimeTo"].ToString();

                if (item.TimeTo != "" && Convert.ToInt32(item.TimeTo) >= 1200)
                {
                    int temp1 = Convert.ToInt32(item.TimeTo) - 1200;
                    string timeTo = "";
                    if (temp1.ToString().Length == 1)
                    {
                        timeTo = "12:00" + " PM";
                    }
                    else if (temp1.ToString().Length == 2)
                    {
                        timeTo = "12:" + temp1 + " PM";
                    }
                    else if (temp1.ToString().Length == 3)
                    {
                        string temp = temp1.ToString();
                        char[] array = temp.ToCharArray();
                        timeTo = "0" + array[0] + ":" + array[1] + array[2] + " PM";
                    }
                    else
                    {
                        string temp = temp1.ToString();
                        char[] array = temp.ToCharArray();
                        timeTo = "" + array[0] + array[1] + ":" + array[2] + array[3] + " PM";
                    }
                    item.TimeTo = timeTo;
                }
                else if (item.TimeTo != "" && Convert.ToInt32(item.TimeTo) < 1200)
                {
                    string temp = item.TimeTo.ToString();
                    char[] array = temp.ToCharArray();
                    item.TimeTo = "" + array[0] + array[1] + ":" + array[2] + array[3] + " AM";
                }

                item.RoomName = reader["RoomNo"].ToString();
                listOfItems.Add(item);

            }
            connection.Close();
            return listOfItems;
        }

        // Modified Code using LINQ.
        //public async Task<List<ClassScheduleView>> GetAllClassScheduleViewsByDeptIdAsync(int deptId)
        //{
        //    var query = @"
        //    SELECT 
        //        Courses.CourseCode, 
        //        Courses.CoursName, 
        //        SevenDayWeeks.DayCode, 
        //        ClassRoomAllocations.TimeFrom,
        //        ClassRoomAllocations.TimeTo, 
        //        ClassRoomAllocations.Status, 
        //        Rooms.RoomNo
        //    FROM ClassRoomAllocations
        //    LEFT JOIN SevenDayWeeks ON SevenDayWeeks.Id = ClassRoomAllocations.SevenDayWeekId
        //    INNER JOIN Rooms ON Rooms.Id = ClassRoomAllocations.RoomId
        //    RIGHT JOIN Courses ON Courses.Id = ClassRoomAllocations.CourseId
        //    WHERE Courses.DepartmentId = @DeptId
        //    ORDER BY CourseCode";

        //    var classScheduleViews = new List<ClassScheduleView>();

        //    await using var connection = new SqlConnection(conString);
        //    await using var command = new SqlCommand(query, connection);
        //    command.Parameters.AddWithValue("@DeptId", deptId);

        //    await connection.OpenAsync();
        //    await using var reader = await command.ExecuteReaderAsync();

        //    while (await reader.ReadAsync())
        //    {
        //        if (reader["Status"].ToString() == "1")
        //        {
        //            continue;
        //        }

        //        var item = new ClassScheduleView
        //        {
        //            CourseCode = reader["CourseCode"].ToString(),
        //            CourseName = reader["CoursName"].ToString(),
        //            DayShortName = reader["DayCode"].ToString(),
        //            RoomName = reader["RoomNo"].ToString(),
        //            TimeFrom = FormatTime(reader["TimeFrom"].ToString()),
        //            TimeTo = FormatTime(reader["TimeTo"].ToString())
        //        };

        //        classScheduleViews.Add(item);
        //    }

        //    return classScheduleViews;
        //}

        //private string FormatTime(string time)
        //{
        //    if (string.IsNullOrEmpty(time)) return string.Empty;

        //    if (!int.TryParse(time, out var timeValue)) return time;

        //    var isPM = timeValue >= 1200;
        //    timeValue = isPM && timeValue > 1200 ? timeValue - 1200 : timeValue;

        //    var hours = timeValue / 100;
        //    var minutes = timeValue % 100;

        //    return $"{hours:D2}:{minutes:D2} {(isPM ? "PM" : "AM")}";
        //}

        public bool UnAllocateClassRoom()
        {
            SqlConnection connection = new SqlConnection(conString);
            string query = "UPDATE ClassRoomAllocations SET Status = " + "1" + " ";
            SqlCommand command = new SqlCommand(query, connection);
            connection.Open();
            int rowAffect = command.ExecuteNonQuery();
            connection.Close();

            if (rowAffect > 0)
            {
                return true;
            }
            return false;
        }

        // Modern Code
        //public bool UnAllocateClassRoom()
        //{
        //    const string query = "UPDATE ClassRoomAllocations SET Status = 1";

        //    using var connection = new SqlConnection(_connectionString);
        //    using var command = new SqlCommand(query, connection);

        //    connection.Open();
        //    var rowsAffected = command.ExecuteNonQuery();
        //    return rowsAffected > 0;
        //}
    }
}
