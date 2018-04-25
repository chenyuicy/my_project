using cn.com.skdb.api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DmpImpTools
{
    class SkdbWriter
    {
        SkapiThriftConnect connection = null;
        List<SkNowval> nval = new List<SkNowval>();

        public bool Connect(string ip, int port, string user, string passwrod)
        {
            int errorCode = -1;
            int cnt = 0;

            connection = new SkapiThriftConnect(ip, port, user, passwrod, out errorCode);
            while (errorCode != 0 && cnt < 3)
            {
                connection = new SkapiThriftConnect(ip, port, user, passwrod, out errorCode);
                Thread.Sleep(10000);
                cnt++;
            }

            return errorCode == 0 ? true : false;
        }

        Dictionary<long, List<SkNowval>> buffer = new Dictionary<long, List<SkNowval>>();

        public void Write(DateTime time, Int32[] ipids, decimal[] values)
        {
            for (int i = 0; i < ipids.Length; i++)
            {
                SkNowval nv = new SkNowval();
                nv.Ipid = ipids[i];
                nv.Value.Value = values[i];
                nv.Value.Status = 1;
                nv.Value.Time = time;
                if (buffer.ContainsKey(ipids[i]))
                {
                    buffer[ipids[i]].Add(nv);
                }
                else
                {
                    List<SkNowval> lst = new List<SkNowval>();
                    lst.Add(nv);
                    buffer.Add(ipids[i], lst);
                }
            }

            if (buffer.Count > 1000)
            {
                for (; buffer.Count != 0;)
                {
                    List<SkNowval> sl = new List<SkNowval>();
                    foreach (List<SkNowval> lst in buffer.Values)
                    {
                        int cnt = lst.Count > 100 ? 100 : lst.Count;
                        sl.AddRange(lst.GetRange(0, cnt));
                        lst.RemoveRange(0, cnt);
                    }

                    int errorCode = -1;
                    while (errorCode != 0)
                    {
                        errorCode = 0;
                        connection.SetNowValue(sl, out errorCode);
                        //log.Write(errorCode + "");
                        if (errorCode != 0)
                        {
                            Thread.Sleep(20);
                        }
                    }

                    for (int i = 0; i < sl.Count; i++)
                    {
                        if (buffer.ContainsKey(sl[i].Ipid))
                        {
                            if (buffer[sl[i].Ipid].Count == 0)
                            {
                                buffer.Remove(sl[i].Ipid);
                            }
                        }
                    }

                    sl.Clear();
                }
            }
        }

        public void Write1(DateTime time, Int32[] ipids, decimal[] values)
        {
            for (int i = 0; i < ipids.Length; i++)
            {
                SkNowval nv = new SkNowval();
                nv.Ipid = ipids[i];
                nv.Value.Value = values[i];
                nv.Value.Status = 1;
                nv.Value.Time = time;
                nval.Add(nv);
            }

            if (nval.Count >= 1000)
            {
                int errorCode = -1;
                while (errorCode != 0)
                {
                    connection.SetNowValue(nval, out errorCode);
                    //log.Write(errorCode + "");
                    if (errorCode != 0)
                    {
                        Thread.Sleep(20);
                    }
                }
                nval.Clear();
            }
        }

        public void Close()
        {
            if (buffer.Count > 0)
            {
                for (; buffer.Count != 0;)
                {
                    List<SkNowval> sl = new List<SkNowval>();
                    foreach (List<SkNowval> lst in buffer.Values)
                    {
                        int cnt = lst.Count > 100 ? 100 : lst.Count;
                        sl.AddRange(lst.GetRange(0, cnt));
                        lst.RemoveRange(0, cnt);
                    }
                    int errorCode = -1;
                    while (errorCode != 0)
                    {
                        //errorCode = 0;
                        connection.SetNowValue(sl, out errorCode);
                        //log.Write(errorCode + "");
                        if (errorCode != 0)
                        {
                            Thread.Sleep(20);
                        }
                    }

                    for (int i = 0; i < sl.Count; i++)
                    {
                        if (buffer.ContainsKey(sl[i].Ipid))
                        {
                            if (buffer[sl[i].Ipid].Count == 0)
                            {
                                buffer.Remove(sl[i].Ipid);
                            }
                        }
                    }

                    sl.Clear();
                }
            }
        }

        public Int32[] GetIpid(string[] cpids, bool addIfNotExsit)
        {
            int errorcode;
            List<long> ipids = new List<long>();
            List<string> cpidlist = new List<string>();
            cpidlist.AddRange(cpids);
            connection.CpidToIpid(cpidlist, out ipids, out errorcode, addIfNotExsit);
            List<Int32> iipids = new List<int>();
            foreach (long i in ipids)
                iipids.Add((int)i);
            return iipids.ToArray();
        }
    }
}
