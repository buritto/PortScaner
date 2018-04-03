using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace PortScaner
{
    public class TcpPortChecker : PortCheckar
    {
        public TcpPortChecker(int beginPort, int finishPort, string hostName)
            : base(beginPort, finishPort, TransportProtocol.Tcp, hostName)
        {
        }

        public override List<int> CheckPorts(int startPort, int finishPort)
        {
            var openPorts = new List<int>();
            for (var port = startPort; port <= finishPort; port++)
            {
                using (var tcp = new TcpClient())
                {
                    try
                    {
                        tcp.Connect(IpHost, port);
                        Console.WriteLine(port);
                        openPorts.Add(port);
                    }
                    catch (Exception e)
                    {
                        // ignored
                        //Console.WriteLine(e);
                    }
                }
            }
            return openPorts;
        }
    }
}
