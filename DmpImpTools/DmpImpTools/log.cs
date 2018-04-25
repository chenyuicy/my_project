using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace DmpImpTools
{
    class log
    {
        public static void Write(string message)
        {
            try
            {
                string path = Process.GetCurrentProcess().MainModule.FileName;
                string homePath = path.Substring(0, path.LastIndexOf("\\"));
                //homePath = homePath.Substring(0, homePath.LastIndexOf("\\"));
                string logPath = String.Copy(homePath + "\\" +Process.GetCurrentProcess().ProcessName + ".log");

                long size = 0;
                if (File.Exists(logPath))
                    size = new FileInfo(logPath).Length;

                if (size > (100 * 1024 * 1024))//100M
                {
                    DateTime dt = DateTime.Now;
                    string filename = logPath;
                    filename += "_" + dt.ToString("yyyyMMddHH");
                    File.Move(logPath, filename);
                }

                FileStream stream = File.Open(logPath, FileMode.Append);
                if (stream == null)
                    return;

                string msg = "<" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + " " + Process.GetCurrentProcess().Id + "> " + message + "\r\n";


                byte[] bs = System.Text.Encoding.Default.GetBytes(msg);
                stream.Write(bs, 0, bs.Length);
                stream.Close();
            }
            catch
            {
                return;
            }
        }
    }
}
