﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="testdb.aspx.cs" Inherits="testdb" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    </div>
    <asp:Button ID="Button1" runat="server" onclick="Button1_Click" Text="Button" />
    <asp:Label ID="lblResponse" runat="server" Text=""></asp:Label><br />

    <asp:TextBox ID="txtPass" runat="server"></asp:TextBox>
    <asp:Label ID="lbPassword" runat="server" Text="Label"></asp:Label>
    </form>
</body>
</html>