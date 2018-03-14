using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Infobasis.Web.Util
{
    /// <summary>
    /// Summary description for TraceUtil.
    /// </summary>
    public class TraceUtil
    {
        private TraceUtil() { }

        //=========================================================================================
        /// <summary>
        /// Writes the calling type and method name to the Trace.
        /// </summary>
        [Conditional("TRACE")]
        public static void Trace()
        {
            traceInternal(null, true);
        }

        //=========================================================================================
        /// <summary>
        /// Writes a message with the calling type and method name to the Trace.
        /// </summary>
        /// <param name="message">The message to write</param>
        [Conditional("TRACE")]
        public static void Trace(object message)
        {
            traceInternal(message, true);
        }

        public static void Trace(object message, bool isWarning)
        {
            traceInternal(message, isWarning);
        }

        //=========================================================================================
        // Does the work of the Trace() methods
        private static void traceInternal(object message, bool isWarning)
        {
            if (HttpContext.Current.Trace.IsEnabled)
            {
                string methodName = "Unknown";

                StackFrame caller = new StackFrame(2); // only works when debugging information is present
                if (caller != null)
                {
                    MethodBase method = caller.GetMethod();
                    if (method != null)
                    {
                        Type type = method.DeclaringType;
                        methodName = type.BaseType.Name + ": " + type.Name + "." + method.Name + "()";
                    }
                }
                if (isWarning)
                {
                    HttpContext.Current.Trace.Warn(methodName, message + string.Empty);
                }
                else
                {
                    HttpContext.Current.Trace.Write(methodName, message + string.Empty);
                }
            }
        }

    }

    public class CumulativeTraceTimer : IDisposable
    {
        private Stopwatch _stopwatch;
        private long _startTicks;
        private string _operation;
        private string _counterName;
        private bool _logOnCompletion;
        private List<Stopwatch> _runningWatches = new List<Stopwatch>();

        private static int depth
        {
            get
            {
                if (HttpContext.Current.Items["_tracetimerdepth_"] == null)
                {
                    return 0;
                }

                return (int)HttpContext.Current.Items["_tracetimerdepth_"];
            }
            set
            {
                if (value < 0) value = 0;
                HttpContext.Current.Items["_tracetimerdepth_"] = value;
            }
        }

        private static Dictionary<string, Stopwatch> getTimers()
        {
            string contextItemKey = "_tracetimers_";
            Dictionary<string, Stopwatch> timers = (Dictionary<string, Stopwatch>)HttpContext.Current.Items[contextItemKey];

            if (timers == null)
            {
                timers = new Dictionary<string, Stopwatch>();
                HttpContext.Current.Items[contextItemKey] = timers;
            }
            return timers;
        }

        public CumulativeTraceTimer(string operation, string counterName)
            : this(operation, counterName, true)
        {
        }

        public CumulativeTraceTimer(string operation, string counterName, bool logOnCompletion)
        {
            _init(operation, counterName, logOnCompletion); // optimises out to a no-op if TRACE is off
        }


        [Conditional("PROFILE")]
        private void _init(string operation, string counterName, bool logOnCompletion)
        {
            if (HttpContext.Current == null) throw new InvalidOperationException();

            _operation = operation;
            _counterName = counterName;
            _logOnCompletion = logOnCompletion;

            Dictionary<string, Stopwatch> timers = getTimers();

            foreach (string timer in timers.Keys)
            {
                if (timers[timer].IsRunning) // should only be one, but we need to be sure.
                {
                    timers[timer].Stop();
                    _runningWatches.Add(timers[timer]);
                }
            }

            depth++;

            if (!timers.ContainsKey(counterName))
            {
                timers[counterName] = new Stopwatch();
            }

            _stopwatch = timers[counterName];

            _startTicks = _stopwatch.Elapsed.Ticks;


            _stopwatch.Start();
        }

        #region IDisposable Members

        public void Dispose()
        {
            _tidy(); // optimises out to a no-op if TRACE is off
        }

        #endregion
        [Conditional("PROFILE")]
        private void _tidy()
        {
            _stopwatch.Stop();


            TimeSpan elapsed = new TimeSpan(_stopwatch.Elapsed.Ticks - _startTicks);

            if (_logOnCompletion)
            {
                TraceWriter trace = GetWriter(elapsed.TotalMilliseconds > 50);

                trace(_counterName, string.Format("{3} Completed {0} in {2}ms (of {1}ms so far)", _operation, _stopwatch.Elapsed.TotalMilliseconds, elapsed.TotalMilliseconds, new string('>', depth)));
            }

            depth--;

            foreach (Stopwatch watch in _runningWatches) // should only be one
            {
                watch.Start();
            }
        }

        [Conditional("PROFILE")]
        public static void TraceAllCumulativeTimerTotals()
        {
            Dictionary<string, Stopwatch> timers = getTimers();

            foreach (string timer in timers.Keys)
            {
                double totalMilliseconds = timers[timer].Elapsed.TotalMilliseconds;
                TraceWriter trace = GetWriter(totalMilliseconds > 100);
                trace(timer, string.Format("Elapsed: {0}ms total", totalMilliseconds));
            }
        }


        private delegate void TraceWriter(string category, string message);

        private static TraceWriter GetWriter(bool warning)
        {
            if (warning)
            {
                return new TraceWriter(HttpContext.Current.Trace.Warn);
            }
            else
            {
                return new TraceWriter(HttpContext.Current.Trace.Write);
            }
        }
    }
}