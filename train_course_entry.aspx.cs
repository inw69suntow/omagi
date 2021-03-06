﻿using System;
using System.Collections;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using omagi_model;
using System.Collections.Generic;

public partial class _train_course_entry : System.Web.UI.Page
{
    override protected void OnInit(EventArgs e)
    {
        btnCreate.Attributes.Add("onclick",GetPostBackEventReference(btnCreate));
        base.OnInit(e);
    }

    protected void btnCreate_Click(object sender, EventArgs e)
    {
        if (Session["uID"] == null)
        {
            Session["uID"] = "";
            Session["uName"] = "";
            Session.Clear();
            Response.Write("<script language='javascript'>parent.document.location='web_login.aspx'</script>");
            return;
        }

        if (newVal.Value == "")
        {
            Load_Table(null);
            return;
        }

        try
        {
            string strConnString = ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString;
            OleDbConnection con = new OleDbConnection(strConnString);
            OleDbCommand objCmd = new OleDbCommand();
            string newValStr = newVal.Value.ToString().Trim();
            newVal.Value = "";
            string[] recstrSet = newValStr.Split(',');
            string sql = "DELETE from tb_train_course;";

            foreach (string recStr in recstrSet)
            {
                if (recStr.Trim() != "")
                {

                    String[] column = recStr.Split('|');
                    if (column[0] != "")
                    {
                        String budget = column[2].Trim().Replace("'", "''").Replace(";", "");
                        if (budget == "")
                        {
                            budget="0";
                        }
                        sql += "INSERT INTO tb_train_Course ( ";
                        sql += " fl_course,fl_province,fl_budget,fl_objective ";
                        sql += " ) VALUES ( ";
                        sql += " '" + column[0].Trim().Replace("'", "''").Replace(";", "") + "'";
                        sql += " ,'" + column[1].Trim().Replace("'", "''").Replace(";", "") + "'";
                        sql += " ," + budget;
                        sql += " ,'" + column[3].Trim().Replace("'", "''").Replace(";", "") + "'";
                        sql += "); ";
                    }
                }
            }

            sql += "INSERT INTO tb_LOG(fl_id,fl_module,fl_action,fl_keyword,fl_datetime,fl_ip) VALUES(";
            sql += " '" + Session["uID"].ToString().Replace("'", "''") + "', ";
            sql += " 'TRAIN COURSE', ";
            sql += " 'UPDATE', ";
            sql += " '', ";
            sql += " '" + DateTime.Now.Year + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0') + DateTime.Now.Hour.ToString().PadLeft(2, '0') + DateTime.Now.Minute.ToString().PadLeft(2, '0') + DateTime.Now.Second.ToString().PadLeft(2, '0') + "', ";
            sql += " '" + Request.UserHostAddress + "'); ";

            con.Open();
            objCmd.Connection = con;
            objCmd.CommandText = sql;
            objCmd.ExecuteNonQuery();
            con.Close();
            Load_Table(null);
        }
        catch (Exception ex) { }
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
            Load_Table(null); 
        }
       
    }




    private void Load_Table(String course)
    {
        dtGrid.Rows.Clear();
        dtGrid.Border = 1;
        dtGrid.CellPadding = 0;
        dtGrid.CellSpacing = 0;
        dtGrid.Rows.Add(new HtmlTableRow());
        dtGrid.Rows[0].Attributes.Add("class", "head");
        dtGrid.Rows[0].Cells.Add(new HtmlTableCell());
        dtGrid.Rows[0].Cells[0].InnerHtml = "&nbsp;";
        dtGrid.Rows[0].Cells[0].Width = "5%";


        dtGrid.Rows[0].Cells.Add(new HtmlTableCell());
        dtGrid.Rows[0].Cells[1].Width = "20%";
        dtGrid.Rows[0].Cells[1].InnerHtml = genSortColumn("ชื่อหลักสูตร");


        dtGrid.Rows[0].Cells.Add(new HtmlTableCell());
        dtGrid.Rows[0].Cells[2].Width = "15%";
        dtGrid.Rows[0].Cells[2].InnerHtml = genSortColumn("จังหวัด");

        dtGrid.Rows[0].Cells.Add(new HtmlTableCell());
        dtGrid.Rows[0].Cells[3].Width = "20%";
        dtGrid.Rows[0].Cells[3].InnerHtml = genSortColumn("งบประมาณ");

        dtGrid.Rows[0].Cells.Add(new HtmlTableCell());
        dtGrid.Rows[0].Cells[4].Width = "30%";
        dtGrid.Rows[0].Cells[4].InnerHtml = genSortColumn("วัตถุประสงค์");
        try
        {
            string strConnString = ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString;
            OleDbConnection con = new OleDbConnection(strConnString);
            con.Open();
            string sql = "SELECT * from tb_train_Course ";
            if (course != null && !"".Equals(course))
            {
                sql += " where fl_course like '%"+course.Replace("'","")+"%'";
               // sql += " where fl_course LIKE '%@course%' ";
            }
            else if ("ชื่อหลักสูตร".Equals(hdSortName.Value))
            {
                sql += " order by fl_course ASC";
            }
            else if ("จังหวัด".Equals(hdSortName.Value))
            {
                sql += " order by fl_province ASC";
            }
            else if ("งบประมาณ".Equals(hdSortName.Value))
            {
                sql += " order by fl_budget ASC ";
            }
            else if ("วัตถุประสงค์".Equals(hdSortName.Value))
            {
                sql += " order by fl_objective ASC ";
            }
            else
            {
                sql += " order by fl_course ASC ";
            }
            OleDbDataReader dtReader;
            OleDbCommand objCmd = new OleDbCommand();
            objCmd.CommandText = sql;
            objCmd.Connection = con;
  
            dtReader = objCmd.ExecuteReader();

            int count = 1;
            while (dtReader.Read())
            {
                if (!dtReader.IsDBNull(0))
                {

                    dtGrid.Rows.Add(new HtmlTableRow());
                    dtGrid.Rows[count].Attributes.Add("class", "off");
                    dtGrid.Rows[count].Cells.Add(new HtmlTableCell());
                    dtGrid.Rows[count].Cells[0].ColSpan = 1;
                    dtGrid.Rows[count].Cells[0].Align = "RIGHT";
                    dtGrid.Rows[count].Cells[0].InnerHtml = count.ToString();

                    dtGrid.Rows[count].Cells.Add(new HtmlTableCell());
                    dtGrid.Rows[count].Cells[1].ColSpan = 1;
                    dtGrid.Rows[count].Cells[1].Align = "Left";
                    dtGrid.Rows[count].Cells[1].InnerHtml = "<input type='text' id='id_" + count + "' value='" + dtReader["fl_course"] + "' style='width:200px' maxlength='255'>";

                    dtGrid.Rows[count].Cells.Add(new HtmlTableCell());
                    dtGrid.Rows[count].Cells[2].ColSpan = 1;
                    dtGrid.Rows[count].Cells[2].Align = "Left";
                    dtGrid.Rows[count].Cells[2].InnerHtml = "<input type='text' id='id_province_" + count + "' value='" + dtReader["fl_province"] + "' style='width:200px' maxlength='255'>";

                    dtGrid.Rows[count].Cells.Add(new HtmlTableCell());
                    dtGrid.Rows[count].Cells[3].ColSpan = 1;
                    dtGrid.Rows[count].Cells[3].Align = "Left";
                    dtGrid.Rows[count].Cells[3].InnerHtml = "<input type='text' id='id_budget_" + count + "' value='" + dtReader["fl_budget"] + "' style='width:200px' maxlength='15'>";

                    dtGrid.Rows[count].Cells.Add(new HtmlTableCell());
                    dtGrid.Rows[count].Cells[4].ColSpan = 1;
                    dtGrid.Rows[count].Cells[4].Align = "Left";
                    dtGrid.Rows[count].Cells[4].InnerHtml = "<input type='text' id='id_objective_" + count + "' value='" + dtReader["fl_objective"] + "' style='width:200px' maxlength='255'>";
                    count = count + 1;
                }
            }
            dtReader.Close();
            con.Close();

            dtGrid.Rows.Add(new HtmlTableRow());
            dtGrid.Rows[count].Attributes.Add("class", "off");
            dtGrid.Rows[count].Attributes.Add("onmouseover", "this.className='on'");
            dtGrid.Rows[count].Attributes.Add("onmouseout", "this.className='off'");
            dtGrid.Rows[count].Cells.Add(new HtmlTableCell());
            dtGrid.Rows[count].Cells[0].ColSpan = 1;
            dtGrid.Rows[count].Cells[0].Align = "Center";
            dtGrid.Rows[count].Cells[0].InnerHtml = "*";


            dtGrid.Rows[count].Cells.Add(new HtmlTableCell());
            dtGrid.Rows[count].Cells[1].ColSpan = 1;
            dtGrid.Rows[count].Cells[1].Align = "left";
            dtGrid.Rows[count].Cells[1].InnerHtml = "<input type='text' id='id_" + count + "' value='' style='width:200px' maxlength='255'>";

            dtGrid.Rows[count].Cells.Add(new HtmlTableCell());
            dtGrid.Rows[count].Cells[2].ColSpan = 1;
            dtGrid.Rows[count].Cells[2].Align = "Left";
            dtGrid.Rows[count].Cells[2].InnerHtml = "<input type='text' id='id_province_" + count + "' value='' style='width:200px' maxlength='255'>";

            dtGrid.Rows[count].Cells.Add(new HtmlTableCell());
            dtGrid.Rows[count].Cells[3].ColSpan = 1;
            dtGrid.Rows[count].Cells[3].Align = "Left";
            dtGrid.Rows[count].Cells[3].InnerHtml = "<input type='text' id='id_budget_" + count + "' value='' style='width:200px' maxlength='15'>";

            dtGrid.Rows[count].Cells.Add(new HtmlTableCell());
            dtGrid.Rows[count].Cells[4].ColSpan = 1;
            dtGrid.Rows[count].Cells[4].Align = "Left";
            dtGrid.Rows[count].Cells[4].InnerHtml = "<input type='text' id='id_objective_" + count + "' value='' style='width:200px' maxlength='255'>";
            count = count + 1;

            maxJ.Value = count.ToString();
        }
        catch (Exception ex) { }
    }



    //private void Load_GvCourse()
    //{
    //    IList<Course> list = new List<Course>();
    //    OleDbConnection con = null;
    //    OleDbDataReader dtReader=null;
    //    try
    //    {
    //        string strConnString = ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString;
    //        con = new OleDbConnection(strConnString);
    //        con.Open();
    //        string sql = "SELECT fl_course from tb_train_Course order by fl_course ";

    //        OleDbCommand objCmd = new OleDbCommand();
    //        objCmd.CommandText = sql;
    //        objCmd.Connection = con;
    //        dtReader = objCmd.ExecuteReader();
    //        while (dtReader.Read())
    //        {
    //            if (!dtReader.IsDBNull(0))
    //            {
    //                Course course = new Course();
    //                course.Topic = dtReader.GetString(0);
    //                list.Add(course);
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //    finally
    //    {
    //        if (dtReader != null)
    //        {
    //            dtReader.Close();
    //        }
    //        if (con != null)
    //        {
                
    //            con.Close();
    //        }
    //    }
    //    gvTrainCourse.DataSource = list;
    //    gvTrainCourse.DataBind();
    //}


    //protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        TextBox txtTopic = new TextBox();
    //        txtTopic.Width = 600;
    //        txtTopic.MaxLength = 255;
    //        txtTopic.ID = "txtTopic";
    //        // txtTopic.Text = (e.Row.DataItem as DataRowView).Row["Topic"].ToString();
    //         txtTopic.Text=(e.Row.DataItem as omagi_model.Course).Topic;
    //        e.Row.Cells[1].Controls.Add(txtTopic);

    
    //    }
    //}
    protected void btnSearch_Click(object sender, ImageClickEventArgs e)
    {
        Load_Table(txtKeyword.Text.Trim());
    }
    private String genSortColumn(String solumnName)
    {
        String htmlSort = "<center><a style=\"text-decoration: none;\" href=\"#\" onclick=\"sortColumn('" + solumnName + "')\">" + solumnName + "</a></center>";
        return htmlSort;
    }

}
