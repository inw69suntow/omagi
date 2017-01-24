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
using System.Text;
using System.Data.OleDb;
using System.Net.Mail;

public partial class _mail_entry: System.Web.UI.Page
{
    override protected void OnInit(EventArgs e)
    {
        btnDel.Attributes.Add("onclick",
                  this.GetPostBackEventReference(btnDel));
        base.OnInit(e);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if ((Session["uName"] == null) || (Session["uName"].ToString() == ""))
            {
                Response.Redirect("login.aspx");
                return;
            }
        }
        catch (Exception ex)
        {
            Response.Redirect("login.aspx");
            return;
        }
        if (!Page.IsPostBack)
        {
            if(Session["workFolder"]==null) Session["workFolder"]="";
            if(Session["workFolder"].ToString().Trim()!="")
            {
                try
                {
                    //Delete last work folder
                    if (System.IO.Directory.Exists(Server.MapPath(Session["workFolder"].ToString())))
                    {
                        System.IO.Directory.Delete(Server.MapPath(Session["workFolder"].ToString()), true);
                    }

                    foreach (string ff in System.IO.Directory.GetDirectories(Server.MapPath("tmpAtt") , Session["uID"].ToString() + "_*"))
                    {
                        System.IO.Directory.Delete(ff, true);
                    }
                }
                catch (Exception ex)
                {
                    Response.Write(ex.Message + "<BR>" + ex.StackTrace);
                }
            }
            Session["workFolder"] = "tmpAtt/" + Session["uID"].ToString() + "_" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0') + DateTime.Now.Hour.ToString().PadLeft(2, '0') + DateTime.Now.Minute.ToString().PadLeft(2, '0') + DateTime.Now.Second.ToString().PadLeft(2, '0');
            try
            {
                //Delete last work folder
                if (System.IO.Directory.Exists(Server.MapPath(Session["workFolder"].ToString())))
                {
                    System.IO.Directory.Delete(Server.MapPath(Session["workFolder"].ToString()), true);
                }

                System.IO.Directory.CreateDirectory(Server.MapPath(Session["workFolder"].ToString()));
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message + "<BR>" + ex.StackTrace);
            }
        }
        userDataSet();

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

    protected void userDataSet()
    {
        dtGrid.Rows.Clear();
        dtGrid.BorderColor = ConfigurationManager.AppSettings["gridBorderColor"];
        dtGrid.Border = 1;
        dtGrid.CellPadding = 0;
        dtGrid.CellSpacing = 0;
        int i = 0;

        foreach(string ff in System.IO.Directory.GetFiles(Server.MapPath(Session["workFolder"].ToString())))
        {
	    string ff2=System.IO.Path.GetFileName(ff);
            dtGrid.Rows.Add(new HtmlTableRow());
            dtGrid.Rows[i].Attributes.Add("class", "off");

            dtGrid.Rows[i].Cells.Add(new HtmlTableCell());
            dtGrid.Rows[i].Cells[0].ColSpan = 1;
            dtGrid.Rows[i].Cells[0].Align="CENTER";
            dtGrid.Rows[i].Cells[0].Width="5%";
            dtGrid.Rows[i].Cells[0].InnerHtml = "<input type='checkbox' id='check_" + i.ToString() + "'\">";

            dtGrid.Rows[i].Cells.Add(new HtmlTableCell());
            dtGrid.Rows[i].Cells[1].ColSpan = 1;
            dtGrid.Rows[i].Cells[1].Align="Left";
            dtGrid.Rows[i].Cells[1].Width="95%";
            dtGrid.Rows[i].Cells[1].InnerHtml = ff2 + "<input type='hidden' id='name_" + i.ToString() + "' value='" + ff2 +"'>";

            i++;
        }
	maxi.Value=i.ToString();
    }

    protected void btnAttach_Click(object sender, EventArgs e)
    {
        if ((txtAtt.PostedFile != null) && (txtAtt.PostedFile.ContentLength > 0))
        {
            string path = "";
            if (txtAtt.PostedFile.ContentLength < 4096000)
            {
                string fileName = System.IO.Path.GetFileName(txtAtt.PostedFile.FileName);
                path += Session["workFolder"].ToString() + "/" + fileName;
                try
                {
                    txtAtt.SaveAs(Server.MapPath(path));
                }
                catch (Exception ex)
                {
                    lblResponse.Text = ex.Message.ToString();
                    lblResponse.ForeColor = System.Drawing.Color.Red;
                }
            }
            else
            {
                lblResponse.Text = "ไฟล์ใหญ่เกิน 4 MB";
                lblResponse.ForeColor = System.Drawing.Color.Red;
            }
        }
        userDataSet();
    }
    protected void btnDel_Click(object sender, EventArgs e)
    {
        string path = "";
        if(selFile.Value.Trim()!="")
        {
            string[] sFile = selFile.Value.Trim().Split('|');
            foreach (string s in sFile)
            {
                string fileName = System.IO.Path.GetFileName(s);
txtMsg.Text += fileName;
                path += Session["workFolder"].ToString() + "/" + fileName;
                try
                {
                    System.IO.File.Delete(Server.MapPath(path));
                }
                catch (Exception ex)
                {
Response.Write(ex.Message + "<BR>" + ex.StackTrace);
                }
            }
        }
        userDataSet();
    }
    protected void btnSend_Click(object sender, EventArgs e)
    {                
        if (txtReply.Text.Trim() == "")
        {
            lblResponse.Text = "ไม่มีที่อยู่ผู้ส่ง";
            lblResponse.ForeColor = System.Drawing.Color.Red;
            userDataSet();
            return;
        }

        if (txtTo.Text.Trim() == "")
        {
            lblResponse.Text = "ไม่มีที่อยู่ผู้รับ";
            lblResponse.ForeColor = System.Drawing.Color.Red;
            userDataSet();
            return;
        }

        if (txtSubj.Text.Trim() == "")
        {
            lblResponse.Text = "ไม่มีชื่อเรื่อง";
            lblResponse.ForeColor = System.Drawing.Color.Red;
            userDataSet();
            return;
        }

        MailAddress tmpCheck;
        MailMessage mailMessage = new MailMessage();
        try
        {
            tmpCheck = new MailAddress(txtReply.Text.Trim());
            mailMessage.ReplyToList.Add(tmpCheck);
        }
        catch (Exception ee)
        {
            lblResponse.Text = "ที่อยู่ผู้ส่งไม่ถูกต้อง";
            lblResponse.ForeColor = System.Drawing.Color.Red;
            userDataSet();
            return;
        }

        string[] tmpAddrList = txtTo.Text.Trim().Split(';');
        foreach (string s in tmpAddrList)
        {
            try
            {
                tmpCheck = new MailAddress(s);
                mailMessage.To.Add(tmpCheck);
            }
            catch (Exception ee)
            {            }
        }

        tmpAddrList = txtCc.Text.Trim().Split(';');
        foreach (string s in tmpAddrList)
        {
            try
            {
                tmpCheck = new MailAddress(s);
                mailMessage.CC.Add(tmpCheck);
            }
            catch (Exception ee)
            { }
        }

        tmpAddrList = txtBcc.Text.Trim().Split(';');
        foreach (string s in tmpAddrList)
        {
            try
            {
                tmpCheck = new MailAddress(s);
                mailMessage.Bcc.Add(tmpCheck);
            }
            catch (Exception ee)
            { }
        }

        mailMessage.Subject = txtSubj.Text.Trim();
        mailMessage.From = new MailAddress(ConfigurationManager.AppSettings["frMail"].ToString().Trim());

        foreach (string ff in System.IO.Directory.GetFiles(Server.MapPath(Session["workFolder"].ToString())))
        {
            mailMessage.Attachments.Add(new Attachment(ff));
        }

        mailMessage.Body = txtMsg.Text.Replace(Convert.ToChar(13).ToString(), "<br>");
        mailMessage.IsBodyHtml = true;

        SmtpClient smtpC = new SmtpClient(ConfigurationManager.AppSettings["serverMail"].ToString());

        try
        {
            smtpC.Send(mailMessage);
        }
        catch (Exception ex) {
            lblResponse.Text = ex.Message;
            lblResponse.ForeColor = System.Drawing.Color.Red;
            userDataSet();
            return;
	 }
	lblResponse.Text = "ส่งสำเร็จ";
        lblResponse.ForeColor = System.Drawing.Color.Green;

	txtTo.Text="";
	txtCc.Text="";
	txtBcc.Text="";
    }
}