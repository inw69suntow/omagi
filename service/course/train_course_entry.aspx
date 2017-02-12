<%@ OutputCache Location="None" VaryByParam="none" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="train_course_entry.aspx.cs" Inherits="_train_course_entry" MasterPageFile="~/MasterPage.master" ViewStateEncryptionMode="Always"%>
<asp:Content ID="content" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<table width="750px" border="0" align="center" cellspacing="0" cellpadding="0" class="css">
  <tr>
    <td style="height:25px; width: 750px;" class="header_1">&nbsp;ข้อมูลหลักสูตรการฝึกอบรม</td>
  </tr>
</table>

<table width="750px" align="center"  border="0" cellspacing="0" cellpadding="0" class="css">
  <tr>
    <td>
            <table width="750px" border="0" cellspacing="0" cellpadding="0" class="css">
            <tr>
                <td width="6px" style="height:23px;" background="<%=Page.ResolveClientUrl("~/photo/box_topleft.gif")%>"></td>   
                <th style="height:20px; width:688px;"  background="<%=Page.ResolveClientUrl("~/photo/box_topbg.gif")%>">
                    <div align="left"  valign="middle">
                        <b>รายการข้อมูลหลักสูตรการฝึกอบรม</b>
                    </div>
                </th>
                <th style="height:20px; width:200px;"  background="<%=Page.ResolveClientUrl("~/photo/box_topbg.gif")%>">
                <div align="right">
                    <asp:Button ID="btnCreate" runat="server" class="css" Text="บันทึก" OnClick="btnCreate_Click" OnClientClick="composeValue();" Width="70px"/>
                    <input type="reset" value="คืนค่า" class="css" style="width:70px" />
                </div>
                </th>
                <td width="6px" style="height:23px;" background="photo/box_topright.gif"></td>   
            </tr>
        </table>
        </td>
        </tr>        
        <tr>
            <td align="left" valign="top">
                <table id="dtGrid" border="1" cellspacing="0" cellpadding="0" class="css" style="width: 740; border-right: #15ff15; border-top: #15ff15; border-left: #15ff15; border-bottom: #15ff15;" runat="server" rules="all" ></table>
            </td>    
        </tr>
        <asp:HiddenField ID="maxJ" runat="server" Value="0" />
        <asp:HiddenField ID="newVal" runat="server" Value="" />
</table>
<script language="javascript">
    function composeValue() {
        var i = document.getElementById("maxJ").value;
        document.getElementById("newVal").value = "";
        for (x = 1; x < i; x++) {
            if (document.getElementById("newVal").value != "") document.getElementById("newVal").value += ",";
            document.getElementById("newVal").value += document.getElementById("id_" + x).value;
        }
    }
</script>
</asp:Content>