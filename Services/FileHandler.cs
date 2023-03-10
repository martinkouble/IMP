using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMP_reseni.Services
{
    public class FileHandler
    {
        private string savePath = FileSystem.Current.AppDataDirectory;
        public string imageFolderPath
        {
            get
            {
                if (!Directory.Exists(savePath + "/img"))
                    Directory.CreateDirectory(savePath + "/img");
                return savePath + "/img";
            }
        }
        public string SaveImage(string picturePath)
        {
            string newName = DateTime.Now.GetHashCode().ToString();
            while (File.Exists(imageFolderPath + "/" + newName))
                newName = DateTime.Now.GetHashCode().ToString();
            File.Copy(picturePath, imageFolderPath + "/" + newName);
            return imageFolderPath + "/" + newName;
        }

    }
}
