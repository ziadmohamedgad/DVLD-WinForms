using System;
using System.Data;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using DVLD_DataLayer;
namespace DVLD_BusinessLayer
{
    public class clsPerson
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;
        public int PersonID { set; get; }
        public string NationalNo { set; get; }
        public string FirstName { set; get; }
        public string SecondName { set; get; }
        public string ThirdName { set; get; }
        public string LastName { set; get; }
        public string FullName
        {
            get
            {
                return FirstName + " " + SecondName + " " + ThirdName + " " + LastName;
            }
        }
        public DateTime DateOfBirth { set; get; }
        public byte Gender { set; get; }
        public string Address {  set; get; }
        public string Phone { set; get; }
        public string Email { set; get; }
        public int NationalityCountryID { set; get; }
        private string _ImagePath;
        public string ImagePath
        {
            get { return _ImagePath; }
            set { _ImagePath = value; }
        }
        public clsCountry CountryInfo;
        public clsPerson()
        {
            this.PersonID = -1;
            this.NationalNo = "";
            this.FirstName = "";
            this.SecondName = "";
            this.ThirdName = "";
            this.LastName = "";
            this.DateOfBirth = DateTime.Now;
            this.Gender = 3;
            this.Address = "";
            this.Phone = "";
            this.Email = "";
            this.NationalityCountryID = -1;
            this.ImagePath = "";
            CountryInfo = null;
            Mode = enMode.AddNew;
        }
        public clsPerson(int PersonID, string NationalNo, string FirstName, string SecondName, string ThirdName,
            string LastName, DateTime DateOfBirth, byte Gender, string Address, string Phone, string Email,
            int NationalityCountryID, string ImagePath)
        {
            this.PersonID = PersonID;
            this.NationalNo = NationalNo;
            this.FirstName = FirstName;
            this.SecondName = SecondName;
            this.ThirdName = ThirdName;
            this.LastName = LastName;
            this.DateOfBirth = DateOfBirth;
            this.Gender = Gender;
            this.Address = Address;
            this.Phone = Phone;
            this.Email = Email;
            this.NationalityCountryID = NationalityCountryID;
            this.ImagePath = ImagePath;
            CountryInfo = clsCountry.Find(NationalityCountryID);
            Mode = enMode.Update;
        }
        private bool _AddNewPerson()
        {
            this.PersonID = clsPersonData.AddNewPerson(
                this.FirstName, this.SecondName, this.ThirdName,
                this.LastName, this.NationalNo,
                this.DateOfBirth, this.Gender, this.Address, this.Phone, this.Email,
                this.NationalityCountryID, this.ImagePath);
            return (this.PersonID != -1);
        }
        private bool _UpdatePerson()
        {
            return clsPersonData.UpdatePerson(
                this.PersonID, this.FirstName, this.SecondName, this.ThirdName,
                this.LastName, this.NationalNo, this.DateOfBirth, this.Gender,
                this.Address, this.Phone, this.Email,
                  this.NationalityCountryID, this.ImagePath);
        }
        public static clsPerson Find(int PersonID)
        {
            string NationalNo = "", FirstName = "", SecondName = "", ThirdName = "", LastName = "", Address = "",
                Phone = "", Email = "", ImagePath = "";
            DateTime DateOfBirth = DateTime.Now;
            int NationalityCountryID = -1;
            byte Gender = 3;
            bool IsFound = clsPersonData.GetPersonInfoByID(PersonID, ref NationalNo, ref FirstName, ref SecondName, ref ThirdName,
                ref LastName, ref DateOfBirth, ref Gender, ref Address, ref Phone, ref Email, ref NationalityCountryID,
                ref ImagePath);
            if (IsFound)
                return new clsPerson(PersonID, NationalNo, FirstName, SecondName, ThirdName, LastName, DateOfBirth,
                    Gender, Address, Phone, Email, NationalityCountryID, ImagePath);
            else return null;
        }
        public static clsPerson Find(string NationalNo)
        {
            string FirstName = "", SecondName = "", ThirdName = "", LastName = "", Address = "",
            Phone = "", Email = "", ImagePath = "";
            DateTime DateOfBirth = DateTime.Now;
            int NationalityCountryID = -1, PersonID = -1;
            byte Gender = 3;
            bool IsFound = clsPersonData.GetPersonInfoByNationalNo(NationalNo, ref PersonID, ref FirstName, ref SecondName, ref ThirdName,
                ref LastName, ref DateOfBirth, ref Gender, ref Address, ref Phone, ref Email, ref NationalityCountryID,
                ref ImagePath);
            if (IsFound)
                return new clsPerson(PersonID, NationalNo, FirstName, SecondName, ThirdName, LastName, DateOfBirth,
                    Gender, Address, Phone, Email, NationalityCountryID, ImagePath);
            else return null;
        }
        public bool Save()
        {
            switch(Mode)
            {
                case enMode.AddNew:
                    {
                        if (_AddNewPerson())
                        {
                            Mode = enMode.Update;
                            return true;
                        }
                        else
                            return false;
                    }
                case enMode.Update:
                    return _UpdatePerson();
            }
            return false;
        }
        public static DataTable GetAllPeople()
        {
            return clsPersonData.GetAllPeople();
        }
        public static bool DeletePerson(int PersonID)
        {
            return clsPersonData.DeletePerson(PersonID);
        }
        public static bool IsPersonExist(int PersonID)
        {
            return clsPersonData.IsPersonExist(PersonID);
        }
        public static bool IsPersonExist(string NationalNo)
        {
            return clsPersonData.IsPersonExist(NationalNo);
        }
    }
}