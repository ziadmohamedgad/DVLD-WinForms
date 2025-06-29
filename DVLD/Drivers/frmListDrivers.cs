using DVLD.Applications.International_License;
using DVLD.Licenses;
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
namespace DVLD.Drivers
{
    public partial class frmListDrivers : Form
    {
        private DataTable _dtAllDrivers;
        public frmListDrivers()
        {
            InitializeComponent();
        }
        private void frmListDrivers_Load(object sender, EventArgs e)
        {
            _dtAllDrivers = clsDriver.GetAllDrivers();
            dgvDrivers.DataSource = _dtAllDrivers;
            lblRecordsCount.Text = dgvDrivers.Rows.Count.ToString();
            cbFilterBy.SelectedIndex = 0;
            if (dgvDrivers.Rows.Count > 0 )
            {
                dgvDrivers.Columns[0].HeaderText = "Driver ID";
                dgvDrivers.Columns[0].Width = 120;
                dgvDrivers.Columns[1].HeaderText = "Person ID";
                dgvDrivers.Columns[1].Width = 120;
                dgvDrivers.Columns[2].HeaderText = "National No.";
                dgvDrivers.Columns[2].Width = 140;
                dgvDrivers.Columns[3].HeaderText = "Full Name";
                dgvDrivers.Columns[3].Width = 320;
                dgvDrivers.Columns[4].HeaderText = "Date";
                dgvDrivers.Columns[4].Width = 170;
                dgvDrivers.Columns[5].HeaderText = "Active Licenses";
                dgvDrivers.Columns[5].Width = 150;
            }
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilterValue.Visible = cbFilterBy.Text != "None";
            txtFilterValue.Clear();
            txtFilterValue.Focus();
        }
        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilterBy.Text == "Person ID" || cbFilterBy.Text == "Driver ID")
                e.Handled = !char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar);
        }
        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            string FilterColumn = "";
            switch(cbFilterBy.Text)
            {
                case "Driver ID":
                    {
                        FilterColumn = "DriverID";
                        break;
                    }
                case "Person ID":
                    {
                        FilterColumn = "PersonID";
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
                default:
                    {
                        FilterColumn = "None";
                        break;
                    }
            }
            if (txtFilterValue.Text.Trim() == "" || FilterColumn == "None")
            {
                _dtAllDrivers.DefaultView.RowFilter = "";
                lblRecordsCount.Text = dgvDrivers.Rows.Count.ToString();
                return;
            }
            if (FilterColumn == "PersonID" || FilterColumn == "DriverID")
                _dtAllDrivers.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterColumn, txtFilterValue.Text.Trim());
            else
                _dtAllDrivers.DefaultView.RowFilter = string.Format("[{0}] Like '{1}%'", FilterColumn, txtFilterValue.Text.Trim());
            lblRecordsCount.Text = dgvDrivers.Rows.Count.ToString();
        }
        private void showPersonInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmShowPersonInfo((int)dgvDrivers.CurrentRow.Cells[1].Value);
            frm.ShowDialog();
            frmListDrivers_Load(null, null);
        }
        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmShowPersonLicenseHistory((int)dgvDrivers.CurrentRow.Cells[1].Value);
            frm.ShowDialog();
            frmListDrivers_Load(null, null);
        }
    }
}