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

public partial class _logoff: System.Web.UI.Page
{    
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            OleDbConnection Conn = new OleDbConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString);
            OleDbCommand command = new OleDbCommand();

            Conn.Open();
            command.Connection = Conn;

            string sql = "DELETE from tb_logontime where fl_user_id='" + Session["uID"].ToString().Replace("'", "''") + "' ";
            command.CommandText = sql;
            command.ExecuteNonQuery();

            string datetime = DateTime.Now.ToString("yyyyMMddHHmmss");

            sql = "insert into tb_chatlog (";
            sql += " FL_DATETIME, ";
            sql += " FL_USER_ID, ";
            sql += " FL_TEXT) ";
            sql += " values( ";
            sql += " '" + datetime + "', ";
            sql += " 'SYSTEM', ";
            sql += " '" + Session["uID"].ToString().Replace("'", "''") + " ออกจากระบบ'); ";
            command.CommandText = sql;
            command.ExecuteNonQuery();

            Conn.Close();
        }
        catch (Exception ex)
        {        }
        
        Session["uID"] = "";
        Session["uName"] = "";

        Session["uGroup"] = "";

        Session["uDept"] = "";
        Session["uCitizen"] = "";

        Session["UserUpdater"] = "";
        Session["UserUpdateTime"] = "";
        Session["UserLastLogin"] = "";

        Session["alert"] = "";
        Session.Clear();
        Session.Abandon();
        Response.Redirect("login.aspx");
    }
}