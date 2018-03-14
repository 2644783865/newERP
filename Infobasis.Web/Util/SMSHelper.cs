using Infobasis.Data.DataAccess;
using Infobasis.Data.DataEntity;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Top.Api;
using Top.Api.Request;
using Top.Api.Response;

namespace Infobasis.Web.Util
{
    public class SMSHelper
    {
        public static bool SendFindPasswordCode(string recNum, string company, string code, string extendMsg, string currentIP, out string msg)
        {
            JObject param = new JObject();
            param.Add("company", company.ToString());
            param.Add("code", code.ToString());
            param.Add("validMinus", (10).ToString());

            return SendSMS(recNum, SMSType.FindPassword, extendMsg, param, currentIP, out msg);
        }

        public static bool SendUserCreationPassword(string recNum, string company, string password, string extendMsg, string currentIP, out string msg)
        {
            JObject param = new JObject();
            param.Add("company", company.ToString());
            param.Add("password", password.ToString());

            return SendSMS(recNum, SMSType.UserCreation, extendMsg, param, currentIP, out msg);
        }

        public static bool SendRegistrationCode(string recNum, string username, string code, string extendMsg, string currentIP, out string msg)
        {
            JObject param = new JObject();
            param.Add("username", username.ToString());
            param.Add("code", code.ToString());
            param.Add("validMinus", (10).ToString());

            return SendSMS(recNum, SMSType.Registration, extendMsg, param, currentIP, out msg);
        }

        public static bool SendResetPassword(string recNum, string company, string password, string extendMsg, string currentIP, out string msg)
        {
            JObject param = new JObject();
            param.Add("company", company.ToString());
            param.Add("password", password.ToString());

            return SendSMS(recNum, SMSType.ResetPassword, extendMsg, param, currentIP, out msg);
        }

        private static bool _checkSMS_Business_Limit(string mobileNumber, string currentIP,
            MessageHistorySMSType messageHistorySMSType, out string msg)
        {
            UnitOfWork unitOfWork = new UnitOfWork();
            var _repository = unitOfWork.Repository<MessageHistory>();
            var lastEntity = _repository.Get(filter: msh => msh.MobileNumber == mobileNumber && msh.SMSType == messageHistorySMSType, orderBy: q => q.OrderByDescending(item => item.CreateDatetime.Value)).FirstOrDefault();
            //发送间隔小于60秒
            if (lastEntity != null)
            {
                if ((DateTime.Now - lastEntity.CreateDatetime.Value).TotalSeconds < 60)
                {
                    msg = "发送太频繁";
                    return false;
                }
            }

            //同一IP 1小时发送超过5条
            if (_repository.Get(filter: msh => msh.IP == currentIP
                && msh.CreateDatetime.Value.Hour == DateTime.Now.Hour
                && msh.SMSType == messageHistorySMSType).Count() > 5)
            {
                msg = "发送太频繁";
                return false;
            }

            //超过额度
            if (_repository.Get(filter: msh => msh.CreateDatetime.Value.Hour == DateTime.Now.Hour
                && msh.SMSType == messageHistorySMSType
                ).Count() > 100)
            {
                msg = "超过额度";
                return false;
            }

            msg = "";
            return true;

        }

        private static bool SendSMS(string recNum, SMSType smsType, string extendMsg, JObject param, string currentIP, out string msg)
        {
            if (string.IsNullOrWhiteSpace(recNum))
                throw new ArgumentException("手机号码不能为空");

            if (param == null)
                throw new ArgumentException("参数不能为空");

            //check first
            bool checkSMS_Limited = _checkSMS_Business_Limit(recNum, currentIP, MessageHistorySMSType.FindPassword, out msg);
            if (!checkSMS_Limited)
                return false;

            //测试模式
            if (Global.TEST_MODEL)
                return true;

            //禁用注册
            if (Global.DISABLE_REGISTRATION_SMS && smsType == SMSType.Registration)
                return true;

            ITopClient client = new DefaultTopClient(Global.SMS_SURL, Global.SMS_APPKEY, Global.SMS_SECRET);
            AlibabaAliqinFcSmsNumSendRequest req = new AlibabaAliqinFcSmsNumSendRequest();
            req.Extend = extendMsg;
            req.SmsType = "normal";
            req.SmsFreeSignName = "企赋HR";
            req.SmsParam = param.ToString();
            //req.SmsParam = "{\"code\":\"1234\",\"product\":\"alidayu\"}";
            req.RecNum = recNum;
            if (smsType == SMSType.Registration)
                req.SmsTemplateCode = "SMS_12490895";
            else if (smsType == SMSType.UserCreation)
                req.SmsTemplateCode = "SMS_12615251";
            else if (smsType == SMSType.ResetPassword)
                req.SmsTemplateCode = "SMS_12760018";
            else if (smsType == SMSType.FindPassword)
                req.SmsTemplateCode = "SMS_12715068";

            AlibabaAliqinFcSmsNumSendResponse rsp = client.Execute(req);
            if (rsp.IsError)
            {
                //Log
                msg = rsp.ErrMsg;
                return false;
            }

            msg = "";
            return true;
        }

        public static bool CreateSMSMessageHistory(string recNum, string userName, string code,
            string currentIP,
            MessageHistorySMSType smsType, out string msg)
        {
            UnitOfWork unitOfWork = new UnitOfWork();
            var repository = unitOfWork.Repository<MessageHistory>();
            MessageHistory entity = new MessageHistory();

            entity.Code = code; // Helper.EncryptPassword(userName, code.ToLower());
            entity.MobileNumber = recNum;
            entity.UserName = userName;
            if (smsType == MessageHistorySMSType.FindPassword)
                entity.Title = "找回密码";
            else if (smsType == MessageHistorySMSType.Registration)
                entity.Title = "用户注册";
            else if (smsType == MessageHistorySMSType.UserCreation)
                entity.Title = "用户密码";
            else if (smsType == MessageHistorySMSType.ResetPassword)
                entity.Title = "重置密码";

            entity.CreateDatetime = DateTime.Now;
            entity.IP = currentIP;
            entity.IsUsed = false;
            entity.MessageType = MessageHistoryType.SMS;
            entity.SMSType = smsType;
            if (!repository.Insert(entity, out msg, true))
            {
                return false;
            }

            return true;
        }
    }

    public enum SMSType
    {
        Registration = 0,
        UserCreation = 1,
        FindPassword = 2,
        ResetPassword = 3
    }
}