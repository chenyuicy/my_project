using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OracleClient;
using System.Data;
using System.Diagnostics;
using System.IO;

namespace DmpImpTools
{
    public struct OracleXML
    {
        public string DBname;
        public string IP;
        public string UserID;
        public string Password;
    }
    class OraWriter
    {
        public delegate void ReadCallback(DateTime datetime, int[] ipids, decimal[] values);
        public delegate Int32[] GetCpid(string[] cpid, bool addIfNotExsit);
        private List<DataRow> datarow = null;

        OracleConnection connection;
        public OracleXML OraXML;
        public bool connected;
        public int UpdateTime = 0;
        public string Tablename = "";
        public int year = 0;
        public int BUpdate = 0;
        public int FileDate = 0;
        public int BImp = 0;
        public bool BCrossyear = false;

        public int ObjToInt(object o)
        {
            if (o == System.DBNull.Value)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(o);
            }
        }

        public string ObjToString(object o)
        {
            if (o == System.DBNull.Value)
            {
                return "0";
            }
            else
            {
                string s = Convert.ToString(o);
                s = s.Replace("\n", "");
                s = s.Replace("\r", "");
                s = s.Replace(",", "_");
                s = s.Replace(" ", "");
                return s;
            }
        }
        public OraWriter(string OraIP, string OraDBname, string OraUserID, string OraPassword)
        {
            OraXML.IP = OraIP;
            OraXML.DBname = OraDBname;
            OraXML.UserID = OraUserID;
            OraXML.Password = OraPassword;
        }

        public bool Connect()
        {
            try
            {
                //connString = @"Data Source=192.168.10.104/SKPBDB;User Id=pbdb_dt;Password=admin";
                string connString = @"Data Source=" + OraXML.IP + "/" + OraXML.DBname + ";User Id="
                   + OraXML.UserID + ";Password=" + OraXML.Password;
                connection = new OracleConnection(connString);
                connection.Open();
                connected = true;
                return connected;
            }
            catch (Exception e)
            {
                log.Write(e.Message);
                return false;
            }
        }

        public void DisConnect()
        {
            if (connection != null)
            {
                connection.Close();
            }
            connected = false;
        }


        public void Read(string sql, GetCpid getIpid, ReadCallback callback)
        {
            OracleCommand cmd = connection.CreateCommand();
            cmd.CommandText = sql;
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandTimeout = 0;
            OracleDataReader reader = cmd.ExecuteReader();

            string[] cpids = new string[reader.FieldCount - 1];
            Int32[] ipids = new Int32[reader.FieldCount - 1];
            decimal[] values = new decimal[reader.FieldCount - 1];
            int[] ipididexs = new int[reader.FieldCount - 1];
            int timeidx = -1;
            int cpidcnt = 0;

            for (int i = 0; i < reader.FieldCount; i++)
            {
                string columnName = reader.GetName(i);

                if ("sjsj".Equals(columnName.ToLower()))
                {
                    timeidx = i;
                }
                else
                {
                    cpids[cpidcnt] = columnName;
                    ipididexs[cpidcnt++] = i;
                }
            }

            if (timeidx == -1)
            {
                throw new Exception("sql '" + sql + "' timestamp or datetime(SJSJ) filed not found.");
            }

            ipids = getIpid(cpids, true);

            while (reader.Read())
            {
                decimal tmp = 0;
                for (int i = 0; i < ipids.Length; i++)
                {
                    if (reader.IsDBNull(ipididexs[i]))
                    {
                        values[i] = 0;
                    }
                    else
                    {
                        values[i] = reader.GetDecimal(ipididexs[i]);
                        tmp = values[i];
                    }
                }

                DateTime dtm = DateTime.Now;
                if ("sjsj".Equals(reader.GetName(timeidx).ToLower()))
                {
                    dtm = reader.GetDateTime(timeidx);
                }
                else
                {
                    throw new Exception("sql '" + sql + "' timestamp or datetime(SJSJ) filed not found.");
                }

                callback(dtm, ipids, values);
            }

            reader.Close();
        }


        public int Delete(string tablename)
        {
            if (tablename=="")
            {
                log.Write("delete datebase failed");
                return -1;
            }
            string sql = @"TRUNCATE TABLE " + tablename;
            OracleCommand cmd = new OracleCommand(sql, connection);
            int result = cmd.ExecuteNonQuery();
            log.Write("delete datebase");

            return result;
        }

        public void GetInfoDate()
        {
            string sql = @"select UPDATETIME,BUPDATE,BIMP,FILEDATE from DMPIMPINFO";
            OracleCommand cmd = new OracleCommand(sql, connection);
            OracleDataAdapter oda = new OracleDataAdapter(cmd);
            DataTable dt = new DataTable();
            oda.Fill(dt);

            DataRow row = dt.Rows[0];
            UpdateTime = ObjToInt(row.ItemArray[0]);
            BUpdate = ObjToInt(row.ItemArray[1]);
            BImp = ObjToInt(row.ItemArray[2]);
            FileDate = ObjToInt(row.ItemArray[3]);
        }

        public void SetInfoDate(string date)
        {


        }

        public int UpdateInfoDate(int filetime)
        {
            string sql = @"update DMPIMPINFO set UPDATETIME='" + filetime + "'";
            OracleCommand cmd = new OracleCommand(sql, connection);
            int result = cmd.ExecuteNonQuery();
            if (result > 0)
            {
                UpdateTime = filetime;
                log.Write("Update UPDATETIME " + filetime);
            }
            return result;
        }

        public int UpdateFileDate(int filetime)
        {
            string sql = @"update DMPIMPINFO set FILEDATE='" + filetime + "'";
            OracleCommand cmd = new OracleCommand(sql, connection);
            int result = cmd.ExecuteNonQuery();
            if (result > 0)
            {
                FileDate = filetime;
                log.Write("Update FILEDATE " + filetime);
            }
            return result;
        }

        public int BUpdateto1()
        {
            string sql = @"update DMPIMPINFO set BUPDATE='1'";
            OracleCommand cmd = new OracleCommand(sql, connection);
            int result = cmd.ExecuteNonQuery();
            if (result > 0)
            {
                BUpdate = 1;
            }
            return result;
        }

        public int BUpdateto0()
        {
            string sql = @"update DMPIMPINFO set BUPDATE='0'";
            OracleCommand cmd = new OracleCommand(sql, connection);
            int result = cmd.ExecuteNonQuery();
            if (result > 0)
            {
                BUpdate = 0;
            }
            return result;
        }

        public void ImpDmp(string sqlpath, string filename)
        {
            try
            {
                string str = "host imp " + OraXML.UserID + "/" + OraXML.Password + "@" + OraXML.DBname + " file='"
                        + filename + "' ignore=y buffer=9999999 full=y;";

                if (!File.Exists(filename))
                {
                    log.Write(filename + " is no exit!");
                    return;
                }
                //写入Impdmp.sql文件中
                FileStream outputStream = new FileStream(sqlpath + "\\Impdmp.sql", FileMode.Create);
                byte[] byteArray = System.Text.Encoding.Default.GetBytes(str);
                outputStream.Write(byteArray, 0, byteArray.Length);
                outputStream.Close();

                if (BCrossyear)
                {
                    makesqlFile(sqlpath);
                    BCrossyear = false;
                }

                //执行脚本
                //string targetDir = string.Format(@"C:\Users\Administrator\Desktop\Oracle项目\sql");
                string targetDir = sqlpath;
                Process proc = new Process();
                proc.StartInfo.WorkingDirectory = targetDir;
                proc.StartInfo.FileName = "IMP_th_data_sslsd.bat";
                proc.StartInfo.Arguments = string.Format("10");//this is argument
                //proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.CreateNoWindow = true;
                proc.Start();
                proc.Dispose();
            }
            catch (Exception e)
            {
                log.Write(e.Message);
            }
        }

        //更新sql文件
        public void makesqlFile(string sqlpath)
        {
            string str = "";
            //生成DropIndex文件
            FileStream dropindex = new FileStream(sqlpath + "\\DropIndex.sql", FileMode.Create);
            StreamWriter sw = new StreamWriter(dropindex);
            str = "update DMPIMPINFO set BIMP='1';";
            sw.WriteLine(str);
            string index1 = "drop index DATA_SSLSD_" + year + "_IDX2";
            sw.WriteLine(index1);
            string index2 = "drop index DATA_SSLSD_" + year + "_IDX3";
            sw.WriteLine(index2);
            string index3 = "drop index TH_DATA_SSLSD_" + year + "_IDX4";
            sw.WriteLine(index3);
            sw.Flush();
            //关闭流
            sw.Close();
            dropindex.Close();

            //Modify CreateIndex文件
            if (!File.Exists(sqlpath + "\\CreateIndex.sql"))
            {
                log.Write(sqlpath + "\\CreateIndex.sql" + " is no exit!");
                return;
            }
            string[] sqls = File.ReadAllLines(sqlpath + "\\CreateIndex.sql");
            for (int i = 0; i < sqls.Length; i++)
            {
                sqls[i] = sqls[i].Replace("DATA_SSLSD_" + (year - 1) + "_IDX2", index1.ToUpper());
                sqls[i] = sqls[i].Replace("DATA_SSLSD_" + (year - 1) + "_IDX3", index2.ToUpper());
                sqls[i] = sqls[i].Replace("TH_DATA_SSLSD_" + (year - 1) + "_IDX4", index3.ToUpper());
                sqls[i] = sqls[i].Replace("TH_DATA_SSLSD_" + (year - 1), Tablename.ToUpper());
            }

            FileStream CreateIndex = new FileStream(sqlpath + "\\CreateIndex.sql", FileMode.Create);
            StreamWriter sw2 = new StreamWriter(CreateIndex);
            for (int i = 0; i < sqls.Length; i++)
            {
                sw2.WriteLine(sqls[i]);
            }
            sw2.Flush();
            //关闭流
            sw2.Close();
            CreateIndex.Close();

            //生成Oracle_SQL文件 方式1 直接替换
            if (!File.Exists(sqlpath + "\\Oracle_SQL.txt"))
            {
                log.Write(sqlpath + "\\Oracle_SQL.txt" + " is no exit!");
                return;
            }
            string[] OracleSQL = File.ReadAllLines(sqlpath + "\\Oracle_SQL.txt");
            for (int i = 0; i < sqls.Length; i++)
            {
                OracleSQL[i] = OracleSQL[i].Replace("th_data_sslsd_" + (year - 1), Tablename);
            }

            FileStream Oracle_SQL = new FileStream(sqlpath + "\\Oracle_SQL.txt", FileMode.Create);
            StreamWriter sw3 = new StreamWriter(Oracle_SQL);
            for (int i = 0; i < OracleSQL.Length; i++)
            {
                sw3.WriteLine(OracleSQL[i]);
            }
            sw3.Flush();
            //关闭流
            sw3.Close();
            Oracle_SQL.Close();


            //生成Oracle_SQL文件 方式2 重新生成
            //todo::有必要重新读取设备表吗？
            //string s = @"select BYQJH,CLDBH from TH_DEV_BYQXX_GB ORDER BY CLDBH";
            //OracleCommand cmd = new OracleCommand(s, connection);
            //OracleDataAdapter oda = new OracleDataAdapter(cmd);
            //DataSet ds = new DataSet();
            //int n = oda.Fill(ds);

            //datarow = new List<DataRow>();
            //string[] BYQJH = new string[n];
            //int[] CLDBH = new int[n];

            //for (int i = 0; i < n; i++)
            //{
            //    DataRow row = ds.Tables[0].Rows[i];
            //    datarow.Add(row);

            //    if (row.ItemArray[0] == System.DBNull.Value)
            //    {
            //        BYQJH[i] = "0";
            //    }
            //    else
            //    {
            //        BYQJH[i] = Convert.ToString(row.ItemArray[2]);
            //        BYQJH[i] = BYQJH[i].Replace("\n", "");
            //    }
            //    if (row.ItemArray[1] == System.DBNull.Value)
            //    {
            //        CLDBH[i] = 0;
            //    }
            //    else
            //    {
            //        CLDBH[i] = Convert.ToInt32(row.ItemArray[3]);
            //    }
            //}

            //FileStream file1 = new FileStream(sqlpath+"\\Oracle_SQL.txt", FileMode.Create);
            //StreamWriter sw1 = new StreamWriter(file1);
            //string str1;
            //for (int i = 0; i < n; i++)
            //{
            //    if ((CLDBH[i] == 0) && (BYQJH[i].Equals("0")))
            //    {
            //        continue;
            //    }
            //    str1 = "SELECT SJSJ,AXDL as AI_" + CLDBH[i] + "_AXDL_" + BYQJH[i] + ", ";
            //    str1 += "PTBB as AI_" + CLDBH[i] + "_PTBB_" + BYQJH[i] + ", ";
            //    str1 += "CTBB as AI_" + CLDBH[i] + "_CTBB_" + BYQJH[i] + ", ";
            //    str1 += "BXDL as AI_" + CLDBH[i] + "_BXDL_" + BYQJH[i] + ", ";
            //    str1 += "CXDL as AI_" + CLDBH[i] + "_CXDL_" + BYQJH[i] + ", ";
            //    str1 += "AXDY as AI_" + CLDBH[i] + "_AXDY_" + BYQJH[i] + ", ";
            //    str1 += "BXDY as AI_" + CLDBH[i] + "_BXDY_" + BYQJH[i] + ", ";
            //    str1 += "CXDY as AI_" + CLDBH[i] + "_CXDY_" + BYQJH[i] + ", ";
            //    str1 += "YGGL as AI_" + CLDBH[i] + "_YGGL_" + BYQJH[i] + ", ";
            //    str1 += "WGGL as AI_" + CLDBH[i] + "_WGGL_" + BYQJH[i] + ", ";
            //    str1 += "GLYS as AI_" + CLDBH[i] + "_GLYS_" + BYQJH[i] + ", ";
            //    str1 += "AXYGGL as AI_" + CLDBH[i] + "_AXYGGL_" + BYQJH[i] + ", ";
            //    str1 += "BXYGGL as AI_" + CLDBH[i] + "_BXYGGL_" + BYQJH[i] + ", ";
            //    str1 += "CXYGGL as AI_" + CLDBH[i] + "_CXYGGL_" + BYQJH[i] + ", ";
            //    str1 += "AXWGGL as AI_" + CLDBH[i] + "_AXWGGL_" + BYQJH[i] + ", ";
            //    str1 += "BXWGGL as AI_" + CLDBH[i] + "_BXWGGL_" + BYQJH[i] + ", ";
            //    str1 += "CXWGGL as AI_" + CLDBH[i] + "_CXWGGL_" + BYQJH[i];
            //    str1 += " from "+Tablename+" where BYQJH=" + BYQJH[i] + "  order by SJSJ";
            //    sw1.WriteLine(str1);
            //}
            //sw1.Flush();
            ////关闭流
            //sw1.Close();
            //file1.Close();
        }

    }
}
