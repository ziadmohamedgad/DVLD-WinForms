using DVLD.Global_Classes;
using DVLD.Properties;
using  DVLD_BusinessLayer;
using DVLD_Shared;
using System;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Resources;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Windows.Forms;
namespace DVLD.People
{
    public partial class frmAddUpdatePerson : Form
    {
        public delegate void DataBackEventHandler(object sender, int PersonID);
        public event DataBackEventHandler DataBack;
        public enum enMode { AddNew = 0, Update = 1 };
        public enum enGender { Male = 0, Female = 1 };
        private enMode _Mode;
        private int _PersonID = -1;
        private clsPerson _Person;
        public frmAddUpdatePerson()
        {
            InitializeComponent();
            _Mode = enMode.AddNew;
        }
        public frmAddUpdatePerson(int PersonID)
        {
            InitializeComponent();
            _PersonID = PersonID;
            _Mode = enMode.Update;
        }
        private void _FillCountriesInComboBox()
        {
            DataTable dtCountries = clsCountry.GetAllCountries();
            foreach (DataRow Row in dtCountries.Rows)
            {
                cbCountry.Items.Add(Row["CountryName"]);
            }
        }
        private void _ResetDefaultValues()
        {
            _FillCountriesInComboBox();
            if (_Mode == enMode.AddNew)
            {
                lblTitle.Text = "Add New Person";
                lblPersonID.Text = "N/A";
                _Person = new clsPerson();
            }
            else
                lblTitle.Text = "Update Person";
            dtpDateOfBirth.MaxDate = DateTime.Now.AddYears(-18);
            dtpDateOfBirth.MinDate = DateTime.Now.AddYears(-100);
            dtpDateOfBirth.Value = dtpDateOfBirth.MaxDate;
            rbMale.Checked = true;
            if (rbMale.Checked) pbPersonImage.Image = Resources.Male_512;
            else pbPersonImage.Image = Resources.Female_512;
            llRemoveImage.Visible = (pbPersonImage.ImageLocation != null);
            txtFirstName.Text = "";
            txtSecondName.Text = "";
            txtThirdName.Text = "";
            txtLastName.Text = "";
            txtNationalNo.Text = "";
            txtEmail.Text = "";
            txtPhone.Text = "";
            cbCountry.SelectedIndex = cbCountry.FindString("Egypt");
            txtAddress.Text = "";
        }
        private void _LoadData()
        {
            _Person = clsPerson.Find(_PersonID);
            if (_Person == null)
            {
                MessageBox.Show("No Person With ID = " + _PersonID,
                    "Person Not Found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.Close();
                return;
            }
            //lblTitle.Text = "Update Person";
            lblPersonID.Text = _Person.PersonID.ToString();
            txtFirstName.Text = _Person.FirstName;
            txtSecondName.Text = _Person.SecondName;
            txtThirdName.Text = _Person.ThirdName;
            txtLastName.Text = _Person.LastName;
            txtNationalNo.Text = _Person.NationalNo;
            dtpDateOfBirth.Value = _Person.DateOfBirth;
            if (_Person.Gender == 0)
                rbMale.Checked = true;
            else
                rbFemale.Checked = true;
            txtPhone.Text = _Person.Phone;
            txtEmail.Text = _Person.Email;
            txtAddress.Text = _Person.Address;
            cbCountry.SelectedIndex = cbCountry.FindString(_Person.CountryInfo.CountryName);
            if (_Person.ImagePath != "")
                pbPersonImage.ImageLocation = _Person.ImagePath;
            llRemoveImage.Visible = (_Person.ImagePath != "");
        }
        private void frmAddUpdatePerson_Load(object sender, EventArgs e)
        {
            _ResetDefaultValues();
            if (_Mode == enMode.Update)
                _LoadData();
        }
        private void rbMale_Click(object sender, EventArgs e)
        {
            if (pbPersonImage.ImageLocation == null)
                pbPersonImage.Image = Resources.Male_512;
        }
        private void rbFemale_Click(object sender, EventArgs e)
        {
            if (pbPersonImage.ImageLocation == null)
                pbPersonImage.Image = Resources.Female_512;
        }
        private void llRemoveImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            pbPersonImage.ImageLocation = null;
            if (rbMale.Checked)
                pbPersonImage.Image = Resources.Male_512;
            else
                pbPersonImage.Image = Resources.Female_512;
            llRemoveImage.Visible = false;
        }
        private bool _HandlePersonImage()
        {
            if (_Person.ImagePath != pbPersonImage.ImageLocation)
            {
                if (_Person.ImagePath != "")
                {
                    if (File.Exists(_Person.ImagePath))
                    {
                        try
                        {
                            File.Delete(_Person.ImagePath);
                        }
                        catch (IOException iox)
                        {
                            MessageBox.Show("Error Deleting The Old Photo: " + iox.Message, "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            clsEventLogger.SaveLog("Application", $"Error deleting the old photo: {iox.Message}",
                                System.Diagnostics.EventLogEntryType.Error);
                            return false;
                        }
                    }
                }
                if (pbPersonImage.ImageLocation != null)
                {
                    string SourceImageFile = pbPersonImage.ImageLocation.ToString();
                    if (File.Exists(SourceImageFile)) // maybe someone moved or deleted the original 
                    { // picture after uploading it to our program
                        if (clsUtil.CopyImageToProjectImagesFolder(ref SourceImageFile))
                        {
                            pbPersonImage.ImageLocation = SourceImageFile;
                            return true;
                        }
                        else
                        {
                            MessageBox.Show("Error Copying Image File", "Error", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                            return false;
                        }
                    }
                    else
                    {
                        MessageBox.Show("The Photo You Uploaded Deleted Or Moved Before Saving," +
                            "Try Choosing It Again", "Unlocated Photo!", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        return false;
                    }
                }
            }
            return true;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                MessageBox.Show("Some Fields Are Not Valid!, Put The Mouse Over The Red Icon(s) To See The Error",
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!_HandlePersonImage())
            {
                MessageBox.Show("Some Issues Happened During Saving Related To Your Personal Photo, Please Try Again Later",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            _Person.FirstName = txtFirstName.Text.Trim();
            _Person.SecondName = txtSecondName.Text.Trim();
            _Person.ThirdName = txtThirdName.Text.Trim();
            _Person.LastName = txtLastName.Text.Trim();
            _Person.NationalNo = txtNationalNo.Text.Trim();
            _Person.DateOfBirth = dtpDateOfBirth.Value;
            _Person.Phone = txtPhone.Text.Trim();
            _Person.Email = txtEmail.Text.Trim();
            _Person.Address = txtAddress.Text.Trim();
            if (rbMale.Checked)
                _Person.Gender = (byte)enGender.Male;
            else
                _Person.Gender = (byte)enGender.Female;
            if (pbPersonImage.ImageLocation != null)
                _Person.ImagePath = pbPersonImage.ImageLocation;
            else
                _Person.ImagePath = "";
            _Person.NationalityCountryID = clsCountry.Find(cbCountry.Text).CountryID;
            if (_Person.Save())
            {
                lblPersonID.Text = _Person.PersonID.ToString();
                lblTitle.Text = "Update Person";
                _Mode = enMode.Update;
                MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DataBack?.Invoke(this, _Person.PersonID);
            }
            else
                MessageBox.Show("Error: Data Is Not Saved Successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        private void _ValidateEmptyTextBox(object sender, CancelEventArgs e)
        {
            TextBox Temp = (TextBox)sender;
            if (string.IsNullOrEmpty(Temp.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(Temp, "This Field Is Required!");
            }
            else
                errorProvider1.SetError(Temp, null);
        }
        private void txtEmail_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtEmail.Text.Trim())) return;
            if (!clsValidation.ValidateEmail(txtEmail.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtEmail, "Invalid Email Address Format!");
            }
            else
                errorProvider1.SetError(txtEmail, null);
        }
        private void txtNationalNo_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtNationalNo.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtNationalNo, "This Field Is Required!");
            }
            else
                errorProvider1.SetError(txtNationalNo, null);
            if (txtNationalNo.Text.Trim() != _Person.NationalNo && clsPerson.IsPersonExist(txtNationalNo.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtNationalNo, "National Number Is Used For Another Person!");
            }
            else
                errorProvider1.SetError(txtNationalNo, null);
        }
        private void llSetImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            openFileDialog1.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pbPersonImage.ImageLocation = openFileDialog1.FileName;
                llRemoveImage.Visible = true;
            }
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}