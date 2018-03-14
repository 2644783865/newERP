<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="House_Form.aspx.cs" Inherits="Infobasis.Web.Pages.Business.House_Form" 
    MasterPageFile="~/PageMaster/Popup.master" %>

<asp:Content ID="HeadContent" runat="server" ContentPlaceHolderID="HeadContent">

</asp:Content>
<asp:Content ID="MainContent" runat="server" ContentPlaceHolderID="MainContent">
    <f:PageManager ID="PageManager1" AutoSizePanelID="Panel1" runat="server" />
    <f:Panel ID="Panel1" ShowBorder="false" ShowHeader="false" runat="server"
        AutoScroll="true" BodyPadding="10px">
        <Toolbars>
            <f:Toolbar ID="Toolbar1" runat="server">
                <Items>
                    <f:Button ID="btnClose" Icon="SystemClose" EnablePostBack="false" runat="server"
                        Text="关闭">
                    </f:Button>
                    <f:ToolbarSeparator ID="ToolbarSeparator2" runat="server">
                    </f:ToolbarSeparator>
                    <f:Button ID="btnContinue" Icon="DatabaseSave" ValidateForms="SimpleForm1" runat="server" Text="保存后继续添加" OnClick="btnSaveContinue_Click">
                    </f:Button>
                    <f:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                    </f:ToolbarSeparator>
                    <f:Button ID="btnSaveClose" ValidateForms="SimpleForm1" Icon="SystemSaveClose"
                        OnClick="btnSaveClose_Click" runat="server" Text="保存后关闭">
                    </f:Button>
                </Items>
            </f:Toolbar>
        </Toolbars>
        <Items>
            <f:SimpleForm ID="SimpleForm1" ShowBorder="false" ShowHeader="false" runat="server"
                Title="SimpleForm" EnableTableStyle="true" MessageTarget="Qtip" Width="600px">
                <Items>
                    <f:TextBox ID="tbxName" runat="server" Label="名称" Required="true" ShowRedStar="true">
                    </f:TextBox>
                    <f:TextBox ID="tbxLocation" runat="server" Label="物业地址" Required="true" ShowRedStar="true">
                    </f:TextBox>
                    <f:NumberBox ID="tbxHouseNum" Label="套数" Required="false" Text="1" ShowRedStar="false" runat="server">
                    </f:NumberBox>
                    <f:NumberBox ID="tbxPrice" Label="均价" Required="false" Text="0" ShowRedStar="false" runat="server">
                    </f:NumberBox>
                    <f:DropDownList ID="DropDownHouseType" Label="房屋类型" Required="false" ShowRedStar="false" runat="server">
                        <f:ListItem />
                    </f:DropDownList>    
                    <f:TextArea ID="tbxRemark" runat="server" Label="备注">
                    </f:TextArea>
                    <f:DropDownList ID="DropDownProvince" Label="城市" Required="false" ShowRedStar="false" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DropDownProvince_SelectedIndexChanged">
                        <f:ListItem />
                    </f:DropDownList>    
                    <f:DropDownList ID="DropDownCity" Label="区域" Required="false" ShowRedStar="false" runat="server">
                        <f:ListItem />
                    </f:DropDownList>       
                    <f:DatePicker ID="tbxStartDate" Label="开工时间" runat="server" EnableEdit="false" AutoShowClearIcon="true"></f:DatePicker> 
                    <f:DatePicker ID="tbxCompletionDate" Label="交房时间" runat="server" EnableEdit="false" AutoShowClearIcon="true"></f:DatePicker>                                                  
                    <f:CheckBox ID="cbxIsImportant" runat="server" Checked="true" Label="重点">
                    </f:CheckBox>
                </Items>
            </f:SimpleForm>
        </Items>
    </f:Panel>
</asp:Content>
<asp:Content ID="ScriptContent" runat="server" ContentPlaceHolderID="ScriptContent">
</asp:Content>