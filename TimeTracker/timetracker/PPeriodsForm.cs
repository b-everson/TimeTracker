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
    public partial class PPeriodsForm : Form
    {
        public PPeriodsForm()
        {
            InitializeComponent();
        }

        public PayPeriods periodsList = new PayPeriods();

        public void AddPayPeriod(DateTime start, DateTime end)
        {
            string message = "";
            if (!PayPeriod.validDates(start, end))
            {
                message = "Pay period start must come before pay period end" + Environment.NewLine;
            }
            
            if(!periodsList.validRange(start,end))
            {
                message += "Pay period range is invalid, make sure there are no pay period overlaps.";
            }

            if (message == "")
            {
                PayPeriod pPeriod = new PayPeriod(start, end);
                periodsList.Add(pPeriod);
                lstPPeriods.Items.Add(periodsList[periodsList.Count - 1]);
            }
            else
            {
                MessageBox.Show(message);
            }

        }

        public void RemovePayPeriod(int index)
        {
            periodsList.RemoveAt(index);
            lstPPeriods.Items.RemoveAt(index);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            int index = lstPPeriods.SelectedIndex;
            if (index != -1)
            {
                //open second form 
                new ShiftForm(periodsList[index]).ShowDialog();
                displayHours(periodsList[index]);
                updateTotal();
            }
        }

        private void displayHours(PayPeriod sPayPeriod)
        {
            int accumulatedHours = (sPayPeriod.TotalLength.Days * 24) + sPayPeriod.TotalLength.Hours;
            txtPPeriodTime.Text = accumulatedHours.ToString() + ":" + sPayPeriod.TotalLength.Minutes.ToString();
        }

        /* create a pay period
         call addPayPeriod(
         */ 
        private void btnCreatePPeriod_Click(object sender, EventArgs e)
        {
            AddPayPeriod(dtpStartDate.Value, dtpEndDate.Value);
            updateTotal();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            int index = lstPPeriods.SelectedIndex;
            if (index != -1)
            {
                RemovePayPeriod(index);
                updateTotal();
            }
            else
            {
                MessageBox.Show("Must select a pay period to remove one.", "Error");
            }
        }

        private void lstPPeriods_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = lstPPeriods.SelectedIndex;
            if (index != -1)
            {
                PayPeriod sPayPeriod = periodsList[index];
                displayHours(sPayPeriod);
                dtpStartDate.Value = sPayPeriod.StartDate;
                dtpEndDate.Value = sPayPeriod.EndDate;
            }
        }

        private void SaveFile(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.CheckPathExists = true;
            dlg.Filter = "Xml File|*.xml";
            dlg.Title = "Specify file name.";
            dlg.ShowDialog();

            if (dlg.FileName != "")
            {
                periodsList.SaveData(dlg.FileName);
            }
        }

        private void LoadFile(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.CheckFileExists = true;
            dlg.Filter = "Xml File|*.xml";
            dlg.Title = "Specify file name.";
            dlg.ShowDialog();

            if (dlg.FileName != "")
            {
                periodsList.Clear();
                lstPPeriods.Items.Clear();
                periodsList.LoadData(dlg.FileName);
                foreach (PayPeriod payPeriod in periodsList)
                {
                    lstPPeriods.Items.Add(payPeriod);
                }
                updateTotal();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            int index = lstPPeriods.SelectedIndex;
            if (index != -1)
            {
                PayPeriod updatePayPeriod = periodsList[index];
                string message = "";
                if (!periodsList.validUpdate(updatePayPeriod,dtpStartDate.Value,dtpEndDate.Value))
                {
                    message = "Pay period may not overlap any other pay periods." + Environment.NewLine;
                }

                if(!PayPeriod.validDates(dtpStartDate.Value,dtpEndDate.Value))
                {
                    message += "Pay period start date must come before end date";
                }

                if (message == "")
                {
                    //remove old pay period, insert new pay period at index from periodsList and lstPPeriods
                    periodsList[index].modifyDates(dtpStartDate.Value, dtpEndDate.Value);
                    lstPPeriods.Items.RemoveAt(index);
                    lstPPeriods.Items.Insert(index, periodsList[index]);
                    updateTotal();
                }
                else
                    MessageBox.Show(message);
            }
        }


        private void updateTotal()
        {
            TimeSpan total = periodsList.PPeriodTotalTime();
            int hours = total.Days * 24 + total.Hours;
            txtTotalTime.Text = hours.ToString() + ":" + total.Minutes;
        }
    }
}
