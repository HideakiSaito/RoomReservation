using System.Data.Entity;

namespace RoomReservation.Lib.Model
{
    public interface IBookingContext
    {
        DbSet<Room> Rooms { get; set; }
        DbSet<Person> People { get; set; }
        DbSet<BookingSchedule> BookingSchedules { get; set; }
    }
}
