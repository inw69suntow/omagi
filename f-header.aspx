<%@ OutputCache Location="None" VaryByParam="none" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="f-header.aspx.cs" Inherits="_f_header" ViewStateEncryptionMode="Always"%>
<html>
<head>
<title>ระบบฐานข้อมูลมวลชน กองอำนวยการรักษาความมั่นคงภายในราชอาณาจักร</title>
<meta http-equiv="Content-Type" content="text/html; charset=windows-874">
<link rel="stylesheet" href="css.css" type="text/css">
</head>

<body leftmargin="0" text="#000000" topmargin="0" marginwidth="0" marginheight="0" bgcolor="#fec24a">
<table width="100%" border="0" cellspacing="0" cellpadding="0">
<tr>
	<td width="800px"><img src="photo/header.jpg" width="800px" height="90px"></td>
	<td>
		<table width="100%" border="0" cellspacing="0" cellpadding="0" align="right" valign="top" class="css">
			<tr><td height="20" align="center" class="css"><font color="white"><b><a href="user_manual.pdf" target="_blank"><font color="white">คู่มือ</font></a> | <a href="mailto:guntap@softnet.co.th" target="_blank"><font color="white">ช่วยเหลือ</font></a> | <a href="logoff.aspx" target="mainFrame"><font color="white">ออกจากระบบ</font></a></b></font></td></tr>
            <tr>
                <td height="20" align="center">
                    <form id="Form1" name="frm" runat="server">
                        <b><font color="#FFFFFF">ชื่อผู้ใช้งาน : </font></b><asp:Label ID="lblUname" Text="" runat="server" ForeColor="white"/><br />
                    </form>
                </td>
            </tr>
		</table>
	</td>
</tr>
</table>
</body>
</html>
