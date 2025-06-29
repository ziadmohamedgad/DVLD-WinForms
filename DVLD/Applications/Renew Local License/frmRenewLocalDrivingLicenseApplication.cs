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
namespace DVLD.Applications.Renew_Local_License
{
    public partial class frmRenewLocalDrivingLicenseApplication : Form
    {
        private int _NewLicenseID = -1;
        public frmRenewLocalDrivingLicenseApplication()
        {
            InitializeComponent();
        }
        private void ctrlDriverLicenseInfoWithFilter1_OnLicenseSelected(int LicenseID)
        {
            int SelectedLicenseID = LicenseID;
            lblOldLicenseID.Text = SelectedLicenseID.ToString();
            llShowLicenseHistory.Enabled = SelectedLicenseID != -1;
            if (SelectedLicenseID == -1) return;
            int DefaultValidityLength = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.LicenseClassInfo.DefaultValidityLength;
            lblExpirationDate.Text = clsFormat.DateToShort(DateTime.Now.AddYears(DefaultValidityLength));
            lblLicenseFees.Text = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.LicenseClassInfo.ClassFees.ToString();
            lblTotalFees.Text = (Convert.ToSingle(lblApplicationFees.Text) + Convert.ToSingle(lblLicenseFees.Text)).ToString();
            txtNotes.Text = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.Notes;
            if (!ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.IsActive)
            {
                MessageBox.Show("Selected License Is Not Active, Choose An Active License.",
                    "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnRenewLicense.Enabled = false;
                return;
            }
            if (!ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.IsLicenseExpired())
            {
                MessageBox.Show("Selected License Is Not Yet Expired, It Will Expire On: "
                    + clsFormat.DateToShort(ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.ExpirationDate)
                    , "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnRenewLicense.Enabled = false;
                return;
            }
            else
            {
                // Here We Check If The Driver Renewed His License Before
                int ActiveLicenseID =
                    clsLicense.GetActiveLicenseIDByPersonID(ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.DriverInfo.PersonID,
                    ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.LicenseClassID);
                if (ActiveLicenseID != -1 && ActiveLicenseID != ctrlDriverLicenseInfoWithFilter1.LicenseID) // this mean the driver already renewed the same license before
                {
                    MessageBox.Show("This License Already Renewed Before With License ID = " + ActiveLicenseID,
                        "Not Allowed!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    btnRenewLicense.Enabled = false;
                    ctrlDriverLicenseInfoWithFilter1.FilterEnabled = false; // temporary disabled to avoid calling the On Person Selected Event (See The ctrl Implementation)
                    ctrlDriverLicenseInfoWithFilter1.LoadLicenseInfo(ActiveLicenseID);
                    ctrlDriverLicenseInfoWithFilter1.FilterEnabled = true; // enabled again, we have to let the user search another License
                    llShowLicenseInfo.Enabled = false;
                    return;
                }
            }
            btnRenewLicense.Enabled = true;
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
        private void btnRenewLicense_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are You Sure You Want To Renew The License?", "Confirm", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }
            clsLicense NewLicense =
                ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.RenewLicense(txtNotes.Text.Trim(),
                clsRegLogger.CurrentUser.UserID);
            if (NewLicense == null)
            {
                MessageBox.Show("Failed To Renew The License", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            lblApplicationID.Text = NewLicense.ApplicationID.ToString();
            _NewLicenseID = NewLicense.LicenseID;
            lblRenewedLicenseID.Text = _NewLicenseID.ToString();
            MessageBox.Show("License Renewed Successfully With ID = " + _NewLicenseID.ToString(),
                    "License Issued", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ctrlDriverLicenseInfoWithFilter1.FilterEnabled = false;// we should disable this first to avoid calling the (on person selected event)
            ctrlDriverLicenseInfoWithFilter1.LoadLicenseInfo(_NewLicenseID); // to update the UI with the Renewed License
            btnRenewLicense.Enabled = false;
            llShowLicenseInfo.Enabled = true;
        }
        private void llShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowLicenseInfo frm = new frmShowLicenseInfo(_NewLicenseID);
            frm.ShowDialog();
        }
        private void frmRenewLocalDrivingLicenseApplication_Load(object sender, EventArgs e)
        {
            ctrlDriverLicenseInfoWithFilter1.TxtLicenseIDFocus();
            lblApplicationDate.Text = clsFormat.DateToShort(DateTime.Now);
            lblIssueDate.Text = lblApplicationDate.Text;
            lblExpirationDate.Text = "???";
            lblApplicationFees.Text = clsApplicationType.Find((int)clsApplication.enApplicationType.RenewDrivingLicense).Fees.ToString();
            lblCreatedByUser.Text = clsRegLogger.CurrentUser.UserName;
        }
        private void frmRenewLocalDrivingLicenseApplication_Activated(object sender, EventArgs e)
        {
            ctrlDriverLicenseInfoWithFilter1.TxtLicenseIDFocus();
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}