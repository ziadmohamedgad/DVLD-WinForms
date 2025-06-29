using DVLD_DataLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace DVLD_BusinessLayer
{
    public class clsTest
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;
        public clsTestAppointment TestAppointmentInfo;
        public int TestID { set; get; }
        public int TestAppointmentID { set; get; }
        public bool TestResult { set; get; }
        public string Notes { set; get; }
        public int CreatedByUserID { set; get; }
        public clsTest()
        {
            this.TestID = -1;
            this.TestAppointmentID = -1;
            this.TestResult = false;
            this.Notes = "";
            this.CreatedByUserID = -1;
            this.TestAppointmentInfo = null;
            Mode = enMode.AddNew;
        }
        public clsTest(int TestID, int TestAppointmentID, bool TestResult, string Notes, int CreatedByUserID)
        {
            this.TestID = TestID;
            this.TestAppointmentID = TestAppointmentID;
            this.TestResult = TestResult;
            this.Notes = Notes;
            this.CreatedByUserID = CreatedByUserID;
            this.TestAppointmentInfo = clsTestAppointment.Find(TestAppointmentID);
            Mode = enMode.Update;
        }
        private bool _AddNewTest()
        {
            this.TestID = clsTestData.AddNewTest(this.TestAppointmentID, this.TestResult,
                this.Notes, this.CreatedByUserID);
            return this.TestID != -1;
        }
        private bool _UpdateTest()
        {
            return clsTestData.UpdateTest(this.TestID, this.TestAppointmentID, this.TestResult,
                this.Notes, this.CreatedByUserID);
        }
        public bool Save()
        {
            switch(Mode)
            {
                case enMode.AddNew:
                    {
                        if (_AddNewTest())
                        {
                            Mode = enMode.Update;
                            return true;
                        }
                        else
                            return false;
                    }
                case enMode.Update:
                    return _UpdateTest();
            }
            return false;
        }
        public static clsTest Find(int TestID)
        {
            int TestAppointmentID = -1, CreatedByUserID = -1;
            bool TestResult = false;
            string Notes = "";
            bool IsFound = clsTestData.GetTestInfoByTestID(TestID, ref TestAppointmentID, ref TestResult, ref Notes, ref CreatedByUserID);
            if (IsFound)
                return new clsTest(TestID, TestAppointmentID, TestResult, Notes, CreatedByUserID);
            else
                return null;
        }
        public static clsTest FindLastTestPerPersonAndLicenseClass(int PersonID, int LicenseClassID, clsTestType.enTestType TestType)
        {
            int TestAppointmentID = -1, CreatedByUserID = -1, TestID = -1;
            bool TestResult = false;
            string Notes = "";
            bool IsFound = clsTestData.GetLastTestByPersonAndTestTypeAndLicenseClass(PersonID, LicenseClassID, (int)TestType, ref TestID,
                ref TestAppointmentID, ref TestResult, ref Notes, ref CreatedByUserID);
            if (IsFound)
                return new clsTest(TestID, TestAppointmentID, TestResult, Notes, CreatedByUserID);
            else
                return null;
        }
        public static DataTable GetAllTests()
        {
            return clsTestData.GetAllTests();
        }
        public static byte GetPassedTestsCount(int LocalDrivingLicenseApplicationID)
        {
            return clsTestData.GetPassedTestsCount(LocalDrivingLicenseApplicationID);
        }
        public static bool PassedAllTests(int LocalDrivingLicenseApplicationID)
        {
            return GetPassedTestsCount(LocalDrivingLicenseApplicationID) == 3;
        }
    }
}