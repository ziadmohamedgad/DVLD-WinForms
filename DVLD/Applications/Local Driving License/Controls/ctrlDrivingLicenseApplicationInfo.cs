using DVLD.Licenses.Local_Licenses;
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
namespace DVLD.Applications.Local_Driving_License.Controls
{
    public partial class ctrlDrivingLicenseApplicationInfo : UserControl
    {
        private int _LDLAPPID = -1;
        private int _LicenseID;
        private clsLocalDrivingLicenseApplication _LDLAPP;
        public int LocalDrivingLicenseApplicationID
        {
            get
            {
                return _LDLAPPID;
            }
        }
        public ctrlDrivingLicenseApplicationInfo()
        {
            InitializeComponent();
        }
        public void LoadApplicationInfoByLocalDrivingLicenseAppID(int LocalDirvingLicenseApplicationID)
        {
            _LDLAPP = clsLocalDrivingLicenseApplication.FindByLocalDrivingAppLicenseID(LocalDirvingLicenseApplicationID);
            if (_LDLAPP == null)
            {
                ResetLocalDrivingLicenseApplicationInfo();
                MessageBox.Show("No Application With ApplicationID = " + LocalDirvingLicenseApplicationID.ToString(),
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            _FillLocalDrivingLicenseApplicationInfo();
        }
        public void LoadApplicationInfoByApplicationID(int ApplicationID)
        {
            _LDLAPP = clsLocalDrivingLicenseApplication.FindByApplicationID(ApplicationID);
            if (_LDLAPP == null)
            {
                ResetLocalDrivingLicenseApplicationInfo();
                MessageBox.Show("No Application With ApplicationID = " + ApplicationID.ToString(),
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            _FillLocalDrivingLicenseApplicationInfo();
        }
        private void _FillLocalDrivingLicenseApplicationInfo()
        {
            _LicenseID = _LDLAPP.GetActiveLicenseID();
            llShowLicenceInfo.Enabled = _LicenseID != -1;
            lblLocalDrivingLicenseApplicationID.Text = _LDLAPP.LocalDrivingLicenseApplicationID.ToString();
            lblAppliedFor.Text = _LDLAPP.LicenseClassInfo.ClassName;
            lblPassedTests.Text = _LDLAPP.GetPassedTestsCount().ToString() + "/3";
            ctrlApplicationBasicInfo1.LoadApplicationInfo(_LDLAPP.ApplicationID);
        }
        public void ResetLocalDrivingLicenseApplicationInfo()
        {
            ctrlApplicationBasicInfo1.ResetDefaultValues();
            _LDLAPPID = -1;
            llShowLicenceInfo.Enabled = false;
            lblLocalDrivingLicenseApplicationID.Text = "[???]";
            lblAppliedFor.Text = "???";
            lblPassedTests.Text = "0";
        }
        private void llShowLicenceInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form frm = new frmShowLicenseInfo(_LDLAPP.GetActiveLicenseID());
            frm.ShowDialog();
        }
    }
}