using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace PortScaner
{
    public class UdpPortChecker : PortCheckar
    {
        public UdpPortChecker(int beginPort, int finishPort) : base(beginPort, finishPort, TransportProtocol.Udp)
        {
        }

        public override List<int> CheckPorts(int startPort, int finishPort)
        {
            var openPorts = new List<int>();
            for (var port = startPort; port <= finishPort; port++)
            {
                using (var udp = new UdpClient())
                {
                    try
                    {
                        var remoteIpEndPoint = new IPEndPoint(localHost, port);
                        var sendBytes = Encoding.ASCII.GetBytes("Are you open ?");
                        udp.Send(sendBytes, sendBytes.Length, remoteIpEndPoint);
                        var task = udp.ReceiveAsync();
                        var timeOut = Task.Delay(100);
                        if (Task.WhenAny(task, timeOut).Result != task)
                        {
                            openPorts.Add(port);
                        }
                    }
                    catch (SocketException e)
                    {
                        //ignore
                    }
                }
            }
            return openPorts;
        }
    }
}
