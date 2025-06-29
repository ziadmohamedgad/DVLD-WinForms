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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
namespace DVLD.Applications.Local_Driving_License
{
    public partial class frmAddUpdateLocalDrivingLicenseApplication : Form
    {
        private enum enMode { AddNew = 0, Update = 1 };
        private enMode _Mode;
        private int _LocalDrivingLicenseApplicationID = -1;
        private int _SelectedPersonID = -1;
        private clsLocalDrivingLicenseApplication _LDLApp;
        public frmAddUpdateLocalDrivingLicenseApplication(int LocalDrivingLicenseApplicationID)
        {
            InitializeComponent();
            _LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;
            _Mode = enMode.Update;
        }
        public frmAddUpdateLocalDrivingLicenseApplication ()
        {
            InitializeComponent();
            _Mode = enMode.AddNew;
        }
        private void _FillComboBoxLicenseClasses()
        {
            DataTable dtLicenseClasses = clsLicenseClass.GetAllLicenseClasses();
            foreach (DataRow Row in  dtLicenseClasses.Rows)
            {
                cbLicenseClass.Items.Add(Row["ClassName"]);
            }
        }
        private void _ResetDefaultValues()
        {
            _FillComboBoxLicenseClasses();
            if (_Mode == enMode.AddNew)
            {
                _LDLApp = new clsLocalDrivingLicenseApplication();
                lblTitle.Text = "New Local Driving License Application";
                this.Text = "New Local Driving License Application";
                ctrlPersonCardWithFilter1.FilterFocus();
                tpApplicationInfo.Enabled = false;
                cbLicenseClass.SelectedIndex = 2;
                lblFees.Text = clsApplicationType.Find((int)clsApplication.enApplicationType.NewDrivingLicense).Fees.ToString();
                lblApplicationDate.Text = DateTime.Now.ToShortDateString();
                lblCreatedByUser.Text = clsRegLogger.CurrentUser.UserName;
            }
            else
            {
                lblTitle.Text = "Update Local Driving License Application";
                this.Text = "Update Local Driving License Application";
                tpApplicationInfo.Enabled = true;
                btnSave.Enabled = true;
            }
            //Other Way
            //lblTitle.Text = this.Text = (_Mode == enMode.AddNew ? "Add New" : "Update") + "Local Driving License Application";
            //it's like: int a, b; => a = b = 5;
        }
        private void _LoadData()
        {
            _LDLApp = clsLocalDrivingLicenseApplication.FindByLocalDrivingAppLicenseID(_LocalDrivingLicenseApplicationID);
            if (_LDLApp == null)
            {
                MessageBox.Show("No Application With ID = " + _LocalDrivingLicenseApplicationID,
                    "Error", MessageBoxButtons.OK,MessageBoxIcon.Error);
                this.Close();
                return;
            }
            ctrlPersonCardWithFilter1.LoadPersonInfo(_LDLApp.ApplicationID);
            lblLocalDrivingLicenseApplicationID.Text = _LDLApp.LocalDrivingLicenseApplicationID.ToString();
            lblApplicationDate.Text = clsFormat.DateToShort(_LDLApp.ApplicationDate);
            cbLicenseClass.SelectedIndex = cbLicenseClass.FindString(clsLicenseClass.Find(_LDLApp.LicenseClassID).ClassName);
            lblFees.Text = _LDLApp.PaidFees.ToString();
            lblCreatedByUser.Text = clsUser.FindByUserID(_LDLApp.CreatedByUserID).UserName;
            // or lblCreatedByUser.Text = _LDLApp.CreatedByUserInfo.UserName;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            int LicenseClassID = clsLicenseClass.Find(cbLicenseClass.Text).LicenseClassID;
            int ActiveApplicationID = clsApplication.GetActiveApplicationIDForLicenseClass(_SelectedPersonID,
                clsApplication.enApplicationType.NewDrivingLicense, LicenseClassID);
            if (ActiveApplicationID != -1) // There is an active application with the same class and still active
            {
                MessageBox.Show("Choose Another License Class, The Selected Person Already Have An Active Application" +
                    "For The Selected Class With ID = " + ActiveApplicationID, "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                cbLicenseClass.Focus();
                return;
            }
            // check if user already have issued license of the same driving class
            if (clsLicense.IsLicenseExistByPersonID(ctrlPersonCardWithFilter1.PersonID, LicenseClassID))
            {
                MessageBox.Show("Person already have a license with the same applied driving class," +
                    " Choose diffrent driving class", "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            _LDLApp.ApplicantPersonID = ctrlPersonCardWithFilter1.PersonID;
            _LDLApp.ApplicationDate = DateTime.Now;
            _LDLApp.ApplicationTypeID = (int)clsApplication.enApplicationType.NewDrivingLicense;
            _LDLApp.ApplicationStatus = clsApplication.enApplicationStatus.New;
            _LDLApp.LastStatusDate = DateTime.Now;
            _LDLApp.PaidFees = Convert.ToSingle(lblFees.Text);
            _LDLApp.CreatedByUserID = clsRegLogger.CurrentUser.UserID;
            _LDLApp.LicenseClassID = LicenseClassID;
            if (_LDLApp.Save())
            {
                lblLocalDrivingLicenseApplicationID.Text = _LDLApp.LocalDrivingLicenseApplicationID.ToString();
                _Mode = enMode.Update;
                lblTitle.Text = "Update Local Driving License Application";
                this.Text = "Update Local Driving License Application";
                MessageBox.Show("Data Saved Successfully", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                MessageBox.Show("Error: Data Is not Saved Successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void btnApplicationInfoNext_Click(object sender, EventArgs e)
        {
            if (_Mode == enMode.Update)
            {
                btnSave.Enabled = true;
                tpApplicationInfo.Enabled = true;
                tcApplication.SelectedTab = tcApplication.TabPages["tpApplicationInfo"];
                return;
            }
            // add new mode
            if (_Mode == enMode.Update || ctrlPersonCardWithFilter1.PersonID != -1)
            {
                btnSave.Enabled = true;
                tpApplicationInfo.Enabled = true;
                tcApplication.SelectedTab = tcApplication.TabPages["tpApplicationInfo"];
            }
            else
            {
                MessageBox.Show("Please Select A Person", "Select A Person", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ctrlPersonCardWithFilter1.FilterFocus();
            }
        }
        private void frmAddUpdateLocalDrivingLicenseApplication_Load(object sender, EventArgs e)
        {
            _ResetDefaultValues();
            if (_Mode == enMode.Update)
                _LoadData();
        }
        private void ctrlPersonCardWithFilter1_OnPersonSelected(int obj)
        {
            _SelectedPersonID = obj;
        }
        private void frmAddUpdateLocalDrivingLicenseApplication_Activated(object sender, EventArgs e)
        {
            ctrlPersonCardWithFilter1.FilterFocus();
        }
    }
}