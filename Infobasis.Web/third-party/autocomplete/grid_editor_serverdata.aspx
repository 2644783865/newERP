<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="grid_editor_serverdata.aspx.cs"
    Inherits="FineUIPro.Examples.autocomplete.grid_editor_serverdata" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title></title>
    <meta name="sourcefiles" content="~/third-party/autocomplete/gridsearch.ashx" />
    <link rel="stylesheet" href="../res/jqueryuiautocomplete/jquery-ui.css" />
    <style>
        .ui-autocomplete {
            border-width: 1px;
            border-style: solid;
        }

        .ui-menu-item.ui-state-focus {
            border-width: 1px;
            border-style: solid;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" runat="server" />
        <f:Grid ID="Grid1" ShowBorder="true" ShowHeader="true" Title="表格（进入编辑状态后，从自动完成提示框中选择）"
            EnableCollapse="true" Width="850px" Height="350px"
            runat="server" DataKeyNames="Id,Name" AllowCellEditing="true" ClicksToEdit="1"
            OnPreDataBound="Grid1_PreDataBound">
            <Toolbars>
                <f:Toolbar ID="Toolbar1" runat="server">
                    <Items>
                        <f:Button ID="btnNew" Text="新增数据" Icon="Add" EnablePostBack="false" runat="server">
                        </f:Button>
                        <f:Button ID="btnDelete" Text="删除选中行" Icon="Delete" EnablePostBack="false" runat="server">
                        </f:Button>
                        <f:ToolbarFill runat="server">
                        </f:ToolbarFill>
                        <f:Button ID="btnReset" Text="重置表格数据" EnablePostBack="false" runat="server">
                        </f:Button>
                    </Items>
                </f:Toolbar>
            </Toolbars>
            <Columns>
                <f:TemplateField ColumnID="Number" Width="60px">
                    <ItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                    </ItemTemplate>
                </f:TemplateField>
                <f:RenderField Width="150px" ColumnID="Name" DataField="Name"
                    HeaderText="姓名">
                    <Editor>
                        <f:TextBox ID="tbxEditorName" EmptyText="请输入汉字“张”试试" runat="server">
                        </f:TextBox>
                    </Editor>
                </f:RenderField>
                <f:RenderField Width="100px" ColumnID="Gender" DataField="Gender" FieldType="Int"
                    RendererFunction="renderGender" HeaderText="性别">
                    <Editor>
                        <f:DropDownList Required="true" runat="server">
                            <f:ListItem Text="男" Value="1" />
                            <f:ListItem Text="女" Value="0" />
                        </f:DropDownList>
                    </Editor>
                </f:RenderField>
                <f:RenderField Width="100px" Hidden="true" ColumnID="EntranceYear" DataField="EntranceYear" FieldType="Int"
                    HeaderText="入学年份">
                    <Editor>
                        <f:NumberBox ID="tbxEditorEntranceYear" NoDecimal="true" NoNegative="true" MinValue="2000"
                            MaxValue="2025" runat="server">
                        </f:NumberBox>
                    </Editor>
                </f:RenderField>
                <f:RenderField Width="120px" Hidden="true" ColumnID="EntranceDate" DataField="EntranceDate" FieldType="Date"
                    Renderer="Date" RendererArgument="yyyy-MM-dd" HeaderText="入学日期">
                    <Editor>
                        <f:DatePicker ID="DatePicker1" Required="true" runat="server">
                        </f:DatePicker>
                    </Editor>
                </f:RenderField>
                <f:RenderCheckField Width="100px" ColumnID="AtSchool" DataField="AtSchool" HeaderText="是否在校" />
                <f:RenderField Width="100px" ColumnID="Major" DataField="Major"
                    ExpandUnusedSpace="true" HeaderText="所学专业">
                    <Editor>
                        <f:TextBox ID="tbxEditorMajor" Required="true" runat="server">
                        </f:TextBox>
                    </Editor>
                </f:RenderField>
                <f:LinkButtonField ColumnID="Delete" Width="80px" EnablePostBack="false"
                    Icon="Delete" />
            </Columns>
        </f:Grid>
        <br />
        注：新增一行数据，在[姓名]列输入字符 - 张，会有自动完成提示列表出现。
        <br />
        <br />
        <f:Button ID="Button2" runat="server" Text="保存数据" OnClick="Button2_Click">
        </f:Button>
        <br />
        <br />
        <f:Label ID="labResult" EncodeText="false" runat="server">
        </f:Label>

    </form>

    <script src="../res/jqueryuiautocomplete/jquery-ui.js" type="text/javascript"></script>
    <script type="text/javascript">
        var tbxEditorNameClientID = '<%= tbxEditorName.ClientID %>';
        var gridClientID = '<%= Grid1.ClientID %>';

        function renderGender(value) {
            return value == 1 ? '男' : '女';
        }

        F.ready(function () {

            var cache = {};


            F(tbxEditorNameClientID).el.find('input').autocomplete({
                source: function (request, response) {
                    var term = request.term;
                    if (term in cache) {
                        response(cache[term]);
                        return;
                    }

                    $.getJSON("gridsearch.ashx", request, function (data, status, xhr) {
                        cache[term] = data;
                        response(data);
                    });
                },
                select: function (event, ui) {
                    var grid = F(gridClientID);

                    // 当前选中的单元格（也就是正在编辑的单元格）
                    var selectedCell = grid.getSelectedCell();
                    var rowId = selectedCell[0];

                    grid.updateCellValue(rowId, {
                        'Name': ui.item.value,
                        'Gender': ui.item.gender,
                        'EntranceYear': ui.item.entranceYear,
                        'EntranceDate': ui.item.entranceDate,
                        'AtSchool': ui.item.atSchool,
                        'Major': ui.item.major
                    });

                    return false;
                }
            });


        });

    </script>
</body>
</html>
