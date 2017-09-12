<%@ OutputCache Location="None" VaryByParam="none" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="user_entry.aspx.cs" Inherits="_user_entry" MasterPageFile="~/MasterPage.master" ViewStateEncryptionMode="Always"%>
<asp:Content ID="content" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<table width="750px" border="0" align="center" cellspacing="0" cellpadding="0" class="css">
  <tr>
    <td style="height:25px; width: 950px;" class="header_1">ข้อมูลผู้ใช้งาน</td>
  </tr>
</table>

<table width="750px" align="center" valign="center" border="0" cellspacing="0" cellpadding="0" class="css">
  <tr><td height="10px"></td></tr>
  <tr>
    <td>
    <table width="750px" border="0" cellspacing="0" cellpadding="0" class="css" id="searchBar" runat="server">
        <tr>
            <td width="6px" style="height:23px;" background="photo/box_topleft.gif"></td>   
            <th style="height:20px; width:738px;"  background="photo/box_topbg.gif">
                <div align="left">
                    รายการข้อมูลผู้ใช้งาน
                    <asp:TextBox ID="txtKeyword" runat="server" CssClass="css" Width="100px"></asp:TextBox>
                    <asp:Button ID="btnSearch" runat="server" CssClass="css" Text="ค้นหา" 
                        Width="70px" onclick="btnSearch_Click" />
                </div>
            </th>
            <td width="6px" style="height:23px;" background="photo/box_topright.gif"></td>   
        </tr>
    </table>
    </td>
  </tr>
    <tr>
    <td align="left" valign="top"> 
    <table id="dtGrid" border="1" cellspacing="0" cellpadding="0" class="css" style="width: 740px; border-right: #f6a836; border-top: #f6a836; border-left: #f6a836; border-bottom: #f6a836;" runat="server" rules="all" ></table>
    </td>
  </tr>
  <tr><td height="20px"></td></tr>
</table>

<table width="750px" align="center" valign="center" border="0" cellspacing="0" cellpadding="0" class="css">
  <tr><td height="10px"></td></tr>
  <tr>
    <td>
    <table width="750px" border="0" cellspacing="0" cellpadding="0" class="css">
        <tr>
            <td width="6px" style="height:23px;" background="photo/box_topleft.gif"></td>   
            <th style="height:20px; width:738px;"  background="photo/box_topbg.gif"><div align="left"><b>แบบบันทึกข้อมูล</b></div></th>
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
                <table cellspacing="0" cellpadding="0" width="100%">            
                    <tr>
                        <td class="choice" style="width: 15%">รหัสผู้ใช้งาน :</td>
                        <td class="choice" style="width: 35%"><asp:TextBox ID="txtEmail" CssClass="css" runat="server" MaxLength="20"></asp:TextBox>
                            <font color="red">*</font>
                            &nbsp;<asp:Label ID="lblResponse1" Visible="false" Text="" CssClass="css" ForeColor="Red" runat="server" />                            
                        </td>
                        <td class="choice" style="width: 12%">ชื่อผู้ใช้งาน :</td>
                        <td class="choice" style="width: 50%"><asp:TextBox ID="txtName" CssClass="css" runat="server" MaxLength="250"></asp:TextBox>
                            <font color="red">*</font>
                            &nbsp;<asp:Label ID="lblResponse2" Visible="false" Text="" CssClass="css" ForeColor="Red" runat="server" />                            
                        </td>
                    </tr>
                    <tr>
                        <td class="choice"><asp:Label id="lblPass1" runat="server" CssClass="css" Text="รหัสผ่าน : " /></td>
                        <td class="choice"><asp:TextBox ID="txtPass1" runat="server" CssClass="css" TextMode="Password"></asp:TextBox>
                            <asp:Label ID="lblS1" Text="*" ForeColor="Red" CssClass="css" runat="server"/>
                            <asp:Label ID="lblResponse3" Visible="false" Text="" CssClass="css" ForeColor="Red" runat="server" />                            
                        </td>
                        <td class="choice"><asp:Label id="lblPass2" runat="server" CssClass="css" Text="พิมพ์รหัสผ่านซ้ำ :" /></td>
                        <td class="choice"><asp:TextBox ID="txtPass2" runat="server" CssClass="css" TextMode="Password"></asp:TextBox>
                            <asp:Label ID="lblS2" Text="*" ForeColor="Red" CssClass="css" runat="server"/>
                            <asp:Label ID="lblResponse4" Visible="false" Text="" CssClass="css" ForeColor="Red" runat="server" />                            
                        </td>
                   </tr>
                    <tr>
                        <td class="choice"></td>
                        <td class="choice"><asp:CheckBox ID="CheckBox6" runat="server" Text="ระงับการใช้งาน" /></td>
                        <td class="choice"><asp:Label ID="lblRight" runat="server" CssClass="css" Text="สิทธิ์ใช้งาน :"/></td>
                        <td class="choice" style="width: 20%">
                            <asp:DropDownList ID="cmbRight" runat="server" CssClass="css">
                                <asp:ListItem Text="ผู้ชม" Value="V"></asp:ListItem>
                                <asp:ListItem Text="ผู้ดูแลระบบ" Value="A"></asp:ListItem>
                                <asp:ListItem Text="ผู้บริหาร" Value="M"></asp:ListItem>
                                <asp:ListItem Text="เจ้าหน้าที่" Value="U"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>                                                  
                    <tr>
                        <td class="choice"><asp:Label ID="lblDept" runat="server" CssClass="css" Text="หน่วยงาน :" /></td>
                        <td class="choice">
                            <asp:DropDownList ID="cmbDept" runat="server" CssClass="css">
                            </asp:DropDownList>
                        </td>
                    </tr>                                                  
		            <tr>
                      <td class="choice" style="width: 5%"></td>
                      <td class="choice" align="right" style="height: 17px" colspan="3"><asp:Label ID="lblResponse" CssClass="css" Text="" runat="server" Width="50%"></asp:Label><div align="right"><asp:Button ID="btnUpdate" runat="server" class="css" Text="บันทึก" OnClick="btnUpdate_Click"  Width="70px"/>
                          <asp:Button ID="brnClear" runat="server" Text="คืนค่า" CssClass="css" 
                              Width="70px" onclick="brnClear_Click"/>
						</div></td>
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