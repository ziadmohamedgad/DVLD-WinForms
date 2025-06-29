using DVLD.Properties;
using DVLD.Tests.Controls;
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
    public partial class frmListTestAppointments : Form
    {
        private DataTable _dtLicenseTestAppointments;
        private int _LDLAppID = -1;
        private clsTestType.enTestType _TestType = clsTestType.enTestType.VisionTest;
        public frmListTestAppointments(int LocalDrivingLicenseApplicationID, clsTestType.enTestType TestType)
        {
            InitializeComponent();
            this._TestType = TestType;
            this._LDLAppID = LocalDrivingLicenseApplicationID;
        }
        private void _LoadTestTypeImageAndTitle()
        {
            switch(_TestType)
            {
                case clsTestType.enTestType.VisionTest:
                    {
                        lblTitle.Text = "Vision Test Appointment";
                        pbTestTypeImage.Image = Resources.Vision_512;
                        break;
                    }
                case clsTestType.enTestType.WrittenTest:
                    {
                        lblTitle.Text = "Written Test Appointment";
                        pbTestTypeImage.Image = Resources.Written_Test_512;
                        break;
                    }
                case clsTestType.enTestType.StreetTest:
                    {
                        lblTitle.Text = "Street Test Appointment";
                        pbTestTypeImage.Image = Resources.driving_test_512;
                        break;
                    }
            }
            this.Text = lblTitle.Text;
        }
        private void frmListTestAppointments_Load(object sender, EventArgs e)
        {
            _LoadTestTypeImageAndTitle();
            ctrlDrivingLicenseApplicationInfo1.LoadApplicationInfoByApplicationID(_LDLAppID);
            _dtLicenseTestAppointments = 
                clsTestAppointment.GetApplicationTestAppointmentsPersTestType(_LDLAppID, _TestType);
            dgvLicenseTestAppointments.DataSource = _dtLicenseTestAppointments;
            lblRecordsCount.Text = dgvLicenseTestAppointments.Rows.Count.ToString();
            if (dgvLicenseTestAppointments.Rows.Count > 0)
            {
                dgvLicenseTestAppointments.Columns[0].HeaderText = "Appointment ID";
                dgvLicenseTestAppointments.Columns[0].Width = 150;
                dgvLicenseTestAppointments.Columns[1].HeaderText = "Appointment Date";
                dgvLicenseTestAppointments.Columns[1].Width = 200;
                dgvLicenseTestAppointments.Columns[2].HeaderText = "Paid Fees";
                dgvLicenseTestAppointments.Columns[2].Width = 150;
                dgvLicenseTestAppointments.Columns[3].HeaderText = "Is Locked";
                dgvLicenseTestAppointments.Columns[3].Width = 100;
            }
        }
        private void btnAddNewAppointment_Click(object sender, EventArgs e)
        {
            clsLocalDrivingLicenseApplication LDLApp = 
                clsLocalDrivingLicenseApplication.FindByLocalDrivingAppLicenseID(_LDLAppID);
            if (LDLApp.IsThereAnActiveScheduledTest(_TestType))
            {
                MessageBox.Show("Person Already Have An Active Appointment For This Test, You Cannot Add New Appointment",
                    "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // Maybe the person took the test before and passed it.
            clsTest LastTest = LDLApp.GetLastTestPerTestType(_TestType);
            if (LastTest == null)
            {
                Form frm1 = new frmScheduleTest(_LDLAppID, _TestType);
                frm1.ShowDialog();
                frmListTestAppointments_Load(null, null);
                return;
            }
            // if Last Test Not Null And The Person Already Passed The Test, He Can't Even Retake it.
            if (LastTest.TestResult == true)
            {
                MessageBox.Show("This Person Already Passed This Test Before, You Can Only Retake Failed Test",
                    "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // we only make a new test for people who don't have a previous test at all or have a previous test with fail result, Not OTHERWIESE.
            Form frm = new frmScheduleTest(LastTest.TestAppointmentInfo.LocalDrivingLicenseApplicationID,
                _TestType);
            frm.ShowDialog();
            frmListTestAppointments_Load(null, null);
        }
        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmScheduleTest(_LDLAppID,
                _TestType, (int)dgvLicenseTestAppointments.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
            frmListTestAppointments_Load(null, null);
        }
        private void takeTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmTakeTest((int)dgvLicenseTestAppointments.CurrentRow.Cells[0].Value, _TestType);
            frm.ShowDialog();
            frmListTestAppointments_Load(null, null);
        }
    }
}