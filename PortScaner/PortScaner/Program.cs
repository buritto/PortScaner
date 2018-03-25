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
                if (args[2] == "Tcp")
                {
                    var tcpChecker = new TcpPortChecker(begin, finish);
                    tcpChecker.GetOpenPorts();
                }

                if (args[2] == "Udp")
                {
                    var udpChecker = new UdpPortChecker(begin, finish);
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
