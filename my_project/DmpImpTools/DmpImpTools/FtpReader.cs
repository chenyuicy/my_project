using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Threading;

namespace DmpImpTools
{
    struct logFileInfo
    {
        public string date;
        public string Exportline;
        public bool bPMAX;
    }
    class FtpReader
    {
        public List<string> filename;
        public List<string> logfile;
        string ftpServerIP = "";
        string ftpRemotePath = "";
        string ftpUserID = "";
        string ftpPassword = "";
        string ftpURI = "";

        // <summary>
        // 连接FTP
        // </summary>
        // <param name="FtpServerIP">FTP连接地址</param>
        // <param name="FtpRemotePath">指定FTP连接成功后的当前目录, 如果不指定即默认为根目录</param>
        // <param name="FtpUserID">用户名</param>
        // <param name="FtpPassword">密码</param>
        public FtpReader(string FtpServerIP, string FtpRemotePath, string FtpUserID, string FtpPassword)
        {
            ftpServerIP = FtpServerIP;
            ftpRemotePath = FtpRemotePath;
            ftpUserID = FtpUserID;
            ftpPassword = FtpPassword;
            ftpURI = "ftp://" + ftpServerIP + ":" + ftpRemotePath + "/";
        }

        /// <summary>
        /// 获取当前目录下文件列表(仅文件)
        /// </summary>
        /// <returns></returns>
        public string[] GetFileList()
        {
            string[] downloadFiles;
            StringBuilder result = new StringBuilder();
            FtpWebRequest reqFTP;
            try
            {
                reqFTP = (FtpWebRequest)WebRequest.Create(new Uri(ftpURI));
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                reqFTP.Method = WebRequestMethods.Ftp.ListDirectory;
                reqFTP.UsePassive = false;
                WebResponse response = reqFTP.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.Default);

                string line = reader.ReadLine();
                while (line != null)
                {
                    result.Append(line);
                    result.Append("\n");
                    line = reader.ReadLine();
                }
                result.Remove(result.ToString().LastIndexOf('\n'), 1);
                reader.Close();
                response.Close();
                return result.ToString().Split('\n');
            }
            catch (Exception ex)
            {
                downloadFiles = null;
                if (ex.Message.Trim() != "远程服务器返回错误: (550) 文件不可用(例如，未找到文件，无法访问文件)。")
                {
                    log.Write(ex.Message);
                    throw new Exception("FtpHelper GetFileList Error --> " + ex.Message.ToString());
                }
                return downloadFiles;
            }
        }

        public void GetDmpFile(string[] files)
        {
            try
            {
                filename = new List<string>();
                foreach (string file in files)//循环文件
                {
                    //string exname = file.Substring(file.LastIndexOf(".") + 1);//得到后缀名                                                                          
                    if ((".DMP".IndexOf(file.ToUpper().Substring(file.LastIndexOf(".") + 1)) > -1) && //如果后缀名为.dmp文件
                        (file.ToUpper().Contains("TH_DATA_SSLSD")))
                    {
                        filename.Add(file);//把.txt文件全名加人到FileInfo对象
                    }
                }
                filename.Sort();
            }
            catch (Exception e)
            {
                log.Write(e.Message);
            }
        }

        public void GetLogFile(string[] files)
        {
            try
            {
                logfile = new List<string>();
                foreach (string file in files)//循环文件
                {
                    //string exname = file.Substring(file.LastIndexOf(".") + 1);//得到后缀名                                                                          
                    if ((".LOG".IndexOf(file.ToUpper().Substring(file.LastIndexOf(".") + 1)) > -1) && //如果后缀名为.log文件
                        (file.ToUpper().Contains("TH_DATA_SSLSD")))
                    {
                        logfile.Add(file);//把.txt文件全名加人到FileInfo对象
                    }
                }
                logfile.Sort();
            }
            catch (Exception e)
            {
                log.Write(e.Message);
            }
        }

        public List<logFileInfo> ParseLogFile(string logfile)
        {//todo::need modify
            try
            {
                if (!File.Exists(logfile))
                {
                    log.Write(logfile + " is no exit!");
                    return null;
                }

                log.Write("Parse file is " + logfile);
                List<logFileInfo> logInfo = new List<logFileInfo>();
                Encoding.GetEncoding("gb2312");
                string[] sqls = File.ReadAllLines(logfile, Encoding.Default);
                bool PMAX = false;
                string date = "";
                for (int i = 0; i < sqls.Length; i++)
                {
                    if (sqls[i].Contains("正在导出分区"))
                    {
                        if (sqls[i].IndexOf("PMAX") > 0)
                        {
                            PMAX=true;
                        }
                        else
                        {
                            int n = sqls[i].IndexOf('P');
                            date = sqls[i].Substring(n + 1, 8);
                        }  
                        bool bNoData = false;

                        while (i < sqls.Length)
                        {
                            if (bNoData)
                            {
                                break;
                            }
                            i++;
                            if (sqls[i].Contains("导出了"))
                            {
                                string str1 = sqls[i];
                                char[] cc = str1.ToCharArray();
                                for (int k = 3; k < str1.Length; k++)
                                {
                                    if (cc[k] != ' ')
                                    {
                                        if (cc[k] == '0')
                                        {
                                            bNoData = true;
                                            break;
                                        }
                                        int num = k;
                                        int len = str1.Length - num - 2;
                                        string s = str1.Substring(num, len);
                             
                                        if (PMAX)
                                        {
                                            log.Write(logfile + "exp date is PMAX");
                                            logFileInfo lf = new logFileInfo();
                                            //lf.date = date;
                                            lf.bPMAX = true;
                                            lf.Exportline = s;
                                            logInfo.Add(lf);
                                            bNoData = true;
                                            break;
                                        }
                                        else
                                        {
                                            log.Write(logfile + "exp date is " + date);
                                            logFileInfo lf = new logFileInfo();
                                            lf.date = date;
                                            lf.bPMAX = false;
                                            lf.Exportline = s;
                                            logInfo.Add(lf);
                                            bNoData = true;
                                            break;
                                        }

                                    }
                                }
                            }
                        }
                    }
                }

                return logInfo;
            }
            catch (Exception e)
            {
                log.Write(e.Message);
                return null;
            }
        }

        // <summary>
        // 获取文件修改时间
        // </summary>
        // <param name="fileName"></param>
        public DateTime GetFileTime(string fileName)
        {
            DateTime fdt = DateTime.Now;
            FtpWebRequest reqFTP;
            FtpWebResponse response = null;
            try
            {
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpURI + fileName));
                reqFTP.Method = WebRequestMethods.Ftp.GetDateTimestamp;
                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                response = (FtpWebResponse)reqFTP.GetResponse();
                //DateTime nowt = DateTime.Now;
                //nowt = nowt.AddHours(-1); //暂定为1hour
                fdt = response.LastModified;
                log.Write(fileName + " time is " + fdt.ToString());
                return fdt;
            }
            catch (Exception ex)
            {
                return fdt;
                throw new Exception("FtpHelper Download Error --> " + ex.Message);
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                }
            }
        }

        // <summary>
        // 下载
        // </summary>
        // <param name="filePath"></param>
        // <param name="fileName"></param>
        public bool Download_IO(string filePath, string fileName)
        {
            try
            {
                long size = 0;

                if (File.Exists(filePath + fileName))
                {
                    using (FileStream outputStream1 = new FileStream(filePath + fileName, FileMode.Open))
                    {
                        size = outputStream1.Length;
                    }
                    return FtpBrokenDownload(filePath, fileName, size);
                }

                return Download(filePath,fileName);
            }
            catch(Exception e)
            {
                log.Write(e.Message);
                return false;
            }
        }

        // <summary>
        // 下载
        // </summary>
        // <param name="filePath"></param>
        // <param name="fileName"></param>
        public bool Download(string filePath, string fileName)
        {
            FtpWebRequest reqFTP;
            Stream ftpStream = null;
            FtpWebResponse response = null;
            FileStream outputStream = null;
            try
            {
                //System.GC.Collect();//垃圾回收
                //System.Net.ServicePointManager.DefaultConnectionLimit = 512;//设置最大连接数

                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpURI + fileName));
                reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
                reqFTP.UseBinary = true;
                reqFTP.UsePassive = false;
                reqFTP.KeepAlive = false;
                //reqFTP.Timeout = 15000;
                //reqFTP.ReadWriteTimeout = 15000;
                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                response = (FtpWebResponse)reqFTP.GetResponse();
                ftpStream = response.GetResponseStream();
                outputStream = new FileStream(filePath + fileName, FileMode.Create);

                int bufferSize = 2048;
                int readCount;
                byte[] buffer = new byte[bufferSize];

                readCount = ftpStream.Read(buffer, 0, bufferSize);
                while (readCount > 0)
                {
                    outputStream.Write(buffer, 0, readCount);
                    readCount = ftpStream.Read(buffer, 0, bufferSize);
                }

                ftpStream.Close();
                outputStream.Close();
                response.Close();
                return true;
            }
            catch (Exception ex)
            {
                log.Write(ex.Message);
                if (outputStream != null)
                {
                    outputStream.Close();
                }
                return false;         
                throw new Exception("FtpHelper Download Error --> " + ex.Message);
            }
            finally
            {
                if (ftpStream != null)
                {
                    ftpStream.Close();
                }
                if (outputStream != null)
                {
                    outputStream.Close();
                }
                if (response != null)
                {
                    response.Close();
                }
            }
        }

        // <summary>  
        // 从FTP服务器下载文件，指定本地路径和本地文件名（支持断点下载）  
        // </summary>  
        // <param name="fileName">远程文件名</param>  
        // <param name="filePath">保存本地的路径）</param>  
        // <param name="size">已下载文件流大小</param>  
        // <returns>是否下载成功</returns>  
        public bool FtpBrokenDownload(string filePath, string fileName,long size)
        {
            FtpWebRequest reqFTP;
            Stream ftpStream = null;
            FtpWebResponse response = null;
            FileStream outputStream = null;
            try
            {
                //System.GC.Collect();//垃圾回收
                //System.Net.ServicePointManager.DefaultConnectionLimit = 512;//设置最大连接数

                outputStream = new FileStream(filePath + fileName, FileMode.Append);
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpURI + fileName));
                Thread.Sleep(30);
                reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
                reqFTP.UseBinary = true;
                reqFTP.UsePassive = true;
                reqFTP.KeepAlive = false;
                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                reqFTP.ContentOffset = size;
                //reqFTP.Timeout = 15000;
                //reqFTP.ReadWriteTimeout = 15000;
                response = (FtpWebResponse)reqFTP.GetResponse();
                ftpStream = response.GetResponseStream();

                int bufferSize = 2048;
                int readCount;
                byte[] buffer = new byte[bufferSize];
                readCount = ftpStream.Read(buffer, 0, bufferSize);
                while (readCount > 0)
                {
                    outputStream.Write(buffer, 0, readCount);
                    readCount = ftpStream.Read(buffer, 0, bufferSize);
                }
                ftpStream.Close();
                outputStream.Close();
                response.Close();
                return true;
            }
            catch (Exception e)
            {
                log.Write(e.Message);
                if (outputStream != null)
                {
                    outputStream.Close();
                }
                return false;
                throw;
            }
            finally
            {
                if (ftpStream != null)
                {
                    ftpStream.Close();
                }
                if (outputStream != null)
                {
                    outputStream.Close();
                }
                if (response != null)
                {
                    response.Close();
                }
            }
        }


        // <summary>
        // 删除FTP服务器上的文件
        // </summary>
        // <param name="fileName">文件名称</param>
        // <returns>返回一个值,指示是否删除成功</returns>
        public void DeleteFile(string fileName)
        {
            FtpWebRequest reqFTP;
            bool result = false;
            try
            {
                reqFTP = (FtpWebRequest)WebRequest.Create(new Uri(ftpURI + fileName));
                // 指定数据传输类型
                reqFTP.UseBinary = true;
                // ftp用户名和密码
                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                // 默认为true，连接不会被关闭
                // 在一个命令之后被执行
                reqFTP.KeepAlive = false;
                // 指定执行什么命令
                reqFTP.Method = WebRequestMethods.Ftp.DeleteFile;
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                response.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("删除文件出错：" + ex.Message);
            }
        }

        public List<string> GetFileName(string folderFullName)
        {
            DirectoryInfo TheFolder = new DirectoryInfo(folderFullName);
            List<string> file = new List<string>();
            ////遍历文件夹
            //foreach (DirectoryInfo NextFolder in TheFolder.GetDirectories())
            //    this.listBox1.Items.Add(NextFolder.Name);
            //遍历文件
            foreach (FileInfo NextFile in TheFolder.GetFiles())
                file.Add(NextFile.Name);

            return file;
        }



    }
}


