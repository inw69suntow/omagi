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

public partial class _G3: System.Web.UI.Page
{

    protected void makeChart1(int subType)
    {
        #region setSub2
        //Set Legends
        sub2.Legends.Clear();
        sub2.Legends.Add("Legend1");

        sub2.Legends["Legend1"].IsTextAutoFit = false;
        sub2.Legends["Legend1"].BackColor = System.Drawing.Color.Transparent;
        sub2.Legends["Legend1"].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
        sub2.Legends["Legend1"].Docking = Docking.Bottom;
        sub2.Legends["Legend1"].Alignment = System.Drawing.StringAlignment.Center;

        //Set Series
        sub2.Series.Clear();
        sub2.Series.Add("Series1");

        sub2.Series["Series1"].ChartType = SeriesChartType.Pie;
        sub2.Series["Series1"].Points.Clear();
        sub2.Series["Series1"]["PointWidth"] = "0.5";
        sub2.Series["Series1"].IsValueShownAsLabel = true;
        sub2.Series["Series1"].BorderWidth = 0;
        sub2.Series["Series1"].ShadowOffset = 1;

        sub2.ChartAreas["mainArea"].Area3DStyle.Enable3D = true;

        sub2.Titles.Clear();
        sub2.Titles.Add(new Title("ภาพแสดงจำนวนมวลชนเปรียบเทียบแยกประเภทกลุ่มมวลชน", Docking.Top, new System.Drawing.Font("Tahoma", 14, System.Drawing.FontStyle.Bold), System.Drawing.Color.Black));
        #endregion

        #region whereCompose
        string whereClause = "where 1=1 ";
        string subClause = "";
        string[] subTok;

        if (Request.QueryString["deptID"] != null)
        {
            subTok = Request.QueryString["deptID"].ToString().Split('|');
            foreach (string tmpStr in subTok)
            {
                if (subClause != "") subClause += " or ";
                subClause = subClause + " isnull(tb_detailgroup.fl_dept,'') like '" + tmpStr.Replace("0", "") + "%' ";
            }
            if (subClause != "") whereClause += " and (" + subClause + ") ";
        }

        subClause = "";

        if (Request.QueryString["proID"] != null)
        {
            subTok = Request.QueryString["proID"].ToString().Split('|');
            foreach (string tmpStr in subTok)
            {
                if (subClause != "") subClause += " or ";
                subClause = subClause + " tb_detailgroup.fl_province = '" + tmpStr + "' ";
            }
            if (subClause != "") whereClause += " and (" + subClause + ") ";
        }
        subClause = "";

        if (Request.QueryString["massID"] != null)
        {
            subTok = Request.QueryString["massID"].ToString().Split('|');
            foreach (string tmpStr in subTok)
            {
                if (subClause != "") subClause += " or ";
                subClause = subClause + " tb_detailgroup.fl_group_id = '" + tmpStr + "' ";
            }
            if (subClause != "") whereClause += " and (" + subClause + ") ";
        }

        //Filter Right
        if (Session["uGroup"] != null)
        {
            //if (Session["uGroup"].ToString() == "U") whereClause += " and isnull(tb_detailgroup.fl_dept,'') like '" + Session["uDept"].ToString().Replace("0", "") + "%' ";
            if (Session["uGroup"].ToString() == "U")
            {
                if (Session["uDept"].ToString().Substring(0) != "0") whereClause += " and isnull(tb_detailgroup.fl_dept,'') like '" + Session["uDept"].ToString().Replace("00", "") + "%' ";
            }
        }

        //Report WhereClause
        whereClause += " and tb_maingroup.fl_group_type <='3' ";

        Session["whereClause"] = whereClause;

        string orderClause = " order by  ";
        orderClause += " main.fl_group_type ";
        #endregion

        #region SQLComposed
        //SQL Composed
        string sql = "SELECT distinct ";

        sql += " main.fl_group_type, ";
        sql += " main.fl_province_name, ";
        sql += " main.fl_province, ";

        sql += " isnull(idCount,0), ";
        sql += " isnull(reportCount,0), ";
        sql += " isnull(headCount,0), ";
        sql += " isnull(headCount2,0) ";

        sql += " from ";

        sql += " (SELECT fl_group_type, ";
        sql += "    '99' as fl_province, ";
        sql += "    'ฮฮฮฮฮฮ' as fl_province_name, ";
        sql += "    isnull(count(fl_id),0) as idCount, ";
        sql += "    isnull(sum(cast(isnull(fl_reportMember,'0') as bigint)),0) as reportCount ";
        sql += " from tb_detailgroup ";
        sql += " inner join tb_maingroup  on tb_detailgroup.fl_group_id = tb_maingroup.fl_group_id ";
        sql += " inner join (select distinct fl_province_code,fl_province_name from tb_moicode) moiCode  on tb_detailgroup.fl_province = moiCode.fl_province_code ";
        sql += whereClause;
        sql += " group by fl_group_type ";
        sql += " ) main ";

        sql += " left outer join ";

        sql += " (SELECT fl_group_type, ";
        sql += " '99' as fl_province, ";
        sql += " isnull(count(tb_membergroup.fl_citizen_id),0) as headCount ";
        sql += " from tb_detailgroup ";
        sql += " inner join tb_maingroup  on tb_detailgroup.fl_group_id = tb_maingroup.fl_group_id ";
        sql += " inner join tb_membergroup on tb_detailgroup.fl_id = tb_membergroup.fl_detailgroup_id ";
        sql += " inner join tb_citizen on tb_membergroup.fl_citizen_id = tb_citizen.fl_citizen_id ";
        sql += whereClause;
        sql += " and (fl_stop='' or fl_stop is null) ";
        sql += " and tb_citizen.fl_status='1' ";
        sql += " group by fl_group_type ";
        sql += " ) c1 ";

        sql += " on main.fl_group_type=c1.fl_group_type ";
        sql += " and main.fl_province=c1.fl_province ";

        sql += " left outer join ";

        sql += " (SELECT fl_group_type, ";
        sql += " '99' as fl_province, ";
        sql += " isnull(count(tb_membergroup.fl_citizen_id),0) as headCount2 ";
        sql += " from tb_detailgroup ";
        sql += " inner join tb_maingroup  on tb_detailgroup.fl_group_id = tb_maingroup.fl_group_id ";
        sql += " inner join tb_membergroup on tb_detailgroup.fl_id = tb_membergroup.fl_detailgroup_id ";
        sql += " inner join tb_citizen on tb_membergroup.fl_citizen_id = tb_citizen.fl_citizen_id ";
        sql += whereClause;
        sql += " and (fl_stop='' or fl_stop is null) ";
        sql += " and tb_citizen.fl_status='1' ";
        sql += " and tb_citizen.fl_targetFlag='1' ";
        sql += " group by fl_group_type ";
        sql += " ) c2 ";
        sql += " on main.fl_group_type=c2.fl_group_type ";
        sql += " and main.fl_province=c2.fl_province ";

        sql += orderClause;
        #endregion

        OleDbConnection Conn = new OleDbConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString);
        OleDbCommand command = new OleDbCommand();

        Conn.Open();

        command.CommandText = sql;
        command.CommandTimeout = 180;
        command.Connection = Conn;
        OleDbDataReader rs = command.ExecuteReader();
        int xx = 0;
        while (rs.Read())
        {
                string namePoint = "";
                if (rs.GetString(0) == "1") namePoint = "มวลชนกอ.รมน.";
                if (rs.GetString(0) == "2") namePoint = "มวลชนภาครัฐ";
                if (rs.GetString(0) == "3") namePoint = "มวลชนภาคเอกชน";
                if (rs.GetString(0) == "4") namePoint = "มวลชนสานประโยชน์";
                if (rs.GetString(0) == "5") namePoint = "มวลชนอุปสรรค";

                sub2.Series["Series1"].Points.AddXY(namePoint, rs.GetValue(4));

                if (rs.GetString(0) == "1") sub2.Series["Series1"].Points[xx].Color = System.Drawing.Color.FromArgb(255, 0, 0);
                if (rs.GetString(0) == "2") sub2.Series["Series1"].Points[xx].Color = System.Drawing.Color.FromArgb(255, 127, 0);
                if (rs.GetString(0) == "3") sub2.Series["Series1"].Points[xx].Color = System.Drawing.Color.FromArgb(255, 255, 0);
                if (rs.GetString(0) == "4") sub2.Series["Series1"].Points[xx].Color = System.Drawing.Color.FromArgb(127, 255, 0);
                if (rs.GetString(0) == "5") sub2.Series["Series1"].Points[xx].Color = System.Drawing.Color.FromArgb(0, 255, 127);

                xx++;
        }
        rs.Close();
        Conn.Close();
    }

    protected void makeChart2(int subType)
    {
        #region setSub2
        //Set Legends
        sub2.Legends.Clear();
        sub2.Legends.Add("Legend1");

        sub2.Legends["Legend1"].IsTextAutoFit = false;
        sub2.Legends["Legend1"].BackColor = System.Drawing.Color.Transparent;
        sub2.Legends["Legend1"].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
        sub2.Legends["Legend1"].Docking = Docking.Bottom;
        sub2.Legends["Legend1"].Alignment = System.Drawing.StringAlignment.Center;

        //Set Series
        sub2.Series.Clear();
        sub2.Series.Add("Series1");

        sub2.Series["Series1"].ChartType = SeriesChartType.Pie;
        sub2.Series["Series1"].Points.Clear();
        sub2.Series["Series1"]["PointWidth"] = "0.5";
        sub2.Series["Series1"].IsValueShownAsLabel = true;
        sub2.Series["Series1"].BorderWidth = 0;
        sub2.Series["Series1"].ShadowOffset = 1;

        sub2.ChartAreas["mainArea"].Area3DStyle.Enable3D = true;

        sub2.Titles.Clear();
        sub2.Titles.Add(new Title("ภาพแสดงจำนวนมวลชนเปรียบเทียบแยกประเภทกลุ่มมวลชน", Docking.Top, new System.Drawing.Font("Tahoma", 14, System.Drawing.FontStyle.Bold), System.Drawing.Color.Black));
        #endregion

        #region whereCompose
        string whereClause = "where 1=1 ";
        string subClause = "";
        string[] subTok;

        if (Request.QueryString["deptID"] != null)
        {
            subTok = Request.QueryString["deptID"].ToString().Split('|');
            foreach (string tmpStr in subTok)
            {
                if (subClause != "") subClause += " or ";
                subClause = subClause + " isnull(tb_detailgroup.fl_dept,'') like '" + tmpStr.Replace("0", "") + "%' ";
            }
            if (subClause != "") whereClause += " and (" + subClause + ") ";
        }

        subClause = "";

        if (Request.QueryString["proID"] != null)
        {
            subTok = Request.QueryString["proID"].ToString().Split('|');
            foreach (string tmpStr in subTok)
            {
                if (subClause != "") subClause += " or ";
                subClause = subClause + " tb_detailgroup.fl_province = '" + tmpStr + "' ";
            }
            if (subClause != "") whereClause += " and (" + subClause + ") ";
        }
        subClause = "";

        if (Request.QueryString["massID"] != null)
        {
            subTok = Request.QueryString["massID"].ToString().Split('|');
            foreach (string tmpStr in subTok)
            {
                if (subClause != "") subClause += " or ";
                subClause = subClause + " tb_detailgroup.fl_group_id = '" + tmpStr + "' ";
            }
            if (subClause != "") whereClause += " and (" + subClause + ") ";
        }

        //Filter Right
        if (Session["uGroup"] != null)
        {
            //if (Session["uGroup"].ToString() == "U") whereClause += " and isnull(tb_detailgroup.fl_dept,'') like '" + Session["uDept"].ToString().Replace("0", "") + "%' ";
            if (Session["uGroup"].ToString() == "U")
            {
                if (Session["uDept"].ToString().Substring(0) != "0") whereClause += " and isnull(tb_detailgroup.fl_dept,'') like '" + Session["uDept"].ToString().Replace("00", "") + "%' ";
            }
        }

        //Report WhereClause
        whereClause += " and tb_maingroup.fl_group_type <='3' ";

        Session["whereClause"] = whereClause;

        string orderClause = " order by  ";
        orderClause += " main.fl_group_type ";
        #endregion

        #region SQLComposed
        //SQL Composed
        string sql = "SELECT distinct ";

        sql += " main.fl_group_type, ";
        sql += " main.fl_dept_name , ";
        sql += " main.fl_dept , ";

        sql += " isnull(idCount,0), ";
        sql += " isnull(reportCount,0), ";
        sql += " isnull(headCount,0), ";
        sql += " isnull(headCount2,0) ";

        sql += " from ";

        sql += " (SELECT fl_group_type, ";
        sql += "    '99' as fl_dept, ";
        sql += "    'ฮฮฮฮฮฮ' as fl_dept_name, ";
        sql += "    isnull(count(fl_id),0) as idCount, ";
        sql += "    isnull(sum(cast(isnull(fl_reportMember,'0') as bigint)),0) as reportCount ";
        sql += " from tb_detailgroup ";
        sql += " inner join tb_maingroup  on tb_detailgroup.fl_group_id = tb_maingroup.fl_group_id ";
        sql += " inner join (select distinct fl_dept_id,fl_dept_name from tb_dept) moiCode  on tb_detailgroup.fl_dept = moiCode.fl_dept_id ";
        sql += whereClause;
        sql += " group by fl_group_type ";
        sql += " ) main ";

        sql += " left outer join ";

        sql += " (SELECT fl_group_type, ";
        sql += " '99' as fl_dept, ";
        sql += " isnull(count(tb_membergroup.fl_citizen_id),0) as headCount ";
        sql += " from tb_detailgroup ";
        sql += " inner join tb_maingroup  on tb_detailgroup.fl_group_id = tb_maingroup.fl_group_id ";
        sql += " inner join tb_membergroup on tb_detailgroup.fl_id = tb_membergroup.fl_detailgroup_id ";
        sql += " inner join tb_citizen on tb_membergroup.fl_citizen_id = tb_citizen.fl_citizen_id ";
        sql += whereClause;
        sql += " and (fl_stop='' or fl_stop is null) ";
        sql += " and tb_citizen.fl_status='1' ";
        sql += " group by fl_group_type ";
        sql += " ) c1 ";

        sql += " on main.fl_group_type=c1.fl_group_type ";
        sql += " and main.fl_dept=c1.fl_dept ";

        sql += " left outer join ";

        sql += " (SELECT fl_group_type, ";
        sql += " '99' as fl_dept, ";
        sql += " isnull(count(tb_membergroup.fl_citizen_id),0) as headCount2 ";
        sql += " from tb_detailgroup ";
        sql += " inner join tb_maingroup  on tb_detailgroup.fl_group_id = tb_maingroup.fl_group_id ";
        sql += " inner join tb_membergroup on tb_detailgroup.fl_id = tb_membergroup.fl_detailgroup_id ";
        sql += " inner join tb_citizen on tb_membergroup.fl_citizen_id = tb_citizen.fl_citizen_id ";
        sql += whereClause;
        sql += " and (fl_stop='' or fl_stop is null) ";
        sql += " and tb_citizen.fl_status='1' ";
        sql += " and tb_citizen.fl_targetFlag='1' ";
        sql += " group by fl_group_type ";
        sql += " ) c2 ";
        sql += " on main.fl_group_type=c2.fl_group_type ";
        sql += " and main.fl_dept=c2.fl_dept ";

        sql += orderClause;
        #endregion

        OleDbConnection Conn = new OleDbConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString);
        OleDbCommand command = new OleDbCommand();

        Conn.Open();

        command.CommandText = sql;
        command.CommandTimeout = 180;
        command.Connection = Conn;
        OleDbDataReader rs = command.ExecuteReader();

        int xx = 0;
        while (rs.Read())
        {
                string namePoint = "";
                if (rs.GetString(0) == "1") namePoint = "มวลชนกอ.รมน.";
                if (rs.GetString(0) == "2") namePoint = "มวลชนภาครัฐ";
                if (rs.GetString(0) == "3") namePoint = "มวลชนภาคเอกชน";
                if (rs.GetString(0) == "4") namePoint = "มวลชนสานประโยชน์";
                if (rs.GetString(0) == "5") namePoint = "มวลชนอุปสรรค";

                sub2.Series["Series1"].Points.AddXY(namePoint, rs.GetValue(4));

                if (rs.GetString(0) == "1") sub2.Series["Series1"].Points[xx].Color = System.Drawing.Color.FromArgb(255, 0, 0);
                if (rs.GetString(0) == "2") sub2.Series["Series1"].Points[xx].Color = System.Drawing.Color.FromArgb(255, 127, 0);
                if (rs.GetString(0) == "3") sub2.Series["Series1"].Points[xx].Color = System.Drawing.Color.FromArgb(255, 255, 0);
                if (rs.GetString(0) == "4") sub2.Series["Series1"].Points[xx].Color = System.Drawing.Color.FromArgb(127, 255, 0);
                if (rs.GetString(0) == "5") sub2.Series["Series1"].Points[xx].Color = System.Drawing.Color.FromArgb(0, 255, 127);

                xx++;
        }
        rs.Close();
        Conn.Close();
    }

    protected void makeChart3()
    {
        #region setSub2
        //Set Legends
        sub2.Legends.Clear();
        sub2.Legends.Add("Legend1");

        sub2.Legends["Legend1"].IsTextAutoFit = false;
        sub2.Legends["Legend1"].BackColor = System.Drawing.Color.Transparent;
        sub2.Legends["Legend1"].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
        sub2.Legends["Legend1"].Docking = Docking.Bottom;
        sub2.Legends["Legend1"].Alignment = System.Drawing.StringAlignment.Center;

        //Set Series
        sub2.Series.Clear();
        sub2.Series.Add("มวลชน กอ.รมน.");
        sub2.Series.Add("มวลชนภาครัฐ");
        sub2.Series.Add("มวลชนภาคประชาชน");
        for (int xx = 0; xx < 3; xx++)
        {
            sub2.Series[xx].ChartType = SeriesChartType.Line;
            sub2.Series[xx].Points.Clear();
            sub2.Series[xx]["PointWidth"] = "5";
            sub2.Series[xx]["ShowMarkerLines"] = "True";
            sub2.Series[xx].IsValueShownAsLabel = true;

            if (xx == 0) sub2.Series[xx].Color = System.Drawing.Color.FromArgb(255, 0, 0);
            if (xx == 1) sub2.Series[xx].Color = System.Drawing.Color.FromArgb(255, 127, 0);
            if (xx == 2) sub2.Series[xx].Color = System.Drawing.Color.FromArgb(255, 255, 0);
            if (xx == 3) sub2.Series[xx].Color = System.Drawing.Color.FromArgb(127, 255, 0);

            sub2.Series[xx].XAxisType = AxisType.Primary;
            sub2.Series[xx].YAxisType = AxisType.Primary;
        }

        sub2.ChartAreas["mainArea"].Area3DStyle.Enable3D = false;
        sub2.ChartAreas["mainArea"].AxisY.Title = "อัตราเติบโต (%)";

        sub2.Titles.Clear();
        sub2.Titles.Add(new Title("ภาพแสดงความเติบโตมวลชนเปรียบเทียบแยกประเภทกลุ่มมวลชน", Docking.Top, new System.Drawing.Font("Tahoma", 14, System.Drawing.FontStyle.Bold), System.Drawing.Color.Black));
        #endregion

        #region whereCompose
        string whereClause = "where 1=1 ";
        string subClause = "";
        string[] subTok;

        if (Request.QueryString["deptID"] != null)
        {
            subTok = Request.QueryString["deptID"].ToString().Split('|');
            foreach (string tmpStr in subTok)
            {
                if (subClause != "") subClause += " or ";
                subClause = subClause + " isnull(tb_detailgroup.fl_dept,'') like '" + tmpStr.Replace("0", "") + "%' ";
            }
            if (subClause != "") whereClause += " and (" + subClause + ") ";
        }

        subClause = "";

        if (Request.QueryString["proID"] != null)
        {
            subTok = Request.QueryString["proID"].ToString().Split('|');
            foreach (string tmpStr in subTok)
            {
                if (subClause != "") subClause += " or ";
                subClause = subClause + " tb_detailgroup.fl_province = '" + tmpStr + "' ";
            }
            if (subClause != "") whereClause += " and (" + subClause + ") ";
        }
        subClause = "";

        if (Request.QueryString["massID"] != null)
        {
            subTok = Request.QueryString["massID"].ToString().Split('|');
            foreach (string tmpStr in subTok)
            {
                if (subClause != "") subClause += " or ";
                subClause = subClause + " tb_detailgroup.fl_group_id = '" + tmpStr + "' ";
            }
            if (subClause != "") whereClause += " and (" + subClause + ") ";
        }

        //Filter Right
        if (Session["uGroup"] != null)
        {
            //if (Session["uGroup"].ToString() == "U") whereClause += " and isnull(tb_detailgroup.fl_dept,'') like '" + Session["uDept"].ToString().Replace("0", "") + "%' ";
            if (Session["uGroup"].ToString() == "U")
            {
                if (Session["uDept"].ToString().Substring(0) != "0") whereClause += " and isnull(tb_detailgroup.fl_dept,'') like '" + Session["uDept"].ToString().Replace("00", "") + "%' ";
            }
        }

        //Report WhereClause
        whereClause += " and tb_maingroup.fl_group_type <='3' ";

        Session["whereClause"] = whereClause;

        string orderClause = " order by main.fl_yearID, ";
        orderClause += " main.fl_group_type ";
        #endregion

        #region SQLComposed
        //SQL Composed
        string sql = "SELECT distinct ";

        sql += " main.fl_group_type, ";
        sql += " main.fl_YearID, ";

        sql += " isnull(idCount,0), ";
        sql += " isnull(reportCount,0), ";
        sql += " isnull(headCount,0) ";

        sql += " from ";

        sql += " (";
        sql += "SELECT distinct ";
        sql += "    fl_group_type, ";
        sql += "    '" + Convert.ToString(DateTime.Now.Year + 543 - 2) + "' as  fl_YearID, ";
        sql += " isnull(count(fl_id),0) as idCount, ";
        sql += " isnull(sum(cast(isnull(fl_reportMember,'0') as bigint)),0) as reportCount ";
        sql += " from (SELECT distinct fl_id,fl_reportMember,fl_group_type from tb_detailgroup ";
        sql += " inner join tb_maingroup  on tb_detailgroup.fl_group_id = tb_maingroup.fl_group_id ";
        sql += " inner join tb_membergroup on tb_detailgroup.fl_id = tb_membergroup.fl_detailgroup_id ";
        sql += whereClause;
        //At end of year
        sql += " and fl_start <= '" + Convert.ToString(DateTime.Now.Year - 1) + "' ";
        sql += " and (isnull(fl_stop,'')='' or isnull(fl_stop,'') > '" + Convert.ToString(DateTime.Now.Year - 1) + "') ";
        sql += " ) z";
        sql += " group by fl_group_type ";

        sql += " UNION ";

        sql += " SELECT distinct ";
        sql += "    fl_group_type, ";
        sql += "    '" + Convert.ToString(DateTime.Now.Year + 543 - 1) + "' as  fl_YearID, ";
        sql += " isnull(count(fl_id),0) as idCount, ";
        sql += " isnull(sum(cast(isnull(fl_reportMember,'0') as bigint)),0) as reportCount ";
        sql += " from (SELECT distinct fl_id,fl_reportMember,fl_group_type from tb_detailgroup ";
        sql += " inner join tb_maingroup  on tb_detailgroup.fl_group_id = tb_maingroup.fl_group_id ";
        sql += " inner join tb_membergroup on tb_detailgroup.fl_id = tb_membergroup.fl_detailgroup_id ";
        sql += whereClause;
        //At end of year
        sql += " and fl_start <= '" + Convert.ToString(DateTime.Now.Year) + "' ";
        sql += " and (isnull(fl_stop,'')='' or isnull(fl_stop,'') > '" + Convert.ToString(DateTime.Now.Year) + "') ";
        sql += " ) z";
        sql += " group by fl_group_type ";

        sql += " UNION ";

        sql += " SELECT distinct ";
        sql += "    fl_group_type, ";
        sql += "    '" + Convert.ToString(DateTime.Now.Year + 543) + "' as  fl_YearID, ";
        sql += " isnull(count(fl_id),0) as idCount, ";
        sql += " isnull(sum(cast(isnull(fl_reportMember,'0') as bigint)),0) as reportCount ";
        sql += " from (SELECT distinct fl_id,fl_reportMember,fl_group_type from tb_detailgroup ";
        sql += " inner join tb_maingroup  on tb_detailgroup.fl_group_id = tb_maingroup.fl_group_id ";
        sql += " inner join tb_membergroup on tb_detailgroup.fl_id = tb_membergroup.fl_detailgroup_id ";
        sql += whereClause;
        //At end of year
        sql += " and (isnull(fl_stop,'')='') ";
        sql += " ) z";
        sql += " group by fl_group_type ";
        sql += " ) main ";

        sql += " inner join ";

        sql += " (";
        sql += "SELECT distinct ";
        sql += "    fl_group_type, ";
        sql += "    '" + Convert.ToString(DateTime.Now.Year + 543 - 2) + "' as  fl_YearID, ";
        sql += " isnull(count(tb_membergroup.fl_citizen_id),0) as headCount ";
        sql += " from tb_detailgroup ";
        sql += " inner join tb_maingroup  on tb_detailgroup.fl_group_id = tb_maingroup.fl_group_id ";
        sql += " inner join tb_membergroup on tb_detailgroup.fl_id = tb_membergroup.fl_detailgroup_id ";
        sql += whereClause;
        //At end of year
        sql += " and fl_start <= '" + Convert.ToString(DateTime.Now.Year - 1) + "' ";
        sql += " and (isnull(fl_stop,'')='' or isnull(fl_stop,'') > '" + Convert.ToString(DateTime.Now.Year - 1) + "') ";
        sql += " group by fl_group_type ";

        sql += " UNION ";

        sql += " SELECT distinct ";
        sql += "    fl_group_type, ";
        sql += "    '" + Convert.ToString(DateTime.Now.Year + 543 - 1) + "' as  fl_YearID, ";
        sql += " isnull(count(tb_membergroup.fl_citizen_id),0) as headCount ";
        sql += " from tb_detailgroup ";
        sql += " inner join tb_maingroup  on tb_detailgroup.fl_group_id = tb_maingroup.fl_group_id ";
        sql += " inner join tb_membergroup on tb_detailgroup.fl_id = tb_membergroup.fl_detailgroup_id ";
        sql += whereClause;
        //At end of year
        sql += " and fl_start <= '" + Convert.ToString(DateTime.Now.Year) + "' ";
        sql += " and (isnull(fl_stop,'')='' or isnull(fl_stop,'') > '" + Convert.ToString(DateTime.Now.Year) + "') ";
        sql += " group by fl_group_type ";

        sql += " UNION ";

        sql += " SELECT distinct ";
        sql += "    fl_group_type, ";
        sql += "    '" + Convert.ToString(DateTime.Now.Year + 543) + "' as  fl_YearID, ";
        sql += " isnull(count(tb_membergroup.fl_citizen_id),0) as headCount ";
        sql += " from tb_detailgroup ";
        sql += " inner join tb_maingroup  on tb_detailgroup.fl_group_id = tb_maingroup.fl_group_id ";
        sql += " inner join tb_membergroup on tb_detailgroup.fl_id = tb_membergroup.fl_detailgroup_id ";
        sql += whereClause;
        //At end of year
        sql += " and (isnull(fl_stop,'')='') ";
        sql += " group by fl_group_type ";
        sql += " ) s1 ";

        sql += " on main.fl_group_type=s1.fl_group_type ";
        sql += " and main.fl_yearID=s1.fl_yearID ";

        sql += orderClause;
        #endregion

        OleDbConnection Conn = new OleDbConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString);
        OleDbCommand command = new OleDbCommand();

        Conn.Open();

        command.CommandText = sql;
        command.CommandTimeout = 180;
        command.Connection = Conn;
        OleDbDataReader rs = command.ExecuteReader();

        long tmpLY1 = 0;
        long tmpLY2 = 0;
        long tmpLY3 = 0;
 
        long tmpL1Y1 = 0;
        long tmpL1Y2 = 0;
        long tmpL1Y3 = 0;
 
        double amtPlot1 = 0;
        double amtPlot2 = 0; 
                            
        while (rs.Read())
        {
            if (rs.GetString(0) == "1")
            {
                if (tmpLY1 != 0) amtPlot1 = (Convert.ToInt64(rs.GetValue(2)) - tmpLY1) *1.0 / tmpLY1 * 100;
                tmpLY1 = Convert.ToInt64(rs.GetValue(2));

                if (tmpL1Y1 != 0) amtPlot2 = (Convert.ToInt64(rs.GetValue(4)) - tmpL1Y1) *1.0 / tmpL1Y1 * 100;
                tmpL1Y1 = Convert.ToInt64(rs.GetValue(4));
                sub2.Series[0].Points.AddXY(rs.GetString(1), Convert.ToDouble(amtPlot2.ToString("#,##0.00")));
            }
            if (rs.GetString(0) == "2")
            {
                if (tmpLY2 != 0) amtPlot1 = (Convert.ToInt64(rs.GetValue(2)) - tmpLY2) *1.0 / tmpLY2 * 100;
                tmpLY2 = Convert.ToInt64(rs.GetValue(2));

                if (tmpL1Y2 != 0) amtPlot2 = (Convert.ToInt64(rs.GetValue(4)) - tmpL1Y2) * 1.0 / tmpL1Y2 * 100;
                tmpL1Y2 = Convert.ToInt64(rs.GetValue(4));
                sub2.Series[1].Points.AddXY(rs.GetString(1), Convert.ToDouble(amtPlot2.ToString("#,##0.00")));
            }
            if (rs.GetString(0) == "3")
            {
                if (tmpLY3 != 0) amtPlot1 = (Convert.ToInt64(rs.GetValue(2)) - tmpLY3) * 1.0 / tmpLY3 * 100;
                tmpLY3 = Convert.ToInt64(rs.GetValue(2));

                if (tmpL1Y3 != 0) amtPlot2 = (Convert.ToInt64(rs.GetValue(4)) - tmpL1Y3) * 1.0 / tmpL1Y3 * 100;
                tmpL1Y3 = Convert.ToInt64(rs.GetValue(4));
                sub2.Series[2].Points.AddXY(rs.GetString(1), Convert.ToDouble(amtPlot2.ToString("#,##0.00")));
            }
        }
        rs.Close();
        Conn.Close();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if ((Session["uName"] == null) || (Session["uName"].ToString() == ""))
            {
                if (Request.QueryString["skip"].ToString() != "1") return;
            }
        }
        catch (Exception ex)
        {
            return;
        }

        if (Request.QueryString["repID"].ToString() == "11") makeChart1(1);
        if (Request.QueryString["repID"].ToString() == "12") makeChart2(1);
        if (Request.QueryString["repID"].ToString() == "20") makeChart3();
        if (Request.QueryString["repID"].ToString() == "31") makeChart1(2);
        if (Request.QueryString["repID"].ToString() == "32") makeChart2(2);
        if (Request.QueryString["repID"].ToString() == "41") makeChart1(3);
        if (Request.QueryString["repID"].ToString() == "42") makeChart2(3);

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
}