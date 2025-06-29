using DVLD_DataAccess;
using DVLD_Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
namespace DVLD_DataLayer
{
    public class clsTestAppointmentData
    {
        public static DataTable GetAllTestAppointments()
        {
            DataTable dt = new DataTable();
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string Query = @"SELECT * FROM TestAppointments_View ORDER BY AppointmentDate DESC";
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
                    $"all tests appointments." , EventLogEntryType.Error);
            }
            finally
            {
                Connection.Close();
            }
            return dt;
        }
        public static bool GetTestAppointmentInfoByID(int TestAppointmentID, ref int TestTypeID, ref int LocalDrivingLicenseApplicationID,
            ref DateTime AppointmentDate, ref float PaidFees, ref int CreatedByUserID, ref bool IsLocked, ref int RetakeTestApplicationID)
        {
            bool IsFound = false;
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string Query = @"SELECT * FROM TestAppointments WHERE TestAppointmentID = @TestAppointmentID";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
            try
            {
                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();
                if (Reader.Read())
                {
                    IsFound = true;
                    TestTypeID = (int)Reader["TestTypeID"];
                    LocalDrivingLicenseApplicationID = (int)Reader["LocalDrivingLicenseApplicationID"];
                    AppointmentDate = (DateTime)Reader["AppointmentDate"];
                    PaidFees = Convert.ToSingle(Reader["PaidFees"]);
                    CreatedByUserID = (int)Reader["CreatedByUserID"];
                    IsLocked = (bool)Reader["IsLocked"];
                    RetakeTestApplicationID = Reader["RetakeTestApplicationID"] == DBNull.Value ? -1 : (int)Reader["RetakeTestApplicationID"];
                }
                Reader.Close();
            }
            catch (SqlException ex)
            {
                IsFound = false;
                clsEventLogger.SaveLog("Application", $"{ex.Message}: failed through fetching " +
                    $"test appointment info with ID = {TestAppointmentID}.", EventLogEntryType.Error);
            }
            finally
            {
                Connection.Close();
            }
            return IsFound;
        }
        public static bool GetLastTestAppointmentint(int LocalDrivingLicenseApplicationID,  int TestTypeID,
            ref int TestAppointmentID,ref DateTime AppointmentDate,
            ref float PaidFees, ref int CreatedByUserID,ref bool IsLocked,ref int RetakeTestApplicationID)
        {
            bool IsFound = false;
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string Query = @"SELECT TOP 1 * FROM TestAppointments 
                             WHERE TestTypeID = @TestTypeID 
                             AND LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID 
                             ORDER BY TestAppointmentID DESC";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
            Command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
            try
            {
                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();
                if (Reader.Read())
                {
                    TestAppointmentID = (int)Reader["TestAppointmentID"];
                    AppointmentDate = (DateTime)Reader["AppointmentDate"];
                    PaidFees = Convert.ToSingle(Reader["PaidFees"]);
                    CreatedByUserID = (int)Reader["CreatedByUserID"];
                    IsLocked = (bool)Reader["IsLocked"];
                    RetakeTestApplicationID = Reader["RetakeTestApplicationID"] == DBNull.Value ? -1 : (int)Reader["RetakeTestApplicationID"];
                }
                Reader.Close();
            }
            catch (SqlException ex)
            {
                IsFound = false;
                clsEventLogger.SaveLog("Application", $"{ex.Message}: failed through fetching last test appointment with " +
                    $"test type ID = {TestTypeID} for local driving license application ID = {LocalDrivingLicenseApplicationID}.", EventLogEntryType.Error);
            }
            finally
            {
                Connection.Close();
            }
            return IsFound;
        }
        public static DataTable GetApplicationTestAppointmentsPersTestType(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            DataTable dt = new DataTable();
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string Query = @"SELECT TestAppointmentID, AppointmentDate, PaidFees, IsLocked 
                             FROM TestAppointments 
                             WHERE TestTypeID = @TestTypeID 
                             AND LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID 
                             ORDER BY TestAppointmentID DESC";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
            Command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
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
                    $"application test appointments with local driving license application ID = {LocalDrivingLicenseApplicationID} for " +
                    $"test type ID = {TestTypeID}.", EventLogEntryType.Error);
            }
            finally
            {
                Connection.Close();
            }
            return dt;
        }
        public static bool UpdateTestAppointment(int TestAppointmentID, int TestTypeID, int LocalDrivingLicenseApplicationID,
            DateTime AppointmentDate, float PaidFees, int CreatedByUserID, bool IsLocked, int RetakeTestApplicationID)
        {
            int RowsAffected = 0;
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string Query = @"UPDATE TestAppointments 
                             SET TestTypeID = @TestTypeID,
                                 LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID,
                                 AppointmentDate = @AppointmentDate,
                                 PaidFees = @PaidFees,
                                 CreatedByUserID = @CreatedByUserID,
                                 IsLocked = @IsLocked,
                                 RetakeTestApplicationID = @RetakeTestApplicationID
                                 WHERE TestAppointmentID = @TestAppointmentID";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
            Command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
            Command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
            Command.Parameters.AddWithValue("@AppointmentDate", AppointmentDate);
            Command.Parameters.AddWithValue("@PaidFees", PaidFees);
            Command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
            Command.Parameters.AddWithValue("@IsLocked", IsLocked);
            if (RetakeTestApplicationID == -1)
                Command.Parameters.AddWithValue("@RetakeTestApplicationID", DBNull.Value);
            else
                Command.Parameters.AddWithValue("@RetakeTestApplicationID", RetakeTestApplicationID);
            try
            {
                Connection.Open();
                RowsAffected = Command.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                RowsAffected = 0;
                clsEventLogger.SaveLog("Application", $"{ex.Message}: failed through updating" +
                    $" test appointment with test appointment ID = {TestAppointmentID} " +
                    $"and test type ID = {TestTypeID} for local driving license application with ID = {LocalDrivingLicenseApplicationID}.", EventLogEntryType.Error);
            }
            finally
            {
                Connection.Close();
            }
            return RowsAffected > 0;
        }
        public static int AddNewTestAppointment(int TestTypeID, int LocalDrivingLicenseApplicationID, DateTime AppointmentDate,
            float PaidFees, int CreatedByUserID, bool IsLocked, int RetakeTestApplicationID)
        {
            int TestAppointmentID = -1;
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string Query = @"INSERT INTO TestAppointments (TestTypeID, LocalDrivingLicenseApplicationID, AppointmentDate,
                                                           PaidFees, CreatedByUserID, IsLocked, RetakeTestApplicationID) 
                             VALUES (@TestTypeID, @LocalDrivingLicenseApplicationID, @AppointmentDate, @PaidFees,
                                     @CreatedByUserID, @IsLocked, @RetakeTestApplicationID);
                             SELECT SCOPE_IDENTITY();";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
            Command.Parameters.AddWithValue("@LocalDrivingLicenceApplicationID", LocalDrivingLicenseApplicationID);
            Command.Parameters.AddWithValue("@AppointmentDate", AppointmentDate);
            Command.Parameters.AddWithValue("@PaidFees", PaidFees);
            Command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
            Command.Parameters.AddWithValue("@IsLocked", IsLocked);
            if (RetakeTestApplicationID == -1)
                Command.Parameters.AddWithValue("@RetakeTestApplicationID", DBNull.Value);
            else
                Command.Parameters.AddWithValue("@RetakeTestApplicationID", RetakeTestApplicationID);
            try
            {
                Connection.Open();
                object Result = Command.ExecuteScalar();
                if (Result != null && int.TryParse(Result.ToString(), out int InsertedID))
                    TestAppointmentID = InsertedID;
            }
            catch (SqlException ex)
            {
                TestAppointmentID = -1;
                clsEventLogger.SaveLog("Application", $"{ex.Message}: failed through adding new test appointment with test type ID = {TestTypeID} " +
                    $"for local driving license application with ID = {LocalDrivingLicenseApplicationID}.", EventLogEntryType.Error);
            }
            finally
            {
                Connection.Close();
            }
            return TestAppointmentID;
        }
        public static int GetTestID(int TestAppointmentID)
        {
            int TestID = -1;
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string Query = @"SELECT TestID FROM Tests WHERE TestAppointmentID = @TestAppointmentID";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
            try
            {
                Connection.Open();
                object Result = Command.ExecuteScalar();
                if (Result != null && int.TryParse(Result.ToString(), out int InsertedID))
                    TestID = InsertedID;
            }
            catch (SqlException ex)
            {
                TestID = -1;
                clsEventLogger.SaveLog("Application", $"{ex.Message}: failed through fetching " +
                    $"test info with test appointment ID = {TestAppointmentID}.", EventLogEntryType.Error);
            }
            finally
            {
                Connection.Close();
            }
            return TestID;
        }
    }
}