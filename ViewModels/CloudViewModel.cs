using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CG.Web.MegaApiClient;
using IMP_reseni.Services;

namespace IMP_reseni.ViewModels
{
    public class CloudViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public ICommand GetCommand { get; set; }
        public ICommand TestCommand { get; set; }

        public List<string> source;
        public CloudViewModel()
        {
            CloudService cloudService= App.cloudService;
            source = new List<string>();
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

            GetCommand = new Command(
            () =>
            {
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

        }
    }
}
