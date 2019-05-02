using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Excercise_1
{
    //FunctionsContainer class
    public class FunctionsContainer
    {
        //the delegate will hold functions that get and return double
        public delegate double FuncToDo(double nameToCal);
        //member: dictionary that will hold string and function
        private Dictionary<string, FuncToDo> funcMap;

        //constructor
        public FunctionsContainer()
        {
            this.funcMap = new Dictionary<string, FuncToDo>();
        }

        //properties
        public Dictionary<string, FuncToDo> FuncMap
        {
            get { return this.funcMap; }
            set { this.funcMap = value; }
        }

        /*
         * the function makes the operator [] ready to use
         * @param [string str] - the string to convert
         * @return the function 
         */
        public FuncToDo this[string str]
        {
            get
            {
                //if the mission is not in the map
                if (!funcMap.ContainsKey(str))
                {
                    //add to the map
                    this.funcMap.Add(str, value => value);
                    
                }
                //return the mission to do
                return this.FuncMap[str];
            }
            set
            {
                //if the mission isnt in the map
                if (!funcMap.ContainsKey(str))
                {
                    //add to the map
                    this.funcMap.Add(str, value);
                }
                else
                {
                    //put the value in the right place
                    this.funcMap[str] = value;
                }

            }
        }
        /*
         * getAllMissions functions returns all the missions in a list
         * @return List of strings of all the missions
         */
        public List<string> getAllMissions()
        {
            //initiolize the list
            List<string> allMissionsList = new List<string>();
            //loop on all over the map with the missions
            foreach (var pair in this.funcMap)
            {
                //add the mission to the list
                allMissionsList.Add(pair.Key);
            }
            //return the list with all the missions
            return allMissionsList;
        }
    }
}
