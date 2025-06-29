using DVLD.Global_Classes;
using DVLD.Licenses;
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
namespace DVLD.Applications.Release_Detained_License
{
    public partial class frmReleaseDetainedLicenseApplication : Form
    {
        private int _SelectedLicenseID = -1;
        public frmReleaseDetainedLicenseApplication()
        {
            InitializeComponent();
        }
        public frmReleaseDetainedLicenseApplication(int LicenseID)
        {
            InitializeComponent();
            _SelectedLicenseID = LicenseID;
            ctrlDriverLicenseInfoWithFilter1.LoadLicenseInfo(_SelectedLicenseID);
            ctrlDriverLicenseInfoWithFilter1.FilterEnabled = false;
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void btnRelease_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are You Sure You Want To Release This Detained License?", 
                "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }
            int ReleaseApplicationID = -1;
            bool IsReleased = 
                ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.ReleaseDetainedLicense(clsRegLogger.CurrentUser.UserID, ref ReleaseApplicationID);
            if (!IsReleased)
            {
                MessageBox.Show("Failed To Release The Detained License.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.IsActive = true;
            ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.Save(); // to set it to active
            MessageBox.Show("Detained License Released Successfully.", "License Released Successfully",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            lblApplicationID.Text = ReleaseApplicationID.ToString();
            llShowLicenseInfo.Enabled = true;
            ctrlDriverLicenseInfoWithFilter1.FilterEnabled = false;
            ctrlDriverLicenseInfoWithFilter1.LoadLicenseInfo(ctrlDriverLicenseInfoWithFilter1.LicenseID);
            btnRelease.Enabled = false;
        }
        private void llShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowLicenseInfo frm = new frmShowLicenseInfo(_SelectedLicenseID);
            frm.ShowDialog();
        }
        private void ctrlDriverLicenseInfoWithFilter1_OnLicenseSelected(int LicenseID)
        {
            _SelectedLicenseID = LicenseID;
            llShowLicenseHistory.Enabled = _SelectedLicenseID != -1;
            if (_SelectedLicenseID == -1) return;
            if (!ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.IsDetained)
            {
                MessageBox.Show("Selected License Is Not Detained, Choose Another One.",
                    "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.IsLicenseExpired())
            {
                MessageBox.Show("Selected License Is Expired!,",
                    "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.IsActive)
            {
                MessageBox.Show("Selected License Is Not Active!,",
                    "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            lblLicenseID.Text = _SelectedLicenseID.ToString();
            lblApplicationFees.Text = 
                clsApplicationType.Find((int)clsApplication.enApplicationType.ReleaseDetainedDrivingLicenses).Fees.ToString();
            lblCreatedByUser.Text = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.DetainedInfo.CreatedByUserInfo.UserName.ToString();
            lblDetainID.Text = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.DetainedInfo.DetainID.ToString();
            lblDetainDate.Text = clsFormat.DateToShort(ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.DetainedInfo.DetainDate);
            lblFineFees.Text = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.DetainedInfo.FineFees.ToString();
            lblTotalFees.Text = (Convert.ToSingle(lblFineFees.Text.Trim()) + Convert.ToSingle(lblApplicationFees.Text.Trim())).ToString();
            btnRelease.Enabled = true;
        }
        private void llShowLicenseHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form frm = new frmShowPersonLicenseHistory(ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.DriverInfo.PersonID);
            frm.ShowDialog();
            bool FilterValue = ctrlDriverLicenseInfoWithFilter1.FilterEnabled;
            ctrlDriverLicenseInfoWithFilter1.FilterEnabled = false;
            ctrlDriverLicenseInfoWithFilter1.LoadLicenseInfo(_SelectedLicenseID);
            ctrlDriverLicenseInfoWithFilter1.FilterEnabled = FilterValue;
        }
        private void frmReleaseDetainedLicenseApplication_Activated(object sender, EventArgs e)
        {
            ctrlDriverLicenseInfoWithFilter1.TxtLicenseIDFocus();
        }
    }
}