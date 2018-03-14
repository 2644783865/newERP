using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Infobasis.Api.Controllers
{
    [RoutePrefix("api/employee")]
    public class EmployeeController : BaseApiController
    {
        [Route("ids")]
        [HttpGet]
        public IEnumerable<AutocompleteDTO> listAutocompleteEEs(string term, int? hireStatus = 0)
        {
            IQueryable<Infobasis.Data.DataEntity.User> q = DB.Users;
            q = q.Where(item => item.ChineseName.Contains(term) || item.EnglishName.Contains(term)
                || item.EmployeeSpellCode.Contains(term));

            q = q.Where(u => u.HireStatus == (hireStatus.HasValue ? hireStatus.Value : 0));

            var rtn = q.Select(item => new AutocompleteDTO() {
                Value = item.ID.ToString(),
                Label = item.ChineseName
            });

            return rtn;
        }
    }

    public class AutocompleteDTO
    {
        public string Value { get; set; }
        public string Label { get; set; }
    }
}
