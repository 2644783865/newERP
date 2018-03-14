<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EE_Form.aspx.cs" Inherits="Infobasis.Web.Pages.HR.EE_Form"
    MasterPageFile="~/PageMaster/Page.master" %>

<asp:Content ID="HeadContent" runat="server" ContentPlaceHolderID="HeadContent">
    <link rel="stylesheet" href="../../res/third/jqueryuiautocomplete/jquery-ui.css" />
    <style>
        .userphoto .f-field-label {
            margin-top: 0;
            padding: 5px 10px !important;
        }

        /*
        .userphoto .f-field-body-cell {
            border-bottom-width: 0;
        }
        */

        .userphoto img {
            width: 100%;
        }

        .uploadbutton .f-field-body-cell-rightpart {
            padding: 5px 20px !important;
        }

        .uploadbutton .f-btn {
            width: 100%;
        }
    </style>
</asp:Content>
<asp:Content ID="MainContent" runat="server" ContentPlaceHolderID="MainContent">
    <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="PanelMain" />
    <f:Panel ID="PanelMain" ShowBorder="false" ShowHeader="false" runat="server"
        BodyPadding="10px" AutoScroll="true">
        <Items>
            <f:TabStrip ID="TabStripPage" runat="server" ShowBorder="false" TabPosition="Top" TabBorderColor="true" 
                TabPlain="true" BodyPadding="5px" MarginTop="3px" AutoScroll="false" CssClass="f-tabstrip-theme-simple" Height="525px">
                <Tabs>
                    <f:Tab ID="Tab1" Title="基本信息" runat="server">
                        <Items>
                            <f:Form ID="Form1" LabelAlign="Right" RedStarPosition="BeforeText" LabelWidth="90px"
                                ShowBorder="false" ShowHeader="false" runat="server"
                                EnableTableStyle="true" MessageTarget="Qtip" AutoScroll="true">
                                <Toolbars>
                                    <f:Toolbar runat="server" Position="Bottom">
                                        <Items>
                                            <f:Button ID="btnSave" IconFont="Save" Text="保存信息" ValidateForms="Form1" ValidateMessageBox="false" runat="server" OnClick="btnSave_Click">
                                            </f:Button>
                                        </Items>
                                    </f:Toolbar>
                                </Toolbars>
                                <Items>

                                    <f:Panel ID="Panel2" runat="server" ShowBorder="false" ShowHeader="false" Layout="HBox" BoxConfigAlign="StretchMax">
                                        <Items>
                                            <f:Panel ID="Panel1" Title="面板1" BoxFlex="5" runat="server" ShowBorder="false" ShowHeader="false">
                                                <Items>
                                                    <f:TextBox ID="tbxName" runat="server" Label="姓名" ShowRedStar="true" Required="true"></f:TextBox>
                                                    <f:DropDownList ID="ddlGender" Label="性别" runat="server" ShowRedStar="true" Required="true" CompareMessage="性别不能为空！">
                                                        <f:ListItem Text="男" Value="男" Selected="true" />
                                                        <f:ListItem Text="女" Value="女" />
                                                    </f:DropDownList>
                                                    <f:DatePicker ID="dpBirthDay" Label="出生日期" EmptyText="请选择日期" runat="server" EnableEdit="false" ShowRedStar="true" Required="true" EnableAjax="true" AutoPostBack="true" OnTextChanged="dpBirthDay_TextChanged"></f:DatePicker>
                                                    <f:TextBox ID="tbxAge" runat="server" Label="年龄" Readonly="true" ShowRedStar="false" CompareMessage="年龄不能为空！"></f:TextBox>
                                                    <f:TextBox ID="tbxTel" runat="server" Label="手机" EmptyText="手机号码" Required="true" ShowRedStar="true" Regex="^[1]+[358]+\d{9}$"></f:TextBox>
                                                    <f:TextBox ID="tbxEmail" runat="server" Label="邮箱" RegexPattern="EMAIL" RegexMessage="请输入有效的邮箱地址"></f:TextBox>                                   
                                                </Items>
                                            </f:Panel>
                                            <f:Panel ID="Panel4" runat="server" BoxFlex="3" ShowBorder="false" ShowHeader="false" Layout="VBox">
                                                <Items>
                                                    <f:TextBox ID="tbxEmployeeNum" runat="server" Label="员工号" ShowRedStar="false" Required="true"></f:TextBox>
                                                    <f:DropDownList ID="ddlEmploymentType" Label="用工形式" ShowRedStar="false" Required="false" runat="server">
                                                        <f:ListItem Text="全职" Value="1" Selected="true" />
                                                        <f:ListItem Text="兼职" Value="2" />
                                                        <f:ListItem Text="实习生" Value="3" />
                                                        <f:ListItem Text="临时工" Value="4" />
                                                    </f:DropDownList>
                                                    <f:TextArea ID="tbxRemark" Label="备注" runat="server" BoxFlex="1"></f:TextArea>
                                                </Items>
                                            </f:Panel>
                                            <f:Panel ID="Panel5" CssClass="nolabelpanel" Title="面板1" BoxFlex="2" runat="server" ShowBorder="false" ShowHeader="false" Layout="VBox">
                                                <Items>
                                                    <f:Image ID="imgUserPortal" ImageUrl="~/res/images/blank_180.png" runat="server" BoxFlex="1" CssClass="userphoto">
                                                    </f:Image>
                                                    <f:FileUpload ID="userPortraitUpload" runat="server" ButtonText="上传照片" ButtonOnly="true" CssClass="uploadbutton" AutoPostBack="true" OnFileSelected="userPortraitUpload_FileSelected" Hidden="true"></f:FileUpload>
                                                </Items>
                                            </f:Panel>
                                        </Items>
                                    </f:Panel>
                                    <f:Form ID="Form7" ShowBorder="false" ShowHeader="false" runat="server"
                                        MessageTarget="Qtip">
                                        <Rows>
                                            <f:FormRow>
                                                <Items>
                                                     <f:DropDownList ID="ddlMarriage" Label="婚姻状况" ShowRedStar="true" Required="true" runat="server">
                                                        <f:ListItem Text="未婚" Value="未婚" />
                                                        <f:ListItem Text="已婚" Value="已婚" />
                                                        <f:ListItem Text="离异" Value="离异" />
                                                        <f:ListItem Text="丧偶" Value="丧偶" />
                                                        <f:ListItem Text="未说明" Value="未说明" Selected="true"  />
                                                    </f:DropDownList>
                                                    <f:DropDownList ID="ddlNation" Label="民族" ShowRedStar="true" Required="true" runat="server">
                                                        <f:ListItem Text="汉族" Value="汉族" Selected="true" />
                                                        <f:ListItem Text="回族" Value="回族" />
                                                        <f:ListItem Text="壮族" Value="壮族" />
                                                        <f:ListItem Text="回族" Value="回族" />
                                                        <f:ListItem Text="苗族" Value="苗族" />
                                                        <f:ListItem Text="维吾尔族" Value="维吾尔族" />
                                                        <f:ListItem Text="土家族" Value="土家族" />
                                                        <f:ListItem Text="彝族" Value="彝族" />
                                                        <f:ListItem Text="蒙古族" Value="蒙古族" />
                                                        <f:ListItem Text="藏族" Value="藏族" />
                                                        <f:ListItem Text="布依族" Value="布依族" />
                                                        <f:ListItem Text="侗族" Value="侗族" />
                                                        <f:ListItem Text="瑶族" Value="瑶族" />
                                                        <f:ListItem Text="朝鲜族" Value="朝鲜族" />
                                                        <f:ListItem Text="白族" Value="白族" />
                                                        <f:ListItem Text="其他" Value="其他" />
                                                    </f:DropDownList>
                                                </Items>
                                            </f:FormRow>
                                            <f:FormRow>
                                                <Items>
                                                    <f:TextBox ID="tbxUserName" runat="server" Label="登录名" EmptyText="默认为手机号码"></f:TextBox>
                                                    <f:TextBox ID="tbxPassWord" runat="server" Label="密码" EmptyText="默认为手机号码后6位"></f:TextBox>
                                                </Items>
                                            </f:FormRow>
                                            <f:FormRow>
                                                <Items>
                                                    <f:DatePicker ID="tbxOnBoardDate" Label="入职日期" runat="server" EnableEdit="false" Required="true" ShowRedStar="true"></f:DatePicker>
                                                     <f:DatePicker ID="tbxBecomeRegularDate" Label="转正日期" runat="server" EnableEdit="false" Required="false" ShowRedStar="false"></f:DatePicker>
                                                </Items>
                                            </f:FormRow>
                                            <f:FormRow>
                                                <Items>
                                                    <f:DropDownBox runat="server" ID="ddbDept" Label="所属部门" AutoShowClearIcon="true">
                                                        <PopPanel>
                                                            <f:Grid ID="gridDept" ShowBorder="true" ShowHeader="false" runat="server"
                                                                DataKeyNames="ID" Hidden="true" Width="550px" ShowGridHeader="false"
                                                                EnableTree="true" TreeColumn="Name" DataIDField="ID" DataParentIDField="ParentID" DataTextField="Name"
                                                                ExpandAllTreeNodes="true"
                                                                EnableRowLines="false" EnableAlternateRowColor="false">
                                                                <Columns>
                                                                    <f:BoundField ColumnID="Name" DataField="Name" HeaderText="部门名称" ExpandUnusedSpace="true" />
                                                                </Columns>
                                                            </f:Grid>
                                                        </PopPanel>
                                                    </f:DropDownBox>
                                                    <f:DropDownBox runat="server" ID="ddbTitles" Label="拥有职称" AutoShowClearIcon="true" DataControlID="cblTitles" EnableMultiSelect="false">
                                                        <PopPanel>
                                                            <f:SimpleForm ID="SimpleForm3" BodyPadding="10px" runat="server" AutoScroll="true"
                                                                ShowBorder="True" ShowHeader="false" Hidden="true">
                                                                <Items>
                                                                    <f:CheckBoxList ID="cblTitles" ColumnNumber="3" DataTextField="Name" DataValueField="ID" runat="server">
                                                                    </f:CheckBoxList>
                                                                </Items>
                                                            </f:SimpleForm>
                                                        </PopPanel>
                                                    </f:DropDownBox>
                                                </Items>
                                            </f:FormRow>
                                            <f:FormRow>
                                                <Items>
                                                    <f:DropDownBox runat="server" ID="ddbRoles" Label="所属角色" AutoShowClearIcon="true" DataControlID="cblRoles" EnableMultiSelect="true">
                                                        <PopPanel>
                                                            <f:SimpleForm ID="SimpleForm2" BodyPadding="10px" runat="server" AutoScroll="true"
                                                                ShowBorder="True" ShowHeader="false" Hidden="true">
                                                                <Items>
                                                                    <f:CheckBoxList ID="cblRoles" ColumnNumber="3" DataTextField="Name" DataValueField="ID" runat="server">
                                                                    </f:CheckBoxList>
                                                                </Items>
                                                            </f:SimpleForm>
                                                        </PopPanel>
                                                    </f:DropDownBox>
                                                    <f:TextBox ID="tbxReportTo" runat="server" Label="汇报上级" ShowRedStar="false" Required="false" EmptyText="输入姓名直接查找"></f:TextBox>
                                                    <f:HiddenField ID="tbxReportToID" runat="server"></f:HiddenField>
                                                </Items>
                                            </f:FormRow>
                                            <f:FormRow>
                                                <Items>
                                                    <f:DropDownList ID="ddlCertificates" Label="证件类别" runat="server">
                                                        <f:ListItem Text="身份证" Value="身份证" Selected="true" />
                                                        <f:ListItem Text="军官证" Value="军官证" />
                                                        <f:ListItem Text="护照" Value="护照" />
                                                        <f:ListItem Text="驾驶证" Value="驾驶证" />
                                                    </f:DropDownList>
                                                    <f:TextBox ID="tbxIDCardEdit" runat="server" Label="证件号"></f:TextBox>
                                                </Items>
                                            </f:FormRow>
                                            <f:FormRow>
                                                <Items>
                                                    <f:DropDownList ID="ddlEducation" Label="文化程度" runat="server">
                                                        <f:ListItem Text="" Value="" Selected="true" />
                                                        <f:ListItem Text="研究生" Value="研究生" />
                                                        <f:ListItem Text="大学本科" Value="大学本科" />
                                                        <f:ListItem Text="大学专科" Value="大学专科 " />
                                                        <f:ListItem Text="中等职业" Value="中等职业" />
                                                        <f:ListItem Text="普通高级中学" Value="普通高级中学" />
                                                        <f:ListItem Text="初级中学" Value="初级中学" />
                                                        <f:ListItem Text="小学" Value="小学" />
                                                        <f:ListItem Text="小学以下" Value="小学以下" />
                                                    </f:DropDownList>
                                                    <f:DropDownList ID="ddlNativePlace" Label="籍贯" runat="server">
                                                        <f:ListItem Text="上海" Value="上海" Selected="true" />
                                                        <f:ListItem Text="北京" Value="北京" />
                                                        <f:ListItem Text="湖北" Value="湖北" />
                                                        <f:ListItem Text="湖南" Value="湖南" />
                                                        <f:ListItem Text="山东" Value="山东" />
                                                        <f:ListItem Text="河南" Value="河南" />
                                                        <f:ListItem Text="其他" Value="其他" />
                                                    </f:DropDownList>
                                                </Items>
                                            </f:FormRow>
                                            <f:FormRow>
                                                <Items>
                                                    <f:TextBox ID="tbxNativePlace" runat="server" Label="居住地址"></f:TextBox>
                                                    <f:DropDownList ID="ddlUserType" Label="员工类型" runat="server" CompareMessage="员工类型不能为空！">
                                                        <f:ListItem Text="员工" Value="0" Selected="true" />
                                                        <f:ListItem Text="供应商" Value="1" />
                                                    </f:DropDownList>
                                                </Items>
                                            </f:FormRow>
                                        </Rows>
                                    </f:Form>
                                </Items>
                            </f:Form>
                        </Items>
                    </f:Tab>
                    <f:Tab ID="TabEducation" Title="教育培训" runat="server" AutoScroll="true">
                        <Items>
                            <f:Grid ID="GridEducation" runat="server" ShowBorder="true" ShowHeader="false"
                                EnableCheckBoxSelect="true" AutoScroll="true"
                                DataKeyNames="ID" AllowSorting="true" SortField="StartDate"
                                SortDirection="DESC" AllowPaging="true" IsDatabasePaging="true"
                                MouseWheelSelection="true" QuickPaging="true" EnableTextSelection="true"
                                AllowColumnLocking="false">
                                <Toolbars>
                                    <f:Toolbar runat="server" Position="Top">
                                        <Items>
                                            <f:Button ID="btnNewEducation" runat="server" Icon="Add" EnablePostBack="false" Text="新增记录"></f:Button>
                                        </Items>
                                    </f:Toolbar>
                                </Toolbars>
                                <Columns>
                                    <f:RowNumberField Width="25px" EnablePagingNumber="true" />
                                    <f:BoundField DataField="EducationalInstitution" SortField="EducationalInstitution" Width="120px" HeaderText="学校/机构" />
                                    <f:BoundField DataField="Major" SortField="Major" Width="120px" HeaderText="专业" />
                                    <f:BoundField DataField="StartDate" SortField="StartDate" Width="90px" HeaderText="开始时间" DataToolTipFormatString="{0:yyyy-MM-dd}" DataFormatString="{0:yyyy-MM-dd}" />
                                    <f:BoundField DataField="EndDate" SortField="EndDate" Width="90px" HeaderText="结束时间" DataToolTipFormatString="{0:yyyy-MM-dd}" DataFormatString="{0:yyyy-MM-dd}" />
                                    <f:BoundField DataField="AcademicDegree" SortField="AcademicDegree" Width="100px" HeaderText="学位" />
                                    <f:BoundField DataField="EducationName" SortField="Education" Width="100px" HeaderText="学历" />
                                    <f:BoundField DataField="EducationTypeName" SortField="EducationTypeName" Width="100px" HeaderText="类型" />
                                    <f:CheckBoxField DataField="IsHighest" HeaderText="是否最高学历" RenderAsStaticField="true"
                                        AutoPostBack="false" CommandName="IsHighest" Width="90px" />
                                    <f:BoundField DataField="CreateByName" SortField="CreateByName" Width="80px" HeaderText="修改人" />
                                    <f:WindowField ColumnID="editField" TextAlign="Center" Icon="Pencil" ToolTip="编辑"
                                        WindowID="Window1" Title="编辑" DataIFrameUrlFields="ID,UserID" DataIFrameUrlFormatString="~/Pages/HR/EE_Education_Form.aspx?id={0}&uid={1}&innerActiveTab=ctl00_MainContent_PanelMain_TabStripPage_TabEducation"
                                        Width="50px" />
                                </Columns>
                            </f:Grid>
                        </Items>
                    </f:Tab>
                    <f:Tab ID="TabEmployeement" Title="工作经历" runat="server" AutoScroll="true">
                        <Items>
                            <f:Grid ID="GridEmployeement" runat="server" ShowBorder="true" ShowHeader="false"
                                EnableCheckBoxSelect="true" AutoScroll="true"
                                DataKeyNames="ID" AllowSorting="true" SortField="StartDate"
                                SortDirection="DESC" AllowPaging="true" IsDatabasePaging="true"
                                MouseWheelSelection="true" QuickPaging="true" EnableTextSelection="true"
                                AllowColumnLocking="false">
                                <Toolbars>
                                    <f:Toolbar ID="Toolbar1" runat="server" Position="Top">
                                        <Items>
                                            <f:Button ID="btnNewEmployeement" runat="server" Icon="Add" EnablePostBack="false" Text="新增记录"></f:Button>
                                        </Items>
                                    </f:Toolbar>
                                </Toolbars>
                                <Columns>
                                    <f:RowNumberField Width="25px" EnablePagingNumber="true" />
                                    <f:BoundField DataField="CompanyName" SortField="CompanyName" Width="160px" HeaderText="公司" />
                                    <f:BoundField DataField="JobTitle" SortField="JobTitle" Width="120px" HeaderText="职位" />
                                    <f:BoundField DataField="StartDate" SortField="StartDate" Width="100px" HeaderText="开始时间" DataToolTipFormatString="{0:yyyy-MM-dd}" DataFormatString="{0:yyyy-MM-dd}" />
                                    <f:BoundField DataField="EndDate" SortField="EndDate" Width="100px" HeaderText="结束时间" DataToolTipFormatString="{0:yyyy-MM-dd}" DataFormatString="{0:yyyy-MM-dd}" />
                                    <f:BoundField DataField="LeaveReason" SortField="LeaveReason" Width="120px" HeaderText="离职原因" />
                                    <f:BoundField DataField="CreateByName" SortField="CreateByName" Width="80px" HeaderText="修改人" />
                                    <f:WindowField ColumnID="editField" TextAlign="Center" Icon="Pencil" ToolTip="编辑"
                                        WindowID="Window1" Title="编辑" DataIFrameUrlFields="ID,UserID" DataIFrameUrlFormatString="~/Pages/HR/EE_Employeement_Form.aspx?id={0}&uid={1}&innerActiveTab=ctl00_MainContent_PanelMain_TabStripPage_TabEmployeement"
                                        Width="50px" />
                                </Columns>
                            </f:Grid>
                        </Items>
                    </f:Tab>
                    <f:Tab ID="TabContract" Title="合同管理" runat="server" AutoScroll="true">
                       <Items>
                            <f:Grid ID="GridContract" runat="server" ShowBorder="true" ShowHeader="false"
                                EnableCheckBoxSelect="true" AutoScroll="true"
                                DataKeyNames="ID" AllowSorting="true" SortField="ContractStartDate"
                                SortDirection="DESC" AllowPaging="true" IsDatabasePaging="true"
                                MouseWheelSelection="true" QuickPaging="true" EnableTextSelection="true"
                                AllowColumnLocking="false">
                                <Toolbars>
                                    <f:Toolbar ID="Toolbar2" runat="server" Position="Top">
                                        <Items>
                                            <f:Button ID="btnNewContract" runat="server" Icon="Add" EnablePostBack="false" Text="新增记录"></f:Button>
                                        </Items>
                                    </f:Toolbar>
                                </Toolbars>
                                <Columns>
                                    <f:RowNumberField Width="25px" EnablePagingNumber="true" />
                                    <f:BoundField DataField="ContractCode" SortField="ContractCode" Width="160px" HeaderText="合同编号" />
                                    <f:BoundField DataField="JobTitle" SortField="JobTitle" Width="120px" HeaderText="职位" />
                                    <f:BoundField DataField="ContractStartDate" SortField="ContractStartDate" Width="100px" HeaderText="合同开始时间" DataToolTipFormatString="{0:yyyy-MM-dd}" DataFormatString="{0:yyyy-MM-dd}" />
                                    <f:BoundField DataField="ContractEndDate" SortField="ContractEndDate" Width="100px" HeaderText="合同结束时间" DataToolTipFormatString="{0:yyyy-MM-dd}" DataFormatString="{0:yyyy-MM-dd}" />
                                    <f:BoundField DataField="ProbationPeriod" SortField="ProbationPeriod" Width="120px" HeaderText="试用期" />
                                    <f:BoundField DataField="CreateByName" SortField="CreateByName" Width="80px" HeaderText="修改人" />
                                    <f:WindowField ColumnID="editField" TextAlign="Center" Icon="Pencil" ToolTip="编辑"
                                        WindowID="Window1" Title="编辑" DataIFrameUrlFields="ID,UserID" DataIFrameUrlFormatString="~/Pages/HR/EE_Contract_Form.aspx?id={0}&uid={1}&innerActiveTab=ctl00_MainContent_PanelMain_TabStripPage_TabContract"
                                        Width="50px" />
                                </Columns>
                            </f:Grid>
                        </Items>
                    </f:Tab>
                    <f:Tab ID="TabSalary" Title="薪资" runat="server" AutoScroll="true">
                        <Items>
                            <f:Form ID="FormFixPay" MessageTarget="Qtip" ShowHeader="true" Width="600px" BodyPadding="5px" Title="固定薪酬" runat="server">
                                <Items>
                                    <f:Panel ID="Panel3" ShowHeader="false" CssClass="" ShowBorder="false" Layout="Column" runat="server">
                                        <Items>
                                            <f:Label ID="Label2" Width="100px" runat="server" CssClass="marginr" ShowLabel="false"
                                                Text="试用期固定工资：">
                                            </f:Label>
                                            <f:NumberBox ID="tbxProbationFixPayValue" ShowLabel="false" Label="试用期固定工资" Required="false" Width="150px" CssClass="marginr" runat="server">
                                            </f:NumberBox>
                                        </Items>
                                    </f:Panel>
                                    <f:Panel ID="Panel9" ShowHeader="false" CssClass="" ShowBorder="false" Layout="Column" runat="server">
                                        <Items>
                                            <f:Label ID="Label7" Width="100px" runat="server" CssClass="marginr" ShowLabel="false"
                                                Text="正式基本工资：">
                                            </f:Label>
                                            <f:NumberBox ID="tbxFixPayValue" ShowLabel="false" Label="正式工资" Required="false" Width="150px" CssClass="marginr" runat="server">
                                            </f:NumberBox>
                                        </Items>
                                    </f:Panel>
                                    <f:Panel ID="Panel6" ShowHeader="false" ShowBorder="false" Layout="Column" runat="server">
                                        <Items>
                                            <f:Label ID="Label1" Width="100px" runat="server" CssClass="marginr" ShowLabel="false"
                                                Text="岗位津贴：">
                                            </f:Label>
                                            <f:NumberBox ID="tbxJobAllowanceValue" ShowLabel="false" Label="岗位津贴" Required="false" Width="150px" CssClass="marginr" runat="server">
                                            </f:NumberBox>
                                        </Items>
                                    </f:Panel>
                                    <f:Panel ID="Panel7" ShowHeader="false" ShowBorder="false" Layout="Column" CssClass="" runat="server">
                                        <Items>
                                            <f:Label ID="Label3" Width="100px" runat="server" CssClass="marginr" ShowLabel="false"
                                                Text="交通补贴：">
                                            </f:Label>
                                            <f:NumberBox ID="tbxTrafficAllowanceValue" ShowLabel="false" Label="交通补贴" Required="false" Width="150px" CssClass="marginr" runat="server">
                                            </f:NumberBox>
                                        </Items>
                                    </f:Panel>
                                    <f:Panel ID="Panel13" ShowHeader="false" ShowBorder="false" Layout="Column" CssClass="" runat="server">
                                        <Items>
                                            <f:Label ID="Label9" Width="100px" runat="server" CssClass="marginr" ShowLabel="false"
                                                Text="餐饮补贴：">
                                            </f:Label>
                                            <f:NumberBox ID="tbxDiningAllowanceValue" ShowLabel="false" Label="餐饮补贴" Required="false" Width="150px" CssClass="marginr" runat="server">
                                            </f:NumberBox>
                                        </Items>
                                    </f:Panel>
                                    <f:Button ID="btnFixPay" Text="保存" ValidateForms="FormFixPay" ValidateMessageBox="true" runat="server" OnClick="btnFixPay_Click">
                                    </f:Button>
                                </Items>
                            </f:Form>

                            <f:Form ID="FormBank" MessageTarget="Qtip" ShowHeader="true" Width="600px" BodyPadding="5px" Title="银行卡信息" runat="server" MarginTop="10px">
                                <Items>
                                    <f:Panel ID="Panel8" ShowHeader="false" CssClass="" ShowBorder="false" Layout="Column" runat="server">
                                        <Items>
                                            <f:Label ID="Label4" Width="100px" runat="server" CssClass="marginr" ShowLabel="false"
                                                Text="银行账号：">
                                            </f:Label>
                                            <f:NumberBox ID="tbxBankAccount" ShowLabel="false" Label="银行账号" NoDecimal="true" Required="false" Width="150px" CssClass="marginr" runat="server">
                                            </f:NumberBox>
                                        </Items>
                                    </f:Panel>
                                    <f:Panel ID="Panel10" ShowHeader="false" CssClass="" ShowBorder="false" Layout="Column" runat="server">
                                        <Items>
                                            <f:Label ID="Label5" Width="100px" runat="server" CssClass="marginr" ShowLabel="false"
                                                Text="开户行：">
                                            </f:Label>
                                            <f:TextBox ID="tbxBankName" ShowLabel="false" Label="开户行" Required="false" Width="250px" CssClass="marginr" runat="server">
                                            </f:TextBox>
                                        </Items>
                                    </f:Panel>
                                    <f:Panel ID="Panel11" ShowHeader="false" ShowBorder="false" Layout="Column" runat="server">
                                        <Items>
                                            <f:Label ID="Label6" Width="100px" runat="server" CssClass="marginr" ShowLabel="false"
                                                Text="开户名：">
                                            </f:Label>
                                            <f:TextBox ID="tbxBankUserName" ShowLabel="false" Label="开户名" Required="false" Width="250px" CssClass="marginr" runat="server">
                                            </f:TextBox>
                                        </Items>
                                    </f:Panel>
                                    <f:Panel ID="Panel12" ShowHeader="false" ShowBorder="false" Layout="Column" CssClass="" runat="server">
                                        <Items>
                                            <f:Label ID="Label8" Width="100px" runat="server" CssClass="marginr" ShowLabel="false"
                                                Text="备注：">
                                            </f:Label>
                                            <f:TextBox ID="tbxBankRemark" ShowLabel="false" Label="备注" Required="false" Width="150px" CssClass="marginr" runat="server">
                                            </f:TextBox>
                                        </Items>
                                    </f:Panel>
                                    <f:Button ID="btnBank" Text="保存" ValidateForms="FormBank" ValidateMessageBox="true" runat="server" OnClick="btnBank_Click">
                                    </f:Button>
                                </Items>
                            </f:Form>

                        </Items>
                    </f:Tab>
                    <f:Tab ID="Tab6" Title="变动记录" runat="server" AutoScroll="true">
                        <Items>
                            <f:Grid ID="GridAdjust" runat="server" ShowBorder="true" ShowHeader="false"
                                EnableCheckBoxSelect="true" AutoScroll="true"
                                DataKeyNames="ID" AllowSorting="true" OnSort="GridAdjust_Sort" SortField="AdjustDate"
                                SortDirection="DESC" AllowPaging="true" IsDatabasePaging="true"
                                MouseWheelSelection="true" QuickPaging="true" EnableTextSelection="true"
                                AllowColumnLocking="true">
                                <Columns>
                                    <f:RowNumberField Width="25px" EnablePagingNumber="true" />
                                    <f:BoundField DataField="AdjustItemName" SortField="AdjustItemName" Width="100px" HeaderText="变动类型" />
                                    <f:BoundField DataField="AdjustDate" SortField="AdjustDate" Width="100px" HeaderText="变动日期" DataToolTipFormatString="{0:yyyy-MM-dd hh:mm:ss}" DataFormatString="{0:yyyy-MM-dd}" />
                                    <f:BoundField DataField="CreateByName" SortField="CreateByName" Width="80px" HeaderText="修改人" />
                                    <f:BoundField DataField="AllChangeData" SortField="AllChangeData" Width="200px" HeaderText="记录" ExpandUnusedSpace="true" />
                                </Columns>
                            </f:Grid>
                        </Items>
                    </f:Tab>
                </Tabs>
            </f:TabStrip>
        </Items>
    </f:Panel>
    <f:Window ID="Window1" CloseAction="Hide" runat="server" IsModal="true" Hidden="true"
        Target="Top" EnableResize="true" EnableMaximize="true" EnableIFrame="true" IFrameUrl="about:blank"
        Width="900px" Height="500px" OnClose="Window1_Close">
    </f:Window>
    <br />
    <br />
</asp:Content>

<asp:Content ID="ScriptContent" runat="server" ContentPlaceHolderID="ScriptContent">
    <script src="../../res/third/jqueryuiautocomplete/jquery-ui.js" type="text/javascript"></script>
    <script src="../../res/js/axios.min.js"></script>
    <script src="../../res/js/api.js"></script>
    <script type="text/javascript">
        var textbox1ID = '<%= tbxReportTo.ClientID %>';
        var textboxReportID = '<%= tbxReportToID.ClientID %>';
        var emailBoxID = '<%= tbxEmail.ClientID %>';
        var gridEducationID = '<%= GridEducation.ClientID %>';
        var tabStripPageID = '<%= TabStripPage.ClientID %>';

        function setActiveTabById(activeTabId) {
            var tabStripPage = F(tabStripPageID);
            console.log("active:" + activeTabId);
            tabStripPage.activeTab(activeTabId);
        }

        F.ready(function () {

            // 将字符串 val 以逗号空格作为分隔符，分隔成数组
            function split(val) {
                return val.split(/,\s*/);
            }

            // 取得以逗号空格为分隔符的最后一个单词
            // 比如，输入为 "C++, C#, JavaScript" 则输入出 "JavaScript"
            function extractLast(term) {
                return split(term).pop();
            }

            $('#' + textbox1ID + ' input').bind("keydown", function (event) {
                // 通过 Tab 选择一项时，不会使当前文本框失去焦点
                if (event.keyCode === $.ui.keyCode.TAB &&
                        $(this).data("autocomplete").menu.active) {
                    event.preventDefault();
                }
            }).autocomplete({
                minLength: 0,
                source: function (request, response) {
                    infobasisService.getAjaxInstance.get('/employee/ids?term=' + extractLast(request.term))
                      .then(function (res) {
                          response($.map(res.data, function (item) {
                              return {
                                  label: item.label,
                                  value: item.value
                              }
                          }));
                      })
                      .catch(function (error) {
                          console.log(error);
                      });

                    //$.getJSON("../../Handler/SearchHandler.ashx", {
                    //    term: extractLast(request.term)
                    //}, response);
                },
                search: function () {
                    // 自定义的minLength（如果要限制两个字符才提示，把下面的1改为2即可）
                    var term = extractLast(this.value);
                    if (term.length < 1) {
                        return false;
                    }
                },
                focus: function () {
                    // 阻止某一项获得焦点时，更新文本框的值
                    return false;
                },
                select: function (event, ui) {
                    var $textboxReportID = $("#" + textboxReportID);
                    var reportTo = F(textboxReportID);
                    this.value = ui.item.label;
                    $textboxReportID.val(ui.item.value);
                    reportTo.setValue(ui.item.value);
                    return false;
                    //console.log("Selected: " + ui.item.value + " aka " + ui.item.label);
                    var terms = split(this.value);
                    var ids = split($textboxReportID.val());
                    // 移除用户正在输入项
                    terms.pop();
                    // 添加用户选择的项
                    terms.push(ui.item.label);
                    ids.push(ui.item.value);
                    // 添加占位符，确保字符串的最后以逗号空格结束
                    terms.push("");
                    ids.push("");
                    this.value = terms.join(", ");
                    $textboxReportID.val(ids.join(", "));
                    return false;
                }
            });

            var availableTags = [
                "qq.com",
                "163.com",
                "gmail.com",
                "outlook.com",
                "126.com",
                "sina.com",
                "yahoo.com",
                "sohu.com",
                "foxmail.com",
                "live.com",
                "mail.ustc.edu.cn"];


            function getFullEmails(name) {
                var emails = [];
                for (var i = 0, count = availableTags.length; i < count; i++) {
                    emails.push(name + "@" + availableTags[i]);
                }
                return emails;
            }

            $('#' + emailBoxID + ' input').autocomplete({
                source: function (request, response) {
                    if (request.term.indexOf('@') === -1) {
                        response(getFullEmails(request.term));
                    }
                }
            });

        });

    </script>
</asp:Content>