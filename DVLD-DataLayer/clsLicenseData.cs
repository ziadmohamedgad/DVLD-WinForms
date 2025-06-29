using DVLD_DataAccess;
using DVLD_Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace DVLD_DataLayer
{
    public class clsLicenseData
    {
        public static bool GetLicenseInfoByID(int LicenseID, ref int ApplicationID, ref int DriverID, ref int LicenseClassID,
            ref DateTime IssueDate, ref DateTime ExpirationDate, ref string Notes, ref float PaidFees, ref bool IsActive,
            ref byte IssueReason, ref int CreatedByUserID)
        {
            bool IsFound = false;
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string Query = @"SELECT * FROM Licenses WHERE LicenseID = @LicenseID";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@LicenseID", LicenseID);
            try
            {
                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();
                if (Reader.Read())
                {
                    IsFound = true;
                    ApplicationID = (int)Reader["ApplicationID"];
                    DriverID = (int)Reader["DriverID"];
                    LicenseClassID = (int)Reader["LicenseClassID"];
                    IssueDate = (DateTime)Reader["IssueDate"];
                    ExpirationDate = (DateTime)Reader["ExpirationDate"];
                    Notes = Reader["Notes"] == DBNull.Value ? "" : (string)Reader["Notes"];
                    PaidFees = Convert.ToSingle(Reader["PaidFees"]);
                    IsActive = (bool)Reader["IsActive"];
                    IssueReason = (byte)Reader["IssueReason"];
                    CreatedByUserID = (int)Reader["CreatedByUserID"];
                }
                Reader.Close();
            }
            catch (SqlException ex)
            {
                IsFound = false;
                clsEventLogger.SaveLog("Application", $"{ex.Message}: failed through fetching " +
                    $"license info with license ID = {LicenseID}.", EventLogEntryType.Error);
            }
            finally
            {
                Connection.Close();
            }
            return IsFound;
        }
        public static int AddNewLicense(int ApplicationID, int DriverID, int LicenseClassID, DateTime IssueDate,
            DateTime ExpirationDate, string Notes, float PaidFees, bool IsActive, byte IssueReason, int CreatedByUserID)
        {
            int LicenseID = -1;
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string Query = @"INSERT INTO Licenses (ApplicationID, DriverID, LicenseClassID, IssueDate, ExpirationDate,
                                                   Notes, PaidFees, IsActive, IssueReason, CreatedByUserID) 
                             VALUES (@ApplicationID, @DriverID, @LicenseClassID, @IssueDate, @ExpirationDate,
                                     @Notes, @PaidFees, @IsActive, @IssueReason, @CreatedByUserID);
                             SELECT SCOPE_IDENTITY();";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            Command.Parameters.AddWithValue("@DriverID", DriverID);
            Command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);
            Command.Parameters.AddWithValue("@IssueDate", IssueDate);
            Command.Parameters.AddWithValue("@ExpirationDate", ExpirationDate);
            if (Notes != "")
                Command.Parameters.AddWithValue("@Notes", Notes);
            else
                Command.Parameters.AddWithValue("@Notes", DBNull.Value);
            Command.Parameters.AddWithValue("@PaidFees", PaidFees);
            Command.Parameters.AddWithValue("@IsActive", IsActive);
            Command.Parameters.AddWithValue("@IssueReason", IssueReason);
            Command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
            try
            {
                Connection.Open();
                object Result = Command.ExecuteScalar();
                if (Result != null && int.TryParse(Result.ToString(), out int InsertedID))
                    LicenseID = InsertedID;
            }
            catch (SqlException ex)
            {
                LicenseID = -1;
                clsEventLogger.SaveLog("Application", $"{ex.Message}: failed through adding " +
                                    $"new license with application ID = {ApplicationID} for driver ID = {DriverID}.", EventLogEntryType.Error);
            }
            finally
            {
                Connection.Close();
            }
            return LicenseID;
        }
        public static bool UpdateLicense(int LicenseID, int ApplicationID, int DriverID, int LicenseClassID, DateTime IssueDate,
            DateTime ExpirationDate, string Notes, float PaidFees, bool IsActive, byte IssueReason, int CreatedByUserID)
        {
            int RowsAffected = 0;
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string Query = @"UPDATE Licenses 
                             SET ApplicationID = @ApplicationID, 
                                 DriverID = @DriverID,
                                 LicenseClassID = @LicenseClassID,
                                 IssueDate = @IssueDate,
                                 ExpirationDate = @ExpirationDate,
                                 Notes = @Notes,
                                 PaidFees = @PaidFees,
                                 IsActive = @IsActive,
                                 IssueReason = @IssueReason,
                                 CreatedByUserID = @CreatedByUserID 
                             WHERE LicenseID = @LicenseID";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@LicenseID", LicenseID);
            Command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            Command.Parameters.AddWithValue("@DriverID", DriverID);
            Command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);
            Command.Parameters.AddWithValue("@IssueDate", IssueDate);
            Command.Parameters.AddWithValue("@ExpirationDate", ExpirationDate);
            if (Notes != "")
                Command.Parameters.AddWithValue("@Notes", Notes);
            else
                Command.Parameters.AddWithValue("@Notes", DBNull.Value);
            Command.Parameters.AddWithValue("@PaidFees", PaidFees);
            Command.Parameters.AddWithValue("@IsActive", IsActive);
            Command.Parameters.AddWithValue("@IssueReason", IssueReason);
            Command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
            try
            {
                Connection.Open();
                RowsAffected = Command.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                RowsAffected = 0;
                clsEventLogger.SaveLog("Application", $"{ex.Message}: failed through updating " +
                    $"license info with license ID = {LicenseID} and application ID = {ApplicationID} " +
                    $"for driver ID = {DriverID}.", EventLogEntryType.Error);
            }
            finally
            {
                Connection.Close();
            }
            return RowsAffected > 0;
        }
        public static int GetActiveLicenseIDByPersonID(int PersonID, int LicenseClassID)
        {
            int LicenseID = -1;
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string Query = @"SELECT Licenses.LicenseID FROM Licenses 
                             INNER JOIN Drivers ON Licenses.DriverID = DRivers.DriverID 
                             WHERE Licenses.LicenseClassID = @LicenseClassID 
                             AND Drivers.PersonID = @PersonID 
                             AND Licenses.IsActive = 1";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);
            Command.Parameters.AddWithValue("@PersonID", PersonID);
            try
            {
                Connection.Open();
                object Result = Command.ExecuteScalar();
                if (Result != null && int.TryParse(Result.ToString(), out int ID))
                    LicenseID = ID;
            }
            catch (SqlException ex)
            {
                LicenseID = -1;
                clsEventLogger.SaveLog("Application", $"{ex.Message}: failed through fetching " +
                    $"active license for person ID {PersonID}.", EventLogEntryType.Error);
            }
            finally
            {
                Connection.Close();
            }
            return LicenseID;
        }
        public static bool DeactivateLicense(int LicenseID)
        {
            int RowsAffected = 0;
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string Query = @"UPDATE Licenses SET IsActive = 0 WHERE LicenseID = @LicenseID";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@LicenseID", LicenseID);
            try
            {
                Connection.Open();
                RowsAffected = Command.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                RowsAffected = 0;
                clsEventLogger.SaveLog("Application", $"{ex.Message}: failed through deactivating " +
                    $"license with ID = {LicenseID}.", EventLogEntryType.Error);
            }
            finally
            {
                Connection.Close();
            }
            return RowsAffected > 0;
        }
        public static DataTable GetDriverLicenses(int DriverID)
        {
            DataTable dt = new DataTable();
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string Query = @"SELECT Licenses.LicenseID, Licenses.ApplicationID, LicenseClasses.ClassName,
                                Licenses.IssueDate, Licenses.ExpirationDate, Licenses.IsActive 
                             FROM Licenses INNER JOIN LicenseClasses 
                             ON Licenses.LicenseClassID = LicenseClasses.LicenseClassID 
                             WHERE Licenses.DriverID = @DriverID 
                             ORDER BY IsActive Desc, ExpirationDate Desc";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@DriverID", DriverID);
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
                    $"all driver licenses for driver ID = {DriverID}.", EventLogEntryType.Error);
            }
            finally
            {
                Connection.Close();
            }
            return dt;
        }
        public static DataTable GetAllLicenses()
        {
            DataTable dt = new DataTable();
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string Query = @"SELECT * FROM Licenses";
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
                    $"all licenses.", EventLogEntryType.Error);
            }
            finally
            {
                Connection.Close();
            }
            return dt;
        }
    }
}