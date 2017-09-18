using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.OleDb;

/// <summary>
/// Summary description for PageUtils
/// </summary>
public class PageUtils
{
    public static int  processPage(DropDownList pageID, Button btnPrev, Button btnNext, int rowCount, int curPage, OleDbDataReader rs)
	{
        String pageSizeConfig = ConfigurationManager.AppSettings["pageSize"].ToString();
        int pageSize = Convert.ToInt32(pageSizeConfig);
        if ((rowCount % pageSize) > 0)
        {
            rowCount = ((rowCount - (rowCount % pageSize)) / pageSize) + 1;
        }
        else
        {
            rowCount = rowCount / pageSize;
        }
        pageID.Items.Clear();
        for (int x = 1; x <= rowCount; x++)
        {
            pageID.Items.Add(new ListItem(x.ToString(), x.ToString()));
        }

        if (rowCount != 0)
        {
            if (curPage <= rowCount)
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

        if (rowCount == 0)
        {
            btnPrev.Visible = false;
            btnNext.Visible = false;
        }
        if (pageID.SelectedValue == "1")
            btnPrev.Visible = false;
        if (pageID.SelectedValue == rowCount.ToString())
        {
            btnNext.Visible = false;
        }


        int i = 0;
        while (i < (curPage - 1) * pageSize) {
            rs.Read(); i++; 
        }
        return pageSize;
	}
}