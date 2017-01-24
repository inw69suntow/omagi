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

public partial class _summary_REP : System.Web.UI.Page
{

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
            chkDist.Value = "";
            cmbDept.Value = "";
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
        if (Request.QueryString["t"].ToString() == "0") chkDist.Value = "0";
        if (Request.QueryString["id"] != null) cmbDept.Value = Request.QueryString["id"].ToString();
        if(chkDist.Value=="0") qParam.Text = "สรุปภาพรวม";
        if (chkDist.Value != "0")
        {
            qParam.Text = "รายจังหวัด";
            if (cmbDept.Value != "")
            {
                qParam.Text += " เฉพาะกอรมน.";
                if (cmbDept.Value.Substring(0, 1) == "0")
                {
                    qParam.Text += "ส่วนกลาง";
                }
                else
                {
                    if (cmbDept.Value.Substring(0, 1) == "5")
                    {
                        qParam.Text += "ภาค 4 ส่วนหน้า";
                    }
                    else
                    {
                        qParam.Text += "ภาค " + cmbDept.Value.Substring(0, 1);
                    }
                }

            }
        }
        #region header
        tab1.Rows.Clear();
        tab1.BorderColor = ConfigurationManager.AppSettings["gridBorderColor"];
        tab1.Border = 1;
        tab1.CellPadding = 0;
        tab1.CellSpacing = 0;

        tab1.Rows.Add(new HtmlTableRow());
        tab1.Rows[0].Attributes.Add("class", "head");


        tab1.Rows[0].Cells.Add(new HtmlTableCell());
        tab1.Rows[0].Cells[0].InnerHtml = "&nbsp;";
        tab1.Rows[0].Cells[0].Width = "20px";

        tab1.Rows[0].Cells.Add(new HtmlTableCell());
        tab1.Rows[0].Cells[1].InnerHtml = "หน่วยงาน";
        tab1.Rows[0].Cells[1].Width = "380px";

        tab1.Rows[0].Cells.Add(new HtmlTableCell());
        tab1.Rows[0].Cells[2].InnerHtml = "<center>มวลชนกอ.รมน.</center>";
        tab1.Rows[0].Cells[2].Width = "150px";
        tab1.Rows[0].Cells.Add(new HtmlTableCell());
        tab1.Rows[0].Cells[3].InnerHtml = "<center>มวลชนภาครัฐ</center>";
        tab1.Rows[0].Cells[3].Width = "150px";
        tab1.Rows[0].Cells.Add(new HtmlTableCell());
        tab1.Rows[0].Cells[4].InnerHtml = "<center>มวลชนภาคประชาชน</center>";
        tab1.Rows[0].Cells[4].Width = "150px";
        tab1.Rows[0].Cells.Add(new HtmlTableCell());
        tab1.Rows[0].Cells[5].InnerHtml = "<center>รวม</center>";
        tab1.Rows[0].Cells[5].Width = "150px";
        #endregion

        #region whereCompose
        string whereClause = "where 1=1 ";
        string deptWhere = whereClause;

        if (cmbDept.Value != "")
        {
            whereClause += " and isnull(tb_detailgroup.fl_dept,'') like '" + cmbDept.Value.Substring(0, 1) + "%' ";
            deptWhere += " and isnull(fl_dept_id,'') like '" + cmbDept.Value.Substring(0, 1) + "%' ";
        }

        //Filter Right
        if (Session["uGroup"].ToString() == "U")
        {
            if (Session["uDept"].ToString().Substring(0) != "0")
            {
                whereClause += " and isnull(tb_detailgroup.fl_dept,'') like '" + Session["uDept"].ToString().Replace("00", "") + "%' ";
                deptWhere += " and isnull(fl_dept_id,'') like '" + Session["uDept"].ToString().Replace("00", "") + "%' ";
            }
        }
        //Report WhereClause
        whereClause += " and tb_maingroup.fl_group_type <='3' ";

        Session["whereClause"] = whereClause;

        //string orderClause = " order by fl_dept_id, ";
        //orderClause += " isnull(fl_group_type,0) ";

        string orderClause = " order by sortID, ";
        orderClause += " isnull(fl_group_type,0) ";

        #endregion

        #region SQLComposed
        //SQL Composed
        string sql = "SELECT distinct ";

        sql += " isnull(fl_group_type,0), ";
        sql += " main.fl_dept_name , ";
        sql += " fl_dept_id , ";

        sql += " isnull(reportCount,0), ";
        sql += " case SUBSTRING(fl_dept_id,2,2) ";
        sql += " when '00' then SUBSTRING(fl_dept_id,1,1) ";
        sql += " else SUBSTRING(fl_dept_id,1,1) + main.fl_dept_name end as sortID ";

        sql += " from ";

        if (chkDist.Value!="0")
        {
            sql += "(select distinct fl_dept_id,fl_dept_name from tb_dept " + deptWhere + " ) main ";
            sql += " left outer join ";
            sql += " (SELECT distinct ";
            sql += "    fl_group_type, ";
            sql += "    fl_dept, ";
            sql += "    isnull(sum(cast(isnull(fl_reportMember,'0') as bigint)),0) as reportCount ";
            sql += " from tb_detailgroup ";
            sql += " inner join tb_maingroup  on tb_detailgroup.fl_group_id = tb_maingroup.fl_group_id ";
            sql += whereClause;
            sql += " group by fl_group_type,fl_dept ";
            sql += " ) data ";
            sql += " on main.fl_dept_id = data.fl_dept ";
        }
        else
        {
            sql += "(select distinct fl_dept_id,fl_dept_name from tb_dept " + deptWhere + " and fl_dept_id like '%00') main ";
            sql += " left outer join ";
            sql += " (SELECT distinct ";
            sql += "    fl_group_type, ";
            sql += "    substring(fl_dept,1,1) fl_dept, ";
            sql += "    isnull(sum(cast(isnull(fl_reportMember,'0') as bigint)),0) as reportCount ";
            sql += " from tb_detailgroup ";
            sql += " inner join tb_maingroup  on tb_detailgroup.fl_group_id = tb_maingroup.fl_group_id ";
            sql += whereClause;
            sql += " group by fl_group_type,substring(fl_dept,1,1) ";
            sql += " ) data ";
            sql += " on substring(main.fl_dept_id,1,1) = data.fl_dept ";
        }

        sql += orderClause;
        #endregion

        OleDbConnection Conn = new OleDbConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString);
        OleDbCommand command = new OleDbCommand();

        Conn.Open();

        Session["sumREPSQL"] = sql;
        command.CommandText = sql;
        command.CommandTimeout = 180;
        command.Connection = Conn;
        OleDbDataReader rs = command.ExecuteReader();

        int i = 1;
        string tmpVal = "";
        int j = 0;
        int rowSum = 0;
        int g1Sum = 0;
        int g2Sum = 0;
        int g3Sum = 0;

        int g1Sum_s = 0;
        int g2Sum_s = 0;
        int g3Sum_s = 0;

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

                if (chkDist.Value != "0")
                {
                    if (rs.GetString(2).Substring(1, 2) == "00")
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

                            tab1.Rows[i].Cells.Add(new HtmlTableCell());
                            tab1.Rows[i].Cells[4].Align = "RIGHT";
                            tab1.Rows[i].Cells[4].ColSpan = 1;
                            tab1.Rows[i].Cells[4].InnerHtml = "<b>" + g3Sum_s.ToString("#,##0") + "</b>";

                            rowSum = g1Sum_s + g2Sum_s + g3Sum_s;
                            tab1.Rows[i].Cells.Add(new HtmlTableCell());
                            tab1.Rows[i].Cells[5].Align = "RIGHT";
                            tab1.Rows[i].Cells[5].ColSpan = 1;
                            tab1.Rows[i].Cells[5].InnerHtml = "<b>" + rowSum.ToString("#,##0") + "</b>";

                            g1Sum_s = 0;
                            g2Sum_s = 0;
                            g3Sum_s = 0;

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
                }

                for (int xx = 0; xx < 6; xx++)
                {
                    tab1.Rows[i].Cells.Add(new HtmlTableCell());
                    if (xx <= 1) tab1.Rows[i].Cells[xx].Align = "LEFT"; else tab1.Rows[i].Cells[xx].Align = "RIGHT";
                    tab1.Rows[i].Cells[xx].ColSpan = 1;
                    tab1.Rows[i].Cells[xx].InnerHtml = "&nbsp;";
                }
                tmpVal = rs.GetString(1);
                rowSum = 0;
                j++;

                tab1.Rows[i].Cells[0].InnerHtml = j.ToString("#,##0");
                tab1.Rows[i].Cells[1].InnerHtml = tmpVal;
            }
            if (rs.GetString(0) == "1")
            {
                tab1.Rows[i].Cells[2].InnerHtml = Convert.ToUInt64(rs.GetValue(3)).ToString("#,##0");
                g1Sum += Convert.ToInt32(rs.GetValue(3));
                g1Sum_s += Convert.ToInt32(rs.GetValue(3));
            }
            if (rs.GetString(0) == "2")
            {
                tab1.Rows[i].Cells[3].InnerHtml = Convert.ToUInt64(rs.GetValue(3)).ToString("#,##0");
                g2Sum += Convert.ToInt32(rs.GetValue(3));
                g2Sum_s += Convert.ToInt32(rs.GetValue(3));
            }
            if (rs.GetString(0) == "3")
            {
                tab1.Rows[i].Cells[4].InnerHtml = Convert.ToUInt64(rs.GetValue(3)).ToString("#,##0");
                g3Sum += Convert.ToInt32(rs.GetValue(3));
                g3Sum_s += Convert.ToInt32(rs.GetValue(3));
            }
            rowSum += Convert.ToInt32(rs.GetValue(3));
            tab1.Rows[i].Cells[5].InnerHtml = rowSum.ToString("#,##0");
        }
        maxi.Value = i.ToString();
        rs.Close();

        if (tab1.Rows.Count == 0)
        {
            //noDataFound
            tab1.Rows.Add(new HtmlTableRow());
            i = tab1.Rows.Count - 1;
            tab1.Rows[i].Attributes.Add("class", "off");
            tab1.Rows[i].Attributes.Add("onmouseover", "this.className='on'");
            tab1.Rows[i].Attributes.Add("onmouseout", "this.className='off'");
            tab1.Rows[i].Cells.Clear();

            tab1.Rows[i].Cells.Add(new HtmlTableCell());
            tab1.Rows[i].Cells[0].ColSpan = 6;
            tab1.Rows[i].Cells[0].Align = "CENTER";
            tab1.Rows[i].Cells[0].InnerHtml = "<font color='red' size='4'><b>" + ConfigurationManager.AppSettings["nodataMsg"] + "</b></font><input type='hidden' name ='cust1' value='" + ConfigurationManager.AppSettings["nodataMsg"] + "'>";
        }
        else
        {
            if (chkDist.Value != "0")
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

                tab1.Rows[i].Cells.Add(new HtmlTableCell());
                tab1.Rows[i].Cells[4].Align = "RIGHT";
                tab1.Rows[i].Cells[4].ColSpan = 1;
                tab1.Rows[i].Cells[4].InnerHtml = "<b>" + g3Sum_s.ToString("#,##0") + "</b>";

                rowSum = g1Sum_s + g2Sum_s + g3Sum_s;
                tab1.Rows[i].Cells.Add(new HtmlTableCell());
                tab1.Rows[i].Cells[5].Align = "RIGHT";
                tab1.Rows[i].Cells[5].ColSpan = 1;
                tab1.Rows[i].Cells[5].InnerHtml = "<b>" + rowSum.ToString("#,##0") + "</b>";

                g1Sum_s = 0;
                g2Sum_s = 0;
                g3Sum_s = 0;

            }
            tab1.Rows.Add(new HtmlTableRow());
            i = tab1.Rows.Count - 1;
            tab1.Rows[i].Attributes.Add("class", "head");
            tab1.Rows[i].Cells.Clear();
            
            for (int xx = 0; xx < 6; xx++)
            {
                tab1.Rows[i].Cells.Add(new HtmlTableCell());
                if (xx <= 1) tab1.Rows[i].Cells[xx].Align = "LEFT"; else tab1.Rows[i].Cells[xx].Align = "RIGHT";
                tab1.Rows[i].Cells[xx].ColSpan = 1;
                tab1.Rows[i].Cells[xx].InnerHtml = "&nbsp;";
            }

            tab1.Rows[i].Cells[0].InnerHtml = "&nbsp;";
            tab1.Rows[i].Cells[1].InnerHtml = "รวม";
            tab1.Rows[i].Cells[2].InnerHtml = g1Sum.ToString("#,##0");
            tab1.Rows[i].Cells[3].InnerHtml = g2Sum.ToString("#,##0");
            tab1.Rows[i].Cells[4].InnerHtml = g3Sum.ToString("#,##0");
            rowSum = g1Sum + g2Sum + g3Sum;
            tab1.Rows[i].Cells[5].InnerHtml = rowSum.ToString("#,##0");
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

        dt.Columns.Add("name1");
        dt.Columns.Add("name2");
        dt.Columns.Add("name3");
        dt.Columns.Add("name4");
        dt.Columns.Add("name5");
        dt.Columns.Add("name6");

        DataRow dr = dt.NewRow();
        dr["param1"] = "รายงานสรุปยอดมวลชน" + qParam.Text;
        #endregion

        string sql = Session["sumREPSQL"].ToString(); 

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
        int rowSum = 0;
        int g1Sum = 0;
        int g2Sum = 0;
        int g3Sum = 0;

        int g1Sum_s = 0;
        int g2Sum_s = 0;
        int g3Sum_s = 0;

        while (rs.Read())
        {
            if (tmpVal != rs.GetString(1))
            {
                dt.Rows.Add(dr);
                dr = dt.NewRow();
                if (chkDist.Value != "0")
                {
                    if (rs.GetString(2).Substring(1, 2) == "00")
                    {
                        if (dt.Rows.Count > 1)
                        {
                            //Sum last dept
                            dr["name1"] = " ";
                            dr["name2"] = "รวม";
                            dr["name3"] = g1Sum_s.ToString("#,##0");
                            dr["name4"] = g2Sum_s.ToString("#,##0");
                            dr["name5"] = g3Sum_s.ToString("#,##0");
                            rowSum = g1Sum_s + g2Sum_s + g3Sum_s;
                            dr["name6"] = rowSum.ToString("#,##0");

                            g1Sum_s = 0;
                            g2Sum_s = 0;
                            g3Sum_s = 0;

                            dt.Rows.Add(dr);
                            dr = dt.NewRow();

                            dr["name1"] = "" + rs.GetString(1) + "";

                            j = 0;
                        }
                    }
                }

                for (int xx = 1; xx < 7; xx++) dr["name" + xx.ToString()] = " ";
                tmpVal = rs.GetString(1);
                rowSum = 0;
                j++;

                dr["name1"] = j.ToString("#,##0");
                dr["name2"] = tmpVal;
            }
            if (rs.GetString(0) == "1")
            {
                dr["name3"] = Convert.ToUInt64(rs.GetValue(3)).ToString("#,##0");
                g1Sum += Convert.ToInt32(rs.GetValue(3));
                g1Sum_s += Convert.ToInt32(rs.GetValue(3));
            }
            if (rs.GetString(0) == "2")
            {
                dr["name4"] = Convert.ToUInt64(rs.GetValue(3)).ToString("#,##0");
                g2Sum += Convert.ToInt32(rs.GetValue(3));
                g2Sum_s += Convert.ToInt32(rs.GetValue(3));
            }
            if (rs.GetString(0) == "3")
            {
                dr["name5"] = Convert.ToUInt64(rs.GetValue(3)).ToString("#,##0");
                g3Sum += Convert.ToInt32(rs.GetValue(3));
                g3Sum_s += Convert.ToInt32(rs.GetValue(3));
            }
            rowSum += Convert.ToInt32(rs.GetValue(3));
            dr["name6"] = rowSum.ToString("#,##0");
        }
        rs.Close();

        if (dt.Rows.Count == 0)
        {
            //noDataFound
            dt.Rows.Add(dr);
            dr = dt.NewRow();
            dr["name2"] = ConfigurationManager.AppSettings["nodataMsg"];
        }
        else
        {
            if (chkDist.Value != "0")
            {
                //Sum last dept
                dt.Rows.Add(dr);
                dr = dt.NewRow();

                dr["name1"] = " ";
                dr["name2"] = "รวม";
                dr["name3"] = g1Sum_s.ToString("#,##0");
                dr["name4"] = g2Sum_s.ToString("#,##0");
                dr["name5"] = g3Sum_s.ToString("#,##0");

                rowSum = g1Sum_s + g2Sum_s + g3Sum_s;
                dr["name6"] = rowSum.ToString("#,##0");

                g1Sum_s = 0;
                g2Sum_s = 0;
                g3Sum_s = 0;

            }
            dt.Rows.Add(dr);
            dr = dt.NewRow();

            for (int xx = 1; xx < 7; xx++) dr["name" + xx.ToString()] = " ";

            dr["name1"] = " ";
            dr["name2"] = "รวม";
            dr["name3"] = g1Sum.ToString("#,##0");
            dr["name4"] = g2Sum.ToString("#,##0");
            dr["name5"] = g3Sum.ToString("#,##0");

            rowSum = g1Sum + g2Sum + g3Sum;
            dr["name6"] = rowSum.ToString("#,##0");
        }
        Conn.Close();
        //Add last row
        if (dt.Rows.Count > 1) dt.Rows.Add(dr);

        LocalReport rpt = new LocalReport();
        string fName = "summary_rep.pdf";
        rpt.ReportPath = "Memo2.rdlc";
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