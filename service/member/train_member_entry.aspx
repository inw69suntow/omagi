<%@ OutputCache Location="None" VaryByParam="none" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="train_member_entry.aspx.cs" Inherits="_train_member_entry" MasterPageFile="~/MasterPage.master" ViewStateEncryptionMode="Always"%>
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
    <script language="vbscript">
sub prepareText()    
    fileName=document.getElementById("exclFile").value
    if Trim(fileName)<>"" then
        Set Excl = CreateObject("excel.application")
	    Excl.Workbooks.Open fileName
	    Set WB = Excl.Activeworkbook
        Set WS = WB.Worksheets(1)

        document.getElementById("ctl00_ContentPlaceHolder1_importText").value=""
	    i=2
	    while ws.cells(i,2)<>""
		    if document.getElementById("ctl00_ContentPlaceHolder1_importText").value<>"" then document.getElementById("ctl00_ContentPlaceHolder1_importText").value=document.getElementById("ctl00_ContentPlaceHolder1_importText").value & ","
		
            for j=1 to 28
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
    <div align="center">
<table width="750px" border="0" align="center" cellspacing="0" cellpadding="0" class="css">
  <tr>
    <td style="height:25px; width: 750px;" class="header_1">�����ż������Ѻ��ý֡ͺ��</td>
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
                    <div align="left"  valign="middle"><b>��¹���������Ѻ��ý֡ͺ��</b></div>
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
                            <td class="choice" style="width: 200px">��ѡ�ٵ�:</td>
                            <td class="choice">
                                <asp:DropDownList ID="cmbCourseSearch" cssClass="css" runat="server" Width="90px">
                                </asp:DropDownList><font color="red">*</font>
                            </td>
                        </tr>
                        <tr height="20px">
                            <td class="choice" >���:</td>
                            <td class="choice" >
                                <asp:DropDownList ID="cmbGenSearch" cssClass="css" runat="server" Width="50px">
                                </asp:DropDownList>/
                                <asp:DropDownList ID="cmbYearSearch" cssClass="css" runat="server" Width="90px">
                                </asp:DropDownList><font color="red">*</font>
                            </td>
                        </tr>
                        <tr height="20px">
                            <td class="choice" >�ѧ��Ѵ:</td>
                            <td class="choice" >
                                <asp:DropDownList ID="cmbProvinceSearch" cssClass="css" runat="server" Width="90px">
                                </asp:DropDownList><font color="red">*</font>
                            </td>
                        </tr>
                        <tr height="20px">
                            <td class="choice" >˹��§ҹ:</td>
                            <td class="choice" >
                                <asp:DropDownList ID="cmbDeptSearch" cssClass="css" runat="server" Width="70px">
                                </asp:DropDownList><font color="red">*</font>
                            </td>
                        </tr>
                        <tr height="20px">
                            <td class="choice" >�Ӥ���:</td>
                            <td class="choice" >
                                <asp:TextBox ID="txtKeyword" runat="server" CssClass="css" Width="90px"/>                        
                                <asp:ImageButton ID="btnSearch" runat="server" CssClass="css" ImageUrl="photo/search.gif" 
                                    Width="18px" Height="18px" onclick="btnSearch_Click" />
	                            <asp:ImageButton ID="btnExport" class="css" runat="server" OnClick="btnExport_Click" ImageUrl="photo/x.gif"  Width="18px" Height="18px"/>
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
			<b>����Ҩҡ Excel : </b>
                                <input type="file" id="exclFile" class="css"/>
                                <asp:Button ID="btnImport" runat="server" CssClass="css" Text="�����" 
                                    Width="70px" onclick="btnImport_Click" OnClientClick="prepareText()"/>
                                <asp:HiddenField ID="importText" runat="server" value=""/>
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
                            <td class="choice" style="width: 100px">��ѡ�ٵ�:</td>
                            <td class="choice">
                                <asp:DropDownList ID="cmbCourse" runat="server" CssClass="css"></asp:DropDownList>
                                <font color="red">*</font>
                            </td>
                        </tr>
                        <tr height="20px">
                            <td class="choice" >���:</td>
                            <td class="choice" >
                                <asp:DropDownList ID="cmbGen" runat="server" CssClass="css"></asp:DropDownList>
                                /
                                <asp:DropDownList ID="cmbYear" runat="server" CssClass="css"></asp:DropDownList>
                                <font color="red">*</font>
                            </td>
                        </tr>
                        <tr height="20px">
                            <td class="choice">�ѧ��Ѵ:</td>
                            <td class="choice">
                                <asp:DropDownList ID="cmbTrainProvince" runat="server" CssClass="css"></asp:DropDownList>
                                <font color="red">*</font>
                            </td>
                        </tr>
                        <tr height="20px">
                            <td class="choice">˹��§ҹ:</td>
                            <td class="choice">
                                <asp:DropDownList ID="cmbDept" runat="server" CssClass="css"></asp:DropDownList>
                                <font color="red">*</font>
                            </td>
                        </tr>
                        <tr height="20px">
                            <td class="choice" >�ѹ������֡ͺ��:</td>
                            <td class="choice" >
                                <asp:DropDownList ID="cmbStartDD" runat="server" CssClass="css"></asp:DropDownList> -
                                <asp:DropDownList ID="cmbStartMM" runat="server" CssClass="css"></asp:DropDownList> -
                                <asp:DropDownList ID="cmbStartYY" runat="server" CssClass="css"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr height="20px">
                            <td class="choice">�ѹ����ش��ý֡ͺ��:</td>
                            <td class="choice">
                                <asp:DropDownList ID="cmbStopDD" runat="server" CssClass="css"></asp:DropDownList> -
                                <asp:DropDownList ID="cmbStopMM" runat="server" CssClass="css"></asp:DropDownList> -
                                <asp:DropDownList ID="cmbStopYY" runat="server" CssClass="css"></asp:DropDownList>
                            </td>
                        </tr>
                       <tr height="20px">
                            <td class="choice">���˹�:</td>
                            <td class="choice">
                                <asp:DropDownList ID="cmbPos" runat="server" CssClass="css">
                                    <asp:ListItem Text="��иҹ" Value="P"></asp:ListItem>
                                    <asp:ListItem Text="�ͧ��иҹ" Value="V"></asp:ListItem>
                                    <asp:ListItem Text="�Ţҹء��" Value="Y"></asp:ListItem>
                                    <asp:ListItem Text="��Ҫԡ" Value="Z"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr height="20px">
                            <td class="header_2" colspan="2">&nbsp;</td>
                        </tr>
                        <tr height="20px">
                            <td class="choice" >�ѵû�ЪҪ�:</td>
                            <td class="choice" >
                                <asp:TextBox ID="txtCardID" runat="server" width="200px" MaxLength="500" 
                                    CssClass="css" AutoPostBack="true" ontextchanged="txtCardID_TextChanged"/>
                                <font color="red">*</font>
                            </td>
                        </tr>
                        <tr height="20px">
                            <td class="choice">����:</td>
                            <td class="choice">
                                <asp:DropDownList ID="cmbTitle" runat="server" CssClass="css"></asp:DropDownList>
                                <asp:Textbox ID="txtFName" runat="server" CssClass="css" Width="200" MaxLength="250" />
                                <asp:Textbox ID="txtSName" runat="server" CssClass="css" Width="200" MaxLength="250" />
                                <font color="red">*</font>
                            </td>
                        </tr>
                        <tr height="20px">
                            <td class="choice">��ҹ�Ţ���:</td>
                            <td class="choice">
                                <asp:Textbox ID="txtAddrNo" runat="server" CssClass="css" Width="100" MaxLength="250" />
                            </td>
                        </tr>
                        <tr height="20px">
                            <td class="choice">�����ҹ/�Ҥ��:</td>
                            <td class="choice">
                                <asp:Textbox ID="txtHome" runat="server" CssClass="css" Width="200" MaxLength="250" />
                            </td>
                        </tr>
                        <tr height="20px">
                            <td class="choice">���:</td>
                            <td class="choice">
                                <asp:Textbox ID="txtSoy" runat="server" CssClass="css" Width="200" MaxLength="250" />
                            </td>
                        </tr>
                        <tr height="20px">
                            <td class="choice">���:</td>
                            <td class="choice">
                                <asp:Textbox ID="txtRoad" runat="server" CssClass="css" Width="200" MaxLength="250" />
                            </td>
                        </tr>
                        <tr height="20px">
                            <td class="choice">�ѧ��Ѵ:</td>
                            <td class="choice">
                                <asp:DropDownList ID="cmbProvince" runat="server" CssClass="css" 
                                    AutoPostBack="true" onselectedindexchanged="cmbProvince_SelectedIndexChanged"></asp:DropDownList>                            </td>
                        </tr>
                        <tr height="20px">
                            <td class="choice">�����/ࢵ:</td>
                            <td class="choice">
                                <asp:DropDownList ID="cmbDistrict" runat="server" CssClass="css" 
                                    AutoPostBack="true" onselectedindexchanged="cmbDistrict_SelectedIndexChanged"></asp:DropDownList>                            </td>
                        </tr>
                        <tr height="20px">
                            <td class="choice">�Ӻ�/�ǧ:</td>
                            <td class="choice">
                                <asp:DropDownList ID="cmbTambon" runat="server" CssClass="css"></asp:DropDownList>                            </td>
                        </tr>
                        <tr height="20px">
                            <td class="choice">������:</td>
                            <td class="choice">
                                <asp:Textbox ID="txtMoo" runat="server" CssClass="css" Width="30px" MaxLength="2"/>                            </td>
                        </tr>
                       <tr height="20px">
                            <td class="choice">������ɳ���:</td>
                            <td class="choice">
                                <asp:Textbox ID="txtPost" runat="server" CssClass="css" Width="100" MaxLength="5" />
                            </td>
                        </tr>
                        <tr height="20px">
                            <td class="choice" >���Ѿ���ҹ:</td>
                            <td class="choice" >
                                <asp:Textbox ID="txtTelNo" runat="server" CssClass="css" Width="100" MaxLength="10" />
                            </td>
                        </tr>
                       <tr height="20px">
                            <td class="choice">���Ѿ����ӧҹ:</td>
                            <td class="choice">
                                <asp:Textbox ID="txtOffNo" runat="server" CssClass="css" Width="100" MaxLength="10" />
                            </td>
                        </tr>
                        <tr height="20px">
                            <td class="choice" >���Ѿ����Ͷ��:</td>
                            <td class="choice" >
                                <asp:Textbox ID="txtMobNo" runat="server" CssClass="css" Width="100" MaxLength="10" />
                            </td>
                        </tr>
                       <tr height="20px">
                            <td class="choice">������:</td>
                            <td class="choice">
                                <asp:Textbox ID="txtEmail" runat="server" CssClass="css" Width="200" MaxLength="250" />
                            </td>
                        </tr>
                        <tr height="20px">
                            <td class="choice" >�ѹ�Դ:</td>
                            <td class="choice" >
                                <asp:DropDownList ID="cmbBDD" runat="server" CssClass="css"></asp:DropDownList> -
                                <asp:DropDownList ID="cmbBMM" runat="server" CssClass="css"></asp:DropDownList> -
                                <asp:DropDownList ID="cmbBYY" runat="server" CssClass="css"></asp:DropDownList>
                            </td>
                        </tr>
                       <tr height="20px">
                            <td class="choice" ></td>
                            <td class="choice">
                                <asp:CheckBox ID="chkTargetFlag" runat="server" CssClass="css" Text="�Թ����������Ԩ�����ء���駷�� �����. ��ͧ��" />
                            </td>                            
                        </tr>
                        <tr height="20px">
                            <td class="choice" ></td>
                            <td class="choice" >
                                <asp:CheckBox ID="chkStatus" runat="server" CssClass="css" Text="¡��ԡ/���ª��Ե" />
                            </td>
                        </tr>
                        <tr height="20px">
                            <td class="choice" >�ٻ��������:</td>
                            <td class="choice" >
                                <asp:FileUpload id="imgFile" cssclass="css" runat="server"/>
                                <asp:Button ID="btnUpload" runat="server" CssClass="css" Text="�Ѵ��" 
                                    Width="70px" onclick="btnUpload_Click"/>
                            </td>
                        </tr>
                        <tr height="200px">
                            <td class="choice" >�ٻ���·��ѹ�֡���<br />(150x200 JPG 10K):</td>
                            <td class="choice" >
                                <asp:Image runat="server" ID="imgLink" Width="150px" Height="200px" 
                                    ImageAlign="Middle" ImageUrl="CIMG/blank.gif"  />
                            </td>
                        </tr>
                        <tr height="20px">
                            <td class="choice">�ԡѴ Google:</td>
                            <td class="choice">
                                <asp:Textbox ID="txtGoogle" runat="server" CssClass="css" Width="250px" MaxLength="25"/>                            </td>
                        </tr>
                        <tr height="240px">
                            <td class="choice" colspan="2" align="center">
                                <div id="map_canvas" style="width:100%; height:100%"></div>
                            </td>
                        </tr>
                        <tr height="20px">
                            <td class="choice" colspan="2" align="right"><br />
                            <asp:HiddenField ID="idID" Value="" runat="server" />
                            <asp:HiddenField ID="hidID" Value="" runat="server" />
                              <asp:HiddenField ID="maxi" Value="" runat="server" />
                                <asp:Label ID="lblResponse" runat="server" CssClass="css" Width="95%"/>
                                <asp:Button ID="btnSave" runat="server" CssClass="css" Text="��Ѻ��ا" 
                                    Width="70px" onclick="btnSave_Click" />
                                <asp:Button ID="btnDel" runat="server" CssClass="css" Text="ź������" 
                                    Width="70px" onclick="btnDel_Click" />
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