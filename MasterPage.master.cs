using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OleDb;
using System.Configuration;

public partial class MasterPage : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if ((Session["uName"] == null) || (Session["uName"].ToString() == ""))
            {
                Response.Redirect("login.aspx");
            }
        }
        catch (Exception ex)
        {
            Response.Redirect("login.aspx");
            return;
        }
    }

   
    public void insertMsg(string path)
    {
        string sesUserID = "";

        sesUserID = Session["uID"].ToString();

        string messages = txtText.Text.Trim().Replace(";", "").Replace("'", "''").Replace(Convert.ToChar(13).ToString(),"<br>");
        string subj = txtNewSubj.Text.Trim().Replace(";", "").Replace("'", "''");

        string datetime = DateTime.Now.ToString("yyyyMMddHHmmss");
        string datetime2 = DateTime.Now.AddDays(-90).ToString("yyyyMMddHHmmss");
        if (messages != "")
        {
            string sqlStr = "insert into tb_boardlog (";
            sqlStr += " FL_DATETIME, ";
            sqlStr += " FL_FILEURL, ";
            sqlStr += " FL_USER_ID, ";
            sqlStr += " FL_SUBJECT, ";
            sqlStr += " FL_TEXT) ";
            sqlStr += " values( ";
            sqlStr += " '" + datetime + "', ";
            sqlStr += " '" + path + "', ";
            sqlStr += " '" + sesUserID + "', ";
            sqlStr += " '" + subj + "', ";
            sqlStr += " '" + messages + "'); ";

            sqlStr += "delete from tb_boardlog where fl_datetime<'" + datetime2 + "'; ";

            OleDbConnection conn = new OleDbConnection();
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString;
            conn.Open();
            OleDbCommand command = new OleDbCommand(sqlStr, conn);
            command.ExecuteNonQuery();

            conn.Close();
            selectMsg();
        }
    }

    protected void selectMsg()
    {
        lblShoutM.Text = "";

        OleDbConnection conn = new OleDbConnection();
        conn.ConnectionString = ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString;
        conn.Open();
        string datetime2 = DateTime.Now.AddDays(-15).ToString("yyyyMMddHHmmss");
        string sql = "select FL_MESSAGE_ID, ";
        sql += " FL_USER_ID, ";
        sql += " FL_TEXT, ";
        sql += " FL_DATETIME, ";
        sql += " FL_SUBJECT, ";
        sql += " FL_FILEURL ";
        sql += " from tb_boardlog ";
        sql += " where fl_datetime >='" + datetime2 + "' ";
        sql += " order by FL_MESSAGE_ID DESC; ";
        OleDbCommand command = new OleDbCommand(sql, conn);
        OleDbDataReader reader = command.ExecuteReader();
        string userID = "";
        string subj = "";
	    string dt = "";
        while (reader.Read())
        {
            userID = reader.GetString(1);
        	dt= reader.GetString(3).Substring(6,2) + "/" + reader.GetString(3).Substring(4,2) +  "/" + reader.GetString(3).Substring(0,4) + " " + reader.GetString(3).Substring(8,2) + ":" + reader.GetString(3).Substring(10,2) + ":" + reader.GetString(3).Substring(12,2);
            subj = reader.GetString(4);
            string str = "<font color='green'>" + reader.GetString(1) + " : " + dt + "</font><BR><font color=red>เรื่อง : </font>" + subj + "</font> " + Server.HtmlDecode(reader.GetString(5))  + "<br>" + Server.HtmlDecode(reader.GetString(2));
            lblShoutM.Text += str + "<BR /> ";
        }
        reader.Close();
        conn.Close();
    }

    protected void  btnSubmit_Click(object sender, EventArgs e)
    {
        string path = "";
        string datetime2 = DateTime.Now.ToString("yyyyMMddHHmmss");
        if ((txtFile.PostedFile != null) && (txtFile.PostedFile.ContentLength > 0))
        {
            if (txtFile.PostedFile.ContentLength < 4096000)
            {
                string fileName = System.IO.Path.GetFileName(txtFile.PostedFile.FileName);
                path += "fileUpload/" + Session["uID"].ToString() + "_" + datetime2 + "_" + fileName;
                try
                {
                    txtFile.SaveAs(Server.MapPath(path));
                    path = "<a href=\"" + path + "\" target=_blank><img src=photo/file.png border=0 width=20px height=20px></a>";

                    path = Server.HtmlEncode(path);
                }
                catch (Exception ex)
                { Response.Write(ex.Message); }
            }
            else
            {
                Response.Write("<script>alert('ไฟล์มีขนาดใหญ่เกินกำหนด ไม่สามารถอัพโหลดได้');</script>");
            }            
        }
        if (txtText.Text.Trim() != "")
        {
            lblShoutM.Text = "";
            insertMsg(path);
            txtNewSubj.Text = "";
            txtText.Text = "";
            txtNewSubj.Focus();
        }
    }
}
