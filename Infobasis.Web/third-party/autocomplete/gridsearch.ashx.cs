using System;
using System.Collections.Generic;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;

namespace FineUIPro.Examples.autocomplete
{
    /// <summary>
    /// gridsearch 的摘要说明
    /// </summary>
    public class gridsearch : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            //System.Threading.Thread.Sleep(2000);

            String term = context.Request.QueryString["term"];
            if (!String.IsNullOrEmpty(term))
            {
                term = term.ToLower();

                JArray ja = new JArray();

                DataTable source = DataSourceUtil.GetDataTable2();
                DataTable result = source.Clone();

                foreach (DataRow row in source.Rows)
                {
                    string userName = row["Name"].ToString();
                    if (userName.Contains(term))
                    {
                        JObject jo = new JObject();
                        jo.Add("value", userName);
                        jo.Add("gender", Convert.ToInt32(row["Gender"]));
                        jo.Add("entranceYear", Convert.ToInt32(row["EntranceYear"]));
                        jo.Add("entranceDate", row["entranceDate"].ToString());
                        jo.Add("atSchool", Convert.ToBoolean(row["AtSchool"]));
                        jo.Add("major", row["Major"].ToString());

                        ja.Add(jo);
                    }
                }


                context.Response.ContentType = "text/plain";
                context.Response.Write(ja.ToString());
            }

        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}