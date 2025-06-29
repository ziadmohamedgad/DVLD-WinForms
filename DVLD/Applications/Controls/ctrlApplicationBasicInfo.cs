using DVLD.Global_Classes;
using DVLD.People;
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
namespace DVLD.Applications.Controls
{
    public partial class ctrlApplicationBasicInfo : UserControl
    {
        private int _ApplicationID = -1;
        private clsApplication _Application;
        public int ApplicationID
        {
            get
            {
                return _ApplicationID;
            }
        }
        public ctrlApplicationBasicInfo()
        {
            InitializeComponent();
        }
        public void LoadApplicationInfo(int ApplicationID)
        {
            _Application = clsApplication.FindBaseApplication(ApplicationID);
            if (_Application == null)
            {
                ResetDefaultValues();
                MessageBox.Show("No Application With ApplicationID = " + ApplicationID.ToString(), 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
                _FillApplicationInfo();
        }
        private void _FillApplicationInfo()
        {
            _ApplicationID = _Application.ApplicationID;
            llViewPersonInfo.Visible = true;
            lblApplicationID.Text = _Application.ApplicationID.ToString();
            lblStatus.Text = _Application.StatusText;
            lblType.Text = _Application.ApplicationTypeInfo.Title;
            lblFees.Text = _Application.PaidFees.ToString();
            lblApplicant.Text = _Application.ApplicantFullName;
            lblDate.Text = clsFormat.DateToShort(_Application.ApplicationDate);
            lblStatusDate.Text = clsFormat.DateToShort(_Application.LastStatusDate);
            lblCreatedByUser.Text = _Application.CreatedByUserInfo.UserName;
        }
        public void ResetDefaultValues()
        {
            _ApplicationID = -1;
            lblApplicationID.Text = "[???]";
            lblStatus.Text = "[???]";
            lblFees.Text = "[$$$]";
            lblType.Text = "[???]";
            lblApplicant.Text = "[????]";
            lblDate.Text = "[??/??/????]";
            lblStatusDate.Text = "[??/??/????]";
            lblCreatedByUser.Text = "[????]";
            llViewPersonInfo.Visible = false;
        }
        private void llViewPersonInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form frm = new frmShowPersonInfo(_Application.ApplicantPersonID);
            frm.ShowDialog();
            LoadApplicationInfo(_ApplicationID);
        }
    }
}