<%@ OutputCache Location="None" VaryByParam="none" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="unlock_entry.aspx.cs" Inherits="_unlock_entry" MasterPageFile="~/MasterPage.master" ViewStateEncryptionMode="Always"%>
<asp:Content ID="content" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<table width="750px" border="0" align="center" cellspacing="0" cellpadding="0" class="css">
  <tr>
    <td style="height:25px; width: 750px;" class="header_1">ปลดล๊อคผู้ใช้งาน</td>
  </tr>
</table>

<table width="750px" align="center" valign="center" border="0" cellspacing="0" cellpadding="0" class="css">
  <tr><td height="10px"></td></tr>
  <tr>
    <td>
    <table width="750px" border="0" cellspacing="0" cellpadding="0" class="css">
        <tr>
            <td width="6px" style="height:23px;" background="photo/box_topleft.gif"></td>   
            <th style="height:20px; width:738px;"  background="photo/box_topbg.gif"><div align="left"><b>ปลดล๊อคผู้ใช้งาน</b></div></th>
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
                <table cellspacing="0" cellpadding="0" width="50%">            
                    <tr>
                        <td class="choice" style="width: 35%">&nbsp;ปลดล๊อคผู้ใช้งาน : </td>
                        <td class="choice" style="width: 65%">&nbsp;<asp:TextBox ID="txtOld" runat="server" 
                                MaxLength="250" Width="100px" ontextchanged="txtOld_TextChanged"></asp:TextBox>&nbsp;<font color="red">*</font>&nbsp;<asp:Label ID="lblResponse1" Text="" runat="server"></asp:Label>
                        </td>
                   </tr>
		            <tr> 
                      <td class="choice" align="right" style="height: 17px" colspan="2"><asp:Label ID="lblResponse" Text="" runat="server" Width="80%"></asp:Label><div align="right"><asp:Button ID="btnUpdate" runat="server" class="css" Text="UNLOCK" OnClick="btnUpdate_Click" Width="70px"/><asp:Button ID="brnClear" runat="server" Text="CLEAR" CssClass="css" 
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