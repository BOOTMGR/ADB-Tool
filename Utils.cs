using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADB_Tools
{
    class Utils
    {
        public static bool is_ADBRunning()
        {
            Process[] pname = Process.GetProcessesByName("adb");
            if (pname.Length == 0)
                return false;
            return true;
        }

        public static string startADB()
        {
            Process p = new Process();
            p.StartInfo.FileName = "adb.exe";
            p.StartInfo.Arguments = "server";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.Start();
            p.WaitForExit();
            return p.StandardOutput.ReadToEnd();
        }

        public static bool killADB()
        {
            try
            {
                foreach (Process proc in Process.GetProcessesByName("adb"))
                {
                    proc.Kill();
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public static List<string> listDevices()
        {
            List<string> ret = new List<string>();
            if (!is_ADBRunning())
            {
                startADB();
            }
            Process p = new Process();
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.FileName = "adb.exe";
            p.StartInfo.Arguments = "devices";
            p.Start();
            p.WaitForExit();
            string buf = p.StandardOutput.ReadToEnd();
            using (StringReader reader = new StringReader(buf))
            {
                string line = string.Empty;
                do
                {
                    line = reader.ReadLine();
                    if (line != null && !String.Equals(line, ""))
                    {
                        if (!line.Contains("List of devices attached"))
                        {
                            ret.Add(parseDeviceID(line));
                        }
                    }
                } while (line != null);
            }
            return ret;
        }

        public static string parseDeviceID(string buf)
        {
            char[] ret = new char[buf.Trim().Length];
            int i = 0;
            foreach (char c in buf)
            {
                if (c != '\t')
                {
                    ret[i] = c;
                    i++;
                }
                else
                    break;
            }
            string s = new string(ret);
            return s;
        }
    }
}
