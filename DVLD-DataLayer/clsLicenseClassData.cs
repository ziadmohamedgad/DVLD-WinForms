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
namespace DVLD_DataLayer
{
    public class clsLicenseClassData
    {
        public static DataTable GetAllLicenseClasses()
        {
            DataTable dt = new DataTable();
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string Query = @"SELECT * FROM LicenseClasses ORDER BY ClassName";
            SqlCommand Command = new SqlCommand(Query, Connection);
            try
            {
                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();
                if (Reader.HasRows)
                    dt.Load(Reader);
            }
            catch (SqlException ex)
            {
                dt = null;
                clsEventLogger.SaveLog("Application", $"{ex.Message}: failed through fetching " +
                    $"all license classes.", EventLogEntryType.Error);
            }
            finally
            {
                Connection.Close();
            }
            return dt;
        }
        public static bool GetLicenseClassInfoByID(int ID, ref string ClassName, ref string ClassDescription,
            ref byte MinimumAllowedAge, ref byte DefaultValidityLength, ref float ClassFees)
        {
            bool IsFound = false;
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string Query = @"SELECT * FROM LicenseClasses WHERE LicenseClassID = @ID";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@ID", ID);
            try
            {
                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();
                if (Reader.Read())
                {
                    IsFound = true;
                    ClassName = (string)Reader["ClassName"];
                    ClassDescription = (string)Reader["ClassDescription"];
                    MinimumAllowedAge = (byte)Reader["MinimumAllowedAge"];
                    DefaultValidityLength = (byte)Reader["DefaultValidityLength"];
                    ClassFees = Convert.ToSingle(Reader["ClassFees"]);
                }
                Reader.Close();
            }
            catch (SqlException ex)
            {
                IsFound = false;
                clsEventLogger.SaveLog("Application", $"{ex.Message}: failed through fetching " +
                    $"license class info with license class ID = {ID}.", EventLogEntryType.Error);
            }
            finally
            {
                Connection.Close();
            }
            return IsFound;
        }
        public static bool GetLicenseClassInfoByClassName(string ClassName, ref int ID, ref string ClassDescription,
            ref byte MinimumAllowedAge, ref byte DefaultValidityLength, ref float ClassFees)
        {
            bool IsFound = false;
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string Query = @"SELECT * FROM LicenseClasses WHERE ClassName = @ClassName";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@ClassName", ClassName);
            try
            {
                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();
                if (Reader.Read())
                {
                    IsFound = true;
                    ID = (int)Reader["LicenseClassID"];
                    ClassDescription = (string)Reader["ClassDescription"];
                    MinimumAllowedAge = (byte)Reader["MinimumAllowedAge"];
                    DefaultValidityLength = (byte)Reader["DefaultValidityLength"];
                    ClassFees = Convert.ToSingle(Reader["ClassFees"]);
                }
                Reader.Close();
            }
            catch (SqlException ex)
            {
                IsFound = false;
                clsEventLogger.SaveLog("Application", $"{ex.Message}: failed through fetching " +
                    $"license class info with class name = {ClassName}.", EventLogEntryType.Error);
            }
            finally
            {
                Connection.Close();
            }
            return IsFound;
        }
        public static int AddNewLicenseClass(string ClassName, string ClassDescription, byte MinimumAlloweAge,
            byte DefaultValidityLength, float ClassFees)
        {
            int LicenseClassID = -1;
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string Query = @"INSERT INTO LicenseCalsses (ClassName, ClassDescription, MinimumAllowedAge, 
                                                         DefaultValidityLength, ClassFees) 
                             VALUES (@ClassName, @ClassDescription, @MinimumAllowedAge, @DefaultValidityLength, @ClassFees);
                             SELECT SCOPE_IDENTITY();";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@ClassName", ClassName);
            Command.Parameters.AddWithValue("@ClassDescription", ClassDescription);
            Command.Parameters.AddWithValue("@MinimumAllowedAge", MinimumAlloweAge);
            Command.Parameters.AddWithValue("@DefaultValidityLength", DefaultValidityLength);
            Command.Parameters.AddWithValue("@ClassFees", ClassFees);
            try
            {
                Connection.Open();
                object Result = Command.ExecuteScalar();
                if (Result != null && int.TryParse(Result.ToString(), out int InsertedID))
                {
                    LicenseClassID = InsertedID;
                }
            }
            catch (SqlException ex)
            {
                LicenseClassID = -1;
                clsEventLogger.SaveLog("Application", $"{ex.Message}: failed through adding" +
                    $" new license class with name = {ClassName}.", EventLogEntryType.Error);
            }
            finally
            {
                Connection.Close();
            }
            return LicenseClassID;
        }
        public static bool UpdateLicenseClass(int ID, string ClassName, string ClassDescription,
            byte MinimumAllowedAge, byte DefaultValidityLength, float ClassFees)
        {
            int RowsAffedted = 0;
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string Query = @"UPDATE LicenseClasses 
                             SET ClassName = @ClassName,
                                 ClassDescription = @ClassDescription,
                                 MinimumAllowedAge = @MinimumAllowedAge,
                                 DefaultValidityLength = @DefaultValidityLength,
                                 ClassFees = @ClassFees 
                                 WHERE LicenseClassID = @ID";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@ID", ID);
            Command.Parameters.AddWithValue("@ClassName", ClassName);
            Command.Parameters.AddWithValue("@ClassDescription", ClassDescription);
            Command.Parameters.AddWithValue("@MinimumAllowedAge", MinimumAllowedAge);
            Command.Parameters.AddWithValue("@DefaultValidityLength", DefaultValidityLength);
            Command.Parameters.AddWithValue("@ClassFees", ClassFees);
            try
            {
                Connection.Open();
                RowsAffedted = Command.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                RowsAffedted = 0;
                clsEventLogger.SaveLog("Application", $"{ex.Message}: failed through updating" +
                    $" license class with ID = {ID}", EventLogEntryType.Error);
            }
            finally
            {
                Connection.Close();
            }
            return RowsAffedted > 0;
        }
    }
}