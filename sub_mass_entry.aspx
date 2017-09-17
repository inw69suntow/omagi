<%@ OutputCache Location="None" VaryByParam="none" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="sub_mass_entry.aspx.cs" Inherits="_mass_entry" MasterPageFile="~/MasterPage.master" ViewStateEncryptionMode="Always"%>
<asp:Content ID="content2" ContentPlaceHolderID="head" runat="server">
    <meta name="viewport" content="initial-scale=1.0, user-scalable=no" />
    <style type="text/css">
      #map_canvas { height: 100% }
    </style>
    <script type="text/javascript"
      src="http://maps.googleapis.com/maps/api/js?key=AIzaSyBhV-8I34Asyh9676tt9tkHKN0oL4YCce4&sensor=true">
    </script>
    <script type="text/javascript">
        function initialize() {
            var latlong;
            if (document.getElementById("ctl00_ContentPlaceHolder1_txtGoogle").value != "") {
                latLong = document.getElementById("ctl00_ContentPlaceHolder1_txtGoogle").value.split(",");
            } else {
                latLong = "13.779128,100.512226".split(",");
            }

            var mapOptions = {
                center: new google.maps.LatLng(latLong[0], latLong[1]),
                zoom: 15,
                mapTypeId: google.maps.MapTypeId.ROADMAP
            };
            var map = new google.maps.Map(document.getElementById("map_canvas"),
        mapOptions);
            var marker = new google.maps.Marker({
                position: map.getCenter(),
                map: map,
                title: '�����'
            });
        }
    </script>
</asp:Content>
<asp:Content ID="content" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/vbscript">
sub prepareText()
    fileName=document.getElementById("ctl00_ContentPlaceHolder1_exclFile").value
    if trim(fileName)<>"" then
        Set Excl = CreateObject("excel.application")
	    Excl.Workbooks.Open fileName
	    Set WB = Excl.Activeworkbook
        Set WS = WB.Worksheets(1)

	    i=1
	    while ws.cells(i,2)<>""
		    if document.getElementById("ctl00_ContentPlaceHolder1_importText").value<>"" then document.getElementById("ctl00_ContentPlaceHolder1_importText").value=document.getElementById("ctl00_ContentPlaceHolder1_importText").value & ","
		
            for j=1 to 10
    		    document.getElementById("ctl00_ContentPlaceHolder1_importText").value=document.getElementById("ctl00_ContentPlaceHolder1_importText").value & ws.cells(i,j) & "|"
            next

		    i=i+1
	    wend
	    WB.Close
	    Excl.Quit
        Set WB = Nothing
        Set WS = Nothing
        Set Excl = Nothing
    end if
end sub
</script>

<script type="text/javascript">


    function checkboxClick(chckbox, id, parent,master) {
        if ($(chckbox).is(':checked')) {
            document.location = "sub_mass_entry.aspx?hid=" + id + "&parent_id=" + parent + "&parentSearch=" + master
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

</script>

<table width="750px" border="0" align="center" cellspacing="0" cellpadding="0" class="css">
  <tr>
    <td style="height:25px; width: 750px;" class="header_1">�������ç������� �ͧ �ç�����ѡ  
     <asp:Label ID="parentProjectName" runat="server" Text=""></asp:Label>
    </td>
  </tr>
</table>

<table width="750px" align="center" valign="center" border="0" cellspacing="0" cellpadding="0" class="css">
  <tr><td height="10px"></td></tr>
  <tr>
    <td>
        <table width="750px" border="0" cellspacing="0" cellpadding="0" class="css">
            <tr>
                <td width="6px" style="height:23px;" background="photo/box_topleft.gif"></td>   
                <th style="height:20px; width:598px;"  background="photo/box_topbg.gif">
                    <div align="left" valign="middle"><b>��¹���ç�������</b>
                    </div>
                </th>
                <th style="height:20px; width:140px;"  background="photo/box_topbg.gif">
                    ˹�ҷ�� <asp:DropDownList ID="pageID" CssClass="css" 
                        runat="server" AutoPostBack="true" 
                        onselectedindexchanged="pageID_SelectedIndexChanged" />
                    <asp:Button ID="btnPrev" runat="server" CssClass="css" Width="18px" 
                        Text="&lt;" OnClick="btnPrev_Click" />
                    <asp:Button ID="btnNext" runat="server" CssClass="css" Width="18px" 
                        Text="&gt;" OnClick="btnNext_Click"/>
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
                    <table id="dtGrid" border="1" cellspacing="0" cellpadding="0" class="css" style="width: 750px; border-right: #f6a836; border-top: #f6a836; border-left: #f6a836; border-bottom: #f6a836;" runat="server" rules="all" ></table>
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
                            <td class="choice" style="width: 200px">˹��§ҹ�����Ţ�����:</td>
                            <td class="choice">
                                <asp:DropDownList ID="cmbDeptSearch" cssClass="css" runat="server">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr height="20px">
                            <td class="choice" >��鹷�軯Ժѵԡ�èѧ��Ѵ:</td>
                            <td class="choice" >
                                <asp:DropDownList ID="cmbProvinceSearch" cssClass="css" runat="server">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr height="20px">
                            <td class="choice" >�ç�������:</td>
                            <td class="choice" >
                                <asp:TextBox ID="txtKeyword" runat="server" CssClass="css"/>
                                <asp:ImageButton ID="btnSearch" runat="server" CssClass="css" ImageUrl="~/photo/search.gif" Width="18px" Height="18px" OnClick="btnSearch_Click" />
                            </td>
                        </tr>
                      </table>
                </td>
            </tr>
         </table>
     </td>
  </tr>
  <tr><td height="10px"></td></tr>
  <tr>
    <td>
        <table width="750px" border="0" cellspacing="0" cellpadding="0" class="css">
            <tr>
                <td width="6px" style="height:23px;" background="photo/box_topleft.gif"></td>   
                <th style="height:20px; width:238px;"  background="photo/box_topbg.gif"><div align="left"><b>
                    Ẻ�ѹ�֡������</b></div></th>
                <th style="height:20px; width:500px;"  background="photo/box_topbg.gif">
		<div align="right">
			<asp:Button ID="btnClear" Cssclass="css" runat="server" OnClick="btnClear_Click" Text="���ҧ����"  Width="70px"/>
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
                            <td class="choice" style="width: 200px">�������������Ū�:</td>
                            <td class="choice" >
                                <asp:HiddenField ID="hdParentId" runat="server" />
                                <asp:HiddenField ID="hdParentIdSearch" runat="server" />
                                <asp:DropDownList ID="cmbMassGroup" runat="server" CssClass="css"></asp:DropDownList>
                            <font color="red">*</font>
                            </td>
                        </tr
                         <tr height="20px">
                            <td class="choice" >�����ç������:</td>
                            <td class="choice" >
                                <asp:HiddenField ID="HiddenField1" runat="server" />
                                <asp:Label ID="lbParentName" runat="server"></asp:Label>
                                 <asp:DropDownList ID="ddParent1" runat="server" CssClass="css"  ></asp:DropDownList>
                            </td>
                        </tr>                     
                        <tr height="20px">
                            <td class="choice" >�����ç�������:</td>
                            <td class="choice" >
                                <asp:TextBox ID="txtMassName" runat="server" width="200px" MaxLength="500" CssClass="css"/>
                            </td>
                        </tr>
                         <tr height="20px">
                            <td class="choice" >�дѺ�ç���:</td>
                            <td class="choice" >
                                  <asp:DropDownList ID="ddProjectLevel" cssClass="css" runat="server"  
                                      AutoPostBack="True" onselectedindexchanged="ddParent1_SelectedIndexChanged">
                                 </asp:DropDownList>
                            </td>
                        </tr>
                        <tr height="20px">
                            <td class="choice" >�ӹǹ�������§ҹ:</td>
                            <td class="choice" >
                                <asp:Textbox ID="txtReportAmount" runat="server" CssClass="css" Width="100" MaxLength="10" /> ��
                                <asp:RegularExpressionValidator ID="egtxtReportAmount"  ControlToValidate="txtReportAmount" runat="server"
                                    ErrorMessage="��ͧ�繵���Ţ��ҹ��"
                                    ValidationExpression="\d+">
                                </asp:RegularExpressionValidator>
                            </td>
                        </tr>
                        <tr height="20px">
                            <td class="choice">˹��§ҹ������:</td>
                            <td class="choice">
                                <asp:DropDownList ID="cmbDept" runat="server" CssClass="css"></asp:DropDownList>
                                <font color="red">*</font>
                           </td>
                        </tr>
                        <tr height="20px">
                            <td class="choice">��鹷�軯Ժѵԡ�èѧ��Ѵ:</td>
                            <td class="choice">
                                <asp:DropDownList ID="cmbProvince" runat="server" CssClass="css" 
                                    AutoPostBack="true" onselectedindexchanged="cmbProvince_SelectedIndexChanged"></asp:DropDownList>
                                <font color="red">*</font>
                            </td>
                        </tr>
                        <tr height="20px">
                            <td class="choice">�����/ࢵ:</td>
                            <td class="choice">
                                <asp:DropDownList ID="cmbDistrict" runat="server" CssClass="css" 
                                    AutoPostBack="true" onselectedindexchanged="cmbDistrict_SelectedIndexChanged"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr height="20px">
                            <td class="choice">�Ӻ�/�ǧ:</td>
                            <td class="choice">
                                <asp:DropDownList ID="cmbTambon" runat="server" CssClass="css"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr height="20px">
                            <td class="choice">������:</td>
                            <td class="choice">
                                <asp:Textbox ID="txtMoo" runat="server" CssClass="css" Width="30px" MaxLength="2"/>
                                 <asp:RegularExpressionValidator ID="rgtxtMoo"  ControlToValidate="txtMoo" runat="server"
                                    ErrorMessage="��ͧ�繵���Ţ��ҹ��"
                                    ValidationExpression="\d+">
                                </asp:RegularExpressionValidator>
                            </td>
                        </tr>
                       <tr height="20px" valign="top">
                            <td class="choice">�ԡѴ Google:</td>
                            <td class="choice">
                                <asp:Textbox ID="txtGoogle" runat="server" CssClass="css" Width="250px" MaxLength="25"/>
                            </td>
                        </tr>
                        <tr height="240px">
                            <td class="choice" colspan="2" align="center">
                                <div id="map_canvas" style="width:100%; height:100%"></div>
                            </td>
                        </tr>
                       <tr height="20px" valign="bottom">
                            <td class="choice" colspan="2" align="right"><br />
                            <asp:HiddenField ID="hidID" Value="" runat="server" />
                              <asp:HiddenField ID="maxi" Value="" runat="server" />
                                <asp:Label ID="lblResponse" runat="server" CssClass="css" Width="95%"/>
                                <asp:Button ID="btnSave" runat="server" CssClass="css" Text="��Ѻ��ا" 
                                    Width="70px" OnClick="btnSave_Click" />
                                <asp:Button ID="btnMember" CssClass="css" runat="server" Text="��Ҫԡ" 
                                    Width="70px" OnClick="btnMember_Click" />
                                <asp:Button ID="btnDel" runat="server" CssClass="css" Text="ź������" 
                                    Width="70px" OnClick="btnDel_Click"  />
                            </td>
                        </tr>
                        <tr height="20px">
                            <td class="choice">Create:</td>
                            <td class="choice">
                                <asp:Label ID= "lblCreate" runat="server" CssClass="css" Text="" />
                            </td>
                        </tr>
                        <tr height="20px">
                            <td class="choice">Last updated:</td>
                            <td class="choice">
                                <asp:Label ID= "lblUpdate" runat="server" CssClass="css" Text="" />
                            </td>
                        </tr>
                      </table>
                </td>
            </tr>
         </table>
     </td>
  </tr>
</table>
</asp:Content>