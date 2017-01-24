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
using Microsoft.Reporting.WebForms;
using System.IO;

public partial class _report_REP: System.Web.UI.Page
{

    protected void deptDataSet()
    {
        cmbDept.Items.Clear();
        OleDbConnection Conn = new OleDbConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString);
        OleDbCommand command = new OleDbCommand();

        string sql = "SELECT distinct fl_dept_id,fl_dept_name from tb_dept where fl_dept_status='1' ";
        //if (Session["uGroup"].ToString() == "U") sql = sql + " and fl_dept_id like '" + Session["uDept"].ToString().Replace("0", "") + "%' ";
        if (Session["uGroup"].ToString() == "U")
        {
            if (Session["uDept"].ToString().Substring(0) != "0") sql = sql + " and fl_dept_id like '" + Session["uDept"].ToString().Replace("00", "") + "%' ";
        }
        sql += " order by fl_dept_id ";

        Conn.Open();

        command.CommandText = sql;
        command.Connection = Conn;
        OleDbDataReader rs = command.ExecuteReader();
        while (rs.Read())
        {
            cmbDept.Items.Add(new ListItem(rs.GetString(1), rs.GetString(0)));
        }
        rs.Close();
        Conn.Close();

        if (Session["uGroup"].ToString() == "C") cmbDept.Enabled = false;
    }

    protected void provinceDataSet()
    {
        cmbProvince.Items.Clear();

        OleDbConnection Conn = new OleDbConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString);
        OleDbCommand command = new OleDbCommand();

        string sql = "SELECT distinct fl_province_code,fl_province_name from tb_moicode where fl_status='1' ";
        sql += " order by fl_province_name ";

        Conn.Open();

        command.CommandText = sql;
        command.Connection = Conn;
        OleDbDataReader rs = command.ExecuteReader();
        while (rs.Read())
        {
            cmbProvince.Items.Add(new ListItem(rs.GetString(1), rs.GetString(0)));
        }
        rs.Close();
        Conn.Close();

        if (Session["uGroup"].ToString() == "C") cmbProvince.Enabled = false;
    }

    protected void massDataSet()
    {
        cmbMassGroup.Items.Clear();

        OleDbConnection Conn = new OleDbConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString);
        OleDbCommand command = new OleDbCommand();

        string sql = "SELECT distinct fl_group_id,fl_group_name,fl_group_type from tb_maingroup where fl_group_status='1' ";
        sql += " order by fl_group_type,fl_group_name ";

        Conn.Open();

        command.CommandText = sql;
        command.Connection = Conn;
        OleDbDataReader rs = command.ExecuteReader();
        while (rs.Read())
        {
            cmbMassGroup.Items.Add(new ListItem(rs.GetString(1), rs.GetString(0)));
        }
        rs.Close();
        Conn.Close();

        if (Session["uGroup"].ToString() == "C") cmbMassGroup.Enabled = false;
    }

    protected void reportDataSet()
    {
        cmbReport.Items.Clear();
        cmbReport.Items.Add(new ListItem("รายงานมวลชนพื้นฐานและมวลชนแผนปฎิบัติการจำแนกรายจังหวัดมหาดไทย", "11"));
        cmbReport.Items.Add(new ListItem("รายงานมวลชนพื้นฐานและมวลชนแผนปฎิบัติการจำแนกรายจังหวัดทหาร", "12"));
        cmbReport.Items.Add(new ListItem("รายงานแสดงการเติบโตของกลุ่มมวลชนเทียบย้อนหลังรวม 3 ปี", "20"));
    }

    protected void makeChart1(int subType)
    {
        #region header
        tab1.Rows.Clear();
        tab1.BorderColor = ConfigurationManager.AppSettings["gridBorderColor"];
        tab1.Border = 1;
        tab1.CellPadding = 0;
        tab1.CellSpacing = 0;

        tab1.Rows.Add(new HtmlTableRow());
        tab1.Rows[0].Attributes.Add("class", "head");
        tab1.Rows[0].Cells.Add(new HtmlTableCell());
        tab1.Rows[0].Cells[0].InnerHtml = "จังหวัด";
        tab1.Rows[0].Cells[0].RowSpan = 2;
        tab1.Rows[0].Cells[0].Width = "200px";

        if (subType == 1)
        {
            tab1.Rows[0].Cells.Add(new HtmlTableCell());
            tab1.Rows[0].Cells[1].ColSpan = 4;
            tab1.Rows[0].Cells[1].InnerHtml = "<center>มวลชนกอรมน.</center>";
            tab1.Rows[0].Cells.Add(new HtmlTableCell());
            tab1.Rows[0].Cells[2].ColSpan = 4;
            tab1.Rows[0].Cells[2].InnerHtml = "<center>มวลชนภาครัฐ</center>";
            tab1.Rows[0].Cells.Add(new HtmlTableCell());
            tab1.Rows[0].Cells[3].ColSpan = 4;
            tab1.Rows[0].Cells[3].InnerHtml = "<center>มวลชนภาคประชาชน</center>";
            tab1.Rows[0].Cells.Add(new HtmlTableCell());
            tab1.Rows[0].Cells[4].ColSpan = 4;
            tab1.Rows[0].Cells[4].InnerHtml = "<center>รวม</center>";
        }
        else
        {
            tab1.Rows[0].Cells.Add(new HtmlTableCell());
            tab1.Rows[0].Cells[1].ColSpan = 4;
            if (subType == 2) tab1.Rows[0].Cells[1].InnerHtml = "<center>มวลชนสานประโยชน์</center>";
            if (subType == 3) tab1.Rows[0].Cells[1].InnerHtml = "<center>มวลชนอุปสรรค</center>";
            tab1.Rows[0].Cells.Add(new HtmlTableCell());
            tab1.Rows[0].Cells[2].ColSpan = 4;
            tab1.Rows[0].Cells[2].InnerHtml = "<center>รวม</center>";
        }

        tab1.Rows.Add(new HtmlTableRow());
        tab1.Rows[1].Attributes.Add("class", "head");
        int maxCols = 3;
        if (subType != 1) maxCols = 1;
        for (int mcols = 0; mcols <= maxCols; mcols++)
        {
            for (int scols = 0; scols <= 3; scols++)
            {
                tab1.Rows[1].Cells.Add(new HtmlTableCell());
                string celltxt = "";
                if (scols == 0) celltxt = "กลุ่ม";
                if (scols == 1) celltxt = "รายงาน";
                if (scols == 2) celltxt = "รายชื่อ";
                if (scols == 3) celltxt = "ปฎิบัติการ";
                tab1.Rows[1].Cells[mcols * 4 + scols].InnerHtml = celltxt;
                tab1.Rows[1].Cells[mcols * 4 + scols].Width = "100px";
            }
        }
        #endregion

        #region whereCompose
        string whereClause = "where 1=1 ";
        string subClause="";
        string linkSub = "";
        string titleParam = "";
        string linkParam = "?repID=" + cmbReport.SelectedValue;
        lblRepTitle.Text = "";
        for(int xx=0; xx<cmbDept.Items.Count;xx++)
        {
            if (cmbDept.Items[xx].Selected)
            {
                if(subClause!="") subClause += " or ";
                subClause = subClause + " isnull(tb_detailgroup.fl_dept,'') like '" + cmbDept.Items[xx].Value.Replace("0","") + "%' ";

                if (linkSub != "") linkSub += "|";
                linkSub += cmbDept.Items[xx].Value;

                if (titleParam != "") titleParam += ",";
                titleParam += cmbDept.Items[xx].Text;
            }
        }
        if (subClause != "") whereClause += " and (" + subClause + ") ";
        if (linkSub != "") linkParam += "&deptID=" + linkSub;
        if (titleParam != "") lblRepTitle.Text += " หน่วยงาน: " + titleParam + " ";

        subClause = "";
        linkSub = "";
        titleParam="";

        for (int xx = 0; xx < cmbProvince.Items.Count; xx++)
        {
            if (cmbProvince.Items[xx].Selected)
            {
                if (subClause != "") subClause += " or ";
                subClause = subClause + " tb_detailgroup.fl_province = '" + cmbProvince.Items[xx].Value + "' ";

                if (linkSub != "") linkSub += "|";
                linkSub += cmbProvince.Items[xx].Value;

                if (titleParam != "") titleParam += ",";
                titleParam += cmbProvince.Items[xx].Text;
            }
        }
        if (subClause != "") whereClause += " and (" + subClause + ") ";
        if (linkSub != "") linkParam += "&proID=" + linkSub;
        if (titleParam != "") lblRepTitle.Text += "จังหวัด: " + titleParam + " ";

        subClause = "";
        linkSub = "";
        titleParam="";

        for (int xx = 0; xx < cmbMassGroup.Items.Count; xx++)
        {
            if (cmbMassGroup.Items[xx].Selected)
            {
                if (subClause != "") subClause += " or ";
                subClause = subClause + " tb_detailgroup.fl_group_id = '" + cmbMassGroup.Items[xx].Value + "' ";

                if (linkSub != "") linkSub += "|";
                linkSub += cmbMassGroup.Items[xx].Value;

                if (titleParam != "") titleParam += ",";
                titleParam += cmbMassGroup.Items[xx].Text;
            }
        }
        if (subClause != "") whereClause += " and (" + subClause + ") ";
        if (linkSub != "") linkParam += "&massID=" + linkSub;
        if (titleParam != "") lblRepTitle.Text += "มวลชน: " + titleParam + " ";
        
        //Filter Right
        //if (Session["uGroup"].ToString() == "U") whereClause += " and isnull(tb_detailgroup.fl_dept,'') like '" + Session["uDept"].ToString().Replace("0", "") + "%' ";
        if (Session["uGroup"].ToString() == "U")
        {
            if (Session["uDept"].ToString().Substring(0) != "0") whereClause += " and isnull(tb_detailgroup.fl_dept,'') like '" + Session["uDept"].ToString().Replace("00", "") + "%' ";
        }
        //Report WhereClause
        if (subType == 1) whereClause += " and tb_maingroup.fl_group_type <='3' ";
        if (subType == 2) whereClause += " and tb_maingroup.fl_group_type ='4' ";
        if (subType == 3) whereClause += " and tb_maingroup.fl_group_type ='5' ";

        Session["whereClause"] = whereClause;

        string orderClause = " order by main.fl_province_name, ";
        orderClause += " main.fl_group_type ";

        lblRepTitle.Text = "รายงานข้อมูล " + lblRepTitle.Text;        
        #endregion

        mainChart.ImageUrl = "G1.aspx" + linkParam;
        sub1.ImageUrl = "G2.aspx" + linkParam;
        sub2.ImageUrl = "G3.aspx" + linkParam;

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

        sql += " (SELECT distinct ";
        sql += "    fl_group_type, ";
        sql += "    fl_province, ";
        sql += "    fl_province_name, ";
        sql += "    isnull(count(fl_id),0) as idCount, ";
        sql += "    isnull(sum(cast(isnull(fl_reportMember,'0') as bigint)),0) as reportCount ";
        sql += " from tb_detailgroup ";
        sql += " inner join tb_maingroup  on tb_detailgroup.fl_group_id = tb_maingroup.fl_group_id ";
        sql += " inner join (select distinct fl_province_code,fl_province_name from tb_moicode) moiCode  on tb_detailgroup.fl_province = moiCode.fl_province_code ";
        sql += whereClause;
        sql += " group by fl_group_type,fl_province_name,fl_province ";

        sql += " UNION ";

        sql += " SELECT '9' as fl_group_type, ";
        sql += "    fl_province, ";
        sql += "    fl_province_name, ";
        sql += "    isnull(count(fl_id),0) as idCount, ";
        sql += "    isnull(sum(cast(isnull(fl_reportMember,'0') as bigint)),0) as reportCount ";
        sql += " from tb_detailgroup ";
        sql += " inner join tb_maingroup  on tb_detailgroup.fl_group_id = tb_maingroup.fl_group_id ";
        sql += " inner join (select distinct fl_province_code,fl_province_name from tb_moicode) moiCode  on tb_detailgroup.fl_province = moiCode.fl_province_code ";
        sql += whereClause;
        sql += " group by fl_province_name,fl_province ";

        sql += " UNION ";

        sql += " SELECT fl_group_type, ";
        sql += "    '99' as fl_province, ";
        sql += "    'ฮฮฮฮฮฮ' as fl_province_name, ";
        sql += "    isnull(count(fl_id),0) as idCount, ";
        sql += "    isnull(sum(cast(isnull(fl_reportMember,'0') as bigint)),0) as reportCount ";
        sql += " from tb_detailgroup ";
        sql += " inner join tb_maingroup  on tb_detailgroup.fl_group_id = tb_maingroup.fl_group_id ";
        sql += " inner join (select distinct fl_province_code,fl_province_name from tb_moicode) moiCode  on tb_detailgroup.fl_province = moiCode.fl_province_code ";
        sql += whereClause;
        sql += " group by fl_group_type ";

        sql += " UNION ";

        sql += " SELECT '9' as fl_group_type, ";
        sql += "    '99' as fl_province, ";
        sql += "    'ฮฮฮฮฮฮ' as fl_province_name, ";
        sql += "    isnull(count(fl_id),0) as idCount, ";
        sql += "    isnull(sum(cast(isnull(fl_reportMember,'0') as bigint)),0) as reportCount ";
        sql += " from tb_detailgroup ";
        sql += " inner join tb_maingroup  on tb_detailgroup.fl_group_id = tb_maingroup.fl_group_id ";
        sql += " inner join (select distinct fl_province_code,fl_province_name from tb_moicode) moiCode  on tb_detailgroup.fl_province = moiCode.fl_province_code ";
        sql += whereClause;
        sql += " ) main ";

        sql += " left outer join ";

        sql += " (select distinct ";
        sql += " fl_group_type, ";
        sql += " fl_province, ";
        sql += " isnull(count(tb_membergroup.fl_citizen_id),0) as headCount ";
        sql += " from tb_detailgroup ";
        sql += " inner join tb_maingroup on tb_detailgroup.fl_group_id = tb_maingroup.fl_group_id ";
        sql += " inner join tb_membergroup on tb_detailgroup.fl_id = tb_membergroup.fl_detailgroup_id ";
        sql += " inner join tb_citizen on tb_membergroup.fl_citizen_id = tb_citizen.fl_citizen_id ";
        sql += whereClause;
        sql += " and (fl_stop='' or fl_stop is null) ";
        sql += " and tb_citizen.fl_status='1' ";
        sql += " group by fl_group_type,fl_province ";

        sql += " UNION ";

        sql += " SELECT '9' as fl_group_type, ";
        sql += " fl_province, ";
        sql += " isnull(count(tb_membergroup.fl_citizen_id),0) as headCount ";
        sql += " from tb_detailgroup ";
        sql += " inner join tb_maingroup  on tb_detailgroup.fl_group_id = tb_maingroup.fl_group_id ";
        sql += " inner join tb_membergroup on tb_detailgroup.fl_id = tb_membergroup.fl_detailgroup_id ";
        sql += " inner join tb_citizen on tb_membergroup.fl_citizen_id = tb_citizen.fl_citizen_id ";
        sql += whereClause;
        sql += " and (fl_stop='' or fl_stop is null) ";
        sql += " and tb_citizen.fl_status='1' ";
        sql += " group by fl_province ";

        sql += " UNION ";

        sql += " SELECT fl_group_type, ";
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

        sql += " UNION ";

        sql += " SELECT '9' as fl_group_type, ";
        sql += " '99' as fl_province, ";
        sql += " isnull(count(tb_membergroup.fl_citizen_id),0) as headCount ";
        sql += " from tb_detailgroup ";
        sql += " inner join tb_maingroup  on tb_detailgroup.fl_group_id = tb_maingroup.fl_group_id ";
        sql += " inner join tb_membergroup on tb_detailgroup.fl_id = tb_membergroup.fl_detailgroup_id ";
        sql += " inner join tb_citizen on tb_membergroup.fl_citizen_id = tb_citizen.fl_citizen_id ";
        sql += whereClause;
        sql += " and (fl_stop='' or fl_stop is null) ";
        sql += " and tb_citizen.fl_status='1' ";
        sql += " ) c1 ";

        sql += " on main.fl_group_type=c1.fl_group_type ";
        sql += " and main.fl_province=c1.fl_province ";

        sql += " left outer join ";

        sql += " (select distinct ";
        sql += " fl_group_type, ";
        sql += " fl_province, ";
        sql += " isnull(count(tb_membergroup.fl_citizen_id),0) as headCount2 ";
        sql += " from tb_detailgroup ";
        sql += " inner join tb_maingroup on tb_detailgroup.fl_group_id = tb_maingroup.fl_group_id ";
        sql += " inner join tb_membergroup on tb_detailgroup.fl_id = tb_membergroup.fl_detailgroup_id ";
        sql += " inner join tb_citizen on tb_membergroup.fl_citizen_id = tb_citizen.fl_citizen_id ";
        sql += whereClause;
        sql += " and (fl_stop='' or fl_stop is null) ";
        sql += " and tb_citizen.fl_status='1' ";
        sql += " and tb_citizen.fl_targetFlag='1' ";
        sql += " group by fl_group_type,fl_province ";

        sql += " UNION ";

        sql += " SELECT '9' as fl_group_type, ";
        sql += " fl_province, ";
        sql += " isnull(count(tb_membergroup.fl_citizen_id),0) as headCount2 ";
        sql += " from tb_detailgroup ";
        sql += " inner join tb_maingroup  on tb_detailgroup.fl_group_id = tb_maingroup.fl_group_id ";
        sql += " inner join tb_membergroup on tb_detailgroup.fl_id = tb_membergroup.fl_detailgroup_id ";
        sql += " inner join tb_citizen on tb_membergroup.fl_citizen_id = tb_citizen.fl_citizen_id ";
        sql += whereClause;
        sql += " and (fl_stop='' or fl_stop is null) ";
        sql += " and tb_citizen.fl_status='1' ";
        sql += " and tb_citizen.fl_targetFlag='1' ";
        sql += " group by fl_province ";

        sql += " UNION ";

        sql += " SELECT fl_group_type, ";
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

        sql += " UNION ";

        sql += " SELECT '9' as fl_group_type, ";
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
        sql += " ) c2 ";
        sql += " on main.fl_group_type=c2.fl_group_type ";
        sql += " and main.fl_province=c2.fl_province ";

        //sql += " group by main.fl_group_type,main.fl_province_name,main.fl_province ";

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
        string tmpVal = "";
        while (rs.Read())
        {
            if (tmpVal != rs.GetString(1))
            {
                tab1.Rows.Add(new HtmlTableRow());
                i = tab1.Rows.Count - 1;
                tab1.Rows[i].Attributes.Add("class", "off");
                tab1.Rows[i].Attributes.Add("onmouseover", "this.className='on'");
                tab1.Rows[i].Attributes.Add("onmouseout", "this.className='off'");
                tab1.Rows[i].Cells.Clear();

                maxCols = 17;
                if (subType != 1) maxCols = 9;
                for (int xx = 0; xx < maxCols; xx++)
                {
                    tab1.Rows[i].Cells.Add(new HtmlTableCell());
                    if (xx == 0) tab1.Rows[i].Cells[xx].Align = "LEFT"; tab1.Rows[i].Cells[xx].Align = "RIGHT";
                    tab1.Rows[i].Cells[xx].ColSpan = 1;
                    tab1.Rows[i].Cells[xx].InnerHtml = "&nbsp;";
                }
                
                tmpVal = rs.GetString(1);

                string tmpVal2 = tmpVal;
                if(tmpVal2=="ฮฮฮฮฮฮ") tmpVal2="รวม";
                tab1.Rows[i].Cells[0].InnerHtml = "&nbsp;" + tmpVal2 + "<input type='hidden' name ='cell_" + i.ToString() + "_0' value='" + tmpVal2 + "'>";
            }
            int j = 0;
            if (subType == 1)
            {
                if (rs.GetString(0) == "1") j = 1;
                if (rs.GetString(0) == "2") j = 5;
                if (rs.GetString(0) == "3") j = 9;
                if (rs.GetString(0) == "9") j = 13;
            }
            else
            {
                if (rs.GetString(0) == "9") j = 5; else j = 1;
            }

            for (int xx = 0; xx < 4; xx++)
            {
                if (xx == 0)
                {
                    string action = "group_REP.aspx?c=1";
                    if (rs.GetString(0) != "9") action += "&x=" + rs.GetString(0); else action += "&x=";
                    if (rs.GetString(2) != "99") action += "&y=" + rs.GetString(2); else action += "&y=";

                    tab1.Rows[i].Cells[j + xx].InnerHtml = "<a href='" + action + "' target='_blank'>" + rs.GetValue(xx + 3).ToString() + "</a><input type='hidden' name ='cell_" + i.ToString() + "_" + Convert.ToString(j + xx) + "' value='" + rs.GetValue(xx + 3).ToString() + "'>";
                }
                else
                {
                    tab1.Rows[i].Cells[j + xx].InnerHtml = "" + Convert.ToUInt64(rs.GetValue(xx + 3)).ToString("#,##0") + "<input type='hidden' name ='cell_" + i.ToString() + "_" + Convert.ToString(j + xx) + "' value='" + rs.GetValue(xx + 3).ToString() + "'>";
                }
            }
        }
        maxi.Value = i.ToString();
        rs.Close();

        if ((i - 2) <= 0)
        {
            //noDataFound
            tab1.Rows.Add(new HtmlTableRow());
            tab1.Rows[i].Attributes.Add("class", "off");
            tab1.Rows[i].Attributes.Add("onmouseover", "this.className='on'");
            tab1.Rows[i].Attributes.Add("onmouseout", "this.className='off'");
            tab1.Rows[i].Cells.Clear();

            tab1.Rows[i].Cells.Add(new HtmlTableCell());
            if (subType == 1) tab1.Rows[i].Cells[0].ColSpan = 17; else tab1.Rows[i].Cells[0].ColSpan = 9;
            tab1.Rows[i].Cells[0].Align = "CENTER";
            tab1.Rows[i].Cells[0].InnerHtml = "<font color='red' size='4'><b>" + ConfigurationManager.AppSettings["nodataMsg"] + "</b></font><input type='hidden' name ='cust1' value='" + ConfigurationManager.AppSettings["nodataMsg"] + "'>";
        }
        Conn.Close();

	tabGraph.Visible=true;
        bar.Visible = true;
        tab1.Visible = true;
        mainChart.Visible = true;
        sub1.Visible = true;
        sub2.Visible = true;
    }

    protected void makeChart2(int subType)
    {
        #region header
        tab1.Rows.Clear();
        tab1.BorderColor = ConfigurationManager.AppSettings["gridBorderColor"];
        tab1.Border = 1;
        tab1.CellPadding = 0;
        tab1.CellSpacing = 0;

        tab1.Rows.Add(new HtmlTableRow());
        tab1.Rows[0].Attributes.Add("class", "head");
        tab1.Rows[0].Cells.Add(new HtmlTableCell());
        tab1.Rows[0].Cells[0].InnerHtml = "หน่วยงาน";
        tab1.Rows[0].Cells[0].RowSpan = 2;
        tab1.Rows[0].Cells[0].Width = "200px";

        if (subType == 1)
        {
            tab1.Rows[0].Cells.Add(new HtmlTableCell());
            tab1.Rows[0].Cells[1].ColSpan = 4;
            tab1.Rows[0].Cells[1].InnerHtml = "<center>มวลชนกอรมน.</center>";
            tab1.Rows[0].Cells.Add(new HtmlTableCell());
            tab1.Rows[0].Cells[2].ColSpan = 4;
            tab1.Rows[0].Cells[2].InnerHtml = "<center>มวลชนภาครัฐ</center>";
            tab1.Rows[0].Cells.Add(new HtmlTableCell());
            tab1.Rows[0].Cells[3].ColSpan = 4;
            tab1.Rows[0].Cells[3].InnerHtml = "<center>มวลชนภาคประชาชน</center>";
            tab1.Rows[0].Cells.Add(new HtmlTableCell());
            tab1.Rows[0].Cells[4].ColSpan = 4;
            tab1.Rows[0].Cells[4].InnerHtml = "<center>รวม</center>";
        }
        else
        {
            tab1.Rows[0].Cells.Add(new HtmlTableCell());
            tab1.Rows[0].Cells[1].ColSpan = 4;
            if (subType == 2) tab1.Rows[0].Cells[1].InnerHtml = "<center>มวลชนสานประโยชน์</center>";
            if (subType == 3) tab1.Rows[0].Cells[1].InnerHtml = "<center>มวลชนอุปสรรค</center>";
            tab1.Rows[0].Cells.Add(new HtmlTableCell());
            tab1.Rows[0].Cells[2].ColSpan = 4;
            tab1.Rows[0].Cells[2].InnerHtml = "<center>รวม</center>";
        }

        tab1.Rows.Add(new HtmlTableRow());
        tab1.Rows[1].Attributes.Add("class", "head");
        int maxCols = 3;
        if (subType != 1) maxCols = 1;
        for (int mcols = 0; mcols <= maxCols; mcols++)
        {
            for (int scols = 0; scols <= 3; scols++)
            {
                tab1.Rows[1].Cells.Add(new HtmlTableCell());
                string celltxt = "";
                if (scols == 0) celltxt = "กลุ่ม";
                if (scols == 1) celltxt = "รายงาน";
                if (scols == 2) celltxt = "รายชื่อ";
                if (scols == 3) celltxt = "ปฎิบัติการ";
                tab1.Rows[1].Cells[mcols * 4 + scols].InnerHtml = celltxt;
                tab1.Rows[1].Cells[mcols * 4 + scols].Width = "100px";
            }
        }
        #endregion

        #region whereCompose
        string whereClause = "where 1=1 ";
        string subClause = "";
        string linkSub = "";
        string titleParam = "";
        string linkParam = "?repID=" + cmbReport.SelectedValue;
        lblRepTitle.Text = "";

        for (int xx = 0; xx < cmbDept.Items.Count; xx++)
        {
            if (cmbDept.Items[xx].Selected)
            {
                if (subClause != "") subClause += " or ";
                subClause = subClause + " isnull(tb_detailgroup.fl_dept,'') like '" + cmbDept.Items[xx].Value.Replace("0", "") + "%' ";

                if (linkSub != "") linkSub += "|";
                linkSub += cmbDept.Items[xx].Value;

                if (titleParam != "") titleParam += ",";
                titleParam += cmbDept.Items[xx].Text;
            }
        }
        if (subClause != "") whereClause += " and (" + subClause + ") ";
        if (linkSub != "") linkParam += "&deptID=" + linkSub;

        subClause = "";
        linkSub = "";

        for (int xx = 0; xx < cmbProvince.Items.Count; xx++)
        {
            if (cmbProvince.Items[xx].Selected)
            {
                if (subClause != "") subClause += " or ";
                subClause = subClause + " tb_detailgroup.fl_province = '" + cmbProvince.Items[xx].Value + "' ";

                if (linkSub != "") linkSub += "|";
                linkSub += cmbProvince.Items[xx].Value;

                if (titleParam != "") titleParam += ",";
                titleParam += cmbProvince.Items[xx].Text;
            }
        }
        if (subClause != "") whereClause += " and (" + subClause + ") ";
        if (linkSub != "") linkParam += "&proID=" + linkSub;
        if (titleParam != "") lblRepTitle.Text += " จังหวัด: " + titleParam + " ";

        subClause = "";
        linkSub = "";
        titleParam = "";

        for (int xx = 0; xx < cmbMassGroup.Items.Count; xx++)
        {
            if (cmbMassGroup.Items[xx].Selected)
            {
                if (subClause != "") subClause += " or ";
                subClause = subClause + " tb_detailgroup.fl_group_id = '" + cmbMassGroup.Items[xx].Value + "' ";

                if (linkSub != "") linkSub += "|";
                linkSub += cmbMassGroup.Items[xx].Value;

                if (titleParam != "") titleParam += ",";
                titleParam += cmbMassGroup.Items[xx].Text;
            }
        }
        if (subClause != "") whereClause += " and (" + subClause + ") ";
        if (linkSub != "") linkParam += "&massID=" + linkSub;
        if (titleParam != "") lblRepTitle.Text += "มวลชน: " + titleParam + " ";

        //Filter Right
        //if (Session["uGroup"].ToString() == "U") whereClause += " and isnull(tb_detailgroup.fl_dept,'') like '" + Session["uDept"].ToString().Replace("0", "") + "%' ";
        if (Session["uGroup"].ToString() == "U")
        {
            if (Session["uDept"].ToString().Substring(0) != "0") whereClause += " and isnull(tb_detailgroup.fl_dept,'') like '" + Session["uDept"].ToString().Replace("00", "") + "%' ";
        }
        //Report WhereClause
        if (subType == 1) whereClause += " and tb_maingroup.fl_group_type <='3' ";
        if (subType == 2) whereClause += " and tb_maingroup.fl_group_type ='4' ";
        if (subType == 3) whereClause += " and tb_maingroup.fl_group_type ='5' ";

        Session["whereClause"] = whereClause;

        string orderClause = " order by main.fl_dept, ";
        orderClause += " main.fl_group_type ";

        lblRepTitle.Text = "รายงานข้อมูล " + lblRepTitle.Text;
        #endregion

        mainChart.ImageUrl = "G1.aspx" + linkParam;
        sub1.ImageUrl = "G2.aspx" + linkParam;
        sub2.ImageUrl = "G3.aspx" + linkParam;

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

        sql += " (SELECT distinct ";
        sql += "    fl_group_type, ";
        sql += "    fl_dept, ";
        sql += "    fl_dept_name, ";
        sql += "    isnull(count(fl_id),0) as idCount, ";
        sql += "    isnull(sum(cast(isnull(fl_reportMember,'0') as bigint)),0) as reportCount ";
        sql += " from tb_detailgroup ";
        sql += " inner join tb_maingroup  on tb_detailgroup.fl_group_id = tb_maingroup.fl_group_id ";
        sql += " inner join (select distinct fl_dept_id,fl_dept_name from tb_dept) moiCode  on tb_detailgroup.fl_dept = moiCode.fl_dept_id ";
        sql += whereClause;
        sql += " group by fl_group_type,fl_dept_name,fl_dept ";

        sql += " UNION ";

        sql += " SELECT '9' as fl_group_type, ";
        sql += "    fl_dept, ";
        sql += "    fl_dept_name, ";
        sql += "    isnull(count(fl_id),0) as idCount, ";
        sql += "    isnull(sum(cast(isnull(fl_reportMember,'0') as bigint)),0) as reportCount ";
        sql += " from tb_detailgroup ";
        sql += " inner join tb_maingroup  on tb_detailgroup.fl_group_id = tb_maingroup.fl_group_id ";
        sql += " inner join (select distinct fl_dept_id,fl_dept_name from tb_dept) moiCode  on tb_detailgroup.fl_dept = moiCode.fl_dept_id ";
        sql += whereClause;
        sql += " group by fl_dept_name,fl_dept ";

        sql += " UNION ";

        sql += " SELECT fl_group_type, ";
        sql += "    '99' as fl_dept, ";
        sql += "    'ฮฮฮฮฮฮ' as fl_dept_name, ";
        sql += "    isnull(count(fl_id),0) as idCount, ";
        sql += "    isnull(sum(cast(isnull(fl_reportMember,'0') as bigint)),0) as reportCount ";
        sql += " from tb_detailgroup ";
        sql += " inner join tb_maingroup  on tb_detailgroup.fl_group_id = tb_maingroup.fl_group_id ";
        sql += " inner join (select distinct fl_dept_id,fl_dept_name from tb_dept) moiCode  on tb_detailgroup.fl_dept = moiCode.fl_dept_id ";
        sql += whereClause;
        sql += " group by fl_group_type ";

        sql += " UNION ";

        sql += " SELECT '9' as fl_group_type, ";
        sql += "    '99' as fl_dept, ";
        sql += "    'ฮฮฮฮฮฮ' as fl_dept_name, ";
        sql += "    isnull(count(fl_id),0) as idCount, ";
        sql += "    isnull(sum(cast(isnull(fl_reportMember,'0') as bigint)),0) as reportCount ";
        sql += " from tb_detailgroup ";
        sql += " inner join tb_maingroup  on tb_detailgroup.fl_group_id = tb_maingroup.fl_group_id ";
        sql += " inner join (select distinct fl_dept_id,fl_dept_name from tb_dept) moiCode  on tb_detailgroup.fl_dept = moiCode.fl_dept_id ";
        sql += whereClause;
        sql += " ) main ";

        sql += " left outer join ";

        sql += " (select distinct ";
        sql += " fl_group_type, ";
        sql += " fl_dept, ";
        sql += " isnull(count(tb_membergroup.fl_citizen_id),0) as headCount ";
        sql += " from tb_detailgroup ";
        sql += " inner join tb_maingroup on tb_detailgroup.fl_group_id = tb_maingroup.fl_group_id ";
        sql += " inner join tb_membergroup on tb_detailgroup.fl_id = tb_membergroup.fl_detailgroup_id ";
        sql += " inner join tb_citizen on tb_membergroup.fl_citizen_id = tb_citizen.fl_citizen_id ";
        sql += whereClause;
        sql += " and (fl_stop='' or fl_stop is null) ";
        sql += " and tb_citizen.fl_status='1' ";
        sql += " group by fl_group_type,fl_dept ";

        sql += " UNION ";

        sql += " SELECT '9' as fl_group_type, ";
        sql += " fl_dept, ";
        sql += " isnull(count(tb_membergroup.fl_citizen_id),0) as headCount ";
        sql += " from tb_detailgroup ";
        sql += " inner join tb_maingroup  on tb_detailgroup.fl_group_id = tb_maingroup.fl_group_id ";
        sql += " inner join tb_membergroup on tb_detailgroup.fl_id = tb_membergroup.fl_detailgroup_id ";
        sql += " inner join tb_citizen on tb_membergroup.fl_citizen_id = tb_citizen.fl_citizen_id ";
        sql += whereClause;
        sql += " and (fl_stop='' or fl_stop is null) ";
        sql += " and tb_citizen.fl_status='1' ";
        sql += " group by fl_dept ";

        sql += " UNION ";

        sql += " SELECT fl_group_type, ";
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

        sql += " UNION ";

        sql += " SELECT '9' as fl_group_type, ";
        sql += " '99' as fl_dept, ";
        sql += " isnull(count(tb_membergroup.fl_citizen_id),0) as headCount ";
        sql += " from tb_detailgroup ";
        sql += " inner join tb_maingroup  on tb_detailgroup.fl_group_id = tb_maingroup.fl_group_id ";
        sql += " inner join tb_membergroup on tb_detailgroup.fl_id = tb_membergroup.fl_detailgroup_id ";
        sql += " inner join tb_citizen on tb_membergroup.fl_citizen_id = tb_citizen.fl_citizen_id ";
        sql += whereClause;
        sql += " and (fl_stop='' or fl_stop is null) ";
        sql += " and tb_citizen.fl_status='1' ";
        sql += " ) c1 ";

        sql += " on main.fl_group_type=c1.fl_group_type ";
        sql += " and main.fl_dept=c1.fl_dept ";

        sql += " left outer join ";

        sql += " (select distinct ";
        sql += " fl_group_type, ";
        sql += " fl_dept, ";
        sql += " isnull(count(tb_membergroup.fl_citizen_id),0) as headCount2 ";
        sql += " from tb_detailgroup ";
        sql += " inner join tb_maingroup on tb_detailgroup.fl_group_id = tb_maingroup.fl_group_id ";
        sql += " inner join tb_membergroup on tb_detailgroup.fl_id = tb_membergroup.fl_detailgroup_id ";
        sql += " inner join tb_citizen on tb_membergroup.fl_citizen_id = tb_citizen.fl_citizen_id ";
        sql += whereClause;
        sql += " and (fl_stop='' or fl_stop is null) ";
        sql += " and tb_citizen.fl_status='1' ";
        sql += " and tb_citizen.fl_targetFlag='1' ";
        sql += " group by fl_group_type,fl_dept ";

        sql += " UNION ";

        sql += " SELECT '9' as fl_group_type, ";
        sql += " fl_dept, ";
        sql += " isnull(count(tb_membergroup.fl_citizen_id),0) as headCount2 ";
        sql += " from tb_detailgroup ";
        sql += " inner join tb_maingroup  on tb_detailgroup.fl_group_id = tb_maingroup.fl_group_id ";
        sql += " inner join tb_membergroup on tb_detailgroup.fl_id = tb_membergroup.fl_detailgroup_id ";
        sql += " inner join tb_citizen on tb_membergroup.fl_citizen_id = tb_citizen.fl_citizen_id ";
        sql += whereClause;
        sql += " and (fl_stop='' or fl_stop is null) ";
        sql += " and tb_citizen.fl_status='1' ";
        sql += " and tb_citizen.fl_targetFlag='1' ";
        sql += " group by fl_dept ";

        sql += " UNION ";

        sql += " SELECT fl_group_type, ";
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

        sql += " UNION ";

        sql += " SELECT '9' as fl_group_type, ";
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
        sql += " ) c2 ";
        sql += " on main.fl_group_type=c2.fl_group_type ";
        sql += " and main.fl_dept=c2.fl_dept ";

        //sql += " group by main.fl_group_type,main.fl_dept,main.fl_dept_name ";

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
        string tmpVal = "";
        while (rs.Read())
        {
            if (tmpVal != rs.GetString(1))
            {
                tab1.Rows.Add(new HtmlTableRow());
                i = tab1.Rows.Count - 1;
                tab1.Rows[i].Attributes.Add("class", "off");
                tab1.Rows[i].Attributes.Add("onmouseover", "this.className='on'");
                tab1.Rows[i].Attributes.Add("onmouseout", "this.className='off'");
                tab1.Rows[i].Cells.Clear();

                maxCols = 17;
                if (subType != 1) maxCols = 9;
                for (int xx = 0; xx < maxCols; xx++)
                {
                    tab1.Rows[i].Cells.Add(new HtmlTableCell());
                    if (xx == 0) tab1.Rows[i].Cells[xx].Align = "LEFT"; tab1.Rows[i].Cells[xx].Align = "RIGHT";
                    tab1.Rows[i].Cells[xx].ColSpan = 1;
                    tab1.Rows[i].Cells[xx].InnerHtml = "&nbsp;";
                }

                tmpVal = rs.GetString(1);

                string tmpVal2 = tmpVal;
                if (tmpVal2 == "ฮฮฮฮฮฮ") tmpVal2 = "รวม";
                tab1.Rows[i].Cells[0].InnerHtml = "&nbsp;" + tmpVal2 + "<input type='hidden' name ='cell_" + i.ToString() + "_0' value='" + tmpVal2 + "'>";
            }
            int j = 0;
            if (subType == 1)
            {
                if (rs.GetString(0) == "1") j = 1;
                if (rs.GetString(0) == "2") j = 5;
                if (rs.GetString(0) == "3") j = 9;
                if (rs.GetString(0) == "9") j = 13;
            }
            else
            {
                if (rs.GetString(0) == "9") j = 5; else j = 1;
            }

            for (int xx = 0; xx < 4; xx++)
            {
                if (xx == 0)
                {
                    string action = "group_REP.aspx?c=2";
                    if (rs.GetString(0) != "9") action += "&x=" + rs.GetString(0); else action += "&x=";
                    if (rs.GetString(2) != "99") action += "&y=" + rs.GetString(2); else action += "&y=";

                    tab1.Rows[i].Cells[j + xx].InnerHtml = "<a href='" + action + "' target='_blank'>" + rs.GetValue(xx + 3).ToString() + "</a><input type='hidden' name ='cell_" + i.ToString() + "_" + Convert.ToString(j + xx) + "' value='" + rs.GetValue(xx + 3).ToString() + "'>";
                }
                else
                {
                    tab1.Rows[i].Cells[j + xx].InnerHtml = "" + Convert.ToUInt64(rs.GetValue(xx + 3)).ToString("#,##0") + "<input type='hidden' name ='cell_" + i.ToString() + "_" + Convert.ToString(j + xx) + "' value='" + rs.GetValue(xx + 3).ToString() + "'>";
                }
            }
        }
        maxi.Value = i.ToString();
        rs.Close();

        if ((i - 2) <= 0)
        {
            //noDataFound
            tab1.Rows.Add(new HtmlTableRow());
            tab1.Rows[i].Attributes.Add("class", "off");
            tab1.Rows[i].Attributes.Add("onmouseover", "this.className='on'");
            tab1.Rows[i].Attributes.Add("onmouseout", "this.className='off'");
            tab1.Rows[i].Cells.Clear();

            tab1.Rows[i].Cells.Add(new HtmlTableCell());
            if (subType == 1) tab1.Rows[i].Cells[0].ColSpan = 17; else tab1.Rows[i].Cells[0].ColSpan = 9;
            tab1.Rows[i].Cells[0].Align = "CENTER";
            tab1.Rows[i].Cells[0].InnerHtml = "<font color='red' size='4'><b>" + ConfigurationManager.AppSettings["nodataMsg"] + "</b></font><input type='hidden' name ='cust1' value='" + ConfigurationManager.AppSettings["nodataMsg"] + "'>";
        }
        Conn.Close();

	tabGraph.Visible=true;
        bar.Visible = true;
        tab1.Visible = true;
        mainChart.Visible = true;
        sub1.Visible = true;
        sub2.Visible = true;
    }

    protected void makeChart3()
    {
        #region header
        tab1.Rows.Clear();
        tab1.BorderColor = ConfigurationManager.AppSettings["gridBorderColor"];
        tab1.Border = 1;
        tab1.CellPadding = 0;
        tab1.CellSpacing = 0;

        tab1.Rows.Add(new HtmlTableRow());
        tab1.Rows[0].Attributes.Add("class", "head");
        tab1.Rows[0].Cells.Add(new HtmlTableCell());
        tab1.Rows[0].Cells[0].InnerHtml = "ปี";
        tab1.Rows[0].Cells[0].RowSpan = 2;
        tab1.Rows[0].Cells[0].Width = "200px";
        tab1.Rows[0].Cells.Add(new HtmlTableCell());
        tab1.Rows[0].Cells[1].ColSpan = 4;
        tab1.Rows[0].Cells[1].InnerHtml = "<center>มวลชนกอรมน.</center>";
        tab1.Rows[0].Cells.Add(new HtmlTableCell());
        tab1.Rows[0].Cells[2].ColSpan = 4;
        tab1.Rows[0].Cells[2].InnerHtml = "<center>มวลชนภาครัฐ</center>";
        tab1.Rows[0].Cells.Add(new HtmlTableCell());
        tab1.Rows[0].Cells[3].ColSpan = 4;
        tab1.Rows[0].Cells[3].InnerHtml = "<center>มวลชนภาคประชาชน</center>";
        tab1.Rows[0].Cells.Add(new HtmlTableCell());
        tab1.Rows[0].Cells[4].ColSpan = 4;
        tab1.Rows[0].Cells[4].InnerHtml = "<center>รวม</center>";

        tab1.Rows.Add(new HtmlTableRow());
        tab1.Rows[1].Attributes.Add("class", "head");
        for (int mcols = 0; mcols <= 3; mcols++)
        {
            for (int scols = 0; scols <= 3; scols++)
            {
                tab1.Rows[1].Cells.Add(new HtmlTableCell());
                string celltxt = "";
                if (scols == 0) celltxt = "กลุ่ม";
                if (scols == 1) celltxt = "รายชื่อ";
                if (scols == 2) celltxt = "เติบโตกลุ่ม";
                if (scols == 3) celltxt = "เติบโตรายชื่อ";
                tab1.Rows[1].Cells[mcols * 4 + scols].InnerHtml = celltxt;
                tab1.Rows[1].Cells[mcols * 4 + scols].Width = "100px";
            }
        }
        #endregion

        #region whereCompose
        string whereClause = "where 1=1 ";
        string subClause = "";
        string linkSub = "";
        string titleParam = "";
        string linkParam = "?repID=" + cmbReport.SelectedValue;
        lblRepTitle.Text = "";

        for (int xx = 0; xx < cmbDept.Items.Count; xx++)
        {
            if (cmbDept.Items[xx].Selected)
            {
                if (subClause != "") subClause += " or ";
                subClause = subClause + " isnull(tb_detailgroup.fl_dept,'') like '" + cmbDept.Items[xx].Value.Replace("0", "") + "%' ";

                if (linkSub != "") linkSub += "|";
                linkSub += cmbDept.Items[xx].Value;

                if (titleParam != "") titleParam += ",";
                titleParam += cmbDept.Items[xx].Text;
            }
        }
        if (subClause != "") whereClause += " and (" + subClause + ") ";
        if (linkSub != "") linkParam += "&deptID=" + linkSub;
        if (titleParam != "") lblRepTitle.Text += " หน่วยงาน: " + titleParam + " ";

        subClause = "";
        linkSub = "";
        titleParam = "";

        for (int xx = 0; xx < cmbProvince.Items.Count; xx++)
        {
            if (cmbProvince.Items[xx].Selected)
            {
                if (subClause != "") subClause += " or ";
                subClause = subClause + " tb_detailgroup.fl_province = '" + cmbProvince.Items[xx].Value + "' ";

                if (linkSub != "") linkSub += "|";
                linkSub += cmbProvince.Items[xx].Value;

                if (titleParam != "") titleParam += ",";
                titleParam += cmbProvince.Items[xx].Text;
            }
        }
        if (subClause != "") whereClause += " and (" + subClause + ") ";
        if (linkSub != "") linkParam += "&proID=" + linkSub;
        if (titleParam != "") lblRepTitle.Text += "จังหวัด: " + titleParam + " ";

        subClause = "";
        linkSub = "";
        titleParam = "";

        for (int xx = 0; xx < cmbMassGroup.Items.Count; xx++)
        {
            if (cmbMassGroup.Items[xx].Selected)
            {
                if (subClause != "") subClause += " or ";
                subClause = subClause + " tb_detailgroup.fl_group_id = '" + cmbMassGroup.Items[xx].Value + "' ";

                if (linkSub != "") linkSub += "|";
                linkSub += cmbMassGroup.Items[xx].Value;

                if (titleParam != "") titleParam += ",";
                titleParam += cmbMassGroup.Items[xx].Text;
            }
        }
        if (subClause != "") whereClause += " and (" + subClause + ") ";
        if (linkSub != "") linkParam += "&massID=" + linkSub;
        if (titleParam != "") lblRepTitle.Text += "มวลชน: " + titleParam + " ";

        //Filter Right
        //if (Session["uGroup"].ToString() == "U") whereClause += " and isnull(tb_detailgroup.fl_dept,'') like '" + Session["uDept"].ToString().Replace("0", "") + "%' ";
        if (Session["uGroup"].ToString() == "U")
        {
            if (Session["uDept"].ToString().Substring(0) != "0") whereClause += " and isnull(tb_detailgroup.fl_dept,'') like '" + Session["uDept"].ToString().Replace("00", "") + "%' ";
        }
        //Report WhereClause
        whereClause += " and tb_maingroup.fl_group_type <='3' ";

        Session["whereClause"] = whereClause;

        string orderClause = " order by main.fl_yearID, ";
        orderClause += " main.fl_group_type ";
        #endregion

        mainChart.ImageUrl = "G1.aspx" + linkParam;
        sub1.ImageUrl = "G2.aspx" + linkParam;
        sub2.ImageUrl = "G3.aspx" + linkParam;

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

        sql += " UNION ";

        sql += "SELECT distinct ";
        sql += "    '9' fl_group_type, ";
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
        
        sql += " UNION ";

        sql += " SELECT distinct ";
        sql += "    '9' fl_group_type, ";
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
        sql += " ) z ";

        sql += " UNION ";

        sql += " SELECT distinct ";
        sql += "    '9' fl_group_type, ";
        sql += "    '" + Convert.ToString(DateTime.Now.Year + 543) + "' as  fl_YearID, ";
        sql += " isnull(count(fl_id),0) as idCount, ";
        sql += " isnull(sum(cast(isnull(fl_reportMember,'0') as bigint)),0) as reportCount ";
        sql += " from (SELECT distinct fl_id,fl_reportMember,fl_group_type from tb_detailgroup ";
        sql += " inner join tb_maingroup  on tb_detailgroup.fl_group_id = tb_maingroup.fl_group_id ";
        sql += " inner join tb_membergroup on tb_detailgroup.fl_id = tb_membergroup.fl_detailgroup_id ";
        sql += whereClause;
        //At end of year
        sql += " and (isnull(fl_stop,'')='')  ";
        sql += " ) z";
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

        sql += " UNION ";

        sql += "SELECT distinct ";
        sql += "    '9' fl_group_type, ";
        sql += "    '" + Convert.ToString(DateTime.Now.Year + 543 - 2) + "' as  fl_YearID, ";
        sql += " isnull(count(tb_membergroup.fl_citizen_id),0) as headCount ";
        sql += " from tb_detailgroup ";
        sql += " inner join tb_maingroup  on tb_detailgroup.fl_group_id = tb_maingroup.fl_group_id ";
        sql += " inner join tb_membergroup on tb_detailgroup.fl_id = tb_membergroup.fl_detailgroup_id ";
        sql += whereClause;
        //At end of year
        sql += " and fl_start <= '" + Convert.ToString(DateTime.Now.Year - 1) + "' ";
        sql += " and (isnull(fl_stop,'')='' or isnull(fl_stop,'') > '" + Convert.ToString(DateTime.Now.Year - 1) + "') ";

        sql += " UNION ";

        sql += " SELECT distinct ";
        sql += "    '9' fl_group_type, ";
        sql += "    '" + Convert.ToString(DateTime.Now.Year + 543 - 1) + "' as  fl_YearID, ";
        sql += " isnull(count(tb_membergroup.fl_citizen_id),0) as headCount ";
        sql += " from tb_detailgroup ";
        sql += " inner join tb_maingroup  on tb_detailgroup.fl_group_id = tb_maingroup.fl_group_id ";
        sql += " inner join tb_membergroup on tb_detailgroup.fl_id = tb_membergroup.fl_detailgroup_id ";
        sql += whereClause;
        //At end of year
        sql += " and fl_start <= '" + Convert.ToString(DateTime.Now.Year) + "' ";
        sql += " and (isnull(fl_stop,'')='' or isnull(fl_stop,'') > '" + Convert.ToString(DateTime.Now.Year) + "') ";

        sql += " UNION ";

        sql += " SELECT distinct ";
        sql += "    '9' fl_group_type, ";
        sql += "    '" + Convert.ToString(DateTime.Now.Year + 543) + "' as  fl_YearID, ";
        sql += " isnull(count(tb_membergroup.fl_citizen_id),0) as headCount ";
        sql += " from tb_detailgroup ";
        sql += " inner join tb_maingroup  on tb_detailgroup.fl_group_id = tb_maingroup.fl_group_id ";
        sql += " inner join tb_membergroup on tb_detailgroup.fl_id = tb_membergroup.fl_detailgroup_id ";
        sql += whereClause;
        //At end of year
        sql += " and (isnull(fl_stop,'')='') ";
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

        int i = 1;
        string tmpVal = "";
        long tmpLY1 = 0;
        long tmpLY2 = 0;
        long tmpLY3 = 0;
        long tmpLY4 = 0;

        long tmpL1Y1 = 0;
        long tmpL1Y2 = 0;
        long tmpL1Y3 = 0;
        long tmpL1Y4 = 0;

        double amtPlot1 = 0;
        double amtPlot2 = 0; 
                            
        while (rs.Read())
        {
            if (tmpVal != rs.GetString(1))
            {
                tab1.Rows.Add(new HtmlTableRow());
                i = tab1.Rows.Count - 1;
                tab1.Rows[i].Attributes.Add("class", "off");
                tab1.Rows[i].Attributes.Add("onmouseover", "this.className='on'");
                tab1.Rows[i].Attributes.Add("onmouseout", "this.className='off'");
                tab1.Rows[i].Cells.Clear();

                for (int xx = 0; xx < 17; xx++)
                {
                    tab1.Rows[i].Cells.Add(new HtmlTableCell());
                    if (xx == 0) tab1.Rows[i].Cells[xx].Align = "LEFT"; tab1.Rows[i].Cells[xx].Align = "RIGHT";
                    tab1.Rows[i].Cells[xx].ColSpan = 1;
                    tab1.Rows[i].Cells[xx].InnerHtml = "&nbsp;";
                }

                tmpVal = rs.GetString(1);

                string tmpVal2 = tmpVal;
                tab1.Rows[i].Cells[0].InnerHtml = "&nbsp;" + tmpVal2 + "<input type='hidden' name ='cell_" + i.ToString() + "_0' value='" + tmpVal2 + "'>";
            }

            if (rs.GetString(0) == "1")
            {
                if (tmpLY1 != 0) amtPlot1 = (Convert.ToInt64(rs.GetValue(2)) - tmpLY1) *1.0 / tmpLY1 * 100;
                tmpLY1 = Convert.ToInt64(rs.GetValue(2));

                if (tmpL1Y1 != 0) amtPlot2 = (Convert.ToInt64(rs.GetValue(4)) - tmpL1Y1) *1.0 / tmpL1Y1 * 100;
                tmpL1Y1 = Convert.ToInt64(rs.GetValue(4));
            }
            if (rs.GetString(0) == "2")
            {
                if (tmpLY2 != 0) amtPlot1 = (Convert.ToInt64(rs.GetValue(2)) - tmpLY2) *1.0 / tmpLY2 * 100;
                tmpLY2 = Convert.ToInt64(rs.GetValue(2));

                if (tmpL1Y2 != 0) amtPlot2 = (Convert.ToInt64(rs.GetValue(4)) - tmpL1Y2) * 1.0 / tmpL1Y2 * 100;
                tmpL1Y2 = Convert.ToInt64(rs.GetValue(4));
            }
            if (rs.GetString(0) == "3")
            {
                if (tmpLY3 != 0) amtPlot1 = (Convert.ToInt64(rs.GetValue(2)) - tmpLY3) * 1.0 / tmpLY3 * 100;
                tmpLY3 = Convert.ToInt64(rs.GetValue(2));

                if (tmpL1Y3 != 0) amtPlot2 = (Convert.ToInt64(rs.GetValue(4)) - tmpL1Y3) * 1.0 / tmpL1Y3 * 100;
                tmpL1Y3 = Convert.ToInt64(rs.GetValue(4));
            }
            if (rs.GetString(0) == "9")
            {
                if (tmpLY4 != 0) amtPlot1 = (Convert.ToInt64(rs.GetValue(2)) - tmpLY4) * 1.0 / tmpLY4 * 100;
                tmpLY4 = Convert.ToInt64(rs.GetValue(2));

                if (tmpL1Y4 != 0) amtPlot2 = (Convert.ToInt64(rs.GetValue(4)) - tmpL1Y4) * 1.0 / tmpL1Y4 * 100;
                tmpL1Y4 = Convert.ToInt64(rs.GetValue(4));                
            }

            int j = 0;
            if (rs.GetString(0) == "1") j = 1;
            if (rs.GetString(0) == "2") j = 5;
            if (rs.GetString(0) == "3") j = 9;
            if (rs.GetString(0) == "9") j = 13;

            string action = "group_REP.aspx?c=3";
            if (rs.GetString(0) != "9") action += "&x=" + rs.GetString(0); else action += "&x=";
            action += "&y=" + rs.GetString(1);

            tab1.Rows[i].Cells[j + 0].InnerHtml = "<a href='" + action + "' target='_blank'>" + Convert.ToInt64(rs.GetValue(2)).ToString("#,##0") + "</a><input type='hidden' name ='cell_" + i.ToString() + "_" + Convert.ToString(j + 0) + "' value='" + rs.GetValue(2).ToString() + "'>";
            tab1.Rows[i].Cells[j + 1].InnerHtml = "" + Convert.ToInt64(rs.GetValue(4)).ToString("#,##0") + "<input type='hidden' name ='cell_" + i.ToString() + "_" + Convert.ToString(j + 2) + "' value='" + rs.GetValue(4).ToString() + "'>";
            tab1.Rows[i].Cells[j + 2].InnerHtml = "" + amtPlot1.ToString("#,##0.00") + "</a><input type='hidden' name ='cell_" + i.ToString() + "_" + Convert.ToString(j + 1) + "' value='" + amtPlot1.ToString() + "'>";
            tab1.Rows[i].Cells[j + 3].InnerHtml = "" + amtPlot2.ToString("#,##0.00") + "<input type='hidden' name ='cell_" + i.ToString() + "_" + Convert.ToString(j + 3) + "' value='" + amtPlot2.ToString() + "'>";            
        }
        maxi.Value = i.ToString();
        rs.Close();

        if ((i - 2) <= 0)
        {
            //noDataFound
            tab1.Rows.Add(new HtmlTableRow());
            tab1.Rows[i].Attributes.Add("class", "off");
            tab1.Rows[i].Attributes.Add("onmouseover", "this.className='on'");
            tab1.Rows[i].Attributes.Add("onmouseout", "this.className='off'");
            tab1.Rows[i].Cells.Clear();

            tab1.Rows[i].Cells.Add(new HtmlTableCell());
            tab1.Rows[i].Cells[0].ColSpan = 17;
            tab1.Rows[i].Cells[0].Align = "CENTER";
            tab1.Rows[i].Cells[0].InnerHtml = "<font color='red' size='4'><b>" + ConfigurationManager.AppSettings["nodataMsg"] + "</b></font><input type='hidden' name ='cust1' value='" + ConfigurationManager.AppSettings["nodataMsg"] + "'>";
        }
        Conn.Close();

	tabGraph.Visible=true;
        bar.Visible = true;
        tab1.Visible = true;
        mainChart.Visible = true;
        sub1.Visible = true;
        sub2.Visible = true;
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
        
		if (!Page.IsPostBack)
		{
				deptDataSet();
                provinceDataSet();
                massDataSet();

                reportDataSet();

		tabGraph.Visible=false;

                tab1.Visible = false;
                mainChart.Visible = false;
                sub1.Visible = false;
                sub2.Visible = false;

                pdfPrint.Visible = false;
                xclPrint.Visible = false;

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

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (cmbReport.SelectedValue == "11") makeChart1(1);
        if (cmbReport.SelectedValue == "12") makeChart2(1);
        if (cmbReport.SelectedValue == "20") makeChart3();
        if (cmbReport.SelectedValue == "31") makeChart1(2);
        if (cmbReport.SelectedValue == "32") makeChart2(2);
        if (cmbReport.SelectedValue == "41") makeChart1(3);
        if (cmbReport.SelectedValue == "42") makeChart2(3);

        pdfPrint.Visible = true;
        //xclPrint.Visible = true;
    }
    protected void pdfPrint_Click(object sender, ImageClickEventArgs e)
    {
        if (cmbReport.SelectedValue == "11") makePDF1(1);
        if (cmbReport.SelectedValue == "12") makePDF2(1);
        if (cmbReport.SelectedValue == "20") makePDF3();
        if (cmbReport.SelectedValue == "31") makePDF1(2);
        if (cmbReport.SelectedValue == "32") makePDF2(2);
        if (cmbReport.SelectedValue == "41") makePDF1(3);
        if (cmbReport.SelectedValue == "42") makePDF2(3);
    }

    protected void makePDF1(int subType)
    {

        #region header
        DataTable dt = new DataTable();

        dt.Columns.Add("param1");
        dt.Columns.Add("param2");
        dt.Columns.Add("param3");
        dt.Columns.Add("param4");
        dt.Columns.Add("param5");

        dt.Columns.Add("name1");
        dt.Columns.Add("name2");
        dt.Columns.Add("name3");
        dt.Columns.Add("name4");
        dt.Columns.Add("name5");
        dt.Columns.Add("name6");
        dt.Columns.Add("name7");
        dt.Columns.Add("name8");
        dt.Columns.Add("name9");
        dt.Columns.Add("name10");
        dt.Columns.Add("name11");
        dt.Columns.Add("name12");
        dt.Columns.Add("name13");
        dt.Columns.Add("name14");
        dt.Columns.Add("name15");
        dt.Columns.Add("name16");
        dt.Columns.Add("name17");

        dt.Columns.Add("no1");
        dt.Columns.Add("no2");
        dt.Columns.Add("no3");
        dt.Columns.Add("no4");

        dt.Columns.Add("baht1");
        dt.Columns.Add("baht2");
        dt.Columns.Add("baht3");
        dt.Columns.Add("baht4");
        dt.Columns.Add("baht5");
        dt.Columns.Add("baht6");
        dt.Columns.Add("baht7");
        dt.Columns.Add("baht8");
        dt.Columns.Add("baht9");
        dt.Columns.Add("baht10");
        dt.Columns.Add("baht11");
        dt.Columns.Add("baht12");
        dt.Columns.Add("baht13");
        dt.Columns.Add("baht14");
        dt.Columns.Add("baht15");
        dt.Columns.Add("baht16");
        dt.Columns.Add("baht17");

        DataRow dr = dt.NewRow();
        dr["baht1"] = "จังหวัด";

        if (subType == 1)
        {
            dr["no1"] = "มวลชนกอรมน.";
            dr["no2"] = "มวลชนภาครัฐ";
            dr["no3"] = "มวลชนภาคประชาชน";
            dr["no4"] = "รวม";
        }
        else
        {
            if (subType == 2) dr["no1"] = "มวลชนสานประโยชน์";
            if (subType == 3) dr["no1"] = "มวลชนอุปสรรค";
            dr["no2"] = "รวม";
        }
        int maxCols = 3;
        if (subType != 1) maxCols = 1;
        for (int mcols = 0; mcols <= maxCols; mcols++)
        {
            for (int scols = 0; scols <= 3; scols++)
            {
                string celltxt = "";
                if (scols == 0) celltxt = "กลุ่ม";
                if (scols == 1) celltxt = "รายงาน";
                if (scols == 2) celltxt = "รายชื่อ";
                if (scols == 3) celltxt = "ปฎิบัติการ";
                int index = mcols * 4 + scols + 2;
                dr["baht" + index.ToString()] = celltxt;
            }
        }
        #endregion

        #region whereCompose
        string whereClause = "where 1=1 ";
        string subClause = "";
        string linkSub = "";
        string linkParam = "?repID=" + cmbReport.SelectedValue;

        for (int xx = 0; xx < cmbDept.Items.Count; xx++)
        {
            if (cmbDept.Items[xx].Selected)
            {
                if (subClause != "") subClause += " or ";
                subClause = subClause + " isnull(tb_detailgroup.fl_dept,'') like '" + cmbDept.Items[xx].Value.Replace("0", "") + "%' ";

                if (linkSub != "") linkSub += "|";
                linkSub += cmbDept.Items[xx].Value;
            }
        }
        if (subClause != "") whereClause += " and (" + subClause + ") ";
        if (linkSub != "") linkParam += "&deptID=" + linkSub;

        subClause = "";
        linkSub = "";

        for (int xx = 0; xx < cmbProvince.Items.Count; xx++)
        {
            if (cmbProvince.Items[xx].Selected)
            {
                if (subClause != "") subClause += " or ";
                subClause = subClause + " tb_detailgroup.fl_province = '" + cmbProvince.Items[xx].Value + "' ";

                if (linkSub != "") linkSub += "|";
                linkSub += cmbProvince.Items[xx].Value;
            }
        }
        if (subClause != "") whereClause += " and (" + subClause + ") ";
        if (linkSub != "") linkParam += "&proID=" + linkSub;

        subClause = "";
        linkSub = "";

        for (int xx = 0; xx < cmbMassGroup.Items.Count; xx++)
        {
            if (cmbMassGroup.Items[xx].Selected)
            {
                if (subClause != "") subClause += " or ";
                subClause = subClause + " tb_detailgroup.fl_group_id = '" + cmbMassGroup.Items[xx].Value + "' ";

                if (linkSub != "") linkSub += "|";
                linkSub += cmbMassGroup.Items[xx].Value;
            }
        }
        if (subClause != "") whereClause += " and (" + subClause + ") ";
        if (linkSub != "") linkParam += "&massID=" + linkSub;

        //Filter Right
        //if (Session["uGroup"].ToString() == "U") whereClause += " and isnull(tb_detailgroup.fl_dept,'') like '" + Session["uDept"].ToString().Replace("0", "") + "%' ";
        if (Session["uGroup"].ToString() == "U")
        {
            if (Session["uDept"].ToString().Substring(0) != "0") whereClause += " and isnull(tb_detailgroup.fl_dept,'') like '" + Session["uDept"].ToString().Replace("00", "") + "%' ";
        }
        //Report WhereClause
        if (subType == 1) whereClause += " and tb_maingroup.fl_group_type <='3' ";
        if (subType == 2) whereClause += " and tb_maingroup.fl_group_type ='4' ";
        if (subType == 3) whereClause += " and tb_maingroup.fl_group_type ='5' ";

        Session["whereClause"] = whereClause;

        string orderClause = " order by main.fl_province_name, ";
        orderClause += " main.fl_group_type ";
        #endregion

        dr["param1"] = cmbReport.Items[cmbReport.SelectedIndex].Text;
        dr["param5"] = lblRepTitle.Text;

        WebRequest http = WebRequest.Create(ConfigurationManager.AppSettings["webURL"].ToString() + "G1.aspx" + linkParam + "&skip=1");
        http.Credentials = CredentialCache.DefaultCredentials;
        WebResponse response = http.GetResponse();
        Stream stream = response.GetResponseStream();
        int bytes = 0;
        Byte[] bytesReceived = new Byte[256];
        System.IO.FileStream fs = new System.IO.FileStream(Server.MapPath("TMP") + "//" + Session["uID"].ToString() + "G1.png", System.IO.FileMode.Create);
        do
        {
            bytes = stream.Read(bytesReceived, 0, 256);
            fs.Write(bytesReceived, 0, bytes);
        }
        while (bytes > 0);
        stream.Close();
        fs.Close();

        dr["param2"] = "file://" + Server.MapPath("TMP").Replace("/","//") + "//" + Session["uID"].ToString() + "G1.png";

        http = WebRequest.Create(ConfigurationManager.AppSettings["webURL"].ToString() + "G2.aspx" + linkParam + "&skip=1");
        http.Credentials = CredentialCache.DefaultCredentials;
        response = http.GetResponse();
        stream = response.GetResponseStream();
        bytes = 0;
        bytesReceived = new Byte[256];
        fs = new System.IO.FileStream(Server.MapPath("TMP") + "//" + Session["uID"].ToString() + "G2.png", System.IO.FileMode.Create);
        do
        {
            bytes = stream.Read(bytesReceived, 0, 256);
            fs.Write(bytesReceived, 0, bytes);
        }
        while (bytes > 0);
        stream.Close();
        fs.Close();
        dr["param3"] = "file://" + Server.MapPath("TMP").Replace("/", "//") + "//" + Session["uID"].ToString() + "G3.png";

        http = WebRequest.Create(ConfigurationManager.AppSettings["webURL"].ToString() + "G3.aspx" + linkParam + "&skip=1");
        http.Credentials = CredentialCache.DefaultCredentials;
        response = http.GetResponse();
        stream = response.GetResponseStream();
        bytes = 0;
        bytesReceived = new Byte[256];
        fs = new System.IO.FileStream(Server.MapPath("TMP") + "//" + Session["uID"].ToString() + "G3.png", System.IO.FileMode.Create);
        do
        {
            bytes = stream.Read(bytesReceived, 0, 256);
            fs.Write(bytesReceived, 0, bytes);
        }
        while (bytes > 0);
        stream.Close();
        fs.Close();
        dr["param4"] = "file://" + Server.MapPath("TMP").Replace("/", "//") + "//" + Session["uID"].ToString() + "G3.png";

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

        sql += " (SELECT distinct ";
        sql += "    fl_group_type, ";
        sql += "    fl_province, ";
        sql += "    fl_province_name, ";
        sql += "    isnull(count(fl_id),0) as idCount, ";
        sql += "    isnull(sum(cast(isnull(fl_reportMember,'0') as bigint)),0) as reportCount ";
        sql += " from tb_detailgroup ";
        sql += " inner join tb_maingroup  on tb_detailgroup.fl_group_id = tb_maingroup.fl_group_id ";
        sql += " inner join (select distinct fl_province_code,fl_province_name from tb_moicode) moiCode  on tb_detailgroup.fl_province = moiCode.fl_province_code ";
        sql += whereClause;
        sql += " group by fl_group_type,fl_province_name,fl_province ";

        sql += " UNION ";

        sql += " SELECT '9' as fl_group_type, ";
        sql += "    fl_province, ";
        sql += "    fl_province_name, ";
        sql += "    isnull(count(fl_id),0) as idCount, ";
        sql += "    isnull(sum(cast(isnull(fl_reportMember,'0') as bigint)),0) as reportCount ";
        sql += " from tb_detailgroup ";
        sql += " inner join tb_maingroup  on tb_detailgroup.fl_group_id = tb_maingroup.fl_group_id ";
        sql += " inner join (select distinct fl_province_code,fl_province_name from tb_moicode) moiCode  on tb_detailgroup.fl_province = moiCode.fl_province_code ";
        sql += whereClause;
        sql += " group by fl_province_name,fl_province ";

        sql += " UNION ";

        sql += " SELECT fl_group_type, ";
        sql += "    '99' as fl_province, ";
        sql += "    'ฮฮฮฮฮฮ' as fl_province_name, ";
        sql += "    isnull(count(fl_id),0) as idCount, ";
        sql += "    isnull(sum(cast(isnull(fl_reportMember,'0') as bigint)),0) as reportCount ";
        sql += " from tb_detailgroup ";
        sql += " inner join tb_maingroup  on tb_detailgroup.fl_group_id = tb_maingroup.fl_group_id ";
        sql += " inner join (select distinct fl_province_code,fl_province_name from tb_moicode) moiCode  on tb_detailgroup.fl_province = moiCode.fl_province_code ";
        sql += whereClause;
        sql += " group by fl_group_type ";

        sql += " UNION ";

        sql += " SELECT '9' as fl_group_type, ";
        sql += "    '99' as fl_province, ";
        sql += "    'ฮฮฮฮฮฮ' as fl_province_name, ";
        sql += "    isnull(count(fl_id),0) as idCount, ";
        sql += "    isnull(sum(cast(isnull(fl_reportMember,'0') as bigint)),0) as reportCount ";
        sql += " from tb_detailgroup ";
        sql += " inner join tb_maingroup  on tb_detailgroup.fl_group_id = tb_maingroup.fl_group_id ";
        sql += " inner join (select distinct fl_province_code,fl_province_name from tb_moicode) moiCode  on tb_detailgroup.fl_province = moiCode.fl_province_code ";
        sql += whereClause;
        sql += " ) main ";

        sql += " left outer join ";

        sql += " (select distinct ";
        sql += " fl_group_type, ";
        sql += " fl_province, ";
        sql += " isnull(count(tb_membergroup.fl_citizen_id),0) as headCount ";
        sql += " from tb_detailgroup ";
        sql += " inner join tb_maingroup on tb_detailgroup.fl_group_id = tb_maingroup.fl_group_id ";
        sql += " inner join tb_membergroup on tb_detailgroup.fl_id = tb_membergroup.fl_detailgroup_id ";
        sql += " inner join tb_citizen on tb_membergroup.fl_citizen_id = tb_citizen.fl_citizen_id ";
        sql += whereClause;
        sql += " and (fl_stop='' or fl_stop is null) ";
        sql += " and tb_citizen.fl_status='1' ";
        sql += " group by fl_group_type,fl_province ";

        sql += " UNION ";

        sql += " SELECT '9' as fl_group_type, ";
        sql += " fl_province, ";
        sql += " isnull(count(tb_membergroup.fl_citizen_id),0) as headCount ";
        sql += " from tb_detailgroup ";
        sql += " inner join tb_maingroup  on tb_detailgroup.fl_group_id = tb_maingroup.fl_group_id ";
        sql += " inner join tb_membergroup on tb_detailgroup.fl_id = tb_membergroup.fl_detailgroup_id ";
        sql += " inner join tb_citizen on tb_membergroup.fl_citizen_id = tb_citizen.fl_citizen_id ";
        sql += whereClause;
        sql += " and (fl_stop='' or fl_stop is null) ";
        sql += " and tb_citizen.fl_status='1' ";
        sql += " group by fl_province ";

        sql += " UNION ";

        sql += " SELECT fl_group_type, ";
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

        sql += " UNION ";

        sql += " SELECT '9' as fl_group_type, ";
        sql += " '99' as fl_province, ";
        sql += " isnull(count(tb_membergroup.fl_citizen_id),0) as headCount ";
        sql += " from tb_detailgroup ";
        sql += " inner join tb_maingroup  on tb_detailgroup.fl_group_id = tb_maingroup.fl_group_id ";
        sql += " inner join tb_membergroup on tb_detailgroup.fl_id = tb_membergroup.fl_detailgroup_id ";
        sql += " inner join tb_citizen on tb_membergroup.fl_citizen_id = tb_citizen.fl_citizen_id ";
        sql += whereClause;
        sql += " and (fl_stop='' or fl_stop is null) ";
        sql += " and tb_citizen.fl_status='1' ";
        sql += " ) c1 ";

        sql += " on main.fl_group_type=c1.fl_group_type ";
        sql += " and main.fl_province=c1.fl_province ";

        sql += " left outer join ";

        sql += " (select distinct ";
        sql += " fl_group_type, ";
        sql += " fl_province, ";
        sql += " isnull(count(tb_membergroup.fl_citizen_id),0) as headCount2 ";
        sql += " from tb_detailgroup ";
        sql += " inner join tb_maingroup on tb_detailgroup.fl_group_id = tb_maingroup.fl_group_id ";
        sql += " inner join tb_membergroup on tb_detailgroup.fl_id = tb_membergroup.fl_detailgroup_id ";
        sql += " inner join tb_citizen on tb_membergroup.fl_citizen_id = tb_citizen.fl_citizen_id ";
        sql += whereClause;
        sql += " and (fl_stop='' or fl_stop is null) ";
        sql += " and tb_citizen.fl_status='1' ";
        sql += " and tb_citizen.fl_targetFlag='1' ";
        sql += " group by fl_group_type,fl_province ";

        sql += " UNION ";

        sql += " SELECT '9' as fl_group_type, ";
        sql += " fl_province, ";
        sql += " isnull(count(tb_membergroup.fl_citizen_id),0) as headCount2 ";
        sql += " from tb_detailgroup ";
        sql += " inner join tb_maingroup  on tb_detailgroup.fl_group_id = tb_maingroup.fl_group_id ";
        sql += " inner join tb_membergroup on tb_detailgroup.fl_id = tb_membergroup.fl_detailgroup_id ";
        sql += " inner join tb_citizen on tb_membergroup.fl_citizen_id = tb_citizen.fl_citizen_id ";
        sql += whereClause;
        sql += " and (fl_stop='' or fl_stop is null) ";
        sql += " and tb_citizen.fl_status='1' ";
        sql += " and tb_citizen.fl_targetFlag='1' ";
        sql += " group by fl_province ";

        sql += " UNION ";

        sql += " SELECT fl_group_type, ";
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

        sql += " UNION ";

        sql += " SELECT '9' as fl_group_type, ";
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
        sql += " ) c2 ";
        sql += " on main.fl_group_type=c2.fl_group_type ";
        sql += " and main.fl_province=c2.fl_province ";

        //sql += " group by main.fl_group_type,main.fl_province_name,main.fl_province ";

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
        string tmpVal = "";
        while (rs.Read())
        {
            if (tmpVal != rs.GetString(1))
            {
                if (tmpVal != "")
                {
                    dt.Rows.Add(dr);
                    dr = dt.NewRow();
                }

                maxCols = 17;
                if (subType != 1) maxCols = 9;
                for (int xx = 1; xx <= maxCols; xx++) dr["name" + xx.ToString()] = "";

                tmpVal = rs.GetString(1);

                string tmpVal2 = tmpVal;
                if (tmpVal2 == "ฮฮฮฮฮฮ") tmpVal2 = "รวม";
                dr["name1"] = tmpVal2;

                i++;
            }
            int j = 0;
            if (subType == 1)
            {
                if (rs.GetString(0) == "1") j = 1;
                if (rs.GetString(0) == "2") j = 5;
                if (rs.GetString(0) == "3") j = 9;
                if (rs.GetString(0) == "9") j = 13;
            }
            else
            {
                if (rs.GetString(0) == "9") j = 5; else j = 1;
            }

            for (int xx = 0; xx < 4; xx++) dr["name" + (j + xx + 1).ToString()] = Convert.ToUInt64(rs.GetValue(xx + 3)).ToString("#,##0");                    
        }
        rs.Close();
        Conn.Close();

        //Add last row
        if(i>1) dt.Rows.Add(dr);

        LocalReport rpt = new LocalReport();
        string fName = cmbReport.Items[cmbReport.SelectedIndex].Text + ".pdf";
        rpt.ReportPath = "Memo1.rdlc";
        rpt.DataSources.Clear();
        rpt.DataSources.Add(new ReportDataSource("DataSet1_DataTable1", dt));
        rpt.Refresh();
        rpt.EnableExternalImages = true;

        string reportType = "PDF";
        string mimeType;
        string encoding;
        string fileNameExtension;

        string deviceInfo =
                "<DeviceInfo>" +
                "  <OutputFormat>PDF</OutputFormat>" +
                "  <PageWidth>11.5in</PageWidth>" +
                "  <PageHeight>8.5in</PageHeight>" +
                "  <MarginTop>0.1in</MarginTop>" +
                "  <MarginLeft>0.1in</MarginLeft>" +
                "  <MarginRight>0.1in</MarginRight>" +
                "  <MarginBottom>0.1in</MarginBottom>" +
                "</DeviceInfo>";

        Warning[] warnings;
        string[] streams;
        byte[] renderedBytes;

        //Render the report

        renderedBytes = rpt.Render(
            reportType,
            deviceInfo,
            out mimeType,
            out encoding,
            out fileNameExtension,
            out streams,
            out warnings);

        Response.Clear();
        Response.ContentType = mimeType;
        Response.AddHeader("content-disposition", "attachment; filename=" + fName);
        Response.BinaryWrite(renderedBytes);
        Response.End();
    }

    protected void makePDF2(int subType)
    {
        #region header
        DataTable dt = new DataTable();

        dt.Columns.Add("param1");
        dt.Columns.Add("param2");
        dt.Columns.Add("param3");
        dt.Columns.Add("param4");
        dt.Columns.Add("param5");

        dt.Columns.Add("name1");
        dt.Columns.Add("name2");
        dt.Columns.Add("name3");
        dt.Columns.Add("name4");
        dt.Columns.Add("name5");
        dt.Columns.Add("name6");
        dt.Columns.Add("name7");
        dt.Columns.Add("name8");
        dt.Columns.Add("name9");
        dt.Columns.Add("name10");
        dt.Columns.Add("name11");
        dt.Columns.Add("name12");
        dt.Columns.Add("name13");
        dt.Columns.Add("name14");
        dt.Columns.Add("name15");
        dt.Columns.Add("name16");
        dt.Columns.Add("name17");

        dt.Columns.Add("no1");
        dt.Columns.Add("no2");
        dt.Columns.Add("no3");
        dt.Columns.Add("no4");

        dt.Columns.Add("baht1");
        dt.Columns.Add("baht2");
        dt.Columns.Add("baht3");
        dt.Columns.Add("baht4");
        dt.Columns.Add("baht5");
        dt.Columns.Add("baht6");
        dt.Columns.Add("baht7");
        dt.Columns.Add("baht8");
        dt.Columns.Add("baht9");
        dt.Columns.Add("baht10");
        dt.Columns.Add("baht11");
        dt.Columns.Add("baht12");
        dt.Columns.Add("baht13");
        dt.Columns.Add("baht14");
        dt.Columns.Add("baht15");
        dt.Columns.Add("baht16");
        dt.Columns.Add("baht17");

        DataRow dr = dt.NewRow();
        dr["baht1"] = "หน่วยงาน";

        if (subType == 1)
        {
            dr["no1"] = "มวลชนกอรมน.";
            dr["no2"] = "มวลชนภาครัฐ";
            dr["no3"] = "มวลชนภาคประชาชน";
            dr["no4"] = "รวม";
        }
        else
        {
            if (subType == 2) dr["no1"] = "มวลชนสานประโยชน์";
            if (subType == 3) dr["no1"] = "มวลชนอุปสรรค";
            dr["no2"] = "รวม";
        }
        int maxCols = 3;
        if (subType != 1) maxCols = 1;
        for (int mcols = 0; mcols <= maxCols; mcols++)
        {
            for (int scols = 0; scols <= 3; scols++)
            {
                string celltxt = "";
                if (scols == 0) celltxt = "กลุ่ม";
                if (scols == 1) celltxt = "รายงาน";
                if (scols == 2) celltxt = "รายชื่อ";
                if (scols == 3) celltxt = "ปฎิบัติการ";
                int index = mcols * 4 + scols + 2;
                dr["baht" + index.ToString()] = celltxt;
            }
        }
        #endregion

        #region whereCompose
        string whereClause = "where 1=1 ";
        string subClause = "";
        string linkSub = "";
        string linkParam = "?repID=" + cmbReport.SelectedValue;

        for (int xx = 0; xx < cmbDept.Items.Count; xx++)
        {
            if (cmbDept.Items[xx].Selected)
            {
                if (subClause != "") subClause += " or ";
                subClause = subClause + " isnull(tb_detailgroup.fl_dept,'') like '" + cmbDept.Items[xx].Value.Replace("0", "") + "%' ";

                if (linkSub != "") linkSub += "|";
                linkSub += cmbDept.Items[xx].Value;
            }
        }
        if (subClause != "") whereClause += " and (" + subClause + ") ";
        if (linkSub != "") linkParam += "&deptID=" + linkSub;

        subClause = "";
        linkSub = "";

        for (int xx = 0; xx < cmbProvince.Items.Count; xx++)
        {
            if (cmbProvince.Items[xx].Selected)
            {
                if (subClause != "") subClause += " or ";
                subClause = subClause + " tb_detailgroup.fl_province = '" + cmbProvince.Items[xx].Value + "' ";

                if (linkSub != "") linkSub += "|";
                linkSub += cmbProvince.Items[xx].Value;
            }
        }
        if (subClause != "") whereClause += " and (" + subClause + ") ";
        if (linkSub != "") linkParam += "&proID=" + linkSub;

        subClause = "";
        linkSub = "";

        for (int xx = 0; xx < cmbMassGroup.Items.Count; xx++)
        {
            if (cmbMassGroup.Items[xx].Selected)
            {
                if (subClause != "") subClause += " or ";
                subClause = subClause + " tb_detailgroup.fl_group_id = '" + cmbMassGroup.Items[xx].Value + "' ";

                if (linkSub != "") linkSub += "|";
                linkSub += cmbMassGroup.Items[xx].Value;
            }
        }
        if (subClause != "") whereClause += " and (" + subClause + ") ";
        if (linkSub != "") linkParam += "&massID=" + linkSub;

        //Filter Right
        //if (Session["uGroup"].ToString() == "U") whereClause += " and isnull(tb_detailgroup.fl_dept,'') like '" + Session["uDept"].ToString().Replace("0", "") + "%' ";
        if (Session["uGroup"].ToString() == "U")
        {
            if (Session["uDept"].ToString().Substring(0) != "0") whereClause += " and isnull(tb_detailgroup.fl_dept,'') like '" + Session["uDept"].ToString().Replace("00", "") + "%' ";
        }
        //Report WhereClause
        if (subType == 1) whereClause += " and tb_maingroup.fl_group_type <='3' ";
        if (subType == 2) whereClause += " and tb_maingroup.fl_group_type ='4' ";
        if (subType == 3) whereClause += " and tb_maingroup.fl_group_type ='5' ";

        Session["whereClause"] = whereClause;

        string orderClause = " order by main.fl_dept, ";
        orderClause += " main.fl_group_type ";
        #endregion

        dr["param1"] = cmbReport.Items[cmbReport.SelectedIndex].Text;
        dr["param5"] = lblRepTitle.Text;

        WebRequest http = WebRequest.Create(ConfigurationManager.AppSettings["webURL"].ToString() + "G1.aspx" + linkParam + "&skip=1");
        http.Credentials = CredentialCache.DefaultCredentials;
        WebResponse response = http.GetResponse();
        Stream stream = response.GetResponseStream();
        int bytes = 0;
        Byte[] bytesReceived = new Byte[256];
        System.IO.FileStream fs = new System.IO.FileStream(Server.MapPath("TMP") + "//" + Session["uID"].ToString() + "G1.png", System.IO.FileMode.Create);
        do
        {
            bytes = stream.Read(bytesReceived, 0, 256);
            fs.Write(bytesReceived, 0, bytes);
        }
        while (bytes > 0);
        stream.Close();
        fs.Close();

        dr["param2"] = "file://" + Server.MapPath("TMP").Replace("/", "//") + "//" + Session["uID"].ToString() + "G1.png";

        http = WebRequest.Create(ConfigurationManager.AppSettings["webURL"].ToString() + "G2.aspx" + linkParam + "&skip=1");
        http.Credentials = CredentialCache.DefaultCredentials;
        response = http.GetResponse();
        stream = response.GetResponseStream();
        bytes = 0;
        bytesReceived = new Byte[256];
        fs = new System.IO.FileStream(Server.MapPath("TMP") + "//" + Session["uID"].ToString() + "G2.png", System.IO.FileMode.Create);
        do
        {
            bytes = stream.Read(bytesReceived, 0, 256);
            fs.Write(bytesReceived, 0, bytes);
        }
        while (bytes > 0);
        stream.Close();
        fs.Close();
        dr["param3"] = "file://" + Server.MapPath("TMP").Replace("/", "//") + "//" + Session["uID"].ToString() + "G3.png";

        http = WebRequest.Create(ConfigurationManager.AppSettings["webURL"].ToString() + "G3.aspx" + linkParam + "&skip=1");
        http.Credentials = CredentialCache.DefaultCredentials;
        response = http.GetResponse();
        stream = response.GetResponseStream();
        bytes = 0;
        bytesReceived = new Byte[256];
        fs = new System.IO.FileStream(Server.MapPath("TMP") + "//" + Session["uID"].ToString() + "G3.png", System.IO.FileMode.Create);
        do
        {
            bytes = stream.Read(bytesReceived, 0, 256);
            fs.Write(bytesReceived, 0, bytes);
        }
        while (bytes > 0);
        stream.Close();
        fs.Close();
        dr["param4"] = "file://" + Server.MapPath("TMP").Replace("/", "//") + "//" + Session["uID"].ToString() + "G3.png";

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

        sql += " (SELECT distinct ";
        sql += "    fl_group_type, ";
        sql += "    fl_dept, ";
        sql += "    fl_dept_name, ";
        sql += "    isnull(count(fl_id),0) as idCount, ";
        sql += "    isnull(sum(cast(isnull(fl_reportMember,'0') as bigint)),0) as reportCount ";
        sql += " from tb_detailgroup ";
        sql += " inner join tb_maingroup  on tb_detailgroup.fl_group_id = tb_maingroup.fl_group_id ";
        sql += " inner join (select distinct fl_dept_id,fl_dept_name from tb_dept) moiCode  on tb_detailgroup.fl_dept = moiCode.fl_dept_id ";
        sql += whereClause;
        sql += " group by fl_group_type,fl_dept_name,fl_dept ";

        sql += " UNION ";

        sql += " SELECT '9' as fl_group_type, ";
        sql += "    fl_dept, ";
        sql += "    fl_dept_name, ";
        sql += "    isnull(count(fl_id),0) as idCount, ";
        sql += "    isnull(sum(cast(isnull(fl_reportMember,'0') as bigint)),0) as reportCount ";
        sql += " from tb_detailgroup ";
        sql += " inner join tb_maingroup  on tb_detailgroup.fl_group_id = tb_maingroup.fl_group_id ";
        sql += " inner join (select distinct fl_dept_id,fl_dept_name from tb_dept) moiCode  on tb_detailgroup.fl_dept = moiCode.fl_dept_id ";
        sql += whereClause;
        sql += " group by fl_dept_name,fl_dept ";

        sql += " UNION ";

        sql += " SELECT fl_group_type, ";
        sql += "    '99' as fl_dept, ";
        sql += "    'ฮฮฮฮฮฮ' as fl_dept_name, ";
        sql += "    isnull(count(fl_id),0) as idCount, ";
        sql += "    isnull(sum(cast(isnull(fl_reportMember,'0') as bigint)),0) as reportCount ";
        sql += " from tb_detailgroup ";
        sql += " inner join tb_maingroup  on tb_detailgroup.fl_group_id = tb_maingroup.fl_group_id ";
        sql += " inner join (select distinct fl_dept_id,fl_dept_name from tb_dept) moiCode  on tb_detailgroup.fl_dept = moiCode.fl_dept_id ";
        sql += whereClause;
        sql += " group by fl_group_type ";

        sql += " UNION ";

        sql += " SELECT '9' as fl_group_type, ";
        sql += "    '99' as fl_dept, ";
        sql += "    'ฮฮฮฮฮฮ' as fl_dept_name, ";
        sql += "    isnull(count(fl_id),0) as idCount, ";
        sql += "    isnull(sum(cast(isnull(fl_reportMember,'0') as bigint)),0) as reportCount ";
        sql += " from tb_detailgroup ";
        sql += " inner join tb_maingroup  on tb_detailgroup.fl_group_id = tb_maingroup.fl_group_id ";
        sql += " inner join (select distinct fl_dept_id,fl_dept_name from tb_dept) moiCode  on tb_detailgroup.fl_dept = moiCode.fl_dept_id ";
        sql += whereClause;
        sql += " ) main ";

        sql += " left outer join ";

        sql += " (select distinct ";
        sql += " fl_group_type, ";
        sql += " fl_dept, ";
        sql += " isnull(count(tb_membergroup.fl_citizen_id),0) as headCount ";
        sql += " from tb_detailgroup ";
        sql += " inner join tb_maingroup on tb_detailgroup.fl_group_id = tb_maingroup.fl_group_id ";
        sql += " inner join tb_membergroup on tb_detailgroup.fl_id = tb_membergroup.fl_detailgroup_id ";
        sql += " inner join tb_citizen on tb_membergroup.fl_citizen_id = tb_citizen.fl_citizen_id ";
        sql += whereClause;
        sql += " and (fl_stop='' or fl_stop is null) ";
        sql += " and tb_citizen.fl_status='1' ";
        sql += " group by fl_group_type,fl_dept ";

        sql += " UNION ";

        sql += " SELECT '9' as fl_group_type, ";
        sql += " fl_dept, ";
        sql += " isnull(count(tb_membergroup.fl_citizen_id),0) as headCount ";
        sql += " from tb_detailgroup ";
        sql += " inner join tb_maingroup  on tb_detailgroup.fl_group_id = tb_maingroup.fl_group_id ";
        sql += " inner join tb_membergroup on tb_detailgroup.fl_id = tb_membergroup.fl_detailgroup_id ";
        sql += " inner join tb_citizen on tb_membergroup.fl_citizen_id = tb_citizen.fl_citizen_id ";
        sql += whereClause;
        sql += " and (fl_stop='' or fl_stop is null) ";
        sql += " and tb_citizen.fl_status='1' ";
        sql += " group by fl_dept ";

        sql += " UNION ";

        sql += " SELECT fl_group_type, ";
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

        sql += " UNION ";

        sql += " SELECT '9' as fl_group_type, ";
        sql += " '99' as fl_dept, ";
        sql += " isnull(count(tb_membergroup.fl_citizen_id),0) as headCount ";
        sql += " from tb_detailgroup ";
        sql += " inner join tb_maingroup  on tb_detailgroup.fl_group_id = tb_maingroup.fl_group_id ";
        sql += " inner join tb_membergroup on tb_detailgroup.fl_id = tb_membergroup.fl_detailgroup_id ";
        sql += " inner join tb_citizen on tb_membergroup.fl_citizen_id = tb_citizen.fl_citizen_id ";
        sql += whereClause;
        sql += " and (fl_stop='' or fl_stop is null) ";
        sql += " and tb_citizen.fl_status='1' ";
        sql += " ) c1 ";

        sql += " on main.fl_group_type=c1.fl_group_type ";
        sql += " and main.fl_dept=c1.fl_dept ";

        sql += " left outer join ";

        sql += " (select distinct ";
        sql += " fl_group_type, ";
        sql += " fl_dept, ";
        sql += " isnull(count(tb_membergroup.fl_citizen_id),0) as headCount2 ";
        sql += " from tb_detailgroup ";
        sql += " inner join tb_maingroup on tb_detailgroup.fl_group_id = tb_maingroup.fl_group_id ";
        sql += " inner join tb_membergroup on tb_detailgroup.fl_id = tb_membergroup.fl_detailgroup_id ";
        sql += " inner join tb_citizen on tb_membergroup.fl_citizen_id = tb_citizen.fl_citizen_id ";
        sql += whereClause;
        sql += " and (fl_stop='' or fl_stop is null) ";
        sql += " and tb_citizen.fl_status='1' ";
        sql += " and tb_citizen.fl_targetFlag='1' ";
        sql += " group by fl_group_type,fl_dept ";

        sql += " UNION ";

        sql += " SELECT '9' as fl_group_type, ";
        sql += " fl_dept, ";
        sql += " isnull(count(tb_membergroup.fl_citizen_id),0) as headCount2 ";
        sql += " from tb_detailgroup ";
        sql += " inner join tb_maingroup  on tb_detailgroup.fl_group_id = tb_maingroup.fl_group_id ";
        sql += " inner join tb_membergroup on tb_detailgroup.fl_id = tb_membergroup.fl_detailgroup_id ";
        sql += " inner join tb_citizen on tb_membergroup.fl_citizen_id = tb_citizen.fl_citizen_id ";
        sql += whereClause;
        sql += " and (fl_stop='' or fl_stop is null) ";
        sql += " and tb_citizen.fl_status='1' ";
        sql += " and tb_citizen.fl_targetFlag='1' ";
        sql += " group by fl_dept ";

        sql += " UNION ";

        sql += " SELECT fl_group_type, ";
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

        sql += " UNION ";

        sql += " SELECT '9' as fl_group_type, ";
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
        sql += " ) c2 ";
        sql += " on main.fl_group_type=c2.fl_group_type ";
        sql += " and main.fl_dept=c2.fl_dept ";

        //sql += " group by main.fl_group_type,main.fl_dept,main.fl_dept_name ";

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
        string tmpVal = "";
        while (rs.Read())
        {
            if (tmpVal != rs.GetString(1))
            {
                if (tmpVal != "")
                {
                    dt.Rows.Add(dr);
                    dr = dt.NewRow();
                }

                maxCols = 17;
                if (subType != 1) maxCols = 9;
                for (int xx = 1; xx <= maxCols; xx++) dr["name" + xx.ToString()] = "";

                tmpVal = rs.GetString(1);

                string tmpVal2 = tmpVal;
                if (tmpVal2 == "ฮฮฮฮฮฮ") tmpVal2 = "รวม";
                dr["name1"] = tmpVal2;

                i++;
            }
            int j = 0;
            if (subType == 1)
            {
                if (rs.GetString(0) == "1") j = 1;
                if (rs.GetString(0) == "2") j = 5;
                if (rs.GetString(0) == "3") j = 9;
                if (rs.GetString(0) == "9") j = 13;
            }
            else
            {
                if (rs.GetString(0) == "9") j = 5; else j = 1;
            }

            for (int xx = 0; xx < 4; xx++) dr["name" + (j + xx + 1).ToString()] = Convert.ToUInt64(rs.GetValue(xx + 3)).ToString("#,##0");
        }
        rs.Close();
        Conn.Close();

        //Add last row
        if(i>1) dt.Rows.Add(dr);

        LocalReport rpt = new LocalReport();
        string fName = cmbReport.Items[cmbReport.SelectedIndex].Text + ".pdf";
        rpt.ReportPath = "Memo1.rdlc";
        rpt.DataSources.Clear();
        rpt.DataSources.Add(new ReportDataSource("DataSet1_DataTable1", dt));
        rpt.Refresh();
        rpt.EnableExternalImages = true;

        string reportType = "PDF";
        string mimeType;
        string encoding;
        string fileNameExtension;

        string deviceInfo =
                "<DeviceInfo>" +
                "  <OutputFormat>PDF</OutputFormat>" +
                "  <PageWidth>11.5in</PageWidth>" +
                "  <PageHeight>8.5in</PageHeight>" +
                "  <MarginTop>0.1in</MarginTop>" +
                "  <MarginLeft>0.1in</MarginLeft>" +
                "  <MarginRight>0.1in</MarginRight>" +
                "  <MarginBottom>0.1in</MarginBottom>" +
                "</DeviceInfo>";

        Warning[] warnings;
        string[] streams;
        byte[] renderedBytes;

        //Render the report

        renderedBytes = rpt.Render(
            reportType,
            deviceInfo,
            out mimeType,
            out encoding,
            out fileNameExtension,
            out streams,
            out warnings);

        Response.Clear();
        Response.ContentType = mimeType;
        Response.AddHeader("content-disposition", "attachment; filename=" + fName);
        Response.BinaryWrite(renderedBytes);
        Response.End();
    }

    protected void makePDF3()
    {
        #region header
        DataTable dt = new DataTable();

        dt.Columns.Add("param1");
        dt.Columns.Add("param2");
        dt.Columns.Add("param3");
        dt.Columns.Add("param4");
        dt.Columns.Add("param5");

        dt.Columns.Add("name1");
        dt.Columns.Add("name2");
        dt.Columns.Add("name3");
        dt.Columns.Add("name4");
        dt.Columns.Add("name5");
        dt.Columns.Add("name6");
        dt.Columns.Add("name7");
        dt.Columns.Add("name8");
        dt.Columns.Add("name9");
        dt.Columns.Add("name10");
        dt.Columns.Add("name11");
        dt.Columns.Add("name12");
        dt.Columns.Add("name13");
        dt.Columns.Add("name14");
        dt.Columns.Add("name15");
        dt.Columns.Add("name16");
        dt.Columns.Add("name17");

        dt.Columns.Add("no1");
        dt.Columns.Add("no2");
        dt.Columns.Add("no3");
        dt.Columns.Add("no4");

        dt.Columns.Add("baht1");
        dt.Columns.Add("baht2");
        dt.Columns.Add("baht3");
        dt.Columns.Add("baht4");
        dt.Columns.Add("baht5");
        dt.Columns.Add("baht6");
        dt.Columns.Add("baht7");
        dt.Columns.Add("baht8");
        dt.Columns.Add("baht9");
        dt.Columns.Add("baht10");
        dt.Columns.Add("baht11");
        dt.Columns.Add("baht12");
        dt.Columns.Add("baht13");
        dt.Columns.Add("baht14");
        dt.Columns.Add("baht15");
        dt.Columns.Add("baht16");
        dt.Columns.Add("baht17");

        DataRow dr = dt.NewRow();
        dr["baht1"] = "ปี";

        dr["no1"] = "มวลชนกอรมน.";
        dr["no2"] = "มวลชนภาครัฐ";
        dr["no3"] = "มวลชนภาคประชาชน";
        dr["no4"] = "รวม";
        int maxCols = 3;
        for (int mcols = 0; mcols <= maxCols; mcols++)
        {
            for (int scols = 0; scols <= 3; scols++)
            {
                string celltxt = "";
                if (scols == 0) celltxt = "กลุ่ม";
                if (scols == 1) celltxt = "รายชื่อ";
                if (scols == 2) celltxt = "เติบโตกลุ่ม";
                if (scols == 3) celltxt = "เติบโตรายชื่อ";
                int index = mcols * 4 + scols + 2;
                dr["baht" + index.ToString()] = celltxt;
            }
        }
        #endregion

        #region whereCompose
        string whereClause = "where 1=1 ";
        string subClause = "";
        string linkSub = "";
        string linkParam = "?repID=" + cmbReport.SelectedValue;

        for (int xx = 0; xx < cmbDept.Items.Count; xx++)
        {
            if (cmbDept.Items[xx].Selected)
            {
                if (subClause != "") subClause += " or ";
                subClause = subClause + " isnull(tb_detailgroup.fl_dept,'') like '" + cmbDept.Items[xx].Value.Replace("0", "") + "%' ";

                if (linkSub != "") linkSub += "|";
                linkSub += cmbDept.Items[xx].Value;
            }
        }
        if (subClause != "") whereClause += " and (" + subClause + ") ";
        if (linkSub != "") linkParam += "&deptID=" + linkSub;

        subClause = "";
        linkSub = "";

        for (int xx = 0; xx < cmbProvince.Items.Count; xx++)
        {
            if (cmbProvince.Items[xx].Selected)
            {
                if (subClause != "") subClause += " or ";
                subClause = subClause + " tb_detailgroup.fl_province = '" + cmbProvince.Items[xx].Value + "' ";

                if (linkSub != "") linkSub += "|";
                linkSub += cmbProvince.Items[xx].Value;
            }
        }
        if (subClause != "") whereClause += " and (" + subClause + ") ";
        if (linkSub != "") linkParam += "&proID=" + linkSub;

        subClause = "";
        linkSub = "";

        for (int xx = 0; xx < cmbMassGroup.Items.Count; xx++)
        {
            if (cmbMassGroup.Items[xx].Selected)
            {
                if (subClause != "") subClause += " or ";
                subClause = subClause + " tb_detailgroup.fl_group_id = '" + cmbMassGroup.Items[xx].Value + "' ";

                if (linkSub != "") linkSub += "|";
                linkSub += cmbMassGroup.Items[xx].Value;
            }
        }
        if (subClause != "") whereClause += " and (" + subClause + ") ";
        if (linkSub != "") linkParam += "&massID=" + linkSub;

        //Filter Right
        //if (Session["uGroup"].ToString() == "U") whereClause += " and isnull(tb_detailgroup.fl_dept,'') like '" + Session["uDept"].ToString().Replace("0", "") + "%' ";
        if (Session["uGroup"].ToString() == "U")
        {
            if (Session["uDept"].ToString().Substring(0) != "0") whereClause += " and isnull(tb_detailgroup.fl_dept,'') like '" + Session["uDept"].ToString().Replace("00", "") + "%' ";
        }
        //Report WhereClause
        whereClause += " and tb_maingroup.fl_group_type <='3' ";

        Session["whereClause"] = whereClause;

        string orderClause = " order by main.fl_yearID, ";
        orderClause += " main.fl_group_type ";
        #endregion

        dr["param1"] = cmbReport.Items[cmbReport.SelectedIndex].Text;
        dr["param5"] = lblRepTitle.Text;

        WebRequest http = WebRequest.Create(ConfigurationManager.AppSettings["webURL"].ToString() + "G1.aspx" + linkParam + "&skip=1");
        http.Credentials = CredentialCache.DefaultCredentials;
        WebResponse response = http.GetResponse();
        Stream stream = response.GetResponseStream();
        int bytes = 0;
        Byte[] bytesReceived = new Byte[256];
        System.IO.FileStream fs = new System.IO.FileStream(Server.MapPath("TMP") + "//" + Session["uID"].ToString() + "G1.png", System.IO.FileMode.Create);
        do
        {
            bytes = stream.Read(bytesReceived, 0, 256);
            fs.Write(bytesReceived, 0, bytes);
        }
        while (bytes > 0);
        stream.Close();
        fs.Close();

        dr["param2"] = "file://" + Server.MapPath("TMP").Replace("/", "//") + "//" + Session["uID"].ToString() + "G1.png";

        http = WebRequest.Create(ConfigurationManager.AppSettings["webURL"].ToString() + "G2.aspx" + linkParam + "&skip=1");
        http.Credentials = CredentialCache.DefaultCredentials;
        response = http.GetResponse();
        stream = response.GetResponseStream();
        bytes = 0;
        bytesReceived = new Byte[256];
        fs = new System.IO.FileStream(Server.MapPath("TMP") + "//" + Session["uID"].ToString() + "G2.png", System.IO.FileMode.Create);
        do
        {
            bytes = stream.Read(bytesReceived, 0, 256);
            fs.Write(bytesReceived, 0, bytes);
        }
        while (bytes > 0);
        stream.Close();
        fs.Close();
        dr["param3"] = "file://" + Server.MapPath("TMP").Replace("/", "//") + "//" + Session["uID"].ToString() + "G3.png";

        http = WebRequest.Create(ConfigurationManager.AppSettings["webURL"].ToString() + "G3.aspx" + linkParam + "&skip=1");
        http.Credentials = CredentialCache.DefaultCredentials;
        response = http.GetResponse();
        stream = response.GetResponseStream();
        bytes = 0;
        bytesReceived = new Byte[256];
        fs = new System.IO.FileStream(Server.MapPath("TMP") + "//" + Session["uID"].ToString() + "G3.png", System.IO.FileMode.Create);
        do
        {
            bytes = stream.Read(bytesReceived, 0, 256);
            fs.Write(bytesReceived, 0, bytes);
        }
        while (bytes > 0);
        stream.Close();
        fs.Close();
        dr["param4"] = "file://" + Server.MapPath("TMP").Replace("/", "//") + "//" + Session["uID"].ToString() + "G3.png";


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

        sql += " UNION ";

        sql += "SELECT distinct ";
        sql += "    '9' fl_group_type, ";
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

        sql += " UNION ";

        sql += " SELECT distinct ";
        sql += "    '9' fl_group_type, ";
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
        sql += " ) z ";

        sql += " UNION ";

        sql += " SELECT distinct ";
        sql += "    '9' fl_group_type, ";
        sql += "    '" + Convert.ToString(DateTime.Now.Year + 543) + "' as  fl_YearID, ";
        sql += " isnull(count(fl_id),0) as idCount, ";
        sql += " isnull(sum(cast(isnull(fl_reportMember,'0') as bigint)),0) as reportCount ";
        sql += " from (SELECT distinct fl_id,fl_reportMember,fl_group_type from tb_detailgroup ";
        sql += " inner join tb_maingroup  on tb_detailgroup.fl_group_id = tb_maingroup.fl_group_id ";
        sql += " inner join tb_membergroup on tb_detailgroup.fl_id = tb_membergroup.fl_detailgroup_id ";
        sql += whereClause;
        //At end of year
        sql += " and (isnull(fl_stop,'')='')  ";
        sql += " ) z";
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

        sql += " UNION ";

        sql += "SELECT distinct ";
        sql += "    '9' fl_group_type, ";
        sql += "    '" + Convert.ToString(DateTime.Now.Year + 543 - 2) + "' as  fl_YearID, ";
        sql += " isnull(count(tb_membergroup.fl_citizen_id),0) as headCount ";
        sql += " from tb_detailgroup ";
        sql += " inner join tb_maingroup  on tb_detailgroup.fl_group_id = tb_maingroup.fl_group_id ";
        sql += " inner join tb_membergroup on tb_detailgroup.fl_id = tb_membergroup.fl_detailgroup_id ";
        sql += whereClause;
        //At end of year
        sql += " and fl_start <= '" + Convert.ToString(DateTime.Now.Year - 1) + "' ";
        sql += " and (isnull(fl_stop,'')='' or isnull(fl_stop,'') > '" + Convert.ToString(DateTime.Now.Year - 1) + "') ";

        sql += " UNION ";

        sql += " SELECT distinct ";
        sql += "    '9' fl_group_type, ";
        sql += "    '" + Convert.ToString(DateTime.Now.Year + 543 - 1) + "' as  fl_YearID, ";
        sql += " isnull(count(tb_membergroup.fl_citizen_id),0) as headCount ";
        sql += " from tb_detailgroup ";
        sql += " inner join tb_maingroup  on tb_detailgroup.fl_group_id = tb_maingroup.fl_group_id ";
        sql += " inner join tb_membergroup on tb_detailgroup.fl_id = tb_membergroup.fl_detailgroup_id ";
        sql += whereClause;
        //At end of year
        sql += " and fl_start <= '" + Convert.ToString(DateTime.Now.Year) + "' ";
        sql += " and (isnull(fl_stop,'')='' or isnull(fl_stop,'') > '" + Convert.ToString(DateTime.Now.Year) + "') ";

        sql += " UNION ";

        sql += " SELECT distinct ";
        sql += "    '9' fl_group_type, ";
        sql += "    '" + Convert.ToString(DateTime.Now.Year + 543) + "' as  fl_YearID, ";
        sql += " isnull(count(tb_membergroup.fl_citizen_id),0) as headCount ";
        sql += " from tb_detailgroup ";
        sql += " inner join tb_maingroup  on tb_detailgroup.fl_group_id = tb_maingroup.fl_group_id ";
        sql += " inner join tb_membergroup on tb_detailgroup.fl_id = tb_membergroup.fl_detailgroup_id ";
        sql += whereClause;
        //At end of year
        sql += " and (isnull(fl_stop,'')='') ";
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

        int i = 1;
        string tmpVal = "";
        long tmpLY1 = 0;
        long tmpLY2 = 0;
        long tmpLY3 = 0;
        long tmpLY4 = 0;

        long tmpL1Y1 = 0;
        long tmpL1Y2 = 0;
        long tmpL1Y3 = 0;
        long tmpL1Y4 = 0;

        double amtPlot1 = 0;
        double amtPlot2 = 0;

        while (rs.Read())
        {
            if (tmpVal != rs.GetString(1))
            {
                if (tmpVal != "")
                {
                    dt.Rows.Add(dr);
                    dr = dt.NewRow();
                }
                maxCols = 17;
                for (int xx = 1; xx <= maxCols; xx++) dr["name" + xx.ToString()] = "";

                tmpVal = rs.GetString(1);

                string tmpVal2 = tmpVal;
                dr["name1"] = tmpVal2;

                i++;
            }
            if (rs.GetString(0) == "1")
            {
                if (tmpLY1 != 0) amtPlot1 = (Convert.ToInt64(rs.GetValue(2)) - tmpLY1) * 1.0 / tmpLY1 * 100;
                tmpLY1 = Convert.ToInt64(rs.GetValue(2));

                if (tmpL1Y1 != 0) amtPlot2 = (Convert.ToInt64(rs.GetValue(4)) - tmpL1Y1) * 1.0 / tmpL1Y1 * 100;
                tmpL1Y1 = Convert.ToInt64(rs.GetValue(4));
            }
            if (rs.GetString(0) == "2")
            {
                if (tmpLY2 != 0) amtPlot1 = (Convert.ToInt64(rs.GetValue(2)) - tmpLY2) * 1.0 / tmpLY2 * 100;
                tmpLY2 = Convert.ToInt64(rs.GetValue(2));

                if (tmpL1Y2 != 0) amtPlot2 = (Convert.ToInt64(rs.GetValue(4)) - tmpL1Y2) * 1.0 / tmpL1Y2 * 100;
                tmpL1Y2 = Convert.ToInt64(rs.GetValue(4));
            }
            if (rs.GetString(0) == "3")
            {
                if (tmpLY3 != 0) amtPlot1 = (Convert.ToInt64(rs.GetValue(2)) - tmpLY3) * 1.0 / tmpLY3 * 100;
                tmpLY3 = Convert.ToInt64(rs.GetValue(2));

                if (tmpL1Y3 != 0) amtPlot2 = (Convert.ToInt64(rs.GetValue(4)) - tmpL1Y3) * 1.0 / tmpL1Y3 * 100;
                tmpL1Y3 = Convert.ToInt64(rs.GetValue(4));
            }
            if (rs.GetString(0) == "9")
            {
                if (tmpLY4 != 0) amtPlot1 = (Convert.ToInt64(rs.GetValue(2)) - tmpLY4) * 1.0 / tmpLY4 * 100;
                tmpLY4 = Convert.ToInt64(rs.GetValue(2));

                if (tmpL1Y4 != 0) amtPlot2 = (Convert.ToInt64(rs.GetValue(4)) - tmpL1Y4) * 1.0 / tmpL1Y4 * 100;
                tmpL1Y4 = Convert.ToInt64(rs.GetValue(4));
            }

            int j = 0;
            if (rs.GetString(0) == "1") j = 1;
            if (rs.GetString(0) == "2") j = 5;
            if (rs.GetString(0) == "3") j = 9;
            if (rs.GetString(0) == "9") j = 13;

            dr["name" + (j + 1).ToString()] = Convert.ToInt64(rs.GetValue(2)).ToString("#,##0");
            dr["name" + (j + 2).ToString()] = Convert.ToInt64(rs.GetValue(4)).ToString("#,##0");
            dr["name" + (j + 3).ToString()] = amtPlot1.ToString("#,##0.00");
            dr["name" + (j + 4).ToString()] = amtPlot2.ToString("#,##0.00");
        }
        rs.Close();
        Conn.Close();

        //Add last row
        if(i>1) dt.Rows.Add(dr);

        LocalReport rpt = new LocalReport();
        string fName = cmbReport.Items[cmbReport.SelectedIndex].Text + ".pdf";
        rpt.ReportPath = "Memo1.rdlc";
        rpt.DataSources.Clear();
        rpt.DataSources.Add(new ReportDataSource("DataSet1_DataTable1", dt));
        rpt.Refresh();
        rpt.EnableExternalImages = true;

        string reportType = "PDF";
        string mimeType;
        string encoding;
        string fileNameExtension;

        string deviceInfo =
                "<DeviceInfo>" +
                "  <OutputFormat>PDF</OutputFormat>" +
                "  <PageWidth>11.5in</PageWidth>" +
                "  <PageHeight>8.5in</PageHeight>" +
                "  <MarginTop>0.1in</MarginTop>" +
                "  <MarginLeft>0.1in</MarginLeft>" +
                "  <MarginRight>0.1in</MarginRight>" +
                "  <MarginBottom>0.1in</MarginBottom>" +
                "</DeviceInfo>";

        Warning[] warnings;
        string[] streams;
        byte[] renderedBytes;

        //Render the report

        renderedBytes = rpt.Render(
            reportType,
            deviceInfo,
            out mimeType,
            out encoding,
            out fileNameExtension,
            out streams,
            out warnings);

        Response.Clear();
        Response.ContentType = mimeType;
        Response.AddHeader("content-disposition", "attachment; filename=" + fName);
        Response.BinaryWrite(renderedBytes);
        Response.End();
    }
}