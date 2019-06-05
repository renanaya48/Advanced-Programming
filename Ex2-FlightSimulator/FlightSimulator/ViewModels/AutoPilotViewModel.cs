using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.ComponentModel;
using FlightSimulator.Model;
using System.Windows;
using System.Windows.Controls;

namespace FlightSimulator.ViewModels
{
    class AutoPilotViewModel: BaseNotify
    {
        private string data;
        private AutoPilotModel autoModel;
        private Brush brush = Brushes.White;
        private ICommand okCommand;
        private ICommand clearCommand;
        private string _AssignedTo;

        public AutoPilotViewModel()
        {
            this.autoModel = new AutoPilotModel();
            // clearCommand=new AutoPilotModel()
        }


        public string Data
        {
            get
            {
                return this.data;
            }
            set
            {
                this.data = value;

                if (string.IsNullOrEmpty(Data)) Color = Brushes.White;
                else if ((!String.IsNullOrEmpty(Data)) && (Color == Brushes.White))
                {
                    Color = Brushes.LightPink;
                }
            }
        }

        public Brush Color
        {
            get
            {
                return this.brush;
            }

            set
            {
                brush = value;
                NotifyPropertyChanged("Color");
            }
        }

        public ICommand OkCommand
        {
            get
            {
                return okCommand ?? (okCommand = new CommandHandler(() =>
                {
                    string send = Data;
                    Data = "";
                    Color = Brushes.White;
                    autoModel.scriptCommands(send);
                }));
            }
        }
        public ICommand ClearCommand
        {
            get
            {
                return clearCommand ?? (clearCommand = new CommandHandler(() =>
                {
                    Data = "";
                    Color = Brushes.White;
                    NotifyPropertyChanged(Data);
                }));
            }
        }
        public string AssignedTo
        {
            get
            {
                return _AssignedTo;
            }
            set
            {
                this.data = " ";
                //AssignedTo = string.Empty;
                /*
                if (_AssignedTo != value)
                {
                    _AssignedTo = value;

                    //RaisePropertyChanged("AssignedTo");
                }*/
            }
        }

        //void RaisePropertyChanged(string prop) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));


        //ClearTextBox

        /*
        public string MyTextProperty
        {
            get { return _text; ; }
            set { _text = value; OnPropertyChanged(" MyTextProperty"); }
        }
        */
    }
}
        