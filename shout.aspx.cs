using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OleDb;
using System.Configuration;
using System.Web.Script.Services;
using System.Web.Services;
using System.Drawing;

    public partial class shout : System.Web.UI.Page
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
            if (Session["uID"] == null)
            {
                Session["uID"] = "0";
            }

            if (Session["msgID"] == null)
            {
                Session["msgID"] = "0";
            }
            selectMsg();

            //set all date time into culture
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
        }
        public void insertMsg(string messages)
        {
            string sesUserID = "";

            sesUserID = Session["uID"].ToString();

            string datetime = DateTime.Now.ToString("yyyyMMddHHmmss");
            string datetime2 = DateTime.Now.AddDays(-90).ToString("yyyyMMddHHmmss");

            if (messages != "")
            {
                string sqlStr = "insert into tb_chatlog (";
                sqlStr += " FL_DATETIME, ";
                sqlStr += " FL_USER_ID, ";
                sqlStr += " FL_TEXT) ";
                sqlStr += " values( ";
                sqlStr += " '" + datetime + "', ";
                sqlStr += " '" + sesUserID + "', ";
                sqlStr += " '" + messages + "'); ";

                sqlStr += "delete from tb_chatLog where fl_datetime<'" + datetime2 +"'; ";

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
            string sesUserID = "";
            string sesMsgID = "";

            sesUserID = Session["uID"].ToString();
            sesMsgID = Session["msgID"].ToString();

            OleDbConnection conn = new OleDbConnection();
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString;
            conn.Open();
            string sql ="select TOP 100 FL_MESSAGE_ID, ";
            sql += " FL_USER_ID, ";
            sql += " FL_TEXT, ";
            sql += " FL_DATETIME ";
            sql += " from tb_chatlog ";
            sql += " order by FL_MESSAGE_ID DESC; ";

            OleDbCommand command = new OleDbCommand(sql, conn);
            OleDbDataReader reader = command.ExecuteReader();
            string msgID = "";
            string userID = "";
            string text = "";
	        string dt = "";
            while (reader.Read())
            {
                msgID = reader.GetValue(0).ToString();
                userID = reader.GetString(1);
                text = reader.GetString(2);
        		dt= reader.GetString(3).Substring(6,2) + "/" + reader.GetString(3).Substring(4,2) +  "/" + reader.GetString(3).Substring(0,4) + " " + reader.GetString(3).Substring(8,2) + ":" + reader.GetString(3).Substring(10,2) + ":" + reader.GetString(3).Substring(12,2);  

                string str = "<font color='green'>" + reader.GetString(1) + " : " + dt + "</font><BR />" + Server.HtmlDecode(reader.GetString(2));
                long iMsgID1 = Convert.ToInt64(msgID);
                long iMsgID2 = Convert.ToInt64(sesMsgID);

                if (userID == "SYSTEM")
                {
                    string str1 = userID;
                    string str2 = Server.HtmlDecode(text);
                    str = "<font color='green'>" + str1 + " : " + dt + "</font><BR />" + str2;
                    str = "<font color='red'>" + "<font color='green'>" + str1 + " : " + dt + "</font><BR />" + str2 + "</font>";
                }
                lblShoutM.Text += str + "<BR /> ";
            }
            conn.Close();

            conn.Open();
            OleDbCommand sesCommand = new OleDbCommand("select max(FL_MESSAGE_ID) from tb_chatlog", conn);
            OleDbDataReader sesReader = command.ExecuteReader();
            if (sesReader.Read())
            {
                Session["msgID"] = sesReader.GetValue(0).ToString();
            }
            conn.Close();
        }
        protected void btnUP_Click(object sender, EventArgs e)
        {
            string txtStr = txtShoutS.Text;

            if (txtStr != "")
            {
                txtStr = txtStr.Replace("'", "''");
                txtStr = txtStr.Replace(";", "");
                string msg = txtStr;
                lblShoutM.Text = "";
                insertMsg(msg);
                msg = "";
                txtShoutS.Text = "";
                txtShoutS.Focus();
            }

            if ((fuShout.PostedFile != null) && (fuShout.PostedFile.ContentLength > 0))
            {
                string path = "";
                if (fuShout.PostedFile.ContentLength < 4096000)
                {
                    string fileName = System.IO.Path.GetFileName(fuShout.PostedFile.FileName);
                    path += "fileUpload/" + fileName;
                    try
                    {
                        fuShout.SaveAs(Server.MapPath(path));
                        string filePath = "<a href=\"" + path + "\" target=_blank>รับไฟล์</a>";

                        string msg = Server.HtmlEncode(filePath);
                        lblShoutM.Text = "";
                        insertMsg(msg);
                        msg = "";
                    }
                    catch (Exception ex)
                    {
                        string s = ex.Message.ToString();
                        //Response.Write("Error: " + ex.Message);
                    }
                }
                else
                {
                    txtShoutS.ForeColor = Color.Red;
                    txtShoutS.Text = "ไฟล์มีขนาดใหญ่เกินกำหนด ไม่สามารถอัพโหลดได้";
                }
            }
        }
    }
