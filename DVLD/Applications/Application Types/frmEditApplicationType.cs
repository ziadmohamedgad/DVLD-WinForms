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
using DVLD.Global_Classes;
namespace DVLD.Applications.Application_Types
{
    public partial class frmEditApplicationType : Form
    {
        private int _ApplicationTypeID = -1;
        private clsApplicationType _ApplicationType;
        public frmEditApplicationType(int ApplicationTypeID)
        {
            InitializeComponent();
            _ApplicationTypeID = ApplicationTypeID;
        }
        private void _ResetDefaultValues()
        {
            lblApplicationTypeID.Text = "[???]";
            txtTitle.Text = "";
            txtFees.Text = "";
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void _LoadData()
        {
            _ApplicationType = clsApplicationType.Find(_ApplicationTypeID);
            if (_ApplicationType != null)
            {
                lblApplicationTypeID.Text = _ApplicationType.ID.ToString();
                txtTitle.Text = _ApplicationType.Title;
                txtFees.Text = _ApplicationType.Fees.ToString();
            }
            else
            {
                MessageBox.Show("Could not find Application Type with Id = " + _ApplicationTypeID.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }
        private void frmEditApplicationTypes_Load(object sender, EventArgs e)
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
            if (_ApplicationType == null) return; // there is not Add New Mode, so our object couldn't be null
            _ApplicationType.Title = txtTitle.Text;
            _ApplicationType.Fees = Convert.ToSingle(txtFees.Text.Trim());
            if (_ApplicationType.Save())
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
        private void txtFees_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtFees.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtFees, "Title Cannot Be Empty!");
                return;
            }
            else
                errorProvider1.SetError(txtTitle, null);
            // there is a Key Press Event Already and we Handled the input already, but this is an extra 
            // way for validating the whole inputs
            //if (!clsValidation.IsNumber(txtFees.Text))
            //{
            //    e.Cancel = true;
            //    errorProvider1.SetError(txtFees, "Invalid Number.");
            //}
            //else
            //{
            //    errorProvider1.SetError(txtFees, null);
            //};
        }
        private void txtFees_KeyPress(object sender, KeyPressEventArgs e)
        {
            //bool DotInsertedBefore = txtFees.Text.IndexOf('.') > -1; #if true this means (.) Inserted Before
            if (e.KeyChar == '.' && txtFees.Text.IndexOf('.') > -1)
            {
                e.Handled = true;
            }
            else
                e.Handled = !char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.';
        }
    }
}