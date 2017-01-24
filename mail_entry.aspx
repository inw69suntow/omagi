<%@ OutputCache Location="None" VaryByParam="none" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="mail_entry.aspx.cs" Inherits="_mail_entry" MasterPageFile="~/MasterPage.master" ViewStateEncryptionMode="Always"%>
<asp:Content ID="content" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<script language="javaScript">
function prepareText() {
    document.getElementById("ctl00_ContentPlaceHolder1_selFile").value = "";
    for(i=0;i<document.getElementById("ctl00_ContentPlaceHolder1_maxi").value*1;i++) {
       if(document.getElementById("check_" + i).checked) {
           if (document.getElementById("ctl00_ContentPlaceHolder1_selFile").value != "") document.getElementById("ctl00_ContentPlaceHolder1_selFile").value += "|";
           document.getElementById("ctl00_ContentPlaceHolder1_selFile").value += document.getElementById("name_" + i).value;
       }
    }
}              
</script>
<div align="center">
<table width="750px" border="0" align="center" cellspacing="0" cellpadding="0" class="css">
  <tr>
    <td style="height:25px; width: 750px;" class="header_1">ส่งเมล์</td>
  </tr>
</table>

<table width="750px" align="center" valign="center" border="0" cellspacing="0" cellpadding="0" class="css">
  <tr><td height="10px"></td></tr>
  <tr>
    <td>
        <table width="750px" border="0" cellspacing="0" cellpadding="0" class="css">
            <tr>
                <td width="6px" style="height:23px;" background="photo/box_topleft.gif"></td>   
                <th style="height:20px; width:738px;"  background="photo/box_topbg.gif">
                    <div align="left" valign="middle"><b>จดหมาย</b>
                    </div>
                </th>
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
                    <table border="0" align="center" width="100%" cellspacing="0" cellpadding="0">            
                        <tr height="20px">
                            <td class="choice" style="width: 25%">ที่อยู่ผู้ส่งจดหมาย (From):</td>
                            <td class="choice">
                                <asp:TextBox CssClass="css" MaxLength="250" ID="txtReply" Width="100%" runat="server"/>
                            </td>
                        </tr>
                        <tr height="20px">
                            <td class="choice">รายการที่อยู่ผู้รับจดหมาย (To):</td>
                            <td class="choice">
                                <asp:TextBox CssClass="css" MaxLength="250" ID="txtTo" Width="100%" runat="server"/>
                            </td>
                        </tr>
                        <tr height="20px">
                            <td class="choice">รายการที่อยู่ผู้รับสำเนา (Cc):</td>
                            <td class="choice">
                                <asp:TextBox CssClass="css" MaxLength="250" ID="txtCc" Width="100%" runat="server"/>
                            </td>
                        </tr>
                        <tr height="20px">
                            <td class="choice">รายการที่อยู่ผู้รับสำเนาลับ (Bcc):</td>
                            <td class="choice">
                                <asp:TextBox CssClass="css" MaxLength="250" ID="txtBcc" Width="100%" runat="server"/>
                            </td>
                        </tr>
                        <tr height="20px">
                            <td class="choice">เรื่อง :</td>
                            <td class="choice">
                                <asp:TextBox CssClass="css" MaxLength="250" ID="txtSubj" Width="100%" runat="server"/>
                            </td>
                        </tr>
                        <tr height="20px">
                            <td class="choice">เอกสารแนบ :</td>
                            <td class="choice">
                                <asp:FileUpload CssClass="css" runat="server" ID="txtAtt" />
                                <asp:Button CssClass="css" ID="btnAttach" runat="server" Text="แนบไฟล์" 
                                    Width="70px" onclick="btnAttach_Click" />
                                <asp:Button ID="btnDel" runat="server" CssClass="css" Text="ลบไฟล์" 
                                    Width="70px" OnClientClick="prepareText()" OnClick="btnDel_Click"/>
                                <asp:HiddenField ID="maxi" runat="server" />
                                <asp:HiddenField ID="selFile" runat="server" />
                            </td>
                        </tr>
                        <tr height="20px">
                            <td class="choice" colspan="2">
                                <table id="dtGrid" border="1" cellspacing="0" cellpadding="0" class="css" style="width: 740px; border-right: #f6a836; border-top: #f6a836; border-left: #f6a836; border-bottom: #f6a836;" runat="server" rules="all" ></table>                                        
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="center">
                            </td>
                        </tr>
                      </table>
                </td>
            </tr>
         </table>
     </td>
  </tr>
  <tr>
    <td>
        <table width="750px" border="0" cellspacing="0" cellpadding="0" class="css">
            <tr>
                <td width="6px" style="height:23px;" background="photo/box_topleft.gif"></td>   
                <th style="height:20px; width:738px;"  background="photo/box_topbg.gif">
                    <div align="left" valign="middle"><b>ข้อความ</b>
                    </div>
                </th>
                <td width="6px" style="height:23px;" background="photo/box_topright.gif"></td>   
            </tr>
        </table>
    </td>
  </tr>
  <tr><td height="10px"></td></tr>
  <tr>
    <td>
        <asp:TextBox ID="txtMsg" Width="100%" runat="server" Rows="10" 
            TextMode="MultiLine"/>
    </td>
  </tr>
  <tr>
    <td align="right"><asp:Label ID="lblResponse" runat="server" CssClass="css"/>
        <asp:Button ID="btnSend" runat="server" cssClass="css" Text="ส่ง" Width="70px" 
            onclick="btnSend_Click" /></td>
  </tr>
</table>
</asp:Content>