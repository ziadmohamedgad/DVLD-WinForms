using DVLD_DataLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
namespace DVLD_BusinessLayer
{
    public class clsCountry
    {
        public int CountryID { get; set; }
        public string CountryName { get; set; }
        public clsCountry()
        {
            CountryID = -1;
            CountryName = "";
        }
        public clsCountry(int CountryID, string CountryName)
        {
            this.CountryID = CountryID;
            this.CountryName = CountryName;
        }
        public static DataTable GetAllCountries()
        {
            return clsCountryData.GetAllCountries();
        }
        public static clsCountry Find(int ID)
        {
            string CountryName = "";
            bool IsFound = clsCountryData.GetCountryInfoByID(ID, ref CountryName);
            if (IsFound)
                return new clsCountry(ID,  CountryName);
            else 
                return null;
        }
        public static clsCountry Find(string CountryName)
        {
            int ID = -1;
            bool IsFound = clsCountryData.GetCountryInfoByName(CountryName, ref ID);
            if (IsFound)
                return new clsCountry(ID, CountryName);
            else
                return null;
        }
    }
}