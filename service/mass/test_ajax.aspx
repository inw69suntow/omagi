<%@ Page Language="C#" AutoEventWireup="true" CodeFile="test_ajax.aspx.cs" Inherits="service_mass_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
        <meta name="viewport" content="width=device-width, initial-scale=1"/>
       <script src='<%=Page.ResolveClientUrl("~/resources/js/common/jquery.js")%>' type="text/javascript"></script>
       <link rel="stylesheet" type="text/css" href="<%=Page.ResolveClientUrl("~/resources/js/common/jquery-ui.css")%>"/>
       <script src='<%=Page.ResolveClientUrl("~/resources/js/common/jquery-ui.js")%>' type="text/javascript"></script>
        <script src='<%=Page.ResolveClientUrl("~/resources/datatables/js/jquery.dataTables.js")%>' type="text/javascript"></script>
        <script src='<%=Page.ResolveClientUrl("~/resources/datatables/js/dataTables.bootstrap.js")%>' type="text/javascript"></script>
       <script src='<%=Page.ResolveClientUrl("~/resources/js/common/utils.js")%>' type="text/javascript"></script>
       <script src='<%=Page.ResolveClientUrl("~/resources/js/common/utilsDataTables.js")%>' type="text/javascript"></script>


        <link rel="stylesheet" type="text/css" href="<%=Page.ResolveClientUrl("~/resources/megamenu-js-master/css/style.css")%>"/>
        <link rel="stylesheet" type="text/css" href="<%=Page.ResolveClientUrl("~/resources/megamenu-js-master/css/ionicons.min.css")%>"/>
       
        <link rel="stylesheet" type="text/css" href="<%=Page.ResolveClientUrl("~/resources/js/common/jquery-ui.theme.css")%>"/>
        <link rel="stylesheet" type="text/css" href="<%=Page.ResolveClientUrl("~/resources/datatables/css/jquery.dataTables_themeroller.css")%>"/>
        <link rel="stylesheet" type="text/css" href="<%=Page.ResolveClientUrl("~/resources/datatables/css/jquery.dataTables.css")%>"/>
        <link rel="stylesheet" type="text/css" href="<%=Page.ResolveClientUrl("~/resources/css/global_web.css")%>"/>
</head>
<body>
    <form id="form1" runat="server">
    <div>
      
         <input id="Button2" type="button" value="button" onclick="testAjax()" />
         <div id="result"></div>
    </div>
    </form>
   

      

        <script type="text/javascript">

            function testAjax() {
                utils.rp('#result',{
                    url: '<%=Page.ResolveUrl("~/service/mass/mass_entry_detail.aspx")%>',
                    dataType:'html'
                });
            }

   
        </script>
</body>
</html>
