using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Alerts;
using CG.Web.MegaApiClient;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;




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

        //private string _password;
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

        private bool _validated;
        public bool Validated
        {
            private set 
            { 
                _validated = value;
                Preferences.Default.Set("CloudCredentialsValidated", value);
            }
            get { return _validated; }
        }

        public CloudService(SaveHolder saveHolder) 
        {
            Validated= Preferences.Default.Get("CloudServiceIsEnable", false);
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
        public bool SetLogin(string email,string password)
        {
            MegaApiClient client = new MegaApiClient();
            try 
            {
                client.Login(email, password);
                client.Logout();
                Email = email;
                Password = password;
                Validated = true;
                return true;
            }
            catch
            {
                Validated = false;
                return false;
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
                if (!nodes.Any(x => x.Name == "Zalohy"))
                {
                    INode root = nodes.Single(x => x.Type == NodeType.Root);
                    myFolder = await client.CreateFolderAsync("Zalohy", root);
                }
                else
                {
                    myFolder = nodes.Single(n => n.Name == "Zalohy");
                }
                using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    var modificationDate = File.GetLastWriteTime(filePath);
                    await client.UploadAsync(fileStream, "Zaloha_" + DateTime.Now.ToString("_dd_MM_yyyy HH-mm-ss") 
                    + ".json", myFolder, null, modificationDate);
                }
                await client.LogoutAsync();
            }
            catch
            {

                await Toast.Make("Došlo k chybě při uploudnu").Show();
            }  
        }
        public bool UploadFile()
        {
            try
            {
                string filePath = FileSystem.Current.AppDataDirectory + "/" + "IMP.json";
                MegaApiClient client = new MegaApiClient();
                client.Login(Email, Password);
                IEnumerable<INode> nodes = client.GetNodes();
                INode myFolder;
                if (!nodes.Any(x => x.Name == "Zalohy"))
                {
                    INode root = nodes.Single(x => x.Type == NodeType.Root);
                    myFolder = client.CreateFolder("Zalohy", root);
                }
                else
                {
                    myFolder = nodes.Single(n => n.Name == "Zalohy");
                }
                using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    var modificationDate = File.GetLastWriteTime(filePath);
                    client.Upload(fileStream, "Zaloha_" + DateTime.Now.ToString("-dd-MM-yyyy HH:mm:ss") 
                           + ".json", myFolder, modificationDate);
                }
                client.Logout();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public void CreateBackUp()
        {
            File.Copy(FileSystem.Current.AppDataDirectory + "/" + "IMP.json", FileSystem.Current.AppDataDirectory + "/" + "~IMP.json",true);
        }

        public void SignOut()
        {
            timerSet = false;
            IsEnabled = false;
            Validated = false;
            Email = "";
            Password = "";
        }
        /*
        public void UploadFile(string filePath)
        {
            MegaApiClient client = new MegaApiClient();
            client.Login(Email, Password);
            IEnumerable<INode> nodes = client.GetNodes();
            INode myFolder;
            if (!nodes.Any(x=>x.Name== "Zalohy"))
            {
                INode root = nodes.Single(x => x.Type == NodeType.Root);
                myFolder = client.CreateFolder("Zalohy", root);
            }
            else
            {
                myFolder = nodes.Single(n => n.Name == "Zalohy");
            }
            client.UploadFile(filePath, myFolder);
            client.Logout();
        }*/

        public async Task<bool> LoadFile(string fileName)
        {
            try
            {
                MegaApiClient client = new MegaApiClient();
                client.Login(Email, Password);
                IEnumerable<INode> nodes = client.GetNodes();
                INode file = nodes.Single(x => x.Name == fileName);
                string cacheDir = FileSystem.Current.CacheDirectory;
                string path = cacheDir + "/" + fileName;
                File.Delete(path);
                await client.DownloadFileAsync(file, path);
                saveHolder.Load(path);
                saveHolder.Save();
                File.Delete(path);
                client.Logout();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public List<string> GetFilesNames()
        {
            try
            {
                MegaApiClient client = new MegaApiClient();
                client.Login(Email, Password);
                IEnumerable<INode> nodes = client.GetNodes();
                INode parent;
                if (!nodes.Any(x => x.Name == "Zalohy"))
                {
                    INode root = nodes.Single(x => x.Type == NodeType.Root);
                    parent = client.CreateFolder("Zalohy", root);
                }
                else
                {
                    parent = nodes.Single(n => n.Name == "Zalohy");
                }
                nodes = client.GetNodes(parent);
                client.Logout();

                return nodes.Select(x => x.Name).ToList();
            }
            catch (Exception)
            {
                Toast.Make("Došlo k chybě, zkontroluje zda jste připojeni k internetu").Show();
                return null;
            }
        }
    }
}
