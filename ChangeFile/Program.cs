using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChangeFile
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Keyin the path: ");
            string path1 = Console.ReadLine();

            // get all the files under this path
            DirectoryInfo root = new DirectoryInfo(path1);
            FileInfo[] files = root.GetFiles();
            List<FileInfo> lstFiles = files.ToList();

            // recurse files
            for(int i=0; i<lstFiles.Count; i++)
            {
                string fileName = lstFiles[i].FullName;

                //Delete the file
                //if (fileName == "TypeTheFileYouWantDelteHere.txt")
                //{
                //    File.Delete(lstFiles[i].FullName);
                //    Console.WriteLine("TypeTheFileYouWantDelteHere.txt has been deleted");
                //} 


                bool isHave = fileName.Contains("TypeIndexHere");
                if (isHave)
                {
                    // change the name of file
                    string srcFilename=lstFiles[i].FullName;
                    string destFilename = lstFiles[i].Directory.FullName + "/NameAfterChange" + lstFiles[i].Extension;
                    //File.Move(srcFilename, destFilename);
                    //Console.WriteLine("");
                    //Console.WriteLine("FILE HAS CHANGED!");
                    //Console.WriteLine("");
                }

                Console.WriteLine(fileName);
                Console.WriteLine(isHave);

                //Console.WriteLine(lstFiles[i].FullName);
                //Console.WriteLine(lstFiles[i].Name);
            }

            Console.ReadKey();
        }
    }
}
