<%@ Page Language="C#" AutoEventWireup="true" CodeFile="detail_REP2.aspx.cs" MasterPageFile="~/MasterPage.master" Inherits="_detail_REP2" ViewStateEncryptionMode="Always"%>
<asp:Content ID="content" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<table width="750px" border="0" align="center" cellspacing="0" cellpadding="0" class="css">
  <tr>
    <td style="height:25px;width:750px;" class="header_1">รายงานรายละเอียดมวลชน <asp:Label ID="qParam" value="" runat="server" /></td>
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
      <tr>
        <td width="6px"></td>
        <td colspan="2">
           <asp:Label ID="lblGroup" runat="server" Text="เฉพาะหน่วยงาน :" CssClass="css" Visible="false"></asp:Label>
           <asp:DropDownList ID="cmbGroup" runat="server" CssClass="css" 
                AutoPostBack="true" onselectedindexchanged="cmbGroup_SelectedIndexChanged" Visible="false"></asp:DropDownList>
        </td>
        <td width="6px"></td>
      </tr>
      <asp:HiddenField ID="idParam" Value="" runat="server" />
      <asp:HiddenField ID="userDataSQL" Value="" runat="server" />
      <asp:HiddenField ID="maxi" Value="" runat="server" />
    </table>
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