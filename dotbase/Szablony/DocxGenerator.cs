using System;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Data;
using System.Collections.Generic;
using System.Globalization;

namespace DotBase.Szablony
{

    class DocxGenerator
    {
        private static string xml2docxExe;
        public delegate void onFinishedDelegate(bool success, string errorMessage);
        public event onFinishedDelegate onFinished;
        private string json;
        private Process proc;
        private StringBuilder stdout = new StringBuilder();
        private StringBuilder stderr = new StringBuilder();

        static DocxGenerator() {
            var dir = N.getProgramDir();
            xml2docxExe = dir + "\\xml2docx.exe";
            if (!File.Exists(xml2docxExe)) {
                xml2docxExe = dir + "\\xml2docx-win.exe";
            }
            if (!File.Exists(xml2docxExe))
            {
                xml2docxExe = null;
            }
        }

        public void generate(string template, object data, string output)
        {
            string jsonFile = null; // TODO: delete file when done
            try
            {
                if (xml2docxExe == null)
                {
                    throw new ApplicationException("Nie znaleziono narzędzia 'xml2docx' w katalogu " + N.getProgramDir());
                }
                json = jsonStringify(data);
                jsonFile = Path.GetTempPath() + Guid.NewGuid().ToString() + ".json";
#if DEBUG
                string jsonDebug = (new Uri(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase) + @"\..\..\Szablony\" + Path.GetFileNameWithoutExtension(template) + ".json")).LocalPath;
                File.WriteAllText(jsonDebug, json);
#endif
                File.WriteAllText(jsonFile, json);
                proc = new Process();
                proc.StartInfo.FileName = xml2docxExe;
                proc.StartInfo.Arguments = String.Format("-d \"{0}\" \"{1}\" \"{2}\"",
                    escapeArg(jsonFile), escapeArg(template), escapeArg(output));
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.RedirectStandardError = true;
                proc.StartInfo.StandardOutputEncoding = Encoding.UTF8;
                proc.StartInfo.StandardErrorEncoding = Encoding.UTF8;
                proc.StartInfo.CreateNoWindow = true;
                proc.Exited += new EventHandler(proc_Exited);
                proc.OutputDataReceived += new DataReceivedEventHandler(proc_OutputDataReceived);
                proc.ErrorDataReceived += new DataReceivedEventHandler(proc_ErrorDataReceived);
                proc.EnableRaisingEvents = true;
                proc.Start();
                proc.BeginOutputReadLine();
                proc.BeginErrorReadLine();
            }
            catch (Exception ex)
            {
                if (onFinished != null)
                {
                    onFinished.Invoke(false, ex.Message);
                }
            }
        }

        private static string escapeArg(string arg)
        {
            return arg.Replace("\"", "\"\"");
        }

        private void proc_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                stdout.Append(e.Data);
                stdout.Append("\r\n");
            }
        }

        private void proc_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                stderr.Append(e.Data);
                stderr.Append("\r\n");
            }
        }

        private void proc_Exited(object sender, EventArgs e)
        {
            if (onFinished != null)
            {
                if (proc.ExitCode == 0)
                {
                    onFinished(true, "");
                }
                else
                {
                    var sb = new StringBuilder();
                    sb.Append(stderr);
                    sb.Append("====================================================================\r\n");
                    if (stdout.Length > 0) {
                        sb.Append(stdout);
                        sb.Append("====================================================================\r\n");
                    }
                    sb.Append("JSON file:\r\n");
                    sb.Append(json);
                    onFinished(false, sb.ToString());
                }
            }
        }


        public static string jsonStringify(object data)
        {
            var sb = new StringBuilder();
            valueToJson(data, sb, "");
            return sb.ToString();
        }

        private static void valueToJson(object data, StringBuilder result, string indent)
        {
            if (data == null || data is DBNull)
            {
                result.Append("null");
                return;
            }

            var type = data.GetType();

            if (type.IsArray)
            {
                var arr = data as Array;
                result.Append('[');
                var len = arr.Length;
                for (int i = 0; i < len; i++)
                {
                    if (i != 0) result.Append(',');
                    result.Append("\n");
                    result.Append(indent);
                    result.Append("  ");
                    valueToJson(arr.GetValue(i), result, indent + "  ");
                }
                result.Append('\n');
                result.Append(indent);
                result.Append(']');
            }
            else if (type.Equals(typeof(String)))
            {
                strToJson(data as String, result);
            }
            else if (type.Equals(typeof(Boolean)))
            {
                result.Append(((Boolean)data).ToString().ToLower());
            }
            else if (type.Equals(typeof(Int64))
                || type.Equals(typeof(Int32))
                || type.Equals(typeof(Int16))
                || type.Equals(typeof(SByte))
                || type.Equals(typeof(UInt64))
                || type.Equals(typeof(UInt32))
                || type.Equals(typeof(UInt16))
                || type.Equals(typeof(Byte))
                || type.Equals(typeof(Double))
                || type.Equals(typeof(Single))
            )
            {
                result.Append(String.Format("{0}", data).Replace(',', '.'));
            }
            else if (data is DateTime)
            {
                objToJson((DateTime)data, result, indent);
                return;
            }
            else if (type.IsClass)
            {
                if (data is DataRowCollection)
                {
                    objToJson(data as DataRowCollection, result, indent);
                    return;
                }
                else if (data is DataTable)
                {
                    objToJson(data as DataTable, result, indent);
                    return;
                }
                else if (data is DataRow)
                {
                    objToJson(data as DataRow, null, result, indent);
                    return;
                }
                else if (data is Dictionary<string, string>)
                {
                    objToJson(data as Dictionary<string, string>, result, indent);
                    return;
                }
                result.Append('{');
                var fields = type.GetFields();
                var first = true;
                foreach (var field in fields)
                {
                    if (!field.IsPublic || field.IsNotSerialized) continue;
                    if (!first) result.Append(',');
                    result.Append("\n");
                    result.Append(indent);
                    result.Append("  ");
                    strToJson(field.Name, result);
                    result.Append(": ");
                    valueToJson(field.GetValue(data), result, indent + "  ");
                    first = false;
                }
                result.Append('\n');
                result.Append(indent);
                result.Append('}');
            }
            else if (type.IsEnum)
            {
                strToJson(data.ToString(), result);
            }
            else
            {
                throw new ApplicationException(String.Format("Unable to convert JSON value: {0}, type {1}", data, type));
            }
        }

        private static void objToJson<T>(Dictionary<string, T> dictionary, StringBuilder result, string indent)
        {
            result.Append('{');
            var first = true;
            foreach (var item in dictionary)
            {
                if (!first) result.Append(',');
                result.Append("\n");
                result.Append(indent);
                result.Append("  ");
                strToJson(item.Key, result);
                result.Append(": ");
                valueToJson(item.Value, result, indent + "  ");
                first = false;
            }
            result.Append('\n');
            result.Append(indent);
            result.Append('}');
        }

        private static void objToJson(DateTime data, StringBuilder result, string indent)
        {
            valueToJson(new DocxDateTime(data), result, indent);
        }

        private static void objToJson(DataTable data, StringBuilder result, string indent)
        {
            objToJson(data.Rows, result, indent);
        }

        private static void objToJson(DataRowCollection data, StringBuilder result, string indent)
        {
            if (data.Count == 0)
            {
                result.Append("[]");
                return;
            }

            var row = data[0];
            string[] columns = new string[row.Table.Columns.Count];
            for (var i = 0; i < columns.Length; i++) {
                columns[i] = row.Table.Columns[i].ColumnName.ToLower();
            }

            result.Append('[');

            for (var i = 0; i < data.Count; i++)
            {
                if (i != 0) result.Append(',');
                result.Append("\n");
                result.Append(indent);
                result.Append("  ");
                objToJson(data[i], columns, result, indent + "  ");
            }
            result.Append('\n');
            result.Append(indent);
            result.Append(']');
        }

        private static void objToJson(DataRow row, string[] columns, StringBuilder result, string indent)
        {
            if (columns == null)
            {
                columns = new string[row.Table.Columns.Count];
                for (var i = 0; i < columns.Length; i++)
                {
                    columns[i] = row.Table.Columns[i].ColumnName.ToLower();
                }
            }
            result.Append('{');
            var first = true;
            for (var i = 0; i < columns.Length; i++)
            {
                if (!first) result.Append(',');
                result.Append("\n");
                result.Append(indent);
                result.Append("  ");
                strToJson(columns[i], result);
                result.Append(": ");
                valueToJson(row.ItemArray[i], result, indent + "  ");
                first = false;
            }
            result.Append('\n');
            result.Append(indent);
            result.Append('}');
        }

        private static void strToJson(string str, StringBuilder result)
        {
            result.Append("\"" + Regex.Replace(str as String, @"[^ !#-\[\]-~\x80-\uFFFF]", new MatchEvaluator(jsonStrEscape)) + "\"");
        }

        private static string jsonStrEscape(Match m)
        {
            char ch = m.Value[0];
            if (ch == '\r') return "\\r";
            if (ch == '\n') return "\\n";
            if (ch == '\t') return "\\t";
            if (ch == '\\') return "\\\\";
            if (ch == '"') return "\\\"";
            if (ch <= 255) return String.Format("\\x{0:X2}", (int)ch);
            return m.Value;
        }
    }
}
