using DVLD.Global_Classes;
using DVLD.Properties;
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
using System.IO;
namespace DVLD.Licenses.International_Licenses.Controls
{
    public partial class ctrlDriverInternationalLicenseInfo : UserControl
    {
        private int _InternationalLicenseID = -1;
        private clsInternationalLicense _InternationalLicense;
        public int InterationalLicenseID
        {
            get
            {
                return _InternationalLicenseID;
            }
        }
        public clsInternationalLicense SelectedInternationalLicenseInfo
        {
            get
            {
                return _InternationalLicense;
            }
        }
        public ctrlDriverInternationalLicenseInfo()
        {
            InitializeComponent();
        }
        public void ResetDefaultValues()
        {
            _InternationalLicenseID = -1;
            lblFullName.Text = "[???]";
            lblInternationalLicenseID.Text = "[???]";
            lblLocalLicenseID.Text = "[????]";
            lblNationalNo.Text = "[????]";
            lblGender.Text = "[????]";
            lblIssueDate.Text = "[????]";
            lblApplicationID.Text = "[???]";
            lblIsActive.Text = "[????]";
            lblDateOfBirth.Text = "[????]";
            lblDriverID.Text = "[????]";
            lblExpirationDate.Text = "[????]";
            pbGender.Image = Resources.Man_32;
            pbPersonImage.Image = Resources.Male_512;
        }
        public void LoadDriverInternationalLicenseInfo(int InternationalLicenseID)
        {
            _InternationalLicense = clsInternationalLicense.Find(InterationalLicenseID);
            if (_InternationalLicense == null)
            {
                ResetDefaultValues();
                MessageBox.Show("");
                return;
            }
            _FillData();
        }
        private void _FillData()
        {
            _InternationalLicenseID = _InternationalLicense.InternationalLicenseID;
            lblFullName.Text = _InternationalLicense.ApplicantFullName;
            lblInternationalLicenseID.Text = _InternationalLicense.InternationalLicenseID.ToString();
            lblLocalLicenseID.Text = _InternationalLicense.IssuedUsingLocalLicenseID.ToString();
            lblNationalNo.Text = _InternationalLicense.PersonInfo.NationalNo;
            lblGender.Text = _InternationalLicense.PersonInfo.Gender == 0 ? "Male" : "Female";
            lblIssueDate.Text = clsFormat.DateToShort(_InternationalLicense.IssueDate);
            lblApplicationID.Text = _InternationalLicense.ApplicationID.ToString();
            lblIsActive.Text = _InternationalLicense.IsActive ? "Yes" : "No";
            lblDateOfBirth.Text = clsFormat.DateToShort(_InternationalLicense.PersonInfo.DateOfBirth);
            lblDriverID.Text = _InternationalLicense.DriverID.ToString();
            lblExpirationDate.Text = clsFormat.DateToShort(_InternationalLicense.ExpirationDate);
            _LoadDriverImage();
        }
        private void _LoadDriverImage()
        {
            if (_InternationalLicense.PersonInfo.Gender == 0)
            {
                pbGender.Image = Resources.Man_32;
                pbPersonImage.Image = Resources.Male_512;
            }
            else
            {
                pbGender.Image = Resources.Woman_32;
                pbPersonImage.Image = Resources.Female_512;
            }
            string ImagePath = _InternationalLicense.PersonInfo.ImagePath;
            if (ImagePath != null)
            {
                if (File.Exists(ImagePath))
                {
                    pbPersonImage.ImageLocation = ImagePath;
                }
                else
                {
                    MessageBox.Show("Could Not Find This Image: = " + ImagePath, "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}