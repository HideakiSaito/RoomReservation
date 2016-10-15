using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace RoomReservation.Lib.Model
{
    public class BookingContext : DbContext, IBookingContext
    {
        public BookingContext() : base("BookingContext")
        {

        }
        public virtual DbSet<Room> Rooms { get; set; }
        public virtual DbSet<Person> People { get; set; }
        public virtual DbSet<BookingSchedule> BookingSchedules { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
