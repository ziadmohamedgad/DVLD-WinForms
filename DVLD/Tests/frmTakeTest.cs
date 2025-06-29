using DVLD.Global_Classes;
using DVLD_BusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace DVLD.Tests
{
    public partial class frmTakeTest : Form
    {
        private int _AppointmentID = -1;
        private clsTestType.enTestType _TestType;
        private int _TestID = -1;
        private clsTest _Test;
        public frmTakeTest(int AppointmentID, clsTestType.enTestType TestType)
        {
            InitializeComponent();
            this._AppointmentID = AppointmentID;
            this._TestType = TestType;
        }
        private void frmTakeTest_Load(object sender, EventArgs e)
        {
            ctrlScheduledTest1.TestTypeID = _TestType;
            ctrlScheduledTest1.LoadInfo(_AppointmentID);
            if (ctrlScheduledTest1.TestAppointmentID == -1) // how to take test without a previous appointment??
                btnSave.Enabled = false;
            else
                btnSave.Enabled = true;
            _TestID = ctrlScheduledTest1.TestID; 
            if (_TestID != -1) // if there is a previous test we have to load it's data and disable editing
            {
                _Test = clsTest.Find(_TestID);
                if (_Test.TestResult)
                    rbPass.Checked = true;
                else
                    rbFail.Checked = true;
                txtNotes.Text = _Test.Notes;
                lblUserMessage.Visible = true; // the message is : 'we cannot change the result'.
                rbFail.Enabled = false;
                rbPass.Enabled = false;
                txtNotes.Enabled = false;
            }
            else
                _Test = new clsTest();
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are You Sure You Want To Save? (You Will Not Be Able To Change The Result Later!",
                "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                return;
            _Test.TestAppointmentID = _AppointmentID;
            _Test.TestResult = rbPass.Checked;
            _Test.Notes = txtNotes.Text.Trim();
            _Test.CreatedByUserID = clsRegLogger.CurrentUser.UserID;
            if (_Test.Save())
            {
                MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                lblUserMessage.Visible = true; // the message is : 'we cannot change the result'.
                rbFail.Enabled = false;
                rbPass.Enabled = false;
                txtNotes.Enabled = false;
                btnSave.Enabled = false;
            }
            else
                MessageBox.Show("Error: Data Is not Saved Successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}