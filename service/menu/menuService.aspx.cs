using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;

public partial class service_menu_menuService : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Dictionary<String, String> map = new Dictionary<String, String>();
            map.Add("id", "12343");
            map.Add("name", "yutthana");
            string json = JsonConvert.SerializeObject(map);
            Response.Clear();
            Response.Write(json);
        }
        catch (Exception ex)
        {
            Response.Clear();
            Response.Write("");
        }
    }
}