using System;
using System.Collections;
using System.Collections.Generic;
using HotelBooking.Core;
using HotelBooking.UnitTests.Fakes;
using Xunit;

namespace HotelBooking.UnitTests
{
    public class BookingManagerTests
    {
        private IBookingManager bookingManager;
         
        public BookingManagerTests(){
            DateTime start = DateTime.Today.AddDays(10);
            DateTime end = DateTime.Today.AddDays(20);
            IRepository<Booking> bookingRepository = new FakeBookingRepository(start, end);
            IRepository<Room> roomRepository = new FakeRoomRepository();
            bookingManager = new BookingManager(bookingRepository, roomRepository);
        }

        public static IEnumerable<object[]> GetLocalData()
        {
            var data = new List<object[]>
            {
                new object[] { DateTime.Today.AddDays(1), DateTime.Today.AddDays(10) },
                new object[] { DateTime.Today.AddDays(10), DateTime.Today.AddDays(15) },
                new object[] { DateTime.Today.AddDays(11), DateTime.Today.AddDays(25) },
            };
            return data;
        }

        [Fact]
        public void FindAvailableRoom_StartDateNotInTheFuture_ThrowsArgumentException()
        {
            // Arrange
            DateTime date = DateTime.Today;

            // Act
            Action act = () => bookingManager.FindAvailableRoom(date, date);

            // Assert
            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void FindAvailableRoom_RoomAvailable_RoomIdNotMinusOne()
        {
            // Arrange
            DateTime date = DateTime.Today.AddDays(1);
            // Act
            int roomId = bookingManager.FindAvailableRoom(date, date);
            // Assert
            Assert.NotEqual(-1, roomId);
        }

        [Theory]
        [MemberData(nameof(GetLocalData))]
        public void FindAvailableRoom_RoomNotAvailable_RoomIdMinusOne(DateTime startDate, DateTime endDate)
        {
            // Arrange

            // Act
            int roomId = bookingManager.FindAvailableRoom(startDate, endDate);
            // Assert
            Assert.Equal(-1, roomId);
        }


    }
}
