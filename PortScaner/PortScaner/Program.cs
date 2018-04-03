using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortScaner
{
    class Program
    {

        static void Main(string[] args)
        {
            try
            {
                var begin = int.Parse(args[0]);
                var finish = int.Parse(args[1]);
                var hostName = args[3];
                if (args[2] == "Tcp")
                {
                    var tcpChecker = new TcpPortChecker(begin, finish, hostName);
                    tcpChecker.GetOpenPorts();
                }

                if (args[2] == "Udp")
                {
                    var udpChecker = new UdpPortChecker(begin, finish, hostName);
                    udpChecker.GetOpenPorts();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
