using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomReservation.Lib.Model
{
    public class DbInitializer
    {
        #region Singleton

        private static readonly Lazy<DbInitializer> _instance = new Lazy<DbInitializer>(() => new DbInitializer());

        private DbInitializer()
        {
        }

        public static DbInitializer GetInstance()
        {
            return _instance.Value;
        }

        #endregion

        public void InitializeDb()
        {
            Database.SetInitializer<BookingContext>(new DropCreateBookingContextWithSeedDataIfModelChanges());
            using (var context = new BookingContext())
            {
                context.Database.Initialize(force: false);
            }
        }
    }
}
