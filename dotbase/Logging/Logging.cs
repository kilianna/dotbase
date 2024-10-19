using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace DotBase.Logging
{
    class LoggingHelper
    {
        private static Dictionary<string, int> stackTraces = new Dictionary<string, int>();
        private static int nextStackTrace = 0;

        private List<object> aux = new List<object>(3);

        public LoggingHelper(int id, string className)
        {
            aux.Add("");
            aux.Add("");
            aux.Add(id.ToString("X6"));
            aux.Add(className);
        }

        public void log(string format, params object[] args)
        {
            if (format == "!")
            {
                int index = args.Length == 2 ? (int)args[0] + 4 : 4;
                object value = args.Length == 2 ? args[1] : args[0];
                while (aux.Count <= index) aux.Add("");
                aux[index] = value;
                return;
            }
            StringBuilder f = new StringBuilder();
            f.Append("{");
            f.Append(args.Length);
            f.Append(":yy-MM-dd HH:mm:ss.fff}: stack {");
            f.Append(args.Length + 1);
            f.Append("}: ");
            for (int i = 2; i < aux.Count; i++)
            {
                f.Append("{");
                f.Append(args.Length + i);
                f.Append("}: ");
            }
            f.Append(format);
            string stackTrace = "\r\n" + Environment.StackTrace;
            stackTrace = Regex.Replace(stackTrace, "(\n[^\n]+ (System|Microsoft|DotBase\\.Log|DotBase\\.Logging|DotBase\\.LoggingHelper)\\.[^\n]+)+", "\n   -").Trim('\r', '\n', ' ', '\t', '-');
            int stackId;
            if (stackTraces.ContainsKey(stackTrace))
            {
                stackId = stackTraces[stackTrace];
                stackTrace = null;
            }
            else
            {
                nextStackTrace++;
                stackId = nextStackTrace;
                stackTraces[stackTrace] = stackId;
            }
            var args2 = args.Concat(aux).ToArray();
            args2[args.Length] = DateTime.Now;
            args2[args.Length + 1] = stackId;
            Log.log(f.ToString(), args2);
            if (stackTrace != null)
            {
                Log.log("   STACK {0}\r\n   {1}", stackId, stackTrace);
            }
        }
    }

    delegate void Logger(string format, params object[] args);

    static class Log
    {
        private static int uid = 0;
        private static Dictionary<string, string> commonMap = new Dictionary<string, string>();

        public static Logger create()
        {
            uid++;
            string className = "";
            var st = new StackTrace(true);
            for (int i = 0; i < st.FrameCount; i++)
            {
                StackFrame sf = st.GetFrame(i);
                if (sf.GetMethod().DeclaringType == typeof(Log))
                {
                    sf = st.GetFrame(i + 1);
                    className = sf.GetMethod().DeclaringType.Name;
                    break;
                }
            }
            return (new LoggingHelper(uid, className)).log;
        }

        public static string common(string text)
        {
            return common("{0}", text);
        }

        public static string common(string format, params object[] args)
        {
            var value = String.Format(format, args);
            if (commonMap.ContainsKey(value))
            {
                return commonMap[value];
            }
            else
            {
                int index = commonMap.Count;
                commonMap[String.Format("@{0}", index)] = value;
                return String.Format("@{0}={1}", index, value);
            }
        }

        public static void log(string format, params object[] args)
        {
            string text = String.Format(format + "\r\n", args);
#if DEBUG
            Debug.Write(text);
#endif
            EncryptedLogger.Push(text);
        }
    }
}
