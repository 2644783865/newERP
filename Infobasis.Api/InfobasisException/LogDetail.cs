using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Infobasis.Api.InfobasisException
{
    public class LogDetail
    {
        public int CompanyID { get; set; }
        public int UserID { get; set; }
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
        public string Navigator { get; set; }
        public string IP { get; set; }
        public string UserHostName { get; set; }
        public string UrlReferrer { get; set; }
        public string Browser { get; set; }
        public string Paramaters { get; set; }
        public string Headers { get; set; }
        public string AttrTitle { get; set; }
        public string ErrorMsg { get; set; }
        public string RequestUri { get; set; }
        public DateTime EnterTime { get; set; }
        public double CostTime { get; set; }
        public string Token { get; set; }
        public string ExecuteResult { get; set; }
        public string Remark { get; set; }
        public string UId { get; set; }

        public override string ToString()
        {
            return this.ActionName + " " + this.ControllerName + " " + this.RequestUri;
        }
    }
}