using DVLD.Licenses;
using DVLD.Licenses.International_Licenses;
using DVLD.Licenses.Local_Licenses;
using DVLD.People;
using DVLD_BusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace DVLD.Applications.International_License
{
    public partial class frmListInternationalLicenseApplications : Form
    {
        private DataTable _dtAllInternationalLicensesApplications;
        public frmListInternationalLicenseApplications()
        {
            InitializeComponent();
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void showPersonDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int DriverID = (int)dgvInternationalLicenses.CurrentRow.Cells[2].Value;
            int PersonID = clsDriver.FindByDriverID(DriverID).PersonID;
            Form frm = new frmShowPersonInfo(PersonID);
            frm.ShowDialog();
        }
        private void showLicenseDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = 
                new frmShowInternationalLicenseInfo((int)dgvInternationalLicenses.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
            frmListInternationalLicenseApplications_Load(null, null);
        }
        private void frmListInternationalLicenseApplications_Load(object sender, EventArgs e)
        {
            _dtAllInternationalLicensesApplications = clsInternationalLicense.GetAllInternationalLicenses();
            cbFilterBy.SelectedIndex = 0;
            dgvInternationalLicenses.DataSource = _dtAllInternationalLicensesApplications;
            lblInternationalLicensesRecords.Text = dgvInternationalLicenses.Rows.Count.ToString();
            if (dgvInternationalLicenses.Rows.Count > 0)
            {
                dgvInternationalLicenses.Columns[0].HeaderText = "Int.License ID";
                dgvInternationalLicenses.Columns[0].Width = 160;
                dgvInternationalLicenses.Columns[1].HeaderText = "Application ID";
                dgvInternationalLicenses.Columns[1].Width = 150;
                dgvInternationalLicenses.Columns[2].HeaderText = "Driver ID";
                dgvInternationalLicenses.Columns[2].Width = 130;
                dgvInternationalLicenses.Columns[3].HeaderText = "L.License ID";
                dgvInternationalLicenses.Columns[3].Width = 130;
                dgvInternationalLicenses.Columns[4].HeaderText = "Issue Date";
                dgvInternationalLicenses.Columns[4].Width = 180;
                dgvInternationalLicenses.Columns[5].HeaderText = "Expiration Date";
                dgvInternationalLicenses.Columns[5].Width = 180;
                dgvInternationalLicenses.Columns[6].HeaderText = "Is Active";
                dgvInternationalLicenses.Columns[6].Width = 120;
            }
        }
        private void btnNewApplication_Click(object sender, EventArgs e)
        {
            Form frm = new frmNewInternationalLicenseApplication();
            frm.ShowDialog();
            frmListInternationalLicenseApplications_Load(null, null);
        }
        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbFilterBy.Text == "Is Active")
            {
                txtFilterValue.Visible = false;
                cbIsReleased.Visible = true;
                cbIsReleased.Focus();
                cbIsReleased.SelectedIndex = 0;
            }
            else
            {
                txtFilterValue.Visible = (cbFilterBy.Text != "None");
                cbIsReleased.Visible = false;
                txtFilterValue.Text = "";
                txtFilterValue.Focus();
            }
        }
        private void cbIsReleased_SelectedIndexChanged(object sender, EventArgs e)
        {
            string FilterColumn = "IsActive";
            string FilterValue = cbIsReleased.Text;
            switch(FilterValue)
            {
                case "All":
                    break;
                case "Yes":
                    {
                        FilterValue = "1";
                        break;
                    }
                case "No":
                    {
                        FilterValue = "0";
                        break;
                    }
            }
            if (FilterValue == "All")
                _dtAllInternationalLicensesApplications.DefaultView.RowFilter = "";
            else
                _dtAllInternationalLicensesApplications.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterColumn, FilterValue);
            lblInternationalLicensesRecords.Text = dgvInternationalLicenses.Rows.Count.ToString();
        }
        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            string FilterColumn = "";
            switch(cbFilterBy.Text)
            {
                case "International License ID":
                    {
                        FilterColumn = "InternationalLicenseID";
                        break;
                    }
                case "Application ID":
                    {
                        FilterColumn = "ApplicationID";
                        break;
                    }
                case "Driver ID":
                    {
                        FilterColumn = "DriverID";
                        break;
                    }
                case "Local License ID":
                    {
                        FilterColumn = "IssuedUsingLocalLicenseID";
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
                _dtAllInternationalLicensesApplications.DefaultView.RowFilter = "";
                lblInternationalLicensesRecords.Text = dgvInternationalLicenses.Rows.Count.ToString();
                return;
            }
            _dtAllInternationalLicensesApplications.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterColumn, txtFilterValue.Text.Trim());
            lblInternationalLicensesRecords.Text = dgvInternationalLicenses.Rows.Count.ToString();
        }
        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }
        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int DriverID = (int)dgvInternationalLicenses.CurrentRow.Cells[2].Value;
            int PersonID = clsDriver.FindByDriverID(DriverID).PersonID;
            frmShowPersonLicenseHistory frm = new frmShowPersonLicenseHistory(PersonID);
            frm.ShowDialog();
        }
    }
}