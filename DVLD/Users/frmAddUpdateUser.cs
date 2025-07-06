using DVLD.People.Conrols;
using DVLD_BusinessLayer;
using DVLD_Hash;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Users
{
    public partial class frmAddUpdateUser : Form
    {
        public enum enMode { AddNew = 0, Update = 1 };
        private enMode _Mode;
        private int _UserID = -1;
        private clsUser _User;
        public frmAddUpdateUser()
        {
            InitializeComponent();
            _Mode = enMode.AddNew;
        }
        public frmAddUpdateUser(int UserID)
        {
            InitializeComponent();
            _UserID = UserID;
            _Mode = enMode.Update;
        }
        private void _ResetDefaultValues()
        {
            if (_Mode == enMode.AddNew)
            {
                lblTitle.Text = "Add New User";
                this.Text = "Add New User";
                _User = new clsUser();
                tpLoginInfo.Enabled = false;
                ctrlPersonCardWithFilter1.FilterFocus();
            }
            else
            {
                lblTitle.Text = "Update User";
                this.Text = "Update User";
                tpLoginInfo.Enabled = true;
                btnSave.Enabled = true;
            }
            txtUserName.Text = "";
            txtPassword.Text = "";
            txtConfirmPassword.Text = "";
            chkIsActive.Checked = true;
        }
        private void _LoadData()
        {
            _User = clsUser.FindByUserID(_UserID);
            if (_User == null)
            {
                MessageBox.Show("No User With ID = " + _UserID, "User Not Found",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }
            ctrlPersonCardWithFilter1.FilterEnabled = false;
            lblUserID.Text = _User.UserID.ToString();
            txtUserName.Text = _User.UserName;
            txtPassword.Text = _User.HashedPassword;
            txtConfirmPassword.Text = _User.HashedPassword;
            chkIsActive.Checked = _User.IsActive;
            ctrlPersonCardWithFilter1.LoadPersonInfo(_User.PersonID);
        }
        private void frmAddUpdateUser_Load(object sender, EventArgs e)
        {
            _ResetDefaultValues();
            if (_Mode == enMode.Update)
                _LoadData();
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                MessageBox.Show("Some Fields Are Not Valid!," +
                    " Put The Mouse Over The Red Icon(s) To See The Error!", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            _User.PersonID = ctrlPersonCardWithFilter1.PersonID;
            _User.UserName = txtUserName.Text.Trim();
            _User.IsActive = chkIsActive.Checked;
            string Salt = clsHash.GenerateRandomSalt();
            string HashedPassword = clsHash.ComputeHash(Salt + txtPassword.Text.Trim());
            _User.Salt = Salt;
            _User.HashedPassword = HashedPassword;
            if (_Mode == enMode.Update)
            {
                if (clsUser.ChangePassword(_UserID, HashedPassword, Salt))
                {
                    _User.Salt = Salt;
                    _User.HashedPassword = HashedPassword;
                }
                else
                {
                    MessageBox.Show("Error Saving Data", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            if (_User.Save())
            {
                lblUserID.Text = _User.UserID.ToString();
                _Mode = enMode.Update;
                lblTitle.Text = "Update User";
                this.Text = "Update User";
                MessageBox.Show("Data Saved Successfully", "Saved", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Error Saving Data", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void txtConfirmPassword_Validating(object sender, CancelEventArgs e)
        {
            if (txtConfirmPassword.Text.Trim() != txtPassword.Text.Trim())
            {
                e.Cancel = true;
                errorProvider1.SetError(txtConfirmPassword, "Password Confirmation Does Not Match Password!");
            }
            else
                errorProvider1.SetError(txtConfirmPassword, null);
        }
        private void txtPassword_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtPassword.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtPassword, "Password Cannot Be Blank!");
            }
            else
                errorProvider1.SetError(txtPassword, null);
            //validating if the user trying to add the same stored password, no need to change
            if (_User != null && clsHash.ComputeHash(txtPassword.Text.Trim() + clsUser.GetPasswordSaltByUserName(_User.UserName)) == _User.HashedPassword)
            {
                e.Cancel = true;
                errorProvider1.SetError(txtPassword, "This is your current password already!");
            }
            else
                errorProvider1.SetError(txtPassword, null);
        }
        private void txtUserName_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtUserName.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtUserName, "Username Cannot Be Blank!");
                return;
            }
            else
                errorProvider1.SetError(txtUserName, null);
            if (_Mode == enMode.AddNew)
            {
                if (clsUser.IsUserExist(txtUserName.Text.Trim()))
                {
                    e.Cancel = true;
                    errorProvider1.SetError(txtUserName, "Username Is Used By Another User");
                }
                else
                    errorProvider1.SetError(txtUserName, null);
            }
            else
            {
                if (_User.UserName != txtUserName.Text.Trim() && clsUser.IsUserExist(txtUserName.Text.Trim()))
                {
                    e.Cancel = true;
                    errorProvider1.SetError(txtUserName, "Username Is Userd By Another User");
                }
                else
                    errorProvider1.SetError(txtUserName, null);
            }
        }
        private void btnPersonInfoNext_Click(object sender, EventArgs e)
        {
            if (_Mode == enMode.Update)
            {
                btnSave.Enabled = true;
                tpLoginInfo.Enabled = true;
                tcUserControl.SelectedTab = tcUserControl.TabPages["tpLoginInfo"];
                ctrlPersonCardWithFilter1.FilterEnabled = false;
                return;
            }
            if (ctrlPersonCardWithFilter1.PersonID != -1)
            {
                if (clsUser.IsUserExistForPersonID(ctrlPersonCardWithFilter1.PersonID))
                {
                    MessageBox.Show("Selected Person Already Has A user, Choose Another One.",
                        "Select Another Person", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ctrlPersonCardWithFilter1.FilterFocus();
                }
                else
                {
                    btnSave.Enabled = true;
                    tpLoginInfo.Enabled = true;
                    tcUserControl.SelectedTab = tcUserControl.TabPages["tpLoginInfo"];
                    ctrlPersonCardWithFilter1.FilterEnabled = false;
                }
            }
            else
            {
                MessageBox.Show("Please Select A Person", "Select A Person", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                ctrlPersonCardWithFilter1.FilterFocus();
            }
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void frmAddUpdateUser_Activated(object sender, EventArgs e)
        {
            ctrlPersonCardWithFilter1.FilterFocus();
        }
    }
}