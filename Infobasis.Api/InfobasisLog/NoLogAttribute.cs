using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infobasis.Api.InfobasisLog
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true)]
    public class NoLogAttribute : Attribute
    {
    }
}
