using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoomReservation.Lib.Model;

namespace RoomReservation.Lib.Services
{
    interface IBookingService
    {
        IEnumerable<Room> GetRoomList();
        IEnumerable<BookingSchedule> GetRoomBookings(int roomId, DateTime date);
        bool BookRoom(BookingSchedule bookingSchedule);
    }
}
