<%@ OutputCache Location="None" VaryByParam="none" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="member_entry.aspx.cs" Inherits="_train_member_entry" MasterPageFile="~/MasterPage.master" ViewStateEncryptionMode="Always"%>
<asp:Content ID="content2" ContentPlaceHolderID="head" runat="server">
    <meta name="viewport" content="initial-scale=1.0, user-scalable=no" />
    <style type="text/css">
      #map_canvas { height: 100% }
        .css
        {}
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
                title: 'ที่นี่'
            });
        }

    </script>
</asp:Content>
<asp:Content ID="content" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<script type="text/javascript">


    $(function () {
        onloadSearchBox();
    });

    function onloadSearchBox() {
        var txtSearchSID = '<%= txtSearchSID.ClientID %>';
        var txtSearchFName = '<%= txtSearchFName.ClientID %>';
        var txtSearchLName = '<%= txtSearchLName.ClientID %>';
        var btnSearch = '<%= btnSearch.ClientID %>';
        document.getElementById(txtSearchSID).addEventListener("keydown", function (e) {
            if (!e) { var e = window.event; }
            if (e.keyCode == 13) {
                document.getElementById(btnSearch).click();
            }
        }, false);

        document.getElementById(txtSearchFName).addEventListener("keydown", function (e) {
            if (!e) { var e = window.event; }
            if (e.keyCode == 13) {
                document.getElementById(btnSearch).click();
            }
        }, false);


        document.getElementById(txtSearchLName).addEventListener("keydown", function (e) {
            if (!e) { var e = window.event; }
            if (e.keyCode == 13) {
                document.getElementById(btnSearch).click();
            }
        }, false);

    }

    function openMember(action, checkbox) {
        if (checkbox.checked) {
            document.location = "train_member_entry.aspx?hid=" + action;
        }
    }

    function sortColumn(title) {
        var hdSortName = '<%= hdSortName.ClientID %>';
        var btnSearch = '<%= btnSearch.ClientID %>';
        document.getElementById(hdSortName).value = title;
        document.getElementById(btnSearch).click();
    }

    function chkClick() {
        if (confirm('ยืนยันลบขู้มูล ?')) {
            var hdDelAll = '<%= hdDelAll.ClientID %>';
            var sidList = "";
            var count = 0;
            $('input[id^="check_"]').each(function (i) {
                //child_
                if ($(this).is(":checked")) {
                    var sid = $('#hd_' + $(this).prop('id') + '').val();
                    if (count == 0) {
                        sidList += "'" + sid + "'";
                    } else {
                        sidList += ",'" + sid + "'";
                    }
                    count++;
                }
            });
            $('#' + hdDelAll).val(sidList);
            return true;
        } else {
            return false;
        }
    }
</script>

    <script language="vbscript">
sub prepareText()    
    fileName=document.getElementById("exclFile").value
    if Trim(fileName)<>"" then
        Set Excl = CreateObject("excel.application")
	    Excl.Workbooks.Open fileName
	    Set WB = Excl.Activeworkbook
        Set WS = WB.Worksheets(1)

        document.getElementById("ctl00_ContentPlaceHolder1_importText").value="";
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
    <asp:HiddenField ID="hdSortName" runat="server" />
<table width="750px" border="0" align="center" cellspacing="0" cellpadding="0" class="css">
  <tr>
    <td style="height:25px; width: 750px;" class="header_1">ข้อมูลสมาชิก</td>
  </tr>
</table>

<table width="750px" align="center" valign="center" border="0" cellspacing="0" cellpadding="0" class="css">
  <tr><td height="10px"></td></tr>
    <tr>
    <td>
        <table width="750px" border="0" cellspacing="0" cellpadding="0" class="css">
            <tr>
                <td width="6px" style="height:23px;" background="photo/box_topleft.gif"></td>   
                <th style="height:20px; width:700px;"  background="photo/box_topbg.gif">
                    <div align="left"  valign="middle"><b>รายนามสมาชิก</b></div>
                </th>
                <td width="6px" style="height:23px;" background="photo/box_topright.gif"></td>   
                 <th style="height:20px; width:178px;"  background="photo/box_topbg.gif" >
                    หน้าที่ <asp:DropDownList ID="pageID" CssClass="css" 
                        runat="server" AutoPostBack="true" 
                        onselectedindexchanged="pageID_SelectedIndexChanged" />
                    <asp:Button ID="btnPrev" runat="server" CssClass="css" Width="18px" 
                        Text="&lt;" OnClick="btnPrev_Click" Visible="false" />
                    <asp:Button ID="btnNext" runat="server" CssClass="css" Width="18px" 
                        Text="&gt;" OnClick="btnNext_Click" Visible="false"/>
                </th>
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
            <tr align="right"><td>
                 <asp:Button ID="btnDelAll_client" runat="server" OnClientClick="return chkClick()" 
                     Text="ลบข้อมูลที่เลือก" onclick="btnDelAll_client_Click" />
        
                <asp:HiddenField ID="hdDelAll" runat="server" />
            </td></tr>
       </table>
    </td>
  </tr>
    <td>
        <table width="750px" border="0" cellspacing="0" cellpadding="0" class="css">
            <tr>
                <td width="6px" style="height:23px;" background="photo/box_topleft.gif"></td>   
                <th style="height:20px; width:738px;"  background="photo/box_topbg.gif"><div align="left"><b>
                    แบบค้นหาข้อมูล</b></div></th>
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
                            <td class="choice" style="width: 200px">เลขบัตรประจำตัวประชาชน:</td>
                            <td class="choice">
                               
                                <asp:TextBox ID="txtSearchSID" runat="server"></asp:TextBox>                              
                                
                               <asp:RegularExpressionValidator ID="rgtxtSearchSID"  ControlToValidate="txtSearchSID" runat="server"
                                    ErrorMessage="ต้องเป็นตัวเลขเท่านั้น"
                                    ValidationExpression="\d+">
                                </asp:RegularExpressionValidator>
                            </td>
                        </tr>
                        <tr height="20px">
                            <td class="choice" >ชื่อ:</td>
                            <td class="choice" >
                                <asp:TextBox ID="txtSearchFName" runat="server" ></asp:TextBox>
                            </td>
                        </tr>
                        <tr height="20px">
                            <td class="choice" >นามสกุล:</td>
                            <td class="choice" >
                                <asp:TextBox ID="txtSearchLName" runat="server"></asp:TextBox>                      
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
                    แบบบันทึกข้อมูล</b></div></th>
                <th style="height:20px; width:500px;"  background="photo/box_topbg.gif">
		<div align="right">
                                <%--<input type="file" id="exclFile" class="css"/>
                                <asp:Button ID="btnImport" runat="server" CssClass="css" Text="นำเข้า" 
                                    Width="70px" onclick="btnImport_Click" OnClientClick="prepareText()"/>--%>
                                <asp:HiddenField ID="importText" runat="server" value=""/>
	                            <asp:Button ID="btnClear" Cssclass="css" runat="server" OnClick="btnClear_Click" Text="สร้างใหม่"  Width="70px"/>
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
                            <td class="choice" >บัตรประชาชน:</td>
                            <td class="choice" >
                                <asp:TextBox ID="txtCardID" runat="server" width="200px" MaxLength="500" 
                                    CssClass="css" AutoPostBack="true" ontextchanged="txtCardID_TextChanged"/>
                                <font color="red">*</font>
                                <asp:RegularExpressionValidator ID="retxtCardID"  ControlToValidate="txtCardID" runat="server"
                                    ErrorMessage="ต้องเป็นตัวเลขเท่านั้น"
                                    ValidationExpression="\d+">
                                </asp:RegularExpressionValidator>
                            </td>
                        </tr>
                        <tr height="20px">
                            <td class="choice">ชื่อ:</td>
                            <td class="choice">
                                <asp:DropDownList ID="cmbTitle" runat="server" CssClass="css"></asp:DropDownList>
                                <asp:Textbox ID="txtFName" runat="server" CssClass="css" Width="200" MaxLength="250" />
                                <asp:Textbox ID="txtSName" runat="server" CssClass="css" Width="200" MaxLength="250" />
                                <font color="red">*</font>
                            </td>
                        </tr>
                        <tr height="20px">
                            <td class="choice">บ้านเลขที่:</td>
                            <td class="choice">
                                <asp:Textbox ID="txtAddrNo" runat="server" CssClass="css" Width="100" MaxLength="250" />
                            </td>
                        </tr>
                        <tr height="20px">
                            <td class="choice">หมู่บ้าน/อาคาร:</td>
                            <td class="choice">
                                <asp:Textbox ID="txtHome" runat="server" CssClass="css" Width="200" MaxLength="250" />
                            </td>
                        </tr>
                        <tr height="20px">
                            <td class="choice">ซอย:</td>
                            <td class="choice">
                                <asp:Textbox ID="txtSoy" runat="server" CssClass="css" Width="200" MaxLength="250" />
                            </td>
                        </tr>
                        <tr height="20px">
                            <td class="choice">ถนน:</td>
                            <td class="choice">
                                <asp:Textbox ID="txtRoad" runat="server" CssClass="css" Width="200" MaxLength="250" />
                            </td>
                        </tr>
                        <tr height="20px">
                            <td class="choice">จังหวัด:</td>
                            <td class="choice">
                                <asp:DropDownList ID="cmbProvince" runat="server" CssClass="css" 
                                    AutoPostBack="true" onselectedindexchanged="cmbProvince_SelectedIndexChanged"></asp:DropDownList>                            </td>
                        </tr>
                        <tr height="20px">
                            <td class="choice">อำเภอ/เขต:</td>
                            <td class="choice">
                                <asp:DropDownList ID="cmbDistrict" runat="server" CssClass="css" 
                                    AutoPostBack="true" onselectedindexchanged="cmbDistrict_SelectedIndexChanged"></asp:DropDownList>                            </td>
                        </tr>
                        <tr height="20px">
                            <td class="choice">ตำบล/แขวง:</td>
                            <td class="choice">
                                <asp:DropDownList ID="cmbTambon" runat="server" CssClass="css"></asp:DropDownList>                            </td>
                        </tr>
                        <tr height="20px">
                            <td class="choice">หมู่ที่:</td>
                            <td class="choice">
                                <asp:Textbox ID="txtMoo" runat="server" CssClass="css" Width="30px" MaxLength="2"/> 
                                <asp:RegularExpressionValidator ID="rgtxtMoo"  ControlToValidate="txtMoo" runat="server"
                                    ErrorMessage="ต้องเป็นตัวเลขเท่านั้น"
                                    ValidationExpression="\d+">
                                </asp:RegularExpressionValidator>                           </td>
                        </tr>
                       <tr height="20px">
                            <td class="choice">รหัสไปรษณีย์:</td>
                            <td class="choice">
                                <asp:Textbox ID="txtPost" runat="server" CssClass="css" Width="100" MaxLength="5" />
                                <asp:RegularExpressionValidator ID="rgtxtPost"  ControlToValidate="txtPost" runat="server"
                                    ErrorMessage="ต้องเป็นตัวเลขเท่านั้น"
                                    ValidationExpression="\d+">
                                </asp:RegularExpressionValidator>  
                            </td>
                        </tr>
                        <tr height="20px">
                            <td class="choice" >โทรศัพท์บ้าน:</td>
                            <td class="choice" >
                                <asp:Textbox ID="txtTelNo" runat="server" CssClass="css" Width="100" MaxLength="10" />
                                <asp:RegularExpressionValidator ID="rgtxtTelNo"  ControlToValidate="txtTelNo" runat="server"
                                    ErrorMessage="ต้องเป็นตัวเลขเท่านั้น"
                                    ValidationExpression="\d+">
                                </asp:RegularExpressionValidator> 
                            </td>
                        </tr>
                       <tr height="20px">
                            <td class="choice">โทรศัพท์ที่ทำงาน:</td>
                            <td class="choice">
                                <asp:Textbox ID="txtOffNo" runat="server" CssClass="css" Width="100" MaxLength="10" />
                                <asp:RegularExpressionValidator ID="rgtxtOffNo"  ControlToValidate="txtOffNo" runat="server"
                                    ErrorMessage="ต้องเป็นตัวเลขเท่านั้น"
                                    ValidationExpression="\d+">
                                </asp:RegularExpressionValidator> 
                            </td>
                        </tr>
                        <tr height="20px">
                            <td class="choice" >โทรศัพท์มือถือ:</td>
                            <td class="choice" >
                                <asp:Textbox ID="txtMobNo" runat="server" CssClass="css" Width="100" MaxLength="10" />
                                <asp:RegularExpressionValidator ID="rgtxtMobNo"  ControlToValidate="txtMobNo" runat="server"
                                    ErrorMessage="ต้องเป็นตัวเลขเท่านั้น"
                                    ValidationExpression="\d+">
                                </asp:RegularExpressionValidator>
                            </td>
                        </tr>
                       <tr height="20px">
                            <td class="choice">อีเมล์:</td>
                            <td class="choice">
                                <asp:Textbox ID="txtEmail" runat="server" CssClass="css" Width="200" MaxLength="250" />
                            </td>
                        </tr>
                        <tr height="20px">
                            <td class="choice" >วันเกิด:</td>
                            <td class="choice" >
                                <asp:DropDownList ID="cmbBDD" runat="server" CssClass="css"></asp:DropDownList> -
                                <asp:DropDownList ID="cmbBMM" runat="server" CssClass="css"></asp:DropDownList> -
                                <asp:DropDownList ID="cmbBYY" runat="server" CssClass="css"></asp:DropDownList>
                            </td>
                        </tr>
                       <tr height="20px">
                            <td class="choice" ></td>
                            <td class="choice">
                                <asp:CheckBox ID="chkTargetFlag" runat="server" CssClass="css" Text="ยินดีเข้าร่วมกิจกรรมทุกครั้งที่ กอรมน. ร้องขอ" />
                            </td>                            
                        </tr>
                        <tr height="20px">
                            <td class="choice" ></td>
                            <td class="choice" >
                                <asp:CheckBox ID="chkStatus" runat="server" CssClass="css" Text="ยกเลิก/เสียชีวิต" />
                            </td>
                        </tr>
                        <tr height="20px">
                            <td class="choice" >รูปถ่ายใหม่:</td>
                            <td class="choice" >
                                <asp:FileUpload id="imgFile" cssclass="css" runat="server"/>
                                <asp:Button ID="btnUpload" runat="server" CssClass="css" Text="จัดเก็บ" 
                                    Width="70px" onclick="btnUpload_Click"/>
                            </td>
                        </tr>

                          <tr height="20px">
                            <td class="choice" >ระดับการศึกษา:</td>
                            <td class="choice" >
                                  <asp:Textbox ID="txtEducational" runat="server" CssClass="css" Width="200" MaxLength="250" />
                            </td>
                        </tr>
                           <tr height="20px">
                            <td class="choice" >สถาบันการศึกษา:</td>
                            <td class="choice" >
                                  <asp:Textbox ID="txtAcademy" runat="server" CssClass="css" Width="200" MaxLength="250" />
                            </td>
                        </tr>
                          <tr height="20px">
                            <td class="choice" >ความสามารถพิเศษ:</td>
                            <td class="choice" >
                                  <asp:Textbox ID="txtTalent" runat="server" CssClass="css" Width="200" MaxLength="250" />
                            </td>
                        </tr>

                           <tr height="20px">
                            <td class="choice" >งาน:</td>
                            <td class="choice" >
                                  <asp:Textbox ID="txtJob" runat="server" CssClass="css" Width="200" MaxLength="250" />
                            </td>
                        </tr>
                          <tr height="20px">
                            <td class="choice" >ตำแหน่ง:</td>
                            <td class="choice" >
                                  <asp:Textbox ID="txtPosition" runat="server" CssClass="css" Width="200" MaxLength="250" />
                            </td>
                        </tr>
                        <tr height="200px">
                            <td class="choice" >ที่อยู่ปัจจุบัน:</td>
                            <td class="choice" >
                                  <asp:TextBox ID="txtCurAddr"  runat="server" CssClass="css" Width="500px" 
                                      MaxLength="250" Height="200px" TextMode="MultiLine" />
                            </td>
                        </tr>


                        <tr height="200px">
                            <td class="choice" >รูปถ่ายที่บันทึกไว้<br />(150x200 JPG 10K):</td>
                            <td class="choice" >
                                <asp:Image runat="server" ID="imgLink" Width="150px" Height="200px" 
                                    ImageAlign="Middle" ImageUrl="CIMG/blank.gif"  />
                            </td>
                        </tr>
                        <tr height="20px">
                            <td class="choice">พิกัด Google:</td>
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
                                <asp:Button ID="btnSave" runat="server" CssClass="css" Text="ปรับปรุง" 
                                    Width="70px" onclick="btnSave_Click" />
                                <asp:Button ID="btnDel" runat="server" CssClass="css" Text="ลบข้อมูล" 
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