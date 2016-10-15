using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RoomReservation.Lib.Model;

namespace RoomReservation.WebApp.App_Start
{
    public class ModelConfig
    {
        public static void InitilizeDb()
        {
            DbInitializer.GetInstance().InitializeDb();
        }
    }
}