using DVLD.Global_Classes;
using DVLD.Properties;
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
namespace DVLD.Tests.Controls
{
    public partial class ctrlScheduledTest : UserControl
    {
        private clsLocalDrivingLicenseApplication _LDLApp;
        private int _TestID = -1;
        private clsTestType.enTestType _TestTypeID;
        private int _TestAppointmentID = -1;
        private int _LDLAppID = -1;
        private clsTestAppointment _TestAppointment;
        public int TestAppointmentID 
        {
            get { return _TestAppointmentID; }
        }
        public int TestID
        {
            get { return _TestID; }
        }
        public clsTestType.enTestType TestTypeID
        {
            get
            {
                return _TestTypeID;
            }
            set 
            { 
                _TestTypeID = value; 
                switch(_TestTypeID)
                {
                    case clsTestType.enTestType.VisionTest:
                        {
                            lblTitle.Text = "Vision Test";
                            pbTestTypeImage.Image = Resources.Vision_512;
                            break;
                        }
                    case clsTestType.enTestType.WrittenTest:
                        {
                            lblTitle.Text = "Vision Test";
                            pbTestTypeImage.Image = Resources.Written_Test_512;
                            break;
                        }
                    case clsTestType.enTestType.StreetTest:
                        {
                            lblTitle.Text = "Street Test";
                            pbTestTypeImage.Image = Resources.driving_test_512;
                            break;
                        }
                }
            }     
        }
        public void LoadInfo(int TestAppointmentID)
        {
            _TestAppointmentID = TestAppointmentID;
            _TestAppointment = clsTestAppointment.Find(_TestAppointmentID);
            if (_TestAppointment == null)
            {
                MessageBox.Show("Error: No Appointment ID = " + _TestAppointmentID.ToString(),
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _TestAppointmentID = -1;
                return;
            }
            _TestID = _TestAppointment.TestID;
            _LDLAppID = _TestAppointment.LocalDrivingLicenseApplicationID;
            _LDLApp = clsLocalDrivingLicenseApplication.FindByLocalDrivingAppLicenseID(_LDLAppID);
            if (_LDLApp == null)
            {
                MessageBox.Show("Error: No Local Driving License Application with ID = " + _LDLAppID.ToString(),
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            lblLocalDrivingLicenseAppID.Text = _LDLApp.LocalDrivingLicenseApplicationID.ToString();
            lblDrivingClass.Text = _LDLApp.LicenseClassInfo.ClassName;
            lblFullName.Text = _LDLApp.PersonFullName;
            lblTrial.Text = _LDLApp.TotalAttemptsPerTest(_TestTypeID).ToString();
            lblDate.Text = clsFormat.DateToShort(_TestAppointment.AppointmentDate);
            lblFees.Text = _TestAppointment.PaidFees.ToString();
            lblTestID.Text = _TestID == -1 ? "Not Taken Yet" : _TestID.ToString();
        }
        public ctrlScheduledTest()
        {
            InitializeComponent();
        }
    }
}