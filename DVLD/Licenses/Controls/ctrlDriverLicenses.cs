﻿using DVLD.Licenses.International_Licenses;
using DVLD.Licenses.Local_Licenses;
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

namespace DVLD.Licenses.Controls
{
    public partial class ctrlDriverLicenses : UserControl
    {
        private int _DriverID = -1;
        private clsDriver _Driver;
        private DataTable _dtDriverLocalLicensesHistory;
        private DataTable _dtDriverInternationalLicensesHistory;
        public ctrlDriverLicenses()
        {
            InitializeComponent();
        }
        private void _LoadLocalLicenseInfo()
        {
            _dtDriverLocalLicensesHistory = clsDriver.GetLicenses(_DriverID);
            dgvLocalLicensesHistory.DataSource = _dtDriverLocalLicensesHistory;
            lblLocalLicensesRecords.Text = dgvLocalLicensesHistory.Rows.Count.ToString();
            if (dgvLocalLicensesHistory.Rows.Count > 0)
            {
                dgvLocalLicensesHistory.Columns[0].HeaderText = "Lic.ID";
                dgvLocalLicensesHistory.Columns[0].Width = 110;
                dgvLocalLicensesHistory.Columns[1].HeaderText = "App.ID";
                dgvLocalLicensesHistory.Columns[1].Width = 110;
                dgvLocalLicensesHistory.Columns[2].HeaderText = "Class Name";
                dgvLocalLicensesHistory.Columns[2].Width = 270;
                dgvLocalLicensesHistory.Columns[3].HeaderText = "Issue Date";
                dgvLocalLicensesHistory.Columns[3].Width = 170;
                dgvLocalLicensesHistory.Columns[4].HeaderText = "Expiration Date";
                dgvLocalLicensesHistory.Columns[4].Width = 170;
                dgvLocalLicensesHistory.Columns[5].HeaderText = "Is Active";
                dgvLocalLicensesHistory.Columns[5].Width = 110;
            }
        }
        private void _LoadInternationalLicenseInfo()
        {
            _dtDriverInternationalLicensesHistory = clsDriver.GetInternationalLicenses(_DriverID);
            dgvInternationalLicensesHistory.DataSource = _dtDriverInternationalLicensesHistory;
            lblInternationalLicensesRecords.Text = dgvInternationalLicensesHistory.Rows.Count.ToString();
            if (dgvInternationalLicensesHistory.Rows.Count > 0)
            {
                dgvInternationalLicensesHistory.Columns[0].HeaderText = "Int.License ID";
                dgvInternationalLicensesHistory.Columns[0].Width = 160;
                dgvInternationalLicensesHistory.Columns[1].HeaderText = "Application ID";
                dgvInternationalLicensesHistory.Columns[1].Width = 135;
                dgvInternationalLicensesHistory.Columns[2].HeaderText = "L.License ID";
                dgvInternationalLicensesHistory.Columns[2].Width = 130;
                dgvInternationalLicensesHistory.Columns[3].HeaderText = "Issue Date";
                dgvInternationalLicensesHistory.Columns[3].Width = 180;
                dgvInternationalLicensesHistory.Columns[4].HeaderText = "Expiration Date";
                dgvInternationalLicensesHistory.Columns[4].Width = 180;
                dgvInternationalLicensesHistory.Columns[5].HeaderText = "Is Active";
                dgvInternationalLicensesHistory.Columns[5].Width = 120;
            }
        }
        public void LoadInfo(int DriverID)
        {
            _Driver = clsDriver.FindByDriverID(DriverID);
            if (_Driver == null)
            {
                MessageBox.Show("There Is No Driver With ID = " + DriverID,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            _DriverID = DriverID;
            _LoadLocalLicenseInfo();
            _LoadInternationalLicenseInfo();
        }
        public void LoadInfoByPersonID(int PersonID)
        {
            _Driver = clsDriver.FindByPersonID(PersonID);
            if (_Driver == null)
            {
                MessageBox.Show("There Is No Driver With Person ID = " + PersonID,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            _DriverID = _Driver.DriverID;
            _LoadLocalLicenseInfo();
            _LoadInternationalLicenseInfo();
        }
        private void showLicenseInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmShowLicenseInfo((int)dgvLocalLicensesHistory.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
        }
        private void InternationalLicenseHistoryToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Form frm = new frmShowInternationalLicenseInfo((int)dgvInternationalLicensesHistory.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
        }
        public void Clear()
        {
            _dtDriverLocalLicensesHistory.Clear();
            _dtDriverInternationalLicensesHistory.Clear();
        }
    }
}