using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CG.Web.MegaApiClient;
using Microsoft.Extensions.DependencyInjection;



namespace IMP_reseni.Services
{
    public class CloudService
    {
        public string Email { get;private set; }
        public string Password { get;private set; }
        private SaveHolder saveHolder;
        public CloudService(SaveHolder saveHolder) 
        {
             this.saveHolder=saveHolder;
        }
        public void SetLogin(string email,string password)
        {
            Email= email;
            Password= password;
            MegaApiClient client = new MegaApiClient();
            try
            {
                client.Login(Email, Password);
            }
            catch (Exception)
            {
                throw ;
            }
            client.Logout();
        }

        public void UploadFile(string filePath)
        {
            MegaApiClient client = new MegaApiClient();
            client.Login(Email, Password);
            IEnumerable<INode> nodes = client.GetNodes();
            INode myFolder;
            if (nodes.Any(x=>x.Name== "Upload"))
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
