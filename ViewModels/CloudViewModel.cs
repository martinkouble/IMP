using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Views;
using CG.Web.MegaApiClient;
using IMP_reseni.Services;
using System.Collections.ObjectModel;
using System.Collections;
using System.Globalization;
using IMP_reseni.Controls;
using Mopups.Services;

namespace IMP_reseni.ViewModels
{
    public class CloudViewModel: INotifyPropertyChanged
    {
        private CloudService cloudService;

        public List<string> UploadedBackups { get; private set; }

        public Page page {private get; set; }

        public bool IsEnable
        {
            set
            {

                cloudService.IsEnabled = value;
                OnPropertyChanged();

            }
            get 
            {
                return cloudService.IsEnabled;
            }
        }

        public bool CanBeSwitchEnabled
        {
            get 
            {
                return cloudService.Validated;
            }
            set
            {
                OnPropertyChanged("CanBeSwitchEnabled");
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
            get 
            {
                return Times.First(x=>x.Value==cloudService.TimerInterval).Key;
            }
        }


        private string _selectedBackup;
        public string SelectedBackup
        {
            set 
            { 
                SetProperty(ref _selectedBackup, value);
                if (value!=null && value!=null)
                {
                    ShowPromt(value);
                    SelectedBackup = null;
                }
            }
            get { return _selectedBackup; }
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

       // public ICommand TestCommand { get; set; }
        public List<string> TimeTable { get;private set; }
        private IDictionary<string, int> Times;
        public CloudViewModel(CloudService cloudService)
        {
            Email = cloudService.Email;
            Password = cloudService.Password;
            Times = new Dictionary<string,int>
            {
                {  "Nikdy" ,0},
                { "1 minut" , 1 },
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

            if (cloudService.Password!=null && cloudService.Password != "" && cloudService.Email!=null && cloudService.Email!="")
            {
                UploadedBackups = new List<string>(cloudService.GetFilesNames());
            }

            //TestCommand = new Command(
            //() => 
            //{ 
            //MegaApiClient client = new MegaApiClient();
            //client.Login(Email, Password);
            //IEnumerable<INode> nodes = client.GetNodes();

            //INode root = nodes.Single(x => x.Type == NodeType.Root);
            //INode myFolder = client.CreateFolder("Upload", root);

            //string _appDirectory = FileSystem.Current.AppDataDirectory;
            //    //App.saveholder.Save("IMP.json");
            //string _fileName = "IMP.json";

            //client.UploadFile(_appDirectory + "/" + _fileName, myFolder);
            //client.Logout();
            //});
            GetCommand = new Command<bool>(
             canExecute: (bool IsValid) =>
            {
                return IsValid;
            },
             execute: async (bool IsValid) =>
            {
                SpinnerPopup spinnerPopup = new SpinnerPopup();
                await MopupService.Instance.PushAsync(spinnerPopup);
                bool loginSucceded=cloudService.SetLogin(Email, Password);
                if (loginSucceded==true)
                {
                   await Toast.Make("Údaje ověřeny").Show();
                    CanBeSwitchEnabled = true;
                }
                else
                {
                   await Toast.Make("Špatné přihlašovací údaje nebo nejste připojeni k internetu").Show();
                }
                if (cloudService.Password != null && cloudService.Password != "" && cloudService.Email != null && cloudService.Email != "")
                {
                    UploadedBackups = new List<string>(cloudService.GetFilesNames());
                }
                await MopupService.Instance.RemovePageAsync(spinnerPopup);
                /*
                MegaApiClient client = new MegaApiClient();
                client.Login(Email, Password);
                IEnumerable<INode> nodes = client.GetNodes();
                INode parent = nodes.Single(n => n.Name == "Upload");
                nodes = client.GetNodes(parent);
                foreach (var item in nodes)
                {
                    source.Add(item.Name);
                }*/
            });
            ManualSaveCommand = new Command(
            async () =>
            {
                bool answer = await page.DisplayAlert("Potvrzení", "Opravdu si přejete uložit záloho", "Ano", "Ne");
                if (answer == true)
                {
                    await MopupService.Instance.PushAsync(new SpinnerPopup());
                    //cloudService.SetLogin(Email, Password);
                    bool UploadSucceded=cloudService.UploadFile();
                    await MopupService.Instance.PopAsync();
                    if (UploadSucceded==true)
                    {
                       await Toast.Make("Uspěšný upload").Show();
                        if (cloudService.Password != null && cloudService.Password != "" && cloudService.Email != null && cloudService.Email != "")
                        {
                            UploadedBackups = new List<string>(cloudService.GetFilesNames());
                        }
                    }
                    else
                    {
                       await Toast.Make("Došlo k chybě při uploudnu, zkontroluje zda jste připojeni k internetu").Show();
                    }

                }
            });
        }
        private void SetTimer(string minutes)
        {
            cloudService.SetInterval(Times[minutes]);
        }
        private async void ShowPromt(string file)
        {
            bool answer = await page.DisplayAlert("Potvrzení", "Opravdu si přejete načíst zálohu? Veškerá data budou přepsána zálohou.", "Ano", "Ne");
            if (answer==true)
            {
                await MopupService.Instance.PushAsync(new SpinnerPopup());
                bool LoadSucceded=await cloudService.LoadFile(file);
                await MopupService.Instance.PopAsync();
                if (LoadSucceded == true)
                {
                    await Toast.Make("Záloha byla načtena").Show();
                }
                else
                {
                    await Toast.Make("Došlo k chybě při stahování, zkontroluje zda jste připojeni k internetu").Show();
                }
            }
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
