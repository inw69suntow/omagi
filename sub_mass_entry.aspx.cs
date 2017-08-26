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

public partial class _mass_entry: System.Web.UI.Page
{

    override protected void OnInit(EventArgs e)
    {
        btnImport.Attributes.Add("onclick",
                  this.GetPostBackEventReference(btnImport));
        base.OnInit(e);
    }

    protected void deptDataSet()
    {
        cmbDept.Items.Clear();
        cmbDeptSearch.Items.Clear();
        //cmbDept.Items.Add(new ListItem("ไม่กำหนด", ""));
        cmbDeptSearch.Items.Add(new ListItem("ทั้งหมด", ""));

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
            cmbDeptSearch.Items.Add(new ListItem(rs.GetString(1), rs.GetString(0)));
        }
        rs.Close();
        Conn.Close();

        if (Session["uGroup"].ToString() == "C") cmbDept.Enabled = false;
    }

    protected void provinceDataSet()
    {
        cmbProvince.Items.Clear();
        cmbProvinceSearch.Items.Clear();

        //cmbProvince.Items.Add(new ListItem("ไม่กำหนด", ""));
        cmbProvinceSearch.Items.Add(new ListItem("ไม่กำหนด", ""));

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
            cmbProvinceSearch.Items.Add(new ListItem(rs.GetString(1), rs.GetString(0)));
        }
        rs.Close();
        Conn.Close();
    }

    protected void districtDataSet()
    {
        cmbDistrict.Items.Clear();
        cmbDistrict.Items.Add(new ListItem("ไม่กำหนด", ""));

        OleDbConnection Conn = new OleDbConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString);
        OleDbCommand command = new OleDbCommand();

        string sql = "SELECT distinct fl_district_code,fl_district_name from tb_moicode where fl_status='1' ";
        sql += " and fl_province_code ='" + cmbProvince.SelectedValue.Trim() +"' ";
        sql += " order by fl_district_name ";

        Conn.Open();

        command.CommandText = sql;
        command.Connection = Conn;
        OleDbDataReader rs = command.ExecuteReader();
        while (rs.Read())
        {
            cmbDistrict.Items.Add(new ListItem(rs.GetString(1), rs.GetString(0)));
        }
        rs.Close();
        Conn.Close();
    }

    protected void tambonDataSet()
    {
        cmbTambon.Items.Clear();
        cmbTambon.Items.Add(new ListItem("ไม่กำหนด", ""));

        OleDbConnection Conn = new OleDbConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString);
        OleDbCommand command = new OleDbCommand();

        string sql = "SELECT distinct fl_tambon_code,fl_tambon_name from tb_moicode where fl_status='1' ";
        sql += " and fl_district_code ='" + cmbDistrict.SelectedValue.Trim() + "' ";
        sql += " order by fl_tambon_name ";

        Conn.Open();

        command.CommandText = sql;
        command.Connection = Conn;
        OleDbDataReader rs = command.ExecuteReader();
        while (rs.Read())
        {
            cmbTambon.Items.Add(new ListItem(rs.GetString(1), rs.GetString(0)));
        }
        rs.Close();
        Conn.Close();
    }

    protected void massDataSet()
    {
        cmbMassGroup.Items.Clear();
        //cmbMassGroup.Items.Add(new ListItem("ไม่กำหนด", ""));

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

    protected void clearBox()
    {
        hidID.Value = "";
        //cmbProvince.SelectedValue = "";
        //cmbDistrict.Items.Clear();
        //cmbDistrict.Items.Add(new ListItem("ไม่กำหนด", ""));
        //cmbTambon.Items.Clear();
        //cmbTambon.Items.Add(new ListItem("ไม่กำหนด", ""));
        //cmbMassGroup.SelectedValue = "";
        txtMassName.Text = "";
        lbParentName.Text = "";
        hdParentId.Value = "";
        txtReportAmount.Text = "";
        //cmbDept.SelectedValue="";
        txtGoogle.Text="";
        lblCreate.Text = "";
        lblUpdate.Text = "";

    }

    protected void boxSet(string hid)
    {
        clearBox();
        hidID.Value = hid.Trim();
        string sql = "SELECT distinct ";
        sql += " isnull(a.fl_province,''), ";
        sql += " isnull(a.fl_district,''), ";
        sql += " isnull(a.fl_tambon,''), ";
        sql += " isnull(a.fl_moo,''), ";
        sql += " isnull(a.fl_group_id,''), ";
        sql += " isnull(a.fl_groupname,''), ";
        sql += " isnull(a.fl_reportMember,''), ";
        sql += " isnull(a.fl_dept,''), ";
        sql += " isnull(a.fl_google,''), ";
        sql += " isnull(a.fl_create_time,''), ";
        sql += " isnull(a.fl_update_time,'') ";

        sql += " ,isnull(parent.fl_groupname,'') parentName ";
        sql += " ,isnull(parent.fl_id,'') parentId ";
        sql += " ,isnull(a.fl_id,'') parentId ";
        sql += " ,a.fl_level level ";

        sql += " from tb_detailgroup a ";
        sql += " left join tb_detailgroup parent on parent.fl_id = a.fl_parent_id ";
        sql += " where a.fl_id='" + hidID.Value + "' ";

        OleDbConnection Conn = new OleDbConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString);
        OleDbCommand command = new OleDbCommand();
        OleDbDataReader rs = null;
        try
        {
            Conn.Open();
            command.Connection = Conn;
            command.CommandText = sql.Replace(";", "");
            rs = command.ExecuteReader();
            if (rs.Read())
            {
                if (rs.GetString(0).Trim() != "") cmbProvince.SelectedValue = rs.GetString(0).Trim();
                districtDataSet();
                if (rs.GetString(1).Trim() != "") cmbDistrict.SelectedValue = rs.GetString(1).Trim();
                tambonDataSet();
                if (rs.GetString(2).Trim() != "") cmbTambon.SelectedValue = rs.GetString(2).Trim();
                txtMoo.Text = rs.GetString(3);

                if (rs.GetString(4).Trim() != "") cmbMassGroup.SelectedValue = rs.GetString(4).Trim();
                txtMassName.Text = rs.GetString(5).Trim();

                txtReportAmount.Text = rs.GetString(6).Trim();
                if (rs.GetString(7).Trim() != "") cmbDept.SelectedValue = rs.GetString(7).Trim();
                txtGoogle.Text = rs.GetString(8).Trim();

                if (rs.GetString(9) != "") lblCreate.Text = rs.GetString(9).Substring(6, 2) + "-" + rs.GetString(9).Substring(4, 2) + "-" + rs.GetString(9).Substring(0, 4) + " " + rs.GetString(9).Substring(8, 2) + ":" + rs.GetString(9).Substring(10, 2) + ":" + rs.GetString(9).Substring(12, 2);
                if (rs.GetString(10) != "") lblUpdate.Text = rs.GetString(10).Substring(6, 2) + "-" + rs.GetString(10).Substring(4, 2) + "-" + rs.GetString(10).Substring(0, 4) + " " + rs.GetString(10).Substring(8, 2) + ":" + rs.GetString(10).Substring(10, 2) + ":" + rs.GetString(10).Substring(12, 2);

                if (rs["parentName"] != null)
                {
                    lbParentName.Text = rs["parentName"].ToString();
                }
                if (rs["parentId"] != null)
                {
                    hdParentId.Value = rs["parentId"].ToString();
                }
                if (rs["parentId"] != null)
                {
                    hdParentId.Value = rs["parentId"].ToString();
                }
                if (rs["level"] != null)
                {
                    ddProjectLevel.SelectedValue = rs["level"].ToString();
                }
            }
        }
        catch (Exception e)
        {
            throw e;
        }
        finally
        {
            if (rs != null)
            {
                rs.Close();
            }
           
            Conn.Close();

        }
       
        cmbDeptSearch.SelectedValue = cmbDept.SelectedValue;
        cmbProvinceSearch.SelectedValue = cmbProvince.SelectedValue;
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

        //Inject Client Script
        HtmlGenericControl body = (HtmlGenericControl)Master.FindControl("masterBody");
        body.Attributes.Add("onload", "initialize()");
        if (!Page.IsPostBack)
        {
            deptDataSet();
            provinceDataSet();
            massDataSet();
            if (Request.QueryString["hid"] != null)
            {
                if (Request.QueryString["hid"].Trim() != "")
                {
                    boxSet(Request.QueryString["hid"].ToString());
                }
            }


            //project level
            ddProjectLevel.DataSource = new String[] { "1", "2" };
            ddProjectLevel.DataBind();
            String parent = Request.Params["parent_id"];
            this.hdParentId.Value = parent == null ? "" : parent.Trim();

            userDataSet();
        }
        else
        {
            this.hdParentId.Value = "";
        }
        if (Session["uGroup"].ToString() == "C") btnSave.Enabled = false;
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
        dtGrid.Rows[0].Cells[0].InnerHtml = "แก้ไข";
        dtGrid.Rows[0].Cells[0].Width = "1%";
        dtGrid.Rows[0].Cells.Add(new HtmlTableCell());
        dtGrid.Rows[0].Cells[1].Width = "25%";
        dtGrid.Rows[0].Cells[1].InnerHtml = "<center>โครงการหลักย่อย</center>";

        dtGrid.Rows[0].Cells.Add(new HtmlTableCell());
        dtGrid.Rows[0].Cells[2].Width = "5%";
        dtGrid.Rows[0].Cells[2].InnerHtml = "<center>ระดับโครงการย่อย</center>";

        dtGrid.Rows[0].Cells.Add(new HtmlTableCell());
        dtGrid.Rows[0].Cells[3].Width = "25%";
        dtGrid.Rows[0].Cells[3].InnerHtml = "<center>โครงการหลักหลัก</center>";

        dtGrid.Rows[0].Cells.Add(new HtmlTableCell());
        dtGrid.Rows[0].Cells[4].Width = "20%";
        dtGrid.Rows[0].Cells[4].InnerHtml = "<center>ประเภทโครงการ</center>"; 

        dtGrid.Rows[0].Cells.Add(new HtmlTableCell());
        dtGrid.Rows[0].Cells[5].Width = "15%";
        dtGrid.Rows[0].Cells[5].InnerHtml = "<center>จังหวัด</center>";

        dtGrid.Rows[0].Cells.Add(new HtmlTableCell());
        dtGrid.Rows[0].Cells[6].Width = "10%";
        dtGrid.Rows[0].Cells[6].InnerHtml = "<center>หน่วยงาน</center>";

        dtGrid.Rows[0].Cells.Add(new HtmlTableCell());
        dtGrid.Rows[0].Cells[7].Width = "4%";
        dtGrid.Rows[0].Cells[7].InnerHtml = "<center>จำนวนรายงาน (คน)</center>";

        string sql = "SELECT distinct a.fl_id, ";
        sql = sql + " isnull(a.fl_groupname,'') groupName ";
        //โครงการหลัก
        sql = sql + " ,case isnull(parent.fl_groupname,'') when '' then isnull(b.fl_group_name,'') else isnull(parent.fl_groupname,'') end parentName ";
        //เพิ่ม group name 
        sql = sql + " ,isnull(b.fl_group_id,'') mainGroupId, isnull(b.fl_group_name,'') mainGroupName, ";

        sql = sql + " case isnull(c.fl_province_name,'') when '' then isnull(a.fl_province,'') else isnull(c.fl_province_name,'') end provinceName,";
        sql = sql + " case isnull(d.fl_dept_name,'') when '' then isnull(a.fl_dept,'') else isnull(d.fl_dept_name,'') end deptName,";
        sql = sql + " cast('0' + isnull(a.fl_reportMember,0) as int) reportMember,  a.fl_dept ";
        
        //For sorting
        //sql = sql + " fl_dept ";
        
        
        sql = sql + " ,ISNULL(parent.fl_id,'') parentId ";
        sql = sql + " ,a.fl_level level ";
        sql = sql + " FROM tb_detailgroup a ";
        sql = sql + " left join tb_detailgroup parent on parent.fl_id = a.fl_parent_id ";
        sql = sql + " left join tb_maingroup b  on a.fl_group_id = b.fl_group_id ";
        sql = sql + " left join tb_moicode c on a.fl_province = c.fl_province_code ";
        sql = sql + " left join tb_dept d on a.fl_dept=d.fl_dept_id ";

        sql = sql + " where 1=1 ";
        // update โครงการลูกเท่านั้น
       // String parent= Request.Params.Get("parent_id");
       
        if (txtParentName.Text != null && txtParentName.Text.Trim() != "")
        {
            sql += " and parent.fl_groupname like \'%" + txtParentName.Text.Replace("'", "").Trim() + "%\' ";
          
        }
        else if (hdParentId.Value != null && !"".Equals(hdParentId.Value.Trim()))
        {
            sql += " and a.fl_parent_id=\'"+hdParentId.Value.Replace("'","").Trim()+"\' ";
         
        }
        else
        {
            sql += " and a.fl_parent_id is not null ";
        }
        //Check filter command
        if (cmbDeptSearch.SelectedValue.Trim() != "") sql = sql + " and isnull(a.fl_dept,'') ='" + cmbDeptSearch.SelectedValue.Trim() + "' ";
        if (cmbProvinceSearch.SelectedValue.Trim() != "") sql = sql + " and a.fl_province='" + cmbProvinceSearch.SelectedValue.Trim() + "' ";
        string[] tmpStringToken;
        if (txtKeyword.Text.Trim()!="")
        {
            tmpStringToken = txtKeyword.Text.Split(' ');
            foreach (string tmp in tmpStringToken)
            {
                sql = sql + " and (";
                sql = sql + " isnull(a.fl_groupname,'') like '%" + tmp.Replace("'", "''") + "%' ";
                sql = sql + " or isnull(b.fl_group_name,'') like '%" + tmp.Replace("'", "''") + "%' ";
                sql = sql + ") ";
            }
        }

        //Check Access Right
        //if (Session["uGroup"].ToString() == "U") sql = sql + " and isnull(a.fl_dept,'') like '" + Session["uDept"].ToString().Replace("0","") + "%' ";
        if (Session["uGroup"].ToString() == "U")
        {
            if (Session["uDept"].ToString().Substring(0) != "0") sql = sql + " and isnull(a.fl_dept,'') like '" + Session["uDept"].ToString().Replace("00", "") + "%' ";
        }
        //if (hidID.Value!= "") sql += " and fl_id='" + hidID.Value + "' ";
        OleDbConnection Conn = new OleDbConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString);
        OleDbCommand command = new OleDbCommand();

        Conn.Open();
        command.Connection = Conn;

        //Count Page        
        command.CommandText = "SELECT IsNULL(COUNT(fl_id),0) as c from (" + sql + ") olderS "; ;
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
        sql = sql + " fl_dept ASC, ";
        sql = sql + " reportMember DESC,";
        sql = sql + " groupname ASC, ";
        sql = sql + " provinceName ";

        int i = 0;
        command.CommandText = sql.Replace(";", "");
        Session["userSQL"]= sql.Replace(";", "");

        rs = command.ExecuteReader();
        while (i < (curPage-1) * pageSize) { rs.Read(); i++; }
        i = 1;
        while ((rs.Read()) && (i <= pageSize))
        {
            //string action = "document.location='mass_entry.aspx?hid=" + rs.GetString(0) + "';";
            string action = "document.location='sub_mass_entry.aspx?hid=" + rs["fl_id"] + "&parent_id=" + rs["parentId"] + "';";
            dtGrid.Rows.Add(new HtmlTableRow());
            if (rs.GetInt32(7) > 0)
            {
                dtGrid.Rows[i].Attributes.Add("class", "off");
                dtGrid.Rows[i].Attributes.Add("onmouseover", "this.className='on'");
                dtGrid.Rows[i].Attributes.Add("onmouseout", "this.className='off'");
              //  dtGrid.Rows[i].Attributes.Add("onclick", action);
            }
            else
            {
                dtGrid.Rows[i].Attributes.Add("class", "close");
                dtGrid.Rows[i].Attributes.Add("onmouseover", "this.className='on'");
                dtGrid.Rows[i].Attributes.Add("onmouseout", "this.className='close'");
               // dtGrid.Rows[i].Attributes.Add("onclick", action);
            }
            dtGrid.Rows[i].Cells.Add(new HtmlTableCell());
            dtGrid.Rows[i].Cells[0].ColSpan = 1;
            dtGrid.Rows[i].Cells[0].Align = "CENTER";

            if (hidID.Value != rs.GetString(0))
                dtGrid.Rows[i].Cells[0].InnerHtml = "<input type='checkbox' id='check_" + i.ToString() + "' onClick=\'checkboxClick(this,\"" + rs["fl_id"] +"\",\""+ rs["parentId"] + "\")\' />"; 
            else
                dtGrid.Rows[i].Cells[0].InnerHtml = "<input type='checkbox' id='check_" + i.ToString() + "' onClick=\'checkboxClick(this,\"" + rs["fl_id"] + "\",\"" + rs["parentId"] + "\")\' />";

    

            dtGrid.Rows[i].Cells.Add(new HtmlTableCell());
            dtGrid.Rows[i].Cells[1].Align = "LEFT";
            dtGrid.Rows[i].Cells[1].InnerHtml = "&nbsp;" + rs["groupName"];

            dtGrid.Rows[i].Cells.Add(new HtmlTableCell());
            dtGrid.Rows[i].Cells[2].Align = "CENTER";
            dtGrid.Rows[i].Cells[2].InnerHtml = "&nbsp;" + rs["level"];


            dtGrid.Rows[i].Cells.Add(new HtmlTableCell());
            dtGrid.Rows[i].Cells[3].Align = "LEFT";
            dtGrid.Rows[i].Cells[3].InnerHtml = "&nbsp;" + rs["parentName"];

            dtGrid.Rows[i].Cells.Add(new HtmlTableCell());
            dtGrid.Rows[i].Cells[4].Align = "LEFT";
            dtGrid.Rows[i].Cells[4].InnerHtml = "&nbsp;" + rs["mainGroupName"];


            dtGrid.Rows[i].Cells.Add(new HtmlTableCell());
            dtGrid.Rows[i].Cells[5].Align = "LEFT";
            dtGrid.Rows[i].Cells[5].InnerHtml = "&nbsp;" + rs["provinceName"];

            dtGrid.Rows[i].Cells.Add(new HtmlTableCell());
            dtGrid.Rows[i].Cells[6].Align = "LEFT";
            dtGrid.Rows[i].Cells[6].InnerHtml = "&nbsp;" + rs["deptName"];

    

            dtGrid.Rows[i].Cells.Add(new HtmlTableCell());
            dtGrid.Rows[i].Cells[7].Align = "RIGHT";
            dtGrid.Rows[i].Cells[7].InnerHtml =  ((Int32)rs["reportMember"]).ToString("#,##0");

            i = i + 1;
        }
        rs.Close();
        Conn.Close();

        if (i == 1)
        {
            //noDataFound
            dtGrid.Rows.Add(new HtmlTableRow());
            dtGrid.Rows[i].Attributes.Add("class", "off");
            dtGrid.Rows[i].Attributes.Add("onmouseover", "this.className='on'");
            dtGrid.Rows[i].Attributes.Add("onmouseout", "this.className='off'");
            dtGrid.Rows[i].Cells.Add(new HtmlTableCell());
            dtGrid.Rows[i].Cells[0].ColSpan = 5;
            dtGrid.Rows[i].Cells[0].Align = "CENTER";
            dtGrid.Rows[i].Cells[0].InnerHtml = "<font color='red' size='4'><b>" + ConfigurationManager.AppSettings["nodataMsg"] + "</b></font>";
        }
    }

    public void btnSearch_Click(object sender, EventArgs e)
    {
        clearBox();
        userDataSet();
    }
    protected void btnExport_Click(object sender, EventArgs e)
    {
        //Make CSV File
        OleDbConnection Conn = new OleDbConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString);
        OleDbCommand command = new OleDbCommand();

        Conn.Open();

        string sql = "";

        sql = "SELECT distinct ";
        sql = sql + " isnull(fl_id,''),";
        sql = sql + " isnull(fl_province_name,''), ";
        sql = sql + " isnull(fl_district_name,''), ";
        sql = sql + " isnull(fl_tambon_name,''), ";
        sql = sql + " isnull(fl_moo,''), ";
        sql = sql + " isnull(fl_dept_name,''), ";
        sql = sql + " isnull(b.fl_group_name,''), ";
        sql = sql + " isnull(a.fl_groupname,''), ";
        sql = sql + " cast(fl_reportMember as int), ";
        sql = sql + " isnull(fl_google,'') ";
        sql = sql + " from tb_detailgroup a  ";
        sql = sql + " left join tb_maingroup b  on a.fl_group_id = b.fl_group_id ";

        //Update 01-10-2012
        sql += " left outer join (";
        sql += " select distinct fl_dept_id,fl_dept_name ";
        sql += " from tb_dept ";
        sql += " ) dept on a.fl_dept=dept.fl_dept_id ";

        sql += " left outer join (";
        sql += " select distinct fl_province_code,fl_province_name ";
        sql += " from tb_moicode where fl_status='1' ";
        sql += " ) province on a.fl_province=province.fl_province_code ";

        sql += " left outer join (";
        sql += " select distinct fl_district_code,fl_district_name ";
        sql += " from tb_moicode where fl_status='1' ";
        sql += " ) district on a.fl_district=district.fl_district_code ";

        sql += " left outer join (";
        sql += " select distinct fl_tambon_code,fl_tambon_name ";
        sql += " from tb_moicode where fl_status='1' ";
        sql += " ) tambon on a.fl_tambon=tambon.fl_tambon_code ";

        sql = sql + " where 1=1 ";
        //Check filter command
        if (cmbDeptSearch.SelectedValue.Trim() != "") sql = sql + " and fl_dept='" + cmbDeptSearch.SelectedValue.Trim() + "' ";
        if (cmbProvinceSearch.SelectedValue.Trim() != "") sql = sql + " and fl_province='" + cmbProvinceSearch.SelectedValue.Trim() + "' ";
        string[] tmpStringToken;
        if (txtKeyword.Text.Trim() != "")
        {
            tmpStringToken = txtKeyword.Text.Split(' ');
            foreach (string tmp in tmpStringToken)
            {
                sql = sql + " and (";
                sql = sql + " isnull(a.fl_groupname,'') like '%" + tmp.Replace("'", "''") + "%' ";
                sql = sql + " or isnull(b.fl_group_name,'') like '%" + tmp.Replace("'", "''") + "%' ";
                sql = sql + ") ";
            }
        }

        //Check Access Right
        //if (Session["uGroup"].ToString() == "U") sql = sql + " and isnull(a.fl_dept,'') like '" + Session["uDept"].ToString().Replace("0", "") + "%' ";
        if (Session["uGroup"].ToString() == "U")
        {
            if (Session["uDept"].ToString().Substring(0) != "0") sql = sql + " and isnull(a.fl_dept,'') like '" + Session["uDept"].ToString().Replace("00", "") + "%' ";
        }
        if (hidID.Value != "") sql += " and a.fl_id='" + hidID.Value + "' ";
        command.CommandText = sql;
        command.Connection = Conn;

        string fName = DateTime.Now.Year + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0') + DateTime.Now.Hour.ToString().PadLeft(2, '0') + DateTime.Now.Minute.ToString().PadLeft(2, '0') + DateTime.Now.Second.ToString().PadLeft(2, '0');
        fName = fName + "_" + Session["uID"].ToString() + "_massGroup";
        if (fName.Length > 100) fName = fName.Substring(0, 100);
        fName = fName + ".csv";

        System.IO.StreamWriter sw = new System.IO.StreamWriter(Server.MapPath("CSV") + "\\" + fName, false, Encoding.Default);
        OleDbDataReader rs = command.ExecuteReader();
        string tmpLine = "";
        tmpLine = tmpLine + "\"รหัสกลุ่ม\",\"รหัสจังหวัด\",\"รหัสอำเภอ\",\"รหัสตำบล\",\"หมู่ที่\",\"รหัสหน่วยงาน\",\"รหัสกลุ่มหลัก\",\"ชื่อกลุ่ม\",\"จำนวนที่รายงาน\",\"รหัสพิกัด google\"";
        sw.WriteLine(tmpLine);
        while (rs.Read())
        {
            tmpLine = "";
            for (int i = 0; i <= 9; i++)
            {
                if (i == 8) tmpLine = tmpLine + "\"" + rs.GetInt32(i).ToString() + "\""; else tmpLine = tmpLine + "\"'" + rs.GetString(i) + "\"";
                if (i < 9) tmpLine = tmpLine + ",";
            }
            sw.WriteLine(tmpLine);
        }
        rs.Close();
        sw.Close();

        sql = "";
        sql += "INSERT INTO tb_LOG(fl_id,fl_module,fl_action,fl_keyword,fl_datetime,fl_ip) VALUES(";
        sql += " '" + Session["uID"].ToString().Replace("'", "''") + "', ";
        sql += " 'MASS GROUP', ";
        sql += " 'EXPORT', ";
        sql += " '', ";
        sql += " '" + DateTime.Now.Year + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0') + DateTime.Now.Hour.ToString().PadLeft(2, '0') + DateTime.Now.Minute.ToString().PadLeft(2, '0') + DateTime.Now.Second.ToString().PadLeft(2, '0') + "', ";
        sql += " '" + Request.UserHostAddress + "'); ";

        command.CommandText = sql;
        command.ExecuteNonQuery();
        Conn.Close();

        Response.Redirect("CSV/" + fName);
    }
    protected void cmbProvince_SelectedIndexChanged(object sender, EventArgs e)
    {
        districtDataSet();
        userDataSet();
    }
    protected void cmbDistrict_SelectedIndexChanged(object sender, EventArgs e)
    {
        tambonDataSet();
        userDataSet();
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (cmbMassGroup.SelectedValue == "")
        {
            lblResponse.Text = "ไม่มีชื่อกลุ่มมวลชน";
            lblResponse.ForeColor = System.Drawing.Color.Red;
            lblResponse.Visible = true;
            userDataSet();
            return;
        }

        if ((cmbMassGroup.SelectedValue == "97") || (cmbMassGroup.SelectedValue == "98") || (cmbMassGroup.SelectedValue == "99"))
        {
            if (txtMassName.Text.Trim() == "")
            {
                lblResponse.Text = "กลุ่มมวลชนนี้จะต้องมีชื่อกลุ่ม";
                lblResponse.ForeColor = System.Drawing.Color.Red;
                lblResponse.Visible = true;
                userDataSet();
                return;
            }
        }

        if (cmbProvince.SelectedValue == "")
        {
            lblResponse.Text = "ไม่มีข้อมูลจังหวัดพื้นที่ปฎิบัติการ";
            lblResponse.ForeColor = System.Drawing.Color.Red;
            lblResponse.Visible = true;
            userDataSet();
            return;
        }

        if (cmbDept.SelectedValue == "")
        {
            lblResponse.Text = "ไม่มีข้อมูลหน่วยงานผู้ดูแล";
            lblResponse.ForeColor = System.Drawing.Color.Red;
            lblResponse.Visible = true;
            userDataSet();
            return;
        }

        txtReportAmount.Text="0" + txtReportAmount.Text.Trim();
        try
        {
            int x = Convert.ToInt32(txtReportAmount.Text);
		txtReportAmount.Text=x.ToString();
        }
        catch (Exception ee)
        {
            txtReportAmount.Text = "0";
        }

        string sql = "";
        OleDbConnection Conn = new OleDbConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString);
        OleDbCommand command = new OleDbCommand();

        //Save Process
        if (hidID.Value != "")
        {
            sql = "UPDATE tb_detailGroup set ";
            sql += " fl_province = '" + cmbProvince.SelectedValue.Trim() + "', ";
            sql += " fl_district = '" + cmbDistrict.SelectedValue.Trim() + "', ";
            sql += " fl_tambon = '" + cmbTambon.SelectedValue.Trim() + "', ";
            sql += " fl_moo = '" + txtMoo.Text.Trim().Replace("'","''").Replace(";","") + "', ";
            sql += " fl_group_id = '" + cmbMassGroup.SelectedValue.Trim() + "', ";
            sql += " fl_groupname = '" + txtMassName.Text.Trim().Replace("'","''").Replace(";","") + "', ";
            sql += " fl_reportMember ='" + txtReportAmount.Text.Trim().Replace("'","''").Replace(";","") + "', ";
            sql += " fl_update_by ='" + Session["uID"].ToString().Trim().Replace("'","''") + "', ";
            sql += " fl_update_ip ='" + Request.UserHostAddress + "', ";
            sql += " fl_update_time ='" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString().PadLeft(2,'0') + DateTime.Now.Day.ToString().PadLeft(2,'0') + DateTime.Now.Hour.ToString().PadLeft(2,'0') + DateTime.Now.Minute.ToString().PadLeft(2,'0') + DateTime.Now.Second.ToString().PadLeft(2,'0') + "', ";
            sql += " fl_dept ='" + cmbDept.SelectedValue.Trim() + "', ";
            sql += " fl_google ='" + txtGoogle.Text.Trim().Replace("'","''").Replace(";","") + "' ";


            sql += " where fl_id = '" + hidID.Value.Trim() +"'; ";

            //Write Log
            sql += "INSERT INTO tb_LOG(fl_id,fl_module,fl_action,fl_keyword,fl_datetime,fl_ip) VALUES(";
            sql += " '" + Session["uID"].ToString().Replace("'", "''") + "', ";
            sql += " 'MASS GROUP', ";
            sql += " 'UPDATE', ";
            sql += " '" + hidID.Value.Trim() + "', ";
            sql += " '" + DateTime.Now.Year + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0') + DateTime.Now.Hour.ToString().PadLeft(2, '0') + DateTime.Now.Minute.ToString().PadLeft(2, '0') + DateTime.Now.Second.ToString().PadLeft(2, '0') + "', ";
            sql += " '" + Request.UserHostAddress + "'); ";
        }
        else
        {
            //Find new ID
            sql = "select distinct isnull(max(cast(fl_id as int)),0)+1 from tb_detailgroup ";
            Conn.Open();
            command.CommandText = sql;
            command.Connection = Conn;
            OleDbDataReader rs = command.ExecuteReader();
            int newID = 1;
            if (rs.Read()) newID = Convert.ToInt32(rs.GetValue(0));
            rs.Close();
            Conn.Close();

            hidID.Value = newID.ToString();

            sql = "INSERT INTO tb_detailGroup ( ";
            sql += " fl_id, ";
            sql += " fl_province, ";
            sql += " fl_district, ";
            sql += " fl_tambon, ";
            sql += " fl_moo, ";
            sql += " fl_group_id, ";
            sql += " fl_groupname, ";
            sql += " fl_reportMember, ";
            sql += " fl_update_by, ";
            sql += " fl_update_ip, ";
            sql += " fl_update_time, ";
            sql += " fl_create_by, ";
            sql += " fl_create_ip, ";
            sql += " fl_create_time, ";
            sql += " fl_dept, ";
            sql += " fl_google ";
            sql += " ,fl_parent_id ";
            sql += " ,fl_level ";
            sql += " ) VALUES ( ";

            sql += " '" + hidID.Value.Trim() + "', ";
            sql += " '" + cmbProvince.SelectedValue.Trim() + "', ";
            sql += " '" + cmbDistrict.SelectedValue.Trim() + "', ";
            sql += " '" + cmbTambon.SelectedValue.Trim() + "', ";
            sql += " '" + txtMoo.Text.Trim().Replace("'", "''").Replace(";", "") + "', ";
            sql += " '" + cmbMassGroup.SelectedValue.Trim() + "', ";
            sql += " '" + txtMassName.Text.Trim().Replace("'", "''").Replace(";", "") + "', ";
            sql += " '" + txtReportAmount.Text.Trim().Replace("'", "''").Replace(";", "") + "', ";
            sql += " '" + Session["uID"].ToString().Trim().Replace("'", "''") + "', ";
            sql += " '" + Request.UserHostAddress + "', ";
            sql += " '" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0') + DateTime.Now.Hour.ToString().PadLeft(2, '0') + DateTime.Now.Minute.ToString().PadLeft(2, '0') + DateTime.Now.Second.ToString().PadLeft(2, '0') + "', ";
            sql += " '" + Session["uID"].ToString().Trim().Replace("'", "''") + "', ";
            sql += " '" + Request.UserHostAddress + "', ";
            sql += " '" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0') + DateTime.Now.Hour.ToString().PadLeft(2, '0') + DateTime.Now.Minute.ToString().PadLeft(2, '0') + DateTime.Now.Second.ToString().PadLeft(2, '0') + "', ";
            sql += " '" + cmbDept.SelectedValue.Trim() + "', ";
            sql += " '" + txtGoogle.Text.Trim().Replace("'", "''").Replace(";", "") + "', "; 
            sql += " '" + hdParentId.Value.Trim().Replace("'", "") + "', " ;
            sql += " '"+ddProjectLevel.SelectedValue+"' ";
            sql+= ") ";

            //Write Log
            sql += "INSERT INTO tb_LOG(fl_id,fl_module,fl_action,fl_keyword,fl_datetime,fl_ip) VALUES(";
            sql += " '" + Session["uID"].ToString().Replace("'", "''") + "', ";
            sql += " 'MASS GROUP', ";
            sql += " 'INSERT', ";
            sql += " '" + hidID.Value.Trim() + "', ";
            sql += " '" + DateTime.Now.Year + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0') + DateTime.Now.Hour.ToString().PadLeft(2, '0') + DateTime.Now.Minute.ToString().PadLeft(2, '0') + DateTime.Now.Second.ToString().PadLeft(2, '0') + "', ";
            sql += " '" + Request.UserHostAddress + "'); ";
        }
        Conn.Open();
        command.CommandText = sql;
        command.Connection = Conn;
        command.ExecuteNonQuery();
        Conn.Close();

        lblResponse.Text = "บันทึกข้อมูลสำเร็จ";
        lblResponse.ForeColor = System.Drawing.Color.Green;
        lblResponse.Visible = true;

        userDataSet();
    }
    protected void btnMember_Click(object sender, EventArgs e)
    {
        if (hidID.Value != "") Response.Redirect("mass_member_entry.aspx?id=" + hidID.Value); else userDataSet();
    }
    protected void btnImport_Click(object sender, EventArgs e)
    {
        //Import
        if (importText.Value.Trim() != "")
        {
            OleDbConnection Conn = new OleDbConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString);
            OleDbCommand command = new OleDbCommand();

            Conn.Open();
            command.Connection = Conn;

            string[] rowToken = importText.Value.Trim().Split(',');
            string sql = "";

            string sql2 = "";
            OleDbDataReader rs;
            long newID = 0;
            sql2 = "SELECT isnull(MAX(cast(fl_id as int)),0) from tb_detailgroup ";
            command.CommandText = sql2;
            rs = command.ExecuteReader();
            if (rs.Read()) if (!rs.IsDBNull(0)) newID = Convert.ToInt64(rs.GetValue(0));
            rs.Close();

            foreach (string rowVal in rowToken)
            {
                string[] colToken = rowVal.Trim().Split('|');

                string procFlag = "UP";

                if (colToken.Length == 11)
                {

                    if (colToken[0].Trim().Replace(";", "").Replace("'", "") == "")
                    {
                        procFlag = "IN";
                    }
                    else
                    {
                        sql2 = "SELECT distinct fl_id from tb_detailgroup ";
                        sql2 += "where fl_id ='" + colToken[0].Trim().Replace(";", "").Replace("'", "") + "' ";
                        command.CommandText = sql2;
                        rs = command.ExecuteReader();
                        if (!rs.Read()) procFlag = "IN";
                        rs.Close();
                    }

                    if (procFlag == "IN")
                    {

                        newID = newID + 1;

                        if (sql != "") sql = sql + ";";

                        sql = "INSERT INTO tb_detailGroup ( ";
                        sql += " fl_id, ";
                        sql += " fl_province, ";
                        sql += " fl_district, ";
                        sql += " fl_tambon, ";
                        sql += " fl_moo, ";
                        sql += " fl_group_id, ";
                        sql += " fl_groupname, ";
                        sql += " fl_reportMember, ";
                        sql += " fl_update_by, ";
                        sql += " fl_update_ip, ";
                        sql += " fl_update_time, ";
                        sql += " fl_create_by, ";
                        sql += " fl_create_ip, ";
                        sql += " fl_create_time, ";
                        sql += " fl_dept, ";
                        sql += " fl_google ";

                        sql += " ) VALUES ( ";

                        sql = sql + "'" + newID.ToString() + "', ";
                        for (int x = 1; x < 10; x++)
                        {
                            if (x == 7)
                            {
                                try
                                {
                                    int xx = Convert.ToInt32(colToken[x].Trim().Replace(";", "").Replace("'", ""));
                                    sql = sql + "" + colToken[x].Trim().Replace(";", "").Replace("'", "") + ", ";
                                }
                                catch (Exception ee)
                                {
                                    sql = sql + "0, ";
                                }

                                //Append update/create date
                                sql += " '" + Session["uID"].ToString().Trim().Replace("'", "''") + "', ";
                                sql += " '" + Request.UserHostAddress + "', ";
                                sql += " '" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0') + DateTime.Now.Hour.ToString().PadLeft(2, '0') + DateTime.Now.Minute.ToString().PadLeft(2, '0') + DateTime.Now.Second.ToString().PadLeft(2, '0') + "', ";
                                sql += " '" + Session["uID"].ToString().Trim().Replace("'", "''") + "', ";
                                sql += " '" + Request.UserHostAddress + "', ";
                                sql += " '" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0') + DateTime.Now.Hour.ToString().PadLeft(2, '0') + DateTime.Now.Minute.ToString().PadLeft(2, '0') + DateTime.Now.Second.ToString().PadLeft(2, '0') + "' ";
                            }
                            else
                            {
                                sql = sql + "'" + colToken[x].Trim().Replace(";", "").Replace("'", "") + "' ";
                            }

                            if (x < 9) sql = sql + ","; else sql = sql + ")";
                        }
                    }
                    else
                    {
                        if (sql != "") sql = sql + ";";

                        sql = sql + "UPDATE tb_detailgroup SET ";
                        sql = sql + " fl_province='" + colToken[1].Trim().Replace(";", "").Replace("'", "") + "', ";
                        sql = sql + " fl_district='" + colToken[2].Trim().Replace(";", "").Replace("'", "") + "', ";
                        sql = sql + " fl_tambon='" + colToken[3].Trim().Replace(";", "").Replace("'", "") + "', ";
                        sql = sql + " fl_moo='" + colToken[4].Trim().Replace(";", "").Replace("'", "") + "', ";
                        sql = sql + " fl_dept='" + colToken[5].Trim().Replace(";", "").Replace("'", "") + "', ";
                        sql = sql + " fl_group_id='" + colToken[6].Trim().Replace(";", "").Replace("'", "") + "', ";
                        sql = sql + " fl_groupname='" + colToken[7].Trim().Replace(";", "").Replace("'", "") + "', ";
                        sql = sql + " fl_reportMember='" + colToken[8].Trim().Replace(";", "").Replace("'", "") + "', ";
                        sql += " fl_update_by ='" + Session["uID"].ToString().Trim().Replace("'", "''") + "', ";
                        sql += " fl_update_ip ='" + Request.UserHostAddress + "', ";
                        sql += " fl_update_time ='" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0') + DateTime.Now.Hour.ToString().PadLeft(2, '0') + DateTime.Now.Minute.ToString().PadLeft(2, '0') + DateTime.Now.Second.ToString().PadLeft(2, '0') + "', ";
                        sql = sql + " fl_google='" + colToken[9].Trim().Replace(";", "").Replace("'", "") + "' ";
                        sql = sql + " where fl_id='" + colToken[0].Trim().Replace(";", "").Replace("'", "") + "' ";
                    }
                }
            }
            if (sql != "") sql = sql + ";";
            sql += "INSERT INTO tb_LOG(fl_id,fl_module,fl_action,fl_keyword,fl_datetime,fl_ip) VALUES(";
            sql += " '" + Session["uID"].ToString().Replace("'", "''") + "', ";
            sql += " 'MASS GROUP', ";
            sql += " 'IMPORT', ";
            sql += " '', ";
            sql += " '" + DateTime.Now.Year + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0') + DateTime.Now.Hour.ToString().PadLeft(2, '0') + DateTime.Now.Minute.ToString().PadLeft(2, '0') + DateTime.Now.Second.ToString().PadLeft(2, '0') + "', ";
            sql += " '" + Request.UserHostAddress + "'); ";

            command.CommandText = sql;
            command.ExecuteNonQuery();
            Conn.Close();
        }

        lblResponse.Text = "นำเข้าข้อมูลสำเร็จ";
        lblResponse.ForeColor = System.Drawing.Color.Green;
        lblResponse.Visible = true;

        userDataSet();
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        clearBox();
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

    protected void btnDel_Click(object sender, EventArgs e)
    {
        OleDbConnection Conn = new OleDbConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString);
        OleDbCommand command = new OleDbCommand();
        Conn.Open();
        command.Connection = Conn;
        string sql = "DELETE from tb_detailgroup "; 
        sql += " where fl_id = '" + hidID.Value.Trim() + "'; ";

        //Write Log
        sql += "INSERT INTO tb_LOG(fl_id,fl_module,fl_action,fl_keyword,fl_datetime,fl_ip) VALUES(";
        sql += " '" + Session["uID"].ToString().Replace("'", "''") + "', ";
        sql += " 'MASS GROUP', ";
        sql += " 'DELETE', ";
        sql += " '" + hidID.Value.Trim() + "', ";
        sql += " '" + DateTime.Now.Year + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0') + DateTime.Now.Hour.ToString().PadLeft(2, '0') + DateTime.Now.Minute.ToString().PadLeft(2, '0') + DateTime.Now.Second.ToString().PadLeft(2, '0') + "', ";
        sql += " '" + Request.UserHostAddress + "'); ";

        command.CommandText = sql;
        command.ExecuteNonQuery();
        Conn.Close();
    }
}