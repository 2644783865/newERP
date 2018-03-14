using Infobasis.Data.DataAccess;
using Infobasis.Web.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace Infobasis.Web.Handler
{
    /// <summary>
    /// Summary description for SearchHandler
    /// </summary>
    public class SearchHandler : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            String term = context.Request.QueryString["term"];
            int companyID = UserInfo.Current.CompanyID;

            IInfobasisDataSource db = InfobasisDataSource.Create();
            DataTable _t = db.ExecuteTable("SELECT [Name] FROM [SYtbUser] Where CompanyID = @companyID AND [Name] like '%' + @ke + '%'", companyID, term);

            DataRow[] list = new DataRow[_t.Rows.Count];
            _t.Rows.CopyTo(list, 0);

            var wapper = new
            {
                query = term,
                suggestions = (from row in list select row["Name"].ToString()).ToArray()
                //, data = new[] { "LR", "LY", "LI", "LT" } 
            };
            var suggestions = (from row in list select row["Name"].ToString()).ToArray();
            context.Response.Write(JsonConvert.SerializeObject(suggestions));
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