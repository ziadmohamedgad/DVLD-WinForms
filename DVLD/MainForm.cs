using DVLD.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DVLD.Global_Classes;
using DVLD.Login;
using DVLD.Tests;
using DVLD.Applications.Application_Types;
using DVLD.Applications.Local_Driving_License;
using DVLD.Applications.International_License;
using DVLD.Applications.Renew_Local_License;
using DVLD_BusinessLayer;
using DVLD.Applications.Replace_Lost_or_Damaged_License;
using DVLD.Drivers;
using DVLD.Licenses.Detain_License;
using DVLD.Applications.Release_Detained_License;
namespace DVLD
{
    public partial class frmMainScreen : Form
    {
        private frmLogin _frmLogin;
        public frmMainScreen(frmLogin frm)
        {
            InitializeComponent();
            _frmLogin = frm;
        }
        private void peopleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmListPeople();
            frm.ShowDialog();
        }
        private void usersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmListUsers();
            frm.ShowDialog();
        }
        private void currentUserInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmShowUserInfo(clsRegLogger.CurrentUser.UserID);
            frm.ShowDialog();
        }
        private void signOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clsRegLogger.CurrentUser = null;
            _frmLogin.Show();
            this.Close();
        }
        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmChangePassword(clsRegLogger.CurrentUser.UserID);
            frm.ShowDialog();
        }
        private void frmMainScreen_FormClosing(object sender, FormClosingEventArgs e)
        {
            clsRegLogger.CurrentUser = null;
            _frmLogin.Show();
        }
        private void manageApplicationTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmListApplicationTypes();
            frm.ShowDialog();
        }
        private void manageTestTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmListTestTypes();
            frm.ShowDialog();
        }
        private void localDrivingLicenseApplicationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmListLocalDrivingLicenseApplications();
            frm.ShowDialog();
        }
        private void localLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmAddUpdateLocalDrivingLicenseApplication();
            frm.ShowDialog();
        }
        private void retakeTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmListLocalDrivingLicenseApplications();
            frm.ShowDialog();
        }
        private void internationalLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmNewInternationalLicenseApplication();
            frm.ShowDialog();
        }
        private void internationalLicenseApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmListInternationalLicenseApplications();
            frm.ShowDialog();
        }
        private void renewDrivingLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmRenewLocalDrivingLicenseApplication();
            frm.ShowDialog();
        }
        private void replacementForLostOrDamagedLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmReplaceLostOrDamagedLicenseApplication();
            frm.ShowDialog();
        }
        private void driversToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmListDrivers();
            frm.ShowDialog();
        }
        private void detainLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmDetainLicenseApplication();
            frm.ShowDialog();
        }
        private void releaseDetainedLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmReleaseDetainedLicenseApplication();
            frm.ShowDialog();
        }
        private void manageDetainedLicensesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmListDetainedLicenses();
            frm.ShowDialog();
        }
        private void releaseDetainedDrivingLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmReleaseDetainedLicenseApplication();
            frm.ShowDialog();
        }
    }
}