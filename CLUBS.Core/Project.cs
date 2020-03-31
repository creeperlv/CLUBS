using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CLUBS.Core
{

    [Serializable]
    public class Project
    {
        public FileInfo ProjectManifest
        {
            get; private set;
        }
        public string Platform = "Any";
        public string Configuration = "All";
        /**
         * 
         * Determine if will build on os.
         * Possible Values:
         * Any, Windows, Linux, macOS
         * 
         **/

        public Dictionary<string, string> TempImportFiles = new Dictionary<string, string>();
        /**
         * 
         * It will work as: Copy from key to value.
         * And delete copied files after compilation.
         * 
         **/
        public Dictionary<string, string> ImportFiles = new Dictionary<string, string>();
        public string WorkingDirectory = "";
        public List<string> CompileCommands = new List<string>();
        public List<string> DeleteItems = new List<string>();
        public Dictionary<string, string> ExportFiles = new Dictionary<string, string>();
        public static Project Load(FileInfo ProjectManifest)
        {
            Project project = new Project();
            var mani = File.ReadAllLines(ProjectManifest.FullName);
            for (int i = 0; i < mani.Length; i++)
            {
                if (mani[i].StartsWith("WorkingDirectory "))
                {
                    project.WorkingDirectory = mani[i].Substring("WorkingDirectory ".Length);
                }
                else if (mani[i].StartsWith("Platform "))
                {
                    project.Platform = mani[i].Substring("Platform ".Length);
                }
                else if (mani[i].StartsWith("ImportFile "))
                {
                    var f = mani[i].Substring("ImportFile ".Length);
                    project.ImportFiles.Add(f.Substring(0, f.IndexOf('=')), f.Substring(f.IndexOf('=')));
                }
                else if (mani[i].StartsWith("ExportFile "))
                {
                    var f = mani[i].Substring("ExportFile ".Length);
                    project.ExportFiles.Add(f.Substring(0, f.IndexOf('=')), f.Substring(f.IndexOf('=')));
                }
                else if (mani[i].StartsWith("CMD "))
                {
                    var cmd = mani[i].Substring("CMD ".Length);
                    project.CompileCommands.Add(cmd);
                }
                else if (mani[i].StartsWith("Delete "))
                {
                    var DeleteItem = mani[i].Substring("Delete ".Length);
                    project.DeleteItems.Add(DeleteItem);
                }
            }
            return project;
        }
        public void Compile(string Config)
        {
            bool willCompile = false;
            if (Platform == "Any")
            {
                willCompile = true;
            }
            else
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    if (Platform.ToUpper() == "WINDOWS" || Platform.ToUpper() == "WIN")
                    {
                        willCompile = true;
                    }
                }
                else
                if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    if (Platform.ToUpper() == "MACOS" || Platform.ToUpper() == "OSX" || Platform.ToUpper() == "MAC")
                    {
                        willCompile = true;
                    }
                }
                else
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    if (Platform.ToUpper() == "LINUX")
                    {
                        willCompile = true;
                    }
                }
                //else
                //if (RuntimeInformation.IsOSPlatform(OSPlatform.FreeBSD))
                //{
                //    if (Platform.ToUpper() == "BSD")
                //    {
                //        willCompile = true;
                //    }
                //}
                //switch (
                //Environment.OSVersion.Platform)
                //{
                //    default:
                //        break;
                //}
                //These code won't work on .netstandard 2.1, only work on .NET 5.
            }
            if (willCompile == true)
            {
                //Check Configuration.
                if (Configuration == "All")
                {
                    willCompile = true;
                }
                else
                {
                    if (Configuration.ToUpper().IndexOf(Config.ToUpper()) != -1)
                    {
                        willCompile = true;

                    }
                    else
                        willCompile = false;
                }
            }

            if (willCompile)
            {
                {
                    //Process import files first.
                }
            }
        }
    }
}
