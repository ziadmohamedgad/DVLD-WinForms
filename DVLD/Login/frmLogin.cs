using DVLD.Global_Classes;
using DVLD_BusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DVLD_Hash;
namespace DVLD.Login
{
    public partial class frmLogin : Form
    {
        private bool IsPasswordHashed = false;
        public frmLogin()
        {
            InitializeComponent();
        }
        private void btnLogin_Click(object sender, EventArgs e)
        {
            clsUser User = new clsUser();
            string HashedPassword = null;
            if (IsPasswordHashed)
            {
                HashedPassword = txtPassword.Text.Trim();
                User = clsUser.FindByUsernameAndHashedPassword(txtUserName.Text.Trim(), HashedPassword);
            }
            else
            {
                string Salt = clsUser.GetPasswordSaltByUserName(txtUserName.Text.Trim());
                HashedPassword = clsHash.ComputeHash(Salt + txtPassword.Text.Trim());
                User = clsUser.FindByUsernameAndHashedPassword(txtUserName.Text.Trim(), HashedPassword);
            }
            if (User != null)
            {
                if (chkRemeberMe.Checked)
                {
                    clsRegLogger.RememberUsernameAndPassword(txtUserName.Text.Trim(), HashedPassword);
                }
                else
                {
                    clsRegLogger.DeleteSavedCredentials();
                    txtUserName.Clear();
                    txtPassword.Clear();
                }
                if (!User.IsActive)
                {
                    txtUserName.Focus();
                    MessageBox.Show("Your Accound Is Not Active, Contact Admin.", "In Active Account",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                clsRegLogger.CurrentUser = User;
                this.Hide();
                Form frmMain = new frmMainScreen(this);
                frmMain.ShowDialog();
                IsPasswordHashed = false;
            }
            else
            {
                txtPassword.Focus();
                MessageBox.Show("Invalid Username/Password.", "Wrong Credentials", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void frmLogin_Load(object sender, EventArgs e)
        {
            string Username = "", HashedPassword = "";
            if (clsRegLogger.GetStoredCredentilas(ref Username, ref HashedPassword))
            {
                txtUserName.Text = Username;
                txtPassword.Text = HashedPassword;
                chkRemeberMe.Checked = true;
                IsPasswordHashed = true;
            }
            else
                chkRemeberMe.Checked = false;
        }
        private void txtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            IsPasswordHashed = false; //user decided to insert a new password.
        }
    }
}