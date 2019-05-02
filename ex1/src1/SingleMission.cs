using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Excercise_1
{
    public class SingleMission : IMission
    {
        //members
        private readonly string name;
        private readonly string type;
        public event EventHandler<double> OnCalculate;
        private readonly FunctionsContainer.FuncToDo function;

        //constructor
        public SingleMission (FunctionsContainer.FuncToDo func, string name1)
        {
            this.function = func;
            this.name = name1;
            this.type = "Single";

        }
        /**
         * get function, returns the name.
         * @return tne name
         */
        public string Name { get { return this.name; } }
        /**
         * get function, returns the type.
         * @return tne type
         */
        public string Type { get { return this.type; } }

       /**
       * Calculate function
       * @param value- the value to calculate
       */
        public double Calculate(double value)
        {
            //answer will be the return parameter
            double answer = function(value);
            //if there are mission to do
            if (this.OnCalculate != null)
            {
                //calaulate
                OnCalculate(this, answer);
            }

            return answer;
        }
    }
}
