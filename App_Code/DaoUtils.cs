using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.OleDb;
using System.Configuration;

/// <summary>
/// Summary description for DaoUtils
/// </summary>
public class DaoUtils
{
    public static int count(String sql)
    {
        int rCount = 0;
        String connectionStr = "";
        OleDbConnection Conn = null;
        OleDbCommand command = null;
        try
        {
            connectionStr = ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString;
            Conn = new OleDbConnection(connectionStr);
            command = new OleDbCommand();
            Conn.Open();
            command.Connection = Conn;

            //Count Page        
            command.CommandText = "SELECT IsNULL(COUNT(*),0) as c from (" + sql + ") olderS "; ;
            command.Connection = Conn;

            OleDbDataReader rs = command.ExecuteReader();
            if (rs.Read())
            {
                rCount = rs.GetInt32(0);
            }
            rs.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine( ex.StackTrace);
            throw ex;
        }
        return rCount;
    }
}