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

namespace DVLD.Login
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }
        private void btnLogin_Click(object sender, EventArgs e)
        {
            clsUser User = clsUser.FindByUsernameAndPassword(txtUserName.Text.Trim(), txtPassword.Text.Trim());
            if (User != null)
            {
                if (chkRemeberMe.Checked)
                {
                    clsRegLogger.RememberUsernameAndPassword(txtUserName.Text.Trim(), txtPassword.Text.Trim());
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
            }
            else
            {
                txtUserName.Focus();
                MessageBox.Show("Invalid Username/Password.", "Wrong Credentials", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void frmLogin_Load(object sender, EventArgs e)
        {
            string Username = "", Password = "";
            if (clsRegLogger.GetStoredCredentilas(ref Username, ref Password))
            {
                txtUserName.Text = Username;
                txtPassword.Text = Password;
                chkRemeberMe.Checked = true;
            }
            else
                chkRemeberMe.Checked = false;
        }
    }
}
