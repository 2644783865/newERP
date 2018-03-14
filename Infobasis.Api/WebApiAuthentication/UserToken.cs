using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Infobasis.Api.WebApiAuthentication
{
    public class UserToken
    {
        public int ID { get; set; }
        public int CompanyID { get; set; }
        public string UserName { get; set; }

        private UserToken()
        {
        }

        public static UserToken ParseAccessToken(string accessToken)
        {
            if (string.IsNullOrEmpty(accessToken))
                return null;

            try
            {
                var payload = JWT.JsonWebToken.DecodeToObject(accessToken, WebApiApplication.SECRETKEY) as IDictionary<string, object>;
                UserToken userToken = new UserToken();
                userToken.ID = (int)payload["id"];
                userToken.CompanyID = (int)payload["companyID"];
                userToken.UserName = (string)payload["userName"];
                return userToken;
            }
            catch (JWT.SignatureVerificationException)
            {
                return null;
            }
        }
    }
}