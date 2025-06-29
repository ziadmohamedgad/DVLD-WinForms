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

namespace DVLD.Licenses.Local_Licenses
{
    public partial class frmIssueDriverLicenseFirstTime : Form
    {
        private int _LDLAppID;
        private clsLocalDrivingLicenseApplication _LDLApp;
        public frmIssueDriverLicenseFirstTime(int LocalDrivingLicenseApplicationID)
        {
            InitializeComponent();
            _LDLAppID = LocalDrivingLicenseApplicationID;
        }
        private void frmIssueDriverLicenseFirstTime_Load(object sender, EventArgs e)
        {
            txtNotes.Focus();
            _LDLApp = clsLocalDrivingLicenseApplication.FindByLocalDrivingAppLicenseID(_LDLAppID);
            if (_LDLApp == null)
            {
                MessageBox.Show("No Applicaiton With ID = " + _LDLAppID.ToString(),
                    "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }
            if (!_LDLApp.PassedAllTests())
            {
                MessageBox.Show("Person Should Pass All Tests First.",
                    "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }
            if (_LDLApp.IsLicenseIssued())
            {
                MessageBox.Show("Person Already Has License Before With License ID = " +
                    _LDLApp.GetActiveLicenseID(),
                    "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }
            ctrlDrivingLicenseApplicationInfo1.LoadApplicationInfoByLocalDrivingLicenseAppID(_LDLAppID);
        }
        private void btnIssueLicense_Click(object sender, EventArgs e)
        {
            int LicenseID =
                _LDLApp.IssueLicenseForTheFirstTime(txtNotes.Text.Trim(), clsRegLogger.CurrentUser.UserID);
            if (LicenseID != -1)
            {
                MessageBox.Show("License Issued Successfully With License ID = " + LicenseID.ToString(),
                    "Succeeded", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else
                MessageBox.Show("License Was Not Issued !",
                 "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}