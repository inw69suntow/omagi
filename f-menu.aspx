<%@ Page Language="C#" AutoEventWireup="true" CodeFile="f-menu.aspx.cs" Inherits="_f_menu"%>
<html>
<head runat="server">
<title>ระบบฐานข้อมูลมวลชน กองอำนวยการรักษาความมั่นคงภายในราชอาณาจักร</title>
<meta http-equiv="Content-Type" content="text/html; charset=windows-874">
<link rel="stylesheet" href="css.css" type="text/css">
</head>
<body text="#000000" leftmargin="0" topmargin="0" marginwidth="0" marginheight="0" class="fmenu">
<div align="left" valign="top">
<form name="frm" runat="server">
  <table width="250" border="0" cellspacing="1" cellpadding="0">
    <tr> 
      <td><a href="blank.aspx" target="mainFrame"><b>ระบบงานฐานข้อมูลมวลชน</b></a></td>
    </tr>
	    <tr class="off"> 
          <td height="19" align="left">&nbsp;<asp:TreeView ID="TreeView1" runat="server" Height="200px" Width="180px" Target="mainFrame" ExpandDepth="0" ImageSet="News" NodeIndent="10" OnSelectedNodeChanged="TreeView1_SelectedNodeChanged" CssClass="css" Font-Names="Microsoft Sans Serif,Vernada,Tahoma,Cordia" Font-Size="Medium">
              <ParentNodeStyle CssClass="css" Font-Bold="true" />
              <NodeStyle CssClass="css" Font-Names="Arial" Font-Size="10pt" ForeColor="Black" HorizontalPadding="5px" NodeSpacing="0px" VerticalPadding="0px" />
              <HoverNodeStyle Font-Underline="True" CssClass="css"/>
              <SelectedNodeStyle Font-Underline="True" HorizontalPadding="0px" VerticalPadding="0px" />
        </asp:TreeView>
        </td>
        </tr>
    </table>
</form>
</div>
</body>
</html>