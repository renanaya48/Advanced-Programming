using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSimulator.Model
{
    class AutoPilotModel
    {
        private Commands command;

        public AutoPilotModel()
        {
            this.command = Commands.Instance;
            // this.command.ServerConnect("127.0.0.1", 5400);

        }

        public void scriptCommands(string data)
        {
            if (!Commands.Instance.IsConnect)
            {
                this.command.ServerConnect("127.0.0.1", 5400);
            }
            Commands.Instance.SendCommands(data);
        }
    }
}


