using Infobasis.Data.DataAccess;
using Infobasis.Web.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Infobasis.Web.Data
{
    public class UserData
    {
        private UserData() { /* Not createable */ }


        //=======================================================================
        public static DataRow VerifyUser(string companyCode, string userName, string password)
        {
            return VerifyUser(companyCode, userName, password, false);
        }
        public static DataRow VerifyUser(string companyCode, string userName, string password, bool isGuest)
        {
            IInfobasisDataSource db = InfobasisDataSource.Create();
            int? companyID = db.ExecuteScalar("SELECT ID FROM SYtbCompany WHERE CompanyCode = @CompanyCode", companyCode) as int?;

            string currentPasswordHash = db.ExecuteScalar("SELECT Password FROM SYtbUser WHERE Name = @UserName AND CompanyID = @CompanyID", userName, companyID) as string;
            if (currentPasswordHash != null && PasswordUtil.ComparePasswords(currentPasswordHash, password))
            {
                string authSql = "EXEC usp_SY_AuthenticateLogon @companyID, @username, @password, @isGuest";
                DataRow userRow = db.ExecuteRow(authSql, companyID, userName, password, isGuest);
                return userRow;
            }
            return null;
        }

        //=======================================================================
        public static DataRow GetPerson(int userID)
        {
            string sql = "EXEC usp_SY_PersonDetailsByID @UserID";
            return InfobasisDataSource.Create().ExecuteRow(sql,
                userID);
        }

        //=======================================================================
        public static DataRow GetPersonByUserName(int companyID, string userName)
        {
            string sql = "EXEC usp_SY_PersonDetailsByUserName @companyID, @UserName";
            return InfobasisDataSource.Create().ExecuteRow(sql,
                companyID,
                userName);
        }

        //=======================================================================
        public static void SetLegalAcceptDate(int userID, DateTime legalAcceptDate)
        {
            string sql = @"UPDATE SYtbUser SET LegalAcceptDate=@acceptDate
							WHERE ID = @userID";

            InfobasisDataSource.Create().ExecuteNonQuery(sql,
                legalAcceptDate, userID);

        }

    }
}