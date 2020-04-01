using CLUBS.Core.Diagnostics;
using CLUBS.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CLUBS.Core
{

    [Serializable]
    public class CLUBSTask
    {
        public FileInfo ProjectManifest
        {
            get; private set;
        }
        public Repo parentRepo;
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
        public static CLUBSTask Load(FileInfo ProjectManifest, Repo Parent)
        {

            CLUBSTask task = new CLUBSTask();
            task.parentRepo = Parent;
            task.ProjectManifest = ProjectManifest;
            var mani = File.ReadAllLines(ProjectManifest.FullName);
            for (int i = 0; i < mani.Length; i++)
            {
                if (mani[i].StartsWith("WorkingDirectory "))
                {
                    task.WorkingDirectory = mani[i].Substring("WorkingDirectory ".Length);
                    if (Directory.Exists(task.WorkingDirectory))
                    {
                        task.WorkingDirectory = new DirectoryInfo(task.WorkingDirectory).FullName;
                    }
                    else
                    {
                        task.WorkingDirectory = new DirectoryInfo(Path.Combine(new DirectoryInfo(".").FullName, task.WorkingDirectory)).FullName;

                    }
                }
                else if (mani[i].StartsWith("Platform "))
                {
                    task.Platform = mani[i].Substring("Platform ".Length);
                }
                else if (mani[i].StartsWith("Configuration "))
                {
                    task.Configuration = mani[i].Substring("Configuration ".Length);
                }
                else if (mani[i].StartsWith("ImportFile "))
                {
                    var f = mani[i].Substring("ImportFile ".Length);
                    task.ImportFiles.Add(f.Substring(0, f.IndexOf('=')), f.Substring(f.IndexOf('=')));
                }
                else if (mani[i].StartsWith("TempImportFile "))
                {
                    var f = mani[i].Substring("TempImportFile ".Length);
                    task.TempImportFiles.Add(f.Substring(0, f.IndexOf('=')), f.Substring(f.IndexOf('=')));
                }
                else if (mani[i].StartsWith("ExportFile "))
                {
                    var f = mani[i].Substring("ExportFile ".Length);
                    task.ExportFiles.Add(f.Substring(0, f.IndexOf('=')), f.Substring(f.IndexOf('=')));
                }
                else if (mani[i].StartsWith("CMD "))
                {
                    var cmd = mani[i].Substring("CMD ".Length);
                    task.CompileCommands.Add(cmd);
                }
                else if (mani[i].StartsWith("Delete "))
                {
                    var DeleteItem = mani[i].Substring("Delete ".Length);
                    task.DeleteItems.Add(DeleteItem);
                }
            }
            return task;
        }
        public void Compile(CompileConfiguration Config)
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
                    if (Configuration.ToUpper().IndexOf(Config.configuration.ToUpper()) != -1)
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
                {
                    //Process Commands.
                    foreach (var item in CompileCommands)
                    {
                        ExecuteCommand(item);
                    }
                }
            }
            else
            {
                Logger.CurrentLogger.Log($"Task Skipped.",LogLevel.Normal);
            }
        }
        public void ExecuteCommand(string cmd)
        {
            Logger.CurrentLogger.Log($"Execute Command:"+cmd, LogLevel.Normal);
            if (cmd.IndexOf(" ") > 0)
            {
                string rCmd = cmd.Substring(0, cmd.IndexOf(" ")).Trim();
                string Args = cmd.Substring(cmd.IndexOf(" ") + 1).Trim();
                if (InternelCommands(rCmd, Args)==false)
                {
                    var item = rCmd;
                    {
                        bool isFind = false;
                        {
                            foreach (var deftool in parentRepo.CustomedTools.ToolPair)
                            {
                                if (isFind == false)
                                    if (deftool.Key.ToUpper() == item.ToUpper())
                                    {
                                        if (deftool.Value.isFile == true)
                                        {
                                            if (deftool.Value.isExists == true)
                                            {
                                                isFind = true;
                                                try
                                                {
                                                    ProcessStartInfo processStartInfo = new ProcessStartInfo(deftool.Value.RealPath);
                                                    processStartInfo.Arguments = Args;
                                                    processStartInfo.WorkingDirectory = WorkingDirectory;
                                                    var p = Process.Start(processStartInfo);

                                                    isFind = true;
                                                }
                                                catch (Exception)
                                                {
                                                    throw new Exception("CLUBS_ERR:002");
                                                }
                                            }
                                            else
                                            {
                                            }
                                        }
                                        else
                                        {
                                            try
                                            {
                                                ProcessStartInfo processStartInfo = new ProcessStartInfo(deftool.Value.OriginalString);
                                                processStartInfo.Arguments = Args;
                                                processStartInfo.WorkingDirectory = WorkingDirectory;
                                                var p = Process.Start(processStartInfo);
                                                isFind = true;
                                            }
                                            catch (Exception)
                                            {
                                                throw new Exception("CLUBS_ERR:002");
                                            }
                                        }
                                    }
                            }

                            if (isFind == false)
                                foreach (var deftool in ToolConfig.DefaultConfig.ToolPair)
                                {
                                    if (isFind == false)
                                        if (deftool.Key.ToUpper() == item.ToUpper())
                                        {
                                            if (deftool.Value.isFile == true)
                                            {
                                                if (deftool.Value.isExists == true)
                                                {
                                                    isFind = true;
                                                    try
                                                    {
                                                        ProcessStartInfo processStartInfo = new ProcessStartInfo(deftool.Value.RealPath);
                                                        processStartInfo.Arguments = Args;
                                                        processStartInfo.WorkingDirectory = WorkingDirectory;
                                                        var p = Process.Start(processStartInfo);
                                                        p.WaitForExit();
                                                        isFind = true;
                                                    }
                                                    catch (Exception)
                                                    {
                                                        throw new Exception("CLUBS_ERR:002");
                                                    }
                                                }
                                                else
                                                {
                                                    throw new Exception("CLUBS_ERR:001");

                                                }
                                            }
                                            else
                                            {
                                                try
                                                {
                                                    ProcessStartInfo processStartInfo = new ProcessStartInfo(deftool.Value.OriginalString);
                                                    processStartInfo.Arguments = Args;
                                                    processStartInfo.WorkingDirectory = WorkingDirectory;
                                                    var p = Process.Start(processStartInfo);
                                                    isFind = true;
                                                }
                                                catch (Exception)
                                                {
                                                    throw new Exception("CLUBS_ERR:002");
                                                }
                                            }
                                        }
                                }

                        }
                    }
                }
            }
            else
            {
            }

        }
        public bool InternelCommands(string cmd, string args = "")
        {
            return false;
        }
    }
}
