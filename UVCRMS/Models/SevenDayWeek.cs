using System.ComponentModel.DataAnnotations;

namespace UVCRMS.Models
{
    public class SevenDayWeek
    {
        public int Id { get; set; }

        [StringLength(20)]
        public string DayCode { get; set; }

        [Required, StringLength(30)]
        public string DayName { get; set; }



        public List<ClassRoomAllocation>? ClassRoomAllocations { get; set; }
    }
}
