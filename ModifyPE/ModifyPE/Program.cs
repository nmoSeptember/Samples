using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ModifyPE
{
    class Program
    {
        static void Main(string[] args)
        {
            BinaryModify(@"D:\Workspaces\Code\temp\aadtb.dll.mui");
        }

        static void BinaryModify(string fileName)
        {
            string str = "COMP temporary modification by aa.     ";

            using (BinaryWriter bw = new BinaryWriter(File.Open(fileName, FileMode.Open, FileAccess.ReadWrite, FileShare.Write)))
            {
                bw.Seek(5 * 16-2, SeekOrigin.Begin);
                bw.Write(System.Text.Encoding.Default.GetBytes(str));
                
            }
        }
    }
}
