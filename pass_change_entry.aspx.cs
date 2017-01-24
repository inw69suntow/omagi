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

public partial class _pass_change_entry: System.Web.UI.Page
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

        if (!Page.IsPostBack)
        {
                Label1.Text = Session["uName"].ToString();
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
        lblResponse2.Text = "";
        lblResponse3.Text = "";

        bool errFound = false;
        if (txtOld.Text == "")
        {
            lblResponse1.Text = "บันทึกรหัสผ่านเดิม";
            lblResponse1.ForeColor = System.Drawing.Color.Red;
            errFound = true;            
        }

        if (txtPass1.Text == "")
        {
            lblResponse2.Text = "บันทึกรหัสผ่านใหม่";
            lblResponse2.ForeColor = System.Drawing.Color.Red;
            errFound = true;
        }

        if (txtPass1.Text != "")
        {
            if (txtPass1.Text.Length < 8)
            {
                lblResponse2.Text = "รหัสผ่านสั้นกว่า 8 อักษร";
                lblResponse2.ForeColor = System.Drawing.Color.Red;
                errFound = true;
            }
        }

        if (txtPass2.Text == "")
        {
            lblResponse3.Text = "บันทึกรหัสผ่านซ้ำ";
            lblResponse3.ForeColor = System.Drawing.Color.Red;
            errFound = true;
        }

        if (txtPass2.Text != "")
        {
            if (txtPass2.Text.Length < 8)
            {
                lblResponse3.Text = "รหัสผ่านสั้นกว่า 8 อักษร";
                lblResponse3.ForeColor = System.Drawing.Color.Red;
                errFound = true;
            }
        }

        string tmpPass = "";
        if ((txtPass1.Text == txtPass2.Text) && (txtPass1.Text.Trim() != ""))
        {
            tmpPass = txtPass1.Text;
        }

        if ((tmpPass.Trim().Length < 8) && (txtPass1.Text.Trim() != ""))
        {
            tmpPass = "";
            lblResponse3.Text = "รหัสผ่านไม่ตรงกัน";
            lblResponse3.ForeColor = System.Drawing.Color.Red;
            errFound = true;
        }

        OleDbConnection Conn = new OleDbConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString);
        OleDbCommand command = new OleDbCommand();

        Conn.Open();
        command.Connection = Conn;
        string sql ="";

        if (txtOld.Text != "")
        {
            sql = "SELECT distinct fl_user_id from tb_user ";
            sql = sql + " where fl_user_id='" + Session["uID"].ToString().Replace("'", "''") + "' ";
            sql = sql + " and fl_user_password='" + txtOld.Text.GetHashCode() + "' ";

            command.CommandText = sql;            
            OleDbDataReader rs = command.ExecuteReader();
            if (!rs.Read())
            {
                lblResponse1.Text = "รหัสผ่านเดิมไม่ถูกต้อง";
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

        if (tmpPass.Trim() != "")
        {
            sql = "UPDATE tb_USER SET ";
            sql = sql + " fl_user_password=" + tmpPass.GetHashCode() + ", ";
            sql = sql + " fl_user_status='1', ";
            sql = sql + "fl_update_by='" + Session["uID"].ToString().Replace("'", "''") + "', ";
            sql = sql + "fl_update_ip='" + Request.UserHostAddress + "', ";
            sql = sql + "fl_update_time='" + DateTime.Now.Year + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0') + DateTime.Now.Hour.ToString().PadLeft(2, '0') + DateTime.Now.Minute.ToString().PadLeft(2, '0') + DateTime.Now.Second.ToString().PadLeft(2, '0') + "' ";

            sql = sql + " where fl_user_id='" + Session["uID"].ToString().Replace("'", "''") + "' ";
            command.CommandText = sql;
            command.ExecuteNonQuery();

            sql = "INSERT INTO tb_LOG(fl_id,fl_module,fl_action,fl_keyword,fl_datetime,fl_ip) VALUES(";
            sql = sql + " '" + Session["uID"].ToString().Replace("'", "''") + "', ";
            sql = sql + " 'User Data (PASS)', ";
            sql = sql + " 'UPDATE', ";
            sql = sql + " '" + Session["uID"].ToString().Replace("'", "''") + "', ";
            sql = sql + " '" + DateTime.Now.Year + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0') + DateTime.Now.Hour.ToString().PadLeft(2, '0') + DateTime.Now.Minute.ToString().PadLeft(2, '0') + DateTime.Now.Second.ToString().PadLeft(2, '0') + "', ";
            sql = sql + " '" + Request.UserHostAddress + "') ";
            command.CommandText = sql;
            command.ExecuteNonQuery();

            lblResponse.Text = "ปรับปรุงข้อมูลสำเร็จ";
            lblResponse.ForeColor = System.Drawing.Color.Green;
            Conn.Close();
        }

        if (Session["alert"].ToString().Trim() != "")
        {
            lblResponse.Text = Session["alert"].ToString().Trim();
            lblResponse.ForeColor = System.Drawing.Color.Red;
        }

        Session["alert"] = "";
    }
    protected void brnClear_Click(object sender, EventArgs e)
    {
        lblResponse1.Text = "";
        lblResponse2.Text = "";
        lblResponse3.Text = "";

        lblResponse.Text = "";
        txtOld.Text = "";
        txtPass1.Text = "";
        txtPass2.Text = "";
    }
}