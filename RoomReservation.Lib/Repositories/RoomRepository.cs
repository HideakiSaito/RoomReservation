using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoomReservation.Lib.Model;

namespace RoomReservation.Lib.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        private readonly IBookingContext _bookingContext;

        public RoomRepository(IBookingContext bookingContext)
        {
            _bookingContext = bookingContext;
        }

        public IEnumerable<Room> GetRoomList()
        {
            return _bookingContext.Rooms.ToList();
        }

        public IEnumerable<BookingSchedule> GetRoomBookings(int roomId, DateTime date)
        {
            var startDate = date.Date;
            var endDate = startDate.AddDays(1);
            return _bookingContext.BookingSchedules
                .Where(b => b.RoomId == roomId && b.StartTime >= startDate && b.StartTime < endDate)
                .OrderBy(b => b.StartTime)
                .ToList();
        }

        public void BookRoom(BookingSchedule bookingSchedule)
        {
            var booking = _bookingContext.BookingSchedules.Create();
            _bookingContext.BookingSchedules.Add(booking);
        }
    }
}
