using Infobasis.Data.DataEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Infobasis.Api.Controllers
{
    [RoutePrefix("api/department")]
    public class DepartmentController : BaseApiController
    {
        [Route("")]
        [HttpGet]
        public IEnumerable<Department> listDepartments(bool includeAll = false)
        {
            IQueryable<Infobasis.Data.DataEntity.Department> q = DB.Departments;
            if (includeAll == false)
                q.Where(d => d.Enabled == true);

            return q;
        }
    }
}
