using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Infobasis.Data.DataAccess
{
    internal class AsyncHelper
    {
        class TargetInfo
        {
            internal readonly Delegate Target;
            internal readonly object[] Args;

            internal TargetInfo(Delegate d, object[] args)
            {
                Target = d;
                Args = args;
            }
        }

        private static WaitCallback dynamicInvokeShim = new WaitCallback(DynamicInvokeShim);

        public static void FireAndForget(Delegate d, params object[] args)
        {
            ThreadPool.QueueUserWorkItem(dynamicInvokeShim, new TargetInfo(d, args));
        }

        static void DynamicInvokeShim(object o)
        {
            TargetInfo ti = (TargetInfo)o;
            try
            {
                ti.Target.DynamicInvoke(ti.Args);
            }
            catch (TargetInvocationException ex)
            {
                string message = "AsyncHelper: " + ex.InnerException.GetType().Name + " occurred calling " + ti.Target.Method.DeclaringType.Name + "." + ti.Target.Method.Name + "(\r\n";

                for (int i = 0; i < ti.Args.Length; i++)
                {
                    if (i > 0) message += ", \r\n";
                    message += "  " + i + ". \"" + ti.Args[i] + "\"";
                }
                message += "): \r\n\r\n" + ex.ToString();

                // log error
                //LogEvent(message, System.Diagnostics.EventLogEntryType.Error);
            }
        }
    }
}
