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
    public class clsLocalDrivingLicenseApplicationData
    {
        public static bool GetLocalDrivingLicenseApplicationInfoByApplicationID(int ApplicationID,
            ref int LocalDrivingLicenseApplicationID, ref int LicenseClassID)
        {
            bool IsFound = false;
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string Query = @"SELECT * FROM LocalDrivingLicenseApplications 
                             WHERE ApplicationID = @ApplicationID";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            try
            {
                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();
                if (Reader.Read())
                {
                    IsFound = true;
                    LocalDrivingLicenseApplicationID = (int)Reader["LocalDrivingLicenseApplicationID"];
                    LicenseClassID = (int)Reader["LicenseClassID"];
                }
                Reader.Close();
            }
            catch (SqlException ex)
            {
                IsFound = false;
                clsEventLogger.SaveLog("Application", $"{ex.Message}: failed through fetching local driving license" +
                    $"application info with application ID = {ApplicationID}.",  EventLogEntryType.Error);
            }
            finally
            {
                Connection.Close();
            }
            return IsFound;
        }
        public static bool GetLocalDrivingLicenseApplicationInfoByID(int LocalDrivingLicenseApplicationID,
            ref int ApplicationID, ref int LicenseClassID)
        {
            bool IsFound = false;
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string Query = @"SELECT * FROM LocalDrivingLicenseApplications 
                             WHERE LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
            try
            {
                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();
                if (Reader.Read())
                {
                    IsFound = true;
                    ApplicationID = (int)Reader["ApplicationID"];
                    LicenseClassID = (int)Reader["LicenseClassID"];
                }
                Reader.Close();
            }
            catch (SqlException ex)
            {
                IsFound = false;
                clsEventLogger.SaveLog("Application", $"{ex.Message}: failed through fetching local driving license application info " +
                    $" with ID = {LocalDrivingLicenseApplicationID}.", EventLogEntryType.Error);
            }
            finally
            {
                Connection.Close();
            }
            return IsFound;
        }
        public static DataTable GetAllLocalDrivingLicenseApplications()
        {
            DataTable dt = new DataTable();
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string Query = @"SELECT * FROM LocalDrivingLicenseApplications_View ORDER BY ApplicationDate DESC";
            SqlCommand Command = new SqlCommand(Query, Connection);
            try
            {
                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();
                if (Reader.Read())
                    dt.Load(Reader);
                Reader.Close();
            }
            catch (SqlException ex)
            {
                dt = null;
                clsEventLogger.SaveLog("Application", $"{ex.Message}: failed through fetching " +
                    $"all local driving license applications.", EventLogEntryType.Error);
            }
            finally
            {
                Connection.Close();
            }
            return dt;
        }
        public static bool UpdateLocalDrivingLicenseApplication(int LocalDrivingLicenseApplicationID, int ApplicationID, int LicenseClassID)
        {
            int RowsAffected = 0;
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string Query = @"UPDATE LocalDrivingLicenseApplications 
                             SET ApplicationID = @ApplicationID,
                                 LicenseClassID = @LicenseClassID 
                             WHERE LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
            Command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            Command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);
            try
            {
                Connection.Open();
                RowsAffected = Command.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                RowsAffected = 0;
                clsEventLogger.SaveLog("Application", $"{ex.Message}: failed through updating local driving license application " +
                    $"with ID {LocalDrivingLicenseApplicationID}. ", EventLogEntryType.Error);
            }
            finally
            {
                Connection.Close();
            }
            return RowsAffected > 0;
        }
        public static int AddNewLocalDrivingLicenseApplication(int ApplicationID, int LicenseClassID)
        {
            int LocalDrivingLicenseApplicationID = -1;
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string Query = @"INSERT INTO LocalDrivingLicenseApplications (ApplicationID, LicenseClassID) 
                             VALUES (@ApplicationID, @LicenseClassID);
                             SELECT SCOPE_IDENTITY();";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            Command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);
            try
            {
                Connection.Open();
                object Result = Command.ExecuteScalar();
                if (Result != null && int.TryParse(Result.ToString(), out int InsertedID))
                {
                    LocalDrivingLicenseApplicationID = InsertedID;
                }
            }
            catch (SqlException ex)
            {
                LocalDrivingLicenseApplicationID = -1;
                clsEventLogger.SaveLog("Application", $"{ex.Message}: failed through adding new local driving license application " +
                    $"with application ID = {ApplicationID}.", EventLogEntryType.Error);
            }
            finally
            {
                Connection.Close();
            }
            return LocalDrivingLicenseApplicationID;
        }
        public static bool DeleteLocalDrivingLicenseApplication(int LocalDrivingLicenseApplicationID)
        {
            int RowsAffected = 0;
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string Query = @"DELETE LocalDrivingLicenseApplications 
                             WHERE LocalDrivingLicenseApplication = @LocalDrivingLicenseApplication";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@LocalDrivingLicenseApplication", LocalDrivingLicenseApplicationID);
            try
            {
                Connection.Open();
                RowsAffected = Command.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                RowsAffected = 0;
                clsEventLogger.SaveLog("Application", $"{ex.Message}: failed through deleting local driving license application" +
                    $" with ID = {LocalDrivingLicenseApplicationID}.", EventLogEntryType.Error);
            }
            finally
            {
                Connection.Close();
            }
            return RowsAffected > 0;
        }
        public static bool DoesPassTestType(int LocalDrivingLicenseApplicationID, int TestTypeID) // this method validates if passed to prevent retaking the test
        {
            // if we caught the latest test, we will validate if it was pass or fail (The person can't take the test again if he passed before).
            bool Result = false;
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string Query = @"SELECT TOP 1 TestResult
                            FROM LocalDrivingLicenseApplications INNER JOIN
                                 TestAppointments ON LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = TestAppointments.LocalDrivingLicenseApplicationID 
                                 INNER JOIN Tests ON TestAppointments.TestAppointmentID = Tests.TestAppointmentID
                            WHERE
                            LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID 
                            AND TestAppointments.TestTypeID = @TestTypeID
                            ORDER BY TestAppointments.TestAppointmentID DESC"; // top 1 res => last test result he made because of ordering desc
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
            Command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
            try
            {
                Connection.Open();
                object Res = Command.ExecuteScalar(); 
                if (Res != null && bool.TryParse(Res.ToString(), out bool ReturnedResult))
                    Result = ReturnedResult; // here if the last test result is 0, it means he deoes not pass the test, so the Result will be FALSE, otherwise TRUE
            }
            catch (SqlException ex)
            {
                Result = false;
                clsEventLogger.SaveLog("Application", $"{ex.Message}: failed through checking if passed test type " +
                    $"for test type with ID = {TestTypeID} and local driving license application with ID = {LocalDrivingLicenseApplicationID}.", EventLogEntryType.Error);
            }
            finally
            {
                Connection.Close();
            }
            return Result;
        }
        public static bool DoesAttendTestType(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            bool Result = false;
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string Query = @"SELECT TOP 1 Found = 1
                            FROM LocalDrivingLicenseApplications INNER JOIN
                                 TestAppointments ON LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = TestAppointments.LocalDrivingLicenseApplicationID 
                                 INNER JOIN Tests ON TestAppointments.TestAppointmentID = Tests.TestAppointmentID
                            WHERE
                            LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID 
                            AND TestAppointments.TestTypeID = @TestTypeID
                            ORDER BY TestAppointments.TestAppointmentID DESC";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
            Command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
            try
            {
                Connection.Open();
                object Res = Command.ExecuteScalar();
                Result = (Res != null);
            }
            catch (SqlException ex)
            {
                Result = false;
                clsEventLogger.SaveLog("Application", $"{ex.Message}: failed through checking if attended test type with ID = {TestTypeID} " +
                    $"for local driving license application with ID = {LocalDrivingLicenseApplicationID}.", EventLogEntryType.Error);
            }
            finally
            {
                Connection.Close();
            }
            return Result;
        }
        public static byte TotalAttemptsPerTest(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            byte TotalAttempts = 0;
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string Query = @"SELECT TotalAttempts = count(TestID)
                            FROM LocalDrivingLicenseApplications INNER JOIN
                                 TestAppointments ON LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = TestAppointments.LocalDrivingLicenseApplicationID INNER JOIN
                                 Tests ON TestAppointments.TestAppointmentID = Tests.TestAppointmentID
                            WHERE
                            LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID  
                            AND TestAppointments.TestTypeID = @TestTypeID";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
            Command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
            try
            {
                Connection.Open();
                object Result = Command.ExecuteScalar();
                if (Result != null && byte.TryParse(Result.ToString(), out byte Attempts))
                    TotalAttempts = Attempts;
            }
            catch (SqlException ex)
            {
                TotalAttempts = 0;
                clsEventLogger.SaveLog("Application", $"{ex.Message}: failed through fetching total attempts per test for " +
                    $"test type with ID = {TestTypeID} and local driving license application with ID = {LocalDrivingLicenseApplicationID}.", EventLogEntryType.Error);
            }
            finally
            {
                Connection.Close();
            }
            return TotalAttempts;
        }
        public static bool IsThereAnActiveScheduledTest(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            bool Result = false;
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string Query = @"SELECT top 1 Found = 1 FROM LocalDrivingLicenseApplications INNER JOIN TestAppointments 
                                ON LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = TestAppointments.LocalDrivingLicenseApplicationID  
                                WHERE LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID 
                                AND TestAppointments.TestTypeID = @TestTypeID 
                                AND TestAppointments.IsLocked = 0  
                                ORDER BY TestAppointments.TestAppointmentID DESC";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
            Command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
            try
            {
                Connection.Open();
                object Res = Command.ExecuteScalar();
                Result = Res != null;
            }
            catch (SqlException ex)
            {
                Result = false;
                clsEventLogger.SaveLog("Application", $"{ex.Message}: failed through checking if there is an active scheduled test " +
                    $"with test type ID = {TestTypeID} and local drving license application with ID = {LocalDrivingLicenseApplicationID}.", EventLogEntryType.Error);
            }
            finally
            {
                Connection.Close();
            }
            return Result;
        }
    }
}