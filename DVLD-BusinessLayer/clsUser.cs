using DVLD_DataLayer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
namespace DVLD_BusinessLayer
{
    public class clsUser
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;
        public int UserID { set; get; }
        public int PersonID { set; get; }
        public string UserName { set; get; }
        public string HashedPassword { set; get; }
        public bool IsActive { set; get; }
        public string PasswordSalt {  set; get; }
        public clsPerson PersonInfo;
        public clsUser()
        {
            this.UserID = -1;
            this.UserName = "";
            this.IsActive = true;
            this.HashedPassword = "";
            this.PasswordSalt = "";
            this.PersonInfo = null;
            Mode = enMode.AddNew;
        }
        public clsUser(int UserID, int PersonID, string UserName, string HashedPassword, bool IsActive, string PasswordSalt)
        {
            this.UserID = UserID;
            this.PersonID = PersonID;
            this.UserName = UserName;
            this.HashedPassword = HashedPassword;
            this.IsActive = IsActive;
            this.PasswordSalt = PasswordSalt;
            this.PersonInfo = clsPerson.Find(PersonID);
            Mode = enMode.Update;
        }
        public static DataTable GetAllUsers()
        {
            return clsUserData.GetAllUsers();
        }
        public static clsUser FindByUsernameAndHashedPassword(string UserName, string HashedPassword)
        {
            int UserID = -1;
            int PersonID = -1;
            bool IsActive = false;
            string PasswordSalt = "";
            bool IsFound = clsUserData.GetUserInfoByUserNameAndHashedPassword(UserName, HashedPassword, ref UserID,
                ref PersonID, ref  IsActive, ref PasswordSalt);
            if (IsFound)
                return new clsUser(UserID, PersonID, UserName, HashedPassword, IsActive, PasswordSalt);
            else
                return null;
        }
        public static clsUser FindByUserID(int UserID)
        {
            int PersonID = -1;
            string UserName = "", HashedPassword = "";
            bool IsActive = false;
            string PasswordSalt = "";
            bool IsFound = clsUserData.GetUserInfoByUserID(UserID, ref PersonID, ref UserName,
                ref HashedPassword, ref IsActive, ref PasswordSalt);
            if (IsFound)
                return new clsUser(UserID, PersonID, UserName, HashedPassword, IsActive, PasswordSalt);
            else
                return null;
        }
        public static clsUser FindByPersonID(int PersonID)
        {
            int UserID = -1;
            string UserName = "", HashedPassword = "";
            bool IsActive = false;
            string PasswordSalt = "";
            bool IsFound = clsUserData.GetUserInfoByPersonID(PersonID, ref UserID, ref UserName,
                ref HashedPassword, ref IsActive, ref PasswordSalt);
            if (IsFound)
                return new clsUser(UserID, PersonID, UserName, HashedPassword, IsActive, PasswordSalt);
            else
                return null;
        }
        private bool _AddNewUser()
        {
             this.UserID = clsUserData.AddNewUser(this.PersonID, this.UserName, this.HashedPassword, this.IsActive, this.PasswordSalt);
            return this.UserID != -1;
        }
        private bool _UpdateUser()
        {
            return clsUserData.UpdateUser(this.UserID, this.PersonID, this.UserName, this.HashedPassword, this.IsActive, this.PasswordSalt);
        }
        public static bool DeleteUser(int UserID)
        {
            return clsUserData.DeleteUser(UserID);
        }
        public bool Save()
        {
            switch(Mode)
            {
                case enMode.AddNew:
                    {
                        if (_AddNewUser())
                        {
                            Mode = enMode.Update;
                            return true;
                        }
                        else
                            return false;
                    }
                case enMode.Update:
                    {
                        return _UpdateUser();
                    }
            }
            return false;
        }
        public static bool IsUserExist(int UserID)
        {
            return clsUserData.IsUserExist(UserID);
        }
        public static bool IsUserExist(string UserName)
        {
            return clsUserData.IsUserExist(UserName);
        }
        public static bool IsUserExistForPersonID(int PersonID)
        {
            return clsUserData.IsUserExistForPersonID((int)PersonID);
        }
        public static bool ChangePassword(int UserID, string HashedPassword, string PasswordSalt)
        {
            return clsUserData.ChangePassword(UserID, HashedPassword, PasswordSalt);
        }
        public static string GetPasswordSaltByUserName(string Username)
        {
            return clsUserData.GetPasswordSaltByUsername(Username);
        }
    }
}