using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OleDb;
using System.Configuration;

public partial class testdb : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        OleDbConnection Conn =null;
        try
        {
            lbPassword.Text = txtPass.Text.GetHashCode()+"";
            Conn = new OleDbConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString);
            OleDbCommand command = Conn.CreateCommand();
            string sql = " SELECT distinct fl_user_id,fl_user_time from tb_logontime ";
            Conn.Open();
            command.CommandText = sql;
            command.Connection = Conn;
            // command.Parameters.Clear();
            OleDbDataReader rs = command.ExecuteReader();
            if (rs.Read())
            {
                lblResponse.Text = "This login is currently in-use";
            }
            rs.Close();
            lblResponse.Text = "connection success " + Conn.ConnectionString;
        }
        catch (Exception ex)
        {
            lblResponse.Text = ex.Message;
        }
        finally
        {
            Conn.Close();
        }
    }
}