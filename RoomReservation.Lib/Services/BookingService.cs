using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoomReservation.Lib.Model;
using RoomReservation.Lib.Repositories;

namespace RoomReservation.Lib.Services
{
    public class BookingService : IBookingService
    {
        private readonly IRoomRepository _roomRepository;

        public BookingService(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        public IEnumerable<Room> GetRoomList()
        {
            return _roomRepository.GetRoomList();
        }

        public IEnumerable<BookingSchedule> GetRoomBookings(int roomId, DateTime date)
        {
            return _roomRepository.GetRoomBookings(roomId, date);
        }

        public bool BookRoom(BookingSchedule bookingSchedule)
        {
            if (!BookingIsAllowed(bookingSchedule)) return false;
            _roomRepository.BookRoom(bookingSchedule);
            return true;
        }

        private bool BookingIsAllowed(BookingSchedule bookingSchedule)
        {
            var bookings = GetRoomBookings(bookingSchedule.RoomId, bookingSchedule.StartTime.Date);
            var overlap = bookings.Any(b => b.StartTime < bookingSchedule.EndTime
                                && b.EndTime > bookingSchedule.StartTime);
            return !overlap;
        }
    }
}
