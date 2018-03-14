using Infobasis.Api.Data;
using Infobasis.Api.InfobasisLog;
using Infobasis.Api.WebApiAuthentication;
using Infobasis.Data.DataAccess;
using Infobasis.Data.DataEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Security.Cryptography;
using System.Data.SqlClient;
using Infobasis.Api.Utils;

namespace Infobasis.Api.Controllers
{
    [RoutePrefix("api/users")]
    [LogAttribute("User API")]
    public class UsersController : ApiController
    {
        private string msg;
        private UnitOfWork _unitOfWork;
        private GenericRepository<User> _repository;

        public UsersController()
        {
            _unitOfWork = new UnitOfWork();
            _repository = _unitOfWork.Repository<User>();
        }

        [Route("signin")]
        [HttpPost]
        public IHttpActionResult SignIn([FromBody] UserSigninDTO user)
        {
            if (user == null)
                return BadRequest("Invalid Data");

            if (user.CompanyCode == null || user.CompanyCode == "")
                return BadRequest("公司代号不能为空！");

            if (user.UserName == null || user.UserName == "")
                return BadRequest("用户名不能为空！");

            if (user.Password == null || user.Password == "")
                return BadRequest("密码不能为空！");

            IInfobasisDataSource db = InfobasisDataSource.Create();
            int? companyID = db.ExecuteScalar("SELECT ID FROM SYtbCompany WHERE CompanyCode = @CompanyCode", user.CompanyCode) as int?;

            var existedUser = _repository.Get(includeProperties: "Company")
                .Where(u => u.Name == user.UserName && u.CompanyID == companyID)
                .FirstOrDefault();

            if (existedUser == null)
            {
                return BadRequest("用户或密码错误，请重新输入！");
            }

            string currentPasswordHash = existedUser.Password;
            if (!PasswordUtil.ComparePasswords(currentPasswordHash, user.Password))
            {
                updateUserInfo(existedUser, null, false);
                return BadRequest("用户或密码错误，请重新输入！");
            }

            if (!existedUser.Enabled)
            {
                updateUserInfo(existedUser, null, false);
                return BadRequest("该用户帐号已经被停用，请与系统管理员联系！");
            }

            /*
                        string authInfo = user.Name + ":" + user.Password; //user.Name + ":" + token;
                        byte[] byteValue = System.Text.Encoding.Default.GetBytes(authInfo);
                        string accessToken = Convert.ToBase64String(byteValue);
            */
            var payload = new Dictionary<string, object>()
                {
                    { "id", existedUser.ID },
                    { "companyID", existedUser.CompanyID },
                    { "userName", existedUser.Name}
                };
            var secretKey = WebApiApplication.SECRETKEY;
            string token = JWT.JsonWebToken.Encode(payload, secretKey, JWT.JwtHashAlgorithm.HS256);

            if (token == null)
                return BadRequest("Token获取失败, 请与系统管理员联系！");

            updateUserInfo(existedUser, token, true);

            var currentUser = new SqlParameter { ParameterName = "UserID", Value = existedUser.ID };
            var levelParam = new SqlParameter { ParameterName = "Level", Value = 1 };
            //var privileges = _repository.ExecuteStoredProcedureList<UserPermissionRolePrivilege>("EXEC usp_EasyHR_GetPermissionRolePrivilegeByUser", currentUser, levelParam);

            LoginResultDTO loginResult = buildUserInfoToClient(existedUser, token, null);

            return Ok<LoginResultDTO>(loginResult);
        }

        private bool updateUserInfo(User existedUser, string token, bool isLogon = true)
        {
            if (isLogon)
            {
                existedUser.LastLogonDate = DateTime.Now;
                existedUser.LogonCount = existedUser.LogonCount + 1;
                //if (string.IsNullOrEmpty(existedUser.Token)
                //    || (existedUser.TokenCreationDate != null && (DateTime.Now - (DateTime)existedUser.TokenCreationDate).TotalSeconds > TOKENEXPIREDSECONDS)
                //)
                existedUser.Token = token;
                existedUser.TokenCreationDate = DateTime.Now;

            }
            else
            {
                existedUser.LastFailedLogonDate = DateTime.Now;
                existedUser.FailedLogonCount = existedUser.FailedLogonCount + 1;
                //existedUser.Token = null; //不清空Token, 否则可以让用户登出
            }

            if (!_repository.Update(existedUser, out msg, true))
            {
                return false;
            }
            return true;
        }

        private LoginResultDTO buildUserInfoToClient(User existedUser, string token, IList<User> privileges)
        {
            LoginResultDTO loginResult = new LoginResultDTO();
            loginResult.ID = existedUser.ID;
            loginResult.AccessToken = token;
            loginResult.TokenCreationDate = existedUser.TokenCreationDate;
            loginResult.UserName = existedUser.Name;
            loginResult.CompanyID = existedUser.CompanyID;
            loginResult.DisplayName = existedUser.Name;
            loginResult.MustChangePassword = existedUser.MustChangePassword;
            loginResult.LastLogonDate = existedUser.LastLogonDate;
            loginResult.LogonCount = existedUser.LogonCount;
            loginResult.defaultPageSize = existedUser.DefaultPageSize;

            return loginResult;
        }
    }

    public class LoginResultDTO
    {
        public int ID { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string EmployeeName { get; set; }
        public string DisplayName { get; set; }
        public int CompanyID { get; set; }
        public bool MustChangePassword { get; set; }
        public string AccessToken { get; set; }
        public DateTime? TokenCreationDate { get; set; }
        public DateTime? LastLogonDate { get; set; }
        public int LogonCount { get; set; }
        public int defaultPageSize { get; set; }
        //public IList<UserPermissionRolePrivilege> UserPermissionRolePrivileges { get; set; }
    }

    public class UserIDsDTO
    {
        public int[] IDs { get; set; }
        public string Password { get; set; }
        public bool MustChangePassword { get; set; }
        public int AccountCreatedBy { get; set; } //0 - 用户名 1 - 手机号码
    }

    public class FindPasswordDTO
    {
        public string MobileNumber { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string VerifyCode { get; set; }
        public string FindPasswordToken { get; set; }
    }

    public class UserAssignRegisterDTO
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool MustChangePassword { get; set; }
    }

    public class UserRegisterDTO
    {
        public string UserName { get; set; }
        public string MobileNumber { get; set; }
        public string VerifyCode { get; set; }
        public string CompanyName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public bool IsClientAdmin { get; set; }
    }

    public class UserSigninDTO
    {
        public string CompanyCode { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class UserDTO
    {
        public int ID { get; set; }
        public string UserName { get; set; }
        public string EmployeeCode { get; set; }
        public DateTime? OnBoardDate { get; set; }
        public string Token { get; set; }
        public int LogonCount { get; set; }
        public DateTime? LastLogonDate { get; set; }
        public bool MustChangePassword { get; set; }
        public DateTime? CreationDate { get; set; }
        public int DefaultPageSize { get; set; }
        public string MobileNumber { get; set; }
        public string Department { get; set; }
        public string Job { get; set; }
        public int? HireStatus { get; set; }
    }
}
