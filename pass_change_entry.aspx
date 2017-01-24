<%@ OutputCache Location="None" VaryByParam="none" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="pass_change_entry.aspx.cs" Inherits="_pass_change_entry" MasterPageFile="~/MasterPage.master" ViewStateEncryptionMode="Always"%>
<asp:Content ID="content" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<table width="750px" border="0" align="center" cellspacing="0" cellpadding="0" class="css">
  <tr>
    <td style="height:25px; width:750px;" class="header_1">เปลี่ยนรหัสผ่าน</td>
  </tr>
</table>

<table width="750px" align="center" valign="center" border="0" cellspacing="0" cellpadding="0" class="css">
  <tr><td height="10px"></td></tr>
  <tr>
    <td>
    <table width="750px" border="0" cellspacing="0" cellpadding="0" class="css">
        <tr>
            <td width="6px" style="height:23px;" background="photo/box_topleft.gif"></td>   
            <th style="height:20px; width:738px;"  background="photo/box_topbg.gif"><div align="left"><b><asp:Label ID="Label1" runat="server" visible="True" Width="750px"/></b></div></th>
            <td width="6px" style="height:23px;" background="photo/box_topright.gif"></td>   
        </tr>
    </table>
    </td>
  </tr>
  <tr>
    <td align="center" valign="top">
        <table width="750px" border="1" style="border:1px solid;" cellspacing="0" cellpadding="0" bgcolor="#ffffff">
        <tr>
            <td>
                <table cellspacing="0" cellpadding="0">            
                    <tr>
                        <td class="choice" style="width: 20%">&nbsp;รหัสผ่านเดิม :</td>
                        <td class="choice" style="width: 80%">&nbsp;<asp:TextBox ID="txtOld" CssClass="css" runat="server" TextMode="Password"></asp:TextBox>&nbsp;<font color="red">*</font>&nbsp;<asp:Label ID="lblResponse1" Text="" CssClass="css" runat="server" Width="50%"></asp:Label>
                        </td>
                   </tr>
                    <tr>
                        <td class="choice" style="width: 20%">&nbsp;รหัสผ่านใหม่ :</td>
                        <td class="choice" style="width: 80%">&nbsp;<asp:TextBox ID="txtPass1" CssClass="css" runat="server" TextMode="Password"></asp:TextBox>&nbsp;<font color="red">*</font>&nbsp;<asp:Label ID="lblResponse2" CssClass="css" Text="" runat="server" Width="50%"></asp:Label>
                        </td>
                   </tr>
                    <tr>
                        <td class="choice" style="width: 20%">&nbsp;พิมพ์รหัสผ่านซ้ำ :</td>
                        <td class="choice" style="width: 80%">&nbsp;<asp:TextBox ID="txtPass2" CssClass="css" runat="server" TextMode="Password"></asp:TextBox>&nbsp;<font color="red">*</font>&nbsp;<asp:Label ID="lblResponse3" Text="" CssClass="css" runat="server" Width="50%"></asp:Label>
                        </td>
                    </tr>
		            <tr> 
                      <td class="choice" align="right" style="height: 17px" colspan="2"><asp:Label ID="lblResponse" CssClass="css" Text="" runat="server" Width="80%"></asp:Label><div align="right"><asp:Button ID="btnUpdate" class="css" runat="server" Text="บันทึก" OnClick="btnUpdate_Click"  Width="70px"/><asp:Button ID="brnClear" runat="server" Text="ล้างค่า" CssClass="css" 
    Width="70px" onclick="brnClear_Click"/>&nbsp;</div></td>
                    </tr>
                  </table>
                </td>
            </tr>
        </table>
    </td>
  </tr>
  <tr><td height="20px"></td></tr>
</table>
</asp:Content>