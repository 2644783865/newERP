using Infobasis.Data.DataAccess;
using Infobasis.Data.DataEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;
using System.Web.Http.Controllers;
using System.Security.Principal;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Security.Cryptography;
using System.Security.Claims;
using System.Data;
using Infobasis.Api.Data;
using System.Web.Security;

namespace Infobasis.Api.WebApiAuthentication
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class AuthenticationFilter : AuthorizationFilterAttribute
    {
        bool Active = true;

        public AuthenticationFilter()
        { }

        /// <summary>
        /// Overriden constructor to allow explicit disabling of this
        /// filter's behavior. Pass false to disable (same as no filter
        /// but declarative)
        /// </summary>
        /// <param name="active"></param>
        public AuthenticationFilter(bool active)
        {
            Active = active;
        }


        /// <summary>
        /// Override to Web API filter method to handle Basic Auth check
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (Active)
            {
                if (Thread.CurrentPrincipal.Identity.IsAuthenticated)
                    return;

                if (actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Count > 0)   // 允许匿名访问
                {
                    return;
                }

                var identity = ParseAuthorizationHeader(actionContext);
                if (identity == null)
                {
                    Challenge(actionContext);
                    return;
                }

                if (!OnAuthorizeUser(identity, actionContext))
                {
                    Challenge(actionContext);
                    return;
                }

                var newIdentity = new ClaimsIdentity(identity);
                newIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, identity.CompanyID.ToString()));

                var principal = new GenericPrincipal(newIdentity, null);
                //var principal = new ClaimsPrincipal(newIdentity); 

                Thread.CurrentPrincipal = principal;

                // inside of ASP.NET this is required
                if (HttpContext.Current != null)
                    HttpContext.Current.User = principal;

                base.OnAuthorization(actionContext);
            }
        }

        /// <summary>
        /// Base implementation for user authentication - you probably will
        /// want to override this method for application specific logic.
        /// 
        /// The base implementation merely checks for username and password
        /// present and set the Thread principal.
        /// 
        /// Override this method if you want to customize Authentication
        /// and store user data as needed in a Thread Principle or other
        /// Request specific storage.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="actionContext"></param>
        /// <returns></returns>
        protected virtual bool OnAuthorizeUser(JWTAuthenticationIdentity identity, HttpActionContext actionContext)
        {
            if (actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Count > 0)   // 允许匿名访问
            {
                return true;
            }

            if (string.IsNullOrEmpty(identity.Name) || string.IsNullOrEmpty(identity.AccessToken))
                return false;

            string accessToken = identity.AccessToken;
            UserToken userToken = UserToken.ParseAccessToken(accessToken);
            if (userToken == null)
                return false;

            //UnitOfWork unitOfWork = new UnitOfWork();

            //var _repository = unitOfWork.Repository<User>();
            UserInfo userInfo = UserInfo.LogonAs(accessToken);
            //Infobasis.Api.Data.UserInfo.LogonAs(accessToken);
            //var user = _repository.Get(filter: item => item.ID == userToken.ID && item.CompanyID == userToken.CompanyID).FirstOrDefault();

            if (userInfo == null)
                return false;

            if (userInfo.AccessToken != identity.AccessToken)
                return false;

            if (userInfo.UserName != userToken.UserName)
                return false;

            if (!userInfo.Enabled)
                return false;

            if (userInfo.TokenCreationDate != null && userInfo.TokenCreationDate != DateTime.MinValue && (DateTime.Now - userInfo.TokenCreationDate).TotalSeconds > WebApiApplication.TOKENEXPIREDSECONDS)
                return false;

            identity.CompanyID = userInfo.CompanyID;
            identity.UserID = userInfo.UserID;
            identity.AccessToken = userInfo.AccessToken;

            return true;
        }

        /// <summary>
        /// Parses the Authorization header and creates user credentials
        /// </summary>
        /// <param name="actionContext"></param>
        protected virtual JWTAuthenticationIdentity ParseAuthorizationHeader(HttpActionContext actionContext)
        {
            string authHeader = null;
            var auth = actionContext.Request.Headers.Authorization;
            if (auth != null && auth.Scheme == "Bearer")
                authHeader = auth.Parameter;

            if (string.IsNullOrEmpty(authHeader))
                return null;

            /*
            byte[] byteValue = Convert.FromBase64String(authHeader);
            authHeader = System.Text.Encoding.Default.GetString(byteValue);
            //authHeader = Encoding.Default.GetString(Convert.FromBase64String(authHeader));

            var tokens = authHeader.Split(':');
            if (tokens.Length < 2)
                return null;
            */

            JWTAuthenticationIdentity identity = new JWTAuthenticationIdentity(authHeader);
            return identity;
        }

        /// <summary>
        /// Send the Authentication Challenge request
        /// </summary>
        /// <param name="message"></param>
        /// <param name="actionContext"></param>
        private void Challenge(HttpActionContext actionContext)
        {
            var host = actionContext.Request.RequestUri.DnsSafeHost;
            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
            //actionContext.Response.Headers.Add("WWW-Authenticate", string.Format("Basic realm=\"{0}\"", host));
        }
    }
}