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
using DVLD.Tests;
using DVLD.Licenses.Local_Licenses;
using DVLD.Licenses;
namespace DVLD.Applications.Local_Driving_License
{
    public partial class frmListLocalDrivingLicenseApplications : Form
    {
        private DataTable _dtAllLocalDrivingLicenseApplications;
        public frmListLocalDrivingLicenseApplications()
        {
            InitializeComponent();
        }
        private void frmListLocalDrivingLicenseApplications_Load(object sender, EventArgs e)
        {
            _dtAllLocalDrivingLicenseApplications = clsLocalDrivingLicenseApplication.GetAllLocalDrivingLicenseApplications();
            dgvLocalDrivingLicenseApplication.DataSource = _dtAllLocalDrivingLicenseApplications;
            lblRecordsCount.Text = dgvLocalDrivingLicenseApplication.Rows.Count.ToString();
            cbFilterBy.SelectedIndex = 0;
            if (dgvLocalDrivingLicenseApplication.Rows.Count > 0)
            {
                dgvLocalDrivingLicenseApplication.Columns[0].HeaderText = "L.D.L.AppID";
                dgvLocalDrivingLicenseApplication.Columns[0].Width = 120;
                dgvLocalDrivingLicenseApplication.Columns[1].HeaderText = "Driving Class";
                dgvLocalDrivingLicenseApplication.Columns[1].Width = 300;
                dgvLocalDrivingLicenseApplication.Columns[2].HeaderText = "National No.";
                dgvLocalDrivingLicenseApplication.Columns[2].Width = 150;
                dgvLocalDrivingLicenseApplication.Columns[3].HeaderText = "Full Name";
                dgvLocalDrivingLicenseApplication.Columns[3].Width = 350;
                dgvLocalDrivingLicenseApplication.Columns[4].HeaderText = "Application Date";
                dgvLocalDrivingLicenseApplication.Columns[4].Width = 170;
                dgvLocalDrivingLicenseApplication.Columns[5].HeaderText = "Passed Tests";
                dgvLocalDrivingLicenseApplication.Columns[5].Width = 150;
                dgvLocalDrivingLicenseApplication.Columns[6].HeaderText = "Status";
                dgvLocalDrivingLicenseApplication.Columns[6].Width = 100;
            }
        }
        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilterValue.Visible = cbFilterBy.Text.Trim() != "None";
            if (txtFilterValue.Visible)
            {
                txtFilterValue.Text = "";
                txtFilterValue.Focus();
            }
            _dtAllLocalDrivingLicenseApplications.DefaultView.RowFilter = "";
            lblRecordsCount.Text = dgvLocalDrivingLicenseApplication.Rows.Count.ToString();
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            string FilterColumn = "";
            switch(cbFilterBy.Text.Trim())
            {
                case "L.D.L.AppID":
                    {
                        FilterColumn = "LocalDrivingLicenseApplicationID";
                        break;
                    }
                case "National No.":
                    {
                        FilterColumn = "NationalNo";
                        break;
                    }
                case "Full Name":
                    {
                        FilterColumn = "FullName";
                        break;
                    }
                case "Status":
                    {
                        FilterColumn = "Status";
                        break;
                    }
                default:
                    {
                        FilterColumn = "None";
                        break;
                    }
            }
            if (FilterColumn == "None" || txtFilterValue.Text.Trim() == "")
            {
                _dtAllLocalDrivingLicenseApplications.DefaultView.RowFilter = "";
                lblRecordsCount.Text = dgvLocalDrivingLicenseApplication.Rows.Count.ToString();
                return;
            }
            if (FilterColumn == "LocalDrivingLicenseApplicationID")
                _dtAllLocalDrivingLicenseApplications.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterColumn, txtFilterValue.Text.Trim());
            else
                _dtAllLocalDrivingLicenseApplications.DefaultView.RowFilter = string.Format("[{0}] LIKE '{1}%'", FilterColumn, txtFilterValue.Text.Trim());
            lblRecordsCount.Text = dgvLocalDrivingLicenseApplication.Rows.Count.ToString();
        }
        private void btnAddNewApplication_Click(object sender, EventArgs e)
        {
            Form frm = new frmAddUpdateLocalDrivingLicenseApplication();
            frm.ShowDialog();
            frmListLocalDrivingLicenseApplications_Load(null, null);
        }
        private void editApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmAddUpdateLocalDrivingLicenseApplication((int)dgvLocalDrivingLicenseApplication.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
            frmListLocalDrivingLicenseApplications_Load(null, null);
        }
        private void cmsApplications_Opening(object sender, CancelEventArgs e)
        {
            clsLocalDrivingLicenseApplication LDLApp =
                clsLocalDrivingLicenseApplication.FindByLocalDrivingAppLicenseID((int)dgvLocalDrivingLicenseApplication.CurrentRow.Cells[0].Value);
            int TotalPassedTests = (int)dgvLocalDrivingLicenseApplication.CurrentRow.Cells[5].Value;
            bool LicenseExists = LDLApp.IsLicenseIssued();
            //Enabled only if person passed all tests and Does not have license
            issueDrivingLicenseFirstTimeToolStripMenuItem.Enabled = (TotalPassedTests == 3) && !LicenseExists;
            showLicenseToolStripMenuItem.Enabled = LicenseExists;
            editApplicationToolStripMenuItem.Enabled = !LicenseExists && (LDLApp.ApplicationStatus == clsApplication.enApplicationStatus.New);
            scheduleTestsToolStripMenuItem.Enabled = !LicenseExists;
            // Enable\Disable Cancel Menu Item
            // We only cancel the application with status = new
            cancelApplicationToolStripMenuItem.Enabled = (LDLApp.ApplicationStatus == clsApplication.enApplicationStatus.New);
            //Enable\Disable Delete Menu Item
            //We Only allow delete incase the application status is new not complete or cancelled
            deleteApplicationToolStripMenuItem.Enabled = (LDLApp.ApplicationStatus == clsApplication.enApplicationStatus.New);
            //Enable\Disable Schedule Menu And it's Sub Menu
            bool PassedVisisonTest = LDLApp.DoesPassTestType(clsTestType.enTestType.VisionTest);
            bool PassedWrittenTest = LDLApp.DoesPassTestType(clsTestType.enTestType.WrittenTest);
            bool PassedStreetTest = LDLApp.DoesPassTestType(clsTestType.enTestType.StreetTest);
            scheduleTestsToolStripMenuItem.Enabled = (!PassedVisisonTest || !PassedWrittenTest || !PassedStreetTest) &&
                (LDLApp.ApplicationStatus == clsApplication.enApplicationStatus.New); // my edit && !LicenseExists (and delete the above sch..etc);
            if (scheduleTestsToolStripMenuItem.Enabled)
            {
                ////To Allow Schdule vision test, Person must not passed the same test before.
                schedultVisionTestToolStripMenuItem.Enabled = !PassedVisisonTest;
                ////To Allow Schdule written test, Person must pass the vision test and must not passed the same test before.
                scheduleWrittenTestToolStripMenuItem.Enabled = PassedVisisonTest && !PassedWrittenTest;
                ////To Allow Schdule steet test, Person must pass the vision * written tests, and must not passed the same test before.
                schedultStreetTestToolStripMenuItem.Enabled = PassedVisisonTest && PassedWrittenTest && !PassedStreetTest;
            }
        }
        private void cancelApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are You Sure You Want To Cancel This Application?", "Confrim",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) 
                return;
            clsLocalDrivingLicenseApplication LDLApp =
                clsLocalDrivingLicenseApplication.FindByLocalDrivingAppLicenseID((int)dgvLocalDrivingLicenseApplication.CurrentRow.Cells[0].Value);
            if (LDLApp != null)
            {
                if (LDLApp.Cancel())
                {
                    MessageBox.Show("Application Cancelled Successfully.", "Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    frmListLocalDrivingLicenseApplications_Load(null, null);
                }
                else
                    MessageBox.Show("Couldn't Cancel Application.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void showApplicationDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmShowLocalDrivingLicenseApplicationInfo((int)dgvLocalDrivingLicenseApplication.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
            frmListLocalDrivingLicenseApplications_Load(null, null);
        }
        private void deleteApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are You Sure You Want To Delete This Application?", "Confrim",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;
            clsLocalDrivingLicenseApplication LDLAPP =
                clsLocalDrivingLicenseApplication.FindByLocalDrivingAppLicenseID((int)dgvLocalDrivingLicenseApplication.CurrentRow.Cells[0].Value);
            if (LDLAPP != null)
            {
                if (LDLAPP.Delete())
                {
                    MessageBox.Show("Application Deleted Successfully.", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    frmListLocalDrivingLicenseApplications_Load(null, null);
                }
                else
                    MessageBox.Show("Could not delete applicatoin, other data depends on it.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show("Null");
            }
        }
        private void _ScheduleTest(clsTestType.enTestType TestType)
        {
            Form frm = new frmListTestAppointments((int)dgvLocalDrivingLicenseApplication.CurrentRow.Cells[0].Value, TestType);
            frm.ShowDialog();
            frmListLocalDrivingLicenseApplications_Load(null, null);
        }
        private void schedultVisionTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _ScheduleTest(clsTestType.enTestType.VisionTest);
        }
        private void scheduleWrittenTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _ScheduleTest(clsTestType.enTestType.WrittenTest);
        }
        private void schedultStreetTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _ScheduleTest(clsTestType.enTestType.StreetTest);
        }
        private void issueDrivingLicenseFirstTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmIssueDriverLicenseFirstTime((int)dgvLocalDrivingLicenseApplication.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
        }
        private void showLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clsLocalDrivingLicenseApplication LDLApp = 
                clsLocalDrivingLicenseApplication.FindByLocalDrivingAppLicenseID((int)dgvLocalDrivingLicenseApplication.CurrentRow.Cells[0].Value);
            int PersonID = LDLApp.ApplicantPersonID;
            int LicenseClassID = LDLApp.LicenseClassID;
            int LicenseID = clsLicense.GetActiveLicenseIDByPersonID(PersonID, LicenseClassID);
            Form frm = new frmShowLicenseInfo(LicenseID);
            frm.ShowDialog();
        }
        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clsLocalDrivingLicenseApplication LDLApp =
                clsLocalDrivingLicenseApplication.FindByLocalDrivingAppLicenseID((int)dgvLocalDrivingLicenseApplication.CurrentRow.Cells[0].Value);
            int PersonID = LDLApp.ApplicantPersonID;
            Form frm = new frmShowPersonLicenseHistory(PersonID);
            frm.ShowDialog();
            frmListLocalDrivingLicenseApplications_Load(null, null);
        }
    }
}