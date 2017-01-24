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
using System.Data.OleDb;

public partial class _user_entry: System.Web.UI.Page
{
    protected void deptDataSet()
    {
        cmbDept.Items.Clear();
        cmbDept.Items.Add(new ListItem("ไม่กำหนด", ""));

        OleDbConnection Conn = new OleDbConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString);
        OleDbCommand command = new OleDbCommand();

        string sql = "SELECT distinct fl_dept_id,fl_dept_name from tb_dept where fl_dept_status='1' ";
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
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if ((Session["uName"] == null) || (Session["uName"].ToString() == ""))
            {
                Response.Redirect("login.aspx");
            }
        }catch(Exception ex)
        {
            Response.Redirect("login.aspx");
            return;
        }

        if (!Page.IsPostBack)
        {            
            if (Session["uGroup"].ToString() != "A") dtGrid.Visible=false;
            if (Session["uGroup"].ToString() != "A") searchBar.Visible = false;


			if (Session["uGroup"].ToString() != "A")
			{
				CheckBox6.Visible = false;

				txtPass1.Visible = false;
				txtPass2.Visible = false;

				lblPass1.Visible = false;
				lblPass2.Visible = false;

				lblS1.Visible = false;
				lblS2.Visible = false;

                cmbDept.Visible = false;
                cmbRight.Visible = false;

                lblDept.Visible = false;
                lblRight.Visible = false;
			}

            if (Session["uGroup"].ToString() != "A")
            {
                boxSet(Session["uID"].ToString());
                txtEmail.Enabled = false;
            }
            else
            {
                deptDataSet();
                userDataSet();
                if (Request.QueryString["uID"] != null) boxSet(Request.QueryString["uID"].ToString());
            }
        }
        if (Session["alert"].ToString().Trim() != "") Response.Write("<font color='red'>"+Session["alert"].ToString().Trim()+"</Font>");
        Session["alert"] = "";

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
        string sql="SELECT fl_user_id, fl_user_Name,fl_user_status FROM tb_User ";
        sql = sql + " where 1=1 ";
        //Check filter command
        string[] tmpStringToken;
        if (txtKeyword.Text.Trim() != "")
        {
            tmpStringToken = txtKeyword.Text.Split(' ');
            foreach (string tmp in tmpStringToken)
            {
                sql = sql + " and (";
                sql = sql + " fl_user_id like '%" + tmp.Replace("'", "''") + "%' ";
                sql = sql + " or fl_user_name like '%" + tmp.Replace("'", "''") + "%' ";
                sql = sql + " or fl_citizen like '%" + tmp.Replace("'", "''") + "%' ";
                sql = sql + ") ";
            }
        }
        sql = sql + " ORDER BY fl_user_id, fl_user_Name";

        dtGrid.Rows.Clear();
        dtGrid.BorderColor = ConfigurationManager.AppSettings["gridBorderColor"];
        dtGrid.Border = 1;
        dtGrid.CellPadding = 0;
        dtGrid.CellSpacing = 0;
        dtGrid.Rows.Add(new HtmlTableRow());
        dtGrid.Rows[0].Attributes.Add("class", "head");
        dtGrid.Rows[0].Cells.Add(new HtmlTableCell());
        dtGrid.Rows[0].Cells[0].InnerHtml = "<center>แก้ไข</center>";
        dtGrid.Rows[0].Cells[0].Width = "1%";
        dtGrid.Rows[0].Cells.Add(new HtmlTableCell());
        dtGrid.Rows[0].Cells[1].Width = "15%";
        dtGrid.Rows[0].Cells[1].InnerHtml = "<center>รหัสผู้ใช้งาน</center>";
        dtGrid.Rows[0].Cells.Add(new HtmlTableCell());
        dtGrid.Rows[0].Cells[2].Width = "10%";
        dtGrid.Rows[0].Cells[2].InnerHtml = "<center>ชื่อ-สกุล</center>";
        
        OleDbConnection Conn = new OleDbConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString);
        OleDbCommand command = new OleDbCommand();

        Conn.Open();
        command.CommandText = sql;
        command.Connection = Conn;

        int i = 1;
        
        OleDbDataReader rs = command.ExecuteReader();
        while (rs.Read()) 
        {
            string fl_user_id = rs.GetString(0);
            string fl_name = rs.GetString(1);
            string fl_disable = rs.GetString(2);

            dtGrid.Rows.Add(new HtmlTableRow());
            if (fl_disable == "1")
            {
                dtGrid.Rows[i].Attributes.Add("class", "off");
                dtGrid.Rows[i].Attributes.Add("onmouseover", "this.className='on'");
                dtGrid.Rows[i].Attributes.Add("onmouseout", "this.className='off'");
            }
            else
            {
                dtGrid.Rows[i].Attributes.Add("class", "close");
                dtGrid.Rows[i].Attributes.Add("onmouseover", "this.className='on'");
                dtGrid.Rows[i].Attributes.Add("onmouseout", "this.className='close'");
            }
            dtGrid.Rows[i].Cells.Add(new HtmlTableCell());
            dtGrid.Rows[i].Cells[0].Width = "1%";
            dtGrid.Rows[i].Cells[0].ColSpan = 1;
            dtGrid.Rows[i].Cells[0].Align = "CENTER";
            string action = "document.location='user_entry.aspx?uID=" + fl_user_id + "';";

            if (Request.QueryString["uID"] != null)
            {
                if (Request.QueryString["uID"].ToString() != fl_user_id) dtGrid.Rows[i].Cells[0].InnerHtml = "<input type='checkbox' id='check_" + i.ToString() + "' onClick=\"" + action + "\">"; else dtGrid.Rows[i].Cells[0].InnerHtml = "<input type='checkbox' id='check_" + i.ToString() + "' checked onClick=\"" + action + "\">";
            }
            else
            {
                dtGrid.Rows[i].Cells[0].InnerHtml = "<input type='checkbox' id='check_" + i.ToString() + "' onClick=\""+ action + "\">";
            }
            dtGrid.Rows[i].Cells.Add(new HtmlTableCell());
            dtGrid.Rows[i].Cells[1].Width = "10%";
            dtGrid.Rows[i].Cells[1].ColSpan = 1;
            dtGrid.Rows[i].Cells[1].Align = "Left";
            dtGrid.Rows[i].Cells.Add(new HtmlTableCell());
            dtGrid.Rows[i].Cells[2].Width = "15%";
            dtGrid.Rows[i].Cells[2].Align = "Left";

            if (fl_disable == "1")
            {
                dtGrid.Rows[i].Cells[1].InnerHtml = "&nbsp;" + fl_user_id;
                dtGrid.Rows[i].Cells[2].InnerHtml = "&nbsp;" + fl_name;
            }
            else
            {
                dtGrid.Rows[i].Cells[1].InnerHtml = "&nbsp;<font color=red>" + fl_user_id + "</font>";
                dtGrid.Rows[i].Cells[2].InnerHtml = "&nbsp;<font color=red>" + fl_name + "</font>";
            }            
            i = i + 1;
        }
        if (i ==1)
        {
            //noDataFound
            dtGrid.Rows.Add(new HtmlTableRow());
            dtGrid.Rows[i].Attributes.Add("class", "off");
            dtGrid.Rows[i].Attributes.Add("onmouseover", "this.className='on'");
            dtGrid.Rows[i].Attributes.Add("onmouseout", "this.className='off'");
            dtGrid.Rows[i].Cells.Add(new HtmlTableCell());
            dtGrid.Rows[i].Cells[0].ColSpan = 3;
            dtGrid.Rows[i].Cells[0].Align = "CENTER";
            dtGrid.Rows[i].Cells[0].InnerHtml = "<font color='red' size='4'><b>" + ConfigurationManager.AppSettings["nodataMsg"] + "</b></font>";
        }
        rs.Close();
        Conn.Close();
    }

    protected void boxSet(string id)
    {
        OleDbConnection Conn = new OleDbConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString);
        OleDbCommand command = new OleDbCommand();

        string sql = " SELECT distinct fl_user_id,fl_user_name, ";
		sql = sql + " fl_user_group, ";
        sql = sql + " fl_user_dept, ";
        sql = sql + " fl_user_status, ";
        sql = sql + " fl_citizen ";
        sql = sql + " from tb_user ";
        sql = sql + " where fl_user_id = '" + id.Replace("'", "''") + "' ";

        Conn.Open();
        command.CommandText = sql;
        command.Connection = Conn;


        //Re-init box
        txtEmail.Text="";
        txtName.Text="";
        txtPass1.Text="";
        txtPass2.Text="";
        cmbRight.SelectedIndex = 0;
        cmbDept.SelectedValue = "";

        CheckBox6.Checked = false;
        
		txtEmail.Enabled=true;


        OleDbDataReader rs = command.ExecuteReader();
        while (rs.Read())
        {
            if (!rs.IsDBNull(0)) txtEmail.Text = rs.GetString(0);
            if (!rs.IsDBNull(1)) txtName.Text = rs.GetString(1);
            if (!rs.IsDBNull(3)) cmbDept.SelectedValue = rs.GetString(3);
            if (!rs.IsDBNull(2)) cmbRight.SelectedValue = rs.GetString(2);
            if (!rs.IsDBNull(4)) if (rs.GetString(4) != "1") CheckBox6.Checked = true;
            
            txtEmail.Enabled=false;
        }
        rs.Close();
        Conn.Close();

        lblResponse.Text = "";

        lblResponse1.Text = "";
        lblResponse2.Text = "";
        lblResponse3.Text = "";
        lblResponse4.Text = "";

        lblResponse1.Visible = false;
        lblResponse2.Visible = false;
        lblResponse3.Visible = false;
        lblResponse4.Visible = false;
        btnUpdate.Focus();
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        lblResponse.Text = "";

        lblResponse1.Text = "";
        lblResponse2.Text = "";
        lblResponse3.Text = "";
        lblResponse4.Text = "";

        lblResponse1.Visible = false;
        lblResponse2.Visible = false;
        lblResponse3.Visible = false;
        lblResponse4.Visible = false;

        bool errFound = false;
        if ((txtEmail.Text.Trim() == "") || (txtEmail.Text.IndexOf("'") > 0) || (txtEmail.Text.IndexOf("\"") > 0))
        {
            lblResponse1.Text = "รหัสผู้ใช้งานไม่ถูกต้อง";
            lblResponse1.ForeColor = System.Drawing.Color.Red;
            lblResponse1.Visible = true;
            errFound = true;
        }

        if (txtName.Text.Trim() == "")
        {
            lblResponse2.Text = "บันทึกชื่อ-สกุลผู้ใช้งาน";
            lblResponse2.ForeColor = System.Drawing.Color.Red;
            lblResponse2.Visible = true;
            errFound = true;
        }

        string tmpPass = "";
        if (txtPass1.Visible)
        {
            if (txtPass1.Text != "")
            {
				if (txtPass1.Text.Length<8)
				{
					lblResponse3.Text = "รหัสผ่านสั้นกว่า 8 อักษร";
					lblResponse3.ForeColor = System.Drawing.Color.Red;
					lblResponse3.Visible = true;
					errFound = true;
				}

				if (txtPass2.Text == "")
				{
					lblResponse4.Text = "บันทึกรหัสผ่านซ้ำ";
					lblResponse4.ForeColor = System.Drawing.Color.Red;
					lblResponse4.Visible = true;
					errFound = true;
				}

				if (txtPass2.Text.Length<8)
				{
					lblResponse4.Text = "รหัสผ่านสั้นกว่า 8 อักษร";
					lblResponse4.ForeColor = System.Drawing.Color.Red;
					lblResponse4.Visible = true;
					errFound = true;
				}

				if (txtPass1.Text == txtPass2.Text) 
				{
					tmpPass = txtPass1.Text;
				}
				else
				{
					lblResponse4.Text = "รหัสผ่านไม่ตรงกัน";
					lblResponse4.ForeColor = System.Drawing.Color.Red;
					lblResponse4.Visible = true;
					errFound = true;
				}
			}
        }


        if (errFound)
        {
            if (Session["uGroup"].ToString() == "A") userDataSet();
            btnUpdate.Focus();
            return;
        }

        OleDbConnection Conn = new OleDbConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString);
        OleDbCommand command = new OleDbCommand();

        Conn.Open();
        command.Connection = Conn;

        string sql = "SELECT fl_user_id from tb_USER ";
        sql = sql + " where fl_user_id='" + txtEmail.Text.Replace("'", "''").Trim() + "' ";
        command.CommandText = sql;

        string processPath = "IN";
		if(!txtEmail.Enabled) processPath="UP";
        OleDbDataReader rs = command.ExecuteReader();
        if (rs.Read()) 
		{
	        if (processPath == "IN") processPath = "NA";
		}
        rs.Close();

        if (processPath == "IN")
        {
            if ((txtEmail.Text == Session["uID"].ToString()) || (Session["uGroup"].ToString()=="A"))
            {
                sql = "INSERT INTO tb_USER(fl_user_id,fl_user_name,";
                if (tmpPass.Trim() != "") sql = sql + "fl_user_password,";
                sql = sql + " fl_user_group, ";
                sql = sql + " fl_user_dept, ";
                sql = sql + " fl_last_login, ";
                sql = sql + " fl_create_by, ";
                sql = sql + " fl_create_ip, ";
                sql = sql + " fl_create_time, ";
                sql = sql + " fl_update_by, ";
                sql = sql + " fl_update_ip, ";
                sql = sql + " fl_update_time, ";
                sql = sql + " fl_user_status, ";
                sql = sql + " fl_citizen ";
                sql = sql + ")VALUES(";
                sql = sql + "'" + txtEmail.Text.Replace("'", "''").Replace(";","").Trim() + "', ";
                sql = sql + "'" + txtName.Text.Replace("'", "''").Replace(";", "").Trim() + "', ";
                if (tmpPass.Trim() != "") sql = sql + tmpPass.GetHashCode() + ", ";
                sql = sql + "'" + cmbRight.SelectedValue + "', ";
                sql = sql + "'" + cmbDept.SelectedValue + "', ";
                sql = sql + "'',";
                sql = sql + "'" + Session["uID"].ToString().Replace("'", "''") + "', ";
                sql = sql + "'" + Request.UserHostAddress + "', ";
                sql = sql + "'" + DateTime.Now.Year + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0') + DateTime.Now.Hour.ToString().PadLeft(2, '0') + DateTime.Now.Minute.ToString().PadLeft(2, '0') + DateTime.Now.Second.ToString().PadLeft(2, '0') + "', ";
                sql = sql + "'" + Session["uID"].ToString().Replace("'", "''") + "', ";
                sql = sql + "'" + Request.UserHostAddress + "', ";
                sql = sql + "'" + DateTime.Now.Year + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0') + DateTime.Now.Hour.ToString().PadLeft(2, '0') + DateTime.Now.Minute.ToString().PadLeft(2, '0') + DateTime.Now.Second.ToString().PadLeft(2, '0') + "', ";
                
				if (CheckBox6.Checked) sql = sql + "'0',"; else sql = sql + "'1',";
                sql = sql + "''); ";

                sql = sql + "INSERT INTO tb_LOG(fl_id,fl_module,fl_action,fl_keyword,fl_datetime,fl_ip) VALUES(";
                sql = sql + " '" + Session["uID"].ToString().Replace("'", "''") + "', ";
                sql = sql + " 'User Data', ";
                sql = sql + " 'INSERT', ";
                sql = sql + " '" + txtEmail.Text.Replace("'", "''").Trim() + "', ";
                sql = sql + " '" + DateTime.Now.Year + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0') + DateTime.Now.Hour.ToString().PadLeft(2, '0') + DateTime.Now.Minute.ToString().PadLeft(2, '0') + DateTime.Now.Second.ToString().PadLeft(2, '0') + "', ";
                sql = sql + " '" + Request.UserHostAddress + "'); ";

                command.CommandText = sql;
                command.ExecuteNonQuery();

                lblResponse.Text = "เพิ่มข้อมูลสำเร็จ";
                lblResponse.ForeColor = System.Drawing.Color.Green;
                txtEmail.Enabled = false;
            }
            else
            {
                lblResponse.Text = "ไม่สามารถดำเนินการได้";
                lblResponse.ForeColor = System.Drawing.Color.Red; 
            }
        }
        else
        {
            if (processPath == "UP")
            {
                sql = "UPDATE tb_USER SET ";
                sql = sql + " fl_user_Name='" + txtName.Text.Replace("'", "''").Trim() + "', ";
                if (tmpPass.Trim() != "") sql = sql + " fl_user_Password=" + tmpPass.GetHashCode() + ", ";

                sql = sql + "fl_user_group='" + cmbRight.SelectedValue + "', ";
                if (Session["uGroup"].ToString()=="A") sql = sql + "fl_user_dept='" + cmbDept.SelectedValue + "', ";
                sql = sql + "fl_create_by='" + Session["uID"].ToString().Replace("'", "''") + "', ";
                sql = sql + "fl_create_ip='" + Request.UserHostAddress + "', ";
                sql = sql + "fl_create_time='" + DateTime.Now.Year + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0') + DateTime.Now.Hour.ToString().PadLeft(2, '0') + DateTime.Now.Minute.ToString().PadLeft(2, '0') + DateTime.Now.Second.ToString().PadLeft(2, '0') + "', ";
                sql = sql + "fl_update_by='" + Session["uID"].ToString().Replace("'", "''") + "', ";
                sql = sql + "fl_update_ip='" + Request.UserHostAddress + "', ";
                sql = sql + "fl_update_time='" + DateTime.Now.Year + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0') + DateTime.Now.Hour.ToString().PadLeft(2, '0') + DateTime.Now.Minute.ToString().PadLeft(2, '0') + DateTime.Now.Second.ToString().PadLeft(2, '0') + "', ";
                                
				if (Session["uGroup"].ToString() == "A")
				{
                    if (CheckBox6.Checked) sql = sql + "fl_user_status='0',"; else sql = sql + "fl_user_status='1',";
				}else{
                    sql = sql + "fl_user_status='1',";
				}

                sql = sql + " fl_citizen='' ";
                sql = sql + " where fl_user_id='" + txtEmail.Text.Replace("'", "''") + "'; ";

                sql = sql + "INSERT INTO tb_LOG(fl_id,fl_module,fl_action,fl_keyword,fl_datetime,fl_ip) VALUES(";
                sql = sql + " '" + Session["uID"].ToString().Replace("'", "''") + "', ";
                sql = sql + " 'User Data', ";
                sql = sql + " 'UPDATE', ";
                sql = sql + " '" + txtEmail.Text.Trim().Replace("'", "''") + "', ";
                sql = sql + " '" + DateTime.Now.Year + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0') + DateTime.Now.Hour.ToString().PadLeft(2, '0') + DateTime.Now.Minute.ToString().PadLeft(2, '0') + DateTime.Now.Second.ToString().PadLeft(2, '0') + "', ";
                sql = sql + " '" + Request.UserHostAddress + "') ";
                command.CommandText = sql;
                command.ExecuteNonQuery();

                lblResponse.Text = "ปรับปรุงข้อมูลสำเร็จ";
                lblResponse.ForeColor = System.Drawing.Color.Green;
            }
            else
            {
                lblResponse.Text = "รหัสผู้ใช้งานซ้ำ: " + txtEmail.Text;
                lblResponse.ForeColor = System.Drawing.Color.Red;
            }
        }
        Conn.Close();

        if (Session["alert"].ToString().Trim() != "")
        {
            lblResponse.Text = Session["alert"].ToString().Trim();
            lblResponse.ForeColor = System.Drawing.Color.Red;
        }

        Session["alert"] = "";

        if (Session["uGroup"].ToString() == "A") userDataSet();
        btnUpdate.Focus();
    }

    protected void brnClear_Click(object sender, EventArgs e)
    {
        boxSet(txtEmail.Text);
        if (Session["uGroup"].ToString() == "A")
        {
            //Re-init box
            txtEmail.Text = "";
            txtName.Text = "";
            txtPass1.Text = "";
            txtPass2.Text = "";

            CheckBox6.Checked = false;

            cmbRight.SelectedIndex = 0;
            cmbDept.SelectedValue = "";

            txtEmail.Enabled = true;
            userDataSet();
        }
        btnUpdate.Focus();
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        userDataSet();
    }
}