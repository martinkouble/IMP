using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CG.Web.MegaApiClient;

namespace IMP_reseni.ViewModels
{
    public class CloudViewModel
    {
        public ICommand TestCommand { get; set; }
        public CloudViewModel()
        {
            TestCommand = new Command(
            () => 
            { 
            MegaApiClient client = new MegaApiClient();
            client.Login("latysa123@gmail.com", "Kouble29+");
            IEnumerable<INode> nodes = client.GetNodes();

            INode root = nodes.Single(x => x.Type == NodeType.Root);
            INode myFolder = client.CreateFolder("Upload", root);

            string _appDirectory = FileSystem.Current.AppDataDirectory;
                //App.saveholder.Save("IMP.json");
            string _fileName = "IMP.json";

            client.UploadFile(_appDirectory + "/" + _fileName, myFolder);
            client.Logout();
            });
        }
    }
}
