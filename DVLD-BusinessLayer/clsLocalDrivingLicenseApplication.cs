using DVLD_DataLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
namespace DVLD_BusinessLayer
{
    public class clsLocalDrivingLicenseApplication : clsApplication
    {
        public enum enMode { AddNew = 0, Update = 1 }
        public enMode Mode = enMode.AddNew;
        public int LocalDrivingLicenseApplicationID { set; get; }
        public int LicenseClassID { set; get; }
        public string PersonFullName
        {
            get
            {
                return base.PersonInfo.FullName; // OR Applicant Full Name
            }
        }
        public clsLicenseClass LicenseClassInfo;
        public clsLocalDrivingLicenseApplication()
        {
            this.ApplicationTypeID = (int)clsApplication.enApplicationType.NewDrivingLicense;
            this.LocalDrivingLicenseApplicationID = -1;
            this.LicenseClassID = -1;
            this.LicenseClassInfo = null;
            Mode = enMode.AddNew;
        }
        public clsLocalDrivingLicenseApplication(int LocalDrivingLicenseApplicationID, int ApplicationID, int ApplicantPersonID,
            DateTime ApplicationDate, int ApplicationTypeID, enApplicationStatus ApplicationStatus, DateTime LastStatusDate,
            float PaidFees, int CreatedByUserID, int LicenseClassID) // Here we give the Application class values aside with LDLApp values
        {
            this.LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;
            this.ApplicationID = ApplicationID;
            this.ApplicantPersonID = ApplicantPersonID;
            this.ApplicationDate = ApplicationDate;
            this.ApplicationTypeID = ApplicationTypeID;
            this.ApplicationStatus = ApplicationStatus;
            this.LastStatusDate = LastStatusDate;
            this.PaidFees = PaidFees;
            this.CreatedByUserID = CreatedByUserID;
            this.LicenseClassID = LicenseClassID;
            this.LicenseClassInfo = clsLicenseClass.Find(LicenseClassID);
            Mode = enMode.Update;
        }
        private bool _AddNewLocalDrivingLicenseApplication() // we handled Application class at save before adding new (including Mode)
        {
            this.LocalDrivingLicenseApplicationID = clsLocalDrivingLicenseApplicationData.AddNewLocalDrivingLicenseApplication(this.ApplicationID, 
                this.LicenseClassID);
            return this.LocalDrivingLicenseApplicationID != -1;
        }
        private bool _UpdateLocalDrivingLicenseApplication() //we handled Application class at save before Updating (including Mode)
        {
            return clsLocalDrivingLicenseApplicationData.UpdateLocalDrivingLicenseApplication(this.LocalDrivingLicenseApplicationID, this.ApplicationID,
                this.LicenseClassID);
        }
        public static clsLocalDrivingLicenseApplication FindByLocalDrivingAppLicenseID(int LocalDrivingLicenseApplicationID)
        {
            int ApplicationID = -1, LicenseClassID = -1;
            bool IsFound = clsLocalDrivingLicenseApplicationData.GetLocalDrivingLicenseApplicationInfoByID(LocalDrivingLicenseApplicationID,
                ref ApplicationID, ref LicenseClassID);
            if (IsFound)
            {
                clsApplication Application = clsApplication.FindBaseApplication(ApplicationID); // we need to find the application first
                if (Application == null) return null;
                return new clsLocalDrivingLicenseApplication(LocalDrivingLicenseApplicationID, Application.ApplicationID, 
                    Application.ApplicantPersonID, Application.ApplicationDate, Application.ApplicationTypeID,
                    Application.ApplicationStatus, Application.LastStatusDate, Application.PaidFees, Application.CreatedByUserID,
                    LicenseClassID);
            }
            else
                return null;
        }
        public static clsLocalDrivingLicenseApplication FindByApplicationID(int ApplicationID)
        {
            int LocalDrivingLicenseApplicationID = -1, LicenseClassID = -1;
            bool IsFound = clsLocalDrivingLicenseApplicationData.GetLocalDrivingLicenseApplicationInfoByApplicationID(ApplicationID,
                ref LocalDrivingLicenseApplicationID, ref LicenseClassID);
            if (IsFound)
            {
                clsApplication Application = clsApplication.FindBaseApplication(ApplicationID); // we also need to find the application first
                if (Application == null) return null;
                return new clsLocalDrivingLicenseApplication(LocalDrivingLicenseApplicationID, Application.ApplicationID,
                    Application.ApplicantPersonID,Application.ApplicationDate, Application.ApplicationTypeID, Application.ApplicationStatus,
                    Application.LastStatusDate,Application.PaidFees, Application.CreatedByUserID, LicenseClassID);
            }
            else return null;
        }
        public bool Save()
        {
            //Because of inheritance first we call the save method in the base class,
            //it will take care of adding all information to the application table.
            base.Mode = (clsApplication.enMode) Mode;
            if (!base.Save())
                return false;
            //After we save the main application then we now save the sub application
            switch (Mode)
            {
                case enMode.AddNew:
                    {
                        if (_AddNewLocalDrivingLicenseApplication())
                        {
                            Mode = enMode.Update;
                            return true;
                        }
                        else
                            return false;
                    }
                case enMode.Update:
                    return _UpdateLocalDrivingLicenseApplication();
            }
            return false;
        }
        public static DataTable GetAllLocalDrivingLicenseApplications()
        {
            return clsLocalDrivingLicenseApplicationData.GetAllLocalDrivingLicenseApplications(); // customized at data layer to serve the data grid view (Presentation Layer)
        }
        public bool Delete()
        {
            // First We Delete The Local Driving License Application
            bool IsLocalDrivingLicenseApplicationDeleted = 
                clsLocalDrivingLicenseApplicationData.DeleteLocalDrivingLicenseApplication(this.LocalDrivingLicenseApplicationID);
            if (!IsLocalDrivingLicenseApplicationDeleted) return false;
            // Then We Delete The Base Application
            bool IsBaseApplicationDeleted = base.Delete();
            return IsBaseApplicationDeleted;
        }
        public bool DoesPassTestType(clsTestType.enTestType TestTypeID)
        {
            return clsLocalDrivingLicenseApplicationData.DoesPassTestType(this.LocalDrivingLicenseApplicationID, (int)TestTypeID);
        }
        public bool DoesPassPreviousTest(clsTestType.enTestType CurrentTestType)
        {
            switch(CurrentTestType)
            {
                case clsTestType.enTestType.VisionTest:
                    return true;
                case clsTestType.enTestType.WrittenTest:
                    return clsLocalDrivingLicenseApplicationData.DoesPassTestType(this.LocalDrivingLicenseApplicationID, (int)clsTestType.enTestType.VisionTest);
                case clsTestType.enTestType.StreetTest:
                    return clsLocalDrivingLicenseApplicationData.DoesPassTestType(this.LocalDrivingLicenseApplicationID, (int)clsTestType.enTestType.WrittenTest); ;
                default:
                    return false;
            }
        }
        public static bool DoesPassTestType(int LocalDrivingLicenseApplicationID, clsTestType.enTestType TesTypeID)
        {
            return clsLocalDrivingLicenseApplicationData.DoesPassTestType(LocalDrivingLicenseApplicationID, (int)TesTypeID);
        }
        public bool DoesAttendTestType(clsTestType.enTestType TestTypeID)
        {
            return clsLocalDrivingLicenseApplicationData.DoesAttendTestType(this.LocalDrivingLicenseApplicationID, (int)TestTypeID);
        }
        public byte TotalAttemptsPerTest(clsTestType.enTestType TestTypeID)
        {
            return clsLocalDrivingLicenseApplicationData.TotalAttemptsPerTest(this.LocalDrivingLicenseApplicationID, (int)TestTypeID);
        }
        public static byte TotalAttemptsPerTest(int LocalDrivingLicenseApplicationID, clsTestType.enTestType TestTypeID)
        {
            return clsLocalDrivingLicenseApplicationData.TotalAttemptsPerTest(LocalDrivingLicenseApplicationID, (int)TestTypeID);
        }
        public static bool AttendedTest(int LocalDrivingLicenseApplicationID, clsTestType.enTestType TestTypeID) // in general does he attended any test?
        {
            return clsLocalDrivingLicenseApplicationData.TotalAttemptsPerTest(LocalDrivingLicenseApplicationID, (int)TestTypeID) > 0;
        }
        public bool AttendedTest(clsTestType.enTestType TestTypeID)
        {
            return clsLocalDrivingLicenseApplicationData.TotalAttemptsPerTest(this.LocalDrivingLicenseApplicationID, (int)TestTypeID) > 0;
        }
        public static bool IsThereAnActiveScheduledTest(int LocalDrivingLicenseApplicationID, clsTestType.enTestType TestTypeID)
        {
            return clsLocalDrivingLicenseApplicationData.IsThereAnActiveScheduledTest(LocalDrivingLicenseApplicationID, (int)TestTypeID);
        }
        public bool IsThereAnActiveScheduledTest(clsTestType.enTestType TestTypeID)
        {
            return clsLocalDrivingLicenseApplicationData.IsThereAnActiveScheduledTest(this.LocalDrivingLicenseApplicationID, (int)TestTypeID);
        }
        public clsTest GetLastTestPerTestType(clsTestType.enTestType TestTypeID)
        {
            return clsTest.FindLastTestPerPersonAndLicenseClass(this.ApplicantPersonID, this.LicenseClassID, TestTypeID);
        }
        public byte GetPassedTestsCount()
        {
            return clsTest.GetPassedTestsCount(this.LocalDrivingLicenseApplicationID);
        }
        public static byte GetPassedTestCount(int LocalDrivingLicenseApplicationID)
        {
            return clsTest.GetPassedTestsCount(LocalDrivingLicenseApplicationID);
        }
        public bool PassedAllTests()
        {
            return clsTest.PassedAllTests(this.LocalDrivingLicenseApplicationID);
        }
        public static bool PassedAllTests(int LocalDrivingLicenseApplicationID)
        {
            return clsTest.PassedAllTests(LocalDrivingLicenseApplicationID);
        }
        public int IssueLicenseForTheFirstTime(string Notes, int CreatedByUserID)
        {
            int DriverID = -1;
            clsDriver Driver = clsDriver.FindByPersonID(this.ApplicantPersonID);
            if (Driver == null)
            {
                //we check if the driver already there for this person.
                Driver = new clsDriver();
                Driver.PersonID = this.ApplicantPersonID;
                Driver.CreatedDate = DateTime.Now;
                Driver.CreatedByUserID = CreatedByUserID;
                if (Driver.Save())
                {
                    DriverID = Driver.DriverID;
                }
                else
                {
                    return -1;
                }
            }
            else
            {
                DriverID = Driver.DriverID;
            }
            //now our diver is there, so we add new license
            clsLicense License = new clsLicense();
            License.ApplicationID = this.ApplicationID;
            License.DriverID = DriverID;
            License.LicenseClassID = this.LicenseClassID;
            License.IssueDate = DateTime.Now;
            License.ExpirationDate = DateTime.Now.AddYears(this.LicenseClassInfo.DefaultValidityLength);
            License.Notes = Notes;
            License.PaidFees = this.LicenseClassInfo.ClassFees;
            License.IsActive = true;
            License.IssueReason = clsLicense.enIssueReason.FirstTime;
            License.CreatedByUserID = CreatedByUserID;
            if (License.Save())
            {
                //now we should set the application status to complete.
                this.SetComplete();
                return License.LicenseID;
            }
            else
                return -1;
        }
        public bool IsLicenseIssued()
        {
            return (GetActiveLicenseID() != -1);
        }
        public int GetActiveLicenseID()
        {//this will get the license id that belongs to this application
            return clsLicense.GetActiveLicenseIDByPersonID(this.ApplicantPersonID, this.LicenseClassID);
        }
    }
}