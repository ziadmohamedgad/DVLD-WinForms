using DVLD.Global_Classes;
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
namespace DVLD.Tests
{
    public partial class frmEditTestType : Form
    {
        private clsTestType.enTestType _TestTypeID = clsTestType.enTestType.VisionTest;
        private clsTestType _TestType;
        public frmEditTestType(clsTestType.enTestType TestTypeID)
        {
            InitializeComponent();
            _TestTypeID = TestTypeID;
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void _ResetDefaultValues()
        {
            lblID.Text = "[???]";
            txtTitle.Text = "";
            txtDescription.Text = "";
            txtFees.Text = "";
        }
        private void _LoadData()
        {
            _TestType = clsTestType.Find(_TestTypeID);
            if (_TestType != null)
            {
                lblID.Text = ((int)_TestType.ID).ToString();
                txtTitle.Text = _TestType.Title;
                txtDescription.Text = _TestType.Description;
                txtFees.Text = _TestType.Fees.ToString();
            }
            else
            {
                MessageBox.Show("Could not find Test Type with id = " + _TestTypeID.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }
        private void frmEditTestType_Load(object sender, EventArgs e)
        {
            _ResetDefaultValues();
            _LoadData();
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                MessageBox.Show("Some Fields Are Not Valid!, Put The Mouse Over The Red Icon(s) To See The" +
                    "Error", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (_TestType == null) return; // there is not Add New Mode, so our object couldn't be null
            _TestType.Title = txtTitle.Text.Trim();
            _TestType.Description = txtDescription.Text.Trim();
            _TestType.Fees = Convert.ToSingle(txtFees.Text.Trim());
            if (_TestType.Save())
            {
                MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                MessageBox.Show("Error: Data Is Not Saved Successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        private void txtTitle_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtTitle.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtTitle, "Title Cannot Be Empty!");
            }
            else
                errorProvider1.SetError(txtTitle, null);
        }
        private void txtDescription_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtDescription.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtDescription, "Description Cannot Be Empty!");
            }
            else
                errorProvider1.SetError(txtDescription, null);
        }
        private void txtFees_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtFees.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtFees, "This Field Cannot Be Empty!");
                return;
            }
            else
                errorProvider1.SetError(txtFees, null);
            // Extra handling in case there isn't KeyPress function for txtFees, but there is.
            if (!clsValidation.IsNumber(txtFees.Text.Trim())) // is number contains integers and floats
            {
                e.Cancel = true;
                errorProvider1.SetError(txtFees, "Invalid Number!");
            }
            else
                errorProvider1.SetError(txtFees, null);
        }
        private void txtFees_KeyPress(object sender, KeyPressEventArgs e)
        {
            // we are handling this to avoid any thing except a  correct (integer/float) value
            if (e.KeyChar == '.' && txtFees.Text.Trim().IndexOf('.') > -1/*There is . already before*/)
            {
                e.Handled = true;
            }
            else
                e.Handled = !char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.';
        }
    }
}