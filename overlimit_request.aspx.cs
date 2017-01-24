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
using System.Data.OleDb;
using System.Data.Common;
using System.Text;
using System.Net.Mail;
using System.IO;
using Microsoft.Reporting.WebForms;


public partial class _overlimit_request: System.Web.UI.Page
{
    static char[] hexDigits = {
    '0', '1', '2', '3', '4', '5', '6', '7',
    '8', '9', 'A', 'B', 'C', 'D', 'E', 'F'};

    protected string ToHexString(byte[] bytes)
    {
        char[] chars = new char[bytes.Length * 3];
        for (int i = 0; i < bytes.Length; i++)
        {
            int b = bytes[i];
            chars[i * 3] = '$';
            chars[i * 3 + 1] = hexDigits[b >> 4];
            chars[i * 3 + 2] = hexDigits[b & 0xF];
        }
        return new string(chars);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        btnPrint();
    }


    protected void btnPrint()
    {
        string fName = "a.pdf";
        LocalReport rpt = new LocalReport();
        DataTable dt = new DataTable();

            rpt.ReportPath = "Memo2.rdlc";
            rpt.DataSources.Clear();

            dt.Columns.Add("param1");
            dt.Columns.Add("param2");
            dt.Columns.Add("param3");
            dt.Columns.Add("param4");
            dt.Columns.Add("param5");
            dt.Columns.Add("param6");

            dt.Columns.Add("name1");
            dt.Columns.Add("name2");
            dt.Columns.Add("name3");

            dt.Columns.Add("baht1");
            dt.Columns.Add("baht2");
            dt.Columns.Add("baht3");
            dt.Columns.Add("baht4");

            dt.Columns.Add("no11");
            dt.Columns.Add("no12");
            dt.Columns.Add("no13");
            dt.Columns.Add("no14");
            dt.Columns.Add("no15");
            dt.Columns.Add("no16");

            dt.Columns.Add("name11");
            dt.Columns.Add("name12");
            dt.Columns.Add("name13");
            dt.Columns.Add("name14");
            dt.Columns.Add("name15");
            dt.Columns.Add("name16");

            dt.Columns.Add("no1");
            dt.Columns.Add("no2");
            dt.Columns.Add("no3");
            dt.Columns.Add("no4");
            dt.Columns.Add("no5");
            dt.Columns.Add("no6");

            dt.Columns.Add("name4");
            dt.Columns.Add("name5");
            dt.Columns.Add("name6");
            dt.Columns.Add("name7");
            dt.Columns.Add("name8");
            dt.Columns.Add("name9");

            dt.Columns.Add("baht11");
            dt.Columns.Add("baht12");
            dt.Columns.Add("baht13");
            dt.Columns.Add("baht14");
            dt.Columns.Add("baht15");
            dt.Columns.Add("baht16");

            dt.Columns.Add("baht17");
            dt.Columns.Add("baht18");
            dt.Columns.Add("baht19");

            dt.Columns.Add("name20");

            DataRow dr = dt.NewRow();
            dr["no2"] = "http://localhost:30568/web/G3.aspx?repID=11&proID=60|70";

        dt.Rows.Add(dr);
        
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
                "  <PageWidth>8.5in</PageWidth>" +
                "  <PageHeight>11.5in</PageHeight>" +
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
        Response.AddHeader("content-disposition", "attachment; filename=PDF//" + fName);
        Response.BinaryWrite(renderedBytes);
        Response.End();
    }
}