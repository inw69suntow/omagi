<%@ OutputCache Location="None" VaryByParam="none" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="login.aspx.cs" Inherits="_login" ViewStateEncryptionMode="Always"%>
<html>
<head>
<title>ระบบฐานข้อมูลมวลชน กองอำนวยการรักษาความมั่นคงภายในราชอาณาจักร</title>
<meta http-equiv="Content-Type" content="text/html; charset=windows-874" />
<link rel="stylesheet" href="css.css" type="text/css"/>
</head>

<body text="#000000" >
<form id="frm" runat="server">
<div align="center">
  <table width="100%" border="0" bgcolor="fec24a">
  <tr>
   <td width="80px">&nbsp;</td>
  <td>
  <table width="800" border="0" cellspacing="1" cellpadding="0" height="600" align="center" valign="middle">
    <tr>
      <td align="center" background="photo/logbox.jpg">
        <table width="55%" align="left" border="0" cellspacing="55" cellpadding="0">
          <tr> 
            <td align="left"> 
              <table width="100%" border="0" cellspacing="1" cellpadding="0">
                <tr > 
                  <td valign="top" align="left" style="height: 100px;width: 600px;">
                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                      <tr> 
                        <td width="1%" style="height: 20px">&nbsp;</td>
                        <td width="32%" align="right" style="height: 20px" class="css">ผู้ใช้งาน 
                          :</td>
                        <td width="2%" style="height: 20px">&nbsp;</td>
                        <td width="52%" style="height: 20px"> 
                          <asp:textbox name="uid" id="uid" size="20" runat="server" BorderStyle="Inset" ToolTip="รหัสผู้ใช้งาน"/>
                        </td>
                      </tr>    
                      <tr> 
                        <td height="20">&nbsp;</td>
                        <td height="20" align="right">รหัสผ่าน 
                          :</td>
                        <td height="20">&nbsp;</td>
                        <td height="20">
                          <asp:TextBox ID="pwd" runat="server" TextMode="Password"></asp:TextBox>
                          <asp:HiddenField ID="otp" runat="server" />
                        </td>
                      </tr> 
                      <tr> 
                        <td height="20">&nbsp;</td>
                        <td height="20" align="right">OTP 
                          :</td>
                        <td height="20">&nbsp;</td>
                        <td height="20">
                          <asp:Image BorderWidth="0" id="Img0" name="Img0" Height="20" Width="14" runat="server"/><asp:Image BorderWidth="0" id="Img1" name="Img1" Height="20" Width="14" runat="server" /><asp:Image BorderWidth="0" id="Img2" name="Img2" Height="20" Width="14" runat="server" /><asp:Image BorderWidth="0" id="Img3" name="Img3" Height="20" Width="14" runat="server"  /><asp:Image BorderWidth="0" id="Img4" name="Img4" Height="20" Width="14" runat="server" /><br>
                          <asp:textbox id="eotp" size="5" maxlength="5" runat="server" BorderStyle="Inset" ToolTip="ใส่หมายเลขตามที่ปรากฎ" ></asp:textbox><input type="button" class="css" value="ขอเลขใหม่" onclick="window.location.reload();" style="width: 70px">
                        </td>
                      </tr>
                      <tr>
                        <td height="20">&nbsp;</td>
                        <td height="20" colspan="3" align="center" valign="middle"><asp:Label name="lblResponse" id="lblResponse"  text="รหัสผู้ใช้งานหรือรหัสผ่านไม่ถูกต้อง" Visible="False" runat="server" Font-Bold="True" ForeColor="Red"/>
                        </td>
                      </tr>                             
                      <tr> 
                        <td height="20">&nbsp;</td>
                        <td height="20" colspan="3" align="center" valign="middle"><asp:Button type="button" class="css" name="btt1" id="btt1"  text="เข้าสู่ระบบ" runat="server" OnClick="btt1_Click" BorderStyle="Outset" Width="70px"></asp:button>
                            </td>
                      </tr>
                    </table>
                    </td>
                </tr>
              </table>
            </td>
          </tr>
        </table>
  </td>
  </tr>
  </table> 
</div>
</form>
</body>
</html>