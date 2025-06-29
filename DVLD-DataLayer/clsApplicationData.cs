using DVLD_DataAccess;
using DVLD_Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
namespace DVLD_DataLayer
{
    public class clsApplicationData
    {
        public static bool UpdateStatus(int ApplicationID, byte NewStatus)
        {
            int RowsAffected = 0;
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string Query = @"UPDATE Applications 
                             SET ApplicationStatus = @NewStatus 
                                 LastStatusDate = @LastStatusDate 
                             WHERE ApplicationID = @ApplicationID";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            Command.Parameters.AddWithValue("@ApplicationStatus", NewStatus);
            Command.Parameters.AddWithValue("@LastStatusDate", DateTime.Now);
            try
            {
                Connection.Open();
                RowsAffected = Command.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                RowsAffected = 0;
                clsEventLogger.SaveLog("Application", $"{ex.Message}: failed through updating status to {NewStatus} " +
                    $"for application with ID = {ApplicationID}.", EventLogEntryType.Error);
            }
            finally
            {
                Connection.Close();
            }
            return RowsAffected > 0;
        }
        public static int GetActiveApplicationIDForLicenseClass(int PersonID, int ApplicationTypeID, int LicenseClassID)
        {
            int ActiveApplicationID = -1;
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string Query = @"SELECT ActiveApplicationID = Applications.ApplicationID  
                             FROM 
                             Applications INNER JOIN 
                             LocalDrivingLicenseApplications ON Applications.ApplicationID = LocalDrivingLicenseApplications.ApplicationID 
                             WHERE Applications.ApplicantPersonID = @ApplicantPersonID 
                             AND Applications.ApplicationTypeID = @ApplicationTypeID 
                             AND LocalDrivingLicenseApplications.LicenseClassID = @LicenseClassID 
                             AND Applications.ApplicationStatus = 1";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@ApplicantPersonID", PersonID);
            Command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
            Command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);
            try
            {
                Connection.Open();
                object Result = Command.ExecuteScalar();
                if (Result != null && int.TryParse(Result.ToString(), out int InsertedID))
                    ActiveApplicationID = InsertedID;
            }
            catch (SqlException ex)
            {
                ActiveApplicationID = -1;
                clsEventLogger.SaveLog("Application", $"{ex.Message}: failed through fetching " +
                    $"active application for person with ID = {PersonID} and application type with ID = " +
                    $"{ApplicationTypeID} and license class with ID = {LicenseClassID}.", EventLogEntryType.Error);
            }
            finally
            {
                Connection.Close();
            }
            return ActiveApplicationID;
        }
        public static bool DoesPersonHaveActiveApplication(int PersonID, int ApplicationTypeID)
        {
            return GetActiveApplicationID(PersonID, ApplicationTypeID) != -1;
        }
        public static int GetActiveApplicationID(int PersonID, int ApplicationTypeID)
        {
            int ActiveApplicationID = -1;
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string Query = @"SELECT ApplicationID FROM Applications WHERE ApplicantPersonID = @ApplicantPersonID 
                                    AND ApplicationTypeID = @ApplicationTypeID AND ApplicationStatus = 1";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@ApplicantPersonID", PersonID);
            Command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
            try
            {
                Connection.Open();
                object Result = Command.ExecuteScalar();
                if (Result != null && int.TryParse(Result.ToString(), out int InsertedID))
                    ActiveApplicationID = InsertedID;
            }
            catch(SqlException ex)
            {
                ActiveApplicationID = -1;
                clsEventLogger.SaveLog("Application", $"{ex.Message}: failed through fetching " +
                    $"active application for person with ID = {PersonID} and application type with ID = " +
                    $"{ApplicationTypeID}.", EventLogEntryType.Error);
            }
            finally
            {
                Connection.Close();
            }
            return ActiveApplicationID;
        }
        public static bool IsApplicationExist(int ApplicationID)
        {
            bool IsFound = false;
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string Query = "SELECT Found = 1 FROM Applications WHERE ApplicationID = @ApplicationID";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            try
            {
                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();
                IsFound = Reader.HasRows;
                Reader.Close();
            }
            catch (SqlException ex)
            {
                IsFound = false;
                clsEventLogger.SaveLog("Application", $"{ex.Message}: failed through fetching " +
                    $"the application with ID = {ApplicationID}.", EventLogEntryType.Error);
            }
            finally
            {
                Connection.Close();
            }
            return IsFound;
        }
        public static bool DeleteApplication(int ApplicationID)
        {
            int RowsAffected = 0;
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string Query = @"DELETE Applications WHERE ApplicationID = @ApplicationID";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            try
            {
                Connection.Open();
                RowsAffected = Command.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                RowsAffected = 0;
                clsEventLogger.SaveLog("Application", $"{ex.Message}: failed deleting application " +
                    $"with ID = {ApplicationID}.", EventLogEntryType.Error);
            }
            finally
            {
                Connection.Close();
            }
            return RowsAffected > 0;
        }
        public static int AddNewApplication(int ApplicantPersonID, DateTime ApplicationDate, int ApplicationTypeID,
            byte ApplicationStatus, DateTime LastStatusDate, float PaidFees, int CreatedByUserID)
        {
            int ApplicationID = -1;
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string Query = @"INSERT INTO Applications (ApplicantPersonID, ApplicationDate, ApplicationTypeID, 
                                                       ApplicationStatus, LastStatusDate, PaidFees, CreatedByUserID) 
                             VALUES (@ApplicantPersonID, @ApplicationDate, @ApplicationTypeID, @ApplicationStatus, 
                                     @LastStatusDate, @PaidFees, @CreatedByUserID);
                             SELECT SCOPE_IDENTITY();";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@ApplicantPersonID", ApplicantPersonID);
            Command.Parameters.AddWithValue("@ApplicationDate", ApplicationDate);
            Command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
            Command.Parameters.AddWithValue("@ApplicationStatus", ApplicationStatus);
            Command.Parameters.AddWithValue("@LastStatusDate", LastStatusDate);
            Command.Parameters.AddWithValue("@PaidFees", PaidFees);
            Command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
            try
            {
                Connection.Open();
                object Result = Command.ExecuteScalar();
                if (Result != null && int.TryParse(Result.ToString(), out int InsertedID))
                    ApplicationID = InsertedID;
            }
            catch(SqlException ex)
            {
                ApplicationID = -1;
                clsEventLogger.SaveLog("Application", $"{ex.Message}: failed through adding " +
                    $"new application for person with ID = {ApplicantPersonID}.", EventLogEntryType.Error);
            }
            finally
            {
                Connection.Close();
            }
            return ApplicationID;
        }
        public static bool UpdateApplication(int ApplicationID, int ApplicantPersonID, DateTime ApplicationDate,
            int ApplicationTypeID, byte ApplicationStaus, DateTime LastStatusDate, float PaidFees, int CreatedByUserID)
        {
            int RowsAffected = 0;
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string Query = @"UPDATE Applications
                             SET ApplicantPersonID = @ApplicantPersonID,
                                 ApplicationDate = @ApplicationDate,
                                 ApplicationTypeID = @ApplicationTypeID,
                                 ApplicationStatus = @ApplicationStatus,
                                 LastStatusDate = @LastStatusDate,
                                 PaidFees = @PaidFees,
                                 CreatedByUserID = @CreatedByUserID 
                                 WHERE ApplicationID = @ApplicationID";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            Command.Parameters.AddWithValue("@ApplicantPersonID", ApplicantPersonID);
            Command.Parameters.AddWithValue("@ApplicationDate", ApplicationDate);
            Command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
            Command.Parameters.AddWithValue("@ApplicationStatus", ApplicationStaus);
            Command.Parameters.AddWithValue("@LastStatusDate", LastStatusDate);
            Command.Parameters.AddWithValue("@PaidFees", PaidFees);
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
                    $"application for person with ID = {ApplicantPersonID}.", EventLogEntryType.Error);
            }
            finally
            {
                Connection.Close();
            }
            return RowsAffected > 0;
        }
        public static bool GetApplicationInfoByID(int ApplicationID, ref int ApplicantPersonID,
            ref DateTime ApplicationDate, ref int ApplicationTypeID, ref byte ApplicationStatus,
            ref DateTime LastStatusDate, ref float PaidFees, ref int CreatedByUserID)
        {
            bool IsFound = false;
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string Query = @"SELECT * FROM Applications WHERE ApplicationID = @ApplicationID";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            try
            {
                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();
                if (Reader.Read())
                {
                    IsFound = true;
                    ApplicantPersonID = (int)Reader["ApplicantPersonID"];
                    ApplicationDate = (DateTime)Reader["ApplicationDate"];
                    ApplicationTypeID = (int)Reader["ApplicationTypeID"];
                    ApplicationStatus = (byte)Reader["ApplicationStatus"];
                    LastStatusDate = (DateTime)Reader["LastStatusDate"];
                    PaidFees = Convert.ToSingle(Reader["PaidFees"]);
                    CreatedByUserID = (int)Reader["CreatedByUserID"];
                }
                Reader.Close();
            }
            catch (SqlException ex)
            {
                IsFound = false;
                clsEventLogger.SaveLog("Application", $"{ex.Message}: failed through fetching " +
                    $"application info for person with ID = {ApplicantPersonID} and application with ID = {ApplicationID}.", EventLogEntryType.Error);
            }
            finally
            {
                Connection.Close();
            }
            return IsFound;
        }
        public static DataTable GetAllApplications()
        {
            DataTable dt = new DataTable();
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string Query = @"SELECT * FROM Applications ORDER BY ApplicationDate DESC";
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
                    $"all applications.", EventLogEntryType.Error);
            }
            finally
            {
                Connection.Close();
            }
            return dt;
        }
    }
}