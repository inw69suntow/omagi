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

public partial class _index: System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
                Response.Redirect("login.aspx");
                return;
    }
}