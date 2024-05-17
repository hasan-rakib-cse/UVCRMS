using System.ComponentModel.DataAnnotations;

namespace UVCRMS.Models
{
    public class Room
    {
        public int Id { get; set; }

        [Required]
        public string RoomNo { get; set; }

        public string? RoomName { get; set; }

        public string? RoomLocation { get; set; }



        public List<ClassRoomAllocation>? ClassRoomAllocations { get; set; }
    }
}
