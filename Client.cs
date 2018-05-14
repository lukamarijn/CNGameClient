using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tic_Tac_Toe
{
    class Client
    {
        public NetworkStream networkStream;
        public StreamReader streamReader;
        public TcpClient newClient;
        public List<String> users = new List<String>();

        public void StartClient(string user)
        {
            try
            {
                newClient = new TcpClient("localHost", 12345);
            }
            catch
            {
                Console.WriteLine(
                "Failed to connect to server at {0}:999", "localhost");
                return;
            }

            NetworkStream networkStream = newClient.GetStream();
            streamReader = new StreamReader(networkStream);
            StreamWriter streamWriter = new StreamWriter(networkStream);

            //streamWriter.WriteLine(user);
            //streamWriter.Flush();

            //Console.WriteLine(streamReader.ReadLine());

            while (true)
            {
                Console.WriteLine("...");
                // Buffer to store the response bytes.
                byte[] recv = new Byte[256];

                // Read the first batch of the TcpServer response bytes.
                int bytes = networkStream.Read(recv, 0, recv.Length); //(**This receives the data using the byte method**)

                byte[] a = new byte[bytes];

                for (int i = 0; i < bytes; i++)
                {
                    a[i] = recv[i];
                }

                Console.WriteLine(a);

                Console.WriteLine("2...");
            }
        }

        public void EndClient()
        {
            networkStream.Close();
            newClient.Close();
        }

    }
}
