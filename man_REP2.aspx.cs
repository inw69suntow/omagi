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
using System.Web.UI.DataVisualization.Charting;

public partial class _man_REP2: System.Web.UI.Page
{
    protected void makeMain()
    {
        string id = "";

        string sql = "SELECT distinct ";
        sql += "fl_citizen_id,";
        sql += "isnull(tb_title.fl_title,''),";
        sql += "fl_fname, ";
        sql += "fl_sname, ";
        sql += "fl_addrno,";
        sql += "fl_moono,";
        sql += "fl_home,";
        sql += "fl_soy,";
        sql += "fl_road,";
        sql += "isnull(a.fl_province_name,''),";
        sql += "isnull(b.fl_district_name,''),";
        sql += "isnull(c.fl_tambon_name,''),";
        sql += "fl_postcode,";
        sql += "fl_telno,";
        sql += "fl_offno,";
        sql += "fl_mobno,";
        sql += "fl_email,";
        sql += "fl_birth,";
        sql += " case ltrim(rtrim(fl_birth)) when '' then 0 else datediff(year,CAST(fl_birth as datetime),getdate()) end as fl_age, ";
        sql += "isnull(fl_targetFlag,''),";
        sql += "isnull(fl_google,'') ";

        sql += " from tb_citizen ";
        sql += " left join tb_title on tb_title.fl_code=tb_citizen.fl_title ";
        sql += " left join (select distinct fl_province_code,fl_province_name from tb_moicode ) a on tb_citizen.fl_province_code=a.fl_province_code ";
        sql += " left join (select distinct fl_district_code,fl_district_name from tb_moicode ) b on tb_citizen.fl_district=b.fl_district_code ";
        sql += " left join (select distinct fl_tambon_code,fl_tambon_name from tb_moicode ) c on tb_citizen.fl_tambon=c.fl_tambon_code ";

        sql += " where 1=1";
        if (txtID.Text.Trim() !="") sql += " and fl_citizen_id like '" + txtID.Text.Trim().Replace(";","").Replace("'","''") + "%' ";
        if (txtName.Text.Trim() != "") sql += " and fl_fname like '" + txtName.Text.Trim().Replace(";", "").Replace("'", "''") + "%' ";
        if (txtSurname.Text.Trim() != "") sql += " and fl_sname like '" + txtSurname.Text.Trim().Replace(";", "").Replace("'", "''") + "%' ";
        if (txtMobile.Text.Trim() != "") sql += " and fl_mobno like '" + txtMobile.Text.Trim().Replace(";", "").Replace("'", "''") + "%' ";
        if (txtHome.Text.Trim() != "") sql += " and fl_telno like '" + txtHome.Text.Trim().Replace(";", "").Replace("'", "''") + "%' ";
        if (txtOffice.Text.Trim() != "") sql += " and fl_offno like '" + txtOffice.Text.Trim().Replace(";", "").Replace("'", "''") + "%' ";

        OleDbConnection Conn = new OleDbConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString);
        OleDbCommand command = new OleDbCommand();

        Conn.Open();

        command.CommandText = sql;
        command.CommandTimeout = 180;
        command.Connection = Conn;
        OleDbDataReader rs = command.ExecuteReader();

        if (rs.Read())
        {
            lblName.Text = "";
            lblName.Text = rs.GetString(0) + " : " + rs.GetString(1) + rs.GetString(2) + " " + rs.GetString(3);

            id = rs.GetString(0);

            if (System.IO.File.Exists(Server.MapPath("CIMG") + "\\" + rs.GetString(0) + ".jpg"))
            {
                imgName.ImageUrl = "CIMG/" + rs.GetString(0) + ".jpg";
            }
            else
            {
                imgName.ImageUrl = "CIMG/blank.gif";
            }

            lblAddress.Text = "";
            lblAddress.Text += rs.GetString(4) + " ";
            if (rs.GetString(6).Trim() != "") lblAddress.Text += "หมู่บ้าน/อาคาร" + rs.GetString(6) + " ";
            if (rs.GetString(5).Trim() != "") lblAddress.Text += "หมู่ " + rs.GetString(5) + "<BR>";
            if (rs.GetString(7).Trim() != "") lblAddress.Text += "ซอย" + rs.GetString(7) + " ";
            if (rs.GetString(8).Trim() != "") lblAddress.Text += "ถนน" + rs.GetString(8) + "<BR>";
            if (rs.GetString(11).Trim() != "") lblAddress.Text += "ตำบล/แขวง" + rs.GetString(11) + " ";
            if (rs.GetString(10).Trim() != "") lblAddress.Text += "อำเภอ/เขต" + rs.GetString(10) + "<BR>";
            if (rs.GetString(9).Trim() != "") lblAddress.Text += "จังหวัด" + rs.GetString(9) + " ";
            if (rs.GetString(12).Trim() != "") lblAddress.Text += "" + rs.GetString(12) + " ";

            lblTelNo.Text = rs.GetString(13);
            lblOffNo.Text = rs.GetString(14);
            lblMobNo.Text = rs.GetString(15);
            lblEmail.Text = rs.GetString(16);

            if(rs.GetString(17).Trim()!="") 
            {
                string tmpBirth="";
                tmpBirth += rs.GetString(17).Substring(6,2) + "/";
                tmpBirth += rs.GetString(17).Substring(4,2) + "/";
                tmpBirth += Convert.ToString(Convert.ToInt32(rs.GetString(17).Substring(0,4)) + 1) + " ";
                tmpBirth += "(" + rs.GetValue(18) + " ปี)";

                lblBirth.Text = tmpBirth;
            }

            if (rs.GetString(19) == "1") lblTarget.Text = "ยินดีเข้าร่วมกิจกรรมตามที่กอ.รมน.ร้องขอโดยสม่ำเสมอ"; else lblTarget.Text = "";
            txtGoogle.Value = rs.GetString(20);
        }
        rs.Close();
        Conn.Close();

        makeRep1(id);
        makeRep2(id);
    }
    protected void makeRep1(string id)
    {
        #region header
        tab1.Rows.Clear();
        tab1.BorderColor = ConfigurationManager.AppSettings["gridBorderColor"];
        tab1.Border = 1;
        tab1.CellPadding = 0;
        tab1.CellSpacing = 0;


        tab1.Rows.Add(new HtmlTableRow());
        tab1.Rows[0].Attributes.Add("class", "head");
        tab1.Rows[0].Cells.Clear();

        tab1.Rows[0].Cells.Add(new HtmlTableCell());
        tab1.Rows[0].Cells[0].InnerHtml = "ลำดับที่";
        tab1.Rows[0].Cells[0].RowSpan = 1;
        tab1.Rows[0].Cells[0].Width = "10px";

        tab1.Rows[0].Cells.Add(new HtmlTableCell());
        tab1.Rows[0].Cells[1].ColSpan = 1;
        tab1.Rows[0].Cells[1].InnerHtml = "<center>ชื่อมวลชน</center>";
        tab1.Rows[0].Cells[1].Width = "200px";
        tab1.Rows[0].Cells.Add(new HtmlTableCell());
        tab1.Rows[0].Cells[2].ColSpan = 1;
        tab1.Rows[0].Cells[2].InnerHtml = "<center>จังหวัด</center>";
        tab1.Rows[0].Cells[2].Width = "100px";
        tab1.Rows[0].Cells.Add(new HtmlTableCell());
        tab1.Rows[0].Cells[3].ColSpan = 1;
        tab1.Rows[0].Cells[3].InnerHtml = "<center>หน่วยงาน</center>";
        tab1.Rows[0].Cells[3].Width = "100px";
        tab1.Rows[0].Cells.Add(new HtmlTableCell());
        tab1.Rows[0].Cells[4].ColSpan = 1;
        tab1.Rows[0].Cells[4].InnerHtml = "<center>ตำแหน่ง</center>";
        tab1.Rows[0].Cells[4].Width = "100px";
        tab1.Rows[0].Cells.Add(new HtmlTableCell());
        tab1.Rows[0].Cells[5].ColSpan = 1;
        tab1.Rows[0].Cells[5].InnerHtml = "<center>ระหว่างวันที่</center>";
        tab1.Rows[0].Cells[5].Width = "100px";
        #endregion

        #region SQLComposed
        //SQL Composed
        string sql = "SELECT distinct ";

        sql += " tb_detailgroup.fl_id, ";
        sql += " tb_detailgroup.fl_groupname as detailName, ";
        sql += " tb_maingroup.fl_group_name as mainName, ";
        sql += " fl_province_name, ";
        sql += " isnull(fl_dept_name,''), ";
        sql += " fl_pos, ";
        sql += " fl_start, ";
        sql += " fl_stop, ";
        sql += " case fl_pos when 'P' then '1' when 'V' then '2' else '3' end posSort ";
        sql += " from tb_detailgroup ";
        sql += " inner join tb_maingroup  on tb_detailgroup.fl_group_id = tb_maingroup.fl_group_id ";
        sql += " inner join (select distinct fl_province_code,fl_province_name from tb_moicode) moiCode  on tb_detailgroup.fl_province = moiCode.fl_province_code ";
        sql += " inner join tb_membergroup on tb_detailgroup.fl_id = tb_membergroup.fl_detailgroup_id ";
        sql += " left join (select distinct fl_dept_id,fl_dept_name from tb_dept) deptCode  on tb_detailgroup.fl_dept = deptCode.fl_dept_id ";
        sql += " where fl_citizen_id='" + id.Trim().Replace(";", "").Replace("'", "''") + "' ";
        sql += " order by posSort, fl_province_name,fl_stop,fl_start ";        
        #endregion

        OleDbConnection Conn = new OleDbConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString);
        OleDbCommand command = new OleDbCommand();

        Conn.Open();

        command.CommandText = sql;
        command.CommandTimeout = 180;
        command.Connection = Conn;
        OleDbDataReader rs = command.ExecuteReader();

        int i = 1;
        int j = 1;
        while (rs.Read())
        {
            tab1.Rows.Add(new HtmlTableRow());
            i = tab1.Rows.Count - 1;
            tab1.Rows[i].Attributes.Add("class", "off");
            tab1.Rows[i].Attributes.Add("onmouseover", "this.className='on'");
            tab1.Rows[i].Attributes.Add("onmouseout", "this.className='off'");
            tab1.Rows[i].Cells.Clear();

            for (int xx = 0; xx < 6; xx++)
            {
                tab1.Rows[i].Cells.Add(new HtmlTableCell());
                tab1.Rows[i].Cells[xx].ColSpan = 1;
                tab1.Rows[i].Cells[xx].InnerHtml = "&nbsp;";
            }

            tab1.Rows[i].Cells[0].InnerHtml = "&nbsp;" + j.ToString();
            string tmpVal=rs.GetString(1);
            if(tmpVal.Trim()=="") tmpVal=rs.GetString(2);
            tab1.Rows[i].Cells[1].InnerHtml = "&nbsp;" + tmpVal;
            tab1.Rows[i].Cells[2].InnerHtml = "&nbsp;" + rs.GetString(3);
            tab1.Rows[i].Cells[3].InnerHtml = "&nbsp;" + rs.GetString(4);
            string tmpPos = "สมาชิก";
            if (rs.GetString(5) == "P") tmpPos = "ประธานกลุ่ม";
            if (rs.GetString(5) == "V") tmpPos = "รองประธานกลุ่ม";

            tab1.Rows[i].Cells[4].InnerHtml = "&nbsp;" + tmpPos;

            if (rs.GetString(6).Trim() != "")
            {
                string frdDate = rs.GetString(6).Substring(6, 2) + "/" + rs.GetString(6).Substring(4, 2) + "/" + Convert.ToString(Convert.ToUInt32(rs.GetString(6).Substring(0, 4)) + 543);
                string todDate = "";
                if(rs.GetString(7).Trim()!="") todDate=rs.GetString(7).Substring(6, 2) + "/" + rs.GetString(7).Substring(4, 2) + "/" + Convert.ToString(Convert.ToUInt32(rs.GetString(7).Substring(0, 4)) + 543);
                tab1.Rows[i].Cells[5].InnerHtml = "&nbsp;" + frdDate + "-" + todDate;
            }
            j++;
        }
        rs.Close();
        Conn.Close();
        if (j == 1)
        {
            //noDataFound
            tab1.Rows.Add(new HtmlTableRow());
            tab1.Rows[i].Attributes.Add("class", "off");
            tab1.Rows[i].Attributes.Add("onmouseover", "this.className='on'");
            tab1.Rows[i].Attributes.Add("onmouseout", "this.className='off'");
            tab1.Rows[i].Cells.Add(new HtmlTableCell());
            tab1.Rows[i].Cells[0].ColSpan = 6;
            tab1.Rows[i].Cells[0].Align = "CENTER";
            tab1.Rows[i].Cells[0].InnerHtml = "<font color='red' size='4'><b>" + ConfigurationManager.AppSettings["nodataMsg"] + "</b></font>";
        }
    }

    protected void makeRep2(string id)
    {
        #region header
        tab2.Rows.Clear();
        tab2.BorderColor = ConfigurationManager.AppSettings["gridBorderColor"];
        tab2.Border = 1;
        tab2.CellPadding = 0;
        tab2.CellSpacing = 0;


        tab2.Rows.Add(new HtmlTableRow());
        tab2.Rows[0].Attributes.Add("class", "head");
        tab2.Rows[0].Cells.Clear();

        tab2.Rows[0].Cells.Add(new HtmlTableCell());
        tab2.Rows[0].Cells[0].InnerHtml = "ลำดับที่";
        tab2.Rows[0].Cells[0].RowSpan = 1;
        tab2.Rows[0].Cells[0].Width = "10px";

        tab2.Rows[0].Cells.Add(new HtmlTableCell());
        tab2.Rows[0].Cells[1].ColSpan = 1;
        tab2.Rows[0].Cells[1].InnerHtml = "<center>ชื่อหลักสูตร</center>";
        tab2.Rows[0].Cells[1].Width = "200px";
        tab2.Rows[0].Cells.Add(new HtmlTableCell());
        tab2.Rows[0].Cells[2].ColSpan = 1;
        tab2.Rows[0].Cells[2].InnerHtml = "<center>จังหวัด</center>";
        tab2.Rows[0].Cells[2].Width = "100px";
        tab2.Rows[0].Cells.Add(new HtmlTableCell());
        tab2.Rows[0].Cells[3].ColSpan = 1;
        tab2.Rows[0].Cells[3].InnerHtml = "<center>หน่วยงาน</center>";
        tab2.Rows[0].Cells[3].Width = "100px";
        tab2.Rows[0].Cells.Add(new HtmlTableCell());
        tab2.Rows[0].Cells[4].ColSpan = 1;
        tab2.Rows[0].Cells[4].InnerHtml = "<center>รุ่นที่</center>";
        tab2.Rows[0].Cells[4].Width = "100px";
        tab2.Rows[0].Cells.Add(new HtmlTableCell());
        tab2.Rows[0].Cells[5].ColSpan = 1;
        tab2.Rows[0].Cells[5].InnerHtml = "<center>ตำแหน่ง</center>";
        tab2.Rows[0].Cells[5].Width = "100px";
        tab2.Rows[0].Cells.Add(new HtmlTableCell());
        tab2.Rows[0].Cells[6].ColSpan = 1;
        tab2.Rows[0].Cells[6].InnerHtml = "<center>ระหว่างวันที่</center>";
        tab2.Rows[0].Cells[6].Width = "100px";
        #endregion

        #region SQLComposed
        //SQL Composed
        string sql = "SELECT distinct ";

        sql += " fl_course , ";
        sql += " fl_province_name, ";
        sql += " isnull(fl_dept_name,''), ";
        sql += " fl_year, ";
        sql += " fl_gen, ";
        sql += " fl_pos, ";
        sql += " fl_start, ";
        sql += " fl_stop, ";
        sql += " case fl_pos when 'P' then '1' when 'V' then '2' else '3' end posSort ";
        sql += " from tb_train_detail ";
        sql += " inner join (select distinct fl_province_code,fl_province_name from tb_moicode) moiCode  on tb_train_detail.fl_province_code = moiCode.fl_province_code ";
        sql += " left join (select distinct fl_dept_id,fl_dept_name from tb_dept) deptCode  on tb_train_detail.fl_dept = deptCode.fl_dept_id ";
        sql += " where fl_citizen_id='" + id.Trim().Replace(";", "").Replace("'", "''") + "' ";
        sql += " order by posSort, fl_province_name,fl_stop,fl_start,fl_year,fl_gen ";
        #endregion

        OleDbConnection Conn = new OleDbConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString);
        OleDbCommand command = new OleDbCommand();

        Conn.Open();

        command.CommandText = sql;
        command.CommandTimeout = 180;
        command.Connection = Conn;
        OleDbDataReader rs = command.ExecuteReader();

        int i = 1;
        int j = 1;
        while (rs.Read())
        {
            tab2.Rows.Add(new HtmlTableRow());
            i = tab2.Rows.Count - 1;
            tab2.Rows[i].Attributes.Add("class", "off");
            tab2.Rows[i].Attributes.Add("onmouseover", "this.className='on'");
            tab2.Rows[i].Attributes.Add("onmouseout", "this.className='off'");
            tab2.Rows[i].Cells.Clear();

            for (int xx = 0; xx < 7; xx++)
            {
                tab2.Rows[i].Cells.Add(new HtmlTableCell());
                tab2.Rows[i].Cells[xx].ColSpan = 1;
                tab2.Rows[i].Cells[xx].InnerHtml = "&nbsp;";
            }

            tab2.Rows[i].Cells[0].InnerHtml = "&nbsp;" + j.ToString();
            tab2.Rows[i].Cells[1].InnerHtml = "&nbsp;" + rs.GetString(0);
            tab2.Rows[i].Cells[2].InnerHtml = "&nbsp;" + rs.GetString(1);
            tab2.Rows[i].Cells[3].InnerHtml = "&nbsp;" + rs.GetString(2);
            tab2.Rows[i].Cells[4].InnerHtml = "&nbsp;" + rs.GetString(3) + "/" + rs.GetString(4);
            string tmpPos = "สมาชิก";
            if (rs.GetString(5) == "P") tmpPos = "ประธานกลุ่ม";
            if (rs.GetString(5) == "V") tmpPos = "รองประธานกลุ่ม";

            tab2.Rows[i].Cells[5].InnerHtml = "&nbsp;" + tmpPos;

            if (rs.GetString(6).Trim() != "")
            {
                string frdDate = rs.GetString(6).Substring(6, 2) + "/" + rs.GetString(6).Substring(4, 2) + "/" + Convert.ToString(Convert.ToUInt32(rs.GetString(6).Substring(0, 4)) + 543);
                string todDate = "";
                if (rs.GetString(7).Trim() != "") todDate = rs.GetString(7).Substring(6, 2) + "/" + rs.GetString(7).Substring(4, 2) + "/" + Convert.ToString(Convert.ToUInt32(rs.GetString(7).Substring(0, 4)) + 543);
                tab2.Rows[i].Cells[6].InnerHtml = "&nbsp;" + frdDate + "-" + todDate;
            }
            j++;
        }
        rs.Close();
        Conn.Close();
        if (j == 1)
        {
            //noDataFound
            tab2.Rows.Add(new HtmlTableRow());
            tab2.Rows[i].Attributes.Add("class", "off");
            tab2.Rows[i].Attributes.Add("onmouseover", "this.className='on'");
            tab2.Rows[i].Attributes.Add("onmouseout", "this.className='off'");
            tab2.Rows[i].Cells.Add(new HtmlTableCell());
            tab2.Rows[i].Cells[0].ColSpan = 7;
            tab2.Rows[i].Cells[0].Align = "CENTER";
            tab2.Rows[i].Cells[0].InnerHtml = "<font color='red' size='4'><b>" + ConfigurationManager.AppSettings["nodataMsg"] + "</b></font>";
        }
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
        }catch(Exception ex)
        {
            Response.Redirect("login.aspx");
            return;
        }

        //Inject Client Script
        HtmlGenericControl body = (HtmlGenericControl)Master.FindControl("masterBody");
        body.Attributes.Add("onload", "initialize()");

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
    protected void btnSearch_Click(object sender, EventArgs e)
    {
            makeMain();
            tab1.Visible = true;
    }
}