<%@ Page Language="C#" AutoEventWireup="true" CodeFile="summary_REP.aspx.cs" Inherits="_summary_REP" MasterPageFile="~/MasterPage.master" ViewStateEncryptionMode="Always"%>
<asp:Content ID="content" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<table width="750px" border="0" align="center" cellspacing="0" cellpadding="0" class="css">
  <tr>
    <td style="height:25px;width:750px;" class="header_1">รายงานสรุปยอดมวลชน <asp:Label ID="qParam" value="" runat="server" /></td>
  </tr>
</table>
<table  id="bar" runat="server"  visible="false" width="750px" align="center" valign="center" border="0" cellspacing="0" cellpadding="0" class="css">
  <tr>
    <td>
    <table width="750px" border="0" cellspacing="0" cellpadding="0" class="css">
      <tr>
        <td width="6px" style="height:23px;" background="photo/box_topleft.gif"></td>   
        <th style="height:23px; width:638px;" background="photo/box_topbg.gif"><div align="left">รายงานรายละเอียด</div></th>
        <th style="height:23px; width:100px;" background="photo/box_topbg.gif">
            <div align="right">
                <asp:ImageButton ID="pdfPrint" runat="server" Height="20px" 
                    ImageUrl="~/photo/pdf.gif" onclick="pdfPrint_Click" />
            </div>
        </th>
        <td width="6px" style="height:23px;" background="photo/box_topright.gif"></td>   
      </tr>
      <asp:HiddenField ID="userDataSQL" Value="" runat="server" />
      <asp:HiddenField ID="maxi" Value="" runat="server" />
      <asp:HiddenField ID="cmbDept" runat="server"/>
      <asp:HiddenField ID="chkDist" runat="server"/>
    </table>
    </td>
  </tr>
  <tr>
    <td>
        จังหวัด 
        <asp:TextBox ID="txtProvince" runat="server" AutoPostBack="true" 
            ontextchanged="txtProvince_TextChanged"></asp:TextBox>
        <asp:ImageButton ID="btnSearch" runat="server" CssClass="css" ImageUrl="photo/search.gif"  Width="18px" Height="18px" onclick="btnSearch_Click" />
    </td>
  </tr>
  <tr>
    <td align="center" valign="top"> 
    <table id="tab1" border="1" cellspacing="0" cellpadding="0" class="css" style="width: 750px; border-right: #f6a836; border-top: #f6a836; border-left: #f6a836; border-bottom: #f6a836;" runat="server" rules="all" ></table>
    </td>
  </tr>
  <tr><td height="20px"></td></tr>
</table>
</asp:Content>