using Infobasis.Api.Data;
using Infobasis.Data.DataEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Infobasis.Api.Controllers
{
    [RoutePrefix("api/home")]
    public class HomeController : BaseApiController
    {
        [Route("announcement")]
        [HttpGet]
        public IEnumerable<Announcement> listAnnouncement(int num)
        {
            int userID = UserInfo.GetCurrentUserID();
            int companyID = UserInfo.GetCurrentCompanyID();


            IQueryable<Infobasis.Data.DataEntity.Announcement> q = DB.Announcements;
            q = q.Where(item => item.CompanyID == companyID).OrderByDescending(item => item.PublishDate);
            if (num > 0)
                q = q.Take(num);

            return q;
        }
    }
}
