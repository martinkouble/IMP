using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMP_reseni.Models
{
    public class Excel
    {
        private string content = "";
        public string Content { get { return content; } }

        public static Excel CombineFiles(Excel file1, Excel file2)
        {
            Excel output = new Excel();
            output.HardSetContent(file1.Content + '\n' + file2.Content);
            return output;
        }
        public void WriteLine(string line)
        {
            content += line + '\n';
        }
        public void HardSetContent(string content)
        {
            this.content = content;
        }
    }
}
