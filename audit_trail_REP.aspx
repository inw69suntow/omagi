<%@ Page Language="C#" AutoEventWireup="true" CodeFile="audit_trail_REP.aspx.cs" Inherits="_audit_trail_REP" MasterPageFile="~/MasterPage.master" ViewStateEncryptionMode="Always"%>
<asp:Content ID="content" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<table width="750px" border="0" align="center" cellspacing="0" cellpadding="0" class="css">
  <tr>
    <td style="height:25px;width:750px;" class="header_1">บันทึกการใช้งาน</td>
  </tr>
</table>

<table width="750px" align="center" valign="center" border="0" cellspacing="0" cellpadding="0" class="css">
  <tr><td height="10px"></td></tr>
  <tr>
    <td>
    <table width="750px" border="0" cellspacing="0" cellpadding="0" class="css">
        <tr>
            <td width="6px" style="height:23px;" background="photo/box_topleft.gif"></td>   
            <th style="height:20px; width:738px;"  background="photo/box_topbg.gif"><div align="left"><b>เงื่อนไขการค้นหาข้อมูล</b></div></th>
            <td width="6px" style="height:23px;" background="photo/box_topright.gif"></td>   
        </tr>
    </table>
    </td>
  </tr>
  <tr>
    <td> 
    <table width="750px" border="1" style="border:1px solid;" cellspacing="0" cellpadding="0" bgcolor="#ffffff">
        <tr>
            <td>
                <table cellspacing="0" cellpadding="0">            
                    <tr>
                        <td class="choice" style="height:20px; width: 10%">&nbsp;รหัสผู้ใช้งาน :</td>
	                    <td class="choice" style="height:20px;" colspan="3">
                            <asp:TextBox ID="txtUID" CssClass="css" runat="server" Width="90px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr> 
                        <td class="choice" style="height:20px; width: 10%">&nbsp;ตั้งแต่วันที่ : </td>
        	            <td class="choice" style="height:20px; width: 40%">
                            <asp:DropDownList CssClass="css" ID="Fr_DD" runat="server" Height="20px" Width="40px"> </asp:DropDownList>
                            <asp:DropDownList CssClass="css" ID="Fr_MM" runat="server" Height="20px" Width="75px"> </asp:DropDownList>
                            <asp:DropDownList CssClass="css" ID="Fr_YY" runat="server" Height="20px" Width="60px"> </asp:DropDownList>
                        </td>
	                    <td class="choice" style="height:20px; width: 10%">ถึงวันที่ :</td>
	                    <td class="choice" style="height:20px; width: 40%">
                            <asp:DropDownList CssClass="css" ID="To_DD" runat="server" Height="20px" Width="40px"> </asp:DropDownList>
                            <asp:DropDownList CssClass="css" ID="To_MM" runat="server" Height="20px" Width="75px"> </asp:DropDownList>
                            <asp:DropDownList CssClass="css" ID="To_YY" runat="server" Height="20px" Width="60px"> </asp:DropDownList>
                            <asp:ImageButton ID="btnSearch" runat="server" CssClass="css" ImageUrl="photo/search.gif" 
                                Width="18px" Height="18px" onclick="btnSearch_Click" />
	                        <asp:ImageButton ID="btnExport" class="css" runat="server" OnClick="btnExport_Click" ImageUrl="photo/x.gif"  Width="18px" Height="18px"/>
                        </td>
                    </tr>
                    <tr>
                        <td height="20px" class="choice">
                            &nbsp;
                        </td>
                        <td height="20px" colspan="3" class="choice">
                            <asp:Label ID="lblResponseS" CssClass="css" ForeColor="Red" Visible="false" runat="server"/>
                        </td>
                    </tr>
                    <tr><td height="20px" colspan="4">&nbsp;</td></tr>
                </table>
            </td>
        </tr>
      </table>
    </td>
  </tr>
  <tr><td height="20px"></td></tr>
</table>

<table  id="bar" runat="server"  visible="false" width="750px" align="center" valign="center" border="0" cellspacing="0" cellpadding="0" class="css">
  <tr>
    <td>
    <table width="750px" border="0" cellspacing="0" cellpadding="0" class="css">
      <tr>
        <td width="6px" style="height:23px;" background="photo/box_topleft.gif"></td>   
        <th style="height:23px; width:738px;" background="photo/box_topbg.gif"><div align="left">บันทึกการใช้งาน</div></th>
        <td width="6px" style="height:23px;" background="photo/box_topright.gif"></td>   
      </tr>
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