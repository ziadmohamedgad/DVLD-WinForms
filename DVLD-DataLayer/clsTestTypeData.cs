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
    public class clsTestTypeData
    {
        public static int AddNewTestType(string Title, string Description, float Fees)
        {
            int TestTypeID = -1;
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string Query = @"INSERT INTO TestTypes (TestTypeTitle, TestTypeDescription, TestTypeFees) 
                            VALUES (@Title, @Description, @Fees);
                            SELECT SCOPE_IDENTITY();";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@Title", Title);
            Command.Parameters.AddWithValue("@Description", Description);
            Command.Parameters.AddWithValue("@Fees", Fees);
            try
            {
                Connection.Open();
                object Result = Command.ExecuteScalar();
                if (Result != null && int.TryParse(Result.ToString(), out int InsertedID))
                    TestTypeID = InsertedID;
            }
            catch (SqlException ex)
            {
                TestTypeID = -1;
                clsEventLogger.SaveLog("Application", $"{ex.Message}: failed through adding " +
                    $"new test type with title = {Title}.", EventLogEntryType.Error);
            }
            finally
            {
                Connection.Close();
            }
            return TestTypeID;
        }
        public static bool UpdateTestType(int ID, string Title, string Description, float Fees)
        {
            int RowsAffected = 0;
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string Query = @"UPDATE TestTypes 
                            SET TestTypeTitle = @Title, 
                                TestTypeDescription = @Description,
                                TestTypeFees = @Fees 
                                WHERE TestTypeID = @ID";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@ID", ID);
            Command.Parameters.AddWithValue("@Title", Title);
            Command.Parameters.AddWithValue("@Description", Description);
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
                    $" test type with ID = {ID}.", EventLogEntryType.Error);
            }
            finally
            {
                Connection.Close();
            }
            return RowsAffected > 0;
        }
        public static bool GetTestTypeInfoByID(int ID, ref string Title, ref string Description, ref float Fees)
        {
            bool IsFound = false;
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string Query = @"SELECT * FROM TestTypes WHERE TestTypeID = @ID";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@ID", ID);
            try
            {
                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();
                if (Reader.Read())
                {
                    IsFound = true;
                    Title = (string)Reader["TestTypeTitle"];
                    Description = (string)Reader["TestTypeDescription"];
                    Fees = Convert.ToSingle(Reader["TestTypeFees"]);
                }
                Reader.Close();
            }
            catch (SqlException ex)
            {
                IsFound = false;
                clsEventLogger.SaveLog("Application", $"{ex.Message}: failed through fetching " +
                    $"test type info with ID = {ID}.", EventLogEntryType.Error);
            }
            finally
            {
                Connection.Close();
            }
            return IsFound;
        }
        public static DataTable GetAllTestTypes()
        {
            DataTable dt = new DataTable();
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string Query = @"SELECT * FROM TestTypes ORDER BY TestTypeID";
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
                    $"all test types.", EventLogEntryType.Error);
            }
            finally
            {
                Connection.Close();
            }
            return dt;
        }
    }
}