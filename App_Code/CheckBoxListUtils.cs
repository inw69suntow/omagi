using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for CheckBoxListUtils
/// </summary>
public class CheckBoxListUtils
{
    public static String getChkBox(String inex, String key)
    {
        String html = "<input type='checkbox' id='check_" + inex.ToString() + "'>";
        html += "<input type='hidden' id='hd_check_" + inex + "' value='" + key + "'>";
        return html;
    }
    public static String getEditButton(String index, String action)
    {
       // String html= "<input type='checkbox' id='check_" + i.ToString() + "' checked onClick=\"" + action + "\">";
        String html = "<input type='button' id='a_" + index + "' onClick=\"" + action + "\" value=\"แก้ไข\"></input> ";
        return html;
    }

    public static string getChkBoxCheck(String inex, string key)
    {
        String html = "<input type='checkbox' checked id='check_" + inex.ToString() + "'>";
        html += "<input type='hidden' id='hd_check_" + inex + "' value='" + key + "'>";
        return html;
    }


}