using System;
using System.Collections.Generic;
using System.Text;

namespace SmartParkingApp
{
    public class ApplicationHelper
    {
        private static ApplicationHelper instance;
        private DateTime? currentDate;
        public DateTime CurrentDate
        {
            get
            {
                if (currentDate.HasValue)
                    return currentDate.Value;
                else return DateTime.Now;
            } 
            set { currentDate = value; }
        }
        public static ApplicationHelper Instance
        {
            get 
            { 
                if(instance == null)
                    instance = new ApplicationHelper();
                return instance;
            }
        }
        private ApplicationHelper()
        {

        }

    }
}
