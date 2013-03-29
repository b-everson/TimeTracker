using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace TimeTracker
{
    public class PayPeriods:System.Collections.CollectionBase
    {
        public void Add(PayPeriod pPeriod)
        {
            string message = "";
          
            if (!validRange(pPeriod.StartDate,pPeriod.EndDate))
            {
                message += "Pay period range must not fall within other pay periods, check dates.";
            }

            if (message == "")
            {
                List.Add(pPeriod);
            }
            else
                throw new ApplicationException(message);
        }

        public void Insert(int index, PayPeriod payPeriod)
        {
            List.Insert(index, payPeriod);
        }

        public void Remove(PayPeriod pPeriod)
        {
            List.Remove(pPeriod);
        }

        public PayPeriod this[int index]
        {
            get{return (PayPeriod)List[index];}
        }

        /* public void SaveData(string fileName)
         * create XmlWriterSettings settings
         * try
         *   create new XmlWriter writer, passing fileName and settings
         *   write start element PayPeriods
         *     foreach PayPeriod in List
         *       write start element payperiod
         *         foreach Shift in Payperiod
         *           write start element shift
         *           write element startTime with content as startTime.ToString()
         *           write element EndTime with content as endTime.ToString()
         *           write shift end element
         *       write payperiod ending element
         *   write PayPeriods end element
         * catch
         *    throw ApplicationException(Error saving file)
         */ 
        public void SaveData(string fileName)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "    ";

                XmlWriter writer = XmlWriter.Create(fileName, settings);

                //write the start of the document
                writer.WriteStartDocument();
                writer.WriteStartElement("PayPeriods");

                foreach(PayPeriod payPeriod in List)
                {
                    writer.WriteStartElement("PayPeriod");
                    writer.WriteElementString("BeginningDate",payPeriod.StartDate.ToString());
                    writer.WriteElementString("EndingDate", payPeriod.EndDate.ToString());
                    foreach (Shift shift in payPeriod)
                    {
                        writer.WriteStartElement("Shift");
                        writer.WriteElementString("StartTime",shift.StartingTime.ToString());
                        writer.WriteElementString("EndTime", shift.EndingTime.ToString());
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
                writer.Close();
        }//save data

        /* public void LoadData(string fileName)
         * create XmlReaderSettings settings
         * set settings.IgnoreComments to true
         * set settings.IgnoreWhiteSpace to true
         * try
         *    create XmlReader reader(fileName,settings)
         *    if(reader.ReadToDescendent("PayPeriod")
         *       do
         *           PayPeriod newPayPeriod 
         *           DateTime start = reader.ReadElementContentAsDateTime
         *           DateTime end = reader.ReadElementContentAsDateTime
         *           newPayPeriod = new PayPeriod(start,end)
         *       
         *           if(reader.ReadToDescendent("Shift")
         *             do
         *                DateTime start = reader.ReadElementContentAsDateTime
         *                DateTime end = reader.ReadElementContentAsDateTime
         *                
         *             while reader.ReadToSibling("Shift")
         *           
         *       while reader.ReadToSibling("PayPeriod")
         */
        public void LoadData(string fileName)
        {
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreComments = true;
            settings.IgnoreWhitespace = true;
            try
            {
                XmlReader reader = XmlReader.Create(fileName, settings);
                try
                {
                    List.Clear();
                    if (reader.ReadToDescendant("PayPeriod"))
                    {
                        do
                        {
                            reader.ReadStartElement("PayPeriod");
                            DateTime start = DateTime.Parse(reader.ReadElementContentAsString());
                            DateTime end = DateTime.Parse(reader.ReadElementContentAsString());
                            PayPeriod newPayPeriod = new PayPeriod(start, end);
                            Add(newPayPeriod);
                            if (reader.Name == "Shift")
                            {
                                do
                                {
                                    reader.ReadStartElement("Shift");
                                    DateTime sStart = DateTime.Parse(reader.ReadElementContentAsString());
                                    DateTime sEnd = DateTime.Parse(reader.ReadElementContentAsString());
                                    Shift newShift = new Shift(sStart, sEnd);
                                    newPayPeriod.Add(newShift);
                                    reader.ReadEndElement();
                                }
                                while (reader.Name =="Shift");
                            }
                            reader.ReadEndElement();
                        }
                        while (reader.Name == "PayPeriod");
                    }
                }
                catch
                {
                    throw new ApplicationException("Error reading file");
                }
                finally
                {
                    reader.Close();
                }

            }
            catch 
            {
                
            }

        }

        /* public bool validRange
         * used to validate a PayPeriod to add
         */ 
        public bool validRange(DateTime start, DateTime end)
        {
            bool validFlag = true;
            foreach (PayPeriod payPeriod in List)
            {
                if (start < payPeriod.EndDate && start >= payPeriod.StartDate)
                {
                    validFlag = false;
                }
                if (end > payPeriod.StartDate && end <= payPeriod.EndDate)
                {
                    validFlag = false;
                }
            }
            return validFlag;
        }

        public bool validDates(DateTime start, DateTime end)
        {
            return start < end;
        }

        public bool validUpdate(PayPeriod payPeriod, DateTime start, DateTime end)
        {
            bool validFlag = true;
            foreach (PayPeriod listPayPeriod in List)
            {
                if (payPeriod == listPayPeriod)
                    continue;
                if (start >= listPayPeriod.StartDate && start < listPayPeriod.EndDate)
                    validFlag = false;
                if (end <= listPayPeriod.EndDate && end > listPayPeriod.StartDate)
                    validFlag = false;
            }
            return validFlag;
        }

        public TimeSpan PPeriodTotalTime()
        {
            TimeSpan total = new TimeSpan();
            foreach (PayPeriod payperiod in List)
            {
                total = total + payperiod.TotalLength;
            }
            return total;
        }
        
    }
}
