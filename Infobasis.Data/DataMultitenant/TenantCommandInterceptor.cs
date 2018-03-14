using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.Infrastructure.Interception;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;

namespace Infobasis.Data.DataMultitenant
{
    /// <summary>
    /// Custom implementation of <see cref="IDbCommandInterceptor"/>.
    /// In this class we set the actual value of the tenantId when querying the database as the command tree is cached  
    /// </summary>
    internal class TenantCommandInterceptor : IDbCommandInterceptor
    {
        private const string SECRETKEY = "B22A8B3c36E3xC4D3882xA30C5gEBDsE4Ae9BE";
        public void NonQueryExecuting(DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
        {
            SetTenantParameterValue(command);
        }

        public void NonQueryExecuted(DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
        {
        }

        public void ReaderExecuting(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext)
        {
            SetTenantParameterValue(command);
        }

        public void ReaderExecuted(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext)
        {
        }

        public void ScalarExecuting(DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
        {
            SetTenantParameterValue(command);
        }

        public void ScalarExecuted(DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
        {
        }

        private static int parseCompanyIDFromJWTToken(string accessToken)
        {
            if (string.IsNullOrEmpty(accessToken))
                return 0;

            try
            {
                var payload = JWT.JsonWebToken.DecodeToObject(accessToken, SECRETKEY) as IDictionary<string, object>;
                int companyID = (int)payload["companyID"];
                return companyID;
            }
            catch (JWT.SignatureVerificationException)
            {
                return 0;
            }
        }

        private static void SetTenantParameterValue(DbCommand command)
        {
            var identity = Thread.CurrentPrincipal.Identity;
            if ((command == null) || (command.Parameters.Count == 0) || identity == null)
            {
                return;
            }
            var userClaim = identity.Name;
            if (string.IsNullOrEmpty(userClaim))
            {
                var identityClaims = Thread.CurrentPrincipal.Identity as ClaimsIdentity;
                var userClaimFromClaims = identityClaims.Claims.SingleOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                if (userClaimFromClaims != null)
                    userClaim = userClaimFromClaims.Value;
            }
            if (userClaim != null)
            {
                int companyId = 0;
                string[] companyAndUserName = userClaim.Split(',');
                companyId = Infobasis.Data.DataAccess.Change.ToInt(companyAndUserName[0]);
                // Enumerate all command parameters and assign the correct value in the one we added inside query visitor
                foreach (DbParameter param in command.Parameters)
                {
                    if (param.ParameterName != TenantAwareAttribute.TenantIdFilterParameterName)
                        continue;
                    param.Value = companyId;
                }
            }
        }

    }
}