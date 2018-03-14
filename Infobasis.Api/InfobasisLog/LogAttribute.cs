using Infobasis.Api.Data;
using Infobasis.Api.InfobasisException;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http.Filters;

namespace Infobasis.Api.InfobasisLog
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class LogAttribute : ActionFilterAttribute
    {
        private string _msg = string.Empty;
        private string _token = string.Empty;
        private string _remark = string.Empty;
        private static readonly log4net.ILog errorLoger = LogManager.GetLogger("Info");

        public LogAttribute() { }

        public LogAttribute(string msg)
        {
            this._msg = msg;
        }

        //http://www.cnblogs.com/shan333chao/p/5002054.html
        private static readonly string key = "enterTime";
        private const string UserToken = "Bearer";
        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            if (actionContext.Request.Method != HttpMethod.Options)
            {
                // 标记log
                var logAction = actionContext.ActionDescriptor.GetCustomAttributes<NoLogAttribute>();
                if (!logAction.Any())
                {
                    actionContext.Request.Properties[key] = DateTime.Now.ToBinary();
                    this._token = GetToken(actionContext, out this._remark);
                }
            }
            base.OnActionExecuting(actionContext);
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.Request.Method != HttpMethod.Options)
            {
                object beginTime = null;
                if (actionExecutedContext.Request.Properties.TryGetValue(key, out beginTime))
                {
                    DateTime time = DateTime.FromBinary(Convert.ToInt64(beginTime));
                    var request = HttpContext.Current.Request;
                    var logDetail = new LogDetail
                    {
                        CompanyID = UserInfo.GetCurrentCompanyID(),
                        UserID = UserInfo.GetCurrentUserID(),
                        //获取action名称
                        ActionName = actionExecutedContext.ActionContext.ActionDescriptor.ActionName,
                        //获取Controller 名称
                        ControllerName = actionExecutedContext.ActionContext.ActionDescriptor.ControllerDescriptor.ControllerName,
                        //获取action开始执行的时间
                        EnterTime = time,
                        //获取执行action的耗时
                        CostTime = (DateTime.Now - time).TotalMilliseconds,
                        Navigator = request.UserAgent,
                        Token = this._token,
                        //获取用户ID
                        UId = UserInfo.GetCurrentUserID().ToString(),
                        //获取访问的ip
                        IP = request.UserHostAddress,
                        UserHostName = request.UserHostName,
                        UrlReferrer = request.UrlReferrer != null ? request.UrlReferrer.AbsoluteUri : "",
                        Browser = request.Browser.Browser + " - " + request.Browser.Version + " - " + request.Browser.Type,
                        //获取request提交的参数
                        Paramaters = GetRequestValues(actionExecutedContext),
                        //获取response响应的结果
                        ExecuteResult = GetResponseValues(actionExecutedContext),
                        AttrTitle = this._msg,
                        Remark = this._remark,
                        RequestUri = request.Url.AbsoluteUri
                    };

                    string logMsg = Newtonsoft.Json.JsonConvert.SerializeObject(logDetail);
                    errorLoger.Info(logMsg);
                }
            }

            base.OnActionExecuted(actionExecutedContext);
        }

        private string GetToken(System.Web.Http.Controllers.HttpActionContext actionContext, out string msg)
        {
            if (actionContext.Request.Headers.Authorization == null)
            {
                msg = "没有登录";
                return "没有登录";
            }

            string authorization = actionContext.Request.Headers.Authorization.Parameter;
            HttpMethod type = actionContext.Request.Method;
            msg = "";
            var token = "";
            if (type == HttpMethod.Post)
            {
                if (!string.IsNullOrEmpty(authorization))
                {
                    token = authorization;
                }
                else
                {
                    token = "没有token";
                }

                if (string.IsNullOrEmpty(token))
                    msg = "匿名用户";
            }
            else if (type == HttpMethod.Get || type == HttpMethod.Put || type == HttpMethod.Delete)
            {
                if (authorization.Contains(UserToken))
                {
                    token = authorization;
                }
                else
                {
                    token = "没有token";
                    msg = "匿名用户";
                }
            }
            else if (type == HttpMethod.Options)
            {

            }
            else
            {
                throw new HttpException(404, "暂未开放除POST，GET之外的访问方式!");
            }
            return token;
        }
        /// <summary>
        /// 读取request 的提交内容
        /// </summary>
        /// <param name="actionExecutedContext"></param>
        /// <returns></returns>
        public string GetRequestValues(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext == null || actionExecutedContext.Request == null || actionExecutedContext.Request.Content == null)
                return "";

            if (actionExecutedContext.ActionContext.ActionDescriptor.ControllerDescriptor.ControllerName.Equals("Imports"))
                return "Import Data";

            Stream stream = actionExecutedContext.Request.Content.ReadAsStreamAsync().Result;
            Encoding encoding = Encoding.UTF8;
            /*
                这个StreamReader不能关闭，也不能dispose， 关了就傻逼了
                因为你关掉后，后面的管道  或拦截器就没办法读取了
            */
            var reader = new StreamReader(stream, encoding);
            string result = reader.ReadToEnd();
            /*
            这里也要注意：   stream.Position = 0;
            当你读取完之后必须把stream的位置设为开始
            因为request和response读取完以后Position到最后一个位置，交给下一个方法处理的时候就会读不到内容了。
            */
            stream.Position = 0;
            return result;
        }

        /// <summary>
        /// 读取action返回的result
        /// </summary>
        /// <param name="actionExecutedContext"></param>
        /// <returns></returns>
        public string GetResponseValues(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext == null || actionExecutedContext.Response == null || actionExecutedContext.Response.Content == null)
                return "";

            Stream stream = actionExecutedContext.Response.Content.ReadAsStreamAsync().Result;
            Encoding encoding = Encoding.UTF8;
            /*
            这个StreamReader不能关闭，也不能dispose， 关了就傻逼了
            因为你关掉后，后面的管道  或拦截器就没办法读取了
            */
            var reader = new StreamReader(stream, encoding);
            string result = reader.ReadToEnd();
            /*
            这里也要注意：   stream.Position = 0; 
            当你读取完之后必须把stream的位置设为开始
            因为request和response读取完以后Position到最后一个位置，交给下一个方法处理的时候就会读不到内容了。
            */
            stream.Position = 0;
            return result;
        }
    }
}