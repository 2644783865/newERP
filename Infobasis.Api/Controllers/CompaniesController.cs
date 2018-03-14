using Infobasis.Data.DataAccess;
using Infobasis.Data.DataEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Infobasis.Api.Controllers
{
    public class CompaniesController : BaseApiController
    {
        private GenericRepository<Company> _repository;
        public CompaniesController()
        {
            _repository = unitOfWork.Repository<Company>();
        }

        public IEnumerable<Company> GetAll(int page = 0, int pageSize = 10)
        {
            IQueryable<Company> query;

            query = _repository.Get().OrderBy(c => c.Name);
            if (page > 0)
            {
                query = AddPagination<Company>(query, page, pageSize);
            }

            return query.ToList();
        }
    }
}
