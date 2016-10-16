using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Moq;
using NUnit.Framework;
using RoomReservation.Lib.Model;
using RoomReservation.Lib.Repositories;

namespace RoomReservation.Lib.Tests.Repositories
{
    [TestFixture]
    public class RoomRepositoryTest
    {
        [Test]
        public void GetRoomList_ReturnsAllRooms()
        {
            var roomList = new List<Room>()
            {
                new Room() {Id = 1, RoomName = "Room1", Capacity = 10},
                new Room() {Id = 2, RoomName = "Room2", Capacity = 4},
                new Room() {Id = 3, RoomName = "Room3", Capacity = 6}
            };
            var data = roomList.AsQueryable();
            var stubRooms = new Mock<DbSet<Room>>();
            stubRooms.As<IQueryable<Room>>().Setup(m => m.Provider).Returns(data.Provider);
            stubRooms.As<IQueryable<Room>>().Setup(m => m.Expression).Returns(data.Expression);
            stubRooms.As<IQueryable<Room>>().Setup(m => m.ElementType).Returns(data.ElementType);
            stubRooms.As<IQueryable<Room>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            var mockContext = new Mock<IBookingContext>();
            var repository = new RoomRepository(mockContext.Object);
            mockContext.Setup(x => x.Rooms).Returns(stubRooms.Object);

            var result = repository.GetRoomList();

            Assert.That(result, Is.EqualTo(data));
        }

        [Test]
        public void GetRoomBookings_WhenDateSpecified_ReturnsCorrectBookings()
        {
            var inputData = new List<BookingSchedule>()
            {
                new BookingSchedule()
                {
                    Id = 1,
                    RoomId = 1,
                    StartTime = new DateTime(2016, 10, 16, 18, 30, 0),
                    DurationInMinuts = 30
                },
                new BookingSchedule()
                {
                    Id = 2,
                    RoomId = 2,
                    StartTime = new DateTime(2016, 10, 16, 12, 0, 0),
                    DurationInMinuts = 60
                },
                new BookingSchedule()
                {
                    Id = 3,
                    RoomId = 1,
                    StartTime = new DateTime(2016, 10, 16, 10, 0, 0),
                    DurationInMinuts = 30
                },
                new BookingSchedule()
                {
                    Id = 4,
                    RoomId = 1,
                    StartTime = new DateTime(2016, 10, 17, 12, 0, 0),
                    DurationInMinuts = 60
                }
            }.AsQueryable();
            // expected output is filtered and ordered based on startTime
            var expectedOutput = new List<BookingSchedule>()
            {
                new BookingSchedule()
                {
                    Id = 3,
                    RoomId = 1,
                    StartTime = new DateTime(2016, 10, 16, 10, 0, 0),
                    DurationInMinuts = 30
                },
                new BookingSchedule()
                {
                    Id = 1,
                    RoomId = 1,
                    StartTime = new DateTime(2016, 10, 16, 18, 30, 0),
                    DurationInMinuts = 30
                }
            };

            var stubBookings = new Mock<DbSet<BookingSchedule>>();
            stubBookings.As<IQueryable<BookingSchedule>>().Setup(m => m.Provider).Returns(inputData.Provider);
            stubBookings.As<IQueryable<BookingSchedule>>().Setup(m => m.Expression).Returns(inputData.Expression);
            stubBookings.As<IQueryable<BookingSchedule>>().Setup(m => m.ElementType).Returns(inputData.ElementType);
            stubBookings.As<IQueryable<BookingSchedule>>()
                .Setup(m => m.GetEnumerator())
                .Returns(inputData.GetEnumerator());
            var stubContext = new Mock<IBookingContext>();
            var repository = new RoomRepository(stubContext.Object);
            stubContext.Setup(x => x.BookingSchedules).Returns(stubBookings.Object);

            var result = repository.GetRoomBookings(1, new DateTime(2016, 10, 16));

            Assert.That(result, Is.EqualTo(expectedOutput));
        }

        [Test]
        public void BookRoom_SavesToContext()
        {
            var bookingSchedule = new BookingSchedule()
            {
                Id = 1,
                StartTime = new DateTime(2016, 5, 5),
                RoomId = 1,
                DurationInMinuts = 30,
                PersonId = 1,
                Room = new Room(),
                Person = new Person()
            };
            var mockBookingScheduleSet = new Mock<DbSet<BookingSchedule>>();
            var mockContext = new Mock<BookingContext>();
            mockContext.Setup(x => x.BookingSchedules).Returns(mockBookingScheduleSet.Object);
            var repository = new RoomRepository(mockContext.Object);

            repository.BookRoom(bookingSchedule);

            // Though it's not recommended to assert against more than 1 mock object
            // here we have to do so as we need to make sure both Add() and SaveChanges() methods
            // are called
            mockBookingScheduleSet.Verify(b => b.Add(It.IsAny<BookingSchedule>()), Times.Once());
            mockContext.Verify(ctx => ctx.SaveChanges(), Times.Once());
        }

        [Test]
        public void RemoveBooking_SavesToContext()
        {
            var bookingSchedule = new BookingSchedule()
            {
                Id = 1,
                StartTime = new DateTime(2016, 5, 5),
                RoomId = 1,
                DurationInMinuts = 30,
                PersonId = 1,
                Room = new Room(),
                Person = new Person()
            };
            var mockBookingScheduleSet = new Mock<DbSet<BookingSchedule>>();
            var mockContext = new Mock<BookingContext>();
            mockContext.Setup(x => x.BookingSchedules).Returns(mockBookingScheduleSet.Object);
            var repository = new RoomRepository(mockContext.Object);

            repository.RemoveBooking(bookingSchedule.Id);

            mockBookingScheduleSet.Verify(b => b.Remove(It.IsAny<BookingSchedule>()), Times.Once());
            mockContext.Verify(ctx => ctx.SaveChanges(), Times.Once());
        }
    }
}
