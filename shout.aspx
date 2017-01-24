<%@ OutputCache Location="None" VaryByParam="none" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="shout.aspx.cs" Inherits="shout" ViewStateEncryptionMode="Always"%>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="Stylesheet" href="css.css" />
    <style type="text/css">
        #form1
        {
            height: 205px;
            width: 240px;
            background:#fec24a;
        }
        body
        {
            font-size:x-small;
            color:ButtonText;
        }
        #divFilUpload 
        {
            border:1px solid green; 
            border-collapse:collapse; 
            background:#fec24a; 
            width:inherit; 
            height:48px;  
            position:relative;
            bottom:0px;
        }
        #outputBox
        {
            text-align:left;
            color:black;
            border:1px solid black; 
            border-collapse:collapse; 
            background:white; 
            width: 240px ;
            height:160px;  
            position:relative;
            overflow:auto;
        }
    </style>
</head>
<body background-color:#fec24a" style="width:100%;height:250px">
<left>
<form id="form1" runat="server">
<div style="width:100%;background-color:#fec24a">
<table style="background-color:#fec24a">
<tr>
<td>
    <div id="outputBox">
        <asp:Label ID="lblShoutM" runat="server"  
            style="margin-left: 0px; text-align:left; max-width:210px; Width:210px;" 
            Font-Size="12px" Font-Bold="True"/>
    </div>
</td>
</tr>
<tr>

<td>
    <div class="divFilUpload">
    <table width="100%">
        <tr>
            <td>
                <asp:FileUpload runat="server" ID="fuShout" Height="20px" 
                    Width="100%"/>
            </td>
        </tr>
        <tr>
            <td>
                <asp:TextBox ID="txtShoutS"  
                    runat="server" Width="85%" MaxLength="127" cssClass="css" Height="20px" Wrap="False">
                </asp:TextBox>
                <asp:Button runat="server" ID="btnUp" Text="ส่ง" onclick="btnUP_Click" 
                    Height="20px"/>
            </td>
        </tr>
    </table>
    </div>
</td>
</tr>
</table>
</div>
</form>
</left>
</body>
</html>
<script type="text/javascript">
    function rerun() {
        document.location = "shout.aspx"; 
    }
    var x = Math.random() * 10000;
    setTimeout("rerun()", 30000 + x);
</script>
