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

namespace DVLD.Licenses.Local_Licenses.Controls
{
    public partial class ctrlDriverLicenseInfoWithFilter : UserControl
    {
        // Define a custom event handler delegate with parameters
        public event Action<int> OnLicenseSelected;
        // Create a protected method to raise the event with a parameter
        protected virtual void LicenseSelected(int LicenseID)
        {
            Action<int> Handler = OnLicenseSelected;
            if (Handler != null)
                Handler(LicenseID); // Raise the event with the parameter
        }
        private int _LicenseID = -1;
        private bool _FilterEnabled = true;
        public bool FilterEnabled
        {
            get
            {
                return _FilterEnabled;
            }
            set
            {
                _FilterEnabled = value;
                gbFilters.Enabled = _FilterEnabled;
            }
        }
        public int LicenseID
        {
            get
            {
                return ctrlDriverLicenseInfo1.LocalLicenseID;
            }
        }
        public clsLicense SelectedLicenseInfo
        { 
            get 
            { 
                return ctrlDriverLicenseInfo1.SelectedLicenseInfo; 
            }
        }
        public ctrlDriverLicenseInfoWithFilter()
        {
            InitializeComponent();
        }
        private void txtLicenseID_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
            if (e.KeyChar == (char)13) // pressed key is Enter
                btnFind.PerformClick();
        }
        private void txtLicenseID_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtLicenseID.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtLicenseID, "This Field Is Required!");
            }
            else
                errorProvider1.SetError(txtLicenseID, null);
        }
        public void TxtLicenseIDFocus()
        {
            txtLicenseID.Focus();
        }
        private void btnFind_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                MessageBox.Show("Some Fileds Are Not Valid!, " +
                    "Put The Mouse Over The Red Icon(s) To See The Erro", 
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtLicenseID.Focus();
                //Here we dont continue becuase the form is not valid
                return;
            }
            _LicenseID = int.Parse(txtLicenseID.Text.Trim());
            LoadLicenseInfo(_LicenseID);
        }
        public void LoadLicenseInfo(int LicenseID)
        {
            txtLicenseID.Text = LicenseID.ToString();
            ctrlDriverLicenseInfo1.LoadInfo(LicenseID);
            _LicenseID = ctrlDriverLicenseInfo1.LocalLicenseID;
            if (OnLicenseSelected != null && FilterEnabled)
                // Raise the event with a parameter
                LicenseSelected(_LicenseID);
        }
    }
}