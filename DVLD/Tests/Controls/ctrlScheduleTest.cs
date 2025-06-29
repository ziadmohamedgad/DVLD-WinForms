using DVLD.Global_Classes;
using DVLD.Properties;
using DVLD_BusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace DVLD.Tests.Controls
{
    public partial class ctrlScheduleTest : UserControl
    {
        // imagine this as clsTestAppointment Business Layer Merged with clsLocalDrivingApplication Business Layer & clsApplication Business Layer
        public enum enMode { AddNew = 0, Update = 1 };
        private enMode _Mode = enMode.AddNew;
        public enum enCreationMode { FirstTimeSchedule = 0, RetakeTestSchedule = 1 };
        private enCreationMode _CreationMode = enCreationMode.FirstTimeSchedule;
        private clsTestType.enTestType _TestTypeID = clsTestType.enTestType.VisionTest;
        private clsLocalDrivingLicenseApplication _LDLApp;
        private clsTestAppointment _TestAppointment;
        private int _LDLAppID = -1;
        private int _TestAppointmentID = -1;
        public clsTestType.enTestType TestTypeID
        {
            get
            {
                return _TestTypeID;
            }
            set
            {
                _TestTypeID = value;
                switch (_TestTypeID)
                {
                    case clsTestType.enTestType.VisionTest:
                        {
                            gbTestType.Text = "Vision Test";
                            pbTestTypeImage.Image = Resources.Vision_512;
                            break;
                        }
                    case clsTestType.enTestType.WrittenTest:
                        {
                            gbTestType.Text = "Written Test";
                            pbTestTypeImage.Image = Resources.Written_Test_512;
                            break;
                        }
                    case clsTestType.enTestType.StreetTest:
                        {
                            gbTestType.Text = "Street Test";
                            pbTestTypeImage.Image = Resources.driving_test_512;
                            break;
                        }
                }
            }
        }
        public ctrlScheduleTest()
        {
            InitializeComponent();
        }
        public void LoadInfo(int LocalDrivingLicenseApplicationID, int AppointmentID = -1)
        {
            if (AppointmentID == -1)
                _Mode = enMode.AddNew;
            else
                _Mode = enMode.Update;
            _LDLAppID = LocalDrivingLicenseApplicationID;
            _TestAppointmentID = AppointmentID;
            _LDLApp = clsLocalDrivingLicenseApplication.FindByLocalDrivingAppLicenseID(LocalDrivingLicenseApplicationID);
            if (_LDLApp == null)
            {
                MessageBox.Show("Error: No Local Driving License Application With ID = " + LocalDrivingLicenseApplicationID.ToString(),
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnSave.Enabled = false;
                dtpTestDate.Enabled = false;
                return;
            }
            lblLocalDrivingLicenseAppID.Text = _LDLApp.LocalDrivingLicenseApplicationID.ToString();
            lblDrivingClass.Text = _LDLApp.LicenseClassInfo.ClassName;
            lblFullName.Text = _LDLApp.PersonFullName;
            // this will show the attempts for this test before
            lblTrial.Text = _LDLApp.TotalAttemptsPerTest(_TestTypeID).ToString();
            // decide if the creation mode is retake test or not, based on (if the person attended the test before)
            if (_LDLApp.DoesAttendTestType(_TestTypeID))
            {
                _CreationMode = enCreationMode.RetakeTestSchedule;
                lblTitle.Text = "Schedule Retake Test";
                gbRetakeTestInfo.Enabled = true;
                lblRetakeAppFees.Text = clsApplicationType.Find((int)clsApplication.enApplicationType.RetakeTest).Fees.ToString();
                lblRetakeTestAppID.Text = "N/A";
            }
            else
            {
                _CreationMode = enCreationMode.FirstTimeSchedule;
                lblTitle.Text = "Schedule Test";
                gbRetakeTestInfo.Enabled = false;
                lblRetakeAppFees.Text = "0";
                lblRetakeTestAppID.Text = "N/A";
            }
            if (_Mode == enMode.AddNew) // there is no previous appointment
            {
                lblFees.Text = clsTestType.Find(TestTypeID).Fees.ToString();
                dtpTestDate.MinDate = DateTime.Now;
                lblRetakeTestAppID.Text = "N/A";
                _TestAppointment = new clsTestAppointment();
            }
            else
            {
                if (!_LoadTestAppointmentData())
                    return;
            }
            lblTotalFees.Text = Convert.ToSingle(lblFees.Text) + Convert.ToSingle(lblRetakeAppFees).ToString();
            if (!_HandleActiveTestAppointmentConstraint())
                return;
            if (_HandleAppointmentLockedConstraint())
                return;
            if (!_HandlePreviousTestConstraint())
                return;
        }
        private bool _LoadTestAppointmentData()
        {
            _TestAppointment = clsTestAppointment.Find(_TestAppointmentID);
            if (_TestAppointment == null)
            {
                MessageBox.Show("Error: No Appointment With ID = " + _TestAppointmentID.ToString(),
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnSave.Enabled = false;
                return false;
            }
            lblFees.Text = _TestAppointment.PaidFees.ToString();
            // we compare the dates to avoid schedule a past date
            if (DateTime.Compare(DateTime.Now, _TestAppointment.AppointmentDate) < 0)
                dtpTestDate.MinDate = DateTime.Now; // avoiding to schedule a past date
            else
                dtpTestDate.MinDate = _TestAppointment.AppointmentDate; // to avoid the user to schedule earlier day even it's not came yet       
            dtpTestDate.Value = dtpTestDate.MinDate;
            if (_TestAppointment.RetakeTestApplicationID == -1) // what if the appointment itself was a retake not first time
            {
                // its an appointment but still for the first time (we handled it already above at Load but this for extra handling).
                lblTitle.Text = "Schedule Test";
                gbRetakeTestInfo.Enabled = false;
                lblRetakeAppFees.Text = "0";
                lblRetakeTestAppID.Text = "N/A";
            }
            else
            {
                lblTitle.Text = "Schedule Retake Test";
                gbRetakeTestInfo.Enabled = true;
                lblRetakeAppFees.Text = _TestAppointment.RetakeTestApplicationInfo.PaidFees.ToString();
                lblRetakeTestAppID.Text = _TestAppointment.RetakeTestApplicationID.ToString();
            }
            return true;
        }
        private bool _HandleActiveTestAppointmentConstraint()
        {
            if (_Mode == enMode.AddNew &&
                clsLocalDrivingLicenseApplication.IsThereAnActiveScheduledTest(_LDLAppID, _TestTypeID))
            {
                lblUserMessage.Visible = true;
                lblUserMessage.Text = "Person Already Have An Active Appointment For This Test";
                btnSave.Enabled = false;
                dtpTestDate.Enabled = false;
                return false;
            }
            else
                lblUserMessage.Visible = false;
            return true;
        }
        private bool _HandleAppointmentLockedConstraint()
        {
            // if appointment is locked that means the person already took this test
            // we cannot update locked appointment
            if (_TestAppointment.IsLocked)
            {
                lblUserMessage.Visible = true;
                lblUserMessage.Text = "Person Already Sat For The Test, Appointment Locked.";
                dtpTestDate.Enabled = false;
                btnSave.Enabled = false;
                return false;
            }
            else
                lblUserMessage.Visible = false;
            return true;
        }
        private bool _HandlePreviousTestConstraint()
        {
            //we need to make sure that this person passed the previous required test before apply to the new test.
            switch (TestTypeID)
            {
                case clsTestType.enTestType.VisionTest:
                    {
                        //in this case no required previous test to pass.
                        lblUserMessage.Visible = false;
                        return true;
                    }
                case clsTestType.enTestType.WrittenTest:
                    {
                        //Written test, person cannot apply for written test unless he passes the vision test.
                        //we check if he passed the vision test
                        if (!_LDLApp.DoesPassTestType(clsTestType.enTestType.VisionTest))
                        {
                            lblUserMessage.Text = "Cannot Schedule, Vision Test Should Be Passed First";
                            lblUserMessage.Visible = true;
                            btnSave.Enabled = false;
                            dtpTestDate.Enabled = false;
                            return false;
                        }
                        else
                        {
                            lblUserMessage.Visible = false;
                            btnSave.Enabled = true;
                            dtpTestDate.Enabled = true;
                        }
                        return true;
                    }
                case clsTestType.enTestType.StreetTest:
                    {
                        //Street Test, you cannot sechdule it before person passes the written test.
                        //we check if he passed the written test
                        if (!_LDLApp.DoesPassTestType(clsTestType.enTestType.WrittenTest))
                        {
                            lblUserMessage.Text = "Cannot Schedule, Written Test Should Be Passed First";
                            lblUserMessage.Visible = true;
                            btnSave.Enabled = false;
                            dtpTestDate.Enabled = false;
                            return false;
                        }
                        else
                        {
                            lblUserMessage.Visible = false;
                            btnSave.Enabled = true;
                            dtpTestDate.Enabled = true;
                        }
                        return true;
                    }
            }
            return true;
        }
        private bool _HandleRetakeApplication()
        {
            //this will decide to create a separate application for retake test or not.
            //and will create it if needed, then it will link it to the appointment.
            if (_Mode == enMode.AddNew && _CreationMode == enCreationMode.RetakeTestSchedule)
            {
                // incase the mode is add new and creation mode is retake test we should create a separate application for it.
                // then we link it with the appointment
                clsApplication Application = new clsApplication();
                Application.ApplicantPersonID = _LDLApp.ApplicantPersonID;
                Application.ApplicationDate = DateTime.Now;
                Application.ApplicationTypeID = (int)clsApplication.enApplicationType.RetakeTest;
                Application.ApplicationStatus = clsApplication.enApplicationStatus.Completed;
                Application.LastStatusDate = DateTime.Now;
                Application.PaidFees = clsApplicationType.Find((int)clsApplication.enApplicationType.RetakeTest).Fees;
                Application.CreatedByUserID = clsRegLogger.CurrentUser.UserID;
                if (!Application.Save())
                {
                    _TestAppointment.RetakeTestApplicationID = -1;
                    MessageBox.Show("Failed To Create Application", "Failed", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return false;
                }
                _TestAppointment.RetakeTestApplicationID = Application.ApplicationID;
            }
            return true;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!_HandleRetakeApplication())
                return;
            _TestAppointment.TestTypeID = _TestTypeID;
            _TestAppointment.LocalDrivingLicenseApplicationID = _LDLApp.LocalDrivingLicenseApplicationID;
            _TestAppointment.AppointmentDate = dtpTestDate.Value;
            _TestAppointment.PaidFees = Convert.ToSingle(lblFees.Text);
            _TestAppointment.CreatedByUserID = clsRegLogger.CurrentUser.UserID;
            if (_TestAppointment.Save())
            {
                dtpTestDate.Enabled = false;
                btnSave.Enabled = false;
                _Mode = enMode.Update;
                MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                MessageBox.Show("Error: Data Is Not Saved Successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}