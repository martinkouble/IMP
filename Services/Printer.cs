using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMP_reseni.Services
{
    public static class Printer
    {
        private static List<byte> Output = new List<byte>();
        public static List<byte> output
        {
            get
            {
                return Output;
            }
        }
        private enum align
        {
            left = 0,
            center = 1,
            right = 2
        }
        public static void PrintLine(string text)
        {
            output.AddRange(text.ToCharArray().Select(x => (byte)x).ToArray());
            output.Add((byte)'\n');
        }
        public static void PrintLine()
        {
            output.Add((byte)'\n');
        }
        public static void Align(string a)
        {
            output.Add(0x1B);
            output.Add(0x61);
            switch (a)
            {
                case "left":
                    output.Add(0);
                    break;
                case "center":
                    output.Add(1);
                    break;
                case "right":
                    output.Add(2);
                    break;
            }
        }
        public static void tab(byte v1, byte v2)
        {
            output.Add(0x1B);
            output.Add(0x44);
            output.Add(v1);
            output.Add(v2);
            output.Add(0);
        }
        public static void Print(string text)
        {
            output.AddRange(text.ToCharArray().Select(x => (byte)x).ToArray());
        }
        public static void tabSkok()
        {
            output.Add(9);
        }
    }
}
