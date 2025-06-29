using DVLD_DataAccess;
using DVLD_DataLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace DVLD_BusinessLayer
{
    public class clsDetainedLicense
    {
        public enum enMode { AddNew = 0, Update =  1 }
        public enMode Mode = enMode.AddNew;
        public int DetainID { set; get; }
        public int LicenseID { set; get; }
        public DateTime DetainDate { set; get; }
        public float FineFees { set; get; }
        public int CreatedByUserID { set; get; }
        public clsUser CreatedByUserInfo { set; get; }
        public bool IsReleased { set; get; }
        public DateTime ReleaseDate { set; get; }
        public int ReleasedByUserID { set; get; }
        public clsUser ReleasedByUserInfo { set; get; }
        public int ReleaseApplicationID { set; get; }
        public clsDetainedLicense()
        {
            this.DetainID = -1;
            this.LicenseID = -1;
            this.DetainDate = DateTime.Now;
            this.FineFees = -1;
            this.CreatedByUserID = -1;
            this.IsReleased = false;
            this.ReleaseDate = DateTime.MaxValue;
            this.ReleasedByUserID = -1;
            this.ReleaseApplicationID = -1;
            this.CreatedByUserInfo = null;
            this.ReleasedByUserInfo = null;
            Mode = enMode.AddNew;
        }
        public clsDetainedLicense(int DetaindID, int LicenseID, DateTime DetainDate, float FineFees,
            int CreatedByUserID, bool IsReleased, DateTime ReleaseDate, int ReleasedByUserID, int ReleaseApplicationID)
        {
            this.DetainID = DetaindID;
            this.LicenseID = LicenseID;
            this.DetainDate = DetainDate;
            this.FineFees = FineFees;
            this.CreatedByUserID = CreatedByUserID;
            this.CreatedByUserInfo = clsUser.FindByUserID(this.CreatedByUserID);
            this.IsReleased = IsReleased;
            this.ReleaseDate = ReleaseDate;
            this.ReleasedByUserID = ReleasedByUserID;
            this.ReleaseApplicationID = ReleaseApplicationID;
            this.ReleasedByUserInfo = clsUser.FindByUserID(this.ReleasedByUserID);
            Mode = enMode.Update;
        }
        private bool _AddNewDetainedLicense()
        {
            this.DetainID = clsDetainedLicenseData.AddNewDetainedLicense(this.LicenseID, this.DetainDate, this.FineFees,
                this.CreatedByUserID);
            return this.DetainID != -1;
        }
        private bool _UpdateDetainedLicense()
        {
            return clsDetainedLicenseData.UpdateDetainedLicense(this.DetainID, this.LicenseID, this.DetainDate, this.FineFees,
                this.CreatedByUserID);
        }
        public static clsDetainedLicense Find(int DetainID)
        {
            int LicenseID = -1, ReleasedByUserID = -1, ReleaseApplicationID = -1, CreatedByUserID = -1;
            DateTime DetainDate = DateTime.Now, ReleaseDate = DateTime.MaxValue;
            float FineFees = 0;
            bool IsReleased = false;
            bool IsFound = clsDetainedLicenseData.GetDetainedLicenseInfoByID(DetainID, ref LicenseID, ref DetainDate,
                ref FineFees, ref CreatedByUserID, ref IsReleased, ref ReleaseDate, ref ReleasedByUserID, ref ReleaseApplicationID);
            if (IsFound)
                return new clsDetainedLicense(DetainID, LicenseID, DetainDate, FineFees, CreatedByUserID, IsReleased, ReleaseDate,
                    ReleasedByUserID, ReleaseApplicationID);
            else
                return null;
        }
        public static clsDetainedLicense FindByLicenseID(int LicenseID)
        {
            int DetainID = -1, ReleasedByUserID = -1, ReleaseApplicationID = -1, CreatedByUserID = -1;
            DateTime DetainDate = DateTime.Now, ReleaseDate = DateTime.MaxValue;
            float FineFees = 0;
            bool IsReleased = false;
            bool IsFound = clsDetainedLicenseData.GetDetainedLicenseInfoByLicenseID(LicenseID, ref DetainID, ref DetainDate,
                ref FineFees, ref CreatedByUserID, ref IsReleased, ref ReleaseDate, ref ReleasedByUserID, ref ReleaseApplicationID);
            if (IsFound)
                return new clsDetainedLicense(DetainID, LicenseID, DetainDate, FineFees, CreatedByUserID, IsReleased, ReleaseDate,
                    ReleasedByUserID, ReleaseApplicationID);
            else
                return null;
        }
        public static DataTable GetAllDetainedLicenses()
        {
            return clsDetainedLicenseData.GetAllDetainedLicenses();
        }
        public bool Save()
        {
            switch(Mode)
            {
                case enMode.AddNew:
                    {
                        if (_AddNewDetainedLicense())
                        {
                            Mode = enMode.Update;
                            return true;
                        }
                        else
                            return false;
                    }
                case enMode.Update:
                    return _UpdateDetainedLicense();
            }
            return false;
        }
        public static bool IsLicenseDetained(int LicenseID)
        {
            return clsDetainedLicenseData.IsLicenseDetained(LicenseID);
        }
        public bool ReleaseDetainedLicense(int ReleasedByUserID, int ReleaseApplicationID)
        {
            if (clsDetainedLicenseData.ReleaseDetainedLicense(this.DetainID, ReleasedByUserID, ReleaseApplicationID))
            {
                this.IsReleased = true;
                return true;
            }
            return false;
        }
    }
}