using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Xml;

namespace Ex3.Models
{
    public class Flight
    {
        //members
        public string Lat { get; set; }
        public string Lon { get; set; }
        public string Rudder { get; set; }
        public string Throttle { get; set; }
        public string fileName { get; set; }
        public string[] linesFromFile { get; set; }

        /*
         * write the data to xml file that will be able to read from
         */
        public void ToXml(XmlWriter writer)
        {
            writer.WriteStartElement("Flight");
            writer.WriteElementString("lon", this.Lon);
            writer.WriteElementString("lat", this.Lat);
            writer.WriteEndElement();
        }


    }

}