<%@ Page Language="C#" AutoEventWireup="true" CodeFile="testpopup.aspx.cs" Inherits="service_mass_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
        <meta name="viewport" content="width=device-width, initial-scale=1"/>
       <script src='<%=Page.ResolveClientUrl("~/resources/js/common/jquery.js")%>' type="text/javascript"></script>
       <link rel="stylesheet" type="text/css" href="<%=Page.ResolveClientUrl("~/resources/js/common/jquery-ui.css")%>"/>
       <script src='<%=Page.ResolveClientUrl("~/resources/js/common/jquery-ui.js")%>' type="text/javascript"></script>
       <script src='<%=Page.ResolveClientUrl("~/resources/js/common/utils.js")%>' type="text/javascript"></script>
        <link rel="stylesheet" type="text/css" href="<%=Page.ResolveClientUrl("~/resources/megamenu-js-master/css/style.css")%>"/>
        <link rel="stylesheet" type="text/css" href="<%=Page.ResolveClientUrl("~/resources/megamenu-js-master/css/ionicons.min.css")%>"/>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    </div>
    </form>
    <p>
        <input id="Button1" type="button" value="button" onclick="openPopup();return false;" /></p>
        <script type="text/javascript">
                function openPopup() {
                    utils.dialog({
                        title: 'Save success!',
                        text: 'Do you want to return to the screen of my list ?',
                        onOk: function () {
                            
                        }
                    });
                }
        </script>
</body>
</html>
