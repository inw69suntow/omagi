<%@ Page Language="C#" AutoEventWireup="true" CodeFile="menu.aspx.cs" Inherits="menu_menu" %>

 <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
       <script src='<%=Page.ResolveClientUrl("~/resources/js/common/jquery.js")%>' type="text/javascript"></script>
       <link rel="stylesheet" type="text/css" href="<%=Page.ResolveClientUrl("~/resources/js/common/jquery-ui.css")%>">
       <script src='<%=Page.ResolveClientUrl("~/resources/js/common/jquery-ui.js")%>' type="text/javascript"></script>
      


    <style>
        .ui-menu { width: 150px; }
    </style>

</head>
<body>

<ul id='menu' class='ui-menu'>
  <li class="ui-state-disabled"><div>Toys (n/a)</div></li>
  <li><div>Books</div></li>
  <li><div>Clothing</div></li>
  <li><div>Electronics</div>
    <ul>
      <li class="ui-state-disabled"><div>Home Entertainment</div></li>
      <li><div>Car Hifi</div></li>
      <li><div>Utilities</div></li>
    </ul>
  </li>
  <li><div>Movies</div></li>
  <li><div>Music</div>
    <ul>
      <li><div>Rock</div>
        <ul>
          <li><div>Alternative</div></li>
          <li><div>Classic</div></li>
        </ul>
      </li>
      <li><div>Jazz</div>
        <ul>
          <li><div>Freejazz</div></li>
          <li><div>Big Band</div></li>
          <li><div>Modern</div></li>
        </ul>
      </li>
      <li><div>Pop</div></li>
    </ul>
  </li>
  <li class="ui-state-disabled"><div>Specials (n/a)</div></li>
</ul>
   
     <script>
         $(function () {
             $("#menu").menu();
         });
  </script>
</body>
</html>
