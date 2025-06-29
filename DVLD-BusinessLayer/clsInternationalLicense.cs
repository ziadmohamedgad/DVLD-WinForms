using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DVLD_DataLayer;
namespace DVLD_BusinessLayer
{
    public class clsInternationalLicense : clsApplication
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;
        public int InternationalLicenseID { set; get; }
        public int DriverID { set; get; }
        public clsDriver DriverInfo;
        public int IssuedUsingLocalLicenseID { set; get; }
        public DateTime IssueDate { set; get; }
        public DateTime ExpirationDate { set; get; }
        public bool IsActive { set; get; }
        public clsInternationalLicense()
        {
            this.ApplicationTypeID = (int)clsApplication.enApplicationType.NewInternationalLicense;
            this.InternationalLicenseID = -1;
            this.DriverID = -1;
            this.DriverInfo = null;
            this.IssuedUsingLocalLicenseID = -1;
            this.IssueDate = DateTime.Now;
            this.ExpirationDate = DateTime.Now;
            this.IsActive = true;
            Mode = enMode.AddNew;
        }
        public clsInternationalLicense(int ApplicationID, int ApplicantPersonID, DateTime ApplicationDate, 
            clsApplication.enApplicationStatus ApplicationStatus, DateTime LastStatusDate, float PaidFees,
            int CreatedByUserID, int InternationalLicenseID, int DriverID, int IssuedUsingLocalLicenseID,
            DateTime IssueDate, DateTime ExpirationDate, bool IsActive)
        {
            this.ApplicationTypeID = (int)clsApplication.enApplicationType.NewInternationalLicense;
            this.ApplicationID = ApplicantPersonID;
            this.ApplicantPersonID = ApplicationID;
            this.ApplicationDate = ApplicationDate;
            this.ApplicationStatus = ApplicationStatus;
            this.LastStatusDate = LastStatusDate;
            this.PaidFees = PaidFees;
            this.CreatedByUserID = CreatedByUserID;
            this.InternationalLicenseID = InternationalLicenseID;
            this.DriverID = DriverID;
            this.IssuedUsingLocalLicenseID = IssuedUsingLocalLicenseID;
            this.IssueDate = IssueDate;
            this.ExpirationDate = ExpirationDate;
            this.IsActive = IsActive;
            this.DriverInfo = clsDriver.FindByDriverID(this.DriverID);
            Mode = enMode.Update;
        }
        private bool _AddNewInternationalLicense()
        {
            this.InternationalLicenseID = clsInternationalLicenseData.AddNewInternationalLicense(this.ApplicationID, this.DriverID,
                this.IssuedUsingLocalLicenseID, this.IssueDate, this.ExpirationDate, this.IsActive, this.CreatedByUserID);
            return this.InternationalLicenseID != -1;
        }
        private bool _UpdateInternationalLicense()
        {
            return clsInternationalLicenseData.UpdateInternationalLicense(this.InternationalLicenseID, this.ApplicationID, this.DriverID,
                this.IssuedUsingLocalLicenseID, this.IssueDate, this.ExpirationDate, this.IsActive, this.CreatedByUserID);
        }
        public static clsInternationalLicense Find(int InternationalLicenseID)
        {
            int ApplicationID = -1, DriverID = -1, IssuedUsingLocalDrivingLicenseID = -1, CreatedByUserID = -1;
            DateTime IssueDate = DateTime.Now, ExpirationDate = DateTime.Now;
            bool IsActive = false;
            bool IsFound = clsInternationalLicenseData.GetInternationalLicenseInfoByID(InternationalLicenseID, ref ApplicationID,
                ref DriverID, ref IssuedUsingLocalDrivingLicenseID, ref IssueDate, ref ExpirationDate, ref IsActive, ref CreatedByUserID);
            if (IsFound)
            {
                clsApplication Application = clsApplication.FindBaseApplication(ApplicationID);
                return new clsInternationalLicense(Application.ApplicationID, Application.ApplicantPersonID, Application.ApplicationDate,
                    (clsApplication.enApplicationStatus)Application.ApplicationStatus, Application.LastStatusDate, Application.PaidFees,
                    Application.CreatedByUserID, InternationalLicenseID, DriverID, IssuedUsingLocalDrivingLicenseID,
                    IssueDate, ExpirationDate, IsActive);
            }
            else
                return null;
        }
        public static DataTable GetAllInternationalLicenses()
        {
            return clsInternationalLicenseData.GetAllInternationalLicenses();
        }
        public bool Save()
        {
            base.Mode = (clsApplication.enMode)Mode;
            if (!base.Save())
                return false;
            switch (Mode)
            {
                case enMode.AddNew:
                    {
                        if (_AddNewInternationalLicense())
                        {
                            Mode = enMode.Update;
                            return true;
                        }
                        else
                            return false;
                    }
                case enMode.Update:
                    return _UpdateInternationalLicense();
            }
            return false;
        }
        public static int GetActiveInternationalLicenseIDByDriverID(int DriverID)
        {
            return clsInternationalLicenseData.GetActiveInternationalLicenseIDByDriverID(DriverID);
        }
        public static DataTable GetDriverInternationalLicenses(int DriverID)
        {
            return clsInternationalLicenseData.GetDriverInternationalLicenses(DriverID);
        }
    }
}