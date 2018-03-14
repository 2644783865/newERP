<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ClientTrace.aspx.cs" Inherits="Infobasis.Web.Pages.Business.ClientTrace"
    MasterPageFile="~/PageMaster/Popup.master" %>

<asp:Content ID="HeadContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style>
        .f-field-fieldlabel-right {
            font-weight:bold;
        }
        .leftUserInfo {
            float: left;
            width: 130px;
            border-right: 1px solid #eee;
            /* border-left: 2px solid #2b7dbc; */
            padding: 2px;
        }
        .leftUserInfo .portraitImg img {
            border-radius: 50%;
            width: 40px;
            height: 40px;
        }
        .rightDesc {
            margin-left: 135px;
        }
        .f-datalist-item-inner {
            overflow:hidden;
        }
    </style>
</asp:Content>
<asp:Content ID="MainContent" runat="server" ContentPlaceHolderID="MainContent">
    <f:PageManager ID="PageManager1" runat="server" />
    <f:Panel ID="PanelInfo1" runat="server" ShowBorder="false" ShowHeader="false" Layout="HBox">
        <Items>
            <f:Label runat="server" ID="labNo" Label="客户编号" BoxFlex="1" LabelAlign="Right"></f:Label>
            <f:Label runat="server" ID="labName" Label="客户名称" BoxFlex="1" LabelAlign="Right"></f:Label>
            <f:Label runat="server" ID="labTel" Label="联系电话" BoxFlex="1" LabelAlign="Right"></f:Label>
        </Items>
    </f:Panel>
    <f:Panel ID="PanelInfo2" runat="server" ShowBorder="false" ShowHeader="false" Layout="HBox">
        <items>
            <f:Label runat="server" ID="labHousesName" Label="楼盘" BoxFlex="1" LabelAlign="Right"></f:Label>
            <f:Label runat="server" ID="labAddress" Label="地址" BoxFlex="1" LabelAlign="Right"></f:Label>
        </items>
    </f:Panel>

    <f:TabStrip ID="TabStrip1" ShowBorder="true" TabBorderColor="true" TabPlain="true" ActiveTabIndex="0"
        runat="server" AutoScroll="true" Height="400px">
        <Tabs>
            <f:Tab ID="Tab1" Title="业务日志" BodyPadding="5px" runat="server">
                <Items>
                    <f:Panel ID="PanelBusiness" runat="server" ShowBorder="False" EnableCollapse="false"
                        Layout="HBox" BodyPadding="5px"
                        BoxConfigChildMargin="0 5 0 0" ShowHeader="False"
                        Title="" Height="350px" AutoScroll="true">
                        <Items>
                            <f:Panel ID="Panel1" Title="面板1" BoxFlex="1" runat="server"
                                BodyPadding="0 5px" ShowBorder="false" ShowHeader="false" Height="160px" AutoScroll="true">
                                <Items>
                                    <f:DataList runat="server" ID="DataListClientTrace" OnItemDataBound="DataList1_ItemDataBound"></f:DataList>
                                </Items>
                            </f:Panel>
                            <f:Panel ID="Panel3" Title="面板2" Width="280px"
                                runat="server" BodyPadding="5px" ShowBorder="false" ShowHeader="false">
                                <Items>
                                    <f:SimpleForm ID="SimpleForm1" BodyPadding="5px" runat="server" LabelAlign="Top"  EnableCollapse="true"
                                        Title="表单" ShowHeader="false" ShowBorder="false">
                                        <Items>
                                            <f:TextArea runat="server" Label="添加日志" ID="HtmlEditorAddTrace" Height="250px"></f:TextArea>
                                            <f:DatePicker runat="server" ID="nextTraceDate" EnableEdit="false" Label="下次跟踪时间" DateFormatString="yyyy-MM-dd HH:mm" ShowTime="true" ShowSecond="true"></f:DatePicker>
                                            <f:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="保存" Icon="SystemSave"
                                                CssClass="marginr">
                                            </f:Button>
                                        </Items>
                                    </f:SimpleForm>                  
                                </Items>
                            </f:Panel>
                        </Items>
                    </f:Panel>
                </Items>
            </f:Tab>
            <f:Tab ID="Tab3" Title="客服日志" EnableClose="false" BodyPadding="5px" runat="server">
                <Items>
                    <f:Label ID="Label12" Text="客服日志" runat="server" />
                </Items>
            </f:Tab>
            <f:Tab ID="Tab4" Title="工程日志" EnableClose="false" BodyPadding="5px" runat="server">
                <Items>
                    <f:Label ID="Label13" Text="工程日志" runat="server" />
                </Items>
            </f:Tab>
            <f:Tab ID="Tab2" Title="预约记录" EnableClose="false" BodyPadding="5px" runat="server">
                <Items>
                    <f:Label ID="Label1" Text="预约记录" runat="server" />
                </Items>
            </f:Tab>
        </Tabs>
    </f:TabStrip>
</asp:Content>
<asp:Content ID="ScriptContent" runat="server" ContentPlaceHolderID="ScriptContent">
</asp:Content>