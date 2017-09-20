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

public partial class _summary_REP_2 : System.Web.UI.Page
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
            titleDataSet();
            provinceDataSet();
            courseDataSet();
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
        tab1.Rows[0].Cells[1].Width = "430px";

        tab1.Rows[0].Cells.Add(new HtmlTableCell());
        tab1.Rows[0].Cells[2].InnerHtml = "<center>ปี อบรม</center>";
        tab1.Rows[0].Cells[2].Width = "100px";

        tab1.Rows[0].Cells.Add(new HtmlTableCell());
        tab1.Rows[0].Cells[3].InnerHtml = "<center>คำนำหน้า</center>";
        tab1.Rows[0].Cells[3].Width = "100";
  

        tab1.Rows[0].Cells.Add(new HtmlTableCell());
        tab1.Rows[0].Cells[4].InnerHtml = "<center>รวม</center>";
        tab1.Rows[0].Cells[4].Width = "100px";
  



        OleDbConnection Conn = new OleDbConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString);
        OleDbCommand command = new OleDbCommand();

        Conn.Open();
        String sql = getQuery();
        Session["sumREPSQL"] = sql;
        command.CommandText = sql;
        command.CommandTimeout = 180;
        command.Connection = Conn;
        OleDbDataReader rs = command.ExecuteReader();

        int i = 1;
        while (rs.Read())
        {
            tab1.Rows.Add(new HtmlTableRow());
            tab1.Rows[i].Cells.Add(new HtmlTableCell());
            tab1.Rows[i].Cells[0].Align = "LEFT";
            tab1.Rows[i].Cells[0].InnerHtml = Convert.ToString(i);

            tab1.Rows[i].Cells.Add(new HtmlTableCell());
            tab1.Rows[i].Cells[1].Align = "LEFT";
            tab1.Rows[i].Cells[1].InnerHtml = Convert.ToString(rs["fl_course"]);

            tab1.Rows[i].Cells.Add(new HtmlTableCell());
            tab1.Rows[i].Cells[2].Align = "LEFT";
            tab1.Rows[i].Cells[2].InnerHtml = Convert.ToString(rs["fl_year"]);

            tab1.Rows[i].Cells.Add(new HtmlTableCell());
            tab1.Rows[i].Cells[3].Align = "LEFT";
            tab1.Rows[i].Cells[3].InnerHtml = Convert.ToString(rs["fl_title"]);
            

            tab1.Rows[i].Cells.Add(new HtmlTableCell());
            tab1.Rows[i].Cells[4].Align = "RIGHT";
            tab1.Rows[i].Cells[4].InnerHtml = Convert.ToInt32(rs["size"]).ToString("#,##0");
            i++;
           
        }
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
           
        Conn.Close();
        bar.Visible = true;
        tab1.Visible = true;
    }



    private String getQuery()
    {
        String sql = "";
        sql+=" select count(c.fl_title) size,td.fl_course,td.fl_year,t.fl_title ";
        sql+=" from tb_train_detail td";
        sql+=" left join tb_train_course tc";
        sql+=" on td.fl_course=tc.fl_course";
        sql+=" inner join tb_citizen c";
        sql+=" on td.fl_citizen_id=c.fl_citizen_id";
        sql+=" left join tb_title t";
        sql+=" on c.fl_title=t.fl_code";
        sql+=" where 1=1 ";
        if (cmbCourse.SelectedValue.Trim() != "")
        {
            sql += " and td.fl_course ='" + cmbCourse.SelectedValue.Trim() + "'";
        }
        if (cmbTitle.SelectedValue.Trim() != "")
        {
            sql += " and c.fl_title='" + cmbTitle.SelectedValue.Trim() + "' ";
        }
        //sql+=" and c.fl_educational is null ";
        if (txtEdu.Text.Trim() != "")
        {
            sql += " and c.fl_educational='" + txtEdu.Text.Trim() + "' ";
        }
        if (cmbProvince.SelectedValue.Trim() != "")
        {
            sql += " and c.fl_province_code='" + cmbProvince.SelectedValue.Trim() + "' ";
        }
        //sql+=" and c.fl_province_code =77";
        //sql+=" and c.fl_birth> '19580814' and c.fl_birth < '19860805'";
        sql +=" group by td.fl_course,td.fl_year,td.fl_gen,t.fl_title";
        return sql;
        
    }



    protected void titleDataSet()
    {
        cmbTitle.Items.Clear();
        cmbTitle.Items.Add(new ListItem("ไม่กำหนด", ""));

        OleDbConnection Conn = new OleDbConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString);
        OleDbCommand command = new OleDbCommand();

        string sql = "SELECT distinct fl_code,fl_title from tb_title ";
        sql += " order by fl_code ";

        Conn.Open();

        command.CommandText = sql;
        command.Connection = Conn;
        OleDbDataReader rs = command.ExecuteReader();
        while (rs.Read())
        {
            cmbTitle.Items.Add(new ListItem(rs.GetString(1), rs.GetString(0)));
        }
        rs.Close();
        Conn.Close();
    }

    protected void provinceDataSet()
    {
        cmbProvince.Items.Clear();
        cmbProvince.Items.Add(new ListItem("ไม่กำหนด", ""));

        //cmbProvince.Items.Add(new ListItem("ไม่กำหนด", ""));
        //cmbProvinceSearch.Items.Add(new ListItem("ไม่กำหนด", ""));
        //cmbTrainProvince.Items.Add(new ListItem("ไม่กำหนด", ""));

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
    }


    protected void courseDataSet()
    {
        cmbCourse.Items.Clear();

        cmbCourse.Items.Add(new ListItem("ไม่กำหนด", ""));
        //cmbCourseSearch.Items.Add(new ListItem("ไม่กำหนด", ""));

        OleDbConnection Conn = new OleDbConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString);
        OleDbCommand command = new OleDbCommand();

        string sql = "SELECT distinct fl_course from tb_train_Course ";
        sql += " order by fl_course ";

        Conn.Open();

        command.CommandText = sql;
        command.Connection = Conn;
        OleDbDataReader rs = command.ExecuteReader();
        while (rs.Read())
        {
            cmbCourse.Items.Add(new ListItem(rs.GetString(0), rs.GetString(0)));
        }
        rs.Close();
        Conn.Close();
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
        //dt.Columns.Add("name6");

        DataRow dr = dt.NewRow();
        dr["param1"] = "รายงานสรุปตามเงื่อนไข";
        dr["name1"] = "ลำดับ";
        dr["name2"] = "หน่วยงาน";
        dr["name3"] = "ปี อบรม";
        dr["name4"] = "คำนำหน้า";
        dr["name5"] = "รวม"; 
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
        while (rs.Read())
        {
            dt.Rows.Add(dr);
            dr = dt.NewRow();
            dr["name1"] = Convert.ToString(i);
            dr["name2"] = Convert.ToString(rs["fl_course"]);
            dr["name3"] = Convert.ToString(rs["fl_year"]);
            dr["name4"] = Convert.ToString(rs["fl_title"]);
            dr["name5"] = Convert.ToInt32(rs["size"]).ToString("#,##0");
            i++;

        }

        if (dt.Rows.Count == 0)
        {
            //noDataFound
            dt.Rows.Add(dr);
            dr = dt.NewRow();
            dr["name2"] = ConfigurationManager.AppSettings["nodataMsg"];
        }
        rs.Close();
        Conn.Close();
        //Add last row
        //if (dt.Rows.Count > 1) 
            dt.Rows.Add(dr);
        //
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
 
    protected void btnSearch_Click(object sender, ImageClickEventArgs e)
    {
        makeReport();
    }
}