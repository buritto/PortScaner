using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace PortScaner
{
    public class UdpPortChecker : PortCheckar
    {
        public UdpPortChecker(int beginPort, int finishPort, string hostName)
            : base(beginPort, finishPort, TransportProtocol.Udp, hostName)
        {
        }

        public override List<int> CheckPorts(int startPort, int finishPort)
        {
            var udp = new UdpClient();
            var openPorts = new List<int>();
            for (var i = startPort; i <= finishPort; i++)
            {
                using (var socket = new Socket(SocketType.Dgram, ProtocolType.Udp))
                {
                    socket.ReceiveTimeout = 2000;
                    try
                    {
                        var remoteServer = new IPEndPoint(IpHost, i);
                        socket.SendTo(Encoding.ASCII.GetBytes("Are you open"), remoteServer);
                        var buffer = new byte[4096];
                        socket.Receive(buffer);
                    }
                    catch (SocketException e)
                    {
                        if (e.ErrorCode != 10060) continue;
                        openPorts.Add(i);
                        Console.WriteLine(i);
                    }
                }
            }
            return openPorts;
        }
    }
}
