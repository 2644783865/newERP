<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Org.aspx.cs" Inherits="Infobasis.Web.Pages.Admin.Org"
    MasterPageFile="~/PageMaster/Page.master" %>

<asp:Content ID="HeadContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="MainContent" runat="server" ContentPlaceHolderID="MainContent">
    <f:PageManager ID="PageManager1" AutoSizePanelID="Panel1" runat="server"></f:PageManager>
    <f:Panel ID="Panel1" Layout="Region" ShowBorder="false" ShowHeader="false" runat="server" BodyPadding="10px">
        <Items>
            <f:TabStrip ID="TabStrip1" TabPlain="true" TabBorderColor="true" CssClass="f-tabstrip-theme-simple" Height="150px"
                ShowBorder="true" TabPosition="Top" MarginBottom="10px"
                EnableTabCloseMenu="false" ActiveTabIndex="0" runat="server">
                <Tabs>
                    <f:Tab ID="Tab1" Title="列表" BodyPadding="5px" AutoScroll="true" IconFont="Home"
                        runat="server">
                        <Content>
                            <ol class="howtouse">
                                <li>从官网示例源代码中拷贝 /res/themes/bootstrap_pure/ 目录到本地项目中的相同位置；</li>
                                <li>Web.config中增加配置项：CustomTheme="bootstrap_pure"；<pre>&lt;FineUIPro DebugMode="false" CustomTheme="bootstrap_pure" /&gt;</pre>
                                </li>
                                <li>完成。
                                </li>
                            </ol>
                        </Content>
                    </f:Tab>
                    <f:Tab ID="Tab2" Title="图表<span class='badge badge-danger'>6</span>" BodyPadding="5px"
                        runat="server">
                        <Items>
                            <f:Label ID="Label2" Text="第二个标签的内容" runat="server" />
                        </Items>
                    </f:Tab>
                    <f:Tab ID="Tab3" Title="标签三" BodyPadding="5px" runat="server">
                        <Items>
                            <f:Label ID="Label3" Text="第三个标签的内容" runat="server" />
                        </Items>
                    </f:Tab>
                </Tabs>
            </f:TabStrip>
        </Items>
    </f:Panel>
</asp:Content>

<asp:Content ID="ScriptContent" runat="server" ContentPlaceHolderID="ScriptContent">
</asp:Content>