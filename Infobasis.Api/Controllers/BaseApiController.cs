using Infobasis.Api.Data;
using Infobasis.Api.WebApiAuthentication;
using Infobasis.Data;
using Infobasis.Data.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using Infobasis.Api.Utils;

namespace Infobasis.Api.Controllers
{
    [AuthenticationFilter(true)]
    [CompressContent]
    public class BaseApiController : ApiController
    {
        protected string msg;
        protected UnitOfWork unitOfWork;
        protected InfobasisContext DB; 

        public BaseApiController()
        {
            unitOfWork = new UnitOfWork();
            DB = new InfobasisContext();
        }

        protected IQueryable<T> AddPagination<T>(IQueryable<T> query, int page, int pageSize)
        {
            int totalCount = query.Count();
            int totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            var paginationHeader = new
            {
                TotalCount = totalCount,
                TotalPages = totalPages
            };

            System.Web.HttpContext.Current.Response.Headers.Add("X-Pagination",
                Newtonsoft.Json.JsonConvert.SerializeObject(paginationHeader));

            query = query
                    .Skip(pageSize * (page - 1))
                    .Take(pageSize);

            return query;
        }

        protected void ProcessEntityFieldPermission<T>(T entity, string entityCode)
        {
            int companyID = UserInfo.GetCurrentCompanyID();
            IInfobasisDataSource db = InfobasisDataSource.Create();
            DataTable dtFields = db.ExecuteTable("EXEC usp_EasyHR_GetFieldPermission @CompanyID, @EntityCode", companyID, entityCode);
            List<string> columns = dtFields.AsEnumerable().Select(r => Change.ToString(r["ColumnName"])).Distinct().ToList();

            Type type = entity.GetType();
            foreach (PropertyInfo pi in type.GetProperties())
            {
                string name = pi.Name;
                if (columns.Contains(name))
                    continue;

                Type valueType = pi.PropertyType;
                if (pi.CanWrite)
                {
                    if (valueType.IsGenericType && valueType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        // If it is NULLABLE, then get the underlying type. eg if "Nullable<int>" then this will return just "int"
                        //columnType = p.PropertyType.GetGenericArguments()[0];
                        pi.SetValue(entity, null, null);
                        continue;
                    }

                    switch (valueType.ToString())
                    {
                        case "System.Nullable":
                            pi.SetValue(entity, null, null);
                            break;
                        case "System.String":
                            pi.SetValue(entity, "", null);
                            break;
                        case "System.Boolean":
                            pi.SetValue(entity, null, null);
                            break;
                        case "System.Int32":
                            pi.SetValue(entity, 0, null);
                            break;
                        case "System.Decimal":
                            pi.SetValue(entity, 0, null);
                            break;
                        case "System.DateTime":
                            pi.SetValue(entity, DateTime.MinValue, null);
                            break;
                        default:
                            pi.SetValue(entity, null, null);
                            break;
                    }
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}
