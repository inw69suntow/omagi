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
using System.IO;
using OfficeOpenXml;

public partial class _mass_member_entry: System.Web.UI.Page
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
        string strCheck = "����������";
        string strRes = "0123456789";
        string tmpParam = "";
        int tmpI = 0;
        string result="";
        for (int i = 0; i < param.Length; i++)
        {
            tmpParam = param.Substring(i, 1);
            tmpI=strCheck.IndexOf(tmpParam);
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
            if (param1.Length == 2) param1Flag=false;
        }
        catch (Exception ee) { }

        if (param1.Length < 2) return "";
        string result = "";

        OleDbConnection Conn = new OleDbConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString);
        OleDbCommand command = new OleDbCommand();

        string sql = "SELECT distinct fl_province_code,fl_province_name from tb_moicode where fl_status='1' ";
        if(param1Flag)
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

    protected string transProvince(string param1,string param2)
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
        if(param1Flag)
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

    protected string transProvince(string param1, string param2,string param3)
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

        cmbStartDD.Items.Clear();
        cmbStartMM.Items.Clear();
        cmbStartYY.Items.Clear();

        cmbStopDD.Items.Clear();
        cmbStopMM.Items.Clear();
        cmbStopYY.Items.Clear();

        cmbBDD.Items.Add(new ListItem("", ""));
        cmbBMM.Items.Add(new ListItem("", ""));
        cmbBYY.Items.Add(new ListItem("", ""));

        cmbStartDD.Items.Add(new ListItem("", ""));
        cmbStartMM.Items.Add(new ListItem("", ""));
        cmbStartYY.Items.Add(new ListItem("", ""));

        cmbStopDD.Items.Add(new ListItem("", ""));
        cmbStopMM.Items.Add(new ListItem("", ""));
        cmbStopYY.Items.Add(new ListItem("", ""));

        for (int i = 1; i <= 31; i++)
        {
            cmbBDD.Items.Add(new ListItem(i.ToString().PadLeft(2,'0'),i.ToString().PadLeft(2,'0')));
            cmbStartDD.Items.Add(new ListItem(i.ToString().PadLeft(2,'0'),i.ToString().PadLeft(2,'0')));
            cmbStopDD.Items.Add(new ListItem(i.ToString().PadLeft(2,'0'),i.ToString().PadLeft(2,'0')));
        }

        for (int i = 1; i <= 12; i++)
        {
            string nameVal="";

            if(i==1) nameVal="���Ҥ�";
            if(i==2) nameVal="����Ҿѹ��";
            if(i==3) nameVal="�չҤ�";
            if(i==4) nameVal="����¹";
            if(i==5) nameVal="����Ҥ�";
            if(i==6) nameVal="�Զع�¹";
            if(i==7) nameVal="�á�Ҥ�";
            if(i==8) nameVal="�ԧ�Ҥ�";
            if(i==9) nameVal="�ѹ��¹";
            if(i==10) nameVal="���Ҥ�";
            if(i==11) nameVal="��Ȩԡ�¹";
            if(i==12) nameVal="�ѹ�Ҥ�";

            cmbBMM.Items.Add(new ListItem(nameVal,i.ToString().PadLeft(2,'0')));
            cmbStartMM.Items.Add(new ListItem(nameVal,i.ToString().PadLeft(2, '0')));
            cmbStopMM.Items.Add(new ListItem(nameVal,i.ToString().PadLeft(2, '0')));
        }

        for (int i = 1900; i <= DateTime.Now.Year; i++)
        {
            int j = i + 543;
            cmbBYY.Items.Add(new ListItem(j.ToString(), i.ToString()));
            cmbStartYY.Items.Add(new ListItem(j.ToString(), i.ToString()));
            cmbStopYY.Items.Add(new ListItem(j.ToString(), i.ToString()));
        }
    }

    protected void titleDataSet()
    {
        cmbTitle.Items.Clear();
        cmbTitle.Items.Add(new ListItem("����˹�", ""));

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

        cmbProvince.Items.Add(new ListItem("����˹�", ""));

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
        cmbDistrict.Items.Add(new ListItem("����˹�", ""));

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
        cmbTambon.Items.Add(new ListItem("����˹�", ""));

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

    protected void clearBox()
    {
        hidID.Value = "";
        txtCardID.Text = "";
        cmbTitle.SelectedValue = "";
        txtFName.Text = "";
        txtSName.Text = "";

        txtAddrNo.Text = "";
        txtHome.Text = "";
        txtSoy.Text = "";
        txtRoad.Text = "";

        lblCreate.Text = "";
        lblUpdate.Text = "";

        //cmbProvince.SelectedValue = "";
        //cmbDistrict.Items.Clear();
        //cmbDistrict.Items.Add(new ListItem("����˹�", ""));
        //cmbTambon.Items.Clear();
        //cmbTambon.Items.Add(new ListItem("����˹�", ""));

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

        txtMemID.Text = "";
        //cmbPos.SelectedIndex = 0;

        //cmbStartDD.SelectedIndex = 0;
        //cmbStartMM.SelectedIndex = 0;
        //cmbStartYY.SelectedIndex = 0;

        cmbStopDD.SelectedIndex = 0;
        cmbStopMM.SelectedIndex = 0;
        cmbStopYY.SelectedIndex = 0;

        imgLink.ImageUrl = "CIMG/blank.gif";
    }

    protected void boxSet(string hid,int flag)
    {
        string sql = "SELECT distinct ";
        sql += " isnull(fl_citizen_id,''), ";
        sql += " isnull(fl_mem_id,''), ";
        sql += " isnull(fl_title,''), ";
        sql += " isnull(fl_fname,''), ";
        sql += " isnull(fl_sname,''), ";
        sql += " isnull(fl_addrno,''), ";
        sql += " isnull(fl_moono,''), ";
        sql += " isnull(fl_home,''), ";
        sql += " isnull(fl_soy,''), ";
        sql += " isnull(fl_road,''), ";
        sql += " isnull(fl_province_code,''), ";
        sql += " isnull(fl_district,''), ";
        sql += " isnull(fl_tambon,''), ";
        sql += " isnull(fl_postcode,''), ";
        sql += " isnull(fl_telno,''), ";
        sql += " isnull(fl_offno,''), ";
        sql += " isnull(fl_mobno,''), ";
        sql += " isnull(fl_email,''), ";
        sql += " isnull(fl_birth,''), ";
        sql += " isnull(fl_targetFlag,''), ";
        sql += " isnull(fl_status,''), ";
        sql += " isnull(fl_google,'') ";

        sql += " from tb_citizen  ";
        sql += " where fl_citizen_id='" + hid.Replace(";", "").Replace("'", "''") + "' ";

        OleDbConnection Conn = new OleDbConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString);
        OleDbCommand command = new OleDbCommand();

        Conn.Open();
        command.Connection = Conn;
        command.CommandText = sql.Replace(";", "");
        OleDbDataReader rs = command.ExecuteReader();
        if (rs.Read())
        {
            hidID.Value = hid.Trim();
            txtCardID.Text = rs.GetString(0);
            if (System.IO.File.Exists(Server.MapPath("CIMG") + "\\" + txtCardID.Text + ".jpg"))
            {
                imgLink.ImageUrl = "CIMG/" + txtCardID.Text + ".jpg";
            }
            else
            {
                imgLink.ImageUrl = "CIMG/blank.gif";
            }
            txtMemID.Text = rs.GetString(1);

            if (rs.GetString(2).Trim() != "") cmbTitle.SelectedValue = rs.GetString(2).Trim();

            txtFName.Text = rs.GetString(3);
            txtSName.Text = rs.GetString(4);
            txtAddrNo.Text = rs.GetString(5);
            txtMoo.Text = rs.GetString(6);
            txtHome.Text = rs.GetString(7);
            txtSoy.Text = rs.GetString(8);
            txtRoad.Text = rs.GetString(9);

            if (rs.GetString(10).Trim() != "") cmbProvince.SelectedValue = rs.GetString(10).Trim();
            districtDataSet();
            if (rs.GetString(11).Trim() != "") cmbDistrict.SelectedValue = rs.GetString(11).Trim();
            tambonDataSet();
            if (rs.GetString(12).Trim() != "") cmbTambon.SelectedValue = rs.GetString(12).Trim();

            txtPost.Text = rs.GetString(13);
            txtTelNo.Text = rs.GetString(14);
            txtOffNo.Text = rs.GetString(15);
            txtMobNo.Text = rs.GetString(16);
            txtEmail.Text = rs.GetString(17);

            if (rs.GetString(18).Trim() != "")
            {
                cmbBYY.SelectedValue = rs.GetString(18).Substring(0, 4);
                cmbBMM.SelectedValue = rs.GetString(18).Substring(4, 2);
                cmbBDD.SelectedValue = rs.GetString(18).Substring(6, 2);
            }

            if (rs.GetString(19) == "1") chkTargetFlag.Checked = true;
            if (rs.GetString(20) != "1") chkStatus.Checked = true;

            txtGoogle.Text = rs.GetString(21).Trim();
        }
        rs.Close();
        Conn.Close();
    }

    protected void boxSet(string hid)
    {
        clearBox();
        string sql = "SELECT distinct ";
        sql += " isnull(a.fl_citizen_id,''), ";
        sql += " isnull(a.fl_mem_id,''), ";
        sql += " isnull(fl_title,''), ";
        sql += " isnull(fl_fname,''), ";
        sql += " isnull(fl_sname,''), ";
        sql += " isnull(fl_addrno,''), ";
        sql += " isnull(fl_moono,''), ";
        sql += " isnull(fl_home,''), ";
        sql += " isnull(fl_soy,''), ";
        sql += " isnull(fl_road,''), ";
        sql += " isnull(fl_province_code,''), ";
        sql += " isnull(fl_district,''), ";
        sql += " isnull(fl_tambon,''), ";
        sql += " isnull(fl_postcode,''), ";
        sql += " isnull(fl_telno,''), ";
        sql += " isnull(fl_offno,''), ";
        sql += " isnull(fl_mobno,''), ";
        sql += " isnull(fl_email,''), ";
        sql += " isnull(fl_birth,''), ";
        sql += " isnull(fl_targetFlag,''), ";
        sql += " isnull(fl_status,''), ";
        sql += " isnull(fl_google,''), ";

        sql += " isnull(fl_pos,'Z'), ";
        sql += " isnull(fl_start,''), ";
        sql += " isnull(fl_stop,''), ";

        sql += " isnull(a.fl_create_time,''), ";
        sql += " isnull(a.fl_update_time,'') ";

        sql += " from tb_membergroup a left join tb_citizen b  ";
        sql += " on a.fl_citizen_id = b.fl_citizen_id  ";
        sql += " where a.fl_citizen_id='" + hid.Replace(";","").Replace("'","''") + "' ";
        sql += " and a.fl_detailgroup_id='" + Request.QueryString["id"].ToString().Trim().Replace(";", "").Replace("'", "''") + "' ";

        OleDbConnection Conn = new OleDbConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString);
        OleDbCommand command = new OleDbCommand();

        Conn.Open();
        command.Connection = Conn;
        command.CommandText = sql.Replace(";","");
        OleDbDataReader rs = command.ExecuteReader();
        if (rs.Read())
        {
            hidID.Value = hid.Trim();
            txtCardID.Text = rs.GetString(0);
            if (System.IO.File.Exists(Server.MapPath("CIMG") + "\\" + txtCardID.Text + ".jpg"))
            {
                imgLink.ImageUrl = "CIMG/" + txtCardID.Text + ".jpg";
            }
            else
            {
                imgLink.ImageUrl = "CIMG/blank.gif";
            }
            txtMemID.Text = rs.GetString(1);

            if (rs.GetString(2).Trim() != "") cmbTitle.SelectedValue = rs.GetString(2).Trim();

            txtFName.Text = rs.GetString(3);
            txtSName.Text = rs.GetString(4);
            txtAddrNo.Text = rs.GetString(5);
            txtMoo.Text = rs.GetString(6);
            txtHome.Text = rs.GetString(7);
            txtSoy.Text = rs.GetString(8);
            txtRoad.Text = rs.GetString(9);

            if (rs.GetString(10).Trim() != "") cmbProvince.SelectedValue = rs.GetString(10).Trim();
            districtDataSet();
            if (rs.GetString(11).Trim() != "") cmbDistrict.SelectedValue = rs.GetString(11).Trim();
            tambonDataSet();
            if (rs.GetString(12).Trim() != "") cmbTambon.SelectedValue = rs.GetString(12).Trim();

            txtPost.Text = rs.GetString(13);
            txtTelNo.Text = rs.GetString(14);
            txtOffNo.Text = rs.GetString(15);
            txtMobNo.Text = rs.GetString(16);
            txtEmail.Text = rs.GetString(17);

            if (rs.GetString(18).Trim() != "")
            {
                cmbBYY.SelectedValue = rs.GetString(18).Substring(0, 4);
                cmbBMM.SelectedValue = rs.GetString(18).Substring(4, 2);
                cmbBDD.SelectedValue = rs.GetString(18).Substring(6, 2);
            }

            if (rs.GetString(19) == "1") chkTargetFlag.Checked = true;
            if (rs.GetString(20) != "1") chkStatus.Checked = true;

            txtGoogle.Text = rs.GetString(21).Trim();

            if (rs.GetString(22).Trim() != "") cmbPos.SelectedValue = rs.GetString(22).Trim();

            if (rs.GetString(23).Trim() != "")
            {
                if (rs.GetString(23).Trim() != "00000000000000")
                {
                    cmbStartYY.SelectedValue = rs.GetString(23).Substring(0, 4);
                    cmbStartMM.SelectedValue = rs.GetString(23).Substring(4, 2);
                    cmbStartDD.SelectedValue = rs.GetString(23).Substring(6, 2);
                }
            }

            if (rs.GetString(24).Trim() != "")
            {
                if (rs.GetString(24).Trim() != "00000000000000")
                {
                    cmbStopYY.SelectedValue = rs.GetString(24).Substring(0, 4);
                    cmbStopMM.SelectedValue = rs.GetString(24).Substring(4, 2);
                    cmbStopDD.SelectedValue = rs.GetString(24).Substring(6, 2);
                }
            }
            if (rs.GetString(25) != "") lblCreate.Text = rs.GetString(25).Substring(6, 2) + "-" + rs.GetString(25).Substring(4, 2) + "-" + rs.GetString(25).Substring(0, 4) + " " + rs.GetString(25).Substring(8, 2) + ":" + rs.GetString(25).Substring(10, 2) + ":" + rs.GetString(25).Substring(12, 2);
            if (rs.GetString(26) != "") lblUpdate.Text = rs.GetString(26).Substring(6, 2) + "-" + rs.GetString(26).Substring(4, 2) + "-" + rs.GetString(26).Substring(0, 4) + " " + rs.GetString(26).Substring(8, 2) + ":" + rs.GetString(26).Substring(10, 2) + ":" + rs.GetString(26).Substring(12, 2);
        }
        rs.Close();
        Conn.Close();
    }

    protected void setBar(string id)
    {
        string sql = "";
        lblGroupName.Text = "";
        idID.Value = "";

        sql = "SELECT distinct ";
        sql += " case isnull(a.fl_groupname,'') when '' then isnull(b.fl_group_name,'') else isnull(a.fl_groupname,'') end, ";
        sql += " case isnull(fl_province_name,'') when '' then isnull(fl_province,'') else isnull(fl_province_name,'') end ";
        sql += " FROM tb_detailgroup a ";
        sql += " left join tb_maingroup b  on a.fl_group_id = b.fl_group_id ";
        sql += " left join tb_moicode c on a.fl_province = c.fl_province_code ";
        sql += " where a.fl_id = '" + id.Trim().Replace(";", "").Replace("'", "''") + "' ";
        OleDbConnection Conn = new OleDbConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString);
        OleDbCommand command = new OleDbCommand();
        Conn.Open();
        command.Connection = Conn;
        command.CommandText = sql.Replace(";", "");
        OleDbDataReader rs = command.ExecuteReader();
        if (rs.Read())
        {
            lblGroupName.Text = rs.GetString(0) + " " + rs.GetString(1);
            idID.Value = id;
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
        //Inject Client Script
        HtmlGenericControl body = (HtmlGenericControl)Master.FindControl("masterBody");
        body.Attributes.Add("onload", "initialize()");

        if (!Page.IsPostBack)
        {
            dateDataSet();
            titleDataSet();
            provinceDataSet();

            if (Request.QueryString["id"] != null)
            {
                if (Request.QueryString["id"].Trim() != "") setBar(Request.QueryString["id"].ToString());
            }

            if (Request.QueryString["hid"] != null)
            {
                if (Request.QueryString["hid"].Trim() != "") boxSet(Request.QueryString["hid"].ToString());
            }
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
        dtGrid.Rows[0].Cells[0].InnerHtml = "���";
        dtGrid.Rows[0].Cells[0].Width = "1%";
        dtGrid.Rows[0].Cells.Add(new HtmlTableCell());
        dtGrid.Rows[0].Cells[1].Width = "15%";
        dtGrid.Rows[0].Cells[1].InnerHtml = "<center>�Ţ��Шӵ��</center>";
        dtGrid.Rows[0].Cells.Add(new HtmlTableCell());
        dtGrid.Rows[0].Cells[2].Width = "25%";
        dtGrid.Rows[0].Cells[2].InnerHtml = "<center>����</center>";
        dtGrid.Rows[0].Cells.Add(new HtmlTableCell());
        dtGrid.Rows[0].Cells[3].Width = "25%";
        dtGrid.Rows[0].Cells[3].InnerHtml = "<center>ʡ��</center>";
        dtGrid.Rows[0].Cells.Add(new HtmlTableCell());
        dtGrid.Rows[0].Cells[4].Width = "20%";
        dtGrid.Rows[0].Cells[4].InnerHtml = "<center>�ѧ��Ѵ</center>";
        dtGrid.Rows[0].Cells.Add(new HtmlTableCell());
        dtGrid.Rows[0].Cells[5].Width = "10%";
        dtGrid.Rows[0].Cells[5].InnerHtml = "<center>���Ѿ����Ͷ��</center>";
        dtGrid.Rows[0].Cells.Add(new HtmlTableCell());
        dtGrid.Rows[0].Cells[6].Width = "10%";
        dtGrid.Rows[0].Cells[6].InnerHtml = "<center>������</center>";

        string sql = "SELECT distinct b.fl_citizen_id,";
        sql = sql + " fl_fname, ";
        sql = sql + " fl_sname, ";
        sql = sql + " case isnull(fl_province_name,'') when '' then isnull(b.fl_province_code,'') else isnull(fl_province_name,'') end provinceName, ";
        sql = sql + " fl_mobNo, ";
        sql = sql + " fl_eMail, ";

        //For coloring
        sql = sql + " isnull(b.fl_status,'0') as fl_status ";

        sql = sql + " FROM tb_membergroup a ";
        sql = sql + " inner join tb_citizen b  on a.fl_citizen_id = b.fl_citizen_id ";
        sql = sql + " left join tb_moicode c on b.fl_province_code = c.fl_province_code ";

        sql = sql + " where 1=1 ";
        sql = sql + " and fl_detailgroup_id = '" + idID.Value.Trim().Replace(";","").Replace("'","''") + "' ";

        //Check filter command
        string[] tmpStringToken;
        if (txtKeyword.Text.Trim()!="")
        {
            tmpStringToken = txtKeyword.Text.Split(' ');
            foreach (string tmp in tmpStringToken)
            {
                sql = sql + " and (";
                sql = sql + " b.fl_fname like '%" + tmp.Replace("'", "''") + "%' ";
                sql = sql + " or b.fl_sname like '%" + tmp.Replace("'", "''") + "%' ";
                sql = sql + " or b.fl_addrno like '%" + tmp.Replace("'", "''") + "%' ";
                sql = sql + " or b.fl_telno like '%" + tmp.Replace("'", "''") + "%' ";
                sql = sql + " or b.fl_offno like '%" + tmp.Replace("'", "''") + "%' ";
                sql = sql + " or b.fl_mobno like '%" + tmp.Replace("'", "''") + "%' ";
                sql = sql + " or b.fl_email like '%" + tmp.Replace("'", "''") + "%' ";
                sql = sql + ") ";
            }
        }

        //if (hidID.Value != "") sql += " and b.fl_citizen_id='" + hidID.Value + "' ";

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
        Session["userSQL"]= sql.Replace(";", "");

        rs = command.ExecuteReader();
        while (i < (curPage - 1) * pageSize) { rs.Read(); i++; }
        i = 1;
        while ((rs.Read()) && (i <= pageSize))
        {
            string action = "document.location='mass_member_entry.aspx?id=" + Request.QueryString["id"].ToString() + "&hid=" + rs.GetString(0) + "';";
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

            if (hidID.Value != rs.GetString(0)) dtGrid.Rows[i].Cells[0].InnerHtml = "<input type='checkbox' id='check_" + i.ToString() + "' onClick=\"" + action + "\">"; else dtGrid.Rows[i].Cells[0].InnerHtml = "<input type='checkbox' id='check_" + i.ToString() + "' checked onClick=\"" + action + "\">";

            for (int j = 0; j < 6; j++)
            {
                dtGrid.Rows[i].Cells.Add(new HtmlTableCell());
                dtGrid.Rows[i].Cells[j+1].Align = "LEFT";
                dtGrid.Rows[i].Cells[j+1].InnerHtml = rs.GetString(j);
            }

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
        clearBox();
        userDataSet();
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
        sql += " isnull(fl_province_name,''), ";
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
        
        sql += " isnull(a.fl_mem_id,''), ";
        sql += " isnull(fl_pos,'Z'), ";
        sql += " isnull(fl_start,''), ";
        sql += " isnull(fl_stop,'') ";
        sql += " from tb_membergroup a inner join tb_citizen b  ";
        sql += " on a.fl_citizen_id = b.fl_citizen_id  ";
        
        //Update 01-10-2012
        sql += " left outer join (";
        sql += " select distinct fl_code,fl_title ";
        sql += " from tb_title ";
        sql += " ) title on b.fl_title=title.fl_code ";

        sql += " left outer join (";
        sql += " select distinct fl_province_code,fl_province_name ";
        sql += " from tb_moicode where fl_status='1' ";
        sql += " ) province on b.fl_province_code=province.fl_province_code ";

        sql += " left outer join (";
        sql += " select distinct fl_district_code,fl_district_name ";
        sql += " from tb_moicode where fl_status='1' ";
        sql += " ) district on b.fl_district=district.fl_district_code ";

        sql += " left outer join (";
        sql += " select distinct fl_tambon_code,fl_tambon_name ";
        sql += " from tb_moicode where fl_status='1' ";
        sql += " ) tambon on b.fl_tambon=tambon.fl_tambon_code ";

        sql += " where a.fl_detailgroup_id='" + Request.QueryString["id"].ToString().Trim().Replace(";", "").Replace("'", "''") + "' ";

        //Check filter command
        string[] tmpStringToken;
        if (txtKeyword.Text.Trim() != "")
        {
            tmpStringToken = txtKeyword.Text.Split(' ');
            foreach (string tmp in tmpStringToken)
            {
                sql = sql + " and (";
                sql = sql + " b.fl_fname like '%" + tmp.Replace("'", "''") + "%' ";
                sql = sql + " or b.fl_sname like '%" + tmp.Replace("'", "''") + "%' ";
                sql = sql + " or b.fl_telno like '%" + tmp.Replace("'", "''") + "%' ";
                sql = sql + " or b.fl_offno like '%" + tmp.Replace("'", "''") + "%' ";
                sql = sql + " or b.fl_mobno like '%" + tmp.Replace("'", "''") + "%' ";
                sql = sql + " or b.fl_email like '%" + tmp.Replace("'", "''") + "%' ";
                sql = sql + ") ";
            }
        }

        if (hidID.Value != "") sql += " and a.fl_citizen_id='" + hidID.Value + "' ";

        command.CommandText = sql;
        command.Connection = Conn;

        string fName = DateTime.Now.Year + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0') + DateTime.Now.Hour.ToString().PadLeft(2, '0') + DateTime.Now.Minute.ToString().PadLeft(2, '0') + DateTime.Now.Second.ToString().PadLeft(2, '0');
        fName = fName + "_" + Session["uID"].ToString() + "_massMember";
        if (fName.Length > 100) fName = fName.Substring(0, 100);
        fName = fName + ".csv";

        System.IO.StreamWriter sw = new System.IO.StreamWriter(Server.MapPath("CSV") + "\\" + fName, false, Encoding.Default);
        OleDbDataReader rs = command.ExecuteReader();
        string tmpLine = "";
        tmpLine = tmpLine + "\"�ѵû�ЪҪ�\",\"�ӹ�˹��\",\"����\",\"ʡ��\",\"��ҹ�Ţ���\",\"�����ҹ\",\"���\",\"���\",\"���ʨѧ��Ѵ\",\"���������\",\"���ʵӺ�\",\"������\",\"������ɳ���\",\"���Ѿ���ҹ\",\"���Ѿ����ӧҹ\",\"���Ѿ����Ͷ��\",\"������\",\"�ѹ�Դ\",\"��Ū��������\",\"ʶҹ�\",\"���ʾԡѴ google\",\"�����Ţ��Ҫԡ\",\"���˹�\",\"�ѹ�������Ҫԡ�Ҿ\",\"�ѹ����ش��Ҫԡ�Ҿ\"";
        sw.WriteLine(tmpLine);
        while (rs.Read())
        {
            tmpLine = "";
            for (int i = 0; i <= 24; i++)
            {
                tmpLine = tmpLine + "\"'" + rs.GetString(i) + "\"";
                if (i < 24) tmpLine = tmpLine + ",";
            }
            sw.WriteLine(tmpLine);
        }
        rs.Close();
        sw.Close();
        sql = "";
        sql += "INSERT INTO tb_LOG(fl_id,fl_module,fl_action,fl_keyword,fl_datetime,fl_ip) VALUES(";
        sql += " '" + Session["uID"].ToString().Replace("'", "''") + "', ";
        sql += " 'MASS MEMBER', ";
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
        userDataSet();
    }
    protected void cmbDistrict_SelectedIndexChanged(object sender, EventArgs e)
    {
        tambonDataSet();
        userDataSet();
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        lblResponse.Text = "";

        if (txtCardID.Text == "")
        {
            //lblResponse.Text = "������Ţ���ѵû�ЪҪ�";
            //lblResponse.ForeColor = System.Drawing.Color.Red;
            //lblResponse.Visible = true;
            //userDataSet();
            //return;

            txtCardID.Text = transID(txtFName.Text.Trim().Replace(";", "").Replace("'", ""), txtSName.Text.Trim().Replace(";", "").Replace("'", ""));
            if (txtCardID.Text.Trim() == "")
            {
                lblResponse.Text = "������Ţ���ѵû�ЪҪ�";
                lblResponse.ForeColor = System.Drawing.Color.Red;
                lblResponse.Visible = true;
                userDataSet();
                return;
            }
        }

        if (txtFName.Text == "")
        {
            lblResponse.Text = "����ժ�����Ҫԡ��Ū�";
            lblResponse.ForeColor = System.Drawing.Color.Red;
            lblResponse.Visible = true;
            userDataSet();
            return;
        }

        if (txtSName.Text == "")
        {
            lblResponse.Text = "����չ��ʶ����Ҫԡ��Ū�";
            lblResponse.ForeColor = System.Drawing.Color.Red;
            lblResponse.Visible = true;
            userDataSet();
            return;
        }

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

        string tmpDateStart = cmbStartYY.SelectedValue.Trim().Replace(";", "").Replace("'", "") + cmbStartMM.SelectedValue.Trim().Replace(";", "").Replace("'", "") + cmbStartDD.SelectedValue.Trim().Replace(";", "").Replace("'", "");
        if (tmpDateStart.Length < 8) tmpDateStart = "";
        if (tmpDateStart != "")
        {
            switch (cmbStartMM.SelectedValue.Trim())
            {
                case "02":
                    if (Convert.ToInt32(cmbStartYY.SelectedValue) % 4 == 0)
                    {
                        if (Convert.ToInt32(cmbStartDD.SelectedValue) > 29) cmbStartDD.SelectedValue = "29";
                    }
                    else
                    {
                        if (Convert.ToInt32(cmbStartDD.SelectedValue) > 28) cmbStartDD.SelectedValue = "28";
                    }
                    break;
                case "04":
                case "06":
                case "09":
                case "11":
                    if (Convert.ToInt32(cmbStartDD.SelectedValue) > 30) cmbStartDD.SelectedValue = "30";
                    break;
            }
            tmpDateStart = cmbStartYY.SelectedValue.Trim().Replace(";", "").Replace("'", "") + cmbStartMM.SelectedValue.Trim().Replace(";", "").Replace("'", "") + cmbStartDD.SelectedValue.Trim().Replace(";", "").Replace("'", "");
        }


        string tmpDateStop = cmbStopYY.SelectedValue.Trim().Replace(";", "").Replace("'", "") + cmbStopMM.SelectedValue.Trim().Replace(";", "").Replace("'", "") + cmbStopDD.SelectedValue.Trim().Replace(";", "").Replace("'", "");
        if (tmpDateStop.Length < 8) tmpDateStop = "";
        if (tmpDateStop != "")
        {
            switch (cmbStopMM.SelectedValue.Trim())
            {
                case "02":
                    if (Convert.ToInt32(cmbStopYY.SelectedValue) % 4 == 0)
                    {
                        if (Convert.ToInt32(cmbStopDD.SelectedValue) > 29) cmbStopDD.SelectedValue = "29";
                    }
                    else
                    {
                        if (Convert.ToInt32(cmbStopDD.SelectedValue) > 28) cmbStopDD.SelectedValue = "28";
                    }
                    break;
                case "04":
                case "06":
                case "09":
                case "11":
                    if (Convert.ToInt32(cmbStopDD.SelectedValue) > 30) cmbStopDD.SelectedValue = "30";
                    break;
            }
            tmpDateStop = cmbStopYY.SelectedValue.Trim().Replace(";", "").Replace("'", "") + cmbStopMM.SelectedValue.Trim().Replace(";", "").Replace("'", "") + cmbStopDD.SelectedValue.Trim().Replace(";", "").Replace("'", "");
        }
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
            sql = sql + " '" + changeDigit(txtTelNo.Text.Trim().Replace(";", "").Replace("'", "").Replace("-","").Replace("(","").Replace(")","").Replace(".","")) + "',";
            sql = sql + " '" + changeDigit(txtOffNo.Text.Trim().Replace(";", "").Replace("'", "").Replace("-","").Replace("(","").Replace(")","").Replace(".","")) + "',";
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
            sql = sql + " '" + txtGoogle.Text.Trim().Replace(";", "").Replace("'", "") + "'); ";

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
            sql = sql + " fl_telno='" + changeDigit(txtTelNo.Text.Trim().Replace(";", "").Replace("'", "").Replace("-","").Replace("(","").Replace(")","").Replace(".","")) + "',";
            sql = sql + " fl_offno='" + changeDigit(txtOffNo.Text.Trim().Replace(";", "").Replace("'", "").Replace("-","").Replace("(","").Replace(")","").Replace(".","")) + "',";
            sql = sql + " fl_mobno='" + changeDigit(txtMobNo.Text.Trim().Replace(";", "").Replace("'", "").Replace("-", "").Replace("(", "").Replace(")", "").Replace(".", "")) + "',";
            sql = sql + " fl_email='" + txtEmail.Text.Trim().Replace(";", "").Replace("'", "") + "',";
            sql = sql + " fl_birth='" + tmpDate + "',";
            if (chkTargetFlag.Checked) sql = sql + " fl_targetFlag='1',"; else sql = sql + " fl_targetFlag='0',";
            if (chkStatus.Checked) sql = sql + " fl_status='0',"; else sql = sql + " fl_status='1',";
            sql += " fl_update_by ='" + Session["uID"].ToString().Trim().Replace("'", "''") + "', ";
            sql += " fl_update_ip ='" + Request.UserHostAddress + "', ";
            sql += " fl_update_time ='" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0') + DateTime.Now.Hour.ToString().PadLeft(2, '0') + DateTime.Now.Minute.ToString().PadLeft(2, '0') + DateTime.Now.Second.ToString().PadLeft(2, '0') + "', ";
            sql = sql + " fl_google='" + txtGoogle.Text.Trim().Replace(";", "").Replace("'", "") + "' ";
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

        //Process membergroup
        #region memberProc
        procFlag = "UP";
        if (txtCardID.Text.Trim().Replace(";", "").Replace("'", "") == "")
        {
            procFlag = "NA";
        }
        else
        {
            sql2 = "SELECT distinct fl_citizen_id from tb_memberGroup ";
            sql2 += "where fl_citizen_id ='" + changeDigit(txtCardID.Text.Trim().Replace(";", "").Replace("'", "").Replace("-", "").Replace(".", "").Replace(" ", "")) + "' ";
            sql2 = sql2 + " and fl_detailgroup_id ='" + Request.QueryString["id"].Trim().Replace(";", "").Replace("'", "") + "'; ";
            command.CommandText = sql2;
            rs = command.ExecuteReader();
            if (!rs.Read()) procFlag = "IN";
            rs.Close();
        }

        if (procFlag == "IN")
        {
            sql = sql + "INSERT INTO tb_memberGroup (";
            sql = sql + " fl_detailgroup_id,";
            sql = sql + " fl_citizen_id,";
            sql = sql + " fl_mem_id,";
            sql = sql + " fl_pos,";
            sql = sql + " fl_start,";
            sql = sql + " fl_stop,";
            sql = sql + " fl_create_by,";
            sql = sql + " fl_create_ip,";
            sql = sql + " fl_create_time,";
            sql = sql + " fl_update_by,";
            sql = sql + " fl_update_IP,";
            sql = sql + " fl_update_time";

            sql = sql + " ) values ( ";

            sql = sql + "'" + Request.QueryString["id"].ToString() + "', ";
            sql = sql + "'" + changeDigit(txtCardID.Text.Trim().Replace(";", "").Replace("'", "").Replace("-", "").Replace(".", "").Replace(" ", "")) + "', ";
            sql = sql + "'" + changeDigit(txtMemID.Text.Trim().Replace(";", "").Replace("'", "")) + "', ";
            sql = sql + "'" + cmbPos.SelectedValue.Trim().Replace(";", "").Replace("'", "") + "', ";
            sql = sql + " '" + tmpDateStart + "',";
            sql = sql + " '" + tmpDateStop + "',";
            //Append create/update time
            sql += " '" + Session["uID"].ToString().Trim().Replace("'", "''") + "', ";
            sql += " '" + Request.UserHostAddress + "', ";
            sql += " '" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0') + DateTime.Now.Hour.ToString().PadLeft(2, '0') + DateTime.Now.Minute.ToString().PadLeft(2, '0') + DateTime.Now.Second.ToString().PadLeft(2, '0') + "', ";
            sql += " '" + Session["uID"].ToString().Trim().Replace("'", "''") + "', ";
            sql += " '" + Request.UserHostAddress + "', ";
            sql += " '" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0') + DateTime.Now.Hour.ToString().PadLeft(2, '0') + DateTime.Now.Minute.ToString().PadLeft(2, '0') + DateTime.Now.Second.ToString().PadLeft(2, '0') + "'); ";

            sql += "INSERT INTO tb_LOG(fl_id,fl_module,fl_action,fl_keyword,fl_datetime,fl_ip) VALUES(";
            sql += " '" + Session["uID"].ToString().Replace("'", "''") + "', ";
            sql += " 'MASS MEMBER', ";
            sql += " 'INSERT', ";
            sql += " '" + changeDigit(txtCardID.Text.Trim().Replace(";", "").Replace("'", "").Replace("-", "").Replace(".", "").Replace(" ", "")) + "," + Request.QueryString["id"].Trim().Replace(";", "").Replace("'", "") + "', ";
            sql += " '" + DateTime.Now.Year + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0') + DateTime.Now.Hour.ToString().PadLeft(2, '0') + DateTime.Now.Minute.ToString().PadLeft(2, '0') + DateTime.Now.Second.ToString().PadLeft(2, '0') + "', ";
            sql += " '" + Request.UserHostAddress + "'); ";
        }
        else
        {
            sql = sql + "UPDATE tb_membergroup SET ";
            sql = sql + " fl_mem_id='" + changeDigit(txtMemID.Text.Trim().Replace(";", "").Replace("'", "")) + "',";
            sql = sql + " fl_pos='" + cmbPos.SelectedValue.Trim().Replace(";", "").Replace("'", "") + "',";
            sql = sql + " fl_start='" + tmpDateStart + "',";
            sql = sql + " fl_stop='" + tmpDateStop + "',";
            sql += " fl_update_by ='" + Session["uID"].ToString().Trim().Replace("'", "''") + "', ";
            sql += " fl_update_ip ='" + Request.UserHostAddress + "', ";
            sql += " fl_update_time ='" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0') + DateTime.Now.Hour.ToString().PadLeft(2, '0') + DateTime.Now.Minute.ToString().PadLeft(2, '0') + DateTime.Now.Second.ToString().PadLeft(2, '0') + "' ";
            sql = sql + " where fl_citizen_id='" + changeDigit(txtCardID.Text.Trim().Replace(";", "").Replace("'", "").Replace("-", "").Replace(".", "").Replace(" ", "")) + "' ";
            sql = sql + " and fl_detailgroup_id ='" + Request.QueryString["id"].Trim().Replace(";", "").Replace("'", "") + "'; ";

            sql += "INSERT INTO tb_LOG(fl_id,fl_module,fl_action,fl_keyword,fl_datetime,fl_ip) VALUES(";
            sql += " '" + Session["uID"].ToString().Replace("'", "''") + "', ";
            sql += " 'MASS MEMBER', ";
            sql += " 'UPDATE', ";
            sql += " '" + changeDigit(txtCardID.Text.Trim().Replace(";", "").Replace("'", "").Replace("-", "").Replace(".", "").Replace(" ", "")) + "," + Request.QueryString["id"].Trim().Replace(";", "").Replace("'", "") + "', ";
            sql += " '" + DateTime.Now.Year + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0') + DateTime.Now.Hour.ToString().PadLeft(2, '0') + DateTime.Now.Minute.ToString().PadLeft(2, '0') + DateTime.Now.Second.ToString().PadLeft(2, '0') + "', ";
            sql += " '" + Request.UserHostAddress + "'); ";
        }
        #endregion

        command.CommandText = sql;
        command.ExecuteNonQuery();
        Conn.Close();

        lblResponse.Text = "�ѹ�֡�����������";
        lblResponse.ForeColor = System.Drawing.Color.Green;
        lblResponse.Visible = true;

        userDataSet();
    }

    protected void prepareText()
    {
        if (exclFile.HasFile && Path.GetExtension(exclFile.FileName) == ".xlsx")
        {
            using (var excel = new ExcelPackage(exclFile.PostedFile.InputStream))
            {
                //var tbl = new DataTable();
                var ws = excel.Workbook.Worksheets[0];
                //var hasHeader = false;  // adjust accordingly
                // add DataColumns to DataTable
                //foreach (var firstRowCell in ws.Cells[1, 1, 1, ws.Dimension.End.Column])
                 //   tbl.Columns.Add(hasHeader ? firstRowCell.Text
                 //       : String.Format("Column {0}", firstRowCell.Start.Column));

                // add DataRows to DataTable
              //  int startRow = hasHeader ? 2 : 1;
               int startRow=1;
               String text="";
               while (ws.Cells[startRow,2].Text !="")
                {
                   // var wsRow = ws.Cells[rowNum, 1, rowNum, ws.Dimension.End.Column];
                    if (text != "")
                    {
                        text += ",";
                    }
                   for(int j=1;j<11;j++){
                     text+=ws.Cells[startRow,2].Text+"|";
                   }
                }
               importText.Value = text;
             //   UploadStatusLabel.Text = msg;
            }
        }
        else
        {
          //  UploadStatusLabel.Text = "You did not specify a file to upload.";
        }
    }


    protected void btnImport_Click(object sender, EventArgs e)
    {

        string tmpImport = "";
        string errLine = "";
        int successLine = 0;
        int LineNo = 1;
        prepareText();
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
                if (colToken.Length == 25)
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
                    if (colToken[19].Trim().Replace(";", "").Replace("'", "") == "") colToken[19] = "1";
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
                    if (colToken[23].IndexOf("/") < 3) yyyymmdd = false;
                    if (colToken[23].IndexOf("-") < 3) yyyymmdd = false;
                    if (colToken[23].IndexOf(".") < 3) yyyymmdd = false;

                    colToken[23] = changeDigit(colToken[23].Trim().Replace(";", "").Replace("'", "").Replace("/", "").Replace("-", ""));
                    if (colToken[23].Length != 8)
                    {
                        colToken[23] = "";
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
                                yyyy = Convert.ToInt32(colToken[23].Substring(0, 4));
                                if (yyyy > 2500) yyyy -= 543;
                                mm = Convert.ToInt32(colToken[23].Substring(4, 2));
                                dd = Convert.ToInt32(colToken[23].Substring(6, 2));
                            }
                            else
                            {
                                yyyy = Convert.ToInt32(colToken[23].Substring(4, 4));
                                if (yyyy > 2500) yyyy -= 543;
                                mm = Convert.ToInt32(colToken[23].Substring(2, 2));
                                dd = Convert.ToInt32(colToken[23].Substring(0, 2));
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
                            colToken[23] = yyyy.ToString() + mm.ToString().PadLeft(2, '0') + dd.ToString().PadLeft(2, '0');
                        }
                        catch (Exception ee) { colToken[23] = ""; }
                    }

                    //Stop
                    yyyymmdd = true;
                    if (colToken[24].IndexOf("/") < 3) yyyymmdd = false;
                    if (colToken[24].IndexOf("-") < 3) yyyymmdd = false;
                    if (colToken[24].IndexOf(".") < 3) yyyymmdd = false;

                    colToken[24] = changeDigit(colToken[24].Trim().Replace(";", "").Replace("'", "").Replace("/", "").Replace("-", ""));
                    if (colToken[24].Length != 8)
                    {
                        colToken[24] = "";
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
                                yyyy = Convert.ToInt32(colToken[24].Substring(0, 4));
                                if (yyyy > 2500) yyyy -= 543;
                                mm = Convert.ToInt32(colToken[24].Substring(4, 2));
                                dd = Convert.ToInt32(colToken[24].Substring(6, 2));
                            }
                            else
                            {
                                yyyy = Convert.ToInt32(colToken[24].Substring(4, 4));
                                if (yyyy > 2500) yyyy -= 543;
                                mm = Convert.ToInt32(colToken[24].Substring(2, 2));
                                dd = Convert.ToInt32(colToken[24].Substring(0, 2));
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
                            colToken[24] = yyyy.ToString() + mm.ToString().PadLeft(2, '0') + dd.ToString().PadLeft(2, '0');
                        }
                        catch (Exception ee) { colToken[24] = ""; }
                    }

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
                        if (tmpImport.Length > 10) tmpImport = tmpImport.Substring(0, 10);
                        sql = sql + "'" + tmpImport + "', ";
                        tmpImport = colToken[5].Trim().Replace(";", "").Replace("'", "");
                        if (tmpImport.Length > 20) tmpImport = tmpImport.Substring(0, 20);
                        sql = sql + "'" + tmpImport + "', ";
                        tmpImport = colToken[6].Trim().Replace(";", "").Replace("'", "");
                        if (tmpImport.Length > 20) tmpImport = tmpImport.Substring(0, 20);
                        sql = sql + "'" + tmpImport + "', ";
                        tmpImport = colToken[7].Trim().Replace(";", "").Replace("'", "");
                        if (tmpImport.Length > 20) tmpImport = tmpImport.Substring(0, 20);
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
                        if (tmpImport.Length > 80) tmpImport = tmpImport.Substring(0, 80);
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
                        if (tmpImport.Length > 10) tmpImport = tmpImport.Substring(0, 10);
                        sql = sql + " fl_addrno='" + tmpImport + "',";
                        tmpImport = colToken[5].Trim().Replace(";", "").Replace("'", "");
                        if (tmpImport.Length > 20) tmpImport = tmpImport.Substring(0, 20);
                        sql = sql + " fl_home='" + tmpImport + "',";
                        tmpImport = colToken[6].Trim().Replace(";", "").Replace("'", "");
                        if (tmpImport.Length > 20) tmpImport = tmpImport.Substring(0, 20);
                        sql = sql + " fl_soy='" + tmpImport + "',";
                        tmpImport = colToken[7].Trim().Replace(";", "").Replace("'", "");
                        if (tmpImport.Length > 20) tmpImport = tmpImport.Substring(0, 20);
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
                        if (tmpImport.Length > 80) tmpImport = tmpImport.Substring(0, 80);
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

                    //Process membergroup
                    #region memberProc
                    procFlag = "UP";
                    if (colToken[0].Trim().Replace(";", "").Replace("'", "") == "")
                    {
                        procFlag = "NA";
                    }
                    else
                    {
                        sql2 = "SELECT distinct fl_citizen_id from tb_memberGroup ";
                        tmpImport = changeDigit(colToken[0].Trim().Replace(";", "").Replace("'", "").Replace("-", "").Replace(".", "").Replace(" ", ""));
                        if (tmpImport.Length > 20) tmpImport = tmpImport.Substring(0, 20);
                        sql2 += " where fl_citizen_id ='" + tmpImport + "' ";
                        sql2 += " and fl_detailgroup_id ='" + Request.QueryString["id"].Trim().Replace(";", "").Replace("'", "") + "'; ";
                        command.CommandText = sql2;
                        rs = command.ExecuteReader();
                        if (!rs.Read()) procFlag = "IN";
                        rs.Close();
                    }

                    if (procFlag == "IN")
                    {
                        sql = sql + "INSERT INTO tb_memberGroup (";
                        sql = sql + " fl_detailgroup_id,";
                        sql = sql + " fl_citizen_id,";
                        sql = sql + " fl_mem_id,";
                        sql = sql + " fl_pos,";
                        sql = sql + " fl_start,";
                        sql = sql + " fl_stop,";
                        sql = sql + " fl_create_by,";
                        sql = sql + " fl_create_ip,";
                        sql = sql + " fl_create_time,";
                        sql = sql + " fl_update_by,";
                        sql = sql + " fl_update_IP,";
                        sql = sql + " fl_update_time";

                        sql = sql + " ) values ( ";

                        sql = sql + "'" + Request.QueryString["id"].ToString() + "', ";

                        tmpImport = changeDigit(colToken[0].Trim().Replace(";", "").Replace("'", "").Replace("-", "").Replace(".", "").Replace(" ", ""));
                        if (tmpImport.Length > 20) tmpImport = tmpImport.Substring(0, 20);
                        sql = sql + "'" + tmpImport + "', ";
                        tmpImport = changeDigit(colToken[21].Trim().Replace(";", "").Replace("'", ""));
                        if (tmpImport.Length > 20) tmpImport = tmpImport.Substring(0, 20);
                        sql = sql + "'" + tmpImport + "',";

                        tmpImport = colToken[22].Trim().Replace(";", "").Replace("'", "");
			if(tmpImport=="��иҹ") tmpImport="P";
			if(tmpImport=="�ͧ��иҹ")tmpImport="V";
			if(tmpImport=="�Ţҹء��")tmpImport="Y";
			if(tmpImport=="��Ҫԡ")tmpImport="Z";
                        if (tmpImport.Length > 1) tmpImport = tmpImport.Substring(0, 1);
			if (tmpImport.Trim()=="") tmpImport="Z";
                        sql = sql + "'" + tmpImport + "',";
                        tmpImport = colToken[23].Trim().Replace(";", "").Replace("'", "");
                        if (tmpImport.Length > 20) tmpImport = tmpImport.Substring(0, 20);
                        sql = sql + "'" + tmpImport + "',";
                        tmpImport = colToken[24].Trim().Replace(";", "").Replace("'", "");
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
                        sql = sql + "UPDATE tb_membergroup SET ";
                        tmpImport = changeDigit(colToken[21].Trim().Replace(";", "").Replace("'", ""));
                        if (tmpImport.Length > 20) tmpImport = tmpImport.Substring(0, 20);
                        sql = sql + " fl_mem_id='" + tmpImport + "',";

                        tmpImport = colToken[22].Trim().Replace(";", "").Replace("'", "");
			if(tmpImport=="��иҹ") tmpImport="P";
			if(tmpImport=="�ͧ��иҹ")tmpImport="V";
			if(tmpImport=="�Ţҹء��")tmpImport="Y";
			if(tmpImport=="��Ҫԡ")tmpImport="Z";
                        if (tmpImport.Length > 1) tmpImport = tmpImport.Substring(0, 1);
			if (tmpImport.Trim()=="") tmpImport="Z";
                        sql = sql + " fl_pos='" + tmpImport + "',";
                        tmpImport = colToken[23].Trim().Replace(";", "").Replace("'", "");
                        if (tmpImport.Length > 20) tmpImport = tmpImport.Substring(0, 20);
                        sql = sql + " fl_start='" + tmpImport + "',";
                        tmpImport = colToken[24].Trim().Replace(";", "").Replace("'", "");
                        if (tmpImport.Length > 20) tmpImport = tmpImport.Substring(0, 20);
                        sql = sql + " fl_stop='" + tmpImport + "',";

                        sql += " fl_update_by ='" + Session["uID"].ToString().Trim().Replace("'", "''") + "', ";
                        sql += " fl_update_ip ='" + Request.UserHostAddress + "', ";
                        sql += " fl_update_time ='" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0') + DateTime.Now.Hour.ToString().PadLeft(2, '0') + DateTime.Now.Minute.ToString().PadLeft(2, '0') + DateTime.Now.Second.ToString().PadLeft(2, '0') + "' ";

                        tmpImport = changeDigit(colToken[0].Trim().Replace(";", "").Replace("'", "").Replace("-", "").Replace(".", "").Replace(" ", ""));
                        if (tmpImport.Length > 20) tmpImport = tmpImport.Substring(0, 20);
                        sql = sql + " where fl_citizen_id='" + tmpImport + "' ";
                        sql = sql + " and fl_detailgroup_id ='" + Request.QueryString["id"].Trim().Replace(";", "").Replace("'", "") + "'; ";
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
            sql += " 'MASS MEMBER', ";
            sql += " 'IMPORT', ";
            sql += " '', ";
            sql += " '" + DateTime.Now.Year + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0') + DateTime.Now.Hour.ToString().PadLeft(2, '0') + DateTime.Now.Minute.ToString().PadLeft(2, '0') + DateTime.Now.Second.ToString().PadLeft(2, '0') + "', ";
            sql += " '" + Request.UserHostAddress + "'); ";

            sql += "DELETE from tb_CITIZEN where fl_citizen_id=''; ";
            sql += "DELETE from tb_MEMBERGROUP where fl_detailgroup_id='' or fl_citizen_id = '' or fl_citizen_id not in (select distinct fl_citizen_id from tb_citizen); ";

            command.CommandText = sql;
            command.ExecuteNonQuery();
            Conn.Close();
        }

        lblResponse.Text = "����Ң���������� " + successLine.ToString() + " ��÷Ѵ";
        if(errLine.Length>0) lblResponse.Text += " ���ѭ��㹢����� Excel ��÷Ѵ��� " + errLine.Substring(0,errLine.Length-1);
        lblResponse.ForeColor = System.Drawing.Color.Green;
        lblResponse.Visible = true;

        userDataSet();
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        clearBox();
        userDataSet();
    }
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        bool errFound=false;
        if(txtCardID.Text=="")
        {
            lblResponse.Text="�ѹ�֡�����š�͹";
            lblResponse.ForeColor = System.Drawing.Color.Red;
            lblResponse.Visible =true;
            errFound = true;
        }
        if(imgFile.PostedFile.FileName=="")
        {
            lblResponse.Text = "���������Ҿ";
            lblResponse.ForeColor = System.Drawing.Color.Red;
            lblResponse.Visible = true;
            errFound = true;
        }
        if(!System.IO.Directory.Exists(Server.MapPath("CIMG")))
        {
            lblResponse.Text = "����վ�鹷��Ѵ�����";
            lblResponse.ForeColor = System.Drawing.Color.Red;
            lblResponse.Visible = true;
            errFound = true;
        }

        if(errFound)
        {
            boxSet(hidID.Value);
            setBar(Request.QueryString["id"].ToString());
            userDataSet();
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
        setBar(Request.QueryString["id"].ToString());

        userDataSet();
        lblResponse.Text = "�Ѵ���Ҿ�����";
        lblResponse.ForeColor = System.Drawing.Color.Green;

    }
    protected void pageID_SelectedIndexChanged(object sender, EventArgs e)
    {
        userDataSet(Convert.ToInt32(pageID.SelectedValue));
    }
    protected void txtCardID_TextChanged(object sender, EventArgs e)
    {
        txtCardID.Text = txtCardID.Text.Trim().Replace(";", "");
        boxSet(txtCardID.Text,1);
        userDataSet();
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
        string sql = "DELETE from tb_membergroup ";
        sql = sql + " where fl_citizen_id='" + txtCardID.Text.Trim().Replace(";", "").Replace("'", "") + "' ";
        sql = sql + " and fl_detailgroup_id ='" + Request.QueryString["id"].Trim().Replace(";", "").Replace("'", "") + "'; ";

        sql += "INSERT INTO tb_LOG(fl_id,fl_module,fl_action,fl_keyword,fl_datetime,fl_ip) VALUES(";
        sql += " '" + Session["uID"].ToString().Replace("'", "''") + "', ";
        sql += " 'MASS MEMBER', ";
        sql += " 'DELETE', ";
        sql += " '" + txtCardID.Text + "," + Request.QueryString["id"].Trim().Replace(";", "").Replace("'", "") + "', ";
        sql += " '" + DateTime.Now.Year + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0') + DateTime.Now.Hour.ToString().PadLeft(2, '0') + DateTime.Now.Minute.ToString().PadLeft(2, '0') + DateTime.Now.Second.ToString().PadLeft(2, '0') + "', ";
        sql += " '" + Request.UserHostAddress + "'); ";

        command.CommandText = sql;
        command.ExecuteNonQuery();
        Conn.Close();
    }
}