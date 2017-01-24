<%@ Page Language="C#" AutoEventWireup="true" CodeFile="man_REP2.aspx.cs" Inherits="_man_REP2" MasterPageFile="~/MasterPage.master" ViewStateEncryptionMode="Always"%>
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
                title: 'ที่นี่'
            });
        }
    </script>
</asp:Content>
<asp:Content ID="content" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<script>    //initialize();</script>
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
                            <asp:TextBox ID="txtHome" runat="server" CssClass="css" MaxLength="10"/>
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
</table>

<table  id="bar" runat="server"  visible="true" width="750px" align="center" valign="center" border="0" cellspacing="0" cellpadding="0" class="css">
  <tr>
    <td>
    <table width="750px" border="0" cellspacing="0" cellpadding="0" class="css">
      <tr>
        <td width="6px" style="height:23px;" background="photo/box_topleft.gif"></td>   
        <th style="height:23px; width:988px;" background="photo/box_topbg.gif"><div align="left">ประวัติสมาชิก</div></th>
        <td width="6px" style="height:23px;" background="photo/box_topright.gif"></td>   
      </tr>
    </table>
    </td>
  </tr>
  <tr>
    <td align="center" valign="top"> 
    <table id="tab" border="1" cellspacing="0" cellpadding="0" class="css" style="width: 750px; border-right: #f6a836; border-top: #f6a836; border-left: #f6a836; border-bottom: #f6a836;" runat="server" rules="all" >
        <tr>
            <td align="center" Class="header_1" colspan="4">
                <asp:Label ID="lblName" runat="server" Text=""/>
            </td>
        </tr>
        <tr>
            <td align="center" Class="header" rowspan="7">
                <asp:Image ID="imgName" runat="server" Width="150px" Height="200px" 
                                    ImageAlign="Middle" ImageUrl="CIMG/blank.gif" />
            </td>
            <td style="width:100px;height:20px" class="header_2">
                ที่อยู่:
            </td>            
            <td style="width:400px; height:50px">
                <asp:Label ID="lblAddress" runat="server" Text="" CssClass="css" />
            </td>            
            <td rowspan="7">
                <asp:HiddenField ID="txtGoogle" runat="server" />
                <div id="map_canvas" style="width:320; height:160"></div>
            </td>            
        </tr>
        <tr>
            <td style="width:100px;height:20px" class="header_2">
                โทรศัพท์บ้าน:
            </td>            
            <td style="width:400px;height:20px">
                <asp:Label ID="lblTelNo" runat="server" Text="" CssClass="css" />
            </td>            
        </tr>
        <tr>
            <td style="width:100px;height:20px" class="header_2">
                โทรศัพท์ที่ทำงาน:
            </td>            
            <td style="width:400px;height:20px">
                <asp:Label ID="lblOffNo" runat="server" Text="" CssClass="css" />
            </td>            
        </tr>
        <tr>
            <td style="width:100px;height:20px" class="header_2">
                โทรศัพท์มือถือ:
            </td>            
            <td style="width:400px;height:20px">
                <asp:Label ID="lblMobNo" runat="server" Text="" CssClass="css" />
            </td>            
        </tr>
        <tr>
            <td style="width:100px;height:20px" class="header_2">
                อีเมล์:
            </td>            
            <td style="width:400px;height:20px">
                <asp:Label ID="lblEmail" runat="server" Text="" CssClass="css" />
            </td>            
        </tr>
        <tr>
            <td style="width:100px;height:20px" class="header_2">
                วันเกิด:
            </td>            
            <td style="width:400px;height:20px">
                <asp:Label ID="lblBirth" runat="server" Text="" CssClass="css" />
            </td>            
        </tr>
        <tr>
            <td colspan="2" align="center">
                <asp:Label ID="lblTarget" runat="server" Text="" CssClass="css" />
            </td>            
        </tr>
    </table>
    </td>
  </tr>
  <tr><td height="20px"></td></tr>
</table>
<table  id="bar2" runat="server" width="750px" align="center" valign="center" border="0" cellspacing="0" cellpadding="0" class="css">
  <tr>
    <td>
    <table width="750px" border="0" cellspacing="0" cellpadding="0" class="css">
      <tr>
        <td width="6px" style="height:23px;" background="photo/box_topleft.gif"></td>   
        <th style="height:23px; width:738px;" background="photo/box_topbg.gif"><div align="left">รายนามมวลชนที่สังกัด</div></th>
        <td width="6px" style="height:23px;" background="photo/box_topright.gif"></td>   
      </tr>
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
<table  id="bar3" runat="server" width="750px" align="center" valign="center" border="0" cellspacing="0" cellpadding="0" class="css">
  <tr>
    <td>
    <table width="750px" border="0" cellspacing="0" cellpadding="0" class="css">
      <tr>
        <td width="6px" style="height:23px;" background="photo/box_topleft.gif"></td>   
        <th style="height:23px; width:738px;" background="photo/box_topbg.gif"><div align="left">ประวัติการฝึกอบรม</div></th>
        <td width="6px" style="height:23px;" background="photo/box_topright.gif"></td>   
      </tr>
    </table>
    </td>
  </tr>
  <tr>
    <td align="center" valign="top"> 
    <table id="tab2" border="1" cellspacing="0" cellpadding="0" class="css" style="width: 750px; border-right: #f6a836; border-top: #f6a836; border-left: #f6a836; border-bottom: #f6a836;" runat="server" rules="all" ></table>
    </td>
  </tr>
  <tr><td height="20px"></td></tr>
</table>
</asp:Content>