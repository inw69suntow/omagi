<%@ Page Language="C#" AutoEventWireup="true" CodeFile="G1.aspx.cs" Inherits="_G1" ViewStateEncryptionMode="Always"%>
            <asp:Chart ID="mainChart" runat="server" Height="250px" Width="750px" BorderlineColor="Black" ImageType="Png" BorderlineDashStyle="Solid" BackColor="White" BorderWidth="2" BorderColor="26, 59, 105" rendertype="BinaryStreaming" >
                <chartareas>
                    <asp:ChartArea Name="mainArea" BorderColor="64, 64, 64, 64" BorderDashStyle="Solid" BackColor="White" ShadowColor="Transparent">
                        <axisy linecolor="64, 64, 64, 64">
                            <labelstyle font="Tahoma, 10pt, style=Bold" />
                            <majorgrid linecolor="64, 64, 64, 64" />
                        </axisy>
                        <axisx linecolor="64, 64, 64, 64">
                            <labelstyle font="Tahoma, 10pt, style=Bold" />
                            <majorgrid linecolor="64, 64, 64, 64" />
                        </axisx>
                        <axisy2 linecolor="64, 64, 64, 64">
                            <labelstyle font="Tahoma, 10pt, style=Bold" />
                            <majorgrid linecolor="64, 64, 64, 64" />
                        </axisy2>
                        <axisx2 linecolor="64, 64, 64, 64">
                            <labelstyle font="Tahoma, 10pt, style=Bold" />
                            <majorgrid linecolor="64, 64, 64, 64" />
                        </axisx2>
                    </asp:ChartArea>
                </chartareas>
            </asp:Chart>                