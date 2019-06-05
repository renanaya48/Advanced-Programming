using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FlightSimulator.Model
{
    class ManualModel
    {
        private double elevator;
        private double throttle;
        private double rudder;
        private double aileron;

        private Info info;
        private Commands command;

        public ManualModel()
        {
            this.info = Info.Instance;
            this.command = Commands.Instance;
            // this.command.ServerConnect("127.0.0.1", 5400);

        }

        public double Elevator
        {
            set { this.elevator = value; }
        }

        public double Throttle
        {
            set { this.throttle = value; }
        }

        public double Rudder
        {
            set { this.rudder = value; }
        }

        public double Aileron
        {
            set { this.aileron = value; }
        }


        public void SendCommand(string command)
        {
           /* if (!Commands.Instance.IsConnect)
            {
                this.command.ServerConnect("127.0.0.1", 5400);
            }*/
            //Commands.Instance.SendCommands(command);

             if (Commands.Instance.IsConnect)           
             {
                 new Thread(delegate ()
                 {
                     Commands.Instance.SendCommands(command);
                 }).Start();
             }
        }
    }
}



