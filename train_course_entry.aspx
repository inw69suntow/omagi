<%@ OutputCache Location="None" VaryByParam="none" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="train_course_entry.aspx.cs" Inherits="_train_course_entry" MasterPageFile="~/MasterPage.master" ViewStateEncryptionMode="Always"%>
<asp:Content ID="content" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


<table width="750px" border="0" align="center" cellspacing="0" cellpadding="0" class="css">
  <tr>
    <td style="height:25px; width: 750px;" class="header_1">&nbsp;��������ѡ�ٵá�ý֡ͺ��</td>
    <asp:HiddenField ID="hdSortName" runat="server" />
  </tr>
</table>

<table width="750px" align="center"  border="0" cellspacing="0" cellpadding="0" class="css">
  <tr>
    <td>
            <table width="750px" border="0" cellspacing="0" cellpadding="0" class="css">
            <tr>
                <td width="6px" style="height:23px;" background="photo/box_topleft.gif"></td>   
                <th style="height:20px; width:688px;"  background="photo/box_topbg.gif">
                    <div align="left"  valign="middle">
                        <b>��¡�â�������ѡ�ٵá�ý֡ͺ��</b>
                    </div>
                </th>
                <th style="height:20px; width:200px;"  background="photo/box_topbg.gif">
                <div align="right">
                <%if (Session["uGroup"].ToString() != "V")
                  { %>
                    <asp:Button ID="btnCreate" runat="server" class="css" Text="�ѹ�֡" OnClick="btnCreate_Click" OnClientClick="composeValue();" Width="70px"/>
                    <input type="reset" value="�׹���" class="css" style="width:70px" />
                    <%} %>
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




        <tr>
            <td>
                <table width="750px" border="0" cellspacing="0" cellpadding="0" class="css">
                    <tr>
                        <td width="6px" style="height:23px;" background="photo/box_topleft.gif"></td>   
                        <th style="height:20px; width:738px;"  background="photo/box_topbg.gif"><div align="left"><b>
                            Ẻ���Ң�����</b></div></th>
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
                                    <td class="choice" >��ѡ�ٵá�ý֡ͺ��:</td>
                                    <td class="choice" >
                                        <asp:TextBox  ID="txtKeyword" runat="server" CssClass="css"/>
                                        <asp:ImageButton ID="btnSearch" runat="server" CssClass="css" ImageUrl="photo/search.gif" 
                                    Width="18px" Height="18px" onclick="btnSearch_Click"  />
                                    </td>
                                </tr>
                              </table>
                        </td>
                    </tr>
                            
                          
                            
                 </table>
             </td>
          </tr>
          </tr>
</table>
<script type="text/javascript">
    function composeValue() {
        var maxJ = '<%= maxJ.ClientID %>';
        var newVal = '<%= newVal.ClientID %>';
        var i = document.getElementById(maxJ).value;
        document.getElementById(newVal).value = "";
        for (x = 1; x < i; x++) {
            if (document.getElementById(newVal).value != "") {
                document.getElementById(newVal).value += ",";
            }
            var str=document.getElementById("id_" + x).value+"|"+
                document.getElementById("id_province_" + x).value+"|"+
                document.getElementById("id_budget_" + x).value+"|"+
                document.getElementById("id_objective_" + x).value;
            document.getElementById(newVal).value += str;
        }
    }


    $(function () {
        var txtKeyWord = '<%= txtKeyword.ClientID %>';
        var btnSearch = '<%= btnSearch.ClientID %>';
        document.getElementById(txtKeyWord).addEventListener("keydown", function (e) {
            if (!e) { var e = window.event; }
            //e.preventDefault(); // sometimes useful

            // Enter is pressed
            if (e.keyCode == 13) {
                document.getElementById(btnSearch).click();
            }
        }, false);
    });


    function sortColumn(title) {
        var hdSortName = '<%= hdSortName.ClientID %>';
        var btnSearch = '<%= btnSearch.ClientID %>';
        document.getElementById(hdSortName).value = title;
        document.getElementById(btnSearch).click();
    }
</script>
</asp:Content>