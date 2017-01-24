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

public partial class _audit_trail_REP: System.Web.UI.Page
{

    private void Init_Data()
    {
        Fr_DD.Items.Clear();
        To_DD.Items.Clear();
        Fr_MM.Items.Clear();
        To_MM.Items.Clear();
        Fr_YY.Items.Clear();
        To_YY.Items.Clear();

        int i;
        for (i = 1; i <= 31; i++)
        {
            Fr_DD.Items.Add(new ListItem(i.ToString().PadLeft(2, '0'), i.ToString().PadLeft(2, '0')));
            To_DD.Items.Add(new ListItem(i.ToString().PadLeft(2, '0'), i.ToString().PadLeft(2, '0')));
        }
        for (i = 1; i <= 12; i++)
        {
            string tmpName = "";
            if (i == 1) tmpName = "มกราคม";
            if (i == 2) tmpName = "กุมภาพันธ์";
            if (i == 3) tmpName = "มีนาคม";
            if (i == 4) tmpName = "เมษายน";
            if (i == 5) tmpName = "พฤษภาคม";
            if (i == 6) tmpName = "มิถุนายน";
            if (i == 7) tmpName = "กรกฎาคม";
            if (i == 8) tmpName = "สิงหาคม";
            if (i == 9) tmpName = "กันยายน";
            if (i == 10) tmpName = "ตุลาคม";
            if (i == 11) tmpName = "พฤศจิกายน";
            if (i == 12) tmpName = "ธันวาคม";

            Fr_MM.Items.Add(new ListItem(tmpName, i.ToString().PadLeft(2, '0')));
            To_MM.Items.Add(new ListItem(tmpName, i.ToString().PadLeft(2, '0')));
        }

        string currentyear = DateTime.Now.ToString("yyyy");
        int tmpyear = Convert.ToInt32(currentyear);
        if (tmpyear > 2500) tmpyear = tmpyear - 543;
        string currentmonth = DateTime.Now.ToString("MM");
        int tmpmonth = Convert.ToInt32(currentmonth);
        string currentday = DateTime.Now.ToString("dd");
        int tmpday = Convert.ToInt32(currentday);

        for (i = tmpyear - 5; i <= tmpyear + 1; i++)
        {
            Fr_YY.Items.Add(new ListItem(Convert.ToString(i + 543), i.ToString()));
            To_YY.Items.Add(new ListItem(Convert.ToString(i + 543), i.ToString()));
        }
        To_DD.SelectedIndex = tmpday - 1;
        To_MM.SelectedIndex = tmpmonth - 1;
        To_YY.SelectedIndex = 5;

        currentyear = DateTime.Now.AddDays(-30).ToString("yyyy");
        int fryear = Convert.ToInt32(currentyear);
        if (fryear > 2500) fryear = fryear - 543;
        currentmonth = DateTime.Now.AddDays(-30).ToString("MM");
        tmpmonth = Convert.ToInt32(currentmonth);
        currentday = DateTime.Now.AddDays(-30).ToString("dd");
        tmpday = Convert.ToInt32(currentday);


        Fr_DD.SelectedIndex = tmpday - 1;
        Fr_MM.SelectedIndex = tmpmonth - 1;
        if (fryear == tmpyear) Fr_YY.SelectedIndex = 5; else Fr_YY.SelectedIndex = 4;
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
				Init_Data();
				btnExport.Visible=false;
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
        lblResponseS.Visible = false;
        lblResponseS.Text = "";

        string whereClause = " where 1=1 ";
		string frDate="";
		string enDate="";
		string tmpString="";
		
		if (Convert.ToInt32(Fr_YY.SelectedItem.ToString()) > Convert.ToInt32(To_YY.SelectedItem.ToString()))
		{
			lblResponseS.Text = "โปรดตรวจสอบช่วงของวันที่";
			lblResponseS.Visible = true;
			return;
		}
		else if ((Fr_MM.SelectedIndex > To_MM.SelectedIndex) && (Fr_YY.SelectedIndex == To_YY.SelectedIndex))
		{
			lblResponseS.Text = "โปรดตรวจสอบช่วงของวันที่";
			lblResponseS.Visible = true;
			return;
		}
		else if ((Fr_DD.SelectedIndex > To_DD.SelectedIndex) && (Fr_MM.SelectedIndex == To_MM.SelectedIndex) && (Fr_YY.SelectedIndex == To_YY.SelectedIndex))
		{
			lblResponseS.Text = "โปรดตรวจสอบช่วงของวันที่";
			lblResponseS.Visible = true;
			return;
		}
		if (Convert.ToInt16(Fr_YY.SelectedItem.ToString()) > 2500)
			frDate = (Convert.ToInt16(Fr_YY.SelectedItem.ToString()) - 543).ToString() + String.Format("{0:00}", Fr_MM.SelectedIndex + 1) + String.Format("{0:00}", Fr_DD.SelectedIndex + 1);
		else
			frDate = Fr_YY.SelectedItem.ToString() + String.Format("{0:00}", Fr_MM.SelectedIndex + 1) + String.Format("{0:00}", Fr_DD.SelectedIndex + 1);

		if (Convert.ToInt16(To_YY.SelectedItem.ToString()) > 2500)
			enDate = (Convert.ToInt16(To_YY.SelectedItem.ToString()) - 543).ToString() + String.Format("{0:00}", To_MM.SelectedIndex + 1) + String.Format("{0:00}", To_DD.SelectedIndex + 1);
		else
			enDate = To_YY.SelectedItem.ToString() + String.Format("{0:00}", To_MM.SelectedIndex + 1) + String.Format("{0:00}", To_DD.SelectedIndex + 1);

        if (frDate != "") whereClause = whereClause + " and fl_datetime>='" + frDate.Replace("'", "''") + "' ";
        if (enDate != "") whereClause = whereClause + " and fl_datetime<='" + enDate.Replace("'", "''") + "A' ";

        if (txtUID.Text.Trim() != "") whereClause = whereClause + " and fl_id like '%" + txtUID.Text.Replace("'", "''") + "%' ";

        tab1.Rows.Clear();
        tab1.BorderColor = ConfigurationManager.AppSettings["gridBorderColor"];
        tab1.Border = 1;
        tab1.CellPadding = 0;
        tab1.CellSpacing = 0;
        tab1.Rows.Add(new HtmlTableRow());
        tab1.Rows[0].Attributes.Add("class", "head");
        tab1.Rows[0].Cells.Add(new HtmlTableCell());
        tab1.Rows[0].Cells[0].Width = "15%";
        tab1.Rows[0].Cells[0].InnerHtml = "<center>รหัสผู้ใช้งาน</center>";
        tab1.Rows[0].Cells.Add(new HtmlTableCell());
        tab1.Rows[0].Cells[1].Width = "15%";
        tab1.Rows[0].Cells[1].InnerHtml = "<center>โปรแกรม</center>";
        tab1.Rows[0].Cells.Add(new HtmlTableCell());
        tab1.Rows[0].Cells[2].Width = "15%";
        tab1.Rows[0].Cells[2].InnerHtml = "<center>การกระทำ</CENTER>";
        tab1.Rows[0].Cells.Add(new HtmlTableCell());
        tab1.Rows[0].Cells[3].Width = "15%";
        tab1.Rows[0].Cells[3].InnerHtml = "<center>คำสำคัญ</center>";
        tab1.Rows[0].Cells.Add(new HtmlTableCell());
        tab1.Rows[0].Cells[4].Width = "15%";
        tab1.Rows[0].Cells[4].InnerHtml = "<center>วันที่</center>";
        tab1.Rows[0].Cells.Add(new HtmlTableCell());
        tab1.Rows[0].Cells[5].Width = "10%";
        tab1.Rows[0].Cells[5].InnerHtml = "<center>เครื่อง</center>";

        OleDbConnection Conn = new OleDbConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString);
        OleDbCommand command = new OleDbCommand();

        string sql = "SELECT fl_id,fl_module,fl_action,fl_keyword,fl_datetime,fl_ip ";
        sql = sql + " from tb_log " + whereClause + " ORDER by fl_datetime, fl_id ";

        Conn.Open();

        command.CommandText = sql;
        userDataSQL.Value = command.CommandText;
        command.Connection = Conn;
        OleDbDataReader rs = command.ExecuteReader();

        int i = 1;
        while (rs.Read())
        {
            tab1.Rows.Add(new HtmlTableRow());
            i = tab1.Rows.Count - 1;
            tab1.Rows[i].Attributes.Add("class", "off");
            tab1.Rows[i].Attributes.Add("onmouseover", "this.className='on'");
            tab1.Rows[i].Attributes.Add("onmouseout", "this.className='off'");
            tab1.Rows[i].Cells.Add(new HtmlTableCell());
            tab1.Rows[i].Cells[0].Width = "15%";
            tab1.Rows[i].Cells[0].ColSpan = 1;
            tab1.Rows[i].Cells[0].InnerHtml = "&nbsp;" + rs.GetString(0) + "<input type='hidden' name ='login" + i + "' value='" + rs.GetString(0) + "'>";

            tab1.Rows[i].Cells.Add(new HtmlTableCell());
            tab1.Rows[i].Cells[1].Width = "15%";
            tab1.Rows[i].Cells[1].Align = "CENTER";
            tab1.Rows[i].Cells[1].InnerHtml = "" + rs.GetString(1) + "<input type='hidden' name ='module" + i + "' value='" + rs.GetString(1) + "'>";

            tab1.Rows[i].Cells.Add(new HtmlTableCell());
            tab1.Rows[i].Cells[2].Width = "20%";
            tab1.Rows[i].Cells[2].Align = "CENTER";
            tab1.Rows[i].Cells[2].InnerHtml = "" + rs.GetString(2) + "<input type='hidden' name ='action" + i + "' value='" + rs.GetString(2) + "'>";

            tab1.Rows[i].Cells.Add(new HtmlTableCell());
            tab1.Rows[i].Cells[3].Width = "10%";
            tab1.Rows[i].Cells[3].Align = "CENTER";
            tab1.Rows[i].Cells[3].InnerHtml = "" + rs.GetString(3) + "&nbsp;<input type='hidden' name ='key" + i + "' value='" + rs.GetString(3) + "'>";

            tab1.Rows[i].Cells.Add(new HtmlTableCell());
            tab1.Rows[i].Cells[4].Width = "10%";
            tab1.Rows[i].Cells[4].Align = "CENTER";
            tmpString = rs.GetString(4);
            tmpString = tmpString.Substring(6, 2) + "/" + tmpString.Substring(4, 2) + "/" + tmpString.Substring(0, 4) + " " + tmpString.Substring(8, 2) + ":" + tmpString.Substring(10, 2) + ":" + tmpString.Substring(12, 2);
            tab1.Rows[i].Cells[4].InnerHtml = "" + tmpString + "<input type='hidden' name ='dt" + i + "' value='" + tmpString + "'>";
        
            tab1.Rows[i].Cells.Add(new HtmlTableCell());
            tab1.Rows[i].Cells[5].Width = "10%";
            tab1.Rows[i].Cells[5].Align = "CENTER";
            tab1.Rows[i].Cells[5].InnerHtml = "" + rs.GetString(5) + "<input type='hidden' name ='mac" + i + "' value='" + rs.GetString(5) + "'>";

            i = i + 1;
        }
        maxi.Value = i.ToString();
        rs.Close();
        if ((i - 1) <= 0)
        {
            //noDataFound
            tab1.Rows.Add(new HtmlTableRow());
            tab1.Rows[i].Attributes.Add("class", "off");
            tab1.Rows[i].Attributes.Add("onmouseover", "this.className='on'");
            tab1.Rows[i].Attributes.Add("onmouseout", "this.className='off'");
            tab1.Rows[i].Cells.Add(new HtmlTableCell());
            tab1.Rows[i].Cells[0].ColSpan = 6;
            tab1.Rows[i].Cells[0].Align = "CENTER";
            tab1.Rows[i].Cells[0].InnerHtml = "<font color='red' size='4'><b>" + ConfigurationManager.AppSettings["nodataMsg"] + "</b></font><input type='hidden' name ='cust1' value='" + ConfigurationManager.AppSettings["nodataMsg"] + "'>";
        }
        Conn.Close();
        bar.Visible = true;
		btnExport.Visible=true;
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        userDataSet();
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        csvPrint();
    }

    protected void csvPrint()
    {
        OleDbConnection Conn = new OleDbConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString);
        OleDbCommand command = new OleDbCommand();

        Conn.Open();
        command.CommandText = userDataSQL.Value;
        command.Connection = Conn;
        
        string fName=DateTime.Now.Year+DateTime.Now.Month.ToString().PadLeft(2,'0')+DateTime.Now.Day.ToString().PadLeft(2,'0')+DateTime.Now.Hour.ToString().PadLeft(2,'0')+DateTime.Now.Minute.ToString().PadLeft(2,'0')+DateTime.Now.Second.ToString().PadLeft(2,'0');
        fName = fName + "_" + Session["uID"].ToString() + "_audit";
        if (fName.Length > 100) fName = fName.Substring(0, 100);
        fName = fName + ".csv";
        
        System.IO.StreamWriter sw = new System.IO.StreamWriter(Server.MapPath("CSV") + "\\" + fName,false,Encoding.Default);
        OleDbDataReader rs = command.ExecuteReader();
        string tmpLine = "";
        tmpLine = tmpLine + "\"รหัสผู้ใช้งาน\",\"โปรแกรม\",\"การทำงาน\",\"คำสำคัญ\",\"วัน-เวลา\",\"เครื่องใช้งาน\"";
        sw.WriteLine(tmpLine);
        while (rs.Read())
        {
            tmpLine = "";
            for (int i = 0; i <= 5; i++)
            {
                if(i==4)
                {
                    string tmpString = rs.GetString(i);
                    tmpString = tmpString.Substring(6, 2) + "/" + tmpString.Substring(4, 2) + "/" + tmpString.Substring(0, 4) + " " + tmpString.Substring(8, 2) + ":" + tmpString.Substring(10, 2) + ":" + tmpString.Substring(12, 2);
                    tmpLine = tmpLine + "\"" + tmpString + "\"";
                }else
                {
                    if (!rs.IsDBNull(i)) tmpLine = tmpLine + "\"" + rs.GetString(i) + "\"";
                }
                if (i < 5) tmpLine = tmpLine + ",";
            }
            sw.WriteLine(tmpLine);
        }
        rs.Close();
        sw.Close();
        Conn.Close();

        Response.Redirect("CSV/" + fName);
    }
}