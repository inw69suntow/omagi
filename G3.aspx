<%@ Page Language="C#" AutoEventWireup="true" CodeFile="G3.aspx.cs" Inherits="_G3" ViewStateEncryptionMode="Always"%>
            <asp:Chart ID="sub2" runat="server" Height="250px" Width="375px" BorderlineColor="Black"  ImageType="Png" BorderlineDashStyle="Solid" BackColor="White" BorderWidth="2" BorderColor="26, 59, 105" rendertype="BinaryStreaming">
                <chartareas>
                    <asp:ChartArea Name="mainArea" BorderColor="64, 64, 64, 64" BorderDashStyle="Solid" BackColor="White" ShadowColor="Transparent">
                        <area3dstyle Rotation="30" perspective="50" Inclination="50" IsRightAngleAxes="False" wallwidth="0" IsClustered="False"></area3dstyle>
                    </asp:ChartArea>
                </chartareas>
            </asp:Chart>                
