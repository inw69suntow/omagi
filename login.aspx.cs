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

public partial class _login: System.Web.UI.Page
{
    protected void btt1_Click(object sender, EventArgs e)
    {
        try
        {
            if ((pwd.Text != "") && (eotp.Text.Trim().CompareTo(Session["rand"].ToString().Trim()) == 0))
            {
                string uID = uid.Text;
                long uPWD = pwd.Text.GetHashCode();
                OleDbConnection Conn = new OleDbConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString);
                OleDbCommand command = Conn.CreateCommand();
                DateTime tmpNow = new DateTime(DateTime.Now.Year , DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

                string sql = " SELECT distinct fl_user_id,fl_user_time from tb_logontime ";
                sql = sql + " where FL_USER_ID = ? ";
                sql = sql + " and fl_user_time>='" + tmpNow.Year + tmpNow.Month.ToString().PadLeft(2, '0') + tmpNow.Day.ToString().PadLeft(2, '0') + tmpNow.Hour.ToString().PadLeft(2, '0') + tmpNow.Minute.ToString().PadLeft(2, '0') + "' ";
                Conn.Open();
                command.CommandText = sql;
                command.Connection = Conn;
                command.Parameters.Clear();
                command.Parameters.AddWithValue("", uID);
                OleDbDataReader rs = command.ExecuteReader();

                if (rs.Read())
                {
                    lblResponse.Text = "This login is currently in-use";
                }
                else
                {
                    lblResponse.Text = "Incorrect login or password";
                }
                rs.Close();

                if (lblResponse.Text == "This login is currently in-use")
                {
                    lblResponse.Visible = true;
                    Conn.Close();
                    return;
                }

                Session.Clear();
                Session["uID"] = "";
                Session["uName"] = "";

                Session["uGroup"] = "";

                Session["uDept"] = "";
                Session["uCitizen"] = "";

                Session["UserUpdater"] = "";
                Session["UserUpdateTime"] = "";
                Session["UserLastLogin"] = "";

                Session["alert"] = "";
                
                sql = " SELECT distinct fl_user_id,fl_user_name, ";
                sql = sql + " fl_user_group,fl_user_dept,fl_citizen,";
                sql = sql + " fl_update_by,fl_update_time,fl_last_login ";
                sql = sql + " from tb_User ";
                sql = sql + " where fl_user_id =? ";
                sql = sql + " AND fl_user_password=? ";
                sql = sql + " AND fl_user_status='1' ";

                //Conn.Open();
                command.Connection = Conn;
                
                command.CommandText = sql;
                command.Parameters.Clear();
                command.Parameters.AddWithValue("", uID);
                command.Parameters.AddWithValue("", uPWD);
                
                bool found = false;
                rs = command.ExecuteReader();
                while (rs.Read())
                {
                    found = true;
                    if (!rs.IsDBNull(0)) Session["uID"] = rs.GetString(0);
                    if (!rs.IsDBNull(1)) Session["uName"] = rs.GetString(1);

                    if (!rs.IsDBNull(2)) Session["uGroup"] = rs.GetString(2);
                    if (!rs.IsDBNull(3)) Session["uDept"] = rs.GetString(3);
                    if (!rs.IsDBNull(4)) Session["uCitizen"] = rs.GetString(4);

                    if (!rs.IsDBNull(5)) Session["UserUpdater"] = rs.GetString(5);
                    if (!rs.IsDBNull(6)) Session["UserUpdateTime"] = rs.GetString(6);
                    if (!rs.IsDBNull(7)) Session["UserLastLogin"] = rs.GetString(7);
                }
                rs.Close();
                
                Session.Timeout = 30;

                if (Session["UserLastLogin"].ToString().Trim() == "")
                {
                    Session["alert"] = "รหัสผ่านสำหรับท่านเป็นรหัสผ่านครั้งเดียวสำหรับการใช้งานเป็นครั้งแรก โปรดปรับปรุงรหัสผ่านของท่านทันที";
                    sql = "UPDATE tb_User set fl_user_status='0' where fl_user_id=? ";
                    command.CommandText = sql;
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("", uID);
                
                    if(found)command.ExecuteNonQuery();

                    sql = "UPDATE tb_user set fl_last_login='" + DateTime.Now.Year + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0') + DateTime.Now.Hour.ToString().PadLeft(2, '0') + DateTime.Now.Minute.ToString().PadLeft(2, '0') + DateTime.Now.Second.ToString().PadLeft(2, '0') + "' where fl_user_id='" + Session["uID"].ToString().Replace("'", "''") + "' ";
                    command.CommandText = sql;
                    command.ExecuteNonQuery();
                }

                if (Session["UserUpdateTime"].ToString() != "")
                {
                    string dF = Session["UserUpdateTime"].ToString().Substring(0, 4) + "-" + Session["UserUpdateTime"].ToString().Substring(4, 2) + "-" + Session["UserUpdateTime"].ToString().Substring(6, 2);
                    string nowDate = DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString().PadLeft(2, '0') + "-" + DateTime.Now.Day.ToString().PadLeft(2, '0');
                    TimeSpan dSP = System.Convert.ToDateTime(nowDate).Subtract(System.Convert.ToDateTime(dF));
                    dF = dSP.TotalDays.ToString();
                    if(dF.IndexOf('.')>0) dF = dF.Substring(0, dF.IndexOf('.'));
                    long dTotal = Convert.ToInt32(dF);

                    //Buddhist Year Correction
                    if (dTotal >= 198327) dTotal = dTotal - 198327;

                    if (dTotal > 90)
                    {
                        if (dTotal < 100)
                        {
                            Session["alert"] = "รหัสผ่านสำหรับท่านไม่ได้ทำการปรับปรุงมานานกว่า 90 วัน โปรดปรับปรุงรหัสผ่านของท่านภายใน " + ((int)(100 - dTotal)).ToString() + " วัน";
                        }
                        else
                        {
                            Session["alert"] = "รหัสผ่านสำหรับท่านไม่ได้ทำการปรับปรุงมานานกว่า 90 วัน โปรดปรับปรุงรหัสผ่านของท่านทันที";
                            sql = "UPDATE tb_User set fl_user_status='0' where fl_user_id=? ";
                            command.CommandText = sql;
                            command.Parameters.Clear();
                            command.Parameters.AddWithValue("", uID);
                            command.ExecuteNonQuery();
                        }
                    }
                }

                if (Session["uID"].ToString() != "")
                {
                    sql = "DELETE tb_logontime where fl_user_id='" + Session["uID"].ToString().Replace("'", "''") + "' ";
                    command.CommandText = sql;
                    command.ExecuteNonQuery();
                    
                    tmpNow = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                    tmpNow = tmpNow.AddMinutes(20);

                    sql = "INSERT INTO tb_logontime(fl_user_id,fl_user_time) values ('" + Session["uID"].ToString().Replace("'", "''") + "','" + tmpNow.Year + tmpNow.Month.ToString().PadLeft(2, '0') + tmpNow.Day.ToString().PadLeft(2, '0') + tmpNow.Hour.ToString().PadLeft(2, '0') + tmpNow.Minute.ToString().PadLeft(2, '0') + tmpNow.Second.ToString().PadLeft(2, '0') + "') ";
                    command.CommandText = sql;
                    command.ExecuteNonQuery();

                    sql = "UPDATE tb_user set fl_last_login='" + DateTime.Now.Year + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0') + DateTime.Now.Hour.ToString().PadLeft(2, '0') + DateTime.Now.Minute.ToString().PadLeft(2, '0') + DateTime.Now.Second.ToString().PadLeft(2, '0') + "' ";
                    sql += " where fl_user_id=?";
                    command.CommandText = sql;
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("", uID);
                    command.ExecuteNonQuery();

                    tmpNow = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                    tmpNow=tmpNow.AddMinutes(20);

                    sql = "INSERT INTO tb_LOG(fl_id,fl_module,fl_action,fl_keyword,fl_datetime,fl_ip) VALUES(";
                    sql = sql + " '" + Session["uID"].ToString().Replace("'", "''") + "', ";
                    sql = sql + " 'Login', ";
                    sql = sql + " 'LOGIN', ";
                    sql = sql + " '" + Session["uID"].ToString().Replace("'", "''") + "', ";
                    sql = sql + " '" + DateTime.Now.Year + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0') + DateTime.Now.Hour.ToString().PadLeft(2, '0') + DateTime.Now.Minute.ToString().PadLeft(2, '0') + DateTime.Now.Second.ToString().PadLeft(2, '0') + "', ";
                    sql = sql + " '" + Request.UserHostAddress + "') ";
                    command.CommandText = sql;
                    command.ExecuteNonQuery();

                    //Delete old Log
                    DateTime tmpTime = DateTime.Now.AddYears(-1);
                    sql = "DELETE tb_LOG where fl_datetime<'" + tmpTime.Year.ToString() + tmpTime.Month.ToString().PadLeft(2, '0') + tmpTime.Day.ToString().PadLeft(2, '0') + "' ";
                    command.CommandText = sql;
                    command.ExecuteNonQuery();
                    sql = "DELETE tb_CHATLOG where fl_datetime<'" + tmpTime.Year.ToString() + tmpTime.Month.ToString().PadLeft(2, '0') + tmpTime.Day.ToString().PadLeft(2, '0') + "' ";
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
                    sql += " '" + Session["uID"].ToString().Replace("'", "''") + " เข้าสู่ระบบ'); ";
                    command.CommandText = sql;
                    command.ExecuteNonQuery();

                    Conn.Close();

                    Response.Redirect("blank.aspx",true);
                }
                else
                {
                    Conn.Close();
                    Response.Redirect("login.aspx?lbl=true", true);
                }
                Conn.Close();
            }
            else
            {
                Response.Redirect("login.aspx?lbl=true", true);
            }
        }
        catch(Exception ex)
        {
            Response.Redirect("login.aspx?lbl=true",true);            
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if ((Session["uName"] != null) && (Session["uName"].ToString() != ""))
            {
                Response.Redirect("blank.aspx");
                return;
            }
        }
        catch (Exception ex)
        {}

        if (Request.QueryString["lbl"] != null)
        {
            if (Request.QueryString["lbl"].ToString() == "true") lblResponse.Visible = true;
        }

        try
        {
            if (!Page.IsPostBack)
            {
                Random rand = new Random();
                Session["rand"] = (rand.Next() % 100000).ToString();
                Session["rand"] = Session["rand"].ToString().PadLeft(5, '0');
                otp.Value = Session["rand"].ToString();
                Img0.ImageUrl = "photo/" + Session["rand"].ToString()[0] + ".gif";
                Img1.ImageUrl = "photo/" + Session["rand"].ToString()[1] + ".gif";
                Img2.ImageUrl = "photo/" + Session["rand"].ToString()[2] + ".gif";
                Img3.ImageUrl = "photo/" + Session["rand"].ToString()[3] + ".gif";
                Img4.ImageUrl = "photo/" + Session["rand"].ToString()[4] + ".gif";

                Session.Timeout = 5;
            }
        }
        catch (Exception ex) { }
      }
}