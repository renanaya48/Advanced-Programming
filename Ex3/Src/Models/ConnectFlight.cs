using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.IO;

namespace Ex3.Models
{
    public class ConnectFlight
    {
        //members
        private TcpClient client;
        private StreamWriter writer;
        public string ip { get; set; }
        public int port { get; set; }
        public int time { get; set; }
        public string fileName { get; set; }
        public bool shouldRead { get; set; }


        public bool IsConnect
        {
            get;
            set;
        } = false;

        #region Singleton

        private static ConnectFlight m_instance = null;

        //Instance ConncectFlight
        public static ConnectFlight Instance
        {
            get
            {
                if (m_instance == null)
                {
                    m_instance = new ConnectFlight();
                }
                return m_instance;
            }
        }
        #endregion

        public Flight Flight { get; private set; }

        //constructor
        public ConnectFlight()
        {
            Flight = new Flight();
        }
        //initiolize
        public void Init()
        {
            m_instance = null;
        }

        public const string SCENARIO_FILE = "/App_Data/{0}.txt";
        /*
         * wrute the data to a file
         */
        public void WriteData(string fileName)
        {
            string path = HttpContext.Current.Server.MapPath(String.Format(SCENARIO_FILE, fileName));
            //if the file doesnt exists, creat
            if (!File.Exists(path))
            {
                File.Create(path).Dispose();
            }

            //write the data to the file
            using (System.IO.StreamWriter file = new StreamWriter(path, true))
            {
                file.Write(Flight.Lat + ";");
                file.Write(Flight.Lon + ";");
                file.Write(Flight.Rudder + ";");
                file.WriteLine(Flight.Throttle);
            }

        }

        /*
         * connect to the server
         */
        public void ServerConnect(string ip, int port)
        {

            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(ip), port);
            client = new TcpClient();

            //when client is trying to connect
            while (!client.Connected)
            {
                //try to connct
                try
                {
                    // Console.WriteLine("Waiting for client connections...");
                    client.Connect(ep);
                }
                //connect fail
                catch (Exception e)
                {
                    throw (e);
                }
            }
            //connect to server
            IsConnect = true;
        }

        /**
         * the function send the right command
         * 
         **/
        public string[] SendCommands()
        {
            NetworkStream stream = client.GetStream();
            writer = new StreamWriter(stream);
            StreamReader reader = new StreamReader(stream);
            string command = "";
            string[] result = new string[4];

            //lat
            command = "get /position/latitude-deg\r\n";

            writer.Write(command);
            writer.Flush();
            result[0] = reader.ReadLine();

            //lon
            command = "get /position/longitude-deg\r\n";

            writer.Write(command);
            writer.Flush();
            result[1] = reader.ReadLine();

            //rudder
            command = "get /controls/flight/rudder\r\n";

            writer.Write(command);
            writer.Flush();
            result[2] = reader.ReadLine();

            //throttle
            command = "get /controls/engines/engine/throttle\r\n";

            writer.Write(command);
            writer.Flush();
            result[3] = reader.ReadLine();

            return result;
        }

        /*
         * the function parses the string toPhras
         */
        public string PhraserValue(string toPhras)
        {
            //split according to =
            string[] words = toPhras.Split('=');
            if (words[1] != null)
            {
                words = words[1].Split('\'');
            }
            // double result = Convert.ToDouble(words[1]);

            return words[1];
        }

    }
}