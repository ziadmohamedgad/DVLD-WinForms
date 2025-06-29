using DVLD_DataLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
namespace DVLD_BusinessLayer
{
    public class clsTestType
    {
        public enum enMode { AddNew = 0, Update = 1 }
        public enMode Mode = enMode.AddNew;
        public enum enTestType { VisionTest = 1, WrittenTest = 2, StreetTest = 3 };
        public clsTestType.enTestType ID { set ; get; }
        public string Title { set; get; }
        public string Description { set; get; }
        public float Fees { set; get; }
        public clsTestType()
        {
            this.ID = clsTestType.enTestType.VisionTest;
            this.Title = "";
            this.Description = "";
            this.Fees = 0;
            Mode = enMode.AddNew;
        }
        public clsTestType(clsTestType.enTestType ID,  string Title, string Description, float Fees)
        {
            this.ID = ID;
            this.Title = Title;
            this.Description = Description;
            this.Fees = Fees;
            Mode = enMode.Update;
        }
        private bool _AddNew()
        {
            this.ID =(clsTestType.enTestType)clsTestTypeData.AddNewTestType(this.Title, this.Description, this.Fees);
            return this.Title != "";
        }
        private bool _Update()
        {
            return clsTestTypeData.UpdateTestType((int)this.ID, this.Title, this.Description, this.Fees);
        }
        public static clsTestType Find(clsTestType.enTestType ID)
        {
            string Title = "", Description = "";
            float Fees = 0;
            bool IsFound = clsTestTypeData.GetTestTypeInfoByID((int)ID, ref Title, ref Description, ref Fees);
            if (IsFound)
                return new clsTestType(ID, Title, Description, Fees);
            else
                return null;
        }
        public bool Save()
        {
            switch(Mode)
            {
                case enMode.AddNew:
                    {
                        if (_AddNew())
                        {
                            Mode = enMode.Update;
                            return true;
                        }
                        return false;
                    }
                case enMode.Update:
                    return _Update();
            }
            return false;
        }
        public static DataTable GetAllTestTypes()
        {
            return clsTestTypeData.GetAllTestTypes();
        }
    }
}