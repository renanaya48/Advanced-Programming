using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using FlightSimulator.ViewModels;


namespace FlightSimulator
{
    class Info: BaseNotify
    {
        private TcpClient client;
        private TcpListener listener;
        private StreamReader reader;

        private double? lat;
        private double? lon;

        public bool IsConnect
        {
            get;
            set;
        } = false;

        public bool ShouldStop
        {
            get;
            set;
        } = false;

        private Info()
        {
            lat = null;
            lon = null;
        }

        #region Singelton
        private static Info m_instance = null;
        public static Info Instance
        {
            get
            {
                if (m_instance == null)
                {
                    m_instance = new Info();
                    // m_instance.StartConnect("127.0.0.1", 5400);
                }
                return m_instance;
            }
        }
        #endregion

        public double? Lat
        {
            get
            {
                return this.lat;
            }

            set
            {
                this.lat = value;
                NotifyPropertyChanged("Lat");

            }
        }

        public double? Lon
        {
            get
            {
                return this.lon;
            }

            set
            {
                this.lon = value;
                NotifyPropertyChanged("Lon");

            }
        }

        public void StartConnect(string ip, int port)
        {
            //ip = "127.0.0.1";
            //port = 5400;
            if (listener != null)
            {
                EndConnect();
            }

            listener = new TcpListener(new IPEndPoint(IPAddress.Parse(ip), port));
            listener.Start();
        }

        public void EndConnect()
        {
            IsConnect = false;
            listener.Stop();
            client.Close();
        }

        public string[] GetInput()
        {
            string commandInput = "";
            string[] input;

            if (!IsConnect)
            {
                IsConnect = true;
                client = listener.AcceptTcpClient();
                reader = new StreamReader(client.GetStream());
            }

            commandInput = reader.ReadLine();
            input = commandInput.Split(',');
            string[] result = { input[0], input[1] };

            return result;

        }

        /*     client.Connect(ep);
             Console.WriteLine("You are connectedddddd");

             using (NetworkStream stream = client.GetStream())
             using (StreamReader reader = new StreamReader(stream))
             using (StreamWriter writer = new StreamWriter(stream))
             {
                 // Send data to server
                 // Console.Write("Please enter a number: ");
                 //int num = int.Parse(Console.ReadLine());
                 //writer.Write(num);
                 // Get result from server
                 string result = "set controls/flight/rudder 1";
                 result = result + "\n";
                 Console.WriteLine(result);
                 writer.WriteLine(result);
             }
             client.Close();
         }*/
    }
}

