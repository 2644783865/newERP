using Infobasis.Data.DataAccess;
using Infobasis.Web.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using Change = Infobasis.Web.Util.Change;

namespace Infobasis.Web.Data
{
    [Serializable] 
    public class UserInfo
    {
		public static bool CurrentlyRetrievingUserInfo
		{
			get
			{
				if (HttpContext.Current == null) return false;

				return HttpContext.Current.Items.Contains("Infobasis.Data.UserInfo.CurrentlyRetrieiving");
			}
			set
			{
				if (value)
				{
					HttpContext.Current.Items["Infobasis.Data.UserInfo.CurrentlyRetrieiving"] = true;
				}
				else
				{
					HttpContext.Current.Items.Remove("Infobasis.Data.UserInfo.CurrentlyRetrieiving");
				}
			}
		}


		#region Constructors
		//=====================================================================
		public UserInfo(DataRow person)
		{
			if (person==null)
				throw new ArgumentNullException("person");

            ID = Change.ToInt(person["ID"]);
            Name = person["Name"].ToString();
            ChineseName = person["ChineseName"].ToString();
            if (string.IsNullOrEmpty(ChineseName))
                ChineseName = Name;

            Email = person["Email"].ToString();
            CompanyID = Change.ToInt(person["CompanyID"]);
            CompanyCode = Change.ToString(person["CompanyCode"]);
            CompanyName = Change.ToString(person["CompanyName"]);
            CompanyLogo = Change.ToString(person["CompanyLogo"]);
			MustChangePassword = Change.ToBool(person["MustChangePassword"]);
			LegalAcceptDate = Change.ToDateTime(person["LegalAcceptDate"]);
            DefaultPageSize = Change.ToInt(person["DefaultPageSize"]);
            if (DefaultPageSize == 0)
                DefaultPageSize = 20;

            LogonCount = Change.ToInt(person["LogonCount"]);
            IsClientAdmin = Change.ToBool(person["IsClientAdmin"]);

            UserPortraitPath = Change.ToString(person["UserPortraitPath"]);
            if (string.IsNullOrEmpty(UserPortraitPath))
                UserPortraitPath = Global.Default_User_Portrait_Path;
		}
		#endregion

		#region Instance members
		public readonly string Name;
		public readonly string Email;
        public readonly string ChineseName;
        public readonly int ID;
		public readonly int CompanyID;
        public readonly string CompanyCode;
        public readonly string CompanyName;
        public readonly string CompanyLogo;
		public readonly bool MustChangePassword;
		public readonly DateTime LegalAcceptDate;
        public readonly int DefaultPageSize;
        public readonly int LogonCount;
        public readonly bool IsClientAdmin;
        public readonly string UserPortraitPath;


		//===========================================================================
		public bool IsSysAdmin
		{
			get
			{
				// Does user have entity code SYSS in their ACL?
                return false;
			}
		}
		
		//=======================================================================
		public override string ToString()
		{
            return string.Format("UserName:{0}; ChineseName:{1}; EmailAddress:{2}; CompanyID:{3}; ",
                Name, ChineseName, Email, CompanyID);
		}

		//=======================================================================
		public void ReInitialise()
		{
            LogonAs(CompanyID, Name);
		}

		#endregion

		#region Static members
		//=======================================================================
		static UserInfo()
		{
		}

		//=======================================================================
		public static bool IsLoggedOn
		{	
			get 
			{
				if(HttpContext.Current==null || HttpContext.Current.User == null || HttpContext.Current.User.Identity == null)
					return false;
				else
                    return HttpContext.Current.User.Identity.IsAuthenticated || (HttpContext.Current.Items["JustLoggedIn"] != null); 
			}
		}

		//=======================================================================
		public static UserInfo Current
		{
			get
			{
				if(IsLoggedOn)
				{
					// Is Session enabled?
					if(HttpContext.Current.Session == null)
						throw new ApplicationException("Either Session is not enabled on this page/handler or it's currently too early in the Request lifecyle (i.e. before AcquireRequestState). "+
							"Make sure the Page directive EnableSessionState='True' or have the handler implement the IRequiresSessionState interface.");

					// UserInfo already cached in session?
					if(HttpContext.Current.Session["IbUserInfo"] == null)
					{
						// No session, or timed out
						doAutoLogon();
					}
					UserInfo currentUser = (UserInfo)HttpContext.Current.Session["IbUserInfo"];
					return currentUser;
				}
				else
					return null;
			}
		}

		//=======================================================================
		static void doAutoLogon()
		{
            string companyUserNameKey = HttpContext.Current.User.Identity.Name;
            if (companyUserNameKey != null)
            {
                string[] companyAndUserName = companyUserNameKey.Split(',');
                LogonAs(Change.ToInt(companyAndUserName[0]), companyAndUserName[1]);
            }
            else
			    LogonAs(0, null );
		}


		//=======================================================================
        public static void LogonAs(int companyID, string userName)
        {
            if (userName == null)
                throw new ArgumentNullException("userName");
            if (userName.Length == 0)
                throw new ArgumentException("userName cannot be an empty string", "userName");

            System.Diagnostics.Debug.WriteLine("LogonAs(" + companyID.ToString() + "," + userName + ")");


            CurrentlyRetrievingUserInfo = true;

            try
            {

                // Look up person
                DataRow personRow = UserData.GetPersonByUserName(companyID, userName);

                // Not found?
				if (personRow == null)
				{
                
                }

				if (personRow.Table.Columns.Contains("Enabled")
                    && Change.ToBool(personRow["Enabled"]) == false)
				{
					RedirectToAccessDeniedPage("UserInfo-AccountStatus");
				}

				// Finally, create and cache in Session
				UserInfo userInfo = new UserInfo(personRow);
				HttpContext.Current.Session["IbUserInfo"] = userInfo;

				HttpContext.Current.Items["JustLoggedIn"] = true;

				System.Diagnostics.Debug.WriteLine("LogonAs complete: " + userInfo);
			}
			finally
			{
				CurrentlyRetrievingUserInfo = false;
			}
		}

        //=======================================================================
        public static UserInfo VerifyGuestUser(string companyCode, string userName)
		{
            return VerifyUser(companyCode, userName, null, true);
		}
        public static UserInfo VerifyUser(string companyCode, string userName, string password)
		{
            return VerifyUser(companyCode, userName, password, false);
		}
        public static UserInfo VerifyUser(string companyCode, string userName, string password, bool isGuest)
		{
			DataRow personRow;

            personRow = UserData.VerifyUser(companyCode, userName, password, isGuest);

			if(personRow != null) // Success
			{
				UserInfo userInfo = new UserInfo(personRow);
				return userInfo;
			}
			else // Failure
			{
				return null;
			}
		}
		#endregion

        //=======================================================================
        public static void RedirectToAccessDeniedPage(string reasonCode)
        {
            RedirectToAccessDeniedPage(reasonCode, true, "");
        }

        public static void RedirectToAccessDeniedPage(string reasonCode, bool useStaticPage, string queryString)
        {
            if (useStaticPage)
                HttpContext.Current.Response.Redirect("~/Local/AccessDenied.htm?" + reasonCode, true);
            else
            {
                Authentication.SignOut();
                HttpContext.Current.Session.Abandon();

                HRef page = new HRef(Global.SiteRootPath + "/AccessDenied.aspx");
                page["ReasonCode"] = reasonCode;
                page.Merge(queryString);
                HttpContext.Current.Response.Redirect(page.ToString(), true);
            }
        }

    }
}