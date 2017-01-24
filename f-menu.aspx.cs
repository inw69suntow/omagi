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

public partial class _f_menu : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Response.AddHeader("X-UA-Compatible", "IE=9");
            if (!Page.IsPostBack)
            {
                string sql = "";
                OleDbConnection Conn = new OleDbConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString);
                OleDbCommand command = new OleDbCommand();
                Conn.Open();
                command.Connection = Conn;
                OleDbDataReader rs;

                OleDbConnection Conn2 = new OleDbConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString);
                OleDbCommand command2 = new OleDbCommand();
                Conn2.Open();
                command2.Connection = Conn2;
                //OleDbDataReader rs2;

                foreach (SiteMapNode node in SiteMap.RootNode.ChildNodes)
                {
                    bool addFlag = true;
                    #region level1
                    if ((node.Title == "บริหารระบบ") && (Session["uGroup"].ToString() != "A")) addFlag = false;
                    if (addFlag)
                    {
                        TreeNode itemX = new TreeNode();
                        itemX.Text = node.Title;
                        itemX.NavigateUrl = node.Url;
                        itemX.ToolTip = node.Description;
                        TreeView1.Nodes.Add(itemX);
                        if (node.HasChildNodes)
                        {
                            foreach (SiteMapNode nodeX in node.ChildNodes)
                            {
                                if ((nodeX.Title == "บันทึกข้อมูลมวลชน") && ((Session["uGroup"].ToString() == "C") || (Session["uGroup"].ToString() == "M"))) addFlag = false;
                                if ((nodeX.Title == "บันทึกข้อมูลการฝึกอบรม") && ((Session["uGroup"].ToString() == "C") || (Session["uGroup"].ToString() == "M"))) addFlag = false;
                                if ((nodeX.Title == "ปูมการใช้ระบบงาน") && (Session["uGroup"].ToString() != "A")) addFlag = false;
                                if ((nodeX.Title == "บันทึกข้อมูลหลักสูตรการฝึกอบรม") && (Session["uGroup"].ToString() != "A")) addFlag = false;
                                if ((nodeX.Title == "ปลดล๊อคผู้ใช้งาน") && (Session["uGroup"].ToString() != "A")) addFlag = false;
                                if (addFlag)
                                {
                                    TreeNode item = new TreeNode();
                                    item.Text = nodeX.Title;
                                    item.NavigateUrl = nodeX.Url;
                                    item.ToolTip = nodeX.Description;
                                    itemX.ChildNodes.Add(item);

                                    #region level2
                                    if (nodeX.HasChildNodes)
                                    {
                                        foreach (SiteMapNode node2 in nodeX.ChildNodes)
                                        {
                                            TreeNode item2 = new TreeNode();
                                            item2.Text = node2.Title;
                                            item2.NavigateUrl = node2.Url;
                                            item2.ToolTip = node2.Description;
                                            item.ChildNodes.Add(item2);

                                            #region summaryReport
                                            if (node2.Title == "รายงานสรุปยอดมวลชน")
                                            {
                                                TreeNode item3 = new TreeNode();
                                                item3.Text = "สรุปภาพรวม";
                                                item3.NavigateUrl = "summary_REP.aspx?t=0";

                                                item2.ChildNodes.Add(item3);

                                                item3 = new TreeNode();
                                                item3.Text = "รายจังหวัดทั้งหมด";
                                                item3.NavigateUrl = "summary_REP.aspx?t=1";

                                                item2.ChildNodes.Add(item3);

                                                sql = "SELECT distinct fl_dept_id,fl_dept_name from tb_dept where fl_dept_status='1' ";
                                                if (Session["uGroup"].ToString() == "U")
                                                {
                                                    if (Session["uDept"].ToString().Substring(0) != "0") sql = sql + " and fl_dept_id like '" + Session["uDept"].ToString().Replace("00", "") + "%' ";
                                                }
                                                sql += " and fl_dept_id like '%00' ";
                                                sql += " order by fl_dept_id ";

                                                command.CommandText = sql;
                                                rs = command.ExecuteReader();
                                                while (rs.Read())
                                                {
                                                    item3 = new TreeNode();
                                                    item3.Text = rs.GetString(1);
                                                    item3.NavigateUrl = "summary_REP.aspx?t=1&id=" + rs.GetString(0);

                                                    item2.ChildNodes.Add(item3);
                                                }
                                                rs.Close();
                                            }
                                            #endregion

                                            #region detailReport
                                            if (node2.Title == "รายงานรายละเอียดข้อมูลมวลชน")
                                            {
                                                TreeNode item3 = new TreeNode();
                                                item3.Text = "จำแนกจังหวัด";

                                                item2.ChildNodes.Add(item3);

                                                TreeNode item4 = new TreeNode();
                                                item4.Text = "ทุกกลุ่มมวลชน";
                                                item4.NavigateUrl = "detail_REP.aspx";

                                                item3.ChildNodes.Add(item4);

                                                TreeNode item5;
                                                #region type1
                                                item4 = new TreeNode();
                                                item4.Text = "มวลชนกอ.รมน.";
                                                item4.NavigateUrl = "detail_REP.aspx?t=1";

                                                item3.ChildNodes.Add(item4);

                                                sql = "SELECT distinct fl_group_id,fl_group_name";
                                                sql += " from tb_maingroup where fl_group_status='1' ";
                                                sql += " and fl_group_type='1' ";
                                                sql += " order by fl_group_id ";

                                                command.CommandText = sql;
                                                rs = command.ExecuteReader();
                                                while (rs.Read())
                                                {
                                                    item5 = new TreeNode();
                                                    item5.Text = rs.GetString(1);
                                                    item5.NavigateUrl = "detail_REP.aspx?t=1&id=" + rs.GetString(0);

                                                    item4.ChildNodes.Add(item5);
                                                }
                                                rs.Close();
                                                #endregion

                                                #region type2
                                                item4 = new TreeNode();
                                                item4.Text = "มวลชนภาครัฐ";
                                                item4.NavigateUrl = "detail_REP.aspx?t=2";

                                                item3.ChildNodes.Add(item4);
                                                #endregion

                                                #region type3
                                                item4 = new TreeNode();
                                                item4.Text = "มวลชนภาคประชาชน";
                                                item4.NavigateUrl = "detail_REP.aspx?t=3";

                                                item3.ChildNodes.Add(item4);
                                                #endregion

                                                item3 = new TreeNode();
                                                item3.Text = "จำแนกกลุ่ม";

                                                item2.ChildNodes.Add(item3);

                                                item4 = new TreeNode();
                                                item4.Text = "ทุกจังหวัด";
                                                item4.NavigateUrl = "detail_REP2.aspx";

                                                item3.ChildNodes.Add(item4);

                                                sql = "SELECT distinct fl_dept_id,fl_dept_name from tb_dept where fl_dept_status='1' ";
                                                if (Session["uGroup"].ToString() == "U")
                                                {
                                                    if (Session["uDept"].ToString().Substring(0) != "0") sql = sql + " and fl_dept_id like '" + Session["uDept"].ToString().Replace("00", "") + "%' ";
                                                }
                                                sql += " and fl_dept_id like '%00' ";
                                                sql += " order by fl_dept_id ";

                                                command.CommandText = sql;
                                                rs = command.ExecuteReader();
                                                while (rs.Read())
                                                {
                                                    item4 = new TreeNode();
                                                    item4.Text = rs.GetString(1);
                                                    item4.NavigateUrl = "detail_REP2.aspx?id=" + rs.GetString(0);

                                                    item3.ChildNodes.Add(item4);
                                                }
                                                rs.Close();
                                            }
                                            #endregion
                                        }
                                    }
                                    #endregion
                                }
                            }
                        }
                    }                
                    #endregion
                }
                Conn.Close();
                Conn2.Close();

                if (Session["alert"].ToString() != "") Response.Write("<script language=\"JavaScript\">alert('" + Session["alert"].ToString() + "');</script>");
                Session["alert"] = "";
            }
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message + "<BR>" + ex.StackTrace);
        }

    }

    protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
    {
        try
        {
            TreeView1.SelectedNode.Expand();
        }
        catch (Exception ex) { }
    }
}