using DVLD.Global_Classes;
using DVLD.Properties;
using DVLD_BusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace DVLD.Licenses.Local_Licenses.Controls
{
    public partial class ctrlDriverLicenseInfo : UserControl
    {
        private int _LocalLicenseID = -1;
        private clsLicense _LocalLicense;
        public int LocalLicenseID
        {
            get
            {
                return _LocalLicenseID;
            }
        }
        public clsLicense SelectedLicenseInfo
        {
            get
            {
                return _LocalLicense;
            }
        }
        public ctrlDriverLicenseInfo()
        {
            InitializeComponent();
        }
        public void ResetDefaults()
        {
            _LocalLicenseID = -1;
            lblClass.Text = "[???]";
            lblFullName.Text = "[????]";
            lblLicenseID.Text = "[????]";
            lblNationalNo.Text = "[????]";
            lblGender.Text = "[????]";
            lblIssueDate.Text = "[????]";
            lblIssueReason.Text = "[????]";
            lblNotes.Text = "[????]";
            lblIsActive.Text = "[????]";
            lblDateOfBirth.Text = "[????]";
            lblDriverID.Text = "[????]";
            lblExpirationDate.Text = "[????]";
            lblIsDetained.Text = "[????]";
            pbPersonImage.Image = Resources.Male_512;
            pbGender.Image = Resources.Man_32;
        }
        public void LoadInfo(int LocalLicenseID)
        {
            _LocalLicense = clsLicense.Find(LocalLicenseID);
            if (_LocalLicense == null)
            {
                MessageBox.Show("Could not find License ID = " + LocalLicenseID.ToString(),
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ResetDefaults();
                return;
            }
            _FillData();
        }
        private void _FillData()
        {
            _LocalLicenseID = _LocalLicense.LicenseID;
            lblClass.Text = _LocalLicense.LicenseClassInfo.ClassName;
            lblFullName.Text = _LocalLicense.DriverInfo.PersonInfo.FullName;
            lblLicenseID.Text = _LocalLicense.LicenseID.ToString();
            lblNationalNo.Text = _LocalLicense.DriverInfo.PersonInfo.NationalNo;
            lblGender.Text = _LocalLicense.DriverInfo.PersonInfo.Gender == 0 ? "Male" : "Female";
            lblIssueDate.Text = clsFormat.DateToShort(_LocalLicense.IssueDate);
            lblIssueReason.Text = _LocalLicense.IssueReasonText;
            lblNotes.Text = _LocalLicense.Notes == "" ? "No Notes" : _LocalLicense.Notes;
            lblIsActive.Text = _LocalLicense.IsActive ? "Yes" : "No";
            lblDateOfBirth.Text = clsFormat.DateToShort(_LocalLicense.DriverInfo.PersonInfo.DateOfBirth);
            lblDriverID.Text = _LocalLicense.DriverID.ToString();
            lblExpirationDate.Text = clsFormat.DateToShort(_LocalLicense.ExpirationDate);
            lblIsDetained.Text = _LocalLicense.IsDetained ? "Yes" : "No";
            _LoadPersonImage();
        }
        private void _LoadPersonImage()
        {
            if (_LocalLicense.DriverInfo.PersonInfo.Gender == 0)
            {
                pbPersonImage.Image = Resources.Male_512;
                pbGender.Image = Resources.Man_32;
            }
            else
            {
                pbPersonImage.Image = Resources.Female_512;
                pbGender.Image = Resources.Woman_32;
            }
            string ImagePath = _LocalLicense.DriverInfo.PersonInfo.ImagePath;
            if (ImagePath != "")
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