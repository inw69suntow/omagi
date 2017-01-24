<%@ Page Language="C#" AutoEventWireup="true" CodeFile="report_REP.aspx.cs" Inherits="_report_REP" MasterPageFile="~/MasterPage.master" ViewStateEncryptionMode="Always"%>
<asp:Content ID="content" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<table width="750px" border="0" align="center" cellspacing="0" cellpadding="0" class="css">
  <tr>
    <td style="height:25px;width:750px;" class="header_1">รายงาน</td>
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
                        <td class="choice" style="height:20px; width: 10%">&nbsp;ชื่อรายงาน :</td>
	                    <td class="choice" style="height:20px;" colspan="3">
                            <asp:DropDownList ID="cmbReport" runat="server" CssClass="css" Width="90%"></asp:DropDownList>
                        </td>
                    </tr>
                    <tr> 
                        <td class="choice" style="height:100px; width: 10%">&nbsp;หน่วยงาน : </td>
        	            <td class="choice" style="height:100px; width: 40%">
                            <asp:ListBox ID="cmbDept" cssClass="css" runat="server" SelectionMode="Multiple" Height="100px" Width="90%" />
                        </td>
	                    <td class="choice" style="height:20px; width: 10%">จังหวัด :</td>
	                    <td class="choice" style="height:20px; width: 40%">
                            <asp:ListBox ID="cmbProvince" cssClass="css" runat="server" SelectionMode="Multiple" Height="100px" Width="90%" />
                        </td>
                    </tr>
                    <tr> 
                        <td class="choice" style="height:20px; width: 10%">&nbsp;เฉพาะมวลชน : </td>
        	            <td class="choice" style="height:20px; width: 15%">
                                <asp:ListBox ID="cmbMassGroup" runat="server" CssClass="css" SelectionMode="Multiple" Height="100px" Width="90%" />
                        </td>
	                    <td class="choice" colspan="2" align="right" valign="bottom">
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
</table>

<table  id="tabGraph" runat="server"  visible="false" width="750px" align="center" valign="center" border="0" cellspacing="0" cellpadding="0" class="css">
  <tr>
    <td>
    <table width="750px" border="0" cellspacing="0" cellpadding="0" class="css">
      <tr>
        <td width="6px" style="height:23px;" background="photo/box_topleft.gif"></td>   
        <th style="height:23px; width:638px;" background="photo/box_topbg.gif"><div align="left">ภาพกราฟรายงาน</div></th>
        <th style="height:23px; width:100px;" background="photo/box_topbg.gif">
            <div align="right">
                <asp:ImageButton ID="pdfPrint" runat="server" Height="20px" 
                    ImageUrl="~/photo/pdf.gif" onclick="pdfPrint_Click" />
                <asp:ImageButton ID="xclPrint" runat="server" Height="20px" ImageUrl="~/photo/x.gif" OnClientClick="xclPrint()" />
            </div>
        </th>
        <td width="6px" style="height:23px;" background="photo/box_topright.gif"></td>   
      </tr>
    </table>
    </td>
  </tr>
  <tr>
    <td align="center" valign="top"> 
    <table id="tabDisplay" border="1" cellspacing="0" cellpadding="0" class="css" style="width: 750px; border-right: #f6a836; border-top: #f6a836; border-left: #f6a836; border-bottom: #f6a836;" runat="server" rules="all" >
        <tr>
            <td colspan="2" align="center">
            <asp:Image ID="mainChart" runat="server" Height="250px" Width="750px" ImageUrl="" />
            </td>
        </tr>
        <tr>
            <td align="center">
            <asp:Image ID="sub1" runat="server" Height="250px" Width="375px" ImageUrl="" />
            </td>
            <td align="center">
            <asp:Image ID="sub2" runat="server" Height="250px" Width="375px" ImageUrl="" />
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
        <th style="height:23px; width:738px;" background="photo/box_topbg.gif"><div align="left"><asp:Label ID="lblRepTitle" runat="server" CssClass="th"></asp:Label></div></th>
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