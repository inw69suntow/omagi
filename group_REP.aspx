<%@ Page Language="C#" AutoEventWireup="true" CodeFile="group_REP.aspx.cs" MasterPageFile="~/MasterPage.master" Inherits="_group_REP" ViewStateEncryptionMode="Always"%>
<asp:Content ID="content" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<table  id="bar" runat="server"  visible="false" width="1000px" align="center" valign="center" border="0" cellspacing="0" cellpadding="0" class="css">
  <tr>
    <td>
    <table width="1000px" border="0" cellspacing="0" cellpadding="0" class="css">
      <tr>
        <td width="6px" style="height:23px;" background="photo/box_topleft.gif"></td>   
        <th style="height:23px; width:988px;" background="photo/box_topbg.gif"><div align="left">รายนามกลุ่มมวลชนและสมาชิก</div></th>
        <td width="6px" style="height:23px;" background="photo/box_topright.gif"></td>   
      </tr>
      <asp:HiddenField ID="userDataSQL" Value="" runat="server" />
      <asp:HiddenField ID="maxi" Value="" runat="server" />
    </table>
    </td>
  </tr>
  <tr>
    <td align="center" valign="top"> 
    <table id="tab1" border="1" cellspacing="0" cellpadding="0" class="css" style="width: 1000px; border-right: #f6a836; border-top: #f6a836; border-left: #f6a836; border-bottom: #f6a836;" runat="server" rules="all" ></table>
    </td>
  </tr>
  <tr><td height="20px"></td></tr>
</table>
</asp:Content>