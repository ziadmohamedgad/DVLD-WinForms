using DVLD.Global_Classes;
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

namespace DVLD.Licenses.Detain_License
{
    public partial class frmDetainLicenseApplication : Form
    {
        private int _DetainedLicenseID = -1;
        private int _SelectedLicenseID = -1;
        public frmDetainLicenseApplication()
        {
            InitializeComponent();
        }
        private void llShowLicenseHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form frm = new frmShowPersonLicenseHistory(ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.DriverInfo.PersonID);
            frm.ShowDialog();
            bool FilterEnabled = ctrlDriverLicenseInfoWithFilter1.FilterEnabled;
            ctrlDriverLicenseInfoWithFilter1.FilterEnabled = false;
            ctrlDriverLicenseInfoWithFilter1.LoadLicenseInfo(ctrlDriverLicenseInfoWithFilter1.LicenseID); // or _SelectedLicenseID
            ctrlDriverLicenseInfoWithFilter1.FilterEnabled = FilterEnabled;
        }
        private void llShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form frm = new frmShowLicenseInfo(_SelectedLicenseID);
            frm.ShowDialog();
        }
        private void ctrlDriverLicenseInfoWithFilter1_OnLicenseSelected(int LicenseID)
        {
            _SelectedLicenseID = LicenseID;
            lblLicenseID.Text = _SelectedLicenseID.ToString();
            llShowLicenseHistory.Enabled = _SelectedLicenseID != -1;
            if (_SelectedLicenseID == -1) return;
            if (ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.IsDetained)
            {
                _DetainedLicenseID = LicenseID;
                btnDetain.Enabled = false;
                llShowLicenseInfo.Enabled = true;
                MessageBox.Show("Selected License Already Detained Choose Another One.",
                    "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.IsLicenseExpired())
            {
                MessageBox.Show("Selected License Is Expired!,", 
                    "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnDetain.Enabled = false;
                return;
            }
            llShowLicenseInfo.Enabled = false;
            llShowLicenseHistory.Enabled = true;
            btnDetain.Enabled = true;
            txtFineFees.Focus();
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void btnDetain_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                MessageBox.Show("Some Fields Are Not Valid!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (MessageBox.Show("Are You Sure You Want To Detain This License?", "Confirm",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }
            _DetainedLicenseID =
                ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.Detain(Convert.ToSingle(txtFineFees.Text.Trim()), clsRegLogger.CurrentUser.UserID);
            if (_DetainedLicenseID == -1)
            {
                MessageBox.Show("Failed To Detain The License", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            lblDetainID.Text = _DetainedLicenseID.ToString();
            MessageBox.Show("License Detained Successfully With ID " + _DetainedLicenseID.ToString(),
                "License Issued", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.DeactivateCurrentLicense();
            txtFineFees.Enabled = false;
            btnDetain.Enabled = false;
            llShowLicenseInfo.Enabled = true;
            ctrlDriverLicenseInfoWithFilter1.FilterEnabled = false;// we should disable this first to avoid calling the (on person selected event)
            ctrlDriverLicenseInfoWithFilter1.LoadLicenseInfo(_SelectedLicenseID); // to update the UI with the Renewed License
        }
        private void frmDetainLicenseApplication_Load(object sender, EventArgs e)
        {
            ctrlDriverLicenseInfoWithFilter1.TxtLicenseIDFocus();
            lblDetainDate.Text = clsFormat.DateToShort(DateTime.Now);
            lblCreatedByUser.Text = clsRegLogger.CurrentUser.UserName;
        }
        private void txtFineFees_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtFineFees.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtFineFees, "This Field Cannot Be Empty!");
                return;
            }
            else
                errorProvider1.SetError(txtFineFees, null);
            // Extra handling in case there isn't KeyPress function form txtFees, but there is
            if (!clsValidation.IsNumber(txtFineFees.Text.Trim())) // is number contains integers and floats
            {
                e.Cancel = true;
                errorProvider1.SetError(txtFineFees, "Invalid Number!");
            }
            else
                errorProvider1.SetError(txtFineFees, null);
        }
        private void txtFineFees_KeyPress(object sender, KeyPressEventArgs e)
        {
            // we are handling this to avoid any thing except a  correct (integer/float) value
            if (e.KeyChar == '.' && txtFineFees.Text.Trim().IndexOf('.') > -1 /*There is . already before*/)
            {
                e.Handled = true;
            }
            else
                e.Handled = !char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.';
        }
    }
}