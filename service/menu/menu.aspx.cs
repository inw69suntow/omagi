using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;

public partial class menu_menu : System.Web.UI.Page
{
    ILog logger = LogManager.GetLogger("log");

    protected void Page_Load(object sender, EventArgs e)
    {
        logger.Debug("test log");
    }
}