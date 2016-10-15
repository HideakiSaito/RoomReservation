using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoomReservation.Lib.Model
{
    public class Room
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string RoomName { get; set; }

        public int Capacity { get; set; }
    }
}
