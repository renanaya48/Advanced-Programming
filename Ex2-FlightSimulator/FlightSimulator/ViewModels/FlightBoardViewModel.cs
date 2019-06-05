using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlightSimulator.ViewModels;
using FlightSimulator.Model;
using System.ComponentModel;
using System.Windows.Input;
using System.Threading;
using FlightSimulator.Views;

namespace FlightSimulator.ViewModels
{
    class FlightBoardViewModel: BaseNotify
    {
        private FlightBoardModel flightBoardModel;
        private double? lat;
        private double? lon;
        private ICommand settingCommand;
        private ICommand connectCommand;

        public FlightBoardViewModel()
        {
            flightBoardModel = new FlightBoardModel();
            flightBoardModel.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e)
            {
                NotifyPropertyChanged(e.PropertyName);
            };
        }

        public double? Lat
        {
            get
            {
                return this.flightBoardModel.Lat;
            }
        }

        public double? Lon
        {
            get
            {
                return this.flightBoardModel.Lon;
            }
        }

        public ICommand ConnectCommand
        {
            get
            {
                return connectCommand ?? (connectCommand = new CommandHandler(() =>
                ConnectClicked()));
            }
        }
        void ConnectClicked()
        {
            if (flightBoardModel.IsConnect())
            {
                flightBoardModel.Stop();
                Commands.Instance.Init();
                System.Threading.Thread.Sleep(1000);
            }
            new Thread(delegate ()
            {
                Commands.Instance.ServerConnect(ApplicationSettingsModel.Instance.FlightServerIP,
                    ApplicationSettingsModel.Instance.FlightCommandPort);
            }).Start();
            flightBoardModel.Connect(ApplicationSettingsModel.Instance.FlightServerIP,
                ApplicationSettingsModel.Instance.FlightInfoPort);
        }

        public ICommand SettingCommand
        {
            get
            {
                return settingCommand ?? (settingCommand = new CommandHandler(() =>
                SettingClicked()));
            }
        }
        void SettingClicked()
        {
            Settings sett = new Settings();
            sett.ShowDialog();
        }
    }
}


