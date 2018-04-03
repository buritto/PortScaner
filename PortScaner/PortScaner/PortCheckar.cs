using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace PortScaner
{
    public enum TransportProtocol
    {
        Udp = 1,
        Tcp = 2
    }
    //-sT remote_host
    public abstract class PortCheckar
    {
        protected int BeginPort;
        protected int FinishPort;
        protected readonly IPAddress IpHost;//=  Dns.GetHostAddresses("cs.usu.edu.ru")[0];// IPAddress.Parse("127.0.0.1");
        protected List<Task<List<int>>> TasksPool;
        protected int portForTask = 20;
        private string logPath = "PortScaner.log";
        private readonly TransportProtocol protocol;
        protected PortCheckar(int beginPort, int finishPort, TransportProtocol protocol, string hostName)
        {
            //cs.usu.edu.ru
            this.protocol = protocol;           
            IpHost = Dns.GetHostAddresses(hostName)[0];
            if (hostName == "localhost")
                IpHost = IPAddress.Parse("127.0.0.1");
            if (beginPort > finishPort)
            {
                var port = beginPort;
                beginPort = finishPort;
                finishPort = port;
            }
            BeginPort = beginPort;
            FinishPort = finishPort;
            TasksPool = new List<Task<List<int>>>();
        }

        public void GetOpenPorts()
        {
            var lastBlock = 0;
            var blocks = GetBlocks();
            foreach (var block in blocks)
            {
                TasksPool.Add(new Task<List<int>>(() => CheckPorts(block.Item1, block.Item2)));
            }
            Parallel.ForEach(TasksPool, task => task.Start());
            Task.WaitAll(TasksPool.ToArray());
            var openPorts = new List<int>();
            foreach (var task in TasksPool)
            {
                openPorts.AddRange(task.Result);
            }
            SaveToLogFile(openPorts);
        }
        // число портов на таск  - потестить
        private IEnumerable<Tuple<int, int>> GetBlocks()
        {
            var wholePart = (FinishPort - BeginPort) / portForTask;
            var residue = (FinishPort - BeginPort) % portForTask;
            var blocks = new List<Tuple<int, int>>();
            var begin = BeginPort;
            for (var i = 0; i < wholePart; i++)
            {
                blocks.Add(Tuple.Create(begin, begin + portForTask - 1));
                begin += portForTask;
            }
            blocks.Add(Tuple.Create(begin, begin + residue));
            return blocks;
        }

        private void SaveToLogFile(List<int> openPorts)
        {
            using (var sw = File.CreateText(protocol + logPath))
            {
                sw.WriteLine($"Open {protocol} ports");
                openPorts.ForEach(port => sw.WriteLine(port));
            }
        }

        public abstract List<int> CheckPorts(int startPort, int finishPort);
    }
}
