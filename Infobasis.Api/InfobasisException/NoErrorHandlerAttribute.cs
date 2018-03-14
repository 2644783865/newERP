using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Infobasis.Api.InfobasisException
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true)]
    public class NoErrorHandlerAttribute : Attribute
    {
    }
}