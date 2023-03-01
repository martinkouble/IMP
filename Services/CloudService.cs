using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Alerts;
using CG.Web.MegaApiClient;
using Microsoft.Extensions.DependencyInjection;



namespace IMP_reseni.Services
{
    public class CloudService
    {
        private IDispatcherTimer timer;
        private bool timerSet
        {
            set
            {
                Preferences.Default.Set("CloudTimerIsSet", value);
            }
            get
            {
                return Preferences.Default.Get("CloudTimerIsSet", false);
            }
        }
        public int TimerInterval
        {
            private set
            {
                Preferences.Default.Set("CloudTimerInterval", value);
            }
            get
            {
                return Preferences.Default.Get("CloudTimerInterval", 0);
            }
        }
        public string Email 
        {
            private set
            {
                Preferences.Default.Set("Email", value);
            }
            get
            {
                return Preferences.Default.Get("Email", "");
            }
        }

        private string _password;
        public string Password
        {
            private set
            {
                SecureStorage.Default.SetAsync("CloudPassword", value);
            }
            get
            {
                return SecureStorage.Default.GetAsync("CloudPassword").Result;
            }
        }
        private SaveHolder saveHolder;
        public bool IsEnabled {
            set
            {
                Preferences.Default.Set("CloudServiceIsEnable", value);
                if(value==true && timerSet==true &&TimerInterval!=0) 
                {
                    SetInterval(TimerInterval);
                }
                else if (value==false)
                {
                    timerSet = false;
                    timer.Stop();
                }
            }
            get 
            { 
                return Preferences.Default.Get("CloudServiceIsEnable", false); 
            }          
        }
        public CloudService(SaveHolder saveHolder) 
        {
            this.saveHolder=saveHolder;
            timer=Application.Current.Dispatcher.CreateTimer();
            if (TimerInterval!=0 && timerSet == true && IsEnabled == true)
            {       
                SetInterval(TimerInterval);
            }
        }
        public void SetInterval(int minuteInterval)
        {
            timer.Stop();
            if (minuteInterval!=0)
            {           
            TimerInterval = minuteInterval;
            timerSet =true;
            timer.Interval = TimeSpan.FromMinutes(minuteInterval);
            timer.Tick += (s, e) => Tick();
            timer.Start();
            }
            else
            {
                TimerInterval = 0;
                timerSet = false;
            }
        }
        //public void SetInterval(int minuteInterval)
        //{
        //    timer.Stop();
        //    if (minuteInterval!=0)
        //    {           
        //    TimerInterval = minuteInterval;
        //    timerSet =true;
        //    timer.Interval = TimeSpan.FromMinutes(minuteInterval);
        //    timer.Tick += (s, e) => Tick();
        //    timer.Start();
        //    }
        //}
        private void Tick()
        {
            CreateBackUp();
            UploadFileAsync();
        }
        public void SetLogin(string email,string password)
        {
            Email= email;
            Password= password;
            MegaApiClient client = new MegaApiClient();
            try 
            {
                client.Login(Email, Password);
                client.Logout();
                Toast.Make("Údaje ověřeny").Show();
            }
            catch
            {
                Toast.Make("Špatné přihlašovací údaje").Show();
            }
        }
        public async void UploadFileAsync()
        {
            try
            {
                string filePath = FileSystem.Current.AppDataDirectory + "/" + "~IMP.json";
                MegaApiClient client = new MegaApiClient();
                await client.LoginAsync(Email, Password);
                IEnumerable<INode> nodes = await client.GetNodesAsync();
                INode myFolder;
                if (!nodes.Any(x => x.Name == "Upload"))
                {
                    INode root = nodes.Single(x => x.Type == NodeType.Root);
                    myFolder = await client.CreateFolderAsync("Upload", root);
                }
                else
                {
                    myFolder = nodes.Single(n => n.Name == "Upload");
                }
                using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    var modificationDate = File.GetLastWriteTime(filePath);
                    await client.UploadAsync(fileStream, "IMP" + DateTime.Now.ToString("_dd_MM_yyyy HH-mm-ss") + ".json", myFolder, null, modificationDate);
                }
                await client.LogoutAsync();
            }
            catch
            {

                await Toast.Make("Došlo k chybě při uploudnu").Show();
            }  
        }
        public void UploadFile()
        {
            try
            {
                string filePath = FileSystem.Current.AppDataDirectory + "/" + "IMP.json";
                MegaApiClient client = new MegaApiClient();
                client.Login(Email, Password);
                IEnumerable<INode> nodes = client.GetNodes();
                INode myFolder;
                if (!nodes.Any(x => x.Name == "Upload"))
                {
                    INode root = nodes.Single(x => x.Type == NodeType.Root);
                    myFolder = client.CreateFolder("Upload", root);
                }
                else
                {
                    myFolder = nodes.Single(n => n.Name == "Upload");
                }
                using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    var modificationDate = File.GetLastWriteTime(filePath);
                    client.Upload(fileStream, "IMP" + DateTime.Now.ToString("-dd-MM-yyyy HH:mm:ss") + ".json", myFolder, modificationDate);
                }
                client.Logout();
            }
            catch
            {
                Toast.Make("Došlo k chybě při uploudnu").Show();
            }
        }
        public void CreateBackUp()
        {
            File.Copy(FileSystem.Current.AppDataDirectory + "/" + "IMP.json", FileSystem.Current.AppDataDirectory + "/" + "~IMP.json",true);
        }
        public void UploadFile(string filePath)
        {
            MegaApiClient client = new MegaApiClient();
            client.Login(Email, Password);
            IEnumerable<INode> nodes = client.GetNodes();
            INode myFolder;
            if (!nodes.Any(x=>x.Name== "Upload"))
            {
                INode root = nodes.Single(x => x.Type == NodeType.Root);
                myFolder = client.CreateFolder("Upload", root);
            }
            else
            {
                myFolder = nodes.Single(n => n.Name == "Upload");
            }
            client.UploadFile(filePath, myFolder);
            client.Logout();
        }

        public void LoadFile(string fileName)
        {
            MegaApiClient client = new MegaApiClient();
            client.Login(Email, Password);
            IEnumerable<INode> nodes = client.GetNodes();
            INode file = nodes.Single(x => x.Name == "fileName");

            client.Download(file);
            client.Logout();
        }

        public List<string> GetFilesNames()
        {
            MegaApiClient client = new MegaApiClient();
            client.Login(Email, Password);
            IEnumerable<INode> nodes = client.GetNodes();
            INode parent = nodes.Single(n => n.Name == "Upload");
            nodes = client.GetNodes(parent);
            client.Logout();

            return nodes.Select(x=>x.Name).ToList();
        }
    }
}
