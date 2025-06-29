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
    public class clsTestData
    {
        public static DataTable GetAllTests()
        {
            DataTable dt = new DataTable();
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string Query = @"SELECT * FROM Tests ORDER BY TestID";
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
                    $"all tests.", EventLogEntryType.Error);
            }
            finally
            {
                Connection.Close();
            }
            return dt;
        }
        public static bool GetTestInfoByTestID(int TestID, ref int TestAppointmentID, ref bool TestResult,
            ref string Notes, ref int CreatedByUserID)
        {
            bool IsFound = false;
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string Query = @"SELECT * FROM Tests WHERE TestID = @TestID";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@TestID", TestID);
            try
            {
                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();
                if (Reader.Read())
                {
                    IsFound = true;
                    TestAppointmentID = (int)Reader["TestAppointmentID"];
                    TestResult = (bool)Reader["TestResult"];
                    Notes = Reader["Notes"] == DBNull.Value ? "" : (string)Reader["Notes"];
                    CreatedByUserID = (int)Reader["CreatedByUserID"];
                }
                Reader.Close();
            }
            catch (SqlException ex)
            {
                IsFound = false;
                clsEventLogger.SaveLog("Application", $"{ex.Message}: failed through fetching " +
                    $"test info with ID = {TestID}.", EventLogEntryType.Error);
            }
            finally
            {
                Connection.Close();
            }
            return IsFound;
        }
        public static bool UpdateTest(int TestID, int TestAppointmentID, bool TestResult, string Notes, int CreatedByUserID)
        {
            int RowsAffected = 0;
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string Query = @"UPDATE Tests
                             SET TestAppointmentID = @TestAppointmentID,
                                 TestResult = @TestResult,
                                 Notes = @Notes,
                                 CreatedByUserID = @CreatedByUserID 
                             WHERE TestID = @TestID";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@TestID", TestID);
            Command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
            Command.Parameters.AddWithValue("@TestResult", TestResult);
            Command.Parameters.AddWithValue("@Notes", Notes);
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
                    $" test with ID = {TestID}.", EventLogEntryType.Error);
            }
            finally
            {
                Connection.Close();
            }
            return RowsAffected > 0;
        }
        public static int AddNewTest(int TestAppointmentID, bool TestResult, string Notes, int CreatedByUserID)
        {
            int TestID = -1;
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string Query = @"INSERT INTO Tests (TestAppointmentID, TestResult, Notes, CreatedByUserID) 
                             VALUES (@TestAppointmentID, @TestResult, @Notes, @CreatedByUserID); 
                             UPDATE TestAppointments 
                             SET IsLocked = 1 WHERE TestAppointmentID = @TestAppointmentID;
                             SELECT SCOPE_IDENTITY();";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
            Command.Parameters.AddWithValue("@TestResult", TestResult);
            if (Notes != "" && Notes != null)
                Command.Parameters.AddWithValue("@Notes", Notes);
            else
                Command.Parameters.AddWithValue("@Notes",DBNull.Value);
            Command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
            try
            {
                Connection.Open();
                object Result = Command.ExecuteScalar();
                if (Result != null && int.TryParse(Result.ToString(), out int InsertedID))
                {
                    TestID = InsertedID;
                }
            }
            catch (SqlException ex)
            {
                TestID = -1;
                clsEventLogger.SaveLog("Application", $"{ex.Message}: failed through adding " +
                    $"new test with test appointment ID = {TestAppointmentID}.", EventLogEntryType.Error);
            }
            finally
            {
                Connection.Close();
            }
            return TestID;
        }
        public static bool GetLastTestByPersonAndTestTypeAndLicenseClass(int PersonID, int LicenseClassID, int TestTypeID,
            ref int TestID, ref int TestAppointmentID, ref bool TestResult, ref string Notes, ref int CreatedByUserID)
        {
            bool isFound = false;
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string Query = @"SELECT  TOP 1 Tests.TestID, 
                Tests.TestAppointmentID, Tests.TestResult, 
			    Tests.Notes, Tests.CreatedByUserID, Applications.ApplicantPersonID
                FROM            LocalDrivingLicenseApplications INNER JOIN
                                         Tests INNER JOIN
                                         TestAppointments ON Tests.TestAppointmentID = TestAppointments.TestAppointmentID ON LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = TestAppointments.LocalDrivingLicenseApplicationID INNER JOIN
                                         Applications ON LocalDrivingLicenseApplications.ApplicationID = Applications.ApplicationID
                WHERE        (Applications.ApplicantPersonID = @PersonID) 
                        AND (LocalDrivingLicenseApplications.LicenseClassID = @LicenseClassID)
                        AND ( TestAppointments.TestTypeID=@TestTypeID)
                ORDER BY Tests.TestAppointmentID DESC";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@PersonID", PersonID);
            Command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);
            Command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
            try
            {
                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();
                if (Reader.Read())
                {
                    isFound = true;
                    TestID = (int)Reader["TestID"];
                    TestAppointmentID = (int)Reader["TestAppointmentID"];
                    TestResult = (bool)Reader["TestResult"];
                    Notes = Reader["Notes"] == DBNull.Value ? "" : (string)Reader["Notes"];
                    CreatedByUserID = (int)Reader["CreatedByUserID"];
                }
                Reader.Close();
            }
            catch (SqlException ex)
            {
                isFound = false;
                clsEventLogger.SaveLog("Application", $"{ex.Message}: failed through fetching last test with test type ID = " +
                    $"{TestTypeID} and license class ID = {LicenseClassID} for person with ID = {PersonID}.", EventLogEntryType.Error);
            }
            finally
            {
                Connection.Close();
            }
            return isFound;
        }
        public static byte GetPassedTestsCount(int LocalDrivingLicenseApplicationID)
        {
            byte PassedTestsCount = 0;
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string Query = @"SELECT PassedTestCount = COUNT(TestTypeID)
                         FROM Tests INNER JOIN
                         TestAppointments ON Tests.TestAppointmentID = TestAppointments.TestAppointmentID
						 WHERE TestAppointments.LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID AND TestResult = 1";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
            try
            {
                Connection.Open();
                object Result = Command.ExecuteScalar();
                if (Result != null && byte.TryParse(Result.ToString(), out byte Count))
                    PassedTestsCount = Count;
            }
            catch (SqlException ex)
            {
                PassedTestsCount = 0;
                clsEventLogger.SaveLog("Application", $"{ex.Message}: failed through fetching the number of " +
                    $"passed tests for local driving license application with ID = {LocalDrivingLicenseApplicationID}.", EventLogEntryType.Error);
            }
            finally
            {
                Connection.Close();
            }
            return PassedTestsCount;
        }
    }
}