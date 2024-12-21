using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace SRBJWithDate.App_Start
{
    public class DataAccessLayer
    {
        string Str = ConfigurationManager.ConnectionStrings["SRBJ_WithDate"].ConnectionString;
        SqlConnection Con; SqlCommand Cmd;
        #region Execute NonQuery
        public bool InsertData(string StoredProcedure, string[] ParameterNames, string[] ParameterValues)
        {
            bool Result = false;
            try
            {
                Con = new SqlConnection(Str); Cmd = new SqlCommand(); Cmd.Connection = Con; Cmd.CommandText = StoredProcedure; Cmd.Parameters.Clear();
                if (ParameterNames.Length == ParameterValues.Length)
                {
                    Cmd.CommandType = CommandType.StoredProcedure;
                    for (int i = 0; i < ParameterNames.Length; i++)
                    {
                        Cmd.Parameters.AddWithValue(ParameterNames[i], ParameterValues[i]);
                    }
                    Con.Open();
                    int A = Cmd.ExecuteNonQuery();
                    if (A > 0)
                        Result = true;
                    else
                        Result = false;
                }
                else { Console.WriteLine("ParameterNames and Values are not equal."); }
            }
            catch (Exception Ex)
            {
                new Exception("Something went wrong.", Ex);
            }
            finally
            {
                if (Con.State == ConnectionState.Open)
                {
                    Con.Close();
                }
            }
            return Result;
        }
        #endregion
        #region Ececute Scalar
        public string InsertExecuteScalar(string StoredProcedure, string[] ParameterNames, string[] ParameterValues)
        {
            string Result = string.Empty;
            try
            {
                Con = new SqlConnection(Str); Cmd = new SqlCommand(); Cmd.Connection = Con; Cmd.CommandText = StoredProcedure; Cmd.Parameters.Clear();
                if (ParameterNames.Length == ParameterValues.Length)
                {
                    Cmd.CommandType = CommandType.StoredProcedure;
                    for (int i = 0; i < ParameterNames.Length; i++)
                    {
                        Cmd.Parameters.AddWithValue(ParameterNames[i], ParameterValues[i]);
                    }
                    Con.Open();
                    string EmpCode = Cmd.ExecuteScalar().ToString();
                    if (!string.IsNullOrEmpty(EmpCode))
                        Result = EmpCode;
                    else
                        Result = "Something went wrong";

                }
            }
            catch (Exception Ex)
            {
                Result = $"Something went wrong.{Ex.Message}";
            }
            return Result;
        }
        #endregion
        #region RetriveData
        public DataSet RetrivedData(string StoredProcedure, string[] ParameterNames = null, string[] ParameterValues = null)
        {
            DataSet Ds = new DataSet();
            try
            {
                Con = new SqlConnection(Str); Cmd = new SqlCommand(); Cmd.Connection = Con; Cmd.CommandText = StoredProcedure; Cmd.Parameters.Clear();
                if (ParameterNames != null && ParameterValues != null)
                {
                    Cmd.CommandType = CommandType.StoredProcedure;
                    for (int i = 0; i < ParameterNames.Length; i++)
                    {
                        Cmd.Parameters.AddWithValue(ParameterNames[i], ParameterValues[i]);
                    }
                }
                SqlDataAdapter Da = new SqlDataAdapter(Cmd);
                Da.Fill(Ds);
                if (Ds.Tables.Count == 0 || Ds.Tables[0].Rows.Count > 0)
                    throw new Exception("No data retrieved from Database.");
            }
            catch (Exception Ex)
            {
                new Exception("Something went wrong.", Ex);
            }
            return Ds;
        }
        #endregion
    }
}