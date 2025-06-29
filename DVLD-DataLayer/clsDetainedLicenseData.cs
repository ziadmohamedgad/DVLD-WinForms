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
    public class clsDetainedLicenseData
    {
        public static int AddNewDetainedLicense(int LicenseID, DateTime DetainDate, float FineFees,
            int CreatedByUserID)
        {
            int DetainID = -1;
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string Query = @"INSERT INTO DetainedLicenses (LicenseID, DetainDate, FineFees, CreatedByUserID, IsReleased) 
                             VALUES (@LicenseID, @DetainDate, @FineFees, @CreatedByUserID, 0);
                             SELECT SCOPE_IDENTITY();"; //  Is Released is 0 (Immediate Detain)
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@LicenseID", LicenseID);
            Command.Parameters.AddWithValue("@DetainDate", DetainDate);
            Command.Parameters.AddWithValue("@FineFees", FineFees);
            Command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
            try
            {
                Connection.Open();
                object Result = Command.ExecuteScalar();
                if (Result != null && int.TryParse(Result.ToString(), out int ID))
                    DetainID = ID;
            }
            catch (SqlException ex)
            {
                DetainID = -1;
                clsEventLogger.SaveLog("Application", $"{ex.Message}: failed through adding" +
                    $"new detained license for license ID = {LicenseID}.", EventLogEntryType.Error);
            }
            finally
            {
                Connection.Close();
            }
            return DetainID;
        }
        public static bool UpdateDetainedLicense(int DetainID, int LicenseID, DateTime DetainDate, float FineFees,
            int CreatedByUserID)
        {
            int RowsAffected = 0;
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string Query = @"UPDATE DetainedLicenses 
                             SET LicenseID = @LicenseID, 
                                 DetainDate = @DetainDate,
                                 FineFees = @FineFees,
                                 CreatedByUserID = @CreatedByUserID,
                             WHERE DetainID = @DetainID";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@DetainID", DetainID);
            Command.Parameters.AddWithValue("@LicenseID", LicenseID);
            Command.Parameters.AddWithValue("@FineFees", FineFees);
            Command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
            try
            {
                Connection.Open();
                RowsAffected = Command.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                RowsAffected = 0;
                clsEventLogger.SaveLog("Application", $"{ex.Message}: failed through updating" +
                    $"detained license with (detain ID = {DetainID} and license ID = {LicenseID}).", EventLogEntryType.Error);
            }
            finally
            {
                Connection.Close();
            }
            return RowsAffected > 0;
        }
        public static bool GetDetainedLicenseInfoByID(int DetainID, ref int LicenseID, ref DateTime DetainDate,
            ref float FineFees, ref int CreatedByUserID, ref bool IsReleased, ref DateTime ReleaseDate,
            ref int ReleasedByUserID, ref int ReleaseApplicationID)
        {
            bool IsFound = false;
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string Query = @"SELECT * FROM DetainedLicenses WHERE DetainID = @DetainID";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@DetainID", DetainID);
            try
            {
                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();
                if (Reader.Read())
                {
                    IsFound = true;
                    LicenseID = (int)Reader["LicenseID"];
                    DetainDate = (DateTime)Reader["DetainDate"];
                    FineFees = Convert.ToSingle(Reader["FineFees"]);
                    CreatedByUserID = (int)Reader["CreatedByUserID"];
                    IsReleased = (bool)Reader["IsReleased"];
                    ReleaseDate = Reader["ReleaseDate"] == DBNull.Value ? DateTime.MaxValue : (DateTime)Reader["ReleaseDate"];
                    ReleasedByUserID = Reader["ReleasedByUserID"] == DBNull.Value ? -1 : (int)Reader["ReleasedByUserID"];
                    ReleaseApplicationID = Reader["ReleaseApplicationID"] == DBNull.Value ? -1 : (int)Reader["ReleaseApplicationID"];
                }
                Reader.Close();
            }
            catch (SqlException ex)
            {
                IsFound = false;
                clsEventLogger.SaveLog("Application", $"{ex.Message}: failed through fetching" +
                    $" detained license info with detain ID = {DetainID}.", EventLogEntryType.Error);
            }
            finally
            {
                Connection.Close();
            }
            return IsFound;
        }
        public static bool GetDetainedLicenseInfoByLicenseID(int LicenseID, ref int DetainID, ref DateTime DetainDate,
            ref float FineFees, ref int CreatedByUserID, ref bool IsReleased, ref DateTime ReleaseDate,
            ref int ReleasedByUserID, ref int ReleaseApplicationID)
        {
            bool IsFound = false;
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string Query = @"SELECT TOP 1 * FROM DetainedLicenses WHERE LicenseID = @LicenseID ORDER BY DetainID DESC";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@LicenseID", LicenseID);
            try
            {
                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();
                if (Reader.Read())
                {
                    IsFound = true;
                    DetainID = (int)Reader["DetainID"];
                    DetainDate = (DateTime)Reader["DetainDate"];
                    FineFees = Convert.ToSingle(Reader["FineFees"]);
                    CreatedByUserID = (int)Reader["CreatedByUserID"];
                    IsReleased = (bool)Reader["IsReleased"];
                    ReleaseDate = Reader["ReleaseDate"] == DBNull.Value ? DateTime.MaxValue : (DateTime)Reader["ReleaseDate"];
                    ReleasedByUserID = Reader["ReleasedByUserID"] == DBNull.Value ? -1 : (int)Reader["ReleasedByUserID"];
                    ReleaseApplicationID = Reader["ReleaseApplicationID"] == DBNull.Value ? -1 : (int)Reader["ReleaseApplicationID"];
                }
                Reader.Close();
            }
            catch (SqlException ex)
            {
                IsFound = false;
                clsEventLogger.SaveLog("Application", $"{ex.Message}: failed through fetching " +
                    $"detined license info for license ID = {LicenseID}.", EventLogEntryType.Error);
            }
            finally
            {
                Connection.Close();
            }
            return IsFound;
        }
        public static DataTable GetAllDetainedLicenses()
        {
            DataTable dt = new DataTable();
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string Query = @"SELECT * FROM DetainedLicenses_View Order By IsReleased, DetainID";
            SqlCommand Command = new SqlCommand(Query, Connection);
            try
            {
                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();
                if (Reader.HasRows)
                    dt.Load(Reader);
                Reader.Close();
            }
            catch (SqlException ex)
            {
                dt = null;
                clsEventLogger.SaveLog("Application", $"{ex.Message}: failed through fetching " +
                    $"all detained licenses.", EventLogEntryType.Error);
            }
            finally
            {
                Connection.Close();
            }
            return dt;
        }
        public static bool IsLicenseDetained(int LicenseID)
        {
            bool IsDetained = false;
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string Query = @"SELECT IsDetained = 1 From DetainedLicenses WHERE LicenseID = @LicenseID And IsReleased = 0";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@LicenseID", LicenseID);
            try
            {
                Connection.Open();
                object Result = Command.ExecuteScalar();
                if (Result != null)
                    IsDetained = Convert.ToBoolean(Result);
            }
            catch (SqlException ex)
            {
                IsDetained = false;
                clsEventLogger.SaveLog("Application", $"{ex.Message}: failed through checking if" +
                    $"license with license ID = {LicenseID} detained or not.", EventLogEntryType.Error);
            }
            finally
            {
                Connection.Close();
            }
            return IsDetained;
        }
        public static bool ReleaseDetainedLicense(int DetainID, int ReleasedByUserID, int ReleaseApplicationID)
        {
            int RowsAffected = 0;
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string Query = @"UPDATE DetainedLicenses 
                             SET IsReleased = 1,
                                 ReleaseDate = @ReleaseDate,
                                 ReleasedByUserID = @ReleasedByUserID,
                                 ReleaseApplicationID = @ReleaseApplicationID 
                              WHERE DetainID = @DetainID";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@DetainID", DetainID);
            Command.Parameters.AddWithValue("@ReleasedByUserID", ReleasedByUserID);
            Command.Parameters.AddWithValue("@ReleaseDate", DateTime.Now);
            Command.Parameters.AddWithValue("@ReleaseApplicationID", ReleaseApplicationID);
            try
            {
                Connection.Open();
                RowsAffected = Command.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                RowsAffected = 0;
                clsEventLogger.SaveLog("Application", $"{ex.Message}: failed through releasing" +
                    $"detained license with detain ID = {DetainID} by user: {ReleasedByUserID} for " +
                    $"application with ID = {ReleaseApplicationID}.", EventLogEntryType.Error);
            }
            finally
            {
                Connection.Close();
            }
            return RowsAffected > 0;
        }
    }
}