using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TimeTracker
{
    public class Shift
    {
        DateTime startingTime;
        DateTime endingTime;

        public DateTime StartingTime
        { get{ return startingTime;}}

        public DateTime EndingTime
        {   get { return endingTime; } }

        public Shift(DateTime start, DateTime end)
        {
            if (start > end)
            {
                throw new ApplicationException("Start of shift must come before end.");
            }
            else
            {
                startingTime = start;
                endingTime = end;
            }
        }

        public static bool validDates(DateTime start, DateTime end)
        {
            return start < end;
        }

        public static bool validDates(string start, string end)
        {
            bool validFlag = false; 

            DateTime startDate;
            DateTime endDate;
            if (DateTime.TryParse(start, out startDate) && DateTime.TryParse(end, out endDate))
            {
                validFlag = validDates(startDate, endDate);
            }

            return validFlag;
        }

        public TimeSpan shiftSpan()
        {
            TimeSpan sSpan = endingTime.Subtract(startingTime);
            return sSpan;
        }

        public override string ToString()
        {
            return startingTime.ToString() + " - " + endingTime.ToString();
        }
    }
}
