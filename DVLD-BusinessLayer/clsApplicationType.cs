using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using DVLD_DataLayer;
namespace DVLD_BusinessLayer
{
    public class clsApplicationType
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;
        public int ID {  get; set; }
        public string Title { get; set; }
        public float Fees { get; set; }
        public clsApplicationType()
        {
            ID = -1;
            Title = "";
            Fees = 0;
            Mode =enMode.AddNew;
        }
        public clsApplicationType(int ID, string Title, float Fees)
        {
            this.ID = ID;
            this.Title = Title;
            this.Fees = Fees;
            Mode = enMode.Update;
        }
        private bool _AddNewApplicationType()
        {
            this.ID = clsApplicationTypeData.AddNewApplicationType(this.Title, this.Fees);
            return this.ID != -1;
        }
        public static clsApplicationType Find(int ID)
        {
            string Title = "";
            float Fee = 0;
            bool IsFound = clsApplicationTypeData.GetApplicationTypeInfoByID(ID, ref Title, ref Fee);
            if (IsFound)
                return new clsApplicationType(ID, Title, Fee);
            else
                return null;
        }
        private bool _UpdateApplicationType()
        {
            return clsApplicationTypeData.UpdateApplicationType(this.ID, this.Title, this.Fees);
        }
        public bool Save()
        {
            switch(Mode)
            {
                case enMode.AddNew:
                    {
                        if (_AddNewApplicationType())
                        {
                            Mode = enMode.Update;
                            return true;
                        }
                        return false;
                    }
                case enMode.Update:
                    return _UpdateApplicationType();
            }
            return false;
        }
        public static DataTable GetAllApplicationTypes()
        {
            return clsApplicationTypeData.GetAllApplicationTypes();
        }
    }
}