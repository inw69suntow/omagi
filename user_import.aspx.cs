using System;
using System.Configuration;
using System.Collections;
using System.Net;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Web.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;

public partial class _user_import: System.Web.UI.Page
{
    protected void DataSet()
    {
        OleDbConnection Conn = new OleDbConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString);
        OleDbCommand command = new OleDbCommand();

        string sql = "SELECT distinct fl_user_id,fl_citizen from tb_user where fl_user_group='U' ";

        Conn.Open();

        command.CommandText = sql;
        command.Connection = Conn;
        OleDbDataReader rs = command.ExecuteReader();
	sql ="";
        while (rs.Read())
        {
            sql += "UPDATE tb_user set fl_user_password=" + rs.GetString(1).GetHashCode() + " where fl_user_id='" + rs.GetString(0) + "'; ";
        }
        rs.Close();
        command.CommandText = sql;
	command.ExecuteNonQuery();
        Conn.Close();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
	DataSet();
    }

}