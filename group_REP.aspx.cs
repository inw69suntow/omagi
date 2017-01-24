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

public partial class _group_REP: System.Web.UI.Page
{


    protected void makeRep(string c, string x, string y)
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
        tab1.Rows[0].Cells[0].InnerHtml = "ลำดับ";
        tab1.Rows[0].Cells[0].RowSpan = 1;
        tab1.Rows[0].Cells[0].Width = "10px";

        tab1.Rows[0].Cells.Add(new HtmlTableCell());
        tab1.Rows[0].Cells[1].ColSpan = 1;
        tab1.Rows[0].Cells[1].InnerHtml = "<center>ชื่อ - สกุล</center>";
        tab1.Rows[0].Cells[1].Width = "150px";
        tab1.Rows[0].Cells.Add(new HtmlTableCell());
        tab1.Rows[0].Cells[2].ColSpan = 1;
        tab1.Rows[0].Cells[2].InnerHtml = "<center>ตำแหน่ง</center>";
        tab1.Rows[0].Cells[2].Width = "100px";
        tab1.Rows[0].Cells.Add(new HtmlTableCell());
        tab1.Rows[0].Cells[3].ColSpan = 1;
        tab1.Rows[0].Cells[3].InnerHtml = "<center>โทรศัพท์</center>";
        tab1.Rows[0].Cells[3].Width = "100px";
        tab1.Rows[0].Cells.Add(new HtmlTableCell());
        tab1.Rows[0].Cells[4].ColSpan = 1;
        tab1.Rows[0].Cells[4].InnerHtml = "<center>มือถือ</center>";
        tab1.Rows[0].Cells[4].Width = "100px";
        tab1.Rows[0].Cells.Add(new HtmlTableCell());
        tab1.Rows[0].Cells[5].ColSpan = 1;
        tab1.Rows[0].Cells[5].InnerHtml = "<center>อีเมล์</center>";
        tab1.Rows[0].Cells[5].Width = "100px";
        tab1.Rows[0].Cells.Add(new HtmlTableCell());
        tab1.Rows[0].Cells[6].ColSpan = 1;
        tab1.Rows[0].Cells[6].InnerHtml = "<center>วันเกิด</center>";
        tab1.Rows[0].Cells[6].Width = "100px";
        tab1.Rows[0].Cells.Add(new HtmlTableCell());
        tab1.Rows[0].Cells[7].ColSpan = 1;
        tab1.Rows[0].Cells[7].InnerHtml = "<center>ปฏิบัติการ</center>";
        tab1.Rows[0].Cells[7].Width = "20px";

        tab1.Rows[0].Cells.Add(new HtmlTableCell());
        tab1.Rows[0].Cells[8].ColSpan = 1;
        tab1.Rows[0].Cells[8].InnerHtml = "<center>ปรับปรุงล่าสุด</center>";
        tab1.Rows[0].Cells[8].Width = "100px";
        #endregion

        #region whereCompose
        string whereClause = "where 1=1 ";
        if (Session["whereClause"] != null)
        {
            if (Session["whereClause"].ToString() != "") whereClause = Session["whereClause"].ToString();
        }
        if (x != "") whereClause += " and fl_group_type='" + x.Trim().Replace(";", "").Replace("'", "''") + "' ";

        if (y != "")
        {
            if (c == "1") whereClause += " and tb_detailgroup.fl_province='" + y.Trim().Replace(";", "").Replace("'", "''") + "' ";
            if (c == "2") whereClause += " and tb_detailgroup.fl_dept='" + y.Trim().Replace(";", "").Replace("'", "''") + "' ";
            if (c == "3")
            {
                whereClause += " and fl_start<'" + Convert.ToString(Convert.ToInt32(y) + 1) + "' ";
                whereClause += " and (isnull(fl_stop,'')='' or isnull(fl_stop,'') >'" + Convert.ToString(Convert.ToInt32(y) + 1) + "' ) ";
            }
        }
        
        string orderClause = " order by ";
	orderClause += " isnull(mainName,''), ";
        orderClause += " isnull(fl_dept,'ฮฮฮฮ'), ";
        orderClause += " isnull(fl_province_name,''), ";
        orderClause += " isnull(detailName,''), ";
        orderClause += " isnull(fl_pos,'Z'), ";
        orderClause += " isnull(fl_fName,'') ";
        #endregion

        #region SQLComposed
        //SQL Composed
        string sql = "SELECT distinct ";
        sql += " isnull(detailName,''), ";
        sql += " isnull(mainName,''), ";
        sql += " isnull(fl_province_name,''), ";
        sql += " isnull(fl_dept_name,''), ";
        sql += " isnull(fl_reportMember,'0'), ";
        sql += " isnull(main.fl_citizen_id,''), ";
        sql += " isnull(fl_pos,'Z'), ";
        sql += " isnull(fl_title,''), ";
        sql += " isnull(fl_fName,''), ";
        sql += " isnull(fl_sName,''), ";
        sql += " isnull(fl_telno,''), ";
        sql += " isnull(fl_mobno,''), ";
        sql += " isnull(fl_email,''), ";
        sql += " isnull(fl_birth,''), ";
        sql += " isnull(fl_age,''), ";
        sql += " isnull(fl_targetFlag,''), ";
        sql += " isnull(fl_id,''), ";
        sql += " isnull(fl_dept,'ฮฮฮฮ'), ";

        sql += " isnull(fl_update_time,'') ";

        sql += " from ";

        sql += " (";
        sql += " SELECT distinct ";
        sql += " tb_detailgroup.fl_id, ";
        sql += " tb_detailgroup.fl_groupname as detailName, ";
        sql += " tb_maingroup.fl_group_name as mainName, ";
        sql += " fl_province_name, ";
        sql += " fl_dept_name, ";
        sql += " fl_reportMember, ";
        sql += " fl_citizen_id, ";
        sql += " fl_pos, ";
	sql += " tb_detailgroup.fl_dept, ";
    sql += " tb_detailgroup.fl_update_time ";

        sql += " from tb_detailgroup ";
        sql += " inner join tb_maingroup  on tb_detailgroup.fl_group_id = tb_maingroup.fl_group_id ";
        sql += " inner join (select distinct fl_province_code,fl_province_name from tb_moicode) moiCode  on tb_detailgroup.fl_province = moiCode.fl_province_code ";
        sql += " left join (select distinct fl_dept_id,fl_dept_name from tb_dept) deptCode  on tb_detailgroup.fl_dept = deptCode.fl_dept_id ";
        sql += " left join tb_membergroup on tb_detailgroup.fl_id = tb_membergroup.fl_detailgroup_id ";
        sql += whereClause;

        sql += " ) main ";

        sql += " left outer join ";

        sql += " (";
        sql += " select distinct ";
        sql += " fl_citizen_id, ";
        sql += " isnull(tb_title.fl_title,'') as fl_title, ";
        sql += " fl_fName, ";
        sql += " fl_sName, ";
        sql += " fl_telno, ";
        sql += " fl_mobno, ";
        sql += " fl_email, ";
        sql += " fl_birth, ";
        sql += " case ltrim(rtrim(fl_birth)) when '' then 0 else datediff(year,CAST(fl_birth as datetime),getdate()) end as fl_age, ";
        sql += " fl_targetFlag ";
        sql += " from tb_citizen ";
        sql += " left join tb_title on tb_title.fl_code=tb_citizen.fl_title ";
        sql += " where tb_citizen.fl_status='1' ";
        sql += " ) sub ";
        sql += " on main.fl_citizen_id=sub.fl_citizen_id ";

        sql += orderClause;
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
        string tmpVal = "";
        while (rs.Read())
        {
            string tmpVal2=rs.GetString(0);
            if(tmpVal2.Trim()=="") tmpVal2=rs.GetString(1);
            if (tmpVal != rs.GetString(16))
            {
                tab1.Rows.Add(new HtmlTableRow());
                i = tab1.Rows.Count - 1;
                tab1.Rows[i].Attributes.Add("class", "break1");
                tab1.Rows[i].Cells.Clear();

                for (int xx = 0; xx < 4; xx++)
                {
                    tab1.Rows[i].Cells.Add(new HtmlTableCell());
                    tab1.Rows[i].Cells[xx].ColSpan = 2;
                    tab1.Rows[i].Cells[xx].InnerHtml = "&nbsp;";
                }

                tmpVal = rs.GetString(16);

                tab1.Rows[i].Cells[0].InnerHtml = "&nbsp;<b>" + tmpVal2 + "</b>";
                tab1.Rows[i].Cells[1].InnerHtml = "&nbsp;<b>จังหวัด" + rs.GetString(2) + "</b>";
                tab1.Rows[i].Cells[2].InnerHtml = "&nbsp;<b>หน่วยงาน" + rs.GetString(3) + "</b>";
                tab1.Rows[i].Cells[3].InnerHtml = "&nbsp;<b>รายงาน&nbsp;" + Convert.ToInt64("0" + rs.GetString(4)).ToString("#,##0") + "&nbsp;คน</b>";
                tab1.Rows[i].Cells[3].ColSpan = 3;
                j = 1;
            }
            if (rs.GetString(8) != "")
            {
                tab1.Rows.Add(new HtmlTableRow());
                i = tab1.Rows.Count - 1;
                tab1.Rows[i].Attributes.Add("class", "off");
                tab1.Rows[i].Attributes.Add("onmouseover", "this.className='on'");
                tab1.Rows[i].Attributes.Add("onmouseout", "this.className='off'");
                tab1.Rows[i].Cells.Clear();

                string action = "man_rep.aspx?id=" + rs.GetString(5);
                for (int xx = 0; xx <= 8; xx++)
                {
                    tab1.Rows[i].Cells.Add(new HtmlTableCell());
                    tab1.Rows[i].Cells[xx].ColSpan = 1;
                    tab1.Rows[i].Cells[xx].InnerHtml = "&nbsp;";
                }

                tab1.Rows[i].Cells[0].InnerHtml = "&nbsp;" + j.ToString();
                tab1.Rows[i].Cells[1].InnerHtml = "&nbsp;<a href='" + action + "' target='_blank'>" + rs.GetString(5) + "</a> " + rs.GetString(7) + rs.GetString(8) + " " + rs.GetString(9);
                string tmpPos = "สมาชิก";
                if (rs.GetString(6) == "P") tmpPos = "ประธานกลุ่ม";
                if (rs.GetString(6) == "V") tmpPos = "รองประธานกลุ่ม";
                if (rs.GetString(6) == "Y") tmpPos = "เลขานุการ";

                tab1.Rows[i].Cells[2].InnerHtml = "&nbsp;" + tmpPos;
                tab1.Rows[i].Cells[3].InnerHtml = "&nbsp;" + rs.GetString(10);
                tab1.Rows[i].Cells[4].InnerHtml = "&nbsp;" + rs.GetString(11);
                tab1.Rows[i].Cells[5].InnerHtml = "&nbsp;" + rs.GetString(12);
                if (rs.GetString(13) != "")
                {
                    string tmpBirth="";
                    tmpBirth += rs.GetString(13).Substring(6,2) + "/";
                    tmpBirth += rs.GetString(13).Substring(4,2) + "/";
                    tmpBirth += rs.GetString(13).Substring(0,4) + " ";
                    tmpBirth += "(" + Convert.ToInt32(rs.GetValue(14)).ToString("#") + " ปี)";
                    tab1.Rows[i].Cells[6].InnerHtml = "&nbsp;" + tmpBirth;
                }
                if (rs.GetString(15) == "1") tab1.Rows[i].Cells[7].InnerHtml = "&nbsp;ใช่";

                if (rs.GetString(18) != "")
                {
                    string tmpUpdate = "";
                    tmpUpdate += rs.GetString(18).Substring(6, 2) + "/";
                    tmpUpdate += rs.GetString(18).Substring(4, 2) + "/";
                    tmpUpdate += rs.GetString(18).Substring(0, 4) + " ";
                    tab1.Rows[i].Cells[8].InnerHtml = "&nbsp;" + tmpUpdate;
                }

                j++;
            }
        }
        maxi.Value = i.ToString();
        rs.Close();
        Conn.Close();

        bar.Visible = true;
        tab1.Visible = true;
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
	try
	{
        
		if (!Page.IsPostBack)
		{
            makeRep(Request.QueryString["c"], Request.QueryString["x"], Request.QueryString["y"]);
                tab1.Visible = true;
		}
        }catch(Exception ee){}
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
        catch (Exception ex) { if (Conn != null) { if (Conn.State == ConnectionState.Open)Conn.Close(); }  }
    }
}