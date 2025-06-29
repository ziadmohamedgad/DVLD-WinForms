using DVLD_DataAccess;
using DVLD_Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
namespace DVLD_DataLayer
{
    public class clsApplicationTypeData
    {
        public static DataTable GetAllApplicationTypes()
        {
            DataTable dt = new DataTable();
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string Query = @"SELECT * FROM ApplicationTypes ORDER By ApplicationTypeTitle";
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
                clsEventLogger.SaveLog("Application", $"{ex.Message}: failed through fetching application types.", EventLogEntryType.Error);
            }
            finally
            {
                Connection.Close();
            }
            return dt;
        }
        public static bool GetApplicationTypeInfoByID(int ID, ref string Title, ref float Fees)
        {
            bool IsFound = false;
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string Query = @"SELECT * FROM ApplicationTypes WHERE ApplicationTypeID = @ID";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@ID", ID);
            try
            {
                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();
                if (Reader.Read())
                {
                    IsFound = true;
                    Title = (string)Reader["ApplicationTypeTitle"];
                    Fees = Convert.ToSingle(Reader["ApplicationFees"]);
                }
                Reader.Close();
            }
            catch (SqlException ex)
            {
                IsFound = false;
                clsEventLogger.SaveLog("Application", $"{ex.Message}: failed through fetching " +
                    $"application type info with application type ID = {ID}.", EventLogEntryType.Error);
            }
            finally
            {
                Connection.Close();
            }
            return IsFound;
        }
        public static int AddNewApplicationType(string Title, float Fees) // Just IN Case Implementing Add New Feature
        {
            int ApplicationTypeID = -1;
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string Query = @"INSERT INTO ApplicationTypes (ApplicationTypeTitle, ApplicationFees) 
                             VALUES (@Title, @Fees); 
                             SELECT SCOPE_IDENTITY();";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@Title", Title);
            Command.Parameters.AddWithValue("@Fees", Fees);
            try
            {
                object Result = Command.ExecuteScalar();
                if (Result != null && int.TryParse(Result.ToString(), out int InsertedID))
                {
                    ApplicationTypeID = InsertedID;
                }
            }
            catch (SqlException ex)
            {
                ApplicationTypeID = -1;
                clsEventLogger.SaveLog("Application", $"{ex.Message}: failed through adding " +
                    $"new application type.", EventLogEntryType.Error);
            }
            finally
            {
                Connection.Close();
            }
            return ApplicationTypeID;
        }
        public static bool UpdateApplicationType(int ID, string Title, float Fees)
        {
            int RowsAffected = 0;
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string Query = @"UPDATE ApplicationTypes 
                            SET ApplicationTypeTitle = @Title,
                                ApplicationFees = @Fees 
                                WHERE ApplicationTypeID = @ID";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@ID", ID);
            Command.Parameters.AddWithValue("@Title", Title);
            Command.Parameters.AddWithValue("@Fees", Fees);
            try
            {
                Connection.Open();
                RowsAffected = Command.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                RowsAffected = 0;
                clsEventLogger.SaveLog("Application", $"{ex.Message}: failed through updating" +
                    $"application type with application type ID = {ID}.", EventLogEntryType.Error);
            }
            finally
            {
                Connection.Close();
            }
            return RowsAffected > 0;
        }
    }
}