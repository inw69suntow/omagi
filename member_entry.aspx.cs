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

public partial class _train_member_entry: System.Web.UI.Page
{
    override protected void OnInit(EventArgs e)
    {
        btnImport.Attributes.Add("onclick",
                  this.GetPostBackEventReference(btnImport));
        base.OnInit(e);
    }

    protected bool checkID(string param)
    {
        if (param.Length != 13) return false;

        try
        {
            int totalCal = 0;

            for (int i = 0; i < (param.Length - 1); i++) totalCal += Convert.ToInt32(param.Substring(i, 1)) * (13 - i);

            totalCal = totalCal % 11;
            totalCal = 11 - totalCal;
            totalCal = totalCal % 10;

            if (param.Substring(12, 1) == totalCal.ToString()) return true; else return false;
        }
        catch (Exception e) { return false; }
    }

    protected string changeDigit(string param)
    {
        string strCheck = "๐๑๒๓๔๕๖๗๘๙";
        string strRes = "0123456789";
        string tmpParam = "";
        int tmpI = 0;
        string result = "";
        for (int i = 0; i < param.Length; i++)
        {
            tmpParam = param.Substring(i, 1);
            tmpI = strCheck.IndexOf(tmpParam);
            switch (tmpI)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                case 8:
                case 9:
                    tmpParam = strRes.Substring(tmpI, 1);
                    break;
            }
            result += tmpParam;
        }
        return result;
    }

    protected string transID(string name, string surname)
    {
        //Send in code
        if (name.Length < 2) return "";
        if (surname.Length < 2) return "";

        string result = "";

        string idTemplate = "S";
        idTemplate += DateTime.Now.Year.ToString().Substring(2, 2) + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0');

        OleDbConnection Conn = new OleDbConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString);
        OleDbCommand command = new OleDbCommand();

        string sql = "SELECT distinct curID,newID from ";
        sql += " (SELECT max(fl_citizen_id) as newID ";
        sql += " from tb_citizen ";
        sql += " where fl_citizen_id like '" + idTemplate + "%') b  left join ";
        sql += " (SELECT top 1 fl_citizen_id as curID ";
        sql += " from tb_citizen ";
        sql += " where fl_fname = '" + name + "' ";
        sql += " and fl_sname ='" + surname + "') a on 1=1 ";

        Conn.Open();
        command.CommandText = sql;
        command.Connection = Conn;
        OleDbDataReader rs = command.ExecuteReader();
        if (rs.Read())
        {
            if (rs.IsDBNull(0))
            {
                if (!rs.IsDBNull(1))
                {
                    int x = Convert.ToInt32(rs[1].ToString().Substring(7)) + 1;
                    result = idTemplate + x.ToString().PadLeft(6, '0');
                }
            }
            else
            {
                result = rs.GetString(0);
            }
        }
        rs.Close();
        Conn.Close();

        if (result.Trim() == "")
        {
            result = idTemplate + "000001";
        }
        return result;
    }

    protected string transDept(string param)
    {
        //Send in code
        try
        {
            int tmpCheck = Convert.ToInt32(param);
            if (param.Length == 3) return param;
        }
        catch (Exception ee) { }

        if (param.Length < 2) return "";
        string result = "";

        OleDbConnection Conn = new OleDbConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString);
        OleDbCommand command = new OleDbCommand();

        string sql = "SELECT distinct fl_dept_id,fl_dept_name from tb_dept ";
        sql += " where fl_dept_name like '" + param + "%' ";
        sql += " order by fl_dept_name ";

        Conn.Open();

        command.CommandText = sql;
        command.Connection = Conn;
        OleDbDataReader rs = command.ExecuteReader();
        if (rs.Read()) result = rs.GetString(0);
        rs.Close();
        Conn.Close();

        return result;
    }

    protected string transTitle(string param)
    {
        //Send in code
        try
        {
            int tmpCheck = Convert.ToInt32(param);
            if (param.Length == 3) return param;
        }
        catch (Exception ee) { }

        if (param.Length < 2) return "";
        string result = "";

        OleDbConnection Conn = new OleDbConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString);
        OleDbCommand command = new OleDbCommand();

        string sql = "SELECT distinct fl_code,fl_title from tb_title ";
        sql += " where fl_title like '" + param + "%' ";
        sql += " order by fl_title ";

        Conn.Open();

        command.CommandText = sql;
        command.Connection = Conn;
        OleDbDataReader rs = command.ExecuteReader();
        if (rs.Read()) result = rs.GetString(0);
        rs.Close();
        Conn.Close();

        return result;
    }

    protected string transProvince(string param1)
    {
        bool param1Flag = true;
        //Send in code
        try
        {
            int tmpCheck = Convert.ToInt32(param1);
            if (param1.Length == 2) param1Flag = false;
        }
        catch (Exception ee) { }

        if (param1.Length < 2) return "";
        string result = "";

        OleDbConnection Conn = new OleDbConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString);
        OleDbCommand command = new OleDbCommand();

        string sql = "SELECT distinct fl_province_code,fl_province_name from tb_moicode where fl_status='1' ";
        if (param1Flag)
            sql += " and fl_province_name like '" + param1 + "%' ";
        else
            sql += " and fl_province_code = '" + param1 + "' ";
        sql += " order by fl_province_name ";

        Conn.Open();

        command.CommandText = sql;
        command.Connection = Conn;
        OleDbDataReader rs = command.ExecuteReader();
        if (rs.Read()) result = rs.GetString(0);
        rs.Close();
        Conn.Close();

        return result;
    }

    protected string transProvince(string param1, string param2)
    {
        bool param1Flag = true;
        bool param2Flag = true;
        //Send in code
        try
        {
            int tmpCheck = Convert.ToInt32(param1);
            if (param1.Length == 2) param1Flag = false;

            tmpCheck = Convert.ToInt32(param2);
            if (param2.Length == 4) param2Flag = false;
        }
        catch (Exception ee) { }

        if (param1.Length < 2) return "";
        if (param2.Length < 2) return transProvince(param1);

        string result = "";

        OleDbConnection Conn = new OleDbConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString);
        OleDbCommand command = new OleDbCommand();

        string sql = "SELECT distinct fl_district_code,fl_district_name,fl_province_name from tb_moicode where fl_status='1' ";
        if (param1Flag)
            sql += " and fl_province_name like '" + param1 + "%' ";
        else
            sql += " and fl_province_code = '" + param1 + "' ";
        if (param2Flag)
            sql += " and fl_district_name like '" + param2 + "%' ";
        else
            sql += " and fl_district_code = '" + param2 + "' ";
        sql += " order by fl_province_name,fl_district_name ";

        Conn.Open();

        command.CommandText = sql;
        command.Connection = Conn;
        OleDbDataReader rs = command.ExecuteReader();
        if (rs.Read()) result = rs.GetString(0);
        rs.Close();
        Conn.Close();

        return result;
    }

    protected string transProvince(string param1, string param2, string param3)
    {
        bool param1Flag = true;
        bool param2Flag = true;
        bool param3Flag = true;
        //Send in code
        try
        {
            int tmpCheck = Convert.ToInt32(param1);
            if (param1.Length == 2) param1Flag = false;

            tmpCheck = Convert.ToInt32(param2);
            if (param2.Length == 4) param2Flag = false;

            tmpCheck = Convert.ToInt32(param3);
            if (param3.Length == 6) param3Flag = false;
        }
        catch (Exception ee) { }

        if (param1.Length < 2) return "";
        if (param2.Length < 2) return transProvince(param1);
        if (param3.Length < 2) return transProvince(param1, param2);

        string result = "";

        OleDbConnection Conn = new OleDbConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString);
        OleDbCommand command = new OleDbCommand();

        string sql = "SELECT distinct fl_tambon_code,fl_tambon_name,fl_district_name,fl_province_name from tb_moicode where fl_status='1' ";
        if (param1Flag)
            sql += " and fl_province_name like '" + param1 + "%' ";
        else
            sql += " and fl_province_code = '" + param1 + "' ";
        if (param2Flag)
            sql += " and fl_district_name like '" + param2 + "%' ";
        else
            sql += " and fl_district_code = '" + param2 + "' ";
        if (param3Flag)
            sql += " and fl_tambon_name like '" + param3 + "%' ";
        else
            sql += " and fl_tambon_code = '" + param3 + "' ";
        sql += " order by fl_province_name,fl_district_name,fl_tambon_name ";

        Conn.Open();

        command.CommandText = sql;
        command.Connection = Conn;
        OleDbDataReader rs = command.ExecuteReader();
        if (rs.Read()) result = rs.GetString(0);
        rs.Close();
        Conn.Close();

        return result;
    }



    protected void dateDataSet()
    {
        cmbBDD.Items.Clear();
        cmbBMM.Items.Clear();
        cmbBYY.Items.Clear();




        cmbBDD.Items.Add(new ListItem("", ""));
        cmbBMM.Items.Add(new ListItem("", ""));
        cmbBYY.Items.Add(new ListItem("", ""));



        //cmbYear.Items.Add(new ListItem("", ""));
        //cmbYearSearch.Items.Add(new ListItem("", ""));

        //cmbGen.Items.Add(new ListItem("", ""));
        //cmbGenSearch.Items.Add(new ListItem("", ""));

        for (int i = 1; i <= 31; i++)
        {
            cmbBDD.Items.Add(new ListItem(i.ToString().PadLeft(2,'0'),i.ToString().PadLeft(2,'0')));
        }

    

        for (int i = 1; i <= 12; i++)
        {
            string nameVal="";

            if(i==1) nameVal="มกราคม";
            if(i==2) nameVal="กุมภาพันธ์";
            if(i==3) nameVal="มีนาคม";
            if(i==4) nameVal="เมษายน";
            if(i==5) nameVal="พฤษภาคม";
            if(i==6) nameVal="มิถุนายน";
            if(i==7) nameVal="กรกฎาคม";
            if(i==8) nameVal="สิงหาคม";
            if(i==9) nameVal="กันยายน";
            if(i==10) nameVal="ตุลาคม";
            if(i==11) nameVal="พฤศจิกายน";
            if(i==12) nameVal="ธันวาคม";

            cmbBMM.Items.Add(new ListItem(nameVal, i.ToString().PadLeft(2, '0')));
        }

        for (int i = 1900; i <= DateTime.Now.Year; i++)
        {
            int j = i + 543;
            cmbBYY.Items.Add(new ListItem(j.ToString(), i.ToString()));
        }
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

    protected void districtDataSet()
    {
        cmbDistrict.Items.Clear();
        cmbDistrict.Items.Add(new ListItem("ไม่กำหนด", ""));

        OleDbConnection Conn = new OleDbConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString);
        OleDbCommand command = new OleDbCommand();

        string sql = "SELECT distinct fl_district_code,fl_district_name from tb_moicode where fl_status='1' ";
        sql += " and fl_province_code ='" + cmbProvince.SelectedValue.Trim() + "' ";
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

  

    protected void clearBox(bool flag)
    {
        hidID.Value = "";
       // txtCardID.Text = "";
        cmbTitle.SelectedValue = "";
        txtFName.Text = "";
        txtSName.Text = "";

        txtAddrNo.Text = "";
        txtHome.Text = "";
        txtSoy.Text = "";
        txtRoad.Text = "";

        if (cmbProvince.Items != null)
        {
            cmbProvince.Items.Add(new ListItem("ไม่กำหนด", ""));
            cmbProvince.SelectedValue = "";
        }
        if (cmbDistrict.Items != null)
        {
            cmbDistrict.Items.Clear();
        }
        if (cmbTambon.Items != null)
        {
            cmbTambon.Items.Clear();
        }
        //cmbDistrict.Items.Add(new ListItem("ไม่กำหนด", ""));
      
        //cmbTambon.Items.Add(new ListItem("ไม่กำหนด", ""));

        txtMoo.Text = "";
        txtPost.Text = "";
        txtTelNo.Text = "";
        txtOffNo.Text = "";
        txtMobNo.Text = "";
        txtEmail.Text = "";

        cmbBDD.SelectedIndex = 0;
        cmbBMM.SelectedIndex = 0;
        cmbBYY.SelectedIndex = 0;

        chkTargetFlag.Checked = false;
        chkStatus.Checked = false;

        txtGoogle.Text = "";

        //if (flag)
        //{
            //cmbCourse.SelectedIndex = 0;
            //cmbGen.SelectedIndex = 0;
            //cmbYear.SelectedIndex = 0;
            //cmbTrainProvince.SelectedIndex = 0;
            //cmbDept.SelectedIndex = 0;
        //}

        //cmbPos.SelectedIndex = 0;

        //cmbStartDD.SelectedIndex = 0;
        //cmbStartMM.SelectedIndex = 0;
        //cmbStartYY.SelectedIndex = 0;

        //cmbStopDD.SelectedIndex = 0;
        //cmbStopMM.SelectedIndex = 0;
        //cmbStopYY.SelectedIndex = 0;

        imgLink.ImageUrl = "CIMG/blank.gif";

        lblCreate.Text = "";
        lblUpdate.Text = "";
        txtEducational.Text = "";
        txtTalent.Text = "";
        txtJob.Text = "";
        txtPosition.Text = "";
        txtCurAddr.Text = "";
        txtAcademy.Text = "";
    }

    protected void boxSet(string hid)
    {
        clearBox(true);
        String sql ="select * ";
            sql+="from tb_citizen ";
            sql+="where ";
            sql += "fl_citizen_id='" + hid + "' ";

        OleDbConnection Conn = new OleDbConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString);
        OleDbCommand command = new OleDbCommand();

        Conn.Open();
        command.Connection = Conn;
        command.CommandText = sql.Replace(";", "");
        OleDbDataReader rs = command.ExecuteReader();
        if (rs.Read())
        {
            hidID.Value = hid.Trim();
            txtCardID.Text = Convert.ToString(rs["fl_citizen_id"]);
            if (System.IO.File.Exists(Server.MapPath("CIMG") + "\\" + txtCardID.Text + ".jpg"))
            {
                imgLink.ImageUrl = "CIMG/" + txtCardID.Text + ".jpg";
            }
            else
            {
                imgLink.ImageUrl = "CIMG/blank.gif";
            }
            String title = Convert.ToString(rs["fl_title"]);
            if (title != "")
            {
                cmbTitle.SelectedValue = title;
            }

            txtFName.Text = Convert.ToString(rs["fl_fname"]);
            txtSName.Text = Convert.ToString(rs["fl_sname"]);
            txtAddrNo.Text = Convert.ToString(rs["fl_addrno"]);
            txtMoo.Text = Convert.ToString(rs["fl_moono"]);
            txtHome.Text = Convert.ToString(rs["fl_home"]);
            txtSoy.Text = Convert.ToString(rs["fl_soy"]);
            txtRoad.Text = Convert.ToString(rs["fl_road"]);

            if (Convert.ToString(rs["fl_province_code"]).Trim() != "")
            {
                cmbProvince.SelectedValue = Convert.ToString(rs["fl_province_code"]);
            }
            districtDataSet();
            if (Convert.ToString(rs["fl_district"]).Trim() != "")
            {
                cmbDistrict.SelectedValue = Convert.ToString(rs["fl_district"]);
            }
            tambonDataSet();
            if (Convert.ToString(rs["fl_tambon"]).Trim() != "")
            {
                cmbTambon.SelectedValue = Convert.ToString(rs["fl_tambon"]);
            }
            txtPost.Text = Convert.ToString(rs["fl_postcode"]);
            txtTelNo.Text = Convert.ToString(rs["fl_telno"]);
            txtOffNo.Text = Convert.ToString(rs["fl_offno"]);
            txtMobNo.Text = Convert.ToString(rs["fl_mobno"]);
            txtEmail.Text = Convert.ToString(rs["fl_email"]);
            String birth = Convert.ToString(rs["fl_birth"]).Trim();
            if (birth!= "")
            {
                cmbBYY.SelectedValue = birth.Substring(0, 4);
                cmbBMM.SelectedValue = birth.Substring(4, 2);
                cmbBDD.SelectedValue = birth.Substring(6, 2);
            }

            if (Convert.ToString(rs["fl_targetFlag"]) == "1")
            {
                chkTargetFlag.Checked = true;
            }
            else
            {
                chkTargetFlag.Checked = false;
            }


            if (Convert.ToString(rs["fl_status"]) != "1")
            {
                chkStatus.Checked = true;
            }
            else
            {
                chkStatus.Checked = false;
            }

            txtGoogle.Text = Convert.ToString(rs["fl_google"]);
            String creatDate = Convert.ToString(rs["fl_create_time"]);
            if (creatDate != "") 
                lblCreate.Text = creatDate.Substring(6, 2) + "-" + creatDate.Substring(4, 2) + "-" + creatDate.Substring(0, 4) + " " + creatDate.Substring(8, 2) + ":" + creatDate.Substring(10, 2) + ":" + creatDate.Substring(12, 2);
            String updateDate = Convert.ToString(rs["fl_update_time"]);
            if (updateDate != "") 
                lblUpdate.Text = updateDate.Substring(6, 2) + "-" + updateDate.Substring(4, 2) + "-" + updateDate.Substring(0, 4) + " " + updateDate.Substring(8, 2) + ":" + updateDate.Substring(10, 2) + ":" + updateDate.Substring(12, 2);

           
            txtEducational.Text = Convert.ToString(rs["fl_educational"]);
            txtTalent.Text = Convert.ToString(rs["fl_talent"]);
            txtJob.Text = Convert.ToString(rs["fl_job"]);
            txtPosition.Text = Convert.ToString(rs["fl_position"]);
            txtCurAddr.Text = Convert.ToString(rs["fl_current_addr"]);
            txtAcademy.Text = Convert.ToString(rs["fl_academy"]);
        }
        rs.Close();
        Conn.Close();
    }
    protected void boxSet(string hid,int flag)
    {
        clearBox(true);
        string sql = "SELECT * ";
        sql += " from tb_citizen ";
        sql += " where fl_citizen_id='" + hid.Replace(";","").Replace("'","''") + "' ";

        OleDbConnection Conn = new OleDbConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString);
        OleDbCommand command = new OleDbCommand();

        Conn.Open();
        command.Connection = Conn;
        command.CommandText = sql.Replace(";","");
        OleDbDataReader rs = command.ExecuteReader();
        if (rs.Read())
        {
            hidID.Value = hid.Trim();
            txtCardID.Text = Convert.ToString(rs["fl_citizen_id"]);
            if (System.IO.File.Exists(Server.MapPath("CIMG") + "\\" + txtCardID.Text + ".jpg"))
            {
                imgLink.ImageUrl = "CIMG/" + txtCardID.Text + ".jpg";
            }
            else
            {
                imgLink.ImageUrl = "CIMG/blank.gif";
            }

            String title = Convert.ToString(rs["fl_title"]);
            if (title != "")
            {
                cmbTitle.SelectedValue = title;
            }

            txtFName.Text = Convert.ToString(rs["fl_fname"]);
            txtSName.Text = Convert.ToString(rs["fl_sname"]);
            txtAddrNo.Text = Convert.ToString(rs["fl_addrno"]);
            txtMoo.Text = Convert.ToString(rs["fl_moono"]);
            txtHome.Text = Convert.ToString(rs["fl_home"]);
            txtSoy.Text = Convert.ToString(rs["fl_soy"]);
            txtRoad.Text = Convert.ToString(rs["fl_road"]);

            if (Convert.ToString(rs["fl_province_code"]).Trim() != "")
            {
                cmbProvince.SelectedValue = Convert.ToString(rs["fl_province_code"]);
            }
            districtDataSet();
            if (Convert.ToString(rs["fl_district"]).Trim() != "")
            {
                cmbDistrict.SelectedValue = Convert.ToString(rs["fl_district"]);
            }
            tambonDataSet();
            if (Convert.ToString(rs["fl_tambon"]).Trim() != "")
            {
                cmbTambon.SelectedValue = Convert.ToString(rs["fl_tambon"]);
            }
            txtPost.Text = Convert.ToString(rs["fl_postcode"]);
            txtTelNo.Text = Convert.ToString(rs["fl_telno"]);
            txtOffNo.Text = Convert.ToString(rs["fl_offno"]);
            txtMobNo.Text = Convert.ToString(rs["fl_mobno"]);
            txtEmail.Text = Convert.ToString(rs["fl_email"]);
            String birth = Convert.ToString(rs["fl_birth"]).Trim();
            if (birth != "")
            {
                cmbBYY.SelectedValue = birth.Substring(0, 4);
                cmbBMM.SelectedValue = birth.Substring(4, 2);
                cmbBDD.SelectedValue = birth.Substring(6, 2);
            }

            if (Convert.ToString(rs["fl_targetFlag"]) == "1")
            {
                chkTargetFlag.Checked = true;
            }
            else
            {
                chkTargetFlag.Checked = false;
            }


            if (Convert.ToString(rs["fl_status"]) != "1")
            {
                chkStatus.Checked = true;
            }
            else
            {
                chkStatus.Checked = false;
            }

            txtGoogle.Text = Convert.ToString(rs["fl_google"]);
            String creatDate = Convert.ToString(rs["fl_create_time"]);
            if (creatDate != "")
                lblCreate.Text = creatDate.Substring(6, 2) + "-" + creatDate.Substring(4, 2) + "-" + creatDate.Substring(0, 4) + " " + creatDate.Substring(8, 2) + ":" + creatDate.Substring(10, 2) + ":" + creatDate.Substring(12, 2);
            String updateDate = Convert.ToString(rs["fl_update_time"]);
            if (updateDate != "")
                lblUpdate.Text = updateDate.Substring(6, 2) + "-" + updateDate.Substring(4, 2) + "-" + updateDate.Substring(0, 4) + " " + updateDate.Substring(8, 2) + ":" + updateDate.Substring(10, 2) + ":" + updateDate.Substring(12, 2);


            txtEducational.Text = Convert.ToString(rs["fl_educational"]);
            txtTalent.Text = Convert.ToString(rs["fl_talent"]);
            txtJob.Text = Convert.ToString(rs["fl_job"]);
            txtPosition.Text = Convert.ToString(rs["fl_position"]);
            txtCurAddr.Text = Convert.ToString(rs["fl_current_addr"]);
            txtAcademy.Text = Convert.ToString(rs["fl_academy"]);
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
                return;
            }
        }
        catch (Exception ex)
        {
            Response.Redirect("login.aspx");
            return;
        }

       

        if (!IsPostBack)
        {
            //Inject Client Script
            HtmlGenericControl body = (HtmlGenericControl)Master.FindControl("masterBody");
            body.Attributes.Add("onload", "initialize()");

            dateDataSet();
            titleDataSet();
            provinceDataSet();



            if (Request.QueryString["hid"] != null)
            {
                if (Request.QueryString["hid"].Trim() != "")
                {
                    boxSet(Request.QueryString["hid"].ToString());
                }
            }
            userDataSet(null);


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

    protected void userDataSet(String sid)
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
        dtGrid.Rows[0].Cells[1].Width = "15%";
        dtGrid.Rows[0].Cells[1].InnerHtml = "<center>เลขประจำตัว</center>";

        dtGrid.Rows[0].Cells.Add(new HtmlTableCell());
        dtGrid.Rows[0].Cells[2].Width = "5%";
        dtGrid.Rows[0].Cells[2].InnerHtml = "<center>คำนำหน้า</center>";

        dtGrid.Rows[0].Cells.Add(new HtmlTableCell());
        dtGrid.Rows[0].Cells[3].Width = "15%";
        dtGrid.Rows[0].Cells[3].InnerHtml = "<center>ชื่อ</center>";

        dtGrid.Rows[0].Cells.Add(new HtmlTableCell());
        dtGrid.Rows[0].Cells[4].Width = "15%";
        dtGrid.Rows[0].Cells[4].InnerHtml = "<center>สกุล</center>";

        dtGrid.Rows[0].Cells.Add(new HtmlTableCell());
        dtGrid.Rows[0].Cells[5].Width = "20%";
        dtGrid.Rows[0].Cells[5].InnerHtml = "<center>วันเกิด</center>";

        dtGrid.Rows[0].Cells.Add(new HtmlTableCell());
        dtGrid.Rows[0].Cells[6].Width = "10%";
        dtGrid.Rows[0].Cells[6].InnerHtml = "<center>ระดับการศึกษา</center>";

        dtGrid.Rows[0].Cells.Add(new HtmlTableCell());
        dtGrid.Rows[0].Cells[7].Width = "10%";
        dtGrid.Rows[0].Cells[7].InnerHtml = "<center>สถานศึกษา</center>";

        dtGrid.Rows[0].Cells.Add(new HtmlTableCell());
        dtGrid.Rows[0].Cells[8].Width = "10%";
        dtGrid.Rows[0].Cells[8].InnerHtml = "<center>เบอร์โทร</center>";

        dtGrid.Rows[0].Cells.Add(new HtmlTableCell());
        dtGrid.Rows[0].Cells[9].Width = "10%";
        dtGrid.Rows[0].Cells[9].InnerHtml = "<center>อีเมลล์</center>";

        string sql = "SELECT * ";
        sql = sql + " FROM tb_citizen ";
        sql = sql + " where 1=1 ";
         String hid=Request.QueryString["hid"];
         if (sid != null)
         {
             sql += " and fl_citizen_id='" + sid + "' ";
         }
         else if (hid != null && hid != "")
         {
             sql += " and fl_citizen_id='" + hid.Trim() + "' ";
         }
         else
         {
             if (txtSearchSID.Text.Trim() == "" && txtSearchFName.Text.Trim() == "" && txtSearchLName.Text.Trim() == "")
             {
                 lblResponse.Text = "ต้องระบุเงื่อนไขการค้นหา";
                 lblResponse.ForeColor = System.Drawing.Color.Red;
                 lblResponse.Visible = true;
                 //userDataSet();
                 return;
             }

             if (txtSearchSID.Text.Trim() != "")
             {
                 sql += " and fl_citizen_id='" + txtSearchSID.Text.Trim() + "' ";
             }
             if (txtSearchFName.Text.Trim() != "")
             {
                 sql += " and fl_fname like '%" + txtSearchFName.Text.Trim() + "%' ";
             }
             if (txtSearchLName.Text.Trim() != "")
             {
                 sql += " and fl_sname like '%" + txtSearchLName.Text.Trim() + "%' ";
             }
         }

        OleDbConnection Conn = new OleDbConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString);
        OleDbCommand command = new OleDbCommand();

        Conn.Open();
        command.Connection = Conn;

        
        OleDbDataReader rs;
        sql = sql + " order by ";
        sql = sql + " fl_fname ASC, ";
        sql = sql + " fl_sname ASC ";

        int i = 1;
        command.CommandText = sql.Replace(";", "");
        Session["userSQL"]= sql.Replace(";", "");

        rs = command.ExecuteReader();
        while (rs.Read())
        {
            string action = "document.location='member_entry.aspx?hid=" + rs.GetString(0);
            action += "';";

            dtGrid.Rows.Add(new HtmlTableRow());
            dtGrid.Rows[i].Attributes.Add("class", "off");
            dtGrid.Rows[i].Attributes.Add("onmouseover", "this.className='on'");
            dtGrid.Rows[i].Attributes.Add("onmouseout", "this.className='off'");
            //dtGrid.Rows[i].Attributes.Add("ondblclick", action);

            dtGrid.Rows[i].Cells.Add(new HtmlTableCell());
            dtGrid.Rows[i].Cells[0].ColSpan = 1;
            dtGrid.Rows[i].Cells[0].Align = "CENTER";

            if (hidID.Value != rs.GetString(0))
                dtGrid.Rows[i].Cells[0].InnerHtml = "<input type='checkbox' id='check_" + i.ToString() + "' onClick=\"" + action + "\">"; 
            else 
                dtGrid.Rows[i].Cells[0].InnerHtml = "<input type='checkbox' id='check_" + i.ToString() + "' checked onClick=\"" + action + "\">";


            dtGrid.Rows[i].Cells.Add(new HtmlTableCell());
            dtGrid.Rows[i].Cells[1].Align = "LEFT";
            dtGrid.Rows[i].Cells[1].InnerHtml = Convert.ToString(rs["fl_citizen_id"]);

            String title=Convert.ToString(rs["fl_title"]);
            dtGrid.Rows[i].Cells.Add(new HtmlTableCell());
            dtGrid.Rows[i].Cells[2].Align = "LEFT";
            dtGrid.Rows[i].Cells[2].InnerHtml = getTitle(title); 

        

            dtGrid.Rows[i].Cells.Add(new HtmlTableCell());
            dtGrid.Rows[i].Cells[3].Align = "LEFT";
            dtGrid.Rows[i].Cells[3].InnerHtml = Convert.ToString(rs["fl_fname"]);

            dtGrid.Rows[i].Cells.Add(new HtmlTableCell());
            dtGrid.Rows[i].Cells[4].Align = "LEFT";
            dtGrid.Rows[i].Cells[4].InnerHtml = Convert.ToString(rs["fl_sname"]);

            dtGrid.Rows[i].Cells.Add(new HtmlTableCell());
            dtGrid.Rows[i].Cells[5].Align = "LEFT";
            dtGrid.Rows[i].Cells[5].InnerHtml = Convert.ToString(rs["fl_birthdate"]);

            dtGrid.Rows[i].Cells.Add(new HtmlTableCell());
            dtGrid.Rows[i].Cells[6].Align = "LEFT";
            dtGrid.Rows[i].Cells[6].InnerHtml = Convert.ToString(rs["fl_educational"]);

            dtGrid.Rows[i].Cells.Add(new HtmlTableCell());
            dtGrid.Rows[i].Cells[7].Align = "LEFT";
            dtGrid.Rows[i].Cells[7].InnerHtml = Convert.ToString(rs["fl_academy"]);

            dtGrid.Rows[i].Cells.Add(new HtmlTableCell());
            dtGrid.Rows[i].Cells[8].Align = "LEFT";
            dtGrid.Rows[i].Cells[8].InnerHtml = Convert.ToString(rs["fl_telno"]);

            dtGrid.Rows[i].Cells.Add(new HtmlTableCell());
            dtGrid.Rows[i].Cells[9].Align = "LEFT";
            dtGrid.Rows[i].Cells[9].InnerHtml = Convert.ToString(rs["fl_email"]);

                
            //dtGrid.Rows[i].Cells.Add(new HtmlTableCell());
            //dtGrid.Rows[i].Cells[7].Align = "LEFT";
            //dtGrid.Rows[i].Cells[7].InnerHtml = Convert.ToString(rs["fl_talent"]);

            //dtGrid.Rows[i].Cells.Add(new HtmlTableCell());
            //dtGrid.Rows[i].Cells[8].Align = "LEFT";
            //dtGrid.Rows[i].Cells[8].InnerHtml = Convert.ToString(rs["fl_job"]);

            //dtGrid.Rows[i].Cells.Add(new HtmlTableCell());
            //dtGrid.Rows[i].Cells[9].Align = "LEFT";
            //dtGrid.Rows[i].Cells[9].InnerHtml = Convert.ToString(rs["fl_position"]);
            i = i + 1;
        }
        rs.Close();

        if (i == 1)
        {
            //noDataFound
            HtmlTableRow row=new HtmlTableRow();
            dtGrid.Rows.Add(row);
            row.Attributes.Add("class", "off");
            row.Attributes.Add("onmouseover", "this.className='on'");
            row.Attributes.Add("onmouseout", "this.className='off'");
            row.Cells.Add(new HtmlTableCell());
            row.Cells[0].ColSpan = 10;
            row.Cells[0].Align = "CENTER";
            row.Cells[0].InnerHtml = "<font color='red' size='4'><b>" + ConfigurationManager.AppSettings["nodataMsg"] + "</b></font>";
        }
        rs.Close();
        Conn.Close();
    }

    private String getTitle(String key)
    {
        for (int i = 0; i < cmbTitle.Items.Count; i++)
        {

            if (cmbTitle.Items[i].Value.Equals(key))
            {
                return cmbTitle.Items[i].Text;
            }
        }
        return "";
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        clearBox(true);
        userDataSet(null);
    }
    protected void btnExport_Click(object sender, EventArgs e)
    {
        //Make CSV File
        OleDbConnection Conn = new OleDbConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString);
        OleDbCommand command = new OleDbCommand();

        Conn.Open();

        string sql = "SELECT distinct ";
        sql += " isnull(a.fl_citizen_id,''), ";
        sql += " isnull(title.fl_title,''), ";
        sql += " isnull(fl_fname,''), ";
        sql += " isnull(fl_sname,''), ";
        sql += " isnull(fl_addrno,''), ";
        sql += " isnull(fl_home,''), ";
        sql += " isnull(fl_soy,''), ";
        sql += " isnull(fl_road,''), ";
        sql += " isnull(provinceB.fl_province_name,''), ";
        sql += " isnull(fl_district_name,''), ";
        sql += " isnull(fl_tambon_name,''), ";
        sql += " isnull(fl_moono,''), ";
        sql += " isnull(fl_postcode,''), ";
        sql += " isnull(fl_telno,''), ";
        sql += " isnull(fl_offno,''), ";
        sql += " isnull(fl_mobno,''), ";
        sql += " isnull(fl_email,''), ";
        sql += " isnull(fl_birth,''), ";
        sql += " isnull(fl_targetFlag,''), ";
        sql += " isnull(fl_status,''), ";
        sql += " isnull(fl_google,''), ";

        sql += " isnull(fl_course,''), ";
        sql += " isnull(fl_gen,''), ";
        sql += " isnull(fl_year,''), ";
        sql += " isnull(fl_dept,''), ";
        sql += " isnull(provinceA.fl_province_name,''), ";
        sql += " isnull(fl_pos,'Z'), ";
        sql += " isnull(fl_start,''), ";
        sql += " isnull(fl_stop,'') ";
        sql += " from tb_train_detail a inner join tb_citizen b  ";
        sql += " on a.fl_citizen_id = b.fl_citizen_id  ";

        //Update 01-10-2012
        sql += " left outer join (";
        sql += " select distinct fl_code,fl_title ";
        sql += " from tb_title ";
        sql += " ) title on b.fl_title=title.fl_code ";

        sql += " left outer join (";
        sql += " select distinct fl_province_code,fl_province_name ";
        sql += " from tb_moicode where fl_status='1' ";
        sql += " ) provinceA on a.fl_province_code=provinceA.fl_province_code ";

        sql += " left outer join (";
        sql += " select distinct fl_province_code,fl_province_name ";
        sql += " from tb_moicode where fl_status='1' ";
        sql += " ) provinceB on b.fl_province_code=provinceB.fl_province_code ";

        sql += " left outer join (";
        sql += " select distinct fl_district_code,fl_district_name ";
        sql += " from tb_moicode where fl_status='1' ";
        sql += " ) district on b.fl_district=district.fl_district_code ";

        sql += " left outer join (";
        sql += " select distinct fl_tambon_code,fl_tambon_name ";
        sql += " from tb_moicode where fl_status='1' ";
        sql += " ) tambon on b.fl_tambon=tambon.fl_tambon_code ";
        
        sql += " where 1=1 ";


        //Check filter command
        //if (Session["uGroup"].ToString() == "U") sql = sql + " and isnull(a.fl_dept,'') like '" + Session["uDept"].ToString().Replace("0", "") + "%' ";
        if (Session["uGroup"].ToString() == "U")
        {
            if (Session["uDept"].ToString().Substring(0) != "0") sql = sql + " and isnull(a.fl_dept,'') like '" + Session["uDept"].ToString().Replace("00", "") + "%' ";
        }
 

        //if (hidID.Value != "") sql += " and a.fl_id='" + hidID.Value + "' ";

        command.CommandText = sql;
        command.Connection = Conn;

        string fName = DateTime.Now.Year + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0') + DateTime.Now.Hour.ToString().PadLeft(2, '0') + DateTime.Now.Minute.ToString().PadLeft(2, '0') + DateTime.Now.Second.ToString().PadLeft(2, '0');
        fName = fName + "_" + Session["uID"].ToString() + "_trainMember";
        if (fName.Length > 100) fName = fName.Substring(0, 100);
        fName = fName + ".csv";

        System.IO.StreamWriter sw = new System.IO.StreamWriter(Server.MapPath("CSV") + "\\" + fName, false, Encoding.Default);
        OleDbDataReader rs = command.ExecuteReader();
        string tmpLine = "";
        tmpLine = tmpLine + "\"บัตรประชาชน\",\"คำนำหน้า\",\"ชื่อ\",\"สกุล\",\"บ้านเลขที่\",\"หมู่บ้าน\",\"ซอย\",\"ถนน\",\"รหัสจังหวัด\",\"รหัสอำเภอ\",\"รหัสตำบล\",\"หมู่ที่\",\"รหัสไปรษณีย์\",\"โทรศัพท์บ้าน\",\"โทรศัพท์ที่ทำงาน\",\"โทรศัพท์มือถือ\",\"อีเมล์\",\"วันเกิด\",\"มวลชนเป้าหมาย\",\"สถานะ\",\"รหัสพิกัด google\",\"หลักสูตร\",\"รุ่น\",\"ปี\",\"หน่วยงาน\",\"จังหวัดฝึก\",\"ตำแหน่ง\",\"วันเริ่มการฝึก\",\"วันสิ้นสุดการฝึก\"";
        sw.WriteLine(tmpLine);
        while (rs.Read())
        {
            tmpLine = "";
            for (int i = 0; i <= 28; i++)
            {
                tmpLine = tmpLine + "\"'" + rs.GetString(i) + "\""; 
                if (i < 28) tmpLine = tmpLine + ",";
            }
            sw.WriteLine(tmpLine);
        }
        rs.Close();
        sw.Close();
        sql = "";
        sql += "INSERT INTO tb_LOG(fl_id,fl_module,fl_action,fl_keyword,fl_datetime,fl_ip) VALUES(";
        sql += " '" + Session["uID"].ToString().Replace("'", "''") + "', ";
        sql += " 'TRAIN MEMBER', ";
        sql += " 'EXPORT', ";
        sql += " '" + hidID.Value.Trim() + "', ";
        sql += " '" + DateTime.Now.Year + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0') + DateTime.Now.Hour.ToString().PadLeft(2, '0') + DateTime.Now.Minute.ToString().PadLeft(2, '0') + DateTime.Now.Second.ToString().PadLeft(2, '0') + "', ";
        sql += " '" + Request.UserHostAddress + "'); ";

        command.CommandText=sql;
        command.ExecuteNonQuery();
        Conn.Close();

        Response.Redirect("CSV/" + fName);
    }
    protected void cmbProvince_SelectedIndexChanged(object sender, EventArgs e)
    {
        districtDataSet();
        userDataSet(this.txtCardID.Text);
    }
    protected void cmbDistrict_SelectedIndexChanged(object sender, EventArgs e)
    {
        tambonDataSet();
        userDataSet(this.txtCardID.Text);
    }

    private void validateSave()
    {
        lblResponse.Text = "";

        if (txtCardID.Text == "")
        {

            txtCardID.Text = transID(txtFName.Text.Trim().Replace(";", "").Replace("'", ""), txtSName.Text.Trim().Replace(";", "").Replace("'", ""));
            if (txtCardID.Text.Trim() == "")
            {
                lblResponse.Text = "ไม่มีเลขที่บัตรประชาชน";
                lblResponse.ForeColor = System.Drawing.Color.Red;
                lblResponse.Visible = true;
                userDataSet(this.txtCardID.Text);
                return;
            }
        }

        if (txtFName.Text == "")
        {
            lblResponse.Text = "ไม่มีชื่อผู้รับการฝึกอบรม";
            lblResponse.ForeColor = System.Drawing.Color.Red;
            lblResponse.Visible = true;
            userDataSet(this.txtCardID.Text);
            return;
        }

        if (txtSName.Text == "")
        {
            lblResponse.Text = "ไม่มีนามสถุลผู้รับการฝึกอบรม";
            lblResponse.ForeColor = System.Drawing.Color.Red;
            lblResponse.Visible = true;
            userDataSet(this.txtCardID.Text);
            return;
        }
    }
   

       private String getCMBDate(){
            string tmpDate = cmbBYY.SelectedValue.Trim().Replace(";", "").Replace("'", "") + cmbBMM.SelectedValue.Trim().Replace(";", "").Replace("'", "") + cmbBDD.SelectedValue.Trim().Replace(";", "").Replace("'", "");
            if (tmpDate.Length < 8) tmpDate = "";
            if (tmpDate != "")
            {
                switch (cmbBMM.SelectedValue.Trim())
                {
                    case "02":
                        if (Convert.ToInt32(cmbBYY.SelectedValue) % 4 == 0)
                        {
                            if (Convert.ToInt32(cmbBDD.SelectedValue) > 29) cmbBDD.SelectedValue = "29";
                        }
                        else
                        {
                            if (Convert.ToInt32(cmbBDD.SelectedValue) > 28) cmbBDD.SelectedValue = "28";
                        }
                        break;
                    case "04":
                    case "06":
                    case "09":
                    case "11":
                        if (Convert.ToInt32(cmbBDD.SelectedValue) > 30) cmbBDD.SelectedValue = "30";
                        break;
                }
                tmpDate = cmbBYY.SelectedValue.Trim().Replace(";", "").Replace("'", "") + cmbBMM.SelectedValue.Trim().Replace(";", "").Replace("'", "") + cmbBDD.SelectedValue.Trim().Replace(";", "").Replace("'", "");
            }
            return tmpDate;
       }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        String tmpDate = getCMBDate();
        string sql = "";
        string sql2;
        OleDbConnection Conn = new OleDbConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString);
        OleDbCommand command = new OleDbCommand();
        Conn.Open();
        command.Connection = Conn;
        OleDbDataReader rs;
        //Save Process
        string procFlag;
        //Process citizen table
        #region citizenProc
        procFlag = "UP";
        if (txtCardID.Text.Trim().Replace(";", "").Replace("'", "") == "")
        {
            procFlag = "NA";
        }
        else
        {
            sql2 = "SELECT distinct fl_citizen_id from tb_citizen ";
            sql2 += "where fl_citizen_id ='" + changeDigit(txtCardID.Text.Trim().Replace(";", "").Replace("'", "").Replace("-", "").Replace(".", "").Replace(" ", "")) + "' ";
            command.CommandText = sql2;
            rs = command.ExecuteReader();
            if (!rs.Read()) procFlag = "IN";
            rs.Close();
        }

        if (procFlag == "IN")
        {
            sql = sql + "INSERT INTO tb_citizen (";
            sql = sql + " fl_citizen_id,";
            sql = sql + " fl_title,";
            sql = sql + " fl_fname,";
            sql = sql + " fl_sname,";
            sql = sql + " fl_addrno,";
            sql = sql + " fl_home,";
            sql = sql + " fl_soy,";
            sql = sql + " fl_road,";
            sql = sql + " fl_province_code,";
            sql = sql + " fl_district,";
            sql = sql + " fl_tambon,";
            sql = sql + " fl_moono,";
            sql = sql + " fl_postcode,";
            sql = sql + " fl_telno,";
            sql = sql + " fl_offno,";
            sql = sql + " fl_mobno,";
            sql = sql + " fl_email,";
            sql = sql + " fl_birth,";
            sql = sql + " fl_targetFlag,";
            sql = sql + " fl_status,";
            sql = sql + " fl_create_by,";
            sql = sql + " fl_create_ip,";
            sql = sql + " fl_create_time,";
            sql = sql + " fl_update_by,";
            sql = sql + " fl_update_IP,";
            sql = sql + " fl_update_time,";
            sql = sql + " fl_google ";

            sql = sql + ",fl_educational";
            sql = sql + ",fl_talent";
            sql = sql + ",fl_job";
            sql = sql + " ,fl_position";
            sql = sql + ",fl_current_addr";
            sql = sql + ",fl_academy";

            sql = sql + " ) values ( ";
            sql = sql + " '" + changeDigit(txtCardID.Text.Trim().Replace(";", "").Replace("'", "").Replace("-", "").Replace(".", "").Replace(" ", "")) + "',";
            sql = sql + " '" + cmbTitle.SelectedValue.Trim().Replace(";", "").Replace("'", "") + "',";
            sql = sql + " '" + txtFName.Text.Trim().Replace(";", "").Replace("'", "") + "',";
            sql = sql + " '" + txtSName.Text.Trim().Replace(";", "").Replace("'", "") + "',";
            sql = sql + " '" + changeDigit(txtAddrNo.Text.Trim().Replace(";", "").Replace("'", "")) + "',";
            sql = sql + " '" + txtHome.Text.Trim().Replace(";", "").Replace("'", "") + "',";
            sql = sql + " '" + txtSoy.Text.Trim().Replace(";", "").Replace("'", "") + "',";
            sql = sql + " '" + txtRoad.Text.Trim().Replace(";", "").Replace("'", "") + "',";
            sql = sql + " '" + cmbProvince.SelectedValue.Trim().Replace(";", "").Replace("'", "") + "',";
            sql = sql + " '" + cmbDistrict.SelectedValue.Trim().Replace(";", "").Replace("'", "") + "',";
            sql = sql + " '" + cmbTambon.SelectedValue.Trim().Replace(";", "").Replace("'", "") + "',";
            sql = sql + " '" + changeDigit(txtMoo.Text.Trim().Replace(";", "").Replace("'", "")) + "',";
            sql = sql + " '" + changeDigit(txtPost.Text.Trim().Replace(";", "").Replace("'", "")) + "',";
            sql = sql + " '" + changeDigit(txtTelNo.Text.Trim().Replace(";", "").Replace("'", "").Replace("-", "").Replace("(", "").Replace(")", "").Replace(".", "")) + "',";
            sql = sql + " '" + changeDigit(txtOffNo.Text.Trim().Replace(";", "").Replace("'", "").Replace("-", "").Replace("(", "").Replace(")", "").Replace(".", "")) + "',";
            sql = sql + " '" + changeDigit(txtMobNo.Text.Trim().Replace(";", "").Replace("'", "").Replace("-", "").Replace("(", "").Replace(")", "").Replace(".", "")) + "',";
            sql = sql + " '" + txtEmail.Text.Trim().Replace(";", "").Replace("'", "") + "',";
            sql = sql + " '" + tmpDate + "',"; 
            if (chkTargetFlag.Checked) sql = sql + " '1',"; else sql = sql + " '0',";
            if (chkStatus.Checked) sql = sql + " '0',"; else sql = sql + " '1',";

            //Append create/update time
            sql += " '" + Session["uID"].ToString().Trim().Replace("'", "''") + "', ";
            sql += " '" + Request.UserHostAddress + "', ";
            sql += " '" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0') + DateTime.Now.Hour.ToString().PadLeft(2, '0') + DateTime.Now.Minute.ToString().PadLeft(2, '0') + DateTime.Now.Second.ToString().PadLeft(2, '0') + "', ";
            sql += " '" + Session["uID"].ToString().Trim().Replace("'", "''") + "', ";
            sql += " '" + Request.UserHostAddress + "', ";
            sql += " '" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0') + DateTime.Now.Hour.ToString().PadLeft(2, '0') + DateTime.Now.Minute.ToString().PadLeft(2, '0') + DateTime.Now.Second.ToString().PadLeft(2, '0') + "', ";
            sql = sql + " '" + txtGoogle.Text.Trim().Replace(";", "").Replace("'", "") + "'";

            sql = sql + " ,'" + txtEducational.Text.Trim() + "'";
            sql = sql + " ,'" + txtTalent.Text.Trim() + "'";
            sql = sql + " ,'" + txtJob.Text.Trim() + "'";
            sql = sql + " ,'" + txtPosition.Text.Trim() + "'";
            sql = sql + " ,'" + txtCurAddr.Text.Trim() + "'";
            sql = sql + " ,'" + txtAcademy.Text.Trim() + "'";
            sql = sql + ");";


            sql += "INSERT INTO tb_LOG(fl_id,fl_module,fl_action,fl_keyword,fl_datetime,fl_ip) VALUES(";
            sql += " '" + Session["uID"].ToString().Replace("'", "''") + "', ";
            sql += " 'CITIZEN', ";
            sql += " 'INSERT', ";
            sql += " '" + changeDigit(txtCardID.Text.Trim().Replace(";", "").Replace("'", "").Replace("-", "").Replace(".", "").Replace(" ", "")) + "', ";
            sql += " '" + DateTime.Now.Year + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0') + DateTime.Now.Hour.ToString().PadLeft(2, '0') + DateTime.Now.Minute.ToString().PadLeft(2, '0') + DateTime.Now.Second.ToString().PadLeft(2, '0') + "', ";
            sql += " '" + Request.UserHostAddress + "'); ";
        }
        else
        {
            sql = sql + "UPDATE tb_citizen SET ";
            sql = sql + " fl_title='" + cmbTitle.SelectedValue.Trim().Replace(";", "").Replace("'", "") + "',";
            sql = sql + " fl_fname='" + txtFName.Text.Trim().Replace(";", "").Replace("'", "") + "',";
            sql = sql + " fl_sname='" + txtSName.Text.Trim().Replace(";", "").Replace("'", "") + "',";
            sql = sql + " fl_addrno='" + changeDigit(txtAddrNo.Text.Trim().Replace(";", "").Replace("'", "")) + "',";
            sql = sql + " fl_home='" + txtHome.Text.Trim().Replace(";", "").Replace("'", "") + "',";
            sql = sql + " fl_soy='" + txtSoy.Text.Trim().Replace(";", "").Replace("'", "") + "',";
            sql = sql + " fl_road='" + txtRoad.Text.Trim().Replace(";", "").Replace("'", "") + "',";
            sql = sql + " fl_province_code='" + cmbProvince.SelectedValue.Trim().Replace(";", "").Replace("'", "") + "',";
            sql = sql + " fl_district='" + cmbDistrict.SelectedValue.Trim().Replace(";", "").Replace("'", "") + "',";
            sql = sql + " fl_tambon='" + cmbTambon.SelectedValue.Trim().Replace(";", "").Replace("'", "") + "',";
            sql = sql + " fl_moono='" + changeDigit(txtMoo.Text.Trim().Replace(";", "").Replace("'", "")) + "',";
            sql = sql + " fl_postcode='" + changeDigit(txtPost.Text.Trim().Replace(";", "").Replace("'", "")) + "',";
            sql = sql + " fl_telno='" + changeDigit(txtTelNo.Text.Trim().Replace(";", "").Replace("'", "").Replace("-", "").Replace("(", "").Replace(")", "").Replace(".", "")) + "',";
            sql = sql + " fl_offno='" + changeDigit(txtOffNo.Text.Trim().Replace(";", "").Replace("'", "").Replace("-", "").Replace("(", "").Replace(")", "").Replace(".", "")) + "',";
            sql = sql + " fl_mobno='" + changeDigit(txtMobNo.Text.Trim().Replace(";", "").Replace("'", "").Replace("-", "").Replace("(", "").Replace(")", "").Replace(".", "")) + "',";
            sql = sql + " fl_email='" + txtEmail.Text.Trim().Replace(";", "").Replace("'", "") + "',";
            sql = sql + " fl_birth='" + tmpDate + "',";
            if (chkTargetFlag.Checked) sql = sql + " fl_targetFlag='1',"; else sql = sql + " fl_targetFlag='0',";
            if (chkStatus.Checked) sql = sql + " fl_status='0',"; else sql = sql + " fl_status='1',";
            sql += " fl_update_by ='" + Session["uID"].ToString().Trim().Replace("'", "''") + "', ";
            sql += " fl_update_ip ='" + Request.UserHostAddress + "', ";
            sql += " fl_update_time ='" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0') + DateTime.Now.Hour.ToString().PadLeft(2, '0') + DateTime.Now.Minute.ToString().PadLeft(2, '0') + DateTime.Now.Second.ToString().PadLeft(2, '0') + "', ";
            sql = sql + " fl_google='" + txtGoogle.Text.Trim().Replace(";", "").Replace("'", "") + "' ";
            sql = sql + " ,fl_educational='" + txtEducational.Text.Trim() + "'";
            sql = sql + " ,fl_talent='" + txtTalent.Text.Trim() + "'";
            sql = sql + " ,fl_job='" + txtJob.Text.Trim() + "'";
            sql = sql + " ,fl_position='" + txtPosition.Text.Trim() + "'";
            sql = sql + " ,fl_current_addr='" + txtCurAddr.Text.Trim() + "'";
            sql = sql + " ,fl_academy='" + txtAcademy.Text.Trim() + "'";

            sql = sql + " where fl_citizen_id='" + changeDigit(txtCardID.Text.Trim().Replace(";", "").Replace("'", "").Replace("-", "").Replace(".", "").Replace(" ", "")) + "'; ";

            sql += "INSERT INTO tb_LOG(fl_id,fl_module,fl_action,fl_keyword,fl_datetime,fl_ip) VALUES(";
            sql += " '" + Session["uID"].ToString().Replace("'", "''") + "', ";
            sql += " 'CITIZEN', ";
            sql += " 'UPDATE', ";
            sql += " '" + changeDigit(txtCardID.Text.Trim().Replace(";", "").Replace("'", "").Replace("-", "").Replace(".", "").Replace(" ", "")) + "', ";
            sql += " '" + DateTime.Now.Year + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0') + DateTime.Now.Hour.ToString().PadLeft(2, '0') + DateTime.Now.Minute.ToString().PadLeft(2, '0') + DateTime.Now.Second.ToString().PadLeft(2, '0') + "', ";
            sql += " '" + Request.UserHostAddress + "'); ";
        }
        #endregion

      
        command.CommandText = sql;
        command.ExecuteNonQuery();
        Conn.Close();

        lblResponse.Text = "บันทึกข้อมูลสำเร็จ";
        lblResponse.ForeColor = System.Drawing.Color.Green;
        lblResponse.Visible = true;

        userDataSet(this.txtCardID.Text);
    }
    protected void btnImport_Click(object sender, EventArgs e)
    {
        string tmpImport = "";
        string errLine = "";
        int successLine = 0;
        int LineNo = 1;

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

            foreach (string rowVal in rowToken)
            {
                string[] colToken = rowVal.Trim().Split('|');

                string procFlag;
                LineNo++;

                if (colToken.Length == 29)
                {

                    //Process citizen table
                    #region citizenProc
                    procFlag = "UP";
                    if (colToken[0].Trim().Replace(";", "").Replace("'", "") == "")
                    {
                        //procFlag = "NA";
                        colToken[0] = transID(colToken[2].Trim().Replace(";", "").Replace("'", ""), colToken[3].Trim().Replace(";", "").Replace("'", ""));
                        if (colToken[0].Trim() == "")
                        {
                            procFlag = "NA";
                        }
                        else
                        {
                            sql2 = "SELECT distinct fl_citizen_id from tb_citizen ";
                            tmpImport = changeDigit(colToken[0].Trim().Replace(";", "").Replace("'", "").Replace("-", "").Replace(".", "").Replace(" ", ""));
                            if (tmpImport.Length > 20) tmpImport = tmpImport.Substring(0, 20);
                            sql2 += "where fl_citizen_id ='" + tmpImport + "' ";
                            command.CommandText = sql2;
                            rs = command.ExecuteReader();
                            if (!rs.Read()) procFlag = "IN";
                            rs.Close();
                        }
                    }
                    else
                    {
                        sql2 = "SELECT distinct fl_citizen_id from tb_citizen ";
                        tmpImport = changeDigit(colToken[0].Trim().Replace(";", "").Replace("'", "").Replace("-", "").Replace(".", "").Replace(" ", ""));
                        if (tmpImport.Length > 20) tmpImport = tmpImport.Substring(0, 20);
                        sql2 += "where fl_citizen_id ='" + tmpImport + "' ";
                        command.CommandText = sql2;
                        rs = command.ExecuteReader();
                        if (!rs.Read()) procFlag = "IN";
                        rs.Close();
                    }

                    //Data Correction
                    bool yyyymmdd = true;
                    if (colToken[17].IndexOf("/") < 3) yyyymmdd = false;
                    if (colToken[17].IndexOf("-") < 3) yyyymmdd = false;
                    if (colToken[17].IndexOf(".") < 3) yyyymmdd = false;

                    colToken[17] = changeDigit(colToken[17].Trim().Replace(";", "").Replace("'", "").Replace("/", "").Replace("-", ""));
                    if (colToken[17].Length != 8)
                    {
                        colToken[17] = "";
                    }
                    else
                    {
                        int yyyy = 0;
                        int mm = 0;
                        int dd = 0;
                        try
                        {
                            if (yyyymmdd)
                            {
                                yyyy = Convert.ToInt32(colToken[17].Substring(0, 4));
                                if (yyyy > 2500) yyyy -= 543;
                                mm = Convert.ToInt32(colToken[17].Substring(4, 2));
                                dd = Convert.ToInt32(colToken[17].Substring(6, 2));
                            }
                            else
                            {
                                yyyy = Convert.ToInt32(colToken[17].Substring(4, 4));
                                if (yyyy > 2500) yyyy -= 543;
                                mm = Convert.ToInt32(colToken[17].Substring(2, 2));
                                dd = Convert.ToInt32(colToken[17].Substring(0, 2));
                            }
                            switch (mm)
                            {
                                case 2:
                                    if (yyyy % 4 == 0)
                                    {
                                        if (dd > 29) dd = 29;
                                    }
                                    else
                                    {
                                        if (dd > 28) dd = 28;
                                    }
                                    break;
                                case 4:
                                case 6:
                                case 9:
                                case 11:
                                    if (dd > 30) dd = 30;
                                    break;
                            }
                            colToken[17] = yyyy.ToString() + mm.ToString().PadLeft(2, '0') + dd.ToString().PadLeft(2, '0');
                        }
                        catch (Exception ee) { colToken[17] = ""; }
                    }

                    //Start
                    yyyymmdd = true;
                    if (colToken[27].IndexOf("/") < 3) yyyymmdd = false;
                    if (colToken[27].IndexOf("-") < 3) yyyymmdd = false;
                    if (colToken[27].IndexOf(".") < 3) yyyymmdd = false;

                    colToken[27] = changeDigit(colToken[27].Trim().Replace(";", "").Replace("'", "").Replace("/", "").Replace("-", ""));
                    if (colToken[27].Length != 8)
                    {
                        colToken[27] = "";
                    }
                    else
                    {
                        int yyyy = 0;
                        int mm = 0;
                        int dd = 0;
                        try
                        {
                            if (yyyymmdd)
                            {
                                yyyy = Convert.ToInt32(colToken[27].Substring(0, 4));
                                if (yyyy > 2500) yyyy -= 543;
                                mm = Convert.ToInt32(colToken[27].Substring(4, 2));
                                dd = Convert.ToInt32(colToken[27].Substring(6, 2));
                            }
                            else
                            {
                                yyyy = Convert.ToInt32(colToken[27].Substring(4, 4));
                                if (yyyy > 2500) yyyy -= 543;
                                mm = Convert.ToInt32(colToken[27].Substring(2, 2));
                                dd = Convert.ToInt32(colToken[27].Substring(0, 2));
                            }
                            switch (mm)
                            {
                                case 2:
                                    if (yyyy % 4 == 0)
                                    {
                                        if (dd > 29) dd = 29;
                                    }
                                    else
                                    {
                                        if (dd > 28) dd = 28;
                                    }
                                    break;
                                case 4:
                                case 6:
                                case 9:
                                case 11:
                                    if (dd > 30) dd = 30;
                                    break;
                            }
                            colToken[27] = yyyy.ToString() + mm.ToString().PadLeft(2, '0') + dd.ToString().PadLeft(2, '0');
                        }
                        catch (Exception ee) { colToken[27] = ""; }
                    }

                    //Stop
                    yyyymmdd = true;
                    if (colToken[28].IndexOf("/") < 3) yyyymmdd = false;
                    if (colToken[28].IndexOf("-") < 3) yyyymmdd = false;
                    if (colToken[28].IndexOf(".") < 3) yyyymmdd = false;

                    colToken[28] = changeDigit(colToken[28].Trim().Replace(";", "").Replace("'", "").Replace("/", "").Replace("-", ""));
                    if (colToken[28].Length != 8)
                    {
                        colToken[28] = "";
                    }
                    else
                    {
                        int yyyy = 0;
                        int mm = 0;
                        int dd = 0;
                        try
                        {
                            if (yyyymmdd)
                            {
                                yyyy = Convert.ToInt32(colToken[28].Substring(0, 4));
                                if (yyyy > 2500) yyyy -= 543;
                                mm = Convert.ToInt32(colToken[28].Substring(4, 2));
                                dd = Convert.ToInt32(colToken[28].Substring(6, 2));
                            }
                            else
                            {
                                yyyy = Convert.ToInt32(colToken[28].Substring(4, 4));
                                if (yyyy > 2500) yyyy -= 543;
                                mm = Convert.ToInt32(colToken[28].Substring(2, 2));
                                dd = Convert.ToInt32(colToken[28].Substring(0, 2));
                            }
                            switch (mm)
                            {
                                case 2:
                                    if (yyyy % 4 == 0)
                                    {
                                        if (dd > 29) dd = 29;
                                    }
                                    else
                                    {
                                        if (dd > 28) dd = 28;
                                    }
                                    break;
                                case 4:
                                case 6:
                                case 9:
                                case 11:
                                    if (dd > 30) dd = 30;
                                    break;
                            }
                            colToken[28] = yyyy.ToString() + mm.ToString().PadLeft(2, '0') + dd.ToString().PadLeft(2, '0');
                        }
                        catch (Exception ee) { colToken[28] = ""; }
                    }

                    if (Convert.ToInt32(colToken[23].Trim().Replace(";", "").Replace("'", "")) > 2500) colToken[23] = Convert.ToString(Convert.ToInt32(colToken[23].Trim().Replace(";", "").Replace("'", "")) - 543);

                    string provinceCode = "";
                    string districtCode = "";
                    string tambonCode = "";
                    string tmpCode = transProvince(colToken[8].Trim().Replace(";", "").Replace("'", ""), colToken[9].Trim().Replace(";", "").Replace("'", ""), colToken[10].Trim().Replace(";", "").Replace("'", ""));
                    if (tmpCode != "")
                    {
                        switch (tmpCode.Length)
                        {
                            case 2:
                                provinceCode = tmpCode.Substring(0, 2);
                                break;
                            case 4:
                                provinceCode = tmpCode.Substring(0, 2);
                                districtCode = tmpCode.Substring(0, 4);
                                break;
                            case 6:
                                provinceCode = tmpCode.Substring(0, 2);
                                districtCode = tmpCode.Substring(0, 4);
                                tambonCode = tmpCode.Substring(0, 6);
                                break;
                        }
                    }

                    if (procFlag == "IN")
                    {
                        sql = sql + "INSERT INTO tb_citizen (";
                        sql = sql + " fl_citizen_id,";
                        sql = sql + " fl_mem_id,";
                        sql = sql + " fl_title,";
                        sql = sql + " fl_fname,";
                        sql = sql + " fl_sname,";
                        sql = sql + " fl_addrno,";
                        sql = sql + " fl_home,";
                        sql = sql + " fl_soy,";
                        sql = sql + " fl_road,";
                        sql = sql + " fl_province_code,";
                        sql = sql + " fl_district,";
                        sql = sql + " fl_tambon,";
                        sql = sql + " fl_moono,";
                        sql = sql + " fl_postcode,";
                        sql = sql + " fl_telno,";
                        sql = sql + " fl_offno,";
                        sql = sql + " fl_mobno,";
                        sql = sql + " fl_email,";
                        sql = sql + " fl_birth,";
                        sql = sql + " fl_targetFlag,";
                        sql = sql + " fl_status,";
                        sql = sql + " fl_create_by,";
                        sql = sql + " fl_create_ip,";
                        sql = sql + " fl_create_time,";
                        sql = sql + " fl_update_by,";
                        sql = sql + " fl_update_IP,";
                        sql = sql + " fl_update_time,";
                        sql = sql + " fl_google ";

                        sql = sql + " ) values ( ";

                        tmpImport = changeDigit(colToken[0].Trim().Replace(";", "").Replace("'", "").Replace("-", "").Replace(".", "").Replace(" ", ""));
                        if (tmpImport.Length > 20) tmpImport = tmpImport.Substring(0, 20);
                        sql = sql + "'" + tmpImport + "', ";
                        tmpImport = changeDigit(colToken[21].Trim().Replace(";", "").Replace("'", ""));
                        if (tmpImport.Length > 20) tmpImport = tmpImport.Substring(0, 20);
                        sql = sql + "'" + tmpImport + "', ";
                        tmpImport = transTitle(colToken[1].Trim().Replace(";", "").Replace("'", ""));
                        if (tmpImport.Length > 10) tmpImport = tmpImport.Substring(0, 10);
                        sql = sql + "'" + tmpImport + "', ";
                        tmpImport = colToken[2].Trim().Replace(";", "").Replace("'", "");
                        if (tmpImport.Length > 250) tmpImport = tmpImport.Substring(0, 250);
                        sql = sql + "'" + tmpImport + "', ";
                        tmpImport = colToken[3].Trim().Replace(";", "").Replace("'", "");
                        if (tmpImport.Length > 250) tmpImport = tmpImport.Substring(0, 250);
                        sql = sql + "'" + tmpImport + "', ";

                        tmpImport = changeDigit(colToken[4].Trim().Replace(";", "").Replace("'", ""));
                        if (tmpImport.Length > 250) tmpImport = tmpImport.Substring(0, 250);
                        sql = sql + "'" + tmpImport + "', ";
                        tmpImport = colToken[5].Trim().Replace(";", "").Replace("'", "");
                        if (tmpImport.Length > 250) tmpImport = tmpImport.Substring(0, 250);
                        sql = sql + "'" + tmpImport + "', ";
                        tmpImport = colToken[6].Trim().Replace(";", "").Replace("'", "");
                        if (tmpImport.Length > 250) tmpImport = tmpImport.Substring(0, 250);
                        sql = sql + "'" + tmpImport + "', ";
                        tmpImport = colToken[7].Trim().Replace(";", "").Replace("'", "");
                        if (tmpImport.Length > 250) tmpImport = tmpImport.Substring(0, 250);
                        sql = sql + "'" + tmpImport + "', ";

                        tmpImport = provinceCode;
                        if (tmpImport.Length > 2) tmpImport = tmpImport.Substring(0, 2);
                        sql = sql + "'" + tmpImport + "', ";
                        tmpImport = districtCode;
                        if (tmpImport.Length > 4) tmpImport = tmpImport.Substring(0, 4);
                        sql = sql + "'" + tmpImport + "', ";
                        tmpImport = tambonCode;
                        if (tmpImport.Length > 6) tmpImport = tmpImport.Substring(0, 6);
                        sql = sql + "'" + tmpImport + "', ";

                        tmpImport = changeDigit(colToken[11].Trim().Replace(";", "").Replace("'", ""));
                        if (tmpImport.Length > 10) tmpImport = tmpImport.Substring(0, 10);
                        sql = sql + "'" + tmpImport + "', ";
                        tmpImport = changeDigit(colToken[12].Trim().Replace(";", "").Replace("'", ""));
                        if (tmpImport.Length > 5) tmpImport = tmpImport.Substring(0, 5);
                        sql = sql + "'" + tmpImport + "', ";
                        tmpImport = changeDigit(colToken[13].Trim().Replace(";", "").Replace("'", "").Replace("-", "").Replace("(", "").Replace(")", "").Replace(".", ""));
                        if (tmpImport.Length > 10) tmpImport = tmpImport.Substring(0, 10);
                        sql = sql + "'" + tmpImport + "', ";
                        tmpImport = changeDigit(colToken[14].Trim().Replace(";", "").Replace("'", "").Replace("-", "").Replace("(", "").Replace(")", "").Replace(".", ""));
                        if (tmpImport.Length > 10) tmpImport = tmpImport.Substring(0, 10);
                        sql = sql + "'" + tmpImport + "', ";
                        tmpImport = changeDigit(colToken[15].Trim().Replace(";", "").Replace("'", "").Replace("-", "").Replace("(", "").Replace(")", "").Replace(".", ""));
                        if (tmpImport.Length > 10) tmpImport = tmpImport.Substring(0, 10);
                        sql = sql + "'" + tmpImport + "', ";

                        tmpImport = colToken[16].Trim().Replace(";", "").Replace("'", "");
                        if (tmpImport.Length > 250) tmpImport = tmpImport.Substring(0, 250);
                        sql = sql + "'" + tmpImport + "', ";
                        tmpImport = colToken[17].Trim().Replace(";", "").Replace("'", "");
                        if (tmpImport.Length > 20) tmpImport = tmpImport.Substring(0, 20);
                        sql = sql + "'" + tmpImport + "', ";
                        tmpImport = colToken[18].Trim().Replace(";", "").Replace("'", "");
                        if (tmpImport.Length > 1) tmpImport = tmpImport.Substring(0, 1);
                        sql = sql + "'" + tmpImport + "', ";
                        tmpImport = colToken[19].Trim().Replace(";", "").Replace("'", "");
                        if (tmpImport.Length > 1) tmpImport = tmpImport.Substring(0, 1);
                        sql = sql + "'" + tmpImport + "', ";

                        //Append create/update time
                        sql += " '" + Session["uID"].ToString().Trim().Replace("'", "''") + "', ";
                        sql += " '" + Request.UserHostAddress + "', ";
                        sql += " '" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0') + DateTime.Now.Hour.ToString().PadLeft(2, '0') + DateTime.Now.Minute.ToString().PadLeft(2, '0') + DateTime.Now.Second.ToString().PadLeft(2, '0') + "', ";
                        sql += " '" + Session["uID"].ToString().Trim().Replace("'", "''") + "', ";
                        sql += " '" + Request.UserHostAddress + "', ";
                        sql += " '" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0') + DateTime.Now.Hour.ToString().PadLeft(2, '0') + DateTime.Now.Minute.ToString().PadLeft(2, '0') + DateTime.Now.Second.ToString().PadLeft(2, '0') + "', ";

                        tmpImport = colToken[20].Trim().Replace(";", "").Replace("'", "");
                        if (tmpImport.Length > 50) tmpImport = tmpImport.Substring(0, 50);
                        sql += " '" + tmpImport + "'); ";
                    }
                    else
                    {
                        sql = sql + "UPDATE tb_citizen SET ";
                        tmpImport = transTitle(colToken[1].Trim().Replace(";", "").Replace("'", ""));
                        if (tmpImport.Length > 10) tmpImport = tmpImport.Substring(0, 10);
                        sql = sql + " fl_title='" + tmpImport + "',";
                        tmpImport = colToken[2].Trim().Replace(";", "").Replace("'", "");
                        if (tmpImport.Length > 250) tmpImport = tmpImport.Substring(0, 250);
                        sql = sql + " fl_fname='" + tmpImport + "',";
                        tmpImport = colToken[3].Trim().Replace(";", "").Replace("'", "");
                        if (tmpImport.Length > 250) tmpImport = tmpImport.Substring(0, 250);
                        sql = sql + " fl_sname='" + tmpImport + "',";

                        tmpImport = changeDigit(colToken[4].Trim().Replace(";", "").Replace("'", ""));
                        if (tmpImport.Length > 250) tmpImport = tmpImport.Substring(0, 250);
                        sql = sql + " fl_addrno='" + tmpImport + "',";
                        tmpImport = colToken[5].Trim().Replace(";", "").Replace("'", "");
                        if (tmpImport.Length > 250) tmpImport = tmpImport.Substring(0, 250);
                        sql = sql + " fl_home='" + tmpImport + "',";
                        tmpImport = colToken[6].Trim().Replace(";", "").Replace("'", "");
                        if (tmpImport.Length > 250) tmpImport = tmpImport.Substring(0, 250);
                        sql = sql + " fl_soy='" + tmpImport + "',";
                        tmpImport = colToken[7].Trim().Replace(";", "").Replace("'", "");
                        if (tmpImport.Length > 250) tmpImport = tmpImport.Substring(0, 250);
                        sql = sql + " fl_road='" + tmpImport + "',";

                        tmpImport = provinceCode;
                        if (tmpImport.Length > 2) tmpImport = tmpImport.Substring(0, 2);
                        sql = sql + " fl_province_code='" + tmpImport + "',";
                        tmpImport = districtCode;
                        if (tmpImport.Length > 4) tmpImport = tmpImport.Substring(0, 4);
                        sql = sql + " fl_district='" + tmpImport + "',";
                        tmpImport = tambonCode;
                        if (tmpImport.Length > 6) tmpImport = tmpImport.Substring(0, 6);
                        sql = sql + " fl_tambon='" + tmpImport + "',";

                        tmpImport = changeDigit(colToken[11].Trim().Replace(";", "").Replace("'", ""));
                        if (tmpImport.Length > 10) tmpImport = tmpImport.Substring(0, 10);
                        sql = sql + " fl_moono='" + tmpImport + "',";
                        tmpImport = changeDigit(colToken[12].Trim().Replace(";", "").Replace("'", ""));
                        if (tmpImport.Length > 5) tmpImport = tmpImport.Substring(0, 5);
                        sql = sql + " fl_postcode='" + tmpImport + "',";
                        tmpImport = changeDigit(colToken[13].Trim().Replace(";", "").Replace("'", "").Replace("-", "").Replace("(", "").Replace(")", "").Replace(".", ""));
                        if (tmpImport.Length > 10) tmpImport = tmpImport.Substring(0, 10);
                        sql = sql + " fl_telno='" + tmpImport + "',";
                        tmpImport = changeDigit(colToken[14].Trim().Replace(";", "").Replace("'", "").Replace("-", "").Replace("(", "").Replace(")", "").Replace(".", ""));
                        if (tmpImport.Length > 10) tmpImport = tmpImport.Substring(0, 10);
                        sql = sql + " fl_offno='" + tmpImport + "',";
                        tmpImport = changeDigit(colToken[15].Trim().Replace(";", "").Replace("'", "").Replace("-", "").Replace("(", "").Replace(")", "").Replace(".", ""));
                        if (tmpImport.Length > 10) tmpImport = tmpImport.Substring(0, 10);
                        sql = sql + " fl_mobno='" + tmpImport + "',";

                        tmpImport = colToken[16].Trim().Replace(";", "").Replace("'", "");
                        if (tmpImport.Length > 250) tmpImport = tmpImport.Substring(0, 250);
                        sql = sql + " fl_email='" + tmpImport + "',";
                        tmpImport = colToken[17].Trim().Replace(";", "").Replace("'", "");
                        if (tmpImport.Length > 20) tmpImport = tmpImport.Substring(0, 20);
                        sql = sql + " fl_birth='" + tmpImport + "',";
                        tmpImport = colToken[18].Trim().Replace(";", "").Replace("'", "");
                        if (tmpImport.Length > 1) tmpImport = tmpImport.Substring(0, 1);
                        sql = sql + " fl_targetFlag='" + tmpImport + "',";
                        tmpImport = colToken[19].Trim().Replace(";", "").Replace("'", "");
                        if (tmpImport.Length > 1) tmpImport = tmpImport.Substring(0, 1);
                        sql = sql + " fl_status='" + tmpImport + "',";

                        sql += " fl_update_by ='" + Session["uID"].ToString().Trim().Replace("'", "''") + "', ";
                        sql += " fl_update_ip ='" + Request.UserHostAddress + "', ";
                        sql += " fl_update_time ='" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0') + DateTime.Now.Hour.ToString().PadLeft(2, '0') + DateTime.Now.Minute.ToString().PadLeft(2, '0') + DateTime.Now.Second.ToString().PadLeft(2, '0') + "', ";

                        tmpImport = colToken[20].Trim().Replace(";", "").Replace("'", "");
                        if (tmpImport.Length > 50) tmpImport = tmpImport.Substring(0, 50);
                        sql = sql + " fl_google='" + tmpImport + "' ";

                        tmpImport = changeDigit(colToken[0].Trim().Replace(";", "").Replace("'", "").Replace("-", "").Replace(".", "").Replace(" ", ""));
                        if (tmpImport.Length > 20) tmpImport = tmpImport.Substring(0, 20);
                        sql = sql + " where fl_citizen_id='" + tmpImport + "'; ";
                    }
                    #endregion

                    //Process traingroup
                    #region trainProc

                    string province25=transProvince(colToken[25].Trim().Replace(";", "").Replace("'", ""));
                    if (province25.Trim() != "") province25 = province25.Trim().Substring(0, 2);
                    procFlag = "UP";
                    if (colToken[0].Trim().Replace(";", "").Replace("'", "") == "")
                    {
                        procFlag = "NA";
                    }
                    else
                    {
                        sql2 = "SELECT distinct fl_citizen_id from tb_train_detail ";

                        tmpImport = changeDigit(colToken[0].Trim().Replace(";", "").Replace("'", "").Replace("-", "").Replace(".", "").Replace(" ", ""));
                        if (tmpImport.Length > 20) tmpImport = tmpImport.Substring(0, 20);
                        sql2 += "where fl_citizen_id ='" + tmpImport + "' ";
                        tmpImport = colToken[21].Trim().Replace(";", "").Replace("'", "");
                        if (tmpImport.Length > 255) tmpImport = tmpImport.Substring(0, 255);
                        sql2 += " and fl_course='" + tmpImport + "'";
                        tmpImport = changeDigit(colToken[22].Trim().Replace(";", "").Replace("'", "").Replace("-", "").Replace(".", "").Replace(" ", ""));
                        if (tmpImport.Length > 5) tmpImport = tmpImport.Substring(0, 5);
                        sql2 += " and fl_gen='" + tmpImport + "'";
                        tmpImport = changeDigit(colToken[23].Trim().Replace(";", "").Replace("'", "").Replace("-", "").Replace(".", "").Replace(" ", ""));
                        if (tmpImport.Length > 4) tmpImport = tmpImport.Substring(0, 4);
                        sql2 += " and fl_year='" + tmpImport + "'";
                        tmpImport = transDept(colToken[24].Trim().Replace(";", "").Replace("'", ""));
                        if (tmpImport.Length > 3) tmpImport = tmpImport.Substring(0, 3);
                        sql2 += " and isnull(fl_dept,'')='" + tmpImport + "'";
                        tmpImport = province25;
                        if (tmpImport.Length > 2) tmpImport = tmpImport.Substring(0, 2);
                        sql2 += " and fl_province_code='" + tmpImport + "'";
                        command.CommandText = sql2;
                        rs = command.ExecuteReader();
                        if (!rs.Read()) procFlag = "IN";
                        rs.Close();
                    }

                    if (procFlag == "IN")
                    {
                        sql = sql + "INSERT INTO tb_train_detail (";
                        sql = sql + " fl_citizen_id,";
                        sql = sql + " fl_course,";
                        sql = sql + " fl_gen,";
                        sql = sql + " fl_year,";
                        sql = sql + " fl_dept,";
                        sql = sql + " fl_province_code,";
                        sql = sql + " fl_pos,";
                        sql = sql + " fl_start,";
                        sql = sql + " fl_stop, ";
                        sql = sql + " fl_create_by,";
                        sql = sql + " fl_create_ip,";
                        sql = sql + " fl_create_time,";
                        sql = sql + " fl_update_by,";
                        sql = sql + " fl_update_IP,";
                        sql = sql + " fl_update_time";

                        sql = sql + " ) values ( ";

                        tmpImport = changeDigit(colToken[0].Trim().Replace(";", "").Replace("'", "").Replace("-", "").Replace(".", "").Replace(" ", ""));
                        if (tmpImport.Length > 20) tmpImport = tmpImport.Substring(0, 20);
                        sql = sql + "'" + tmpImport + "', ";

                        tmpImport = colToken[21].Trim().Replace(";", "").Replace("'", "");
                        if (tmpImport.Length > 255) tmpImport = tmpImport.Substring(0, 255);
                        sql = sql + "'" + tmpImport + "', ";
                        tmpImport = changeDigit(colToken[22].Trim().Replace(";", "").Replace("'", "").Replace("-", "").Replace(".", "").Replace(" ", ""));
                        if (tmpImport.Length > 5) tmpImport = tmpImport.Substring(0, 5);
                        sql = sql + "'" + tmpImport + "', ";
                        tmpImport = changeDigit(colToken[23].Trim().Replace(";", "").Replace("'", "").Replace("-", "").Replace(".", "").Replace(" ", ""));
                        if (tmpImport.Length > 4) tmpImport = tmpImport.Substring(0, 4);
                        sql = sql + "'" + tmpImport + "', ";
                        tmpImport = transDept(colToken[24].Trim().Replace(";", "").Replace("'", ""));
                        if (tmpImport.Length > 3) tmpImport = tmpImport.Substring(0, 3);
                        sql = sql + "'" + tmpImport + "', ";

                        tmpImport = province25;
                        if (tmpImport.Length > 2) tmpImport = tmpImport.Substring(0, 2);
                        sql = sql + "'" + tmpImport + "', ";

                        tmpImport = colToken[26].Trim().Replace(";", "").Replace("'", "");
			if(tmpImport=="ประธาน") tmpImport="P";
			if(tmpImport=="รองประธาน")tmpImport="V";
			if(tmpImport=="เลขานุการ")tmpImport="Y";
			if(tmpImport=="สมาชิก")tmpImport="Z";
                        if (tmpImport.Length > 1) tmpImport = tmpImport.Substring(0, 1);
			if (tmpImport.Trim()=="") tmpImport="Z";
                        sql = sql + "'" + tmpImport + "',";
                        tmpImport = colToken[27].Trim().Replace(";", "").Replace("'", "");
                        if (tmpImport.Length > 20) tmpImport = tmpImport.Substring(0, 20);
                        sql = sql + "'" + tmpImport + "',";
                        tmpImport = colToken[28].Trim().Replace(";", "").Replace("'", "");
                        if (tmpImport.Length > 20) tmpImport = tmpImport.Substring(0, 20);
                        sql = sql + "'" + tmpImport + "',";

                        //Append create/update time
                        sql += " '" + Session["uID"].ToString().Trim().Replace("'", "''") + "', ";
                        sql += " '" + Request.UserHostAddress + "', ";
                        sql += " '" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0') + DateTime.Now.Hour.ToString().PadLeft(2, '0') + DateTime.Now.Minute.ToString().PadLeft(2, '0') + DateTime.Now.Second.ToString().PadLeft(2, '0') + "', ";
                        sql += " '" + Session["uID"].ToString().Trim().Replace("'", "''") + "', ";
                        sql += " '" + Request.UserHostAddress + "', ";
                        sql += " '" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0') + DateTime.Now.Hour.ToString().PadLeft(2, '0') + DateTime.Now.Minute.ToString().PadLeft(2, '0') + DateTime.Now.Second.ToString().PadLeft(2, '0') + "'); ";
                    }
                    else
                    {
                        sql = sql + "UPDATE tb_train_detail SET ";

                        tmpImport = colToken[26].Trim().Replace(";", "").Replace("'", "");
			if(tmpImport=="ประธาน") tmpImport="P";
			if(tmpImport=="รองประธาน")tmpImport="V";
			if(tmpImport=="เลขานุการ")tmpImport="Y";
			if(tmpImport=="สมาชิก")tmpImport="Z";
                        if (tmpImport.Length > 1) tmpImport = tmpImport.Substring(0, 1);
			if (tmpImport.Trim()=="") tmpImport="Z";
                        sql = sql + " fl_pos='" + tmpImport + "',";
                        tmpImport = colToken[27].Trim().Replace(";", "").Replace("'", "");
                        if (tmpImport.Length > 20) tmpImport = tmpImport.Substring(0, 20);
                        sql = sql + " fl_start='" + tmpImport + "',";
                        tmpImport = colToken[28].Trim().Replace(";", "").Replace("'", "");
                        if (tmpImport.Length > 20) tmpImport = tmpImport.Substring(0, 20);
                        sql = sql + " fl_stop='" + tmpImport + "',";

                        sql += " fl_update_by ='" + Session["uID"].ToString().Trim().Replace("'", "''") + "', ";
                        sql += " fl_update_ip ='" + Request.UserHostAddress + "', ";
                        sql += " fl_update_time ='" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0') + DateTime.Now.Hour.ToString().PadLeft(2, '0') + DateTime.Now.Minute.ToString().PadLeft(2, '0') + DateTime.Now.Second.ToString().PadLeft(2, '0') + "', ";

                        tmpImport = changeDigit(colToken[0].Trim().Replace(";", "").Replace("'", "").Replace("-", "").Replace(".", "").Replace(" ", ""));
                        if (tmpImport.Length > 20) tmpImport = tmpImport.Substring(0, 20);
                        sql += "where fl_citizen_id ='" + tmpImport + "' ";
                        tmpImport = colToken[21].Trim().Replace(";", "").Replace("'", "");
                        if (tmpImport.Length > 255) tmpImport = tmpImport.Substring(0, 255);
                        sql += " and fl_course='" + tmpImport + "'";
                        tmpImport = changeDigit(colToken[22].Trim().Replace(";", "").Replace("'", "").Replace("-", "").Replace(".", "").Replace(" ", ""));
                        if (tmpImport.Length > 5) tmpImport = tmpImport.Substring(0, 5);
                        sql += " and fl_gen='" + tmpImport + "'";
                        tmpImport = changeDigit(colToken[23].Trim().Replace(";", "").Replace("'", "").Replace("-", "").Replace(".", "").Replace(" ", ""));
                        if (tmpImport.Length > 4) tmpImport = tmpImport.Substring(0, 4);
                        sql += " and fl_year='" + tmpImport + "'";
                        tmpImport = transDept(colToken[24].Trim().Replace(";", "").Replace("'", ""));
                        if (tmpImport.Length > 3) tmpImport = tmpImport.Substring(0, 3);
                        sql += " and isnull(fl_dept,'')='" + tmpImport + "'";
                        tmpImport = province25;
                        if (tmpImport.Length > 2) tmpImport = tmpImport.Substring(0, 2);
                        sql += " and fl_province_code='" + tmpImport + "';";
                    }
                    #endregion
                    try
                    {
                        command.CommandText = sql;
                        command.ExecuteNonQuery();
                        sql = "";
                        successLine++;
                    }
                    catch (Exception eex)
                    {
                        errLine += LineNo.ToString() + ",";
                    }
                }
                else
                {
                    errLine += LineNo.ToString() + ",";
                }
            }
            sql += "INSERT INTO tb_LOG(fl_id,fl_module,fl_action,fl_keyword,fl_datetime,fl_ip) VALUES(";
            sql += " '" + Session["uID"].ToString().Replace("'", "''") + "', ";
            sql += " 'TRAIN MEMBER', ";
            sql += " 'IMPORT', ";
            sql += " '', ";
            sql += " '" + DateTime.Now.Year + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0') + DateTime.Now.Hour.ToString().PadLeft(2, '0') + DateTime.Now.Minute.ToString().PadLeft(2, '0') + DateTime.Now.Second.ToString().PadLeft(2, '0') + "', ";
            sql += " '" + Request.UserHostAddress + "'); ";

            sql += "DELETE from tb_CITIZEN where fl_citizen_id=''; ";
            sql += "DELETE from tb_train_detail where fl_citizen_id = '' or fl_citizen_id not in (select distinct fl_citizen_id from tb_citizen) ";
            sql = sql + " or fl_course=''";
            sql = sql + " or fl_gen=''";
            sql = sql + " or fl_year=''";
            sql = sql + " or fl_dept=''";
            sql = sql + " or fl_province_code='';";

            command.CommandText = sql;
            command.ExecuteNonQuery();
            Conn.Close();
        }

        lblResponse.Text = "นำเข้าข้อมูลสำเร็จ " + successLine.ToString() + " บรรทัด";
        if (errLine.Length > 0) lblResponse.Text += " พบปัญหาในข้อมูล Excel บรรทัดที่ " + errLine.Substring(0, errLine.Length - 1);
        lblResponse.ForeColor = System.Drawing.Color.Green;
        lblResponse.Visible = true;

        userDataSet(null);
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        clearBox(false);
        userDataSet(null);
    }
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        bool errFound=false;
        if(txtCardID.Text=="")
        {
            lblResponse.Text="บันทึกข้อมูลก่อน";
            lblResponse.ForeColor = System.Drawing.Color.Red;
            lblResponse.Visible =true;
            errFound = true;
        }
        if(imgFile.PostedFile.FileName=="")
        {
            lblResponse.Text = "ไม่มีไฟล์ภาพ";
            lblResponse.ForeColor = System.Drawing.Color.Red;
            lblResponse.Visible = true;
            errFound = true;
        }
        if(!System.IO.Directory.Exists(Server.MapPath("CIMG")))
        {
            lblResponse.Text = "ไม่มีพื้นที่จัดเก็บไฟล์";
            lblResponse.ForeColor = System.Drawing.Color.Red;
            lblResponse.Visible = true;
            errFound = true;
        }

        if(errFound)
        {
            boxSet(hidID.Value);
            userDataSet(null);
            return;
        }
        try
        {
            imgFile.PostedFile.SaveAs(Server.MapPath("CIMG") + "\\" + txtCardID.Text.Trim() + ".jpg");
            OleDbConnection Conn=new OleDbConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString);
            OleDbCommand command=new OleDbCommand();

            Conn.Open();
            command.Connection = Conn;

            string sql="";
            sql = "INSERT INTO tb_LOG(fl_email,fl_module,fl_action,fl_keyword,fl_date,fl_machine) VALUES(";
            sql = sql + " '" + Session["uID"].ToString().Replace("'", "''") + "', ";
            sql = sql + " 'IMAGE FILE', ";
            sql = sql + " 'UPLOAD', ";
            sql = sql + " '" + txtCardID.Text.Trim().Replace("'", "''") + "', ";
            sql = sql + " '" + DateTime.Now.Year + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0') + DateTime.Now.Hour.ToString().PadLeft(2, '0') + DateTime.Now.Minute.ToString().PadLeft(2, '0') + DateTime.Now.Second.ToString().PadLeft(2, '0') + "', ";
            sql = sql + " '" + Request.UserHostAddress + "') ";
            command.CommandText = sql;
            command.ExecuteNonQuery();
            Conn.Close();
        }catch(Exception ex){}

        boxSet(hidID.Value);

        userDataSet(null);
        lblResponse.Text = "จัดเก็บภาพสำเร็จ";
        lblResponse.ForeColor = System.Drawing.Color.Green;

    }
    protected void txtCardID_TextChanged(object sender, EventArgs e)
    {
        txtCardID.Text = txtCardID.Text.Trim().Replace(";", "");
        boxSet(txtCardID.Text,1);
        userDataSet(this.txtCardID.Text);
    }
    protected void btnDel_Click(object sender, EventArgs e)
    {
        OleDbConnection Conn = new OleDbConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString);
        OleDbCommand command = new OleDbCommand();
        Conn.Open();
        command.Connection = Conn;
        string sql = "DELETE from tb_citizen ";
        sql = sql + " where fl_citizen_id='" + txtCardID.Text.Trim().Replace(";", "").Replace("'", "") + "' ";


        //sql += "INSERT INTO tb_LOG(fl_id,fl_module,fl_action,fl_keyword,fl_datetime,fl_ip) VALUES(";
        //sql += " '" + Session["uID"].ToString().Replace("'", "''") + "', ";
        //sql += " 'MEMBER ENTRY', ";
        //sql += " 'DELETE', ";
        //sql += " '" + txtCardID.Text + "," + cmbCourse.SelectedValue.Trim().Replace(";", "").Replace("'", "") + "," + cmbGen.SelectedValue.Trim().Replace(";", "").Replace("'", "") + "," + cmbYear.SelectedValue.Trim().Replace(";", "").Replace("'", "") + "," + cmbTrainProvince.SelectedValue.Trim().Replace(";", "").Replace("'", "") + "," + cmbDept.SelectedValue.Trim().Replace(";", "").Replace("'", "") + "', ";
        //sql += " '" + DateTime.Now.Year + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0') + DateTime.Now.Hour.ToString().PadLeft(2, '0') + DateTime.Now.Minute.ToString().PadLeft(2, '0') + DateTime.Now.Second.ToString().PadLeft(2, '0') + "', ";
        //sql += " '" + Request.UserHostAddress + "'); ";

        command.CommandText = sql;
        command.ExecuteNonQuery();
        Conn.Close();
        userDataSet(this.txtCardID.Text);
        txtCardID.Text = "";
        clearBox(true);
    }
}