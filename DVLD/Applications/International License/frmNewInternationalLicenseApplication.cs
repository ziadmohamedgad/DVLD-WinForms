using DVLD.Global_Classes;
using DVLD.Licenses;
using DVLD.Licenses.International_Licenses;
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
namespace DVLD.Applications.International_License
{
    public partial class frmNewInternationalLicenseApplication : Form
    {
        private int _InternationalLicenseID = -1;
        public frmNewInternationalLicenseApplication()
        {
            InitializeComponent();
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void btnIssueLicense_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are You Sure You Want To Issue The License?", "Confirm",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }
            clsInternationalLicense InternationalLicense = new clsInternationalLicense();
            //those are the information for the base application, because they Inherit from application, they are part of the sub class.
            InternationalLicense.ApplicantPersonID = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.DriverInfo.PersonID;
            InternationalLicense.ApplicationDate = DateTime.Now;
            InternationalLicense.ApplicationStatus = clsApplication.enApplicationStatus.Completed;
            InternationalLicense.LastStatusDate = DateTime.Now;
            InternationalLicense.PaidFees = clsApplicationType.Find((int)clsApplication.enApplicationType.NewInternationalLicense).Fees;
            InternationalLicense.CreatedByUserID = clsRegLogger.CurrentUser.UserID;
            InternationalLicense.DriverID = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.DriverID;
            InternationalLicense.IssuedUsingLocalLicenseID = ctrlDriverLicenseInfoWithFilter1.LicenseID;
            InternationalLicense.IssueDate = DateTime.Now;
            InternationalLicense.ExpirationDate = DateTime.Now.AddYears(10);
            InternationalLicense.ApplicationTypeID = clsApplicationType.Find((int)clsApplication.enApplicationType.NewInternationalLicense).ID;
            InternationalLicense.IsActive = true;
            if (!InternationalLicense.Save())
            {
                MessageBox.Show("Faild To Issue International License", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            lblApplicationID.Text = InternationalLicense.ApplicationID.ToString();
            _InternationalLicenseID = InternationalLicense.InternationalLicenseID;
            lblInternationalLicenseID.Text = InternationalLicense.InternationalLicenseID.ToString();
            MessageBox.Show("International License Issued Successfully with ID = " + InternationalLicense.InternationalLicenseID.ToString(),
                "License Issued", MessageBoxButtons.OK, MessageBoxIcon.Information);
            btnIssueLicense.Enabled = false;
            ctrlDriverLicenseInfoWithFilter1.FilterEnabled = false;
            llShowLicenseInfo.Enabled = true;  
        }
        private void llShowLicenseHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form frm = new frmShowPersonLicenseHistory(ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.DriverInfo.PersonID);
            frm.ShowDialog();
        }
        private void llShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form frm = new frmShowInternationalLicenseInfo(_InternationalLicenseID);
            frm.ShowDialog();
        }
        private void _LoadApplicationInfoCtrlDefaultValues()
        {
            lblApplicationDate.Text = clsFormat.DateToShort(DateTime.Now);
            lblIssueDate.Text = clsFormat.DateToShort(DateTime.Now);
            lblFees.Text = clsApplicationType.Find((int)clsApplication.enApplicationType.NewInternationalLicense).Fees.ToString();
            lblExpirationDate.Text =
                clsFormat.DateToShort(DateTime.Now.AddYears(clsLicenseClass.Find(3).DefaultValidityLength));
            lblCreatedByUser.Text = clsRegLogger.CurrentUser.UserName;
        }
        private void frmNewInternationalLicenseApplication_Load(object sender, EventArgs e)
        {
            lblApplicationDate.Text = clsFormat.DateToShort(DateTime.Now);
            lblIssueDate.Text = lblApplicationDate.Text;
            lblExpirationDate.Text = clsFormat.DateToShort(DateTime.Now.AddYears(1));//add one year.
            lblFees.Text = clsApplicationType.Find((int)clsApplication.enApplicationType.NewInternationalLicense).Fees.ToString();
            lblCreatedByUser.Text = clsRegLogger.CurrentUser.UserName;
        }
        private void ctrlDriverLicenseInfoWithFilter1_OnLicenseSelected(int LicenseID)
        {
            int SelectedLicenseID = LicenseID;
            lblLocalLicenseID.Text = SelectedLicenseID.ToString();
            llShowLicenseHistory.Enabled = SelectedLicenseID != -1;
            lblLocalLicenseID.Text = LicenseID.ToString();
            if (SelectedLicenseID == -1) return;
            if (ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.LicenseClassInfo.LicenseClassID != 3)
            {
                MessageBox.Show("Selected License Should Be Class 3, Select Another One.",
                    "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.ExpirationDate < DateTime.Now)
            {
                MessageBox.Show("Selected License Is Expired!",
                    "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.IsActive)
            {
                MessageBox.Show("Selected License Is Not Active!",
                    "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int ActiveInternationalLicenseID = 
                clsInternationalLicense.GetActiveInternationalLicenseIDByDriverID(ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.DriverID); ;
            if (ActiveInternationalLicenseID != -1)
            {
                MessageBox.Show("Person already have an active international license with ID = " + ActiveInternationalLicenseID.ToString(),
                    "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                llShowLicenseInfo.Enabled = true;
                _InternationalLicenseID = ActiveInternationalLicenseID;
                btnIssueLicense.Enabled = false;
                return;
            }
            ctrlDriverLicenseInfoWithFilter1.FilterEnabled = false;
            btnIssueLicense.Enabled = true;
        }
        private void frmNewInternationalLicenseApplication_Activated(object sender, EventArgs e)
        {
            ctrlDriverLicenseInfoWithFilter1.TxtLicenseIDFocus();
        }
    }
}