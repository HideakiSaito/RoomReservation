using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoomReservation.Lib.Model
{
    public class BookingSchedule
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public int PersonId { get; set; }

        [ForeignKey("PersonId")]
        public virtual Person Person { get; set; }

        public int RoomId { get; set; }

        [ForeignKey("RoomId")]
        public virtual Room Room { get; set; }

        public DateTime StartTime { get; set; }

        public int DurationInMinuts { get; set; }

        public DateTime EndTime => StartTime.AddMinutes(DurationInMinuts);

        public override bool Equals(object obj)
        {
            var castedObj = obj as BookingSchedule;
            if (castedObj == null)
                return false;

            return Id == castedObj.Id
                   && RoomId == castedObj.RoomId
                   && StartTime == castedObj.StartTime
                   && DurationInMinuts == castedObj.DurationInMinuts;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
