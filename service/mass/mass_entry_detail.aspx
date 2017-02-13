<%@ OutputCache Location="None" VaryByParam="none" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="mass_entry_detail.aspx.cs" Inherits="_mass_entry" ViewStateEncryptionMode="Always"%>
<html>
    <head>
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
                title: 'ที่นี่'
            });
        }
    </script>
</head>
<body>

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
 <form id="form1" runat="server">
<table border="0" align="center" cellspacing="0" cellpadding="0" class="css">
  <tr>
    <td style="height:25px; width: 750px;" class="header_1">ข้อมูลกลุ่มมวลชน</td>
  </tr>
</table>

<table  align="center" valign="center" border="0" cellspacing="0" cellpadding="0" class="css">
  <tr>
    <td align="center" valign="top">
                <table width="750px" border="1" style="border:1px solid;" cellspacing="0" cellpadding="0">
            <tr>
                <td>
                    <table border="0" align="center" width="100%" cellspacing="0" cellpadding="0">            
                        <tr >
                            <td class="choice" style="width: 200px">ประเภทกลุ่มมวลชน:</td>
                            <td class="choice" >
                                <asp:DropDownList ID="cmbMassGroup" runat="server" CssClass="css"></asp:DropDownList>
                            <font color="red">*</font>
                            </td>
                        </tr>
                        <tr >
                            <td class="choice" >ชื่อกลุ่มมวลชน:</td>
                            <td class="choice" >
                                <asp:TextBox ID="txtMassName" runat="server" width="200px" MaxLength="500" CssClass="css"/>
                            </td>
                        </tr>
                        <tr >
                            <td class="choice" >จำนวนคนตามรายงาน:</td>
                            <td class="choice" >
                                <asp:Textbox ID="txtReportAmount" runat="server" CssClass="css" Width="100" MaxLength="10" /> คน
                            </td>
                        </tr>
                        <tr >
                            <td class="choice">หน่วยงานผู้ดูแล:</td>
                            <td class="choice">
                                <asp:DropDownList ID="cmbDept" runat="server" CssClass="css"></asp:DropDownList>
                                <font color="red">*</font>
                           </td>
                        </tr>
                        <tr >
                            <td class="choice">พื้นที่ปฏิบัติการจังหวัด:</td>
                            <td class="choice">
                                <asp:DropDownList ID="cmbProvince" runat="server" CssClass="css" 
                                    AutoPostBack="true" onselectedindexchanged="cmbProvince_SelectedIndexChanged"></asp:DropDownList>
                                <font color="red">*</font>
                            </td>
                        </tr>
                        <tr >
                            <td class="choice">อำเภอ/เขต:</td>
                            <td class="choice">
                                <asp:DropDownList ID="cmbDistrict" runat="server" CssClass="css" 
                                    AutoPostBack="true" onselectedindexchanged="cmbDistrict_SelectedIndexChanged"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr >
                            <td class="choice">ตำบล/แขวง:</td>
                            <td class="choice">
                                <asp:DropDownList ID="cmbTambon" runat="server" CssClass="css"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr >
                            <td class="choice">หมู่ที่:</td>
                            <td class="choice">
                                <asp:Textbox ID="txtMoo" runat="server" CssClass="css" Width="30px" MaxLength="2"/>
                            </td>
                        </tr>
                       <tr  valign="top">
                            <td class="choice">พิกัด Google:</td>
                            <td class="choice">
                                <asp:Textbox ID="txtGoogle" runat="server" CssClass="css" Width="250px" MaxLength="25"/>
                            </td>
                        </tr>
                        <tr height="240px">
                            <td class="choice" colspan="2" align="center">
                                <div id="map_canvas" style="width:100%; height:100%"></div>
                            </td>
                        </tr>
                       <tr  valign="bottom">
                            <td class="choice" colspan="2" align="right"><br />
                            <asp:HiddenField ID="hidID" Value="" runat="server" />
                              <asp:HiddenField ID="maxi" Value="" runat="server" />
                                <asp:Label ID="lblResponse" runat="server" CssClass="css" Width="95%"/>
                                <asp:Button ID="btnSave" runat="server" CssClass="css" Text="ปรับปรุง" 
                                    Width="70px" OnClick="btnSave_Click" />
                                <asp:Button ID="btnMember" CssClass="css" runat="server" Text="สมาชิก" 
                                    Width="70px" OnClick="btnMember_Click" />
                                <asp:Button ID="btnDel" runat="server" CssClass="css" Text="ลบข้อมูล" 
                                    Width="70px" OnClick="btnDel_Click"  />
                            </td>
                        </tr>
                        <tr >
                            <td class="choice">Create:</td>
                            <td class="choice">
                                <asp:Label ID= "lblCreate" runat="server" CssClass="css" Text="" />
                            </td>
                        </tr>
                        <tr >
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
</form>
</body>
</html>