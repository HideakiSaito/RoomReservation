using System;
using System.Data.Entity;

namespace RoomReservation.Lib.Model
{
    public interface IBookingContext : IDisposable
    {
        DbSet<Room> Rooms { get; set; }
        DbSet<Person> People { get; set; }
        DbSet<BookingSchedule> BookingSchedules { get; set; }

        int SaveChanges();
    }
}
