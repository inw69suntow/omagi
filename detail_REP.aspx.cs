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

public partial class _detail_REP : System.Web.UI.Page
{
    protected void makeGroup()
    {
        if (tParam.Value == "") return;
        if((tParam.Value=="1") && (idParam.Value!="99")) return;
        
        cmbGroup.Items.Clear();
        cmbGroup.Items.Add(new ListItem("ทุกกลุ่ม",""));

        OleDbConnection Conn = new OleDbConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString);
        OleDbCommand command = new OleDbCommand();

        string sql = "SELECT distinct fl_groupname from tb_detailgroup where fl_group_id='" + idParam.Value + "' ";
        sql += " order by fl_groupname ";

        Conn.Open();

        command.CommandText = sql;
        command.Connection = Conn;
        OleDbDataReader rs = command.ExecuteReader();
        while (rs.Read())
        {
            cmbGroup.Items.Add(new ListItem(rs.GetString(0), rs.GetString(0)));
        }
        rs.Close();
        Conn.Close();

        cmbGroup.Visible = true;
        lblGroup.Visible = true;
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
            if (Request.QueryString["t"] != null) tParam.Value = Request.QueryString["t"].ToString();
            if (Request.QueryString["id"] != null) idParam.Value = Request.QueryString["id"].ToString();
            if (tParam.Value == "2") idParam.Value = "98";
            if (tParam.Value == "3") idParam.Value = "97";
            makeGroup();
            makeReport();
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

    protected void makeReport()
    {
        string t = tParam.Value;
        string id = idParam.Value;

        if (t == "") qParam.Text = "ทุกกลุ่ม";
        if (t == "1") qParam.Text = "มวลชนกอ.รมน.";
        if (t == "2") qParam.Text = "มวลชนภาครัฐ";
        if (t == "3") qParam.Text = "มวลชนภาคประชาชน";

        #region header
        tab1.Rows.Clear();
        tab1.BorderColor = ConfigurationManager.AppSettings["gridBorderColor"];
        tab1.Border = 1;
        tab1.CellPadding = 0;
        tab1.CellSpacing = 0;

        tab1.Rows.Add(new HtmlTableRow());
        tab1.Rows[0].Attributes.Add("class", "head");


        tab1.Rows[0].Cells.Add(new HtmlTableCell());
        tab1.Rows[0].Cells[0].InnerHtml = "<center>ลำดับที่</center>";
        tab1.Rows[0].Cells[0].Width = "50px";
        tab1.Rows[0].Cells.Add(new HtmlTableCell());
        tab1.Rows[0].Cells[1].InnerHtml = "<center>จังหวัด</center>";
        tab1.Rows[0].Cells[1].Width = "400px";
        tab1.Rows[0].Cells.Add(new HtmlTableCell());
        tab1.Rows[0].Cells[2].InnerHtml = "<center>จำนวนรายงาน</center>";
        tab1.Rows[0].Cells[2].Width = "100px";
        tab1.Rows[0].Cells.Add(new HtmlTableCell());
        tab1.Rows[0].Cells[3].InnerHtml = "<center>จำนวนรายชื่อ</center>";
        tab1.Rows[0].Cells[3].Width = "100px";
        #endregion

        #region whereCompose
        string whereClause = "where 1=1 ";

        if (id != "") whereClause += " and isnull(tb_detailgroup.fl_group_id,'') = '" + id +"' ";
        if (t!="") whereClause += " and tb_maingroup.fl_group_type ='" + t + "' ";

        //Filter Right
        if (Session["uGroup"].ToString() == "U")
        {
            if (Session["uDept"].ToString().Substring(0) != "0")
            {
                whereClause += " and isnull(tb_detailgroup.fl_dept,'') like '" + Session["uDept"].ToString().Replace("00", "") + "%' ";
            }
        }
        //Report WhereClause
        whereClause += " and tb_maingroup.fl_group_type <='3' ";

        if (cmbGroup.SelectedValue != "") whereClause += " and fl_groupname = '" + cmbGroup.SelectedValue +"' ";

        Session["whereClause"] = whereClause;

        string orderClause = " order by sortID ";

        #endregion

        #region SQLComposed
        //SQL Composed
        string sql = "SELECT distinct ";
        sql += " main.fl_group_name, ";
        sql += " main.fl_dept_name, ";
        sql += " isnull(reportCount,0), ";
        sql += " isnull(headCount,0), ";
        sql += " case SUBSTRING(fl_dept_id,2,2) ";
        sql += " when '00' then SUBSTRING(fl_dept_id,1,1) ";
        sql += " else SUBSTRING(fl_dept_id,1,1) + main.fl_dept_name end as sortID, ";
        sql += " main.fl_dept_id ";

        sql += " from ";

        sql += " (SELECT distinct ";
        if (id != "") sql += " isnull(fl_Group_name,'') as fl_group_name,"; else sql += " '' as fl_group_name, ";
        sql += "    fl_dept_name,fl_dept_id, ";
        sql += "    isnull(sum(cast(isnull(fl_reportMember,'0') as bigint)),0) as reportCount ";
        sql += " from tb_dept ";
        sql += " left outer join (select fl_group_name,fl_reportMember,fl_dept from tb_detailgroup ";
        sql += " inner  join tb_maingroup on tb_detailgroup.fl_group_id = tb_maingroup.fl_group_id ";
        sql += whereClause;
        sql += " ) tmpDet on tb_dept.fl_dept_id=tmpDet.fl_dept ";
        if (id != "") sql += " group by fl_group_name,fl_dept_name,fl_dept_id "; else sql += " group by fl_dept_name,fl_dept_id ";
        sql += " ) main ";

        sql += " left outer join ";

        sql += " (select distinct ";
        if (id != "") sql += " isnull(fl_Group_name,'') as fl_group_name,"; else sql += " '' as fl_group_name, ";
        sql += " fl_dept, ";
        sql += " isnull(count(tb_membergroup.fl_citizen_id),0) as headCount ";
        sql += " from tb_detailgroup ";
        sql += " inner join tb_maingroup on tb_detailgroup.fl_group_id = tb_maingroup.fl_group_id ";
        sql += " inner join tb_membergroup on tb_detailgroup.fl_id = tb_membergroup.fl_detailgroup_id ";
        sql += " inner join tb_citizen on tb_membergroup.fl_citizen_id = tb_citizen.fl_citizen_id ";
        sql += whereClause;
        sql += " and (fl_stop='' or fl_stop is null) ";
        if (id != "") sql += " group by fl_group_name,fl_dept "; else sql += " group by fl_dept ";
        sql += " ) c1 ";

        sql += " on main.fl_group_name=c1.fl_group_name ";
        sql += " and main.fl_dept_id=c1.fl_dept ";

        sql += orderClause;
        #endregion

        OleDbConnection Conn = new OleDbConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString);
        OleDbCommand command = new OleDbCommand();

        Conn.Open();

        //Response.Write(sql);
        //Response.End();
        Session["detailREPSQL"]=sql;
        command.CommandText = sql;
        command.CommandTimeout = 180;
        command.Connection = Conn;
        OleDbDataReader rs = command.ExecuteReader();

        int i = 1;
        string tmpVal = "";
        int j = 0;
        int g1Sum = 0;
        int g2Sum = 0;

        int g1Sum_s = 0;
        int g2Sum_s = 0;

        while (rs.Read())
        {
            tab1.Rows.Add(new HtmlTableRow());
            i = tab1.Rows.Count - 1;
            tab1.Rows[i].Attributes.Add("class", "off");
            tab1.Rows[i].Attributes.Add("onmouseover", "this.className='on'");
            tab1.Rows[i].Attributes.Add("onmouseout", "this.className='off'");
            tab1.Rows[i].Cells.Clear();

            if (rs.GetString(5).Substring(1, 2) == "00")
            {
                if (i > 1)
                {
                    //Sum last dept
                    tab1.Rows[i].Cells.Add(new HtmlTableCell());

                    tab1.Rows[i].Cells[0].Align = "LEFT";
                    tab1.Rows[i].Cells[0].ColSpan = 1;
                    tab1.Rows[i].Cells[0].InnerHtml = "&nbsp;";

                    tab1.Rows[i].Cells.Add(new HtmlTableCell());
                    tab1.Rows[i].Cells[1].Align = "LEFT";
                    tab1.Rows[i].Cells[1].ColSpan = 1;
                    tab1.Rows[i].Cells[1].InnerHtml = "<b>รวม</b>";

                    tab1.Rows[i].Cells.Add(new HtmlTableCell());
                    tab1.Rows[i].Cells[2].Align = "RIGHT";
                    tab1.Rows[i].Cells[2].ColSpan = 1;
                    tab1.Rows[i].Cells[2].InnerHtml = "<b>" + g1Sum_s.ToString("#,##0") + "</b>";

                    tab1.Rows[i].Cells.Add(new HtmlTableCell());
                    tab1.Rows[i].Cells[3].Align = "RIGHT";
                    tab1.Rows[i].Cells[3].ColSpan = 1;
                    tab1.Rows[i].Cells[3].InnerHtml = "<b>" + g2Sum_s.ToString("#,##0") + "</b>";

                    g1Sum_s = 0;
                    g2Sum_s = 0;

                    tab1.Rows.Add(new HtmlTableRow());
                    i = tab1.Rows.Count - 1;
                    tab1.Rows[i].Attributes.Add("class", "off");
                    tab1.Rows[i].Attributes.Add("onmouseover", "this.className='on'");
                    tab1.Rows[i].Attributes.Add("onmouseout", "this.className='off'");
                    tab1.Rows[i].Cells.Clear();
                }

                //Hightlight new dept
                tab1.Rows[i].Cells.Add(new HtmlTableCell());
                tab1.Rows[i].Cells[0].Align = "LEFT";
                tab1.Rows[i].Cells[0].ColSpan = 1;
                tab1.Rows[i].Cells[0].InnerHtml = "&nbsp;";

                tab1.Rows[i].Cells.Add(new HtmlTableCell());
                tab1.Rows[i].Cells[1].Align = "LEFT";
                tab1.Rows[i].Cells[1].ColSpan = 1;
                tab1.Rows[i].Cells[1].InnerHtml = "<b>" + rs.GetString(1) + "</b>";

                tab1.Rows.Add(new HtmlTableRow());
                i = tab1.Rows.Count - 1;
                tab1.Rows[i].Attributes.Add("class", "off");
                tab1.Rows[i].Attributes.Add("onmouseover", "this.className='on'");
                tab1.Rows[i].Attributes.Add("onmouseout", "this.className='off'");
                tab1.Rows[i].Cells.Clear();
                j = 0;
            }

            j++;
            tab1.Rows[i].Cells.Add(new HtmlTableCell());
            tab1.Rows[i].Cells[0].InnerHtml = j.ToString();

	        for (int xx = 1; xx < 4; xx++)
            {
                tab1.Rows[i].Cells.Add(new HtmlTableCell());
                if (xx == 1) tab1.Rows[i].Cells[xx].Align = "LEFT"; else tab1.Rows[i].Cells[xx].Align = "RIGHT";
                tab1.Rows[i].Cells[xx].ColSpan = 1;
                if (xx == 1) tab1.Rows[i].Cells[xx].InnerHtml = rs.GetString(xx); else tab1.Rows[i].Cells[xx].InnerHtml = Convert.ToInt32(rs.GetValue(xx)).ToString("#,##0");
                if (xx == 2)
                {
                    g1Sum += Convert.ToInt32(rs.GetValue(xx));
                    g1Sum_s += Convert.ToInt32(rs.GetValue(xx));
                }

                if (xx == 3)
                {
                    g2Sum += Convert.ToInt32(rs.GetValue(xx));
                    g2Sum_s += Convert.ToInt32(rs.GetValue(xx));
                }

                if (xx == 1)
                {
                    if (id != "") if(qParam.Text.IndexOf(" ")<0) qParam.Text += " " + rs.GetString(0);
                }
            }
        }
        maxi.Value = i.ToString();
        rs.Close();

        if (tab1.Rows.Count == 1)
        {
            //noDataFound
            tab1.Rows.Add(new HtmlTableRow());
            i = tab1.Rows.Count - 1;
            tab1.Rows[i].Attributes.Add("class", "off");
            tab1.Rows[i].Attributes.Add("onmouseover", "this.className='on'");
            tab1.Rows[i].Attributes.Add("onmouseout", "this.className='off'");
            tab1.Rows[i].Cells.Clear();

            tab1.Rows[i].Cells.Add(new HtmlTableCell());
            tab1.Rows[i].Cells[0].ColSpan = 4;
            tab1.Rows[i].Cells[0].Align = "CENTER";
            tab1.Rows[i].Cells[0].InnerHtml = "<font color='red' size='4'><b>" + ConfigurationManager.AppSettings["nodataMsg"] + "</b></font><input type='hidden' name ='cust1' value='" + ConfigurationManager.AppSettings["nodataMsg"] + "'>";
        }
        else
        {
            //Sum last dept
            tab1.Rows.Add(new HtmlTableRow());
            i = tab1.Rows.Count - 1;
            tab1.Rows[i].Attributes.Add("class", "off");
            tab1.Rows[i].Attributes.Add("onmouseover", "this.className='on'");
            tab1.Rows[i].Attributes.Add("onmouseout", "this.className='off'");
            tab1.Rows[i].Cells.Clear();

            tab1.Rows[i].Cells.Add(new HtmlTableCell());
            tab1.Rows[i].Cells[0].Align = "LEFT";
            tab1.Rows[i].Cells[0].ColSpan = 1;
            tab1.Rows[i].Cells[0].InnerHtml = "&nbsp;";

            tab1.Rows[i].Cells.Add(new HtmlTableCell());
            tab1.Rows[i].Cells[1].Align = "LEFT";
            tab1.Rows[i].Cells[1].ColSpan = 1;
            tab1.Rows[i].Cells[1].InnerHtml = "<b>รวม</b>";

            tab1.Rows[i].Cells.Add(new HtmlTableCell());
            tab1.Rows[i].Cells[2].Align = "RIGHT";
            tab1.Rows[i].Cells[2].ColSpan = 1;
            tab1.Rows[i].Cells[2].InnerHtml = "<b>" + g1Sum_s.ToString("#,##0") + "</b>";

            tab1.Rows[i].Cells.Add(new HtmlTableCell());
            tab1.Rows[i].Cells[3].Align = "RIGHT";
            tab1.Rows[i].Cells[3].ColSpan = 1;
            tab1.Rows[i].Cells[3].InnerHtml = "<b>" + g2Sum_s.ToString("#,##0") + "</b>";

            g1Sum_s = 0;
            g2Sum_s = 0;

            tab1.Rows.Add(new HtmlTableRow());
            i = tab1.Rows.Count - 1;
            tab1.Rows[i].Attributes.Add("class", "head");
            tab1.Rows[i].Cells.Clear();
            
            tab1.Rows[i].Cells.Add(new HtmlTableCell());
            tab1.Rows[i].Cells.Add(new HtmlTableCell());
            tab1.Rows[i].Cells.Add(new HtmlTableCell());
            tab1.Rows[i].Cells.Add(new HtmlTableCell());

            tab1.Rows[i].Cells[0].InnerHtml = "&nbsp;";
            tab1.Rows[i].Cells[1].InnerHtml = "รวม";
            tab1.Rows[i].Cells[2].InnerHtml = g1Sum.ToString("#,##0");
            tab1.Rows[i].Cells[3].InnerHtml = g2Sum.ToString("#,##0");

	    tab1.Rows[i].Cells[2].Align = "RIGHT";
	    tab1.Rows[i].Cells[3].Align = "RIGHT";
        }
        Conn.Close();

        bar.Visible = true;
        tab1.Visible = true;
    }
    protected void pdfPrint_Click(object sender, ImageClickEventArgs e)
    {

        #region header
        DataTable dt = new DataTable();

        dt.Columns.Add("param1");
        dt.Columns.Add("param2");

        dt.Columns.Add("name1");
        dt.Columns.Add("name2");
        dt.Columns.Add("name3");
        dt.Columns.Add("name4");

        DataRow dr;
        #endregion

        string sql = Session["detailREPSQL"].ToString();

        OleDbConnection Conn = new OleDbConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString);
        OleDbCommand command = new OleDbCommand();

        Conn.Open();

        command.CommandText = sql;
        command.CommandTimeout = 180;
        command.Connection = Conn;
        OleDbDataReader rs = command.ExecuteReader();

        int i = 1;
        string tmpVal = "";
        int j = 0;
        int g1Sum = 0;
        int g2Sum = 0;

        int g1Sum_s = 0;
        int g2Sum_s = 0;

        while (rs.Read())
        {
            dr = dt.NewRow();
            dr["param1"] = "รายงานรายละเอียดมวลชน " + qParam.Text;
            dr["param2"] = "จังหวัด";

            if (rs.GetString(5).Substring(1, 2) == "00")
            {
                if (i > 1)
                {
                    //Sum last dept
                    dr["name2"] = "รวม";
                    dr["name3"] = g1Sum_s.ToString("#,##0");
                    dr["name4"] = g2Sum_s.ToString("#,##0");

                    g1Sum_s = 0;
                    g2Sum_s = 0;

                    dt.Rows.Add(dr);
                    dr = dt.NewRow();
                }

                //Hightlight new dept
                dr["name2"] = rs.GetString(1);

                dt.Rows.Add(dr);
                dr = dt.NewRow();
            }

            dr["name1"] = i.ToString();

            for (int xx = 1; xx < 4; xx++)
            {                
                if (xx == 1) dr["name" + (xx+1).ToString()]= rs.GetString(xx); else dr["name" + (xx+1).ToString()]= Convert.ToInt32(rs.GetValue(xx)).ToString("#,##0");
                if (xx == 2)
                {
                    g1Sum += Convert.ToInt32(rs.GetValue(xx));
                    g1Sum_s += Convert.ToInt32(rs.GetValue(xx));
                }
                if (xx == 3)
                {
                    g2Sum += Convert.ToInt32(rs.GetValue(xx));
                    g2Sum_s += Convert.ToInt32(rs.GetValue(xx));
                }
            }

            dt.Rows.Add(dr);
            i++;
        }
        rs.Close();

        if (dt.Rows.Count == 0)
        {
            //noDataFound
            dr = dt.NewRow();
            dr["name2"] = ConfigurationManager.AppSettings["nodataMsg"];
            dt.Rows.Add(dr);
        }
        else
        {
            dr = dt.NewRow();
            //Sum last dept
            dr["name2"] = "รวม";
            dr["name3"] = g1Sum_s.ToString("#,##0");
            dr["name4"] = g2Sum_s.ToString("#,##0");

            g1Sum_s = 0;
            g2Sum_s = 0;

            dt.Rows.Add(dr);
            
            dr = dt.NewRow();

            dr["name1"] = " ";
            dr["name2"] = "รวม";
            dr["name3"] = g1Sum.ToString("#,##0");
            dr["name4"] = g2Sum.ToString("#,##0");

            dt.Rows.Add(dr);
        }
        Conn.Close();

        LocalReport rpt = new LocalReport();
        string fName = "detail_rep.pdf";
        rpt.ReportPath = "Memo3.rdlc";
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
    protected void cmbGroup_SelectedIndexChanged(object sender, EventArgs e)
    {
        makeReport();
    }
}