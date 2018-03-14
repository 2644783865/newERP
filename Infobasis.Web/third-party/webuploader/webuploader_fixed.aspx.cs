using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FineUIPro.Examples.thirdparty
{
    public partial class webuploader_fixed : PageBase
    {
        private static readonly string KEY_FOR_DATASOURCE_SESSION = "webuploader.webuploader_fixed";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGrid();
            }
            else
            {
                string arg = GetRequestEventArgument(); // 此函数所在文件：PageBase.cs

                if (arg == "RebindGrid")
                {
                    BindGrid();
                }
                else if (arg.StartsWith("DeleteRow_"))
                {
                    string rowId = arg.Substring("DeleteRow_".Length);

                    DeleteRow(rowId);

                    BindGrid();
                }
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

        #endregion

        #region GetSourceData

        // 模拟在服务器端保存数据
        // 特别注意：在真实的开发环境中，不要在Session放置大量数据，否则会严重影响服务器性能
        private JArray GetSourceData()
        {
            if (Session[KEY_FOR_DATASOURCE_SESSION] == null)
            {
                JArray source = new JArray();
                JObject fileObj;

                fileObj = new JObject();
                fileObj.Add("id", "1_mulu");
                fileObj.Add("sectionname", "目录");
                fileObj.Add("name", "");
                fileObj.Add("type", "doc");
                fileObj.Add("savedName", "");
                fileObj.Add("size", "");
                source.Add(fileObj);

                fileObj = new JObject();
                fileObj.Add("id", "2_yinyan");
                fileObj.Add("sectionname", "引言");
                fileObj.Add("name", "");
                fileObj.Add("type", "doc");
                fileObj.Add("savedName", "");
                fileObj.Add("size", "");
                source.Add(fileObj);

                fileObj = new JObject();
                fileObj.Add("id", "3_zhengwen");
                fileObj.Add("sectionname", "正文");
                fileObj.Add("name", "");
                fileObj.Add("type", "doc");
                fileObj.Add("savedName", "");
                fileObj.Add("size", "");
                source.Add(fileObj);

                fileObj = new JObject();
                fileObj.Add("id", "4_cankao");
                fileObj.Add("sectionname", "参考文献");
                fileObj.Add("name", "");
                fileObj.Add("type", "doc");
                fileObj.Add("savedName", "");
                fileObj.Add("size", "");
                source.Add(fileObj);


                Session[KEY_FOR_DATASOURCE_SESSION] = source;
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

                    item["name"] = "";
                    item["type"] = "doc";
                    item["savedName"] = "";
                    item["size"] = null;
                    item["status"] = "";
                    break;
                }
            }

            Session[KEY_FOR_DATASOURCE_SESSION] = source;
        }

        #endregion



    }
}
