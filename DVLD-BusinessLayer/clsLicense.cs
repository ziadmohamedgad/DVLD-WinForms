using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DVLD_DataLayer;
namespace DVLD_BusinessLayer
{
    public class clsLicense
    {
        public enum enMode { AddNew = 0, Update = 1 }
        public enMode Mode = enMode.AddNew;
        public enum enIssueReason { FirstTime = 1, Renew = 2, ReplacementForDamaged = 3, ReplacementForLost = 4 };
        public clsDriver DriverInfo;
        public int LicenseID { set; get; }
        public int ApplicationID { set; get; }
        public int DriverID { set; get; }
        public int LicenseClassID { set; get; }
        public clsLicenseClass LicenseClassInfo { set; get; }
        public DateTime IssueDate { set; get; }
        public DateTime ExpirationDate { set; get; }
        public string Notes { set; get; }
        public float PaidFees { set; get; }
        public bool IsActive { set; get; }
        public enIssueReason IssueReason { set; get; }
        public string IssueReasonText
        {
            get
            {
                return GetIssueReasonText(this.IssueReason);
            }
        }
        public clsDetainedLicense DetainedInfo;
        public int CreatedByUserID { set; get; }
        public bool IsDetained
        {
            get
            {
                return clsDetainedLicense.IsLicenseDetained(this.LicenseID);
            }
        }
        public clsLicense()
        {
            this.LicenseID = -1;
            this.ApplicationID = -1;
            this.DriverID = -1;
            this.LicenseClassID = -1;
            this.IssueDate = DateTime.Now;
            this.ExpirationDate = DateTime.Now;
            this.Notes = "";
            this.PaidFees = 0;
            this.IsActive = true;
            this.IssueReason = enIssueReason.FirstTime;
            this.CreatedByUserID = -1;
            Mode = enMode.AddNew;
        }
        public clsLicense(int LicenseID, int ApplicationID, int DriverID, int LicenseClassID, DateTime IssueDate, 
            DateTime ExpirationDate, string Notes, float PaidFees, bool IsActive, enIssueReason IssueReason, int CreatedByUserID)
        {
            this.LicenseID = LicenseID;
            this.ApplicationID = ApplicationID;
            this.DriverID = DriverID;
            this.LicenseClassID = LicenseClassID;
            this.IssueDate = IssueDate; 
            this.ExpirationDate = ExpirationDate;
            this.Notes = Notes;
            this.PaidFees = PaidFees;
            this.IsActive = IsActive;
            this.IssueReason = IssueReason;
            this.CreatedByUserID = CreatedByUserID;
            this.DriverInfo = clsDriver.FindByDriverID(this.DriverID);
            this.LicenseClassInfo = clsLicenseClass.Find(this.LicenseClassID);
            this.DetainedInfo = clsDetainedLicense.FindByLicenseID(this.LicenseID);
            Mode = enMode.Update;
        }
        private bool _AddNewLicense()
        {
            this.LicenseID = clsLicenseData.AddNewLicense(this.ApplicationID, this.DriverID, this.LicenseClassID,
                this.IssueDate, this.ExpirationDate, this.Notes, this.PaidFees, this.IsActive, (byte)this.IssueReason, this.CreatedByUserID);
            return this.LicenseID != -1;
        }
        public bool _UpdateLicense()
        {
            return clsLicenseData.UpdateLicense(this.LicenseID, this.ApplicationID, this.DriverID, this.LicenseClassID,
                this.IssueDate, this.ExpirationDate, this.Notes, this.PaidFees, this.IsActive, (byte)this.IssueReason, this.CreatedByUserID);
        }
        public static clsLicense Find(int LicenseID)
        {
            int ApplicationID = -1, DriverID = -1, LicenseClassID = -1, CreatedByUserID = -1;
            DateTime IssueDate = DateTime.Now, ExpirationDate = DateTime.Now;
            string Notes = "";
            float PaidFees = -1;
            bool IsActive = false;
            byte IssueReason = 0;
            bool IsFound = clsLicenseData.GetLicenseInfoByID(LicenseID, ref ApplicationID, ref DriverID, ref LicenseClassID,
                ref IssueDate, ref ExpirationDate, ref Notes, ref PaidFees, ref IsActive, ref IssueReason, ref CreatedByUserID);
            if (IsFound)
                return new clsLicense(LicenseID, ApplicationID, DriverID, LicenseClassID, IssueDate, ExpirationDate,
                    Notes, PaidFees, IsActive, (enIssueReason)IssueReason, CreatedByUserID);
            else
                return null;
        }
        public static DataTable GetAllLicenses()
        {
            return clsLicenseData.GetAllLicenses();
        }
        public bool Save()
        {
            switch(Mode)
            {
                case enMode.AddNew:
                    {
                        if (_AddNewLicense())
                        {
                            Mode = enMode.Update;
                            return true;
                        }
                        else
                            return false;
                    }
                case enMode.Update:
                    return _UpdateLicense();
            }
            return false;
        }
        public static bool IsLicenseExistByPersonID(int PersonID, int LicenseClassID)
        {
            return clsLicenseData.GetActiveLicenseIDByPersonID(PersonID, LicenseClassID) != -1;
        }
        public static int GetActiveLicenseIDByPersonID(int PersonID, int LicenseClassID)
        {
            return clsLicenseData.GetActiveLicenseIDByPersonID(PersonID, LicenseClassID);
        }
        public static DataTable GetDriverLicenses(int DriverID)
        {
            return clsLicenseData.GetDriverLicenses(DriverID);
        }
        public bool IsLicenseExpired()
        {
            return this.ExpirationDate < DateTime.Now;
        }
        public bool DeactivateCurrentLicense()
        {
            return clsLicenseData.DeactivateLicense(this.LicenseID);
        }
        public string GetIssueReasonText(enIssueReason Reason)
        {
            switch(Reason)
            {
                case enIssueReason.FirstTime:
                    return "First Time";
                case enIssueReason.Renew:
                    return "Renew";
                case enIssueReason.ReplacementForDamaged:
                    return "Replacement For Damaged";
                case enIssueReason.ReplacementForLost:
                    return "Replacement For Lost";
                default:
                    return "First Time";
            }
        }
        public int Detain(float FineFees, int CreatedByUserID)
        {
            clsDetainedLicense DetainedLicense = new clsDetainedLicense();
            DetainedLicense.LicenseID = this.LicenseID;
            DetainedLicense.DetainDate = DateTime.Now;
            DetainedLicense.FineFees = Convert.ToSingle(FineFees);
            DetainedLicense.CreatedByUserID = CreatedByUserID;
            if (!DetainedLicense.Save())
                return -1;
            return DetainedLicense.DetainID;
        }
        public bool ReleaseDetainedLicense(int ReleaseByUserID, ref int ApplicationID)
        {
            clsApplication Application = new clsApplication();
            Application.ApplicantPersonID = this.DriverInfo.PersonID;
            Application.ApplicationDate = DateTime.Now;
            Application.ApplicationTypeID = (int)clsApplication.enApplicationType.ReleaseDetainedDrivingLicenses;
            Application.ApplicationStatus = clsApplication.enApplicationStatus.Completed;
            Application.LastStatusDate = DateTime.Now;
            Application.PaidFees = clsApplicationType.Find((int)clsApplication.enApplicationType.ReleaseDetainedDrivingLicenses).Fees;
            Application.CreatedByUserID = ReleaseByUserID;
            if (!Application.Save())
            {
                ApplicationID = -1;
                return false;
            }
            ApplicationID = Application.ApplicationID;
            return this.DetainedInfo.ReleaseDetainedLicense(ReleaseByUserID, ApplicationID); 
        }
        public clsLicense RenewLicense(string Notes, int CreatedByUserID)
        {
            clsApplication Application = new clsApplication();
            Application.ApplicantPersonID = this.DriverInfo.PersonID;
            Application.ApplicationDate = DateTime.Now;
            Application.ApplicationTypeID = (int)clsApplication.enApplicationType.RenewDrivingLicense;
            Application.ApplicationStatus = clsApplication.enApplicationStatus.Completed;
            Application.LastStatusDate = DateTime.Now;
            Application.PaidFees = clsApplicationType.Find((int)clsApplication.enApplicationType.RenewDrivingLicense).Fees;
            Application.CreatedByUserID = CreatedByUserID;
            if (!Application.Save())
                return null;
            clsLicense NewLicense = new clsLicense();
            NewLicense.ApplicationID = Application.ApplicationID;
            NewLicense.DriverID = this.DriverID; // same driver with the past license
            NewLicense.LicenseClassID = this.LicenseClassID;
            NewLicense.IssueDate = DateTime.Now;
            NewLicense.ExpirationDate = DateTime.Now.AddYears(this.LicenseClassInfo.DefaultValidityLength);
            NewLicense.Notes = Notes;
            NewLicense.PaidFees = this.LicenseClassInfo.ClassFees;
            NewLicense.IsActive = true;
            NewLicense.IssueReason = clsLicense.enIssueReason.Renew;
            NewLicense.CreatedByUserID = CreatedByUserID;
            if (!NewLicense.Save()) 
                return null;
            // we have to deactivate the old license.
            DeactivateCurrentLicense();
            return NewLicense;
        }
        public clsLicense Replace(enIssueReason IssueReason, int CreatedByUserID)
        {
            if (IssueReason == enIssueReason.FirstTime) return null;
            if (this.ExpirationDate < DateTime.Now || IssueReason == enIssueReason.Renew)
            {
                RenewLicense(IssueReason.ToString(), CreatedByUserID);
            }
            clsApplication Application = new clsApplication();
            Application.ApplicantPersonID = this.DriverInfo.PersonID;
            Application.ApplicationDate = DateTime.Now;
            Application.ApplicationTypeID = IssueReason == enIssueReason.ReplacementForDamaged ? 
                (int)clsApplication.enApplicationType.ReplaceDamagedDrivingLicense : 
                (int)clsApplication.enApplicationType.ReplaceLostDrivingLicense;
            Application.ApplicationStatus = clsApplication.enApplicationStatus.Completed;
            Application.LastStatusDate = DateTime.Now;
            Application.PaidFees = clsApplicationType.Find(Application.ApplicationTypeID).Fees;
            Application.CreatedByUserID = CreatedByUserID;
            if (!Application.Save()) 
                return null;
            clsLicense NewLicense = new clsLicense();
            NewLicense.ApplicationID = Application.ApplicationID;
            NewLicense.DriverID = this.DriverID;
            NewLicense.LicenseClassID = this.LicenseClassID;
            NewLicense.IssueDate = DateTime.Now;
            NewLicense.ExpirationDate = this.ExpirationDate; // continue as the lost or damaged
            NewLicense.Notes = this.Notes;
            NewLicense.PaidFees = 0; // no fees because it's a replacement
            NewLicense.IsActive = true;
            NewLicense.IssueReason = IssueReason;
            NewLicense.CreatedByUserID = CreatedByUserID;
            if (!NewLicense.Save())
                return null;
            DeactivateCurrentLicense();
            return NewLicense;
        }
    }
}