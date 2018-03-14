using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;

namespace Infobasis.Api.Data
{
    public class CompressContentAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(HttpActionExecutedContext context)
        {
            if (context == null || context.Response == null)
                return;

            var acceptEncoding = context.Response.RequestMessage.Headers.AcceptEncoding;
            if (acceptEncoding == null || acceptEncoding.FirstOrDefault() == null)
                return;
            var firstAcceptedEncoding = acceptEncoding.FirstOrDefault().Value;
            if (!firstAcceptedEncoding.Equals("gzip", StringComparison.InvariantCultureIgnoreCase)
            && !firstAcceptedEncoding.Equals("deflate", StringComparison.InvariantCultureIgnoreCase))
            {
                return;
            }
            if (context.Response.Content == null)
                return;

            // 标记不压缩
            var noCompressActionAction = context.ActionContext.ActionDescriptor.GetCustomAttributes<NoCompressContentAttribute>();
            if (noCompressActionAction.Any())
                return;
            else
            {
                var noCompressActionController = context.ActionContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes<NoCompressContentAttribute>();
                if (noCompressActionController.Any())
                    return;
            }

            context.Response.Content = new CompressContent(context.Response.Content, firstAcceptedEncoding);
        }
    }
}