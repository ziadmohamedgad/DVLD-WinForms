using DVLD_BusinessLayer;
using DVLD_Hash;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace DVLD.Users
{
    public partial class frmChangePassword : Form
    {
        private int _UserID;
        private clsUser _User;
        public frmChangePassword(int UserID)
        {
            InitializeComponent();
            _UserID = UserID;
        }
        private void _ResetDefaultValues()
        {
            txtCurrentPassword.Text = "";
            txtNewPassword.Text = "";
            txtConfirmPassword.Text = "";
            txtCurrentPassword.Focus();
        }
        private void frmChangePassword_Load(object sender, EventArgs e)
        {
            _ResetDefaultValues();
            _User = clsUser.FindByUserID(_UserID);
            if (_User == null)
            {
                MessageBox.Show("Couldn't Find User With ID = " + _UserID, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }
            ctrlUserCard1.LoadUserInfo(_UserID);
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                MessageBox.Show("Some Fields Are Not Valid, Put The Mouse Over The Red Dot(s)",
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string Salt = clsHash.GenerateRandomSalt();
            string HashedPassword = clsHash.ComputeHash(Salt + txtNewPassword.Text.Trim());
            if (clsUser.ChangePassword(_UserID, HashedPassword, Salt))// edit this to store salt
            {
                _User.HashedPassword = HashedPassword;
                MessageBox.Show("Password Changed Successfully.", "Saved", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                _ResetDefaultValues();
            }
            else
            {
                MessageBox.Show("An Error Occured.", "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
        private void txtCurrentPassword_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtCurrentPassword.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtCurrentPassword, "Password Cannot Be Blank!");
                return;
            }
            else
                errorProvider1.SetError(txtCurrentPassword, null);
            if (clsHash.ComputeHash(clsUser.GetPasswordSaltByUserName(_User.UserName) + txtCurrentPassword.Text.Trim()) != _User.HashedPassword)
            {
                e.Cancel = true;
                errorProvider1.SetError(txtCurrentPassword, "Current Password Is Wrong!");
            }
            else
                errorProvider1.SetError(txtCurrentPassword, null);
        }
        private void txtNewPassword_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtNewPassword.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtNewPassword, "New Password Cannot Be Blank!");
                return;
            }
            else
                errorProvider1.SetError(txtNewPassword, null);
            //validating if the user trying to add the same stored password, no need to change
            if (clsHash.ComputeHash(clsUser.GetPasswordSaltByUserName(_User.UserName) + txtNewPassword.Text.Trim()) == _User.HashedPassword)
            {
                e.Cancel = true;
                errorProvider1.SetError(txtNewPassword, "This is your current password already!");
            }
            else
                errorProvider1.SetError(txtNewPassword, null);
        }
        private void txtConfirmPassword_Validating(object sender, CancelEventArgs e)
        {
            if (txtConfirmPassword.Text.Trim() != txtNewPassword.Text.Trim())
            {
                e.Cancel = true;
                errorProvider1.SetError(txtConfirmPassword, "Password Confirmation Does Not Match The New Password!");
            }
            else
                errorProvider1.SetError(txtConfirmPassword, null);
        }
    }
}