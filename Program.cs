using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Configuration;

namespace NugetCleanupTest
{
    class Program
    {
        static void Main(string[] args)
        {
            
            string strRootpath = @"E:\1ES.SCOMCPY\SystemCenter\Migration\SCOM\src";
            string tfsRootpath = @"E:\OneESBaseline\SCOM\private\product";
            string oneesOutRootpath = @"E:\1ES.SCOMCPY\SystemCenter\Migration\SCOM\out";
            
            //string directoryCmdPath = String.Empty;

            
            List<string> allRootfilesItemList1 = new List<string>();
            List<string> allRootfilesItemList2 = new List<string>();

            
            
            if (args.Length != 0)
            {
                strRootpath = args[0];
            }
            

            allRootfilesItemList1 = PlacefileMigration(tfsRootpath);
            //System.Diagnostics.Process.Start("CMD.exe", "/C ipconfig");
            //string command = @"robocopy E:\1ES.SCOMCPY\SystemCenter\Migration\SCOM\src\product\uX\Core\Agent.CSMScripting\objd\amd64 $OutDir\uX\Core Agent.CSMScripting.dll";
            //string command = @"robocopy $(INETROOT)\out\debug-amd64\Agent.CSMScripting $(INETROOT)\out\placefiled\uX\Core Agent.CSMScripting.dll";
            //ExecuteCommandSync(command);

            //System.Diagnostics.Process.Start(@"C:\Users\v-gikala\source\repos\NugetCleanupTest\NugetCleanupTest\test.bat");
            GetBinariesFromOutPath(oneesOutRootpath, allRootfilesItemList1);

            Console.ReadLine();
        }

        private static void GetBinariesFromOutPath(string oneesOutRootpath, List<string> allRootfilesItemList1)
        {
            //string line1 = allRootfilesItemList1[i];
            string str1;


            // Add the below line for INETROOT or any other detinations
            // Replacing "target\%BUILDTYPE%\%CPUTYPE%\ " with "$OutDir"
            //string[] replacedLines = lines.Select(x => x.Replace(@"target\%BUILDTYPE%\%CPUTYPE%\", @"$(INETROOT)\out\placefiled\")).ToArray();


            if (Directory.Exists(oneesOutRootpath))
            {
                List<string> locallist = new List<string>();
                //string[] allfileslist1 = Directory.GetFiles(oneesOutRootpath, "*dirs.*", SearchOption.AllDirectories);

                //string[] lines = System.IO.File.ReadAllLines(line1);
                //string[] lines =

                // string str1, str2;
                // Display the file contents by using a foreach loop.
                foreach (var item in allRootfilesItemList1)
                {
                    if (item != "")
                    {
                        StringBuilder stringBuilder = new StringBuilder(item.Length);
                        int j = 0;
                        foreach (char c in item)
                        {
                            if (c != ' ' || j == 0 || item[j - 1] != ' ')
                                stringBuilder.Append(c);
                            j++;
                        }
                        str1 = stringBuilder.ToString();
                        if (!str1.Contains(";"))
                        {
                            str1 = str1.Replace(":", ",");
                            locallist.Add(str1);
                        }
                    }
                }


            }
        }

        private static List<string> PlacefileMigration(string tfsRootpath)
        {
            List<string> allRootfilesItemList2 = new List<string>();
            allRootfilesItemList2 = MasterPlaceFileList(tfsRootpath);
            return allRootfilesItemList2;
        }
        private static List<string> MasterPlaceFileList(string tfsRootpath)
        {
            List<string> allRootfilesItemList1 = new List<string>();

            if (Directory.Exists(tfsRootpath))
            {
                
                string[] allfileslist = Directory.GetFiles(tfsRootpath, "*placefile*", SearchOption.AllDirectories);
                string str1 = string.Empty;
                
                //foreach (var item in allfileslist)
                //{
                //    i = i + 1;
                //    str1 = i.ToString() + "," + item;
                //    System.Console.WriteLine(str1);
                //    allRootfilesItemList.Add(str1);
                //}
                foreach (string line1 in allfileslist)
                {
                    string[] lines = System.IO.File.ReadAllLines(line1);

                    // Add the below line for INETROOT or any other detinations
                    // Replacing "target\%BUILDTYPE%\%CPUTYPE%\ " with "$OutDir"
                    //string[] replacedLines = lines.Select(x => x.Replace(@"target\%BUILDTYPE%\%CPUTYPE%\", @"$(INETROOT)\out\placefiled\")).ToArray();
                    string temp = @"E:\1ES.SCOMCPY\SystemCenter\Migration\SCOM\out\placefiled\";
                    string[] replacedLines = lines.Select(x => x.Replace(@"target\%BUILDTYPE%\%CPUTYPE%\", temp)).ToArray();

                    // string str1, str2;
                    // Display the file contents by using a foreach loop.
                    foreach (var item in replacedLines)
                    {
                        if (item != "")
                        {
                            StringBuilder stringBuilder = new StringBuilder(item.Length);
                            int j = 0;
                            foreach (char c in item)
                            {
                                if (c != ' ' || j == 0 || item[j - 1] != ' ')
                                    stringBuilder.Append(c);
                                j++;
                            }
                            str1 = stringBuilder.ToString();
                            if (!str1.Contains(";"))
                            {
                                str1 = str1.Replace(":", ",");
                                allRootfilesItemList1.Add(str1);
                            }
                        }
                    }
                }
                allfileslist = null;
                File.WriteAllLines(@"C:\Users\v-gikala\source\repos\NugetCleanupTest\NugetCleanupTest\masterPlaceFileList.txt", allRootfilesItemList1.ToList());
            }
            else
            {
                Assembly ass = Assembly.GetExecutingAssembly();
                string path = System.IO.Path.GetDirectoryName(ass.Location);
                //System.IO.File.Copy(@"\\sccxe-scratch\scratch\v-satvin\IES\InternalBinaryList.txt", @path + @"\InternalBinaryList.txt", true);
            }
            //Console.ReadLine();
            return allRootfilesItemList1;
        }

        private static void ExecuteCommandSync(object command)
        {
            try
            {
                // create the ProcessStartInfo using "cmd" as the program to be run,
                // and "/c " as the parameters.
                // Incidentally, /c tells cmd that we want it to execute the command that follows,
                // and then exit.
                System.Diagnostics.ProcessStartInfo procStartInfo =
                    new System.Diagnostics.ProcessStartInfo("cmd", "/c " + command);

                // The following commands are needed to redirect the standard output.
                // This means that it will be redirected to the Process.StandardOutput StreamReader.
                procStartInfo.RedirectStandardOutput = true;
                procStartInfo.UseShellExecute = false;
                // Do not create the black window.
                procStartInfo.CreateNoWindow = true;
                // Now we create a process, assign its ProcessStartInfo and start it
                System.Diagnostics.Process proc = new System.Diagnostics.Process();
                proc.StartInfo = procStartInfo;
                proc.Start();
                // Get the output into a string
                string result = proc.StandardOutput.ReadToEnd();
                // Display the command output.
                Console.WriteLine(result);
            }
            catch (Exception objException)
            {
                // Log the exception
            }
        }
      
    }
}
