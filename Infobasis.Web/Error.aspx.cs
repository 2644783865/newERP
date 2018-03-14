using Infobasis.Web.Data;
using Infobasis.Web.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Infobasis.Web
{
    public partial class Error : System.Web.UI.Page
    {

        protected string ErrorMessage;
        protected string UserErrorMessage = "非常抱歉，发生了一个错误。我们将通知系统管理员，很快就会处理好.";

#if DEBUG
        protected string DebugOrRelease = "Debug";
#else
		protected string DebugOrRelease = "Release";
#endif

        //=======================================================================
        HttpCompileException recurseForCompileError(Exception error)
        {
            try
            {
                while (error != null && !(error is HttpCompileException))
                    error = error.InnerException;
                return (HttpCompileException)error;
            }
            catch
            {
                return null;
            }
        }

        //=======================================================================
        protected void Page_Load(object sender, System.EventArgs e)
        {

            // If we're being requested by CompileAll.aspx then stop everything as we don't want to litter EventLog with errors.
            if (Page.Request.UrlReferrer != null &&
                StringUtil.EndsWithIgnoreCase(Page.Request.UrlReferrer.AbsolutePath, "CompileAll.aspx"))
                Response.End();

            // Don't present newly logged on user with an error page if they were given a dodgy initial URL
            bool referrerWasLoginPage = (Request.UrlReferrer != null && Request.UrlReferrer.Query.StartsWith("?ReturnUrl="));
            if (referrerWasLoginPage)
                Response.Redirect("~/Main.aspx", true);

            Response.ClearContent();
            Response.StatusCode = 500;

            // So that we don't Trace this actual Error page (we want to trace the page that caused the error)
            Trace.IsEnabled = false;

            // Pick up error (stored by Global.asax)
            Exception theError = (Exception)Context.Items["LastError"];

            // look for HttpCompileException
            HttpCompileException compileError = recurseForCompileError(theError);

            // Get the actual exception rather than the HttpUnhandledException "wrapper"
            if (theError is HttpUnhandledException)
                theError = theError.InnerException;

            if (compileError != null)
            {
                ErrorMessage = "Error in .ASPX code:\r\n";
                foreach (System.CodeDom.Compiler.CompilerError error in compileError.Results.Errors)
                    ErrorMessage += error + "\r\n";
            }
            else if (theError is UserException)
            {
                UserErrorMessage = theError.Message;
                ErrorMessage = theError.ToString();
                Response.StatusCode = 200; // This is not really an "error" in the code as far as we're concerned.
            }
            else if (theError is HttpException)
            {
                Response.StatusCode = ((HttpException)theError).GetHttpCode();
                ErrorMessage = theError.ToString();
            }
            else
            {
                ErrorMessage = theError.ToString();
            }

            if (theError.InnerException != null && theError.InnerException is HttpException)
            {
                HttpException httpError = (HttpException)theError.InnerException;
                int httpCode = httpError.GetHttpCode();
                Response.StatusCode = httpCode;
                if (httpCode == 401) //access denied
                    ClientAlert("Access denied: " + httpError.Message);
            }


#if DEBUG
            if (Debugger.IsAttached)
            {
                Regex fileAndLine = new Regex(@"^.*at (?<method>.*) in (?<file>[\w\\:\.]+):line (?<line>[0-9]+).*$",
                    RegexOptions.ExplicitCapture | RegexOptions.Multiline);


                Debugger.Log(9, Debugger.DefaultCategory, fileAndLine.Replace(ErrorMessage, "${file}(${line}):\n$0"));
                debugLabel.Visible = true;
            }
#endif

            // Remove some noise from stacktrace
            ErrorMessage = removeFilePathsFromStackTrace(ErrorMessage);

            // Write error to EventLog and email it
            LogAndEmailError(ErrorMessage);

            if (Global.DynamicTraceAllowed)
            {
                //loadTraceTableControl();
            }
            else
            {
                // Don't dump stack trace onto page
                ErrorMessage = "Extra error information in server Application EventLog (set IB.config DynamicTraceAllowed=True to display information).";

                // ...but don't hint at extra information in EventLog when there won't be
                if (theError is UserException) ErrorMessage = string.Empty;
            }

            DataBind();
        }


        public static void LogAndEmailError(string errorMessage)
        {
            string formattedError = formattedErrorMessage(errorMessage);

            LogUtils.Error(formattedError);

            string subject = HttpContext.Current.Request.Url.Host + Global.SiteRootPath + " - "
                + left(errorMessage, 100, "...").Replace('\r', ' ').Replace('\n', ' ');
            sendErrorEmail(subject, formattedError);
        }

        private static string removeFilePathsFromStackTrace(string stackTrace)
        {
            return Regex.Replace(stackTrace, @"(\s in \s)  (.*? \\)  ([^\\]*?:line \s\d+)", "$1$3", RegexOptions.IgnorePatternWhitespace);
        }


        //========================================================================================
        Table createHtmlTraceTable(DataTable traceTable)
        {
            return (Table)callPrivateStaticMethod(typeof(System.Web.Handlers.TraceHandler), "CreateTraceTable", traceTable);
        }


        //========================================================================================
        // Late bound method invocation.
        object callPrivateStaticMethod(Type type, string methodName, params object[] args)
        {
            try
            {
                MethodInfo method = type.GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Static);
                if (method != null)
                    return method.Invoke(null, args);
            }
            catch { }

            return null;
        }


        //========================================================================================
        static void sendErrorEmail(string subject, string formattedErrorMessage)
        {
            try
            {
                // If there's an error email address, send it
                string emailTo = Global.ErrorEmailAddress;
                if ((emailTo != null) && (emailTo.Length > 0))
                {
                    string emailFrom = Global.SystemEmailAddress;
                    // Send Email with error info in it

                    //Global.SendMail(
                    //    emailFrom,
                    //    emailTo,
                    //    subject,
                    //    formattedErrorMessage);
                }
            }
            catch (Exception error)
            {
                LogUtils.Error(error.ToString());
            }
        }

        //======================================================================================
        static string formattedErrorMessage(string errorMessage)
        {
            StringBuilder body = new StringBuilder();

            HttpRequest request = HttpContext.Current.Request;

            body.AppendFormat("URL: {0}\r\n\r\n", request.Url);
            body.Append("Decrypted URL:\r\n" + getAbsolutePath(request.Url) + "?");

            foreach (string queryKey in request.QueryString.Keys)
                body.Append(queryKey + "=" + request.QueryString[queryKey] + "&");

            if (request.UrlReferrer != null)
                body.AppendFormat("\r\nReferrer: {0}\r\n", request.UrlReferrer);

            if (request.HttpMethod == "POST")
            {
                body.AppendFormat("POST data:\r\n");
                foreach (string postKey in request.Form.Keys)
                    body.Append(" * " + postKey + " = \"" + left(request.Form[postKey], 100, '…') + "\"\r\n");
            }


            body.Append("\r\n----------------------------------------------------------\r\n");
            try
            {
                if (HttpContext.Current.User != null && HttpContext.Current.User.Identity != null)
                    body.AppendFormat("UserInfo: {0}.\r\n", UserInfo.Current);
            }
            catch { }
            body.AppendFormat("Date/Time: {0:dd MMM yyyy, hh:mm:ss tt}.\r\n", DateTime.Now);
            body.AppendFormat("Remote address: {0}\r\n", request.ServerVariables["REMOTE_ADDR"]);
            body.AppendFormat("InfoBasis.Web.dll version: {0}\r\n", Global.Version);
            body.Append("Request headers:\r\n");
            foreach (string name in request.Headers)
                body.Append("  " + name + ": " + request.Headers[name] + "\r\n");
            body.Append("----------------------------------------------------------\r\n");
            body.Append("Exception: \r\n" + errorMessage + "\r\n");
            body.Append("----------------------------------------------------------\r\n");

            return body.ToString();
        }

        //========================================================================================
        // Returns leftmost characters from a given string appending a character to indicate 
        // truncation if necessary.
        static string left(string str, int length, char truncChar)
        {
            str += "";
            if (str.Length <= length)
                return str;
            return str.Substring(0, length) + truncChar;
        }

        static string left(string str, int length, string truncString)
        {
            str += "";
            if (str.Length <= length)
                return str;
            return str.Substring(0, length) + truncString;
        }


        //========================================================================================
        // We only get here if there was actually another error on this page too!
        protected void Page_Error(object sender, System.EventArgs e)
        {

            Response.Write("<html><body style='background-color:#ffffff'>");
            Response.Write("<head></head>");
            Response.Write("<body>");


            string message = "非常抱歉，发生了一个错误。我们将通知系统管理员，很快就会处理好.";
            if (Server.GetLastError() is UserException)
                message = Server.GetLastError().Message;

            Response.Write(@"
<table border='0' width='400' height='auto' id='ErrorTable' style=""BORDER-RIGHT: buttonshadow thin solid; BORDER-TOP: buttonhighlight thin solid; MARGIN-TOP: 4em; BORDER-LEFT: buttonhighlight thin solid; BORDER-BOTTOM: buttonshadow thin solid; FONT-FAMILY: 'MS Shell Dlg 2'; BACKGROUND-COLOR: buttonface"" align='center'>
	<tr>
		<th colspan='2' align='left' style='background-color:ActiveCaption; COLOR:captiontext; padding:3px;'>错误提示</th>
	</tr>
	<tr>
		<td style='padding-left:1em'><img src='" + @"/res/images/errorInfo.jpg' style='width:60px' alt='' border='0'/></td>
		<td style='padding:1em'>" + message + @"</td>
	</tr>
	<tr>
		<td colspan='2' align='center'><input value='    OK    ' id='okBtn' type='button' onclick='window.history.back()' /></td>
	</tr>
	<tr>
		<td style='padding-left:0.5em; color:buttonshadow; font-size:0.9em' disabled='true'>v" + Global.Version + @"</td>
	</tr>
</table>
");

#if DEBUG
            Response.Write("<pre style='color:#888; background-color:#ffffff; font-size:120%'>");
            Response.Write(Server.GetLastError().ToString());
            Response.Write("</pre>");
#endif
            Response.Write("</body></html>");

            LogUtils.Error(Server.GetLastError().ToString());

            Server.ClearError();
        }

        //========================================================================================
        static string getAbsolutePath(Uri uri)
        {
            string absoluteUri = uri.AbsoluteUri;

            int queryPos = absoluteUri.IndexOf('?');
            if (queryPos == -1)
                return absoluteUri;

            return absoluteUri.Substring(0, queryPos);
        }

        public void ClientAlert(string message)
        {
            WriteClientScript("alert(\"" + StringUtil.JSEncode(message + String.Empty) + "\");");
        }

        //===========================================================================
        /// <summary>
        /// Sends arbitrary JavaScript to the client.
        /// </summary>
        /// <param name="script">JavaScript code.</param>
        public void WriteClientScript(string script)
        {
            TextWriter writer = Response.Output;
            writer.WriteLine("<script language='javascript' type='text/javascript'>");
            writer.WriteLine("<!--");
            writer.WriteLine(script);
            writer.WriteLine("//-->");
            writer.WriteLine("</script>");
        }

        public string HtmlEncode(object value)
        {
            if (value == null)
                return null;
            return StringUtil.HtmlEncode(value.ToString());
        }
    }
}