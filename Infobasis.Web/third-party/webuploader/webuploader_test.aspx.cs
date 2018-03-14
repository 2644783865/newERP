using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FineUIPro.Examples.thirdparty
{
    public partial class webuploader_test : PageBase
    {
        private static readonly string KEY_FOR_DATASOURCE_SESSION = "webuploader.webuploader";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // 删除选中行
                btnDelete.OnClientClick = Grid1.GetNoSelectionAlertReference("请至少选择一项！");
                btnDelete.ConfirmText = String.Format("你确定要删除选中的&nbsp;<b><script>{0}</script></b>&nbsp;个文件吗？", Grid1.GetSelectedCountReference());
                string str = "<p><br /></p><p><br /></p><p><br /></p><p><span style=\"color:#337FE5;\"><strong>------------------------------------------------------------------------------------------------</strong></span></p>";
                str += "<p><strong>发件人：</strong><br /><strong>发送时间：</strong><br /><strong>收件人：</strong>; <br /><strong>主题：</strong></p>";
                ed_Content.Text = str;

                BindGrid();
            }
        }

        #region BindGrid

        private void BindGrid()
        {
            Grid1.DataSource = GetSourceData();
            Grid1.DataBind();
        }

        #endregion

        #region Events

        protected void PageManager1_CustomEvent(object sender, CustomEventArgs e)
        {
            if (e.EventArgument == "RebindGrid")
            {
                BindGrid();
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            foreach (string rowId in Grid1.SelectedRowIDArray)
            {
                DeleteRow(rowId);
            }

            BindGrid();
        }

        protected void Grid1_RowCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                DeleteRow(e.RowID);

                BindGrid();
            }
        }
        #endregion

        #region GetSourceData

        // 模拟在服务器端保存数据
        // 特别注意：在真实的开发环境中，不要在Session放置大量数据，否则会严重影响服务器性能
        private JArray GetSourceData()
        {
            if (Session[KEY_FOR_DATASOURCE_SESSION] == null)
            {
                Session[KEY_FOR_DATASOURCE_SESSION] = new JArray();
            }
            return (JArray)Session[KEY_FOR_DATASOURCE_SESSION];
        }

        private void DeleteRow(string rowId)
        {
            JArray source = GetSourceData();

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

            Session[KEY_FOR_DATASOURCE_SESSION] = source;
        }

        #endregion



    }
}
