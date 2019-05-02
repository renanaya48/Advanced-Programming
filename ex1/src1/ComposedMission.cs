using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Excercise_1
{
    public class ComposedMission : IMission
    {
        //members
        private readonly string name;
        public string type;
        private readonly List<FunctionsContainer.FuncToDo> listOfFunc;
        private double result;
        public event EventHandler<double> OnCalculate;

        /**
         * constructor.
         * the constructor gets a name
         */
        public ComposedMission(string name1)
        {
            this.name = name1;
            this.type = "Compose";
            this.listOfFunc = new List<FunctionsContainer.FuncToDo>();
        }
        /**
         * add a function to the list of the functions.
         * @param func - the function to add
         * @return ComposedMission
         */
        public ComposedMission Add(FunctionsContainer.FuncToDo func)
        {
            this.listOfFunc.Add(func);
            return this;
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
            //put the value in the result
            this.result = value;
            //a loop all over the functions
            foreach (FunctionsContainer.FuncToDo i in this.listOfFunc)
            {
                //calculate
                this.result = i(this.result);
            }
            //if not null
            if (this.OnCalculate != null)
            {
                //calculate
                OnCalculate(this, this.result);
            }

            //return the result
            return this.result;
            /*
            if (OnCalculate != null)
            {
                OnCalculate(this, result);
            }
            return result;
            */
        }
    }
}
