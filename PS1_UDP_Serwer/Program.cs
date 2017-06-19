using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace PS1_UDP_Serwer
{
    class Program
    {
        IPAddress IP;
        String stringIP;

        static void Main(string[] args)
        {
            Program program = new Program(); //tworze objekt klasy program, zeby miec dostep do wszystkich jej pol oraz metod

            program.isValidIP();
            

            UdpClient client = new UdpClient();

            client.ExclusiveAddressUse = false;
            IPEndPoint localEp = new IPEndPoint(IPAddress.Any, 2222);
            client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            client.ExclusiveAddressUse = false;

            client.Client.Bind(localEp);
            
            client.JoinMulticastGroup(program.IP);

            while (true)
            {
                Byte[] data = client.Receive(ref localEp);
                string strData = Encoding.Unicode.GetString(data);
                Console.WriteLine(strData);
            }
        }

        public void isValidIP()
        {
            uint intIP=0;
            uint intMinIP=0;
            uint intMaxIP=0;
            Console.Write("Podaj adres multicastowy: ");
            stringIP = Console.ReadLine();                  // ip w stringu
            

            try
            {
                IP = IPAddress.Parse(stringIP);                 // ip 

                byte[] bytes = IP.GetAddressBytes();
                Array.Reverse(bytes); // flip big-endian(network order) to little-endian
                intIP = BitConverter.ToUInt32(bytes, 0);

                IPAddress minIP = IPAddress.Parse("224.0.0.0");
                bytes = minIP.GetAddressBytes();
                Array.Reverse(bytes); 
                intMinIP = BitConverter.ToUInt32(bytes, 0);

                IPAddress maxIP = IPAddress.Parse("239.255.255.255");
                bytes = maxIP.GetAddressBytes();
                Array.Reverse(bytes); 
                intMaxIP = BitConverter.ToUInt32(bytes, 0);

                if (intIP >= intMinIP && intIP <= intMaxIP)
                {
                    Console.WriteLine("IP Nalezy do grupy adresow multicastowych.");

                }

                else
                {
                    Console.WriteLine("IP jest spoza zakresu adresow multicastowych.\n");
                    isValidIP();
                }

            }
            catch (System.FormatException e)
            {
                Console.WriteLine("Podana wartosc nie jest poprawnym adresem IP\n");
                isValidIP();
            }


            
        }
    }
}
