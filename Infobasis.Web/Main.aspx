<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Main.aspx.cs" Inherits="Infobasis.Web.Main" %>

<!DOCTYPE html>
<html>
<head id="Head1" runat="server">
    <title>创域国际家居</title>
    <link rel="shortcut icon" type="image/x-icon" href="favicon.ico" />
    <meta name="Title" content="创域国际家居" />
    <meta name="Description" content="创域国际家居" />
    <meta name="Keywords" content="创域国际家居" />
    <link type="text/css" rel="stylesheet" href="~/res/css/default.css" />
</head>
<body class="defaultpage">
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" AutoSizePanelID="Panel1" runat="server"></f:PageManager>
        <f:Panel ID="Panel1" Layout="Region" ShowBorder="false" ShowHeader="false" runat="server">
            <Items>
                <f:ContentPanel ID="topPanel" CssClass="topregion" RegionPosition="Top" ShowBorder="false" ShowHeader="false" EnableCollapse="true" runat="server">
                    <div id="header" class="ui-widget-header f-mainheader">
                        <table>
                            <tr>
                                <td>
                                    <f:Button runat="server" CssClass="icononlyaction" ID="btnHomePage" ToolTip="创域国际家居" IconAlign="Top" IconFont="Home"
                                        EnablePostBack="false" EnableDefaultState="false" EnableDefaultCorner="false"
                                        OnClientClick="window.open('./Main.aspx','_blank');">
                                    </f:Button>
                                    <a class="logo" href="./Main.aspx" title="创域国际家居首页" runat="server" id="linkCompanyTitle">创域国际家居
                                    </a>
                                </td>
                                <td style="text-align: right;">
                                    <f:Button runat="server" CssClass="icontopaction mobile" ID="Button1" Text="系统帮助" IconAlign="Top" IconFont="Question"
                                        EnablePostBack="false" EnableDefaultState="false" EnableDefaultCorner="false"
                                        OnClientClick="window.location.href='./Help/Index.aspx';">
                                    </f:Button>
                                    <f:Button ID="btnUserName" runat="server" CssClass="userpicaction" Text="刘程远" IconUrl="~/res/images/my_face_80.jpg" IconAlign="Left"
                                        EnablePostBack="false" EnableDefaultState="false" EnableDefaultCorner="false">
                                        <Menu ID="Menu1" runat="server">
                                            <f:MenuButton ID="MenuButton1" Text="个人信息" IconFont="User" EnablePostBack="false" runat="server">
                                                <Listeners>
                                                    <f:Listener Event="click" Handler="onUserProfileClick" />
                                                </Listeners>
                                            </f:MenuButton>
                                            <f:MenuButton ID="MenuButton2" Text="个人设置" IconFont="Cogs" EnablePostBack="false" runat="server">
                                                <Listeners>
                                                    <f:Listener Event="click" Handler="onUserSettingClick" />
                                                </Listeners>
                                            </f:MenuButton>
                                            <f:MenuSeparator ID="MenuSeparator1" runat="server"></f:MenuSeparator>
                                            <f:MenuHyperLink ID="LogoutMenuLink" runat="server" IconFont="SignOut" NavigateUrl=""
                                                Text="安全退出">
                                            </f:MenuHyperLink>
                                        </Menu>
                                    </f:Button>
                                </td>
                            </tr>
                        </table>
                    </div>
                </f:ContentPanel>
                <f:Panel ID="leftPanel" CssClass="leftregion"
                    RegionPosition="Left" RegionSplit="true" RegionSplitWidth="3px" RegionSplitIcon="false"
                    ShowBorder="true" Width="220px" ShowHeader="true" Title="&nbsp;"
                    EnableCollapse="false" Collapsed="false" Layout="Fit" runat="server">
                    <Tools>
                        <%--自定义展开折叠工具图标--%>
                        <f:Tool ID="leftPanelToolCollapse" runat="server" IconFont="ChevronCircleLeft" EnablePostBack="false">
                            <Listeners>
                                <f:Listener Event="click" Handler="onLeftPanelToolCollapseClick" />
                            </Listeners>
                        </f:Tool>
                        <f:Tool ID="leftPanelToolGear" runat="server" IconFont="Gear" EnablePostBack="false">
                            <Menu runat="server" ID="menuSettings">
                                <f:MenuButton ID="btnExpandAll" Text="展开菜单" EnablePostBack="false" runat="server">
                                    <Listeners>
                                        <f:Listener Event="click" Handler="onExpandAllClick" />
                                    </Listeners>
                                </f:MenuButton>
                                <f:MenuButton ID="btnCollapseAll" Text="折叠菜单" EnablePostBack="false" runat="server">
                                    <Listeners>
                                        <f:Listener Event="click" Handler="onCollapseAllClick" />
                                    </Listeners>
                                </f:MenuButton>
                                <f:MenuButton ID="btnThemeSelect" Text="主题样式" EnablePostBack="false" runat="server">
                                    <Listeners>
                                        <f:Listener Event="click" Handler="onThemeSelectClick" />
                                    </Listeners>
                                </f:MenuButton>
                                <f:MenuSeparator ID="MenuSeparator2" runat="server">
                                </f:MenuSeparator>
                                <f:MenuButton EnablePostBack="false" Text="菜单样式" ID="MenuStyle" runat="server">
                                    <Menu ID="Menu3" runat="server">
                                        <Items>
                                            <f:MenuCheckBox Text="智能树菜单" ID="MenuStyleTree" AttributeDataTag="tree" Checked="true" GroupName="MenuStyle" runat="server">
                                            </f:MenuCheckBox>
                                            <f:MenuCheckBox Text="智能树菜单（默认折叠）" ID="MenuStyleMiniModeTree" AttributeDataTag="tree_minimode" GroupName="MenuStyle" runat="server">
                                            </f:MenuCheckBox>
                                            <f:MenuCheckBox Text="树菜单" ID="MenuStylePlainTree" AttributeDataTag="plaintree" GroupName="MenuStyle" runat="server">
                                            </f:MenuCheckBox>
                                            <f:MenuCheckBox Text="手风琴+树菜单" ID="MenuStyleAccordion" AttributeDataTag="accordion" GroupName="MenuStyle" runat="server">
                                            </f:MenuCheckBox>
                                        </Items>
                                        <Listeners>
                                            <f:Listener Event="checkchange" Handler="onMenuStyleCheckChange" />
                                        </Listeners>
                                    </Menu>
                                </f:MenuButton>
                            </Menu>
                        </f:Tool>
                    </Tools>
                    <Toolbars>
                        <f:Toolbar ID="leftPanelBottomToolbar" Position="Bottom" runat="server" Layout="Fit">
                            <Items>
                                <f:TwinTriggerBox ID="ttbxSearch" ShowLabel="false" Trigger1Icon="Clear" ShowTrigger1="False" EmptyText="搜索菜单" Trigger2Icon="Search"
                                    EnableTrigger1PostBack="false" EnableTrigger2PostBack="false" runat="server" Width="248px">
                                    <Listeners>
                                        <f:Listener Event="trigger1click" Handler="onSearchTrigger1Click" />
                                        <f:Listener Event="trigger2click" Handler="onSearchTrigger2Click" />
                                    </Listeners>
                                </f:TwinTriggerBox>
                            </Items>
                        </f:Toolbar>
                    </Toolbars>
                </f:Panel>
                <f:TabStrip ID="mainTabStrip" AutoScroll="true" CssClass="centerregion" RegionPosition="Center" ShowBorder="true" EnableTabCloseMenu="true" runat="server">
                    <Tabs>
                        <f:Tab ID="Tab1" Title="首页" IconFont="Home" EnableIFrame="true" IFrameUrl="~/Home2.aspx" runat="server" AutoScroll="true">
                        </f:Tab>
                    </Tabs>
                    <Listeners>
                        <f:Listener Event="tabchange" Handler="onTabChange" />
                    </Listeners>
                    <Tools>
                        <f:Tool runat="server" EnablePostBack="false" IconFont="Refresh" MarginRight="5" CssClass="tabtool" ToolTip="刷新本页" ID="toolRefresh">
                            <Listeners>
                                <f:Listener Event="click" Handler="onToolRefreshClick" />
                            </Listeners>
                        </f:Tool>
                        <f:Tool runat="server" EnablePostBack="false" IconFont="Share" MarginRight="5" CssClass="tabtool" ToolTip="在新标签页中打开" ID="toolNewWindow">
                            <Listeners>
                                <f:Listener Event="click" Handler="onToolNewWindowClick" />
                            </Listeners>
                        </f:Tool>
                        <f:Tool runat="server" EnablePostBack="false" IconFont="Expand" CssClass="tabtool" ToolTip="最大化" ID="toolMaximize">
                            <Listeners>
                                <f:Listener Event="click" Handler="onToolMaximizeClick" />
                            </Listeners>
                        </f:Tool>
                    </Tools>
                </f:TabStrip>
                <f:ContentPanel ID="bottomPanel" CssClass="bottomregion" RegionPosition="Bottom" ShowBorder="false" ShowHeader="false" EnableCollapse="false" runat="server">
                    <table class="bottomtable ui-widget-header f-mainheader">
                        <tr>
                            <td style="width: 260px;">&nbsp;版本：<a target="_blank" href="http://fineui.com/version_pro">v<asp:Literal runat="server" ID="litVersion"></asp:Literal></a>
                                &nbsp;&nbsp;</td>
                            <td style="text-align: center;">Copyright &copy; 2008-2017 创域智慧科技（上海）有限公司</td>
                            <td style="width: 260px; text-align: right;">在线人数：<asp:Literal runat="server" ID="litOnlineUserCount"></asp:Literal>&nbsp;</td>
                        </tr>
                    </table>
                </f:ContentPanel>
            </Items>
        </f:Panel>
        <f:Window ID="windowThemeRoller" Title="主题仓库" Hidden="true" EnableIFrame="true" IFrameUrl="./Common/themes.aspx" ClearIFrameAfterClose="false"
            runat="server" IsModal="true" Width="1020px" Height="600px" EnableClose="true"
            EnableMaximize="true" EnableResize="true">
        </f:Window>
        <f:Window ID="windowLoadingSelector" Title="帮助" Hidden="true" EnableIFrame="true" IFrameUrl="./Help/Index.aspx" ClearIFrameAfterClose="false"
            runat="server" IsModal="true" Width="1000px" Height="600px" EnableClose="true"
            EnableMaximize="true" EnableResize="true">
        </f:Window>

        <asp:XmlDataSource ID="XmlDataSource1" runat="server" EnableCaching="false" DataFile="~/Common/menu.xml"></asp:XmlDataSource>

    </form>
    <script type="text/javascript" src="/res/js/default.js"></script>
    <script>

        var toolRefreshClientID = '<%= toolRefresh.ClientID %>';
        var toolNewWindowClientID = '<%= toolNewWindow.ClientID %>';
        var mainTabStripClientID = '<%= mainTabStrip.ClientID %>';

        var windowThemeRollerClientID = '<%= windowThemeRoller.ClientID %>';

        var MenuStyleClientID = '<%= MenuStyle.ClientID %>';

        var topPanelClientID = '<%= topPanel.ClientID %>';

        var leftPanelClientID = '<%= leftPanel.ClientID %>';
        var leftPanelToolGearClientID = '<%= leftPanelToolGear.ClientID %>';
        var leftPanelBottomToolbarClientID = '<%= leftPanelBottomToolbar.ClientID %>';
        var leftPanelToolCollapseClientID = '<%= leftPanelToolCollapse.ClientID %>';
        var tab1ClientID = '<%= Tab1.ClientID %>';

        // 点击主题仓库
        function onThemeSelectClick(event) {
            var windowThemeRoller = F(windowThemeRollerClientID);
            windowThemeRoller.show();
        }

        // 点击下一个主题
        function onNextThemeClick(event) {
            var themes = [["default", "默认"], ["metro_blue", "Metro 蓝"], ["metro_dark_blue", "Metro 深蓝"],
                           ["metro_gray", "Metro Gray"], ["metro_green", "Metro Green"], ["metro_orange", "Metro Orange"],
                           ["black_tie", "Black Tie"], ["blitzer", "Blitzer"], ["cupertino", "Cupertino"], ["dark_hive", "Dark Hive"],
                           ["dot_luv", "Dot Luv"], ["eggplant", "Eggplant"], ["excite_bike", "Excite Bike"], ["flick", "Flick"],
                           ["hot_sneaks", "Hot Sneaks"], ["humanity", "Humanity"], ["le_frog", "Le Frog"], ["mint_choc", "Mint Choc"],
                           ["overcast", "Overcast"], ["pepper_grinder", "Pepper Grinder"], ["redmond", "Redmond"], ["smoothness", "Smoothness"],
                           ["south_street", "South Street"], ["start", "Start"], ["sunny", "Sunny"], ["swanky_purse", "Swanky Purse"],
                           ["trontastic", "Trontastic"], ["ui_darkness", "UI Darkness"], ["ui_lightness", "UI Lightness"], ["vader", "Vader"],
                           ["custom_default", "Custom Default"], ["bootstrap_pure", "Bootstrap Pure"]];

            var themeName = F.cookie('Theme_Pro').toLowerCase();
            if (!themeName) {
                themeName = 'default';
            }

            var themeIndex = 0, themeCount = themes.length;
            $.each(themes, function (index, item) {
                if (item[0] === themeName) {
                    themeIndex = index;
                    return false; // break
                }
            });
            themeIndex++;

            if (themeIndex === themeCount) {
                themeIndex = 0;
            }

            var nextTheme = themes[themeIndex];
            var themeName = nextTheme[0];
            var themeTitle = nextTheme[1];

            F.cookie('Theme_Pro', themeName, {
                expires: 100  // 单位：天
            });
            F.cookie('Theme_Pro_Title', themeTitle, {
                expires: 100  // 单位：天
            });

            top.window.location.reload();
        }

        // 展开左侧面板
        function expandLeftPanel() {
            var leftPanel = F(leftPanelClientID);

            var menuStyle = F.cookie('MenuStyle_Pro') || 'tree';
            if (menuStyle === 'tree' || menuStyle === 'tree_minimode') {
                // 获取左侧树控件实例
                var leftMenuTree = leftPanel.items[0];

                leftMenuTree.miniMode = false;
                // 重新加载树菜单
                leftMenuTree.loadData();


                F(leftPanelToolGearClientID).show();
                F(leftPanelBottomToolbarClientID).show();
                F(leftPanelToolCollapseClientID).setIconFont('chevron-circle-left');

                leftPanel.el.removeClass('minimodeinside');
                leftPanel.setWidth(220);
            } else {
                leftPanel.expand();
            }
        }


        // 展开左侧面板
        function collapseLeftPanel() {
            var leftPanel = F(leftPanelClientID);

            var menuStyle = F.cookie('MenuStyle_Pro') || 'tree';
            if (menuStyle === 'tree' || menuStyle === 'tree_minimode') {
                // 获取左侧树控件实例
                var leftMenuTree = leftPanel.items[0];

                leftMenuTree.miniMode = true;
                leftMenuTree.miniModePopWidth = 300;
                // 重新加载树菜单
                leftMenuTree.loadData();

                F(leftPanelToolGearClientID).hide();
                F(leftPanelBottomToolbarClientID).hide();
                F(leftPanelToolCollapseClientID).setIconFont('chevron-circle-right');

                leftPanel.el.addClass('minimodeinside');
                leftPanel.setWidth(50);
            } else {
                leftPanel.collapse();
            }

        }


        // 自定义展开折叠工具图标
        function onLeftPanelToolCollapseClick(event) {
            var leftPanel = F(leftPanelClientID);

            var menuStyle = F.cookie('MenuStyle_Pro') || 'tree';
            if (menuStyle === 'tree' || menuStyle === 'tree_minimode') {
                // 获取左侧树控件实例
                var leftMenuTree = leftPanel.items[0];

                // 设置 miniMode 模式
                if (leftMenuTree.miniMode) {
                    expandLeftPanel();
                } else {
                    collapseLeftPanel();
                }

                // 对左侧面板重新布局
                leftPanel.doLayout();
            } else {
                leftPanel.toggleCollapse();
            }
        }

        // 点击展开菜单
        function onExpandAllClick(event) {
            var leftPanel = F(leftPanelClientID);
            var firstChild = leftPanel.items[0];

            if (firstChild.isType('tree')) {
                // 左侧为树控件
                firstChild.expandAll();
            } else {
                // 左侧为树控件+手风琴控件
                var activePane = firstChild.getActivePane();
                if (activePane) {
                    activePane.items[0].expandAll();
                }
            }
        }

        // 点击折叠菜单
        function onCollapseAllClick(event) {
            var leftPanel = F(leftPanelClientID);
            var firstChild = leftPanel.items[0];

            if (firstChild.isType('tree')) {
                // 左侧为树控件
                firstChild.collapseAll();
            } else {
                // 左侧为树控件+手风琴控件
                var activePane = firstChild.getActivePane();
                if (activePane) {
                    activePane.items[0].collapseAll();
                }
            }
        }


        // 点击仅显示最新示例
        function onShowOnlyNewClick(event) {
            var checked = this.isChecked();
            if (checked) {
                F.cookie('ShowOnlyNew_Pro', checked, {
                    expires: 100 // 单位：天
                });
            } else {
                F.removeCookie('ShowOnlyNew_Pro');
            }
            top.window.location.reload();
        }


        function onSearchTrigger1Click(event) {
            F.removeCookie('SearchText_Pro');
            top.window.location.reload();
        }

        function onSearchTrigger2Click(event) {
            F.cookie('SearchText_Pro', this.getValue(), {
                expires: 100 // 单位：天
            });
            top.window.location.reload();
        }

        // 点击标题栏工具图标 - 刷新
        function onToolRefreshClick(event) {
            var mainTabStrip = F(mainTabStripClientID);

            var activeTab = mainTabStrip.getActiveTab();
            if (activeTab.iframe) {
                var iframeWnd = activeTab.getIFrameWindow();
                iframeWnd.location.reload();
            }
        }

        // 被其他调用 - 刷新
        function refreshWindow(queryString) {
            var mainTabStrip = F(mainTabStripClientID);

            var activeTab = mainTabStrip.getActiveTab();
            if (activeTab.iframe) {
                var iframeWnd = activeTab.getIFrameWindow();
                var url = iframeWnd.location.href;
                url = mergeQuerystring(url, queryString);
                //console.log(queryString);
                //console.log("url:" + url);
                iframeWnd.location.href = url;
                F.activeWindow.hide();
            }
        }

        // 点击标题栏工具图标 - 在新标签页中打开
        function onToolNewWindowClick(event) {
            var mainTabStrip = F(mainTabStripClientID);

            var activeTab = mainTabStrip.getActiveTab();
            if (activeTab.iframe) {
                var iframeUrl = activeTab.getIFrameUrl();
                iframeUrl = iframeUrl.replace(/\/mobile\/\?file=/ig, '/mobile/');
                window.open(iframeUrl, '_blank');
            }
        }

        // 点击标题栏工具图标 - 最大化
        function onToolMaximizeClick(event) {
            var topPanel = F(topPanelClientID);
            var leftPanel = F(leftPanelClientID);

            var currentTool = this;
            if (currentTool.iconFont.indexOf('expand') >= 0) {
                topPanel.collapse();
                currentTool.setIconFont('compress');

                collapseLeftPanel();
            } else {
                topPanel.expand();
                currentTool.setIconFont('expand');

                expandLeftPanel();
            }
        }


        // 添加示例标签页（通过href在树中查找）
        function addExampleTabByHref(href) {
            var leftPanel = F(leftPanelClientID);
            var firstChild = leftPanel.items[0];

            href = href.toLowerCase();

            // 在树数据中查找href对应的节点id
            function checkInsideTree(tree) {
                var found = false;
                tree.resolveNode(function (node) {
                    var resolveHref = node.href;
                    if (resolveHref) {
                        resolveHref = resolveHref.toLowerCase();
                        if (resolveHref.indexOf(href) >= 0) {

                            // 保证传入的id和点击树节点生成的id相同！！！
                            F.addMainTab(F(mainTabStripClientID), {
                                id: node.id,
                                iframeUrl: node.href,
                                title: node.text,
                                icon: node.icon,
                                iconFont: node.iconFont
                            });

                            found = true;
                            return false; // break
                        }
                    }
                });
                return found;
            }


            if (firstChild.isType('tree')) {
                // 左侧为树控件
                checkInsideTree(firstChild);
            } else {
                // 左侧为树控件+手风琴控件
                $.each(firstChild.items, function (index, accordionpane) {
                    if (checkInsideTree(accordionpane.items[0])) {
                        return false; // break
                    }
                });
            }
        }

        // 移除选中标签页
        function removeActiveTab() {
            var mainTabStrip = F(mainTabStripClientID);

            var activeTab = mainTabStrip.getActiveTab();
            activeTab.hide();
        }

        // 获取当前激活选项卡的ID
        function getActiveTabId() {
            var mainTabStrip = F(mainTabStripClientID);

            var activeTab = mainTabStrip.getActiveTab();
            if (activeTab) {
                return activeTab.id;
            }
            return '';
        }

        // 激活选项卡，并刷新其中的内容，示例：表格控件->杂项->在新标签页中打开（关闭后刷新父选项卡）
        function activeTabAndRefresh(tabId) {
            var mainTabStrip = F(mainTabStripClientID);
            var targetTab = mainTabStrip.getTab(tabId);

            if (targetTab) {
                mainTabStrip.activeTab(targetTab);
                targetTab.refreshIFrame();
            }
        }

        // 激活选项卡，并刷新其中的内容，示例：表格控件->杂项->在新标签页中打开（关闭后更新父选项卡中的表格）
        function activeTabAndUpdate(tabId, param1) {
            var mainTabStrip = F(mainTabStripClientID);
            var targetTab = mainTabStrip.getTab(tabId);

            if (targetTab) {
                mainTabStrip.activeTab(targetTab);
                targetTab.getIFrameWindow().updatePage(param1);
            }
        }

        // 通知框
        function notify(msg) {
            F.notify({
                message: msg,
                messageIcon: 'information',
                target: '_top',
                header: false,
                displayMilliseconds: 3 * 1000,
                positionX: 'center',
                positionY: 'center'
            });
        }

        // 点击菜单样式
        function onMenuStyleCheckChange(event, item, checked) {
            var menuStyle = item.getAttr('data-tag');

            F.cookie('MenuStyle_Pro', menuStyle, {
                expires: 100 // 单位：天
            });
            top.window.location.reload();
        }

        // 点击显示模式
        function onMenuModeCheckChange(event, item, checked) {
            var menuMode = item.getAttr('data-tag');

            F.cookie('MenuMode_Pro', menuMode, {
                expires: 100 // 单位：天
            });
            top.window.location.reload();
        }

        // 点击语言
        function onMenuLangCheckChange(event, item, checked) {
            var lang = item.getAttr('data-tag');

            F.cookie('Language_Pro', lang, {
                expires: 100 // 单位：天
            });
            top.window.location.reload();
        }

        // 添加标签页
        // id： 选项卡ID
        // iframeUrl: 选项卡IFrame地址 
        // title： 选项卡标题
        // icon： 选项卡图标
        // createToolbar： 创建选项卡前的回调函数（接受tabOptions参数）
        // refreshWhenExist： 添加选项卡时，如果选项卡已经存在，是否刷新内部IFrame
        // iconFont： 选项卡图标字体
        function addTab(tabOptions) {

            if (typeof (tabOptions) === 'string') {
                tabOptions = {
                    id: arguments[0],
                    iframeUrl: arguments[1],
                    title: arguments[2],
                    icon: arguments[3],
                    createToolbar: arguments[4],
                    refreshWhenExist: arguments[5],
                    iconFont: arguments[6]
                };
            }

            F.addMainTab(F(mainTabStripClientID), tabOptions);
        }

        function onUserProfileClick() {
            addTab('userProfile', '/Pages/User/UserProfile.aspx', '用户信息');
        }

        function onUserSettingClick() {
            addTab('userSetting', '/Pages/User/UserSetting.aspx', '用户设置');
        }

        function onTabChange(event, tab) {
            // 如果激活的是第一个选项卡，则重新加载其中的IFrame
            if (tab.id === tab1ClientID) {
                tab.refreshIFrame();
            }
        }

        F.ready(function () {

            var mainTabStrip = F(mainTabStripClientID);

            var leftPanel = F(leftPanelClientID);
            var treeMenu = leftPanel.items[0];



            // 初始化主框架中的树(或者Accordion+Tree)和选项卡互动，以及地址栏的更新
            // treeMenu： 主框架中的树控件实例，或者内嵌树控件的手风琴控件实例
            // mainTabStrip： 选项卡实例
            // updateHash: 切换Tab时，是否更新地址栏Hash值（默认值：true）
            // refreshWhenExist： 添加选项卡时，如果选项卡已经存在，是否刷新内部IFrame（默认值：false）
            // refreshWhenTabChange: 切换选项卡时，是否刷新内部IFrame（默认值：false）
            // maxTabCount: 最大允许打开的选项卡数量
            // maxTabMessage: 超过最大允许打开选项卡数量时的提示信息
            F.initTreeTabStrip(treeMenu, mainTabStrip, {
                maxTabCount: 10,
                maxTabMessage: '请先关闭一些选项卡（最多允许打开 10 个）！'
            });

            var themeTitle = F.cookie('Theme_Pro_Title');
            var themeName = F.cookie('Theme_Pro');
            if (themeTitle) {
                F.removeCookie('Theme_Pro_Title');
                notify('主题更改为：' + themeTitle + '（' + themeName + '）');
            }

        });


    </script>

</body>
</html>