using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace TimeTracker
{
    public class PayPeriod: System.Collections.CollectionBase
    {
        private DateTime startDate;
        private DateTime endDate;

        public DateTime StartDate { get { return startDate; } }
        public DateTime EndDate   { get { return endDate; }}
        const int LAST_HOUR = 23;
        const int LAST_MINUTE = 59;
        const int LAST_SECOND = 59;

        public PayPeriod(DateTime start, DateTime end)
        {
            string message = "";
            if (!(end >= start))
            {
                message += "Start date must be before end date";
            }
            if (message == "")
            {
                int month = end.Month;
                int day = end.Day;
                int year = end.Year;
                endDate = new DateTime(year, month, day, LAST_HOUR, LAST_MINUTE, LAST_SECOND);
                startDate = start;
            }
            else
                throw new ApplicationException(message);
        }

        public static bool validDates(DateTime start, DateTime end)
        {
            return start < end;
        }

        public void Add(Shift newShift)
        {
            List.Add(newShift);
        }

        public void Remove(Shift rShift)
        {
            List.Remove(rShift);
        }

        public Shift this[int index]
        {
            get { return (Shift)List[index]; }
        }

        private TimeSpan totalLength()
        {
            TimeSpan total = new TimeSpan();
            foreach (Shift shift in List)
            {
                TimeSpan sSpan = shift.shiftSpan();
                total = total.Add(sSpan);
            }
            return total;
        }

        public TimeSpan TotalLength
        {
            get { return totalLength(); }
        }

        public void Insert(int index,Shift shift)
        {
            List.Insert(index, shift);
        }

        public override string ToString()
        {
            return String.Format("{0} - {1}", StartDate.ToShortDateString(), EndDate.ToShortDateString());
        }

        /*validShiftDate(start, end)
         * if shift start is before pay period start
         *    validflag is false
         * if shift start is after pay period end 
         *    validflag= false
         * foreach shift in List
         *    if start is after shift start and before shift end
         *       validflag is false
         *    if end is after shift start and before shift end  
         *       validflag is false
         * return validflag      
         */ 
        public bool validShift(DateTime start, DateTime end)
        {
            bool validFlag = true;
            if (start < startDate || start > endDate)
            {
                validFlag = false;
            }

            foreach (Shift shift in List)
            {
                if (start >= shift.StartingTime && start < shift.EndingTime)
                {
                    validFlag = false;
                }
                if(end > shift.StartingTime && end <= shift.EndingTime)
                {
                    validFlag = false;
                }
            }

            return validFlag;
        }

        public bool validShift(string start, string end)
        {
            bool validFlag = false;
            DateTime startDate;
            DateTime endDate;
            if (DateTime.TryParse(start, out startDate) && DateTime.TryParse(end, out endDate))
            {
                return validShift(startDate, endDate);
            }
            return validFlag;
        }

        public void modifyDates(DateTime start, DateTime end)
        {
            startDate = start;
            endDate = end;
        }
    }
}
