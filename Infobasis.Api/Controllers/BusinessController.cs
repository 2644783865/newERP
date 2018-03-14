using Infobasis.Api.Data;
using Infobasis.Data.DataEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;

namespace Infobasis.Api.Controllers
{
    [RoutePrefix("api/business")]
    public class BusinessController : BaseApiController
    {
        [Route("{year:int}/myGoals")]
        [HttpGet]
        public MyGoalDTO listMyGoals(int year)
        {
            int userID = UserInfo.GetCurrentUserID();
            int companyID = UserInfo.GetCurrentCompanyID();
            int thisMonth = DateTime.Now.Month;

            MyGoalDTO goalRtn = new MyGoalDTO();
            UserGoal goal = DB.UserGoals.Where(item => item.Year == year && item.UserID == userID && item.CompanyID == companyID).FirstOrDefault();
            if (goal != null)
            {
                var goalList = new decimal[] { goal.Month1.Value, goal.Month2.Value, goal.Month3.Value, goal.Month4.Value, goal.Month5.Value
                , goal.Month6.Value, goal.Month7.Value, goal.Month8.Value, goal.Month9.Value, goal.Month10.Value, goal.Month11.Value, goal.Month12.Value};
                var doneList = new decimal[] { 12, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

                decimal maxGoal = goalList.Max();
                decimal minGoal = goalList.Min();
                decimal totalGoal = goalList.Sum();
                decimal thisMonthGoal = goalList.ToArray()[thisMonth - 1];

                decimal maxDone = doneList.Max();
                decimal minDone = doneList.Min();
                decimal totalDone = doneList.Sum();
                decimal thisMonthDone = doneList.ToArray()[thisMonth - 1];

                goalRtn.GoalValues = goalList;
                goalRtn.DoneValues = doneList;
                goalRtn.MaxGoal = maxGoal;
                goalRtn.MinGoal = minGoal;
                goalRtn.TotalDone = totalDone;
                goalRtn.ThisMonthGoal = thisMonthGoal;

                goalRtn.MaxDone = maxDone;
                goalRtn.MinDone = minDone;
                goalRtn.TotalDone = totalDone;
                goalRtn.ThisMonthDone = thisMonthDone;
            }
           
            return goalRtn;
        }

        [Route("houseName")]
        [HttpGet]
        public IEnumerable<AutocompleteDTO> listAutocompleteHouseNames(string term)
        {
            int userID = UserInfo.GetCurrentUserID();
            int companyID = UserInfo.GetCurrentCompanyID();

            IQueryable<Infobasis.Data.DataEntity.HouseInfo> q = DB.HouseInfos;
            q = q.Where(item => item.Name.Contains(term) || item.NameSpellCode.Contains(term));

            var rtn = q.Select(item => new AutocompleteDTO()
            {
                Value = item.ID.ToString(),
                Label = item.Name
            });

            return rtn;
        }

        [Route("brand")]
        [HttpGet]
        public IEnumerable<AutocompleteDTO> listAutocompleteBrands(string term)
        {
            int userID = UserInfo.GetCurrentUserID();
            int companyID = UserInfo.GetCurrentCompanyID();

            IQueryable<Infobasis.Data.DataEntity.Brand> q = DB.Brands;
            q = q.Where(item => item.Name.Contains(term) || item.Code.Contains(term));

            var rtn = q.Select(item => new AutocompleteDTO()
            {
                Value = item.ID.ToString(),
                Label = item.Name
            });

            return rtn;
        }
    }

    public class MyGoalDTO
    {
        public decimal[] GoalValues { get; set; }
        public decimal[] DoneValues { get; set; }
        public decimal MaxGoal { get; set; }
        public decimal MinGoal { get; set; }
        public decimal MaxDone { get; set; }
        public decimal MinDone { get; set; }
        public decimal TotalGoal { get; set; }
        public decimal TotalDone { get; set; }
        public decimal ThisMonthGoal { get; set; }
        public decimal ThisMonthDone { get; set; }
    }
}
