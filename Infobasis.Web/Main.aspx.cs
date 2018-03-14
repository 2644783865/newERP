using FineUIPro;
using Infobasis.Data.DataAccess;
using Infobasis.Data.DataEntity;
using Infobasis.Web.Data;
using Infobasis.Web.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace Infobasis.Web
{
    public partial class Main : PageBase
    {
        #region Page_Init

        private string _menuType = "menu";
        private bool _showOnlyNew = false;
        private bool _compactMode = false;
        private int _examplesCount = 0;
        private string _searchText = "";


        #region Page_Init


        protected void Page_Init(object sender, EventArgs e)
        {
            ////////////////////////////////////////////////////////////////
            string themeStr = Request.QueryString["theme"];
            string menuStr = Request.QueryString["menu"];
            if (!String.IsNullOrEmpty(themeStr) || !String.IsNullOrEmpty(menuStr))
            {
                if (!String.IsNullOrEmpty(themeStr))
                {
                    if (themeStr == "bootstrap1")
                    {
                        themeStr = "bootstrap_pure";
                    }
                    HttpCookie cookie = new HttpCookie("Theme_Pro", themeStr);
                    cookie.Expires = DateTime.Now.AddYears(1);
                    Response.Cookies.Add(cookie);
                }

                if (!String.IsNullOrEmpty(menuStr))
                {
                    HttpCookie cookie = new HttpCookie("MenuStyle_Pro", menuStr);
                    cookie.Expires = DateTime.Now.AddYears(1);
                    Response.Cookies.Add(cookie);
                }

                PageContext.Redirect("~/Main.aspx");
                return;
            }
            ////////////////////////////////////////////////////////////////



            // 从Cookie中读取 - 左侧菜单类型
            HttpCookie menuCookie = Request.Cookies["MenuStyle_Pro"];
            if (menuCookie != null)
            {
                _menuType = menuCookie.Value;
            }

            // 从Cookie中读取 - 是否仅显示最新示例
            HttpCookie menuShowOnlyNew = Request.Cookies["ShowOnlyNew_Pro"];
            if (menuShowOnlyNew != null)
            {
                _showOnlyNew = Convert.ToBoolean(menuShowOnlyNew.Value);
            }

            // 从Cookie中读取 - 是否启用紧凑模式
            HttpCookie menuCompactMode = Request.Cookies["EnableCompactMode_Pro"];
            if (menuCompactMode != null)
            {
                _compactMode = Convert.ToBoolean(menuCompactMode.Value);
            }


            // 从Cookie中读取 - 搜索文本
            HttpCookie searchText = Request.Cookies["SearchText_Pro"];
            if (searchText != null)
            {
                _searchText = HttpUtility.UrlDecode(searchText.Value);
            }




            if (_menuType == "accordion")
            {
                InitAccordionMenu();
            }
            else
            {
                InitTreeMenu();
            }

            if (_showOnlyNew)
            {
                leftPanel.Title = String.Format("菜单（{0}）", _examplesCount);
            }
            else
            {
                leftPanel.Title = String.Format("菜单（{0}）", _examplesCount);
            }
        }

        #endregion

        #region InitAccordionMenu

        private Accordion InitAccordionMenu()
        {
            Accordion accordionMenu = new Accordion();
            accordionMenu.ID = "accordionMenu";
            accordionMenu.EnableFill = false;
            accordionMenu.ShowBorder = false;
            accordionMenu.ShowHeader = false;
            leftPanel.Items.Add(accordionMenu);


            IInfobasisDataSource db = InfobasisDataSource.Create();
            XmlDocument xmlDoc = db.ExecuteXmlDoc("Tree", "EXEC usp_SY_GetModuleTreeXML @CompanyID, @UserID", 
                UserInfo.Current.CompanyID, UserInfo.Current.ID);

            XmlNodeList xmlNodes = xmlDoc.SelectNodes("/Tree/TreeNode");
            foreach (XmlNode xmlNode in xmlNodes)
            {
                if (xmlNode.HasChildNodes)
                {
                    string accordionPaneTitle = xmlNode.Attributes["Text"].Value;
                    string isNewHtml = GetIsNewHtml(xmlNode);
                    if (!String.IsNullOrEmpty(isNewHtml))
                    {
                        accordionPaneTitle += isNewHtml;
                    }

                    AccordionPane accordionPane = new AccordionPane();
                    accordionPane.Title = accordionPaneTitle;
                    accordionPane.Layout = Layout.Fit;
                    accordionPane.ShowBorder = false;

                    var accordionPaneIconAttr = xmlNode.Attributes["Icon"];
                    if (accordionPaneIconAttr != null)
                    {
                        accordionPane.Icon = (Icon)Enum.Parse(typeof(Icon), accordionPaneIconAttr.Value, true);
                    }

                    accordionMenu.Items.Add(accordionPane);

                    Tree innerTree = new Tree();
                    innerTree.ShowBorder = false;
                    innerTree.ShowHeader = false;
                    innerTree.EnableIcons = true;
                    innerTree.AutoScroll = true;
                    innerTree.EnableSingleClickExpand = true;
                    accordionPane.Items.Add(innerTree);

                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(String.Format("<?xml version=\"1.0\" encoding=\"utf-8\" ?><Tree>{0}</Tree>", xmlNode.InnerXml));
                    ResolveXmlDocument(doc);

                    // 绑定AccordionPane内部的树控件
                    innerTree.NodeDataBound += treeMenu_NodeDataBound;
                    innerTree.PreNodeDataBound += treeMenu_PreNodeDataBound;
                    innerTree.DataSource = doc;
                    innerTree.DataBind();

                }
            }

            return accordionMenu;
        }

        #endregion

        #region InitTreeMenu

        private Tree InitTreeMenu()
        {
            Tree treeMenu = new Tree();
            treeMenu.ID = "treeMenu";
            treeMenu.ShowBorder = false;
            treeMenu.ShowHeader = false;
            treeMenu.EnableIcons = true;
            treeMenu.AutoScroll = true;
            treeMenu.EnableSingleClickExpand = true;

            if (_menuType == "tree" || _menuType == "tree_minimode")
            {
                treeMenu.HideHScrollbar = true;
                treeMenu.ExpanderToRight = true;
                treeMenu.HeaderStyle = true;

                //leftPanel.RegionSplit = false;
                //leftPanel.CssStyle = "border-right-width:0;";


                if (_menuType == "tree_minimode")
                {
                    treeMenu.MiniMode = true;
                    treeMenu.MiniModePopWidth = Unit.Pixel(300);

                    leftPanelToolGear.Hidden = true;
                    leftPanelBottomToolbar.Hidden = true;
                    leftPanelToolCollapse.IconFont = IconFont.ChevronCircleRight;

                    leftPanel.Width = Unit.Pixel(50);
                    leftPanel.CssClass = "minimodeinside";
                }

            }

            leftPanel.Items.Add(treeMenu);

            IInfobasisDataSource db = InfobasisDataSource.Create();
            XmlDocument xmlDoc = db.ExecuteXmlDoc("Tree", "EXEC usp_SY_GetModuleTreeXML @CompanyID, @UserID", 
                UserInfo.Current.CompanyID, UserInfo.Current.ID);
            ResolveXmlDocument(xmlDoc);

            // 绑定 XML 数据源到树控件
            treeMenu.NodeDataBound += treeMenu_NodeDataBound;
            treeMenu.PreNodeDataBound += treeMenu_PreNodeDataBound;
            treeMenu.DataSource = xmlDoc;
            treeMenu.DataBind();

            return treeMenu;
        }

        #endregion

        #region ResolveXmlDocument

        private void ResolveXmlDocument(XmlDocument doc)
        {
            ResolveXmlDocument(doc, doc.DocumentElement.ChildNodes);
        }

        private int ResolveXmlDocument(XmlDocument doc, XmlNodeList nodes)
        {
            // nodes 中渲染到页面上的节点个数
            int nodeVisibleCount = 0;

            foreach (XmlNode node in nodes)
            {
                // Only process Xml elements (ignore comments, etc)
                if (node.NodeType == XmlNodeType.Element)
                {
                    XmlAttribute removedAttr;

                    // 是否叶子节点
                    bool isLeaf = node.ChildNodes.Count == 0;


                    // 所有过滤条件均是对叶子节点而言，而是否显示目录，要看是否存在叶子节点
                    if (isLeaf)
                    {
                        // 存在搜索关键字
                        if (!String.IsNullOrEmpty(_searchText))
                        {
                            XmlAttribute textAttr = node.Attributes["Text"];
                            if (textAttr != null)
                            {
                                if (!textAttr.Value.Contains(_searchText) && isLeaf)
                                {
                                    removedAttr = doc.CreateAttribute("Removed");
                                    removedAttr.Value = "true";

                                    node.Attributes.Append(removedAttr);
                                }
                            }
                        }

                        // 如果仅显示最新示例
                        if (_showOnlyNew)
                        {
                            XmlAttribute isNewAttr = node.Attributes["IsNew"];
                            if (isNewAttr == null)
                            {
                                removedAttr = doc.CreateAttribute("Removed");
                                removedAttr.Value = "true";

                                node.Attributes.Append(removedAttr);

                            }
                        }
                    }

                    // 存在子节点
                    if (!isLeaf)
                    {
                        // 递归
                        int childVisibleCount = ResolveXmlDocument(doc, node.ChildNodes);

                        if (childVisibleCount == 0)
                        {
                            removedAttr = doc.CreateAttribute("Removed");
                            removedAttr.Value = "true";

                            node.Attributes.Append(removedAttr);
                        }
                    }


                    removedAttr = node.Attributes["Removed"];
                    if (removedAttr == null)
                    {
                        nodeVisibleCount++;
                    }
                }
            }

            return nodeVisibleCount;
        }

        #endregion

        #region treeMenu_NodeDataBound treeMenu_PreNodeDataBound
        /// <summary>
        /// 树节点的绑定后事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeMenu_NodeDataBound(object sender, FineUIPro.TreeNodeEventArgs e)
        {
            // 是否叶子节点
            bool isLeaf = e.XmlNode.ChildNodes.Count == 0;

            string isNewHtml = GetIsNewHtml(e.XmlNode);
            if (!String.IsNullOrEmpty(isNewHtml))
            {
                e.Node.Text += isNewHtml;
            }

            if (isLeaf)
            {
                // 设置节点的提示信息
                e.Node.ToolTip = e.Node.Text;
            }

            // 如果仅显示最新示例，或者存在搜索文本
            if (_showOnlyNew || !String.IsNullOrEmpty(_searchText))
            {
                e.Node.Expanded = true;
            }
        }

        /// <summary>
        /// 树节点的预绑定事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeMenu_PreNodeDataBound(object sender, TreePreNodeEventArgs e)
        {
            // 是否叶子节点
            bool isLeaf = e.XmlNode.ChildNodes.Count == 0;

            XmlAttribute removedAttr = e.XmlNode.Attributes["Removed"];
            if (removedAttr != null)
            {
                e.Cancelled = true;
            }

            if (isLeaf && !e.Cancelled)
            {
                _examplesCount++;
            }

        }

        #endregion

        #region GetIsNewHtml

        private string GetIsNewHtml(XmlNode node)
        {
            string result = String.Empty;

            XmlAttribute isNewAttr = node.Attributes["IsNew"];
            if (isNewAttr != null)
            {
                if (Convert.ToBoolean(isNewAttr.Value))
                {
                    result = "&nbsp;<span class=\"isnew\">New!</span>";
                }
            }

            return result;
        }


        #endregion

        #endregion

        #region Page_Load

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Append anti-CSRF token to Logoff URL
                var url = new HRef(ResolveUrl("~/Default.aspx?action=logoff"));
                url["token"] = AntiCsrf.Current.SecureToken;
                LogoutMenuLink.NavigateUrl = url.ToString();

                InitSearchBox();
                InitMenuStyleButton();
                InitMenuModeButton();
                InitLangMenuButton();

                //cbxEnableCompactMode.Checked = _compactMode;

                litVersion.Text = FineUIPro.GlobalConfig.ProductVersion;

                InitOnlineUserCount();
                btnUserName.Text = UserInfo.Current.ChineseName;
                linkCompanyTitle.InnerText = UserInfo.Current.CompanyName;
                btnUserName.IconUrl = UserInfo.Current.UserPortraitPath;

                //Tab1.IFrameUrl = "~/Home2.aspx";
                //Tab1.RefreshIFrame();

            }
        }


        private void InitSearchBox()
        {
            if (!String.IsNullOrEmpty(_searchText))
            {
                ttbxSearch.Text = _searchText;
                ttbxSearch.ShowTrigger1 = true;
            }
        }





        private void InitMenuStyleButton()
        {
            string menuStyle = "tree";

            HttpCookie menuStyleCookie = Request.Cookies["MenuStyle_Pro"];
            if (menuStyleCookie != null)
            {
                menuStyle = menuStyleCookie.Value;
            }

            SetSelectedMenuItem(MenuStyle, menuStyle);
        }

        private void InitMenuModeButton()
        {
            string menuMode = "normal";

            HttpCookie menuModeCookie = Request.Cookies["MenuMode_Pro"];
            if (menuModeCookie != null)
            {
                menuMode = menuModeCookie.Value;
            }

            //SetSelectedMenuItem(MenuMode, menuMode);
        }


        private void InitLangMenuButton()
        {
            string language = "zh_CN";

            HttpCookie languageCookie = Request.Cookies["Language_Pro"];
            if (languageCookie != null)
            {
                language = languageCookie.Value;
            }
        }

        private void SetSelectedMenuItem(MenuButton menuButton, string selectedDataTag)
        {
            foreach (FineUIPro.MenuItem item in menuButton.Menu.Items)
            {
                MenuCheckBox checkBox = (item as MenuCheckBox);
                if (checkBox != null)
                {
                    checkBox.Checked = checkBox.AttributeDataTag == selectedDataTag;
                }
            }
        }

        #endregion

        private void InitOnlineUserCount()
        {
            litOnlineUserCount.Text = Application["OnlineUserCount"].ToString();
        }
    }
}