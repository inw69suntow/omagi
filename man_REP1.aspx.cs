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

public partial class _man_REP1: System.Web.UI.Page
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
            userDataSet();
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

    protected void userDataSet()
    {
        if (pageID.SelectedValue != "") userDataSet(Convert.ToInt32(pageID.SelectedValue)); else userDataSet(1);
    }

    protected void userDataSet(int curPage)
    {
        dtGrid.Rows.Clear();
        dtGrid.BorderColor = ConfigurationManager.AppSettings["gridBorderColor"];
        dtGrid.Border = 1;
        dtGrid.CellPadding = 0;
        dtGrid.CellSpacing = 0;
        dtGrid.Rows.Add(new HtmlTableRow());
        dtGrid.Rows[0].Attributes.Add("class", "head");
        dtGrid.Rows[0].Cells.Add(new HtmlTableCell());
        dtGrid.Rows[0].Cells[0].InnerHtml = "เลือก";
        dtGrid.Rows[0].Cells[0].Width = "1%";
        dtGrid.Rows[0].Cells.Add(new HtmlTableCell());
        dtGrid.Rows[0].Cells[1].Width = "15%";
        dtGrid.Rows[0].Cells[1].InnerHtml = "<center>เลขประจำตัว</center>";
        dtGrid.Rows[0].Cells.Add(new HtmlTableCell());
        dtGrid.Rows[0].Cells[2].Width = "25%";
        dtGrid.Rows[0].Cells[2].InnerHtml = "<center>ชื่อ</center>";
        dtGrid.Rows[0].Cells.Add(new HtmlTableCell());
        dtGrid.Rows[0].Cells[3].Width = "25%";
        dtGrid.Rows[0].Cells[3].InnerHtml = "<center>สกุล</center>";
        dtGrid.Rows[0].Cells.Add(new HtmlTableCell());
        dtGrid.Rows[0].Cells[4].Width = "20%";
        dtGrid.Rows[0].Cells[4].InnerHtml = "<center>จังหวัด</center>";
        dtGrid.Rows[0].Cells.Add(new HtmlTableCell());
        dtGrid.Rows[0].Cells[5].Width = "10%";
        dtGrid.Rows[0].Cells[5].InnerHtml = "<center>โทรศัพท์มือถือ</center>";
        dtGrid.Rows[0].Cells.Add(new HtmlTableCell());
        dtGrid.Rows[0].Cells[6].Width = "10%";
        dtGrid.Rows[0].Cells[6].InnerHtml = "<center>อีเมล์</center>";
        dtGrid.Rows[0].Cells.Add(new HtmlTableCell());
        dtGrid.Rows[0].Cells[7].Width = "10%";
        dtGrid.Rows[0].Cells[7].InnerHtml = "<center>ปรับปรุงล่าสุด</center>";

        string sql = "SELECT distinct b.fl_citizen_id,";
        sql = sql + " fl_fname, ";
        sql = sql + " fl_sname, ";
        sql = sql + " case isnull(fl_province_name,'') when '' then isnull(b.fl_province_code,'') else isnull(fl_province_name,'') end provinceName, ";
        sql = sql + " fl_mobNo, ";
        sql = sql + " fl_eMail, ";

        //For coloring
        sql = sql + " isnull(b.fl_status,'0') as fl_status, ";

        sql += " isnull(fl_update_time,'') as fl_update";

        sql = sql + " FROM tb_citizen b ";
        sql = sql + " left join tb_moicode c on b.fl_province_code = c.fl_province_code ";

        sql = sql + " where 1=1 ";

        //Check filter command
        if (txtID.Text.Trim() != "") sql += " and fl_citizen_id like '" + txtID.Text.Trim().Replace("'", "").Replace(";", "") + "%' ";
        if (txtName.Text.Trim() != "") sql += " and fl_fname like '" + txtName.Text.Trim().Replace("'", "").Replace(";", "") + "%' ";
        if (txtSurname.Text.Trim() != "") sql += " and fl_sname like '" + txtSurname.Text.Trim().Replace("'", "").Replace(";", "") + "%' ";
        if (txtMobile.Text.Trim() != "") sql += " and fl_mobNo like '" + txtMobile.Text.Trim().Replace("'", "").Replace(";", "") + "%' ";
        if (txtOffice.Text.Trim() != "") sql += " and fl_offNo like '" + txtOffice.Text.Trim().Replace("'", "").Replace(";", "") + "%' ";
        if (txtHomeNo.Text.Trim() != "") sql += " and fl_telNo like '" + txtHomeNo.Text.Trim().Replace("'", "").Replace(";", "") + "%' ";

        OleDbConnection Conn = new OleDbConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString);
        OleDbCommand command = new OleDbCommand();

        Conn.Open();
        command.Connection = Conn;

        //Count Page        
        command.CommandText = "SELECT isnull(COUNT(fl_citizen_id),0) as c from (" + sql + ") olderS "; ;
        command.Connection = Conn;

        OleDbDataReader rs = command.ExecuteReader();
        int rCount = 0;
        
        if (rs.Read()) rCount = rs.GetInt32(0);
        rs.Close();

        int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["pageSize"].ToString());
        if ((rCount % pageSize) > 0) rCount = ((rCount - (rCount % pageSize)) / pageSize) + 1; else rCount = rCount / pageSize;

        pageID.Items.Clear();
        for (int x = 1; x <= rCount; x++) pageID.Items.Add(new ListItem(x.ToString(), x.ToString()));
        if (rCount != 0)
        {
            if (curPage <= rCount)
            {
                pageID.SelectedIndex = curPage - 1;
            }
            else
            {
                pageID.SelectedIndex = 0;
                curPage = 1;
            }
        }

        btnPrev.Visible = true;
        btnNext.Visible = true;

        if (rCount == 0)
        {
            btnPrev.Visible = false;
            btnNext.Visible = false;
        }

        if (pageID.SelectedValue == "1") btnPrev.Visible = false;
        if (pageID.SelectedValue == rCount.ToString()) btnNext.Visible = false;

        sql = sql + " order by ";
        sql = sql + " fl_fname ASC, ";
        sql = sql + " fl_sname ASC ";

        int i = 0;
        command.CommandText = sql.Replace(";", "");
        
        rs = command.ExecuteReader();
        while (i < (curPage - 1) * pageSize) { rs.Read(); i++; }
        i = 1;
        while ((rs.Read()) && (i <= pageSize))
        {
            string action = "window.open('man_REP.aspx?id=" + rs.GetString(0) + "');";
            dtGrid.Rows.Add(new HtmlTableRow());
            if (Convert.ToInt64("0" + rs.GetString(6)) > 0)
            {
                dtGrid.Rows[i].Attributes.Add("class", "off");
                dtGrid.Rows[i].Attributes.Add("onmouseover", "this.className='on'");
                dtGrid.Rows[i].Attributes.Add("onmouseout", "this.className='off'");
                dtGrid.Rows[i].Attributes.Add("onclick", action);
            }
            else
            {
                dtGrid.Rows[i].Attributes.Add("class", "close");
                dtGrid.Rows[i].Attributes.Add("onmouseover", "this.className='on'");
                dtGrid.Rows[i].Attributes.Add("onmouseout", "this.className='close'");
                dtGrid.Rows[i].Attributes.Add("onclick", action);
            }
            dtGrid.Rows[i].Cells.Add(new HtmlTableCell());
            dtGrid.Rows[i].Cells[0].ColSpan = 1;
            dtGrid.Rows[i].Cells[0].Align = "CENTER";

            dtGrid.Rows[i].Cells[0].InnerHtml = "<input type='checkbox' id='check_" + i.ToString() + "' onClick=\"" + action + "\">"; 
            
            for (int j = 0; j < 6; j++)
            {
                dtGrid.Rows[i].Cells.Add(new HtmlTableCell());
                dtGrid.Rows[i].Cells[j+1].Align = "LEFT";
                dtGrid.Rows[i].Cells[j+1].InnerHtml = rs.GetString(j);
            }

            string tmpUpdate = "";
            if (rs.GetString(7) != "")
            {
                tmpUpdate += rs.GetString(7).Substring(6, 2) + "/";
                tmpUpdate += rs.GetString(7).Substring(4, 2) + "/";
                tmpUpdate += rs.GetString(7).Substring(0, 4) + " ";
            }
            dtGrid.Rows[i].Cells.Add(new HtmlTableCell());
            dtGrid.Rows[i].Cells[7].Align = "LEFT";
            dtGrid.Rows[i].Cells[7].InnerHtml = "&nbsp;" + tmpUpdate;

            i = i + 1;
        }
        rs.Close();

        if (i == 1)
        {
            //noDataFound
            dtGrid.Rows.Add(new HtmlTableRow());
            dtGrid.Rows[i].Attributes.Add("class", "off");
            dtGrid.Rows[i].Attributes.Add("onmouseover", "this.className='on'");
            dtGrid.Rows[i].Attributes.Add("onmouseout", "this.className='off'");
            dtGrid.Rows[i].Cells.Add(new HtmlTableCell());
            dtGrid.Rows[i].Cells[0].ColSpan = 7;
            dtGrid.Rows[i].Cells[0].Align = "CENTER";
            dtGrid.Rows[i].Cells[0].InnerHtml = "<font color='red' size='4'><b>" + ConfigurationManager.AppSettings["nodataMsg"] + "</b></font>";
        }
        rs.Close();
        Conn.Close();
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        userDataSet();
    }
    protected void pageID_SelectedIndexChanged(object sender, EventArgs e)
    {
        userDataSet(Convert.ToInt32(pageID.SelectedValue));
    }
    protected void btnPrev_Click(object sender, EventArgs e)
    {
        userDataSet(Convert.ToInt32(pageID.SelectedValue) - 1);
    }
    protected void btnNext_Click(object sender, EventArgs e)
    {
        userDataSet(Convert.ToInt32(pageID.SelectedValue) + 1);
    }
}