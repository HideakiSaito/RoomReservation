using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoomReservation.Lib.Model;

namespace RoomReservation.Lib.Repositories
{
    public interface IRoomRepository
    {
        IEnumerable<Room> GetRoomList();
        IEnumerable<BookingSchedule> GetRoomBookings(int roomId, DateTime date);
        void BookRoom(BookingSchedule bookingSchedule);
    }
}
