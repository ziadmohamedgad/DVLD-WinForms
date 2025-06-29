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
using System.Xml.Linq;
namespace DVLD.Users
{
    public partial class frmListUsers : Form
    {
        private static DataTable _dtAllUsers;
        public frmListUsers()
        {
            InitializeComponent();
        }
        private void _RefreshUsersList()
        {
            _dtAllUsers = clsUser.GetAllUsers();
            dgvUsers.DataSource = _dtAllUsers;
            lblRecordsCount.Text = dgvUsers.Rows.Count.ToString();
        }
        private void frmListUsers_Load(object sender, EventArgs e)
        {
             _dtAllUsers = clsUser.GetAllUsers();
            dgvUsers.DataSource = _dtAllUsers;
            cbFilterBy.SelectedIndex = 0;
            lblRecordsCount.Text = dgvUsers.Rows.Count.ToString();
            if (dgvUsers.Rows.Count > 0)
            {
                dgvUsers.Columns[0].HeaderText = "User ID";
                dgvUsers.Columns[0].Width = 110;
                dgvUsers.Columns[1].HeaderText = "Person ID";
                dgvUsers.Columns[1].Width = 120;
                dgvUsers.Columns[2].HeaderText = "Full Name";
                dgvUsers.Columns[2].Width = 350;
                dgvUsers.Columns[3].HeaderText = "UserName";
                dgvUsers.Columns[3].Width = 120;
                dgvUsers.Columns[4].HeaderText = "Is Active";
                dgvUsers.Columns[4].Width = 120;
            }
        }
        private void dgvUsers_DoubleClick(object sender, EventArgs e)
        {
            Form frm = new frmShowUserInfo((int)dgvUsers.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
        }
        private void btnAddUser_Click(object sender, EventArgs e)
        {
            Form frm = new frmAddUpdateUser();
            frm.ShowDialog();
            _RefreshUsersList(); // WE CAN CALL frmListUsers_Load(null, null); INSTEAD
        }
        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmAddUpdateUser((int)dgvUsers.CurrentRow.Cells[1].Value);
            frm.ShowDialog();
            _RefreshUsersList();
        }
        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            string FilterColumn = "";
            switch(cbFilterBy.Text)
            {
                case "User ID":
                    {
                        FilterColumn = "UserID";
                        break;
                    }
                case "User Name":
                    {
                        FilterColumn = "UserName";
                        break;
                    }
                case "Person ID":
                    {
                        FilterColumn = "PersonID";
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
            if (FilterColumn == "None" || txtFilterValue.Text.Trim() == "")
            {
                _dtAllUsers.DefaultView.RowFilter = "";
                lblRecordsCount.Text = dgvUsers.Rows.Count.ToString();
                return;
            }
            if (FilterColumn == "PersonID" || FilterColumn == "UserID")
            {
                _dtAllUsers.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterColumn, txtFilterValue.Text.Trim());
            }
            else
                _dtAllUsers.DefaultView.RowFilter = string.Format("[{0}] LIKE '{1}%'", FilterColumn, txtFilterValue.Text.Trim());
            lblRecordsCount.Text = dgvUsers.Rows.Count.ToString();
        }
        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbFilterBy.Text == "Is Active")
            {
                txtFilterValue.Visible = false;
                cbIsActive.Visible = true;
                cbIsActive.Focus();
                cbIsActive.SelectedIndex = 0;
            }
            else
            {
                txtFilterValue.Visible = (cbFilterBy.Text != "None");
                cbIsActive.Visible = false;
                txtFilterValue.Text = "";
                txtFilterValue.Focus();
            }
        }
        private void cbIsActive_SelectedIndexChanged(object sender, EventArgs e)
        {
            string FilterColumn = "IsActive";
            string FilterValue = cbIsActive.Text;
            switch (FilterValue)
            {
                case "All":
                    break;
                case "Yes":
                    FilterColumn = "1";
                    break;
                case "No":
                    FilterColumn = "0";
                    break;
            }
            if (FilterValue == "All")
            {
                _dtAllUsers.DefaultView.RowFilter = "";
            }
            else
                _dtAllUsers.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterColumn, FilterValue);
            lblRecordsCount.Text = dgvUsers.Rows.Count.ToString();
        }
        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilterBy.Text == "User ID" || cbFilterBy.Text == "Person ID")
            {
                e.Handled = (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar));
            }
        }
        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (clsUser.DeleteUser((int)dgvUsers.CurrentRow.Cells[0].Value))
            {
                MessageBox.Show("User Deleted Successfully", "Success", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                frmListUsers_Load(null, null);
            }
            else
            {
                MessageBox.Show("Couldn't Delete User Because It Has Data Linked to him",
                    "Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmChangePassword((int)dgvUsers.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
        }
        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmShowUserInfo((int)dgvUsers.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
        }
        private void NotImplementedFeatures(object sender, EventArgs e)
        {
            MessageBox.Show("This Feature Is Not Implemented Yet!", "Not Ready!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}