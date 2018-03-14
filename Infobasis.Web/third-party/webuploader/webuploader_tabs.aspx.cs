using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FineUIPro.Examples.thirdparty
{
    public partial class webuploader_tabs : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                
                BindGrid1();

                BindGrid2();
            }
            else
            {
                string arg = GetRequestEventArgument(); // 此函数所在文件：PageBase.cs

                if (arg == "RebindGrid1")
                {
                    BindGrid1();
                }
                else if (arg == "RebindGrid2")
                {
                    BindGrid2();
                }
            }
        }

        #region BindGrid

        private void BindGrid1()
        {
            Grid1.DataSource = GetSourceData("webuploader.webuploader_tabs.1");
            Grid1.DataBind();

        }

        private void BindGrid2()
        {
            Grid2.DataSource = GetSourceData("webuploader.webuploader_tabs.2");
            Grid2.DataBind();
        }

        #endregion

        #region Events


        protected void Grid1_RowCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                DeleteRow(e.RowID, "webuploader.webuploader_tabs.1");

                BindGrid1();
            }
        }

        protected void Grid2_RowCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                DeleteRow(e.RowID, "webuploader.webuploader_tabs.2");

                BindGrid2();
            }
        }
        #endregion

        #region GetSourceData

        // 模拟在服务器端保存数据
        // 特别注意：在真实的开发环境中，不要在Session放置大量数据，否则会严重影响服务器性能
        private JArray GetSourceData(string key)
        {
            if (Session[key] == null)
            {
                Session[key] = new JArray();
            }
            return (JArray)Session[key];
        }

        private void DeleteRow(string rowId, string key)
        {
            JArray source = GetSourceData(key);

            for (int i = 0, count = source.Count; i < count; i++)
            {
                JObject item = source[i] as JObject;

                if (item.Value<string>("id") == rowId)
                {

                    try
                    {
                        string savedName = item.Value<string>("savedName");
                        File.Delete(Server.MapPath("~/upload/" + savedName));
                    }
                    catch (Exception)
                    {
                        // 尝试删除物理文件失败，不做处理
                    }

                    source.RemoveAt(i);
                    break;
                }
            }

            Session[key] = source;
        }

        #endregion



    }
}
