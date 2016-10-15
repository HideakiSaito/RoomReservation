using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomReservation.Lib.Model
{
    public class DropCreateBookingContextWithSeedDataIfModelChanges : DropCreateDatabaseIfModelChanges<BookingContext>
    {
        protected override void Seed(BookingContext context)
        {

        }
    }
}
