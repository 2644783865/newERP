using Infobasis.Api.WebApiAuthentication;
using Infobasis.Data.DataAccess;
using Infobasis.Data.DataEntity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Infobasis.Api.Utils;

namespace Infobasis.Api.Data
{
    [Serializable]
    public class UserInfo
    {
        public static bool CurrentlyRetrievingUserInfo
        {
            get
            {
                if (HttpContext.Current == null) return false;

                return HttpContext.Current.Items.Contains("EasyHr.Data.UserInfo.CurrentlyRetrieiving");
            }
            set
            {
                if (value)
                {
                    HttpContext.Current.Items["EasyHr.Data.UserInfo.CurrentlyRetrieiving"] = true;
                }
                else
                {
                    HttpContext.Current.Items.Remove("EasyHr.Data.UserInfo.CurrentlyRetrieiving");
                }
            }
        }

        public readonly int CompanyID;
        public readonly int UserID;
        public readonly string UserName;
        public readonly string ChineseName;
        public readonly string AccessToken;
        public readonly string Email;
        public readonly int DefaultPageSize;
        public readonly bool Enabled;
        public readonly DateTime TokenCreationDate;

        #region Constructors
        static UserInfo()
        {
        }

		//=====================================================================
        public UserInfo(DataRow person)
        {
            if (person == null)
                throw new ArgumentNullException("person");

            UserID = Change.ToInt(person["ID"]);
            UserName = person["Name"].ToString();
            ChineseName = person["ChineseName"].ToString();
            Email = person["Email"].ToString();
            CompanyID = Change.ToInt(person["CompanyID"]);
            DefaultPageSize = Change.ToInt(person["DefaultPageSize"]);
            AccessToken = person["Token"].ToString();
            Enabled = Change.ToBool(person["Enabled"]);
            TokenCreationDate = Change.ToDateTime(person["TokenCreationDate"]);
        }
        //=====================================================================
        public UserInfo(User person)
        {
            if (person == null)
                throw new ArgumentNullException("person");

            UserID = person.ID;
            UserName = person.Name;
            ChineseName = person.ChineseName;
            AccessToken = person.Token;
            Email = person.Email;
            CompanyID = person.CompanyID;
            DefaultPageSize = person.DefaultPageSize;
            Enabled = person.Enabled;
            TokenCreationDate = person.TokenCreationDate.Value;
        }
        #endregion

        public void ReInitialise()
        {
            LogonAs(UserName);
        }

        //=======================================================================
        public static bool IsLoggedOn
        {
            get
            {
                if (HttpContext.Current == null || HttpContext.Current.User == null || HttpContext.Current.User.Identity == null)
                    return false;
                else
                    return HttpContext.Current.User.Identity.IsAuthenticated || (HttpContext.Current.Items["JustLoggedIn"] != null);
            }
        }

        public static int GetCurrentUserID()
        {
            return UserInfo.IsLoggedOn ? UserInfo.Current.UserID : 0;
        }

        public static int GetCurrentCompanyID()
        {
            return UserInfo.IsLoggedOn ? UserInfo.Current.CompanyID : 0;
        }

        //=======================================================================
        public static UserInfo Current
        {
            get
            {
                if (IsLoggedOn)
                {
                    return LogonAs(HttpContext.Current.User.Identity.Name); //token
                }
                else
                    return null;
            }
        }

        //=======================================================================
        public static UserInfo LogonAs(string accessToken)
        {
            if (accessToken == null)
                throw new ArgumentNullException("accessToken");
            if (accessToken.Length == 0)
                throw new ArgumentException("accessToken cannot be an empty string", "accessToken");

            System.Diagnostics.Debug.WriteLine("LogonAs(" + accessToken + ")");


            CurrentlyRetrievingUserInfo = true;

            UserToken userToken = UserToken.ParseAccessToken(accessToken);
            if (userToken == null)
                throw new ApplicationException("验证JWT信息错误");

            try
            {

                // Look up person
                IInfobasisDataSource db = InfobasisDataSource.Create();
                DataRow userRow = db.ExecuteRow("SELECT * FROM SYtbUser WHERE ID = @ID", userToken.ID); 

                // Not found?
                if (userRow == null)
                {
                    throw new ApplicationException("找不到此用户");
                }

                // Finally, create and cache in Session
                UserInfo userInfo = new UserInfo(userRow);

                HttpContext.Current.Items["JustLoggedIn"] = true;

                System.Diagnostics.Debug.WriteLine("LogonAs complete: " + userInfo);

                return userInfo;
            }
            finally
            {
                CurrentlyRetrievingUserInfo = false;
            }
        }


    }
}