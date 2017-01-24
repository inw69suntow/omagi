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

        try
        {
            //set all date time into culture
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            if (!Page.IsPostBack)
            {
                makeMenu();
                lblUname.Text = Session["uName"].ToString();
            }
            selectMsg();
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message + "<br>" + ex.StackTrace);
        }
    }

    protected void makeMenuA()
    {
        string sql = "";
        OleDbConnection Conn = new OleDbConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString);
        OleDbCommand command = new OleDbCommand();
        Conn.Open();
        command.Connection = Conn;
        OleDbDataReader rs;

        TreeView1.Items.Add(new MenuItem("<font color=#51290f>บันทึกข้อมูล</font>", "", "", ""));
        TreeView1.Items[TreeView1.Items.Count - 1].Selectable = false;
        TreeView1.Items[TreeView1.Items.Count - 1].ChildItems.Add(new MenuItem("<font color=#51290f>บันทึกข้อมูลกลุ่มมวลชน</font>", "", "", "mass_entry.aspx"));
        TreeView1.Items[TreeView1.Items.Count - 1].ChildItems.Add(new MenuItem("<font color=#51290f>บันทึกข้อมูลการฝึกอบรม</font>", "", "", "train_member_entry.aspx"));
        TreeView1.Items[TreeView1.Items.Count - 1].ChildItems.Add(new MenuItem("<font color=#51290f>บันทึกข้อมูลหลักสูตรการฝึกอบรม</font>", "", "", "train_course_entry.aspx"));

        TreeView1.Items.Add(new MenuItem("รายงาน", "", "", ""));
        TreeView1.Items[TreeView1.Items.Count - 1].Selectable = false;
        TreeView1.Items[TreeView1.Items.Count - 1].ChildItems.Add(new MenuItem("<font color=#51290f>รายงานข้อมูลมวลชนและกราฟ</font>", "", "", "report_REP.aspx"));

        TreeView1.Items[TreeView1.Items.Count - 1].ChildItems.Add(new MenuItem("<font color=#51290f>รายงานข้อมูลมวลชนสำหรับผู้บริหาร</font>", "", "", ""));
        TreeView1.Items[TreeView1.Items.Count - 1].ChildItems[1].Selectable=false;

        MenuItem tmpItem = TreeView1.Items[TreeView1.Items.Count - 1].ChildItems[1];
        tmpItem.ChildItems.Add(new MenuItem("<font color=#51290f>รายงานสรุปยอดมวลชน</font>", "", "", ""));
        tmpItem.ChildItems[0].Selectable = false;
        tmpItem = tmpItem.ChildItems[0];

        tmpItem.ChildItems.Add(new MenuItem("<font color=#51290f>สรุปภาพรวม</font>", "", "", "summary_REP.aspx?t=0"));
        tmpItem.ChildItems.Add(new MenuItem("<font color=#51290f>รายจังหวัดทั้งหมด</font>", "", "", "summary_REP.aspx?t=1"));

        sql = "SELECT distinct fl_dept_id,fl_dept_name from tb_dept where fl_dept_status='1' ";
        sql += " and fl_dept_id like '%00' ";
        sql += " order by fl_dept_id ";

        command.CommandText = sql;
        rs = command.ExecuteReader();
        while (rs.Read())
        {
            tmpItem.ChildItems.Add(new MenuItem("<font color=#51290f>" + rs.GetString(1) + "</font>", "", "", "summary_REP.aspx?t=1&id=" + rs.GetString(0)));
        }
        rs.Close();

        tmpItem = TreeView1.Items[TreeView1.Items.Count - 1].ChildItems[1];
        tmpItem.ChildItems.Add(new MenuItem("<font color=#51290f>รายงานรายละเอียดข้อมูลมวลชน</font>", "", "", ""));
        tmpItem.ChildItems[1].Selectable = false;
        tmpItem = tmpItem.ChildItems[1];

        tmpItem.ChildItems.Add(new MenuItem("<font color=#51290f>จำแนกจังหวัด</font>", "", "", ""));
        tmpItem.ChildItems[0].Selectable = false;
        
        MenuItem tmpItem2 = tmpItem.ChildItems[0];

        tmpItem2.ChildItems.Add(new MenuItem("<font color=#51290f>ทุกกลุ่มมวลชน</font>", "", "", "detail_REP.aspx"));
        tmpItem2.ChildItems.Add(new MenuItem("<font color=#51290f>มวลชนกอ.รมน.</font>", "", "", "detail_REP.aspx?t=1"));

        MenuItem tmpItem3 = tmpItem2.ChildItems[1];
        sql = "SELECT distinct fl_group_id,fl_group_name";
        sql += " from tb_maingroup where fl_group_status='1' ";
        sql += " and fl_group_type='1' ";
        sql += " order by fl_group_id ";

        command.CommandText = sql;
        rs = command.ExecuteReader();
        while (rs.Read())
        {
            tmpItem3.ChildItems.Add(new MenuItem("<font color=#51290f>" + rs.GetString(1) + "</font>", "", "", "detail_REP.aspx?t=1&id=" + rs.GetString(0)));
        }
        rs.Close();

        tmpItem2.ChildItems.Add(new MenuItem("<font color=#51290f>มวลชนภาครัฐ</font>", "", "", "detail_REP.aspx?t=2"));
        tmpItem2.ChildItems.Add(new MenuItem("<font color=#51290f>มวลชนภาคประชาชน</font>", "", "", "detail_REP.aspx?t=3"));

        tmpItem.ChildItems.Add(new MenuItem("<font color=#51290f>จำแนกกลุ่ม</font>", "", "", ""));
        tmpItem.ChildItems[1].Selectable = false;

        tmpItem2 = tmpItem.ChildItems[1];

        tmpItem2.ChildItems.Add(new MenuItem("<font color=#51290f>ทุกจังหวัด</font>", "", "", "detail_REP2.aspx"));

        tmpItem3 = tmpItem2.ChildItems[0];
        sql = "SELECT distinct fl_dept_id,fl_dept_name from tb_dept where fl_dept_status='1' ";
        sql += " and fl_dept_id like '%00' ";
        sql += " order by fl_dept_id ";

        command.CommandText = sql;
        rs = command.ExecuteReader();
        while (rs.Read())
        {
            tmpItem3.ChildItems.Add(new MenuItem("<font color=#51290f>" + rs.GetString(1) + "</font>", "", "", "detail_REP2.aspx?id=" + rs.GetString(0)));
        }
        rs.Close();

        TreeView1.Items[TreeView1.Items.Count - 1].ChildItems.Add(new MenuItem("<font color=#51290f>รายงานข้อมูลบุคคล</font>", "", "", "man_REP1.aspx"));

        TreeView1.Items.Add(new MenuItem("<font color=#51290f>ส่งเมล์</font>", "", "", "mail_entry.aspx"));

        TreeView1.Items.Add(new MenuItem("<font color=#51290f>เปลี่ยนรหัสผ่าน</font>", "", "", "pass_change_entry.aspx"));

        TreeView1.Items.Add(new MenuItem("<font color=#51290f>บริหารระบบ</font>", "", "", ""));
        TreeView1.Items[TreeView1.Items.Count - 1].Selectable = false;
        TreeView1.Items[TreeView1.Items.Count - 1].ChildItems.Add(new MenuItem("<font color=#51290f>บันทึกข้อมูลผู้ใช้งาน</font>", "", "", "user_entry.aspx"));
        TreeView1.Items[TreeView1.Items.Count - 1].ChildItems.Add(new MenuItem("<font color=#51290f>ปลดล๊อคผู้ใช้งาน</font>", "", "", "unlock_entry.aspx"));
        TreeView1.Items[TreeView1.Items.Count - 1].ChildItems.Add(new MenuItem("<font color=#51290f>ปูมการใช้ระบบงาน</font>", "", "", "audit_trail_rep.aspx"));

        Conn.Close();
    }

    protected void makeMenuC()
    {
        string sql = "";
        OleDbConnection Conn = new OleDbConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString);
        OleDbCommand command = new OleDbCommand();
        Conn.Open();
        command.Connection = Conn;
        OleDbDataReader rs;

        TreeView1.Items.Add(new MenuItem("<font color=#51290f>รายงาน</font>", "", "", ""));
        TreeView1.Items[TreeView1.Items.Count - 1].Selectable = false;
        TreeView1.Items[TreeView1.Items.Count - 1].ChildItems.Add(new MenuItem("<font color=#51290f>รายงานข้อมูลมวลชนและกราฟ</font>", "", "", "report_REP.aspx"));

        TreeView1.Items[TreeView1.Items.Count - 1].ChildItems.Add(new MenuItem("<font color=#51290f>รายงานข้อมูลมวลชนสำหรับผู้บริหาร</font>", "", "", ""));
        TreeView1.Items[TreeView1.Items.Count - 1].ChildItems[1].Selectable=false;

        MenuItem tmpItem = TreeView1.Items[TreeView1.Items.Count - 1].ChildItems[1];
        tmpItem.ChildItems.Add(new MenuItem("<font color=#51290f>รายงานสรุปยอดมวลชน</font>", "", "", ""));
        tmpItem.ChildItems[0].Selectable = false;
        tmpItem = tmpItem.ChildItems[0];

        tmpItem.ChildItems.Add(new MenuItem("<font color=#51290f>สรุปภาพรวม</font>", "", "", "summary_REP.aspx?t=0"));
        tmpItem.ChildItems.Add(new MenuItem("<font color=#51290f>รายจังหวัดทั้งหมด</font>", "", "", "summary_REP.aspx?t=1"));

        sql = "SELECT distinct fl_dept_id,fl_dept_name from tb_dept where fl_dept_status='1' ";
        sql += " and fl_dept_id like '%00' ";
        sql += " order by fl_dept_id ";

        command.CommandText = sql;
        rs = command.ExecuteReader();
        while (rs.Read())
        {
            tmpItem.ChildItems.Add(new MenuItem("<font color=#51290f>" + rs.GetString(1) + "</font>", "", "", "summary_REP.aspx?t=1&id=" + rs.GetString(0)));
        }
        rs.Close();

        tmpItem = TreeView1.Items[TreeView1.Items.Count - 1].ChildItems[1];
        tmpItem.ChildItems.Add(new MenuItem("<font color=#51290f>รายงานรายละเอียดข้อมูลมวลชน</font>", "", "", ""));
        tmpItem.ChildItems[1].Selectable = false;
        tmpItem = tmpItem.ChildItems[1];

        tmpItem.ChildItems.Add(new MenuItem("<font color=#51290f>จำแนกจังหวัด</font>", "", "", ""));
        tmpItem.ChildItems[0].Selectable = false;
        
        MenuItem tmpItem2 = tmpItem.ChildItems[0];

        tmpItem2.ChildItems.Add(new MenuItem("<font color=#51290f>ทุกกลุ่มมวลชน</font>", "", "", "detail_REP.aspx"));
        tmpItem2.ChildItems.Add(new MenuItem("<font color=#51290f>มวลชนกอ.รมน.</font>", "", "", "detail_REP.aspx?t=1"));

        MenuItem tmpItem3 = tmpItem2.ChildItems[1];
        sql = "SELECT distinct fl_group_id,fl_group_name";
        sql += " from tb_maingroup where fl_group_status='1' ";
        sql += " and fl_group_type='1' ";
        sql += " order by fl_group_id ";

        command.CommandText = sql;
        rs = command.ExecuteReader();
        while (rs.Read())
        {
            tmpItem3.ChildItems.Add(new MenuItem(rs.GetString(1),"","","detail_REP.aspx?t=1&id=" + rs.GetString(0)));
        }
        rs.Close();

        tmpItem2.ChildItems.Add(new MenuItem("<font color=#51290f>มวลชนภาครัฐ</font>", "", "", "detail_REP.aspx?t=2"));
        tmpItem2.ChildItems.Add(new MenuItem("<font color=#51290f>มวลชนภาคประชาชน</font>", "", "", "detail_REP.aspx?t=3"));

        tmpItem.ChildItems.Add(new MenuItem("<font color=#51290f>จำแนกกลุ่ม</font>", "", "", ""));
        tmpItem.ChildItems[1].Selectable = false;

        tmpItem2 = tmpItem.ChildItems[1];

        tmpItem2.ChildItems.Add(new MenuItem("<font color=#51290f>ทุกจังหวัด</font>", "", "", "detail_REP2.aspx"));

        tmpItem3 = tmpItem2.ChildItems[0];
        sql = "SELECT distinct fl_dept_id,fl_dept_name from tb_dept where fl_dept_status='1' ";
        sql += " and fl_dept_id like '%00' ";
        sql += " order by fl_dept_id ";

        command.CommandText = sql;
        rs = command.ExecuteReader();
        while (rs.Read())
        {
            tmpItem3.ChildItems.Add(new MenuItem("<font color=#51290f>" + rs.GetString(1) + "</font>", "", "", "detail_REP2.aspx?id=" + rs.GetString(0)));
        }
        rs.Close();

        TreeView1.Items[TreeView1.Items.Count - 1].ChildItems.Add(new MenuItem("<font color=#51290f>รายงานข้อมูลบุคคล</font>", "", "", "man_REP1.aspx"));

        TreeView1.Items.Add(new MenuItem("<font color=#51290f>เปลี่ยนรหัสผ่าน</font>", "", "", "pass_change_entry.aspx"));

        Conn.Close();
    }

    protected void makeMenuU()
    {
        string sql = "";
        OleDbConnection Conn = new OleDbConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString);
        OleDbCommand command = new OleDbCommand();
        Conn.Open();
        command.Connection = Conn;
        OleDbDataReader rs;

        TreeView1.Items.Add(new MenuItem("<font color=#51290f>บันทึกข้อมูล</font>", "", "", ""));
        TreeView1.Items[TreeView1.Items.Count - 1].Selectable = false;
        TreeView1.Items[TreeView1.Items.Count - 1].ChildItems.Add(new MenuItem("<font color=#51290f>บันทึกข้อมูลกลุ่มมวลชน</font>", "", "", "mass_entry.aspx"));
        TreeView1.Items[TreeView1.Items.Count - 1].ChildItems.Add(new MenuItem("<font color=#51290f>บันทึกข้อมูลการฝึกอบรม</font>", "", "", "train_member_entry.aspx"));

        TreeView1.Items.Add(new MenuItem("<font color=#51290f>รายงาน</font>", "", "", ""));
        TreeView1.Items[TreeView1.Items.Count - 1].Selectable = false;
        TreeView1.Items[TreeView1.Items.Count - 1].ChildItems.Add(new MenuItem("<font color=#51290f>รายงานข้อมูลมวลชนและกราฟ</font>", "", "", "report_REP.aspx"));

        TreeView1.Items[TreeView1.Items.Count - 1].ChildItems.Add(new MenuItem("<font color=#51290f>รายงานข้อมูลมวลชนสำหรับผู้บริหาร</font>", "", "", ""));
        TreeView1.Items[TreeView1.Items.Count - 1].ChildItems[1].Selectable=false;

        MenuItem tmpItem = TreeView1.Items[TreeView1.Items.Count - 1].ChildItems[1];
        tmpItem.ChildItems.Add(new MenuItem("<font color=#51290f>รายงานสรุปยอดมวลชน</font>", "", "", ""));
        tmpItem.ChildItems[0].Selectable = false;
        tmpItem = tmpItem.ChildItems[0];

        tmpItem.ChildItems.Add(new MenuItem("<font color=#51290f>สรุปภาพรวม</font>", "", "", "summary_REP.aspx?t=0"));
        tmpItem.ChildItems.Add(new MenuItem("<font color=#51290f>รายจังหวัดทั้งหมด</font>", "", "", "summary_REP.aspx?t=1"));

        sql = "SELECT distinct fl_dept_id,fl_dept_name from tb_dept where fl_dept_status='1' ";
        if (Session["uDept"].ToString().Substring(0) != "0") sql = sql + " and fl_dept_id like '" + Session["uDept"].ToString().Replace("00", "") + "%' ";
        sql += " and fl_dept_id like '%00' ";
        sql += " order by fl_dept_id ";

        command.CommandText = sql;
        rs = command.ExecuteReader();
        while (rs.Read())
        {
            tmpItem.ChildItems.Add(new MenuItem("<font color=#51290f>" + rs.GetString(1) + "</font>", "", "", "summary_REP.aspx?t=1&id=" + rs.GetString(0)));
        }
        rs.Close();

        tmpItem = TreeView1.Items[TreeView1.Items.Count - 1].ChildItems[1];
        tmpItem.ChildItems.Add(new MenuItem("<font color=#51290f>รายงานรายละเอียดข้อมูลมวลชน</font>", "", "", ""));
        tmpItem.ChildItems[1].Selectable = false;
        tmpItem = tmpItem.ChildItems[1];

        tmpItem.ChildItems.Add(new MenuItem("<font color=#51290f>จำแนกจังหวัด</font>", "", "", ""));
        tmpItem.ChildItems[0].Selectable = false;
        
        MenuItem tmpItem2 = tmpItem.ChildItems[0];

        tmpItem2.ChildItems.Add(new MenuItem("<font color=#51290f>ทุกกลุ่มมวลชน</font>", "", "", "detail_REP.aspx"));
        tmpItem2.ChildItems.Add(new MenuItem("<font color=#51290f>มวลชนกอ.รมน.</font>", "", "", "detail_REP.aspx?t=1"));

        MenuItem tmpItem3 = tmpItem2.ChildItems[1];
        sql = "SELECT distinct fl_group_id,fl_group_name";
        sql += " from tb_maingroup where fl_group_status='1' ";
        sql += " and fl_group_type='1' ";
        sql += " order by fl_group_id ";

        command.CommandText = sql;
        rs = command.ExecuteReader();
        while (rs.Read())
        {
            tmpItem3.ChildItems.Add(new MenuItem("<font color=#51290f>" + rs.GetString(1) + "</font>", "", "", "detail_REP.aspx?t=1&id=" + rs.GetString(0)));
        }
        rs.Close();

        tmpItem2.ChildItems.Add(new MenuItem("<font color=#51290f>มวลชนภาครัฐ</font>", "", "", "detail_REP.aspx?t=2"));
        tmpItem2.ChildItems.Add(new MenuItem("<font color=#51290f>มวลชนภาคประชาชน</font>", "", "", "detail_REP.aspx?t=3"));

        tmpItem.ChildItems.Add(new MenuItem("<font color=#51290f>จำแนกกลุ่ม</font>", "", "", ""));
        tmpItem.ChildItems[1].Selectable = false;

        tmpItem2 = tmpItem.ChildItems[1];

        tmpItem2.ChildItems.Add(new MenuItem("<font color=#51290f>ทุกจังหวัด</font>", "", "", "detail_REP2.aspx"));

        tmpItem3 = tmpItem2.ChildItems[0];
        sql = "SELECT distinct fl_dept_id,fl_dept_name from tb_dept where fl_dept_status='1' ";
        if (Session["uDept"].ToString().Substring(0) != "0") sql = sql + " and fl_dept_id like '" + Session["uDept"].ToString().Replace("00", "") + "%' ";
        sql += " and fl_dept_id like '%00' ";
        sql += " order by fl_dept_id ";

        command.CommandText = sql;
        rs = command.ExecuteReader();
        while (rs.Read())
        {
            tmpItem3.ChildItems.Add(new MenuItem("<font color=#51290f>" + rs.GetString(1) + "</font>", "", "", "detail_REP2.aspx?id=" + rs.GetString(0)));
        }
        rs.Close();

        TreeView1.Items[TreeView1.Items.Count - 1].ChildItems.Add(new MenuItem("<font color=#51290f>รายงานข้อมูลบุคคล</font>", "", "", "man_REP1.aspx"));

        TreeView1.Items.Add(new MenuItem("<font color=#51290f>ส่งเมล์</font>", "", "", "mail_entry.aspx"));

        TreeView1.Items.Add(new MenuItem("<font color=#51290f>เปลี่ยนรหัสผ่าน</font>", "", "", "pass_change_entry.aspx"));

        Conn.Close();
    }

    protected void makeMenu()
    {
        TreeView1.Items.Clear();
        if (Session["uGroup"].ToString() == "A") makeMenuA();
        if (Session["uGroup"].ToString() == "C") makeMenuC();
        if (Session["uGroup"].ToString() == "M") makeMenuC();
        if (Session["uGroup"].ToString() == "U") makeMenuU();
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
