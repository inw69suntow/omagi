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
        base.OnInit(e);
    }

   
  

    protected void clearBox()
    {
        cmbMassGroup.Items.Clear();
        hidID.Value = "";
        //cmbProvince.SelectedValue = "";
        //cmbDistrict.Items.Clear();
        //cmbDistrict.Items.Add(new ListItem("ไม่กำหนด", ""));
        //cmbTambon.Items.Clear();
        //cmbTambon.Items.Add(new ListItem("ไม่กำหนด", ""));
        //cmbMassGroup.SelectedValue = "";
        txtMassName.Text = "";
        txtReportAmount.Text = "";
        //cmbDept.SelectedValue="";
        txtGoogle.Text="";
        lblCreate.Text = "";
        lblUpdate.Text = "";

    }


    protected void Page_Load(object sender, EventArgs e)
    {
        
    }




 

    protected void cmbProvince_SelectedIndexChanged(object sender, EventArgs e)
    {
        
    }
    protected void cmbDistrict_SelectedIndexChanged(object sender, EventArgs e)
    {
        
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        
    }
    protected void btnMember_Click(object sender, EventArgs e)
    {
       
    }
    protected void btnImport_Click(object sender, EventArgs e)
    {
       
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
    }
    protected void pageID_SelectedIndexChanged(object sender, EventArgs e)
    {
        

    }
    protected void btnPrev_Click(object sender, EventArgs e)
    {
        
    }
    protected void btnNext_Click(object sender, EventArgs e)
    {
       
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