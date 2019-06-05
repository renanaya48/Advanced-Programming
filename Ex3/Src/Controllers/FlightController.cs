using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using Ex3.Models;

namespace Ex3.Controllers
{
    public class FlightController : Controller
    {
        //members
        private ConnectFlight connect;
        private bool read;

        // GET: Flight
        [HttpGet]
        public ActionResult display(string ip, int port, int time)
        {
            //check the URL - if is ip and port or file name
            int ipOrDoc = CheckURL(ip);
            //if its file name
            if (ipOrDoc == 0)
            {
                //read from file
                ConnectFlight.Instance.shouldRead = true;
                Session["time"] = port;
                ReadFromFile(ip);
            }
            //ip and port
            else
            {
                if (ipOrDoc == 1)
                {
                    //connect to the server
                    ConnectFlight.Instance.shouldRead = false;
                    connect = ConnectFlight.Instance;
                    connect.ServerConnect(ip, port);

                    Session["time"] = time;
                }
            }

            //view
            return View();
        }

        /* 
         * which kind of display
         */
        int CheckURL(string str)
        {
            string[] words = str.Split('.');
            //if it possible to split according to ., its ip and port
            if (words.Length > 1)
            {
                return 1;
            }
            return 0;
        }

        /**
         * get the data of the flight
         */
        [HttpPost]
        public string GetFlightData()
        {
            //if should get the data from a file
            if (ConnectFlight.Instance.shouldRead)
            {
                string[] lines = ConnectFlight.Instance.Flight.linesFromFile;
                //no lines to read
                if (lines.Length == 0 || lines[0] == "")
                {
                    ConnectFlight.Instance.Flight.Lat = "?";
                    ConnectFlight.Instance.Flight.Lon = "?";

                    //return null;
                }
                else
                {
                    //read the data, split according to ";"
                    string[] tempLine = lines[0].Split(';');
                    ConnectFlight.Instance.Flight.Lat = tempLine[0];
                    ConnectFlight.Instance.Flight.Lon = tempLine[1];
                    ConnectFlight.Instance.Flight.linesFromFile = ConnectFlight.Instance.Flight.linesFromFile.Skip(1).ToArray();
                }
            }
            else
            {
                string[] result = new string[4];
                result = ConnectFlight.Instance.SendCommands();
                ConnectFlight.Instance.Flight.Lat = ConnectFlight.Instance.PhraserValue(result[0]);
                ConnectFlight.Instance.Flight.Lon = ConnectFlight.Instance.PhraserValue(result[1]);
            }
            var fly = ConnectFlight.Instance.Flight;
            return ToXml(fly);
        }

        [HttpPost]
        public string GetFlightDataForSave()
        {
            string[] result = new string[4];
            result = ConnectFlight.Instance.SendCommands();
            ConnectFlight.Instance.Flight.Lat = ConnectFlight.Instance.PhraserValue(result[0]);
            ConnectFlight.Instance.Flight.Lon = ConnectFlight.Instance.PhraserValue(result[1]);
            ConnectFlight.Instance.Flight.Rudder = ConnectFlight.Instance.PhraserValue(result[2]);
            ConnectFlight.Instance.Flight.Throttle = ConnectFlight.Instance.PhraserValue(result[3]);

            var fly = ConnectFlight.Instance.Flight;
            ConnectFlight.Instance.WriteData(ConnectFlight.Instance.fileName);
            return ToXml(fly);
        }

        private string ToXml(Flight flight)
        {
            //if (Models.ConnectFlight.Instance.IsConnect)
            //{
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings();
            XmlWriter writer = XmlWriter.Create(sb, settings);
            writer.WriteStartDocument();
            writer.WriteStartElement("Flight");

            writer.WriteElementString("lat", flight.Lat);
            writer.WriteElementString("lon", flight.Lon);


            //flight.ToXml(writer);

            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();

            Console.Write(sb.ToString());
            return sb.ToString();
            //    }
            //  return null;
        }

        public const string SCENARIO_FILE = "~/App_Data/{0}.txt";
        public void ReadFromFile(string fileName)
        {
            string path = System.Web.HttpContext.Current.Server.MapPath(String.Format(SCENARIO_FILE, fileName));
            string[] lines = System.IO.File.ReadAllLines(path);
            ConnectFlight.Instance.Flight.linesFromFile = lines;
        }



        /*public string ReadFromFile(string fileName)
        {
            string line;
            //string fileName = "file1";
            string path = System.Web.HttpContext.Current.Server.MapPath(String.Format(SCENARIO_FILE, fileName));
            //string path = "C:/Users/ornab/source/repos/ReutBaruch/Ex3/Ex3/fonts/" + fileName + ".txt";
            System.IO.StreamReader file = new System.IO.StreamReader(path);
            line = file.ReadLine();
            if (line != null)
            {
                string[] words = line.Split(';');
                if (words.Length != 4)
                {
                    Console.WriteLine("problem");
                }
                else
                {
                    StringBuilder sb = new StringBuilder();
                    XmlWriterSettings settings = new XmlWriterSettings();
                    XmlWriter writer = XmlWriter.Create(sb, settings);
                    writer.WriteStartDocument();
                    writer.WriteStartElement("Flight");
                    writer.WriteElementString("lat", words[0]);
                    writer.WriteElementString("lon", words[1]);
                    writer.WriteElementString("rudder", words[2]);
                    writer.WriteElementString("throttle", words[3]);
                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                    writer.Flush();
                    Console.Write(sb.ToString());
                    return sb.ToString();
                }
            }
            //}
            return "blabls";
        }
        */
        [HttpPost]
        public string Search(string data)
        {
            //ConnectFlight.Instance.ReadData(data);
            return ToXml(ConnectFlight.Instance.Flight);
        }

        public string Index()
        {
            return "Welcome to our Project! Please enter a URL";
        }

        [HttpGet]
        public ActionResult save(string ip, int port, int time, int countDown, string fileName)
        {
            connect = ConnectFlight.Instance;
            connect.ServerConnect(ip, port);
            Session["time"] = time;
            Session["countDown"] = countDown;
            connect.fileName = fileName;
            return View();
        }
    }
}