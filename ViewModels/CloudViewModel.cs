﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Maui.Alerts;
using CG.Web.MegaApiClient;
using IMP_reseni.Services;
using System.Collections.ObjectModel;
using System.Collections;
using System.Globalization;

namespace IMP_reseni.ViewModels
{
    public class CloudViewModel: INotifyPropertyChanged
    {
        private CloudService cloudService;

        public bool IsEnable
        {
            set 
            {
                cloudService.IsEnabled=value;
                OnPropertyChanged();
            }
            get 
            {
                return cloudService.IsEnabled;
            }
        }

        private string _setTime;
        public string SetTime
        {
            set 
            { 
                SetProperty(ref _setTime, value);
                SetTimer(value);
            }
            get { return _setTime; }
        }

        private string _email;
        public string Email 
        {
            set { SetProperty(ref _email, value); }
            get { return _email; }
        }

        private string _password;
        public string Password
        {
            set { SetProperty(ref _password, value); }
            get { return _password; }
        }

        private bool _isValid;
        public bool IsValid
        {
            set { SetProperty(ref _isValid, value); }
            get { return _isValid; }
        }
        public ICommand GetCommand { get; set; }
        public ICommand ManualSaveCommand { get; set; }

        public ICommand TestCommand { get; set; }
        public List<string> TimeTable { get;private set; }
        private IDictionary<string, int> Times;
        public CloudViewModel(CloudService cloudService)
        {
            Times = new Dictionary<string,int>
            {
                {  "Nikdy" ,0},
                { "5 minut" , 5 },
                { "15 minut" , 15 },
                { "30 minut" , 30 },
                { "1 hodina" , 60 },
                {  "2 hodiny",2*60  },
                { "5 hodin" , 5 * 60 },
                { "12 hodin" , 12 * 60 }
            };
            TimeTable = new List<string>(Times.Keys);
            this.cloudService = cloudService;
            //cloudService = App.cloudService;
            //IsEnable = cloudService.IsEnabled;
            Email = "latysa123@gmail.com";
            Password = "Kouble29+";
            TestCommand = new Command(
            () => 
            { 
            MegaApiClient client = new MegaApiClient();
            client.Login(Email, Password);
            IEnumerable<INode> nodes = client.GetNodes();

            INode root = nodes.Single(x => x.Type == NodeType.Root);
            INode myFolder = client.CreateFolder("Upload", root);

            string _appDirectory = FileSystem.Current.AppDataDirectory;
                //App.saveholder.Save("IMP.json");
            string _fileName = "IMP.json";

            client.UploadFile(_appDirectory + "/" + _fileName, myFolder);
            client.Logout();
            });

            GetCommand = new Command<bool>(
            canExecute: (bool IsValid) =>
            {
                return IsValid;
            },
            execute: (bool IsValid) =>
            {
                cloudService.SetLogin(Email, Password);
                //MegaApiClient client = new MegaApiClient();
                //client.Login(Email, Password);
                //IEnumerable<INode> nodes = client.GetNodes();
                //INode parent = nodes.Single(n => n.Name == "Upload");
                //nodes = client.GetNodes(parent);
                //foreach (var item in nodes)
                //{
                //    source.Add(item.Name);
                //}
            });

            ManualSaveCommand = new Command(
            () =>
            {
                cloudService.UploadFile();
                Toast.Make("Uspěšně uloženo").Show();
            });
        }
        private void SetTimer(string minutes)
        {
            cloudService.SetInterval(Times[minutes]);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Object.Equals(storage, value))
                return false;

            storage = value;

            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
