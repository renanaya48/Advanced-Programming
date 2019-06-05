using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlightSimulator.Model;

namespace FlightSimulator.ViewModels
{
    class ManualViewModel
    {
        private string pathElevator = "/controls/flight/elevator";
        private string pathAileron = "/controls/flight/aileron";
        private string pathRudder = "/controls/flight/rudder";
        private string pathThrottle = "/controls/engines/cirrent-engine/throttle";
        private ManualModel manualModel;

        public ManualViewModel()
        {
            this.manualModel = new ManualModel();
        }

        public double Elevator
        {
            set => manualModel.SendCommand("set " + pathElevator + " " + Convert.ToString(value));
        }

        public double Aileron
        {
            set => manualModel.SendCommand("set " + pathAileron + " " + Convert.ToString(value));
        }

        public double Rudder
        {
            set => manualModel.SendCommand("set " + pathRudder + " " + Convert.ToString(value));
        }

        public double Throttle
        {
            set => manualModel.SendCommand("set " + pathThrottle + " " + Convert.ToString(value));
        }
    }
}

        