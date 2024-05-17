using System.ComponentModel.DataAnnotations;

namespace UVCRMS.Models
{
    public class ClassRoomAllocation
    {
        public int Id { get; set; }

        [Required, Display(Name = "TimeFrom From"), StringLength(50)]
        public string TimeFrom { get; set; }

        [Required, Display(Name = "TimeFrom To"), StringLength(50)]
        public string TimeTo { get; set; }

        [StringLength(50)]
        public string? Status { get; set; }

        [Required, Display(Name = "Select Department")]
        public int DepartmentId { get; set; }

        [Required, Display(Name = "Select Course")]
        public int CourseId { get; set; }

        [Required, Display(Name = "Select Room")]
        public int RoomId { get; set; }

        [Required, Display(Name = "Select Day")]
        public int SevenDayWeekId { get; set; }


        public Department? Department { get; set; }
        public Course? Course { get; set; }
        public Room? Room { get; set; }
        public SevenDayWeek? SevenDayWeek { get; set;}
    }
}
