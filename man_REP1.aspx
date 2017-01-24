<%@ OutputCache Location="None" VaryByParam="none" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="man_REP1.aspx.cs" Inherits="_man_REP1" MasterPageFile="~/MasterPage.master" ViewStateEncryptionMode="Always"%>
<asp:Content ID="content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<table width="750px" border="0" align="center" cellspacing="0" cellpadding="0" class="css">
  <tr>
    <td style="height:25px; width: 750px;" class="header_1">รายงานข้อมูลบุคคล</td>
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
                        <td class="choice" style="height:20px; width: 20%">&nbsp;ชื่อ : </td>
        	            <td class="choice" style="height:20px; width: 30%">
                            <asp:TextBox ID="txtName" runat="server" CssClass="css" MaxLength="250"/>
                        </td>
	                    <td class="choice" style="height:20px; width: 20%">สกุล :</td>
	                    <td class="choice" style="height:20px; width: 30%">
                            <asp:TextBox ID="txtSurname" runat="server" CssClass="css" MaxLength="250"/>
                        </td>
                    </tr>
                    <tr> 
                        <td class="choice">&nbsp;รหัสประจำตัวประชาชน :</td>
	                    <td class="choice">
                            <asp:TextBox ID="txtID" runat="server" CssClass="css" MaxLength="13"/>
                        </td>
                        <td class="choice">โทรศัพท์มือถือ : </td>
        	            <td class="choice">
                            <asp:TextBox ID="txtMobile" runat="server" CssClass="css" MaxLength="10"/>
                        </td>
                    </tr>
                    <tr> 
                        <td class="choice">&nbsp;โทรศัพท์บ้าน :</td>
	                    <td class="choice">
                            <asp:TextBox ID="txtHomeNo" runat="server" CssClass="css" MaxLength="10"/>
                        </td>
                        <td class="choice" style="height:20px; width: 10%">โทรศัพท์ที่ทำงาน : </td>
        	            <td class="choice" style="height:20px; width: 40%">
                            <asp:TextBox ID="txtOffice" runat="server" CssClass="css" MaxLength="10"/>
                        </td>
                    </tr>
                    <tr>
	                    <td class="choice" colspan="4" align="right" valign="bottom">
                            <asp:Label ID="lblResponseS" CssClass="css" ForeColor="Red" Visible="false" runat="server"/>
	                        <asp:Button ID="btnSearch" class="css" runat="server" OnClick="btnSearch_Click" Text="ค้นหา"  Width="70px"/>
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
  <tr>
    <td>
        <table width="750px" border="0" cellspacing="0" cellpadding="0" class="css">
            <tr>
                <td width="6px" style="height:23px;" background="photo/box_topleft.gif"></td>   
                <th style="height:20px; width:648px;" align="left" background="photo/box_topbg.gif">
                    <div align="left  valign="left"><b>รายนามบุคคล</b> <asp:Label runat="server" ID="lblGroupName" Text=""/>
                    </div>
                </th>
                <th style="height:20px; width:240px;"  background="photo/box_topbg.gif">
                    หน้าที่ <asp:DropDownList ID="pageID" CssClass="css" 
                        runat="server" AutoPostBack="true" 
                        onselectedindexchanged="pageID_SelectedIndexChanged" />
                    <asp:Button ID="btnPrev" runat="server" CssClass="css" Width="18px" 
                        Text="&lt;" onclick="btnPrev_Click" />
                    <asp:Button ID="btnNext" runat="server" CssClass="css" Width="18px" 
                        Text="&gt;" onclick="btnNext_Click" />
                </th>
                <td width="6px" style="height:23px;" background="photo/box_topright.gif"></td>   
            </tr>
        </table>
    </td>
  </tr>
  <tr>
    <td align="left" valign="top">
        <table width="750px" border="1" style="border:1px solid;" cellspacing="0" cellpadding="0" bgcolor="#ffffff">
            <tr>
                <td>
                    <table id="dtGrid" border="1" cellspacing="0" cellpadding="0" class="css" style="width: 740px; border-right: #f6a836; border-top: #f6a836; border-left: #f6a836; border-bottom: #f6a836;" runat="server" rules="all" ></table>
                </td>
            </tr>
       </table>
    </td>
  </tr>
</table>
</asp:Content>