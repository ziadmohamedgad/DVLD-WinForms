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
namespace DVLD.Applications.Replace_Lost_or_Damaged_License
{
    public partial class frmReplaceLostOrDamagedLicenseApplication : Form
    {
        private int _NewLicenseID = -1;
        public frmReplaceLostOrDamagedLicenseApplication()
        {
            InitializeComponent();
        }
        private void ctrlDriverLicenseInfoWithFilter1_OnLicenseSelected(int LicenseID)
        {
            int SelectedLicenseID = LicenseID;
            lblOldLicenseID.Text = SelectedLicenseID.ToString();
            llShowLicenseHistory.Enabled = SelectedLicenseID != -1;
            if (SelectedLicenseID == -1) return;
            if (!ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.IsActive)
            {
                MessageBox.Show("Selected License Is Not Active, Choose An Active License.",
                    "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnIssueReplacement.Enabled = false;
                return;
            }
            if (ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.IsLicenseExpired())
            {
                MessageBox.Show("Your Lost/Damaged License Is Already Expired, So You Have To Renew It",
                    "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnIssueReplacement.Enabled = false;
                return;
            }
            btnIssueReplacement.Enabled = true;
        }
        private void frmReplaceLostOrDamagedLicenseApplication_Load(object sender, EventArgs e)
        {
            lblApplicationDate.Text = clsFormat.DateToShort(DateTime.Now);
            lblCreatedByUser.Text = clsRegLogger.CurrentUser.UserName;
            rbDamagedLicense.PerformClick();
        }
        private void btnIssueReplacement_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are You Sure You Want To Issue A Replacement For The License?", "Confirm",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }
            clsLicense.enIssueReason IssueReason =
                rbDamagedLicense.Checked ? clsLicense.enIssueReason.ReplacementForDamaged : clsLicense.enIssueReason.ReplacementForLost;
            clsLicense NewLicense = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.Replace(IssueReason, clsRegLogger.CurrentUser.UserID);
            if (NewLicense == null)
            {
                MessageBox.Show("Failed To Replace The License", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            lblApplicationID.Text = NewLicense.ApplicationID.ToString();
            _NewLicenseID = NewLicense.LicenseID;
            lblRreplacedLicenseID.Text = _NewLicenseID.ToString();
            MessageBox.Show("License Replaced Successfully With ID = " + _NewLicenseID.ToString(),
                "License Issued", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ctrlDriverLicenseInfoWithFilter1.FilterEnabled = false;// we should disable this first to avoid calling the (on person selected event)
            ctrlDriverLicenseInfoWithFilter1.LoadLicenseInfo(_NewLicenseID); // to update the UI with the Renewed License
            btnIssueReplacement.Enabled = false;
            gbReplacementFor.Enabled = false;
            llShowLicenseInfo.Enabled = true;
        }
        private void llShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowLicenseInfo frm = new frmShowLicenseInfo(_NewLicenseID);
            frm.ShowDialog();
        }
        private void llShowLicenseHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowPersonLicenseHistory frm =
                new frmShowPersonLicenseHistory(ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.DriverInfo.PersonID);
            frm.ShowDialog();
            bool FilterEnabled = ctrlDriverLicenseInfoWithFilter1.FilterEnabled;
            ctrlDriverLicenseInfoWithFilter1.FilterEnabled = false;
            ctrlDriverLicenseInfoWithFilter1.LoadLicenseInfo(ctrlDriverLicenseInfoWithFilter1.LicenseID); // just to refresh if the user changed the driver personal Data
            ctrlDriverLicenseInfoWithFilter1.FilterEnabled = FilterEnabled; // same as before
        }
        private void rbDamagedLicense_CheckedChanged(object sender, EventArgs e)
        {
            lblApplicationFees.Text = clsApplicationType.Find((int)clsApplication.enApplicationType.ReplaceDamagedDrivingLicense).Fees.ToString();
            this.Text = "Replacement For Lost License";
            lblTitle.Text = this.Text;
        }
        private void rbLostLicense_CheckedChanged(object sender, EventArgs e)
        {
            lblApplicationFees.Text = clsApplicationType.Find((int)clsApplication.enApplicationType.ReplaceLostDrivingLicense).Fees.ToString();
            this.Text = "Replacement For Damaged License";
            lblTitle.Text = this.Text;
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}