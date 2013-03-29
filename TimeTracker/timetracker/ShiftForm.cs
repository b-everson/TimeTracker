using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TimeTracker
{
    public partial class ShiftForm : Form
    {
        PayPeriod myPayPeriod;

        /* public ShiftForm(Payperiod pPeriod)
         * 
         */ 
        public ShiftForm(PayPeriod pPeriod)
        {
            InitializeComponent();
            myPayPeriod = pPeriod;
            foreach (Shift shift in myPayPeriod)
            {
                lstShifts.Items.Add(shift);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int index = lstShifts.SelectedIndex;
            if (index != -1)
            {
                myPayPeriod.RemoveAt(index);
                lstShifts.Items.RemoveAt(index);
            }
            else
                MessageBox.Show("You must select an item to remove it.");
        }

        /* btnAdd_click
         * 
         * 
         */ 
        private void btnAdd_Click(object sender, EventArgs e)
        {
            string message = "";
            ClearErrors();
            if (!(Shift.validDates(mtbStart.Text, mtbEnd.Text)))
            {
                message = "End date must come after beginning date." + Environment.NewLine;
            }

            if (!(myPayPeriod.validShift(mtbStart.Text, mtbEnd.Text)))
            {
                message += "Shift cannot overlap other shifts in pay period.";
            }

            if (message == "")
            {
                myPayPeriod.Add(new Shift(DateTime.Parse(mtbStart.Text), DateTime.Parse(mtbEnd.Text)));
                lstShifts.Items.Add(myPayPeriod[myPayPeriod.Count - 1]);
            }
            else
            {
                errorProvider1.SetError(mtbEnd,message);
            }
        }

        private void ClearErrors()
        {
            errorProvider1.SetError(mtbEnd, "");
            errorProvider1.SetError(lstShifts, "");
        }

        /* update button
         * get index of selected item from listbox
         * if index != -1
         *   string message = ""
         *   if not Shift.validDates(txtStart, txtEnd)
         *      message = Shift start must come before shift end
         *   if not myPayPeriod.validShift(txtStart,txtEnd)
         *      message += Shift may not overlap other shifts
         *   if message is empty string
         *      myPayPeriod.Insert(new Shift(txtStart,txtEnd),index)
         *      myPayPeriod.RemoveAt(index +1)
         *   else
         *     errorprovider1.setError(mtbEnd,message)
         */ 
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            int index = lstShifts.SelectedIndex;
            if (index != -1)
            {
                string message = "";
                if (!Shift.validDates(mtbStart.Text, mtbEnd.Text))
                    message = "Shift start must come before shift end." + Environment.NewLine;
                if (!myPayPeriod.validShift(mtbStart.Text, mtbEnd.Text))
                    message += "Shift may not overlap other shifts.";
                if (message == "")
                {
                    myPayPeriod.Insert(index, new Shift(DateTime.Parse(mtbStart.Text), DateTime.Parse(mtbEnd.Text)));
                    myPayPeriod.RemoveAt(index + 1);
                    lstShifts.Items.RemoveAt(index);
                    lstShifts.Items.Insert(index, myPayPeriod[index]);
                }
                else
                    errorProvider1.SetError(mtbEnd, message);
            }
        }
        
    }
}
