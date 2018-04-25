using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace DmpImpTools
{
    public partial class s : Form
    {
        private FtpReader reader;
        private OraWriter writer;
        private bool bftpDownload = false;
        private bool bOra2SKdbIng = false;
        private bool bTimeContinue = false;
        private int iImpdate = 0;
        private int iFileDate = 0;
        private int iSaveDay = 100;
        public s()
        {
            InitializeComponent();
        }
        public static bool IsNumeric(string value)
        {
            return Regex.IsMatch(value, @"^[+-]?\d*[.]?\d*$");
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                bTimeContinue = checkBox1.Checked;
                checkBox1.Enabled = false;
                iImpdate = int.Parse(ImpDate.Text.Trim());
                iFileDate = int.Parse(FileDateBox.Text.Trim());
                iSaveDay = int.Parse(SaveDayLab.Text.Trim());
                string ftpServerIP = FtpIPLEdt.Text.Trim();
                string ftpRemotePath = FtpPathLEdt.Text.Trim();
                string ftpUserID = FtpUserLEdt.Text.Trim();
                string ftpPassword = FtpPasswordLEdt.Text.Trim();
                reader = new FtpReader(ftpServerIP, ftpRemotePath, ftpUserID, ftpPassword);                             
                //reader.GetFileName(LocalPathLEdt.Text);
                //reader.GetDmpFile(reader.filename.ToArray());

                string OraXMLIP = OraIPLEdt.Text.Trim();
                string OraXMLDBname = OraDbnameLEdt.Text.Trim();
                string OraXMLUserID = OraUserLEdt.Text.Trim();
                string OraXMLPassword = OraPasswordLEdt.Text.Trim();
                writer = new OraWriter(OraXMLIP, OraXMLDBname, OraXMLUserID, OraXMLPassword);
                if (writer.connected == false)
                {
                    writer.Connect();
                }
                writer.UpdateInfoDate(iImpdate);
                writer.UpdateFileDate(iFileDate);

                OKBtn.Enabled = false;
                double period = int.Parse(PeriodLEdt.Text.Trim()) * 60 * 60 * 1000.0;
                //double period = int.Parse(PeriodLEdt.Text.Trim()) * 1 * 60 * 1000.0;
                System.Timers.Timer timer = new System.Timers.Timer();
                timer.Elapsed += new ElapsedEventHandler(Imp);
                timer.Interval = period;
                timer.Enabled = true;
                timer.Start();

                Imp(null, null);

                
            }
            catch (Exception ee)
            {
                log.Write(ee.Message);
            }

        }

        private void Imp(object sender, ElapsedEventArgs e)
        {
            try
            {
                DateTime SdtTemp = DateTime.Now;
                DateTime EdtTemp = DateTime.Now;
                DateTime Now = DateTime.Now;
                log.Write("work begin : "+ Now.ToString());
                if ((writer.BImp != 0) || bOra2SKdbIng || bftpDownload)
                {
                    log.Write("work over : " + Now.ToString());
                    return;
                }

                string FilePath = LocalPathLEdt.Text.Trim();
                string[] files = reader.GetFileList();//ftp
                //string[] files = reader.GetFileName(FilePath).ToArray();//TODO::LocalPath
                reader.GetDmpFile(files);
                reader.GetLogFile(files);
                //if (FilePath == "")
                //{
                //    string path = Process.GetCurrentProcess().MainModule.FileName;
                //    string homePath = path.Substring(0, path.LastIndexOf("\\") + 1);
                //    FilePath = homePath;
                //}

                bool bWork = false;
                if (writer.connected == false)
                {
                    writer.Connect();
                }
                writer.GetInfoDate();
                reader.logfile = logFileCmp(reader.logfile, writer.FileDate);

                foreach (string file in reader.logfile)
                {
                    if (writer.connected == false)
                    {
                        writer.Connect();
                    }
                    writer.GetInfoDate();
                    string name1 = file.Substring(0, file.LastIndexOf("."));
                    string[] temp = name1.Split('_');
                    if (!IsNumeric(temp[temp.Length - 1]))
                    {
                        continue;
                    }
                    int filetime = Convert.ToInt32(temp[temp.Length - 1]);
                    DateTime Filedt = DateTime.ParseExact(temp[temp.Length - 1],
                       "yyyyMMdd",System.Globalization.CultureInfo.CurrentCulture);
                    Filedt = Filedt.AddDays(1);

                    string dateString = Convert.ToString(writer.UpdateTime);
                    DateTime dt = DateTime.ParseExact(dateString, "yyyyMMdd",
                        System.Globalization.CultureInfo.CurrentCulture);
                    dt = dt.AddDays(1);
                    writer.year = dt.Year;
                    if (dt.Month==1&&dt.Day==1)
                    {
                        writer.BCrossyear = true;
                    }
                    writer.Tablename = "th_data_sslsd_" + dt.Year;

                    List<logFileInfo> logInfo = reader.ParseLogFile(FilePath + file);
                    if (logInfo.Count <= 0)
                    {
                        continue;
                    }

                    if (logInfo[0].bPMAX)
                    {   //年底数据
                        if (dt.Month == 12 && (dt.Day == 30 || dt.Day == 31))
                        {
                            bWork = true;
                            EdtTemp = dt;
                        }
                        else
                        {
                            log.Write(file + " time is not continuity");
                            continue;
                        }
                    }
                    else
                    {
                        string startTemp = logInfo[0].date;
                        SdtTemp = DateTime.ParseExact(startTemp, "yyyyMMdd",
                        System.Globalization.CultureInfo.CurrentCulture);
                        SdtTemp = SdtTemp.AddDays(-2);
                        string endTemp = logInfo[logInfo.Count - 1].date;
                        EdtTemp = DateTime.ParseExact(endTemp, "yyyyMMdd",
                        System.Globalization.CultureInfo.CurrentCulture);
                        EdtTemp = EdtTemp.AddDays(-2);

                        if (bTimeContinue)
                        {//时间连续                   
                            if ((DateTime.Compare(dt, SdtTemp) >= 0) &&
                                (DateTime.Compare(dt, EdtTemp) <= 0))
                            {
                                bWork = true;
                            }
                            else
                            {
                                log.Write(file + " time is not continuity");
                                continue;
                            }
                        }
                        else
                        {
                            if (DateTime.Compare(SdtTemp, dt) > 0)
                            {
                                bWork = true;
                            }
                        }
                        //bWork = true;
                    }

                    if (bWork)
                    {
                        writer.GetInfoDate();
                        while ((writer.BImp != 0) || bOra2SKdbIng)
                        {//正在数据迁移
                            log.Write("importing date,wait 10min");
                            Thread.Sleep(10 * 60 * 1000);//10min 时间待定
                            writer.GetInfoDate();
                        }

                        if (writer.BUpdate != 0)//Oracle中数据需要迁移
                        {
                            log.Write("Oracle date time is " + writer.UpdateTime + " : Write to SKDB begin");
                            bOra2SKdbIng = true;
                            bool r = Ora2SKdb();
                            bOra2SKdbIng = false;
                            if (!r)
                            {
                                log.Write("Oracle date time is " + writer.UpdateTime + " : Write to SKDB failed");
                                return;
                            }

                            log.Write("Oracle date time is " + writer.UpdateTime + " : Write to SKDB Over");
                            if (writer.connected == false)
                            {
                                writer.Connect();
                            }
                            writer.BUpdateto0();
                            writer.Delete(writer.Tablename);
                        }
                        else
                        {
                            if (writer.connected == false)
                            {
                                writer.Connect();
                            }
                            writer.Delete(writer.Tablename);
                        }

                        string name = file.Substring(0, file.LastIndexOf("."));
                        name = name + ".dmp";
                        DateTime fdt = reader.GetFileTime(name);                    
                        DateTime nowt = DateTime.Now;
                        nowt = nowt.AddHours(-1); //暂定为 one hour
                        if (DateTime.Compare(nowt, fdt) < 0)
                        {
                            log.Write(name + " : file is not uploaded");
                            return;
                        }

                        bftpDownload = true;
                        bool ret = reader.Download_IO(FilePath, name);                  
                        while (!ret)
                        {
                            log.Write("Download " + name + " failed");
                            Thread.Sleep(5 * 60 * 1000);//暂定5*60*1000=5min 时间待定
                            ret = reader.Download_IO(FilePath, name);
                        }
                        bftpDownload = false;
                        log.Write("Download " + name + " succeed");

                        writer.ImpDmp(textBox_File.Text.Trim(), (FilePath + name));
                        string dmpend = EdtTemp.ToString("yyyyMMdd");
                        writer.UpdateInfoDate(int.Parse(dmpend));

                        string filestart = Filedt.ToString("yyyyMMdd");
                        writer.UpdateFileDate(int.Parse(filestart));

                        FileDateBox.Text = Convert.ToString(int.Parse(filestart));
                        ImpDate.Text = dmpend;
                        FileDateBox.Enabled = false;
                        ImpDate.Enabled = false;
                        writer.BUpdateto1();
                    }
                    Thread.Sleep(10000);
                }

                writer.GetInfoDate();
                while ((writer.BImp != 0) || bOra2SKdbIng)
                {//正在数据迁移
                    log.Write("importing date,wait 10min");
                    Thread.Sleep(10 * 60 * 1000);//10min 时间待定
                    writer.GetInfoDate();
                }

                string dateString1 = Convert.ToString(writer.UpdateTime);
                DateTime dt1 = DateTime.ParseExact(dateString1, "yyyyMMdd",
                    System.Globalization.CultureInfo.CurrentCulture);
                dt1 = dt1.AddDays(1);
                writer.Tablename = "th_data_sslsd_" + dt1.Year;
                if (writer.BUpdate != 0)//Oracle中数据需要迁移
                {
                    log.Write("Write to SKDB begin");
                    bOra2SKdbIng = true;
                    bool r = Ora2SKdb();
                    bOra2SKdbIng = false;
                    if (!r)
                    {
                        log.Write("Oracle date time is " + writer.UpdateTime + " : Write to SKDB failed");
                        return;
                    }
                    log.Write("Write to SKDB Over");
                    if (writer.connected == false)
                    {
                        writer.Connect();
                    }
                    writer.BUpdateto0();
                    writer.Delete(writer.Tablename);
                }
                else
                {
                    if (writer.connected == false)
                    {
                        writer.Connect();
                    }
                    writer.Delete(writer.Tablename);
                }

                writer.DisConnect();

                //删除ftp文件与本地文件
                DeleteFile();
                log.Write("work over : " + Now.ToString());
            }
            catch(Exception ec)
            {
                log.Write(ec.Message);
            }
        }

        private void DeleteFile()
        {
            string dateString = Convert.ToString(writer.UpdateTime); ;
            DateTime dt = DateTime.ParseExact(dateString, "yyyyMMdd",
                System.Globalization.CultureInfo.CurrentCulture);
            dt = dt.AddDays(-1 * iSaveDay);

            foreach (string file in reader.filename)
            {
                string name = file.Substring(0, file.LastIndexOf("."));
                string[] temp = name.Split('_');
                if (!IsNumeric(temp[temp.Length - 1]))
                {
                    continue;
                }
                int filetime = Convert.ToInt32(temp[temp.Length-1]);
                string strTemp = Convert.ToString(filetime);
                DateTime dtTemp = DateTime.ParseExact(strTemp, "yyyyMMdd",
                System.Globalization.CultureInfo.CurrentCulture);
                if (DateTime.Compare(dt, dtTemp) > 0)
                {
                    reader.DeleteFile(file);
                    log.Write("Delete ftp file : " + file);
                }
            }

            List<string> fileLocal = reader.GetFileName(LocalPathLEdt.Text);
            List<string> DmpLocal = new List<string>();
            foreach (string file in fileLocal.ToArray())//循环文件
            {
                if ((".dmp".IndexOf(file.Substring(file.LastIndexOf(".") + 1)) > -1) && //如果后缀名为.dmp文件
                    (file.ToUpper().Contains("TH_DATA_SSLSD")))
                {
                    DmpLocal.Add(file);//把.dmp文件全名加人到FileInfo对象
                }
            }
            DmpLocal.Sort();

            foreach (string file in DmpLocal)
            {
                string name = file.Substring(0, file.LastIndexOf("."));
                string[] temp = name.Split('_');
                if (!IsNumeric(temp[temp.Length - 1]))
                {
                    continue;
                }
                int filetime = Convert.ToInt32(temp[temp.Length - 1]);
                string strTemp = Convert.ToString(filetime);
                DateTime dtTemp = DateTime.ParseExact(strTemp, "yyyyMMdd",
                System.Globalization.CultureInfo.CurrentCulture);
                if (DateTime.Compare(dt, dtTemp) > 0)
                {
                    File.Delete(LocalPathLEdt.Text + file);
                    log.Write("Delete local file : " + LocalPathLEdt.Text + file);
                }
            }
        }

        private bool Ora2SKdb()
        {
            SkdbWriter SkdbW = new SkdbWriter();
            string destIP = textBox_destIP.Text.Trim();
            int destPort = int.Parse(textBox_destPort.Text.Trim());
            string destUser = textBox_destUser.Text.Trim();
            string destPass = textBox_destPassword.Text.Trim();
            bool ret = SkdbW.Connect(destIP, destPort, destUser, destPass);
            if (!ret)
            {
                log.Write("Can not connect to shenku database");
                return false;
            }

            string OraSqlFile = textBox_File.Text.Trim() + "Oracle_SQL.txt";
            if (!File.Exists(OraSqlFile))
            {
                log.Write(OraSqlFile + " is no exit!");
                return false;
            }

            string[] sqls = File.ReadAllLines(OraSqlFile);
            int total = sqls.Length;
            int cur = 0;

            for (int i = 0, nextIndex = 0; i < 3; i = 0)
            {
                try
                {
                    if (writer.connected == false)
                    {
                        writer.Connect();
                    }
                    ret = SkdbW.Connect(destIP, destPort, destUser, destPass);
                    if (!ret)
                    {
                        log.Write("Can not connect to shenku database");
                        return false;
                    }

                    for (int j = nextIndex; j < total; j++)
                    {
                        writer.Read(sqls[j], SkdbW.GetIpid, SkdbW.Write);
                        cur = j;
                        nextIndex = j + 1;
                    }
                }
                catch (Exception e)
                {
                    log.Write(e.Message);
                    if (cur > 0)
                        log.Write(nextIndex + " : " + sqls[cur]);
                    ++nextIndex;//跳过这一条语句
                    writer.DisConnect();
                    SkdbW.Close();
                    continue;
                }
                writer.DisConnect();
                SkdbW.Close();
                break;
            }
            return true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void LocalPathBtn_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string str = openFileDialog.SelectedPath;
                int n = str.Length;
                if (str[n - 1] != '\\')
                {
                    str += "\\";
                }
                LocalPathLEdt.Text = str;

            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string str = openFileDialog1.SelectedPath;
                int n = str.Length;
                if (str[n - 1] != '\\')
                {
                    str += "\\";
                }
                textBox_File.Text = str;
            }
        }

        private void OraIPLEdt_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox_File_TextChanged(object sender, EventArgs e)
        {

        }

        private List<string> logFileCmp(List<string> logfile,int filetime)
        {
            List<string> NewLog = new List<string>();
            string FilePath = LocalPathLEdt.Text.Trim();
            logfile.Sort();
            foreach (string file in logfile)
            {
                string name = file.Substring(0, file.LastIndexOf("."));
                string[] temp = name.Split('_');
                if (IsNumeric(temp[temp.Length - 1]))
                {
                    int time = Convert.ToInt32(temp[temp.Length - 1]);
                    if (time >= filetime)
                    {
                        NewLog.Add(file);
                        bftpDownload = true;
                        //bool ret = reader.Download_IO(FilePath, name + ".log");
                        //while (!ret)
                        //{
                        //    log.Write("Download " + name + " failed");
                        //    Thread.Sleep(20 * 60 * 1000);//20min 时间待定
                        //    ret = reader.Download_IO(FilePath, name);
                        //}
                        filetime = time;
                        log.Write("Download " + file + " succeed");
                        bftpDownload = false;
                    }
                }
            }     
            return NewLog;
        }

        private void ImpDate_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
