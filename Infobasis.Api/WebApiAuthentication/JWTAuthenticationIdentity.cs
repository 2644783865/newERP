using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace Infobasis.Api.WebApiAuthentication
{
    public class JWTAuthenticationIdentity : GenericIdentity
    {
        public JWTAuthenticationIdentity(string accessToken)
            : base(accessToken, "JWT")
        {
            this.AccessToken = accessToken;
        }

        public string AccessToken { get; set; }
        public int CompanyID { get; set; }
        public int UserID { get; set; }
    }
}