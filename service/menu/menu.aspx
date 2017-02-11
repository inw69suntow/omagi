<%@ Page Language="C#" AutoEventWireup="true" CodeFile="menu.aspx.cs" Inherits="menu_menu" %>

 <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <meta name="viewport" content="width=device-width, initial-scale=1">
       <script src='<%=Page.ResolveClientUrl("~/resources/js/common/jquery.js")%>' type="text/javascript"></script>
       <link rel="stylesheet" type="text/css" href="<%=Page.ResolveClientUrl("~/resources/js/common/jquery-ui.css")%>"/>
       <script src='<%=Page.ResolveClientUrl("~/resources/js/common/jquery-ui.js")%>' type="text/javascript"></script>
   
    <link rel="stylesheet" type="text/css" href="<%=Page.ResolveClientUrl("~/resources/megamenu-js-master/css/style.css")%>"/>
    <link rel="stylesheet" type="text/css" href="<%=Page.ResolveClientUrl("~/resources/megamenu-js-master/css/ionicons.min.css")%>"/>


</head>
<body>

<div class="menu-container">
        <div class="menu">
            <ul>
                <li><a href="#">บันทึกข้อมูล</a>
                    <ul>
                        <li><a href="#"></a>
                            <ul>
                                <li><a href="#">สมาชิก</a></li>
                                <li><a href="#">กลุ่มมวลชน</a></li>
                            </ul>
                        </li>
                         <li><a href="#"></a>
                            <ul>
                                <li><a href="#">โครงการ</a></li>
                                <li><a href="#">หลักสูตร</a></li>
                                <li><a href="#">การฝึกอบรม</a></li>
                            </ul>
                        </li>
                    </ul>
                </li>
                 <li><a href="#">ค้นหา</a>
                    <ul>
                        <li><a href="#">สมาชิก</a></li>
                        <li><a href="#">กลุ่มมวลชน</a></li>
                        <li><a href="#">หลักสูตร</a></li>
                        <li><a href="#">โครงการ</a></li>
                        <li><a href="#">การฝึกอบรม</a></li>
                    </ul>
                </li>

                <li><a href="http://marioloncarek.com">About</a>
                    <ul>
                        <li><a href="http://marioloncarek.com">School</a>
                            <ul>
                                <li><a href="http://marioloncarek.com">Lidership</a></li>
                                <li><a href="#">History</a></li>
                                <li><a href="#">Locations</a></li>
                                <li><a href="#">Careers</a></li>
                            </ul>
                        </li>


                        <li><a href="#">Study</a>
                            <ul>
                                <li><a href="#">Undergraduate</a></li>
                                <li><a href="#">Masters</a></li>
                                <li><a href="#">International</a></li>
                                <li><a href="#">Online</a></li>
                            </ul>
                        </li>
                        <li><a href="#">Research</a>
                            <ul>
                                <li><a href="#">Undergraduate research</a></li>
                                <li><a href="#">Masters research</a></li>
                                <li><a href="#">Funding</a></li>
                            </ul>
                        </li>
                        <li><a href="#">Something</a>
                            <ul>
                                <li><a href="#">Sub something</a></li>
                                <li><a href="#">Sub something</a></li>
                                <li><a href="#">Sub something</a></li>
                                <li><a href="#">Sub something</a></li>
                            </ul>
                        </li>
                    </ul>
                </li>
                <li><a href="http://marioloncarek.com">News</a>
                    <ul>
                        <li><a href="http://marioloncarek.com">Today</a></li>
                        <li><a href="#">Calendar</a></li>
                        <li><a href="#">Sport</a></li>
                    </ul>
                </li>
                <li><a href="http://marioloncarek.com">Contact</a>
                    <ul>
                        <li><a href="#">School</a>
                            <ul>
                                <li><a href="#">Lidership</a></li>
                                <li><a href="#">History</a></li>
                                <li><a href="#">Locations</a></li>
                                <li><a href="#">Careers</a></li>
                            </ul>
                        </li>
                        <li><a href="#">Study</a>
                            <ul>
                                <li><a href="#">Undergraduate</a></li>
                                <li><a href="#">Masters</a></li>
                                <li><a href="#">International</a></li>
                                <li><a href="#">Online</a></li>
                            </ul>
                        </li>
                        <li><a href="#">Study</a>
                            <ul>
                                <li><a href="#">Undergraduate</a></li>
                                <li><a href="#">Masters</a></li>
                                <li><a href="#">International</a></li>
                                <li><a href="#">Online</a></li>
                            </ul>
                        </li>
                        <li><a href="#">Empty sub</a></li>
                    </ul>
                </li>
            </ul>
        </div>
    </div>
    <script type="text/javascript" src="<%=Page.ResolveClientUrl("~/resources/megamenu-js-master/js/megamenu.js")%>"></script>
</body>
</html>
