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
using System.Data.OleDb;
using System.Data.Common;

public partial class _unlock_entry: System.Web.UI.Page
{    
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if ((Session["uName"] == null) || (Session["uName"].ToString() == ""))
            {
                Response.Redirect("login.aspx");
            }
        }catch(Exception ex)
        {
            Response.Redirect("login.aspx");
            return;
        }

        OleDbConnection Conn = null;
        try
        {
            DateTime tmpNow = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
            tmpNow = tmpNow.AddMinutes(20);
            Conn = new OleDbConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString);
            OleDbCommand command = new OleDbCommand();
            Conn.Open();
            command.Connection = Conn;

            string sql = "DELETE FROM tb_logontime where fl_user_id='" + Session["uID"].ToString().Replace("'", "''") + "' ";
            command.CommandText = sql;
            command.ExecuteNonQuery();

            sql = "INSERT INTO tb_logontime(fl_user_id,fl_user_time) values ('" + Session["uID"].ToString().Replace("'", "''") + "','" + tmpNow.Year + tmpNow.Month.ToString().PadLeft(2, '0') + tmpNow.Day.ToString().PadLeft(2, '0') + tmpNow.Hour.ToString().PadLeft(2, '0') + tmpNow.Minute.ToString().PadLeft(2, '0') + tmpNow.Second.ToString().PadLeft(2, '0') + "') ";
            command.CommandText = sql;
            command.ExecuteNonQuery();
            Conn.Close();
        }
        catch (Exception ex) { if (Conn != null) { if (Conn.State == ConnectionState.Open)Conn.Close(); } }

    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        lblResponse1.Text = "";
        lblResponse.Text = "";

        bool errFound = false;
        if (txtOld.Text == "")
        {
            lblResponse1.Text = "Entry Unlock ID";
            lblResponse1.ForeColor = System.Drawing.Color.Red;
            errFound = true;
        }


        OleDbConnection Conn = new OleDbConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString);
        OleDbCommand command = new OleDbCommand();

        Conn.Open();
        command.Connection = Conn;
        string sql = "";

        if (txtOld.Text != "")
        {
            sql = "SELECT * from tb_logontime where fl_user_id='" + txtOld.Text.Trim().Replace("'", "") + "' ";
        command.CommandText = sql;
            OleDbDataReader rs = command.ExecuteReader();
            if (!rs.Read())
            {
                lblResponse1.Text = "ID Not Found";
                lblResponse1.ForeColor = System.Drawing.Color.Red;
                errFound = true;
            }
            rs.Close();
        }

        if (errFound)
        {
            Conn.Close();
            return;
        }

       
        sql = "DELETE tb_logontime where fl_user_id='" + txtOld.Text.Trim().Replace("'","") + "' ";
        command.CommandText = sql;
        command.ExecuteNonQuery();

        sql = sql + "INSERT INTO tb_LOG(fl_id,fl_module,fl_action,fl_keyword,fl_datetime,fl_ip) VALUES(";
        sql = sql + " '" + Session["uID"].ToString().Replace("'", "''") + "', ";
        sql = sql + " 'User Data', ";
        sql = sql + " 'UNLOCK', ";
        sql = sql + " '" + txtOld.Text.Trim().Replace("'", "''") + "', ";
        sql = sql + " '" + DateTime.Now.Year + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0') + DateTime.Now.Hour.ToString().PadLeft(2, '0') + DateTime.Now.Minute.ToString().PadLeft(2, '0') + DateTime.Now.Second.ToString().PadLeft(2, '0') + "', ";
        sql = sql + " '" + Request.UserHostAddress + "') ";
        command.CommandText = sql;
        command.ExecuteNonQuery();

        lblResponse.Text = "Unlock Completed";
        lblResponse.ForeColor = System.Drawing.Color.Green;
        Conn.Close();
        Session["alert"] = "";
    }
    protected void brnClear_Click(object sender, EventArgs e)
    {
        lblResponse1.Text = "";

        lblResponse.Text = "";
        txtOld.Text = "";
    }
    protected void txtOld_TextChanged(object sender, EventArgs e)
    {
        lblResponse1.Text = "";

        OleDbConnection Conn = new OleDbConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString);
        OleDbCommand command = new OleDbCommand();

        Conn.Open();
        command.Connection = Conn;

        txtOld.Text=txtOld.Text.Replace("\"","").Replace("&","").Replace("$","").Replace("|","");

        DateTime tmpNow = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        string sql = " SELECT distinct fl_user_id from tb_logontime ";
        sql = sql + " where FL_user_id = '" + txtOld.Text.Trim().Replace("'","''") + "' ";
        sql = sql + " and fl_user_time>='" + tmpNow.Year + tmpNow.Month.ToString().PadLeft(2, '0') + tmpNow.Day.ToString().PadLeft(2, '0') + tmpNow.Hour.ToString().PadLeft(2, '0') + tmpNow.Minute.ToString().PadLeft(2, '0') + "' ";

        command.CommandText = sql;
        command.Connection = Conn;
        OleDbDataReader rs = command.ExecuteReader();
        if (!rs.Read())
        {
            lblResponse1.Text = "Not in used";
        }
        rs.Close();
        Conn.Close();
    }
}