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
    public partial class frmScheduleTest : Form
    {
        private int _LDLAppID = -1;
        private int _AppointmentID = -1;
        private clsTestType.enTestType _TestTypeID = clsTestType.enTestType.VisionTest;
        public frmScheduleTest(int LocalDrivingLicenseApplication, clsTestType.enTestType TestTypeID, int AppointmentID = -1)
        {
            InitializeComponent();
            this._LDLAppID = LocalDrivingLicenseApplication;
            this._TestTypeID = TestTypeID;
            this._AppointmentID = AppointmentID;
        }
        private void frmScheduleTest_Load(object sender, EventArgs e)
        {
            ctrlScheduleTest1.TestTypeID = _TestTypeID;
            ctrlScheduleTest1.LoadInfo(_LDLAppID, _AppointmentID);
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}