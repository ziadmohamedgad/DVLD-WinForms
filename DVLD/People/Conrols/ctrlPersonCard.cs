using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using DVLD.Properties;
using DVLD_BusinessLayer;
using System.Reflection.Emit;
namespace DVLD.People.Conrols
{
    public partial class ctrlPersonCard : UserControl
    {
        private int _PersonID = -1;
        private clsPerson _Person;
        public int PersonID
        {
            get { return _PersonID; }
        }
        public clsPerson SelectedPersonInfo
        {
            get { return _Person; }
        }
        public ctrlPersonCard()
        {
            InitializeComponent();
        }
        private void _LoadPersonImage()
        {
            if (_Person.Gender == 0)
            {
                pbPersonImage.Image = Resources.Male_512;
                pbGender.Image = Resources.Man_32;
            }
            else
            {
                pbPersonImage.Image = Resources.Female_512;
                pbGender.Image = Resources.Woman_32;
            }
            if (_Person.ImagePath != "")
            {
                if (File.Exists(_Person.ImagePath))
                {
                    pbPersonImage.ImageLocation = _Person.ImagePath;
                }
                else
                {
                    MessageBox.Show("Could Not Find This Image: = " +  _Person.ImagePath, "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void _FillPersonInfo()
        {
            _PersonID = _Person.PersonID;
            llEditPersonInfo.Visible = true;
            _PersonID = _Person.PersonID;
            lblPersonID.Text = _Person.PersonID.ToString();
            lblFullName.Text = _Person.FullName;
            lblNationalNo.Text = _Person.NationalNo;
            lblGender.Text = _Person.Gender == 0 ? "Male" : "Female";
            lblEmail.Text = _Person.Email;
            lblPhone.Text = _Person.Phone;
            lblAddress.Text = _Person.Address;
            lblDateOfBirth.Text = _Person.DateOfBirth.ToShortDateString();
            lblCountry.Text = _Person.CountryInfo.CountryName;
            _LoadPersonImage();
        }
        public void ResetPersonInfo()
        {
            llEditPersonInfo.Visible = false;
            _PersonID = -1;
            string DefaultValue = "[????]";
            lblPersonID.Text = DefaultValue;
            lblFullName.Text = DefaultValue;
            lblNationalNo.Text = DefaultValue;
            lblGender.Text = DefaultValue;
            pbGender.Image = Resources.Man_32;
            lblEmail.Text = DefaultValue;
            lblAddress.Text = DefaultValue;
            lblDateOfBirth.Text = DefaultValue;
            lblPhone.Text = DefaultValue;
            lblCountry.Text = DefaultValue;
            pbPersonImage.Image = Resources.Male_512;
        }
        public void LoadPersonInfo(int PersonID)
        {
            _Person = clsPerson.Find(PersonID);
            if (_Person == null)
            {
                ResetPersonInfo();
                MessageBox.Show("No Person Info With PersonID = " + PersonID.ToString(),
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            _FillPersonInfo();  
        }
        public void LoadPersonInfo(string NationalNo)
        {
            _Person = clsPerson.Find(NationalNo);
            if (_Person == null)
            {
                ResetPersonInfo();
                MessageBox.Show("No Person Info With PersonID = " + NationalNo.ToString(),
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            _FillPersonInfo();
        }
        private void llEditPersonInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmAddUpdatePerson frm = new frmAddUpdatePerson(_PersonID);
            frm.ShowDialog();
            LoadPersonInfo(_PersonID);
        }
    }
}
