using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSimulator
{
    class Commands
    {
        private TcpClient client;
        private StreamWriter writer;

        public bool IsConnect
        {
            get;
            set;
        } = false;

        #region Singleton

        private static Commands m_instance = null;

        public static Commands Instance
        {
            get
            {
                if (m_instance == null)
                {
                    m_instance = new Commands();
                    //  m_instance.ServerConnect("127.0.0.1", 5402);
                }
                return m_instance;
            }
        }
        #endregion

        public void Init()
        {
            m_instance = null;
        }

        public void ServerConnect(string ip, int port)
        {
            //ip = "127.0.0.1";
            //port = 5402;

            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(ip), port);
            client = new TcpClient();
            //TcpListener listener = new TcpListener(ep);
            //listener.Start();

            //when client is trying to connect
            while (!client.Connected)
            {
                try
                {
                    // Console.WriteLine("Waiting for client connections...");
                    client.Connect(ep);
                }
                catch (Exception)
                {
                    //TODO: throw exp
                }
            }

            Console.WriteLine("Client connected");
            IsConnect = true;

            writer = new StreamWriter(client.GetStream());

            //client = listener.AcceptTcpClient();

            /*
            using (NetworkStream stream = client.GetStream())
            using (StreamReader reader = new StreamReader(stream))
            using (writer = new StreamWriter(stream))
            {
                Console.WriteLine("Waiting for a number");
                string num = reader.ReadLine();
                Console.WriteLine(num);
                writer.Write(num);
                writer.Flush();
            }
            */
            //client.Close();
            //listener.Stop();
        } //end of ServerConnect function

        public void SendCommands(string input)
        {
            if (string.IsNullOrEmpty(input)) return;

            string[] commands = input.Split('\n');

            foreach (string command in commands)
            {
                string finalCommand = command + "\r\n";

                writer.Write(finalCommand);
                writer.Flush();

                //writer.Write(System.Text.Encoding.ASCII.GetBytes(finalCommand));
                System.Threading.Thread.Sleep(2000);
            }
        }
    }
}

