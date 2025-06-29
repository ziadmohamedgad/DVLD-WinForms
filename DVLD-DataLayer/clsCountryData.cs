using DVLD_DataAccess;
using DVLD_Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
namespace DVLD_DataLayer
{
    public class clsCountryData
    {
        public static bool GetCountryInfoByID(int ID, ref string CountryName)
        {
            bool IsFound = false;
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string Query = @"SELECT * FROM Countries WHERE CountryID = @CountryID";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@CountryID", ID);
            try
            {
                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();
                if (Reader.Read())
                {
                    IsFound = true;
                    CountryName = (string)Reader["CountryName"];
                }
                else
                    IsFound = false;
                Reader.Close();
            }
            catch(SqlException ex)
            {
                IsFound = false;
                clsEventLogger.SaveLog("Application", $"{ex.Message}: failed through fetching " +
                    $"country info with ID = {ID}.", EventLogEntryType.Error);
            }
            finally
            {
                Connection.Close();
            }
            return IsFound;
        }
        public static bool GetCountryInfoByName(string CountryName, ref int ID)
        {
            bool IsFound = false;
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string Query = @"SELECT * FROM Countries WHERE CountryName = @CountryName";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@CountryName", CountryName);
            try
            {
                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();
                if (Reader.Read())
                {
                    IsFound = true;
                    ID = (int)Reader["CountryID"];
                }
                else
                    IsFound = false;
                Reader.Close();
            }
            catch (SqlException ex)
            {
                IsFound = false;
                clsEventLogger.SaveLog("Application", $"{ex.Message}: failed through fetching " +
                    $"country info with name = {CountryName}.", EventLogEntryType.Error);
            }
            finally
            {
                Connection.Close();
            }
            return IsFound;
        }
        public static DataTable GetAllCountries()
        {
            DataTable dt = new DataTable();
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string Query = @"SELECT * FROM Countries ORDER BY Countries.CountryName";
            SqlCommand Commadn = new SqlCommand(Query, Connection);
            try
            {
                Connection.Open();
                SqlDataReader Reader = Commadn.ExecuteReader();
                if (Reader.HasRows)
                {
                    dt.Load(Reader);
                }
                Reader.Close();
            }
            catch (SqlException ex)
            {
                dt = null;
                clsEventLogger.SaveLog("Application", $"{ex.Message}: failed through fetching" +
                    $"all countries.", EventLogEntryType.Error);
            }
            finally
            {
                Connection.Close();
            }
            return dt;
        }
    }
}