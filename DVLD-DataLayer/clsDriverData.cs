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
    public class clsDriverData
    {
        public static bool GetDriverInfoByDriverID(int DriverID, ref int PersonID, ref int CreatedByUserID, ref DateTime CreatedDate)
        {
            bool IsFound = false;
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string Query = @"SELECT * FROM Drivers WHERE DriverID = @DriverID";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@DriverID", DriverID);
            try
            {
                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();
                if (Reader.Read())
                {
                    IsFound = true;
                    PersonID = (int)Reader["PersonID"];
                    CreatedByUserID = (int)Reader["CreatedByUserID"];
                    CreatedDate = (DateTime)Reader["CreatedDate"];
                }
                Reader.Close();
            }
            catch (SqlException ex)
            {
                IsFound = false;
                clsEventLogger.SaveLog("Application", $"{ex.Message}: failed through fetching " +
                    $"driver info with driver ID = {DriverID} for person ID = {PersonID}.", EventLogEntryType.Error);
            }
            finally
            {
                Connection.Close();
            }
            return IsFound;
        }
        public static bool GetDriverInfoByPersonID(int PersonID, ref int DriverID, ref int CreatedByUserID, ref DateTime CreatedDate)
        {
            bool IsFound = false;
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string Query = @"SELECT * FROM Drivers WHERE PersonID = @PersonID";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@PersonID", PersonID);
            try
            {
                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();
                if (Reader.Read())
                {
                    IsFound = true;
                    DriverID = (int)Reader["DriverID"];
                    CreatedByUserID = (int)Reader["CreatedByUserID"];
                    CreatedDate = (DateTime)Reader["CreatedDate"];
                }
                Reader.Close();
            }
            catch (SqlException ex)
            {
                IsFound = false;
                clsEventLogger.SaveLog("Application", $"{ex.Message}: failed through fetching " +
                    $"driver info for person ID = {PersonID}.", EventLogEntryType.Error);
            }
            finally
            {
                Connection.Close();
            }
            return IsFound;
        }
        public static int AddNewDriver(int PersonID, int CreatedByUserID)
        {
            int DriverID = -1;
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string Query = @"INSERT INTO Drivers (PersonID, CreatedByUSerID, CreatedDate) 
                             VALUES (@PersonID, @CreatedByUserID, @CreatedDate); 
                             SELECT SCOPE_IDENTITY();";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@PersonID", PersonID);
            Command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
            Command.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
            try
            {
                Connection.Open();
                object Result = Command.ExecuteScalar();
                if (Result != null && int.TryParse(Result.ToString(), out int ID))
                    DriverID = ID;
            }
            catch (SqlException ex)
            {
                DriverID = -1;
                clsEventLogger.SaveLog("Application", $"{ex.Message}: failed through adding" +
                    $" new driver for person ID = {PersonID}.", EventLogEntryType.Error);
            }
            finally
            {
                Connection.Close();
            }
            return DriverID;
        }
        public static bool UpdateDriver(int DriverID, int PersonID, int CreatedByUserID)
        {
            int RowsAffected = 0;
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string Query = @"UPDATE Drivers 
                             SET PersonID = @PersonID, 
                                 CreatedByUserID = @CreatedByUserID, 
                             WHERE DriverID = @DriverID";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@DriverID", DriverID);
            Command.Parameters.AddWithValue("@PersonID", PersonID);
            Command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
            // we must not update the creation date for the driver
            try
            {
                Connection.Open();
                RowsAffected = Command.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                RowsAffected = 0;
                clsEventLogger.SaveLog("Application", $"{ex.Message}: failed through updating" +
                    $" driver with driver ID = {DriverID} for person ID = {PersonID}.", EventLogEntryType.Error);
            }
            finally
            {
                Connection.Close();
            }
            return RowsAffected > 0;
        }
        public static DataTable GetAllDrivers()
        {
            DataTable dt = new DataTable();
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string Query = @"SELECT * FROM Drivers_View order by FullName";
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
                    $"all drivers.", EventLogEntryType.Error);
            }
            finally
            {
                Connection.Close();
            }
            return dt;
        }
    }
}