using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Moq;
using NUnit.Framework;
using RoomReservation.Lib.Model;
using RoomReservation.Lib.Repositories;
using RoomReservation.Lib.Services;

namespace RoomReservation.Lib.Tests.Services
{
    [TestFixture]
    public class BookingServiceTest
    {
        [Test]
        public void BookRoom_WhenOverlapsForTheSameRoom_ReturnsFalse1()
        {
            var currentBooking1 = new BookingSchedule()
            {
                Id = 1,
                StartTime = new DateTime(2016, 10, 15, 12, 30, 0),
                DurationInMinuts = 30,
                RoomId = 1
            };
            var currentBookings = new List<BookingSchedule>()
            {
                currentBooking1
            };
            var bookingService = CreateServiceBasedOnCurrentBookingScheduleList(currentBookings);

            var newBooking = new BookingSchedule()
            {
                Id = 100,
                StartTime = new DateTime(2016, 10, 15, 12, 0, 0),
                DurationInMinuts = 60,
                RoomId = 1
            };

            var result = bookingService.BookRoom(newBooking);

            Assert.That(result, Is.EqualTo(false));
        }

        [Test]
        public void BookRoom_WhenOverlapsForTheSameRoom_ReturnsFalse2()
        {
            var currentBooking1 = new BookingSchedule()
            {
                Id = 1,
                StartTime = new DateTime(2016, 10, 15, 13, 0, 0),
                DurationInMinuts = 30,
                RoomId = 1
            };
            var currentBookings = new List<BookingSchedule>()
            {
                currentBooking1
            };
            var bookingService = CreateServiceBasedOnCurrentBookingScheduleList(currentBookings);

            var newBooking = new BookingSchedule()
            {
                Id = 100,
                StartTime = new DateTime(2016, 10, 15, 12, 30, 0),
                DurationInMinuts = 60,
                RoomId = 1
            };

            var result = bookingService.BookRoom(newBooking);

            Assert.That(result, Is.EqualTo(false));
        }

        [Test]
        public void BookRoom_WhenBoundaryTimesMeetForTheSameRoom_ReturnsTrue()
        {
            var currentBooking1 = new BookingSchedule()
            {
                Id = 1,
                StartTime = new DateTime(2016, 10, 15, 12, 30, 0),
                DurationInMinuts = 30,
                RoomId = 1
            };
            var currentBooking2 = new BookingSchedule()
            {
                Id = 1,
                StartTime = new DateTime(2016, 10, 15, 14, 0, 0),
                DurationInMinuts = 60,
                RoomId = 1
            };
            var currentBookings = new List<BookingSchedule>()
            {
                currentBooking1,
                currentBooking2
            };
            var bookingService = CreateServiceBasedOnCurrentBookingScheduleList(currentBookings);

            var newBooking = new BookingSchedule()
            {
                Id = 100,
                StartTime = new DateTime(2016, 10, 15, 13, 0, 0),
                DurationInMinuts = 60,
                RoomId = 1
            };

            var result = bookingService.BookRoom(newBooking);


            Assert.That(result, Is.EqualTo(true));
        }

        [Test]
        public void BookRoom_WhenOverlapsForTheDifferentRooms_ReturnsTrue()
        {
            var currentBooking1 = new BookingSchedule()
            {
                Id = 1,
                StartTime = new DateTime(2016, 10, 15, 12, 30, 0),
                DurationInMinuts = 30,
                RoomId = 1
            };
            var currentBookings = new List<BookingSchedule>()
            {
                currentBooking1
            };
            var bookingService = CreateServiceBasedOnCurrentBookingScheduleList(currentBookings);

            var newBooking = new BookingSchedule()
            {
                Id = 100,
                StartTime = new DateTime(2016, 10, 15, 12, 0, 0),
                DurationInMinuts = 60,
                RoomId = 2
            };

            var result = bookingService.BookRoom(newBooking);

            Assert.That(result, Is.EqualTo(true));
        }

        [Test]
        public void BookRoom_WhenNoBookingOverlapForTheSameRoom_ReturnsTrue()
        {
            var currentBooking1 = new BookingSchedule()
            {
                Id = 1,
                StartTime = new DateTime(2016, 10, 15, 13, 30, 0),
                DurationInMinuts = 30,
                RoomId = 1
            };
            var currentBookings = new List<BookingSchedule>()
            {
                currentBooking1
            };
            var bookingService = CreateServiceBasedOnCurrentBookingScheduleList(currentBookings);

            var newBooking = new BookingSchedule()
            {
                Id = 100,
                StartTime = new DateTime(2016, 10, 15, 12, 0, 0),
                DurationInMinuts = 60,
                RoomId = 1
            };

            var result = bookingService.BookRoom(newBooking);

            Assert.That(result, Is.EqualTo(true));
        }

        private static BookingService CreateServiceBasedOnCurrentBookingScheduleList(List<BookingSchedule> currentBookings)
        {
            var data = currentBookings.AsQueryable();
            var stubBookingSet = new Mock<DbSet<BookingSchedule>>();
            stubBookingSet.As<IQueryable<BookingSchedule>>().Setup(m => m.Provider).Returns(data.Provider);
            stubBookingSet.As<IQueryable<BookingSchedule>>().Setup(m => m.Expression).Returns(data.Expression);
            stubBookingSet.As<IQueryable<BookingSchedule>>().Setup(m => m.ElementType).Returns(data.ElementType);
            stubBookingSet.As<IQueryable<BookingSchedule>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var stubContext = new Mock<BookingContext>();
            stubContext.Setup(x => x.BookingSchedules).Returns(stubBookingSet.Object);
            var roomRepository = new RoomRepository(stubContext.Object);
            var bookingService = new BookingService(roomRepository);
            return bookingService;
        }
    }
}
