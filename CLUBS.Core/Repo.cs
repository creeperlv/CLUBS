using CLUBS.Core.Diagnostics;
using CLUBS.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLUBS.Core
{
    [Serializable]
    public class Repo
    {
        public DirectoryInfo RepoDirectory;
        public FileInfo RepoManifest;
        public List<string> Dependencies = new List<string>();
        public ToolConfig CustomedTools=new ToolConfig();
        public string DefaultConfiguration = "Debug";
        public string LogFolder = "/Logs/";
        public List<CLUBSTask> Tasks = new List<CLUBSTask>();
        public Repo(DirectoryInfo RepoDir)
        {
            RepoDirectory = RepoDir;
            RepoManifest = new FileInfo(Path.Combine(RepoDir.FullName, ".clubsrepo"));
            Load();
        }
        public Repo(FileInfo Manifest)
        {
            RepoManifest = Manifest;
            RepoDirectory = Manifest.Directory;
            Load();
        }
        public void Reload()
        {
            Load();
        }
        private void Load()
        {
            if (!RepoManifest.Exists)
                throw new Exception("Not a CLUBS repo!");
            var content_Lines = File.ReadAllLines(RepoManifest.FullName);
            for (int i = 0; i < content_Lines.Length; i++)
            {
                if (content_Lines[i].StartsWith("#"))
                {
                }else
                if (content_Lines[i].StartsWith("Dep:"))
                {
                    Dependencies.Add(content_Lines[i].Substring("Dep:".Length));
                }
                else if (content_Lines[i].StartsWith("Task:"))
                {
                    Tasks.Add(CLUBSTask.Load(new FileInfo(Path.Combine(RepoDirectory.FullName, content_Lines[i].Substring("Task:".Length))), this));
                }
                else if (content_Lines[i].StartsWith("CustomedTools:"))
                {
                    CustomedTools = ToolConfig.ResolveFromFile(new FileInfo(Path.Combine(RepoDirectory.FullName, content_Lines[i].Substring("CustomedTools:".Length))).FullName);
                }
                else if (content_Lines[i].StartsWith("Logs:"))
                {
                    LogFolder = content_Lines[i].Substring("Logs:".Length).Trim();
                }
                else if (content_Lines[i].StartsWith("DefaultConfiguration:"))
                {
                    DefaultConfiguration = content_Lines[i].Substring("DefaultConfiguration:".Length);
                }
            }
        }
        public static void CreateNew()
        {
            {

                string Template = @"[CLUBS Repo Template]
#The character '#' is used for comment.
#Dep:<string>
#   This means your repo depends on certain tool.
#
#Task:<Task-File-Location>
#   This represents tasks your repo will do during the compilation process.
#
Task:default.task
#CustomedTools:<Tool-Definitions-File>
#   This defines customed tools your repo will use.
#
#DefaultConfiguration:<ConfigurationName>
#   This defines the default configuration your repo will use, if it is not defined, the default value is 'Debug'.
";
                File.WriteAllText("./.clubsrepo", Template);
            }
            {

                string Template = @"[CLUBS Task Template]
#The character '#' is used for comment.
#WorkingDirectory <Folder>
#   Defines working directory of this task, it can be absolute and relative.
#Platform <Any|Windows|Linux|macOS>
#ImportFile <File>
#TempImportFile <File>
#   Import, then delete when the task completes.
#ExportFile <File>
#CMD <CMD>
#   Command you want to execute.
#Delete <File>
#Configuration <ConfigurationName>
Configuration All
#   The configuration name that this task will run in.
";
                File.WriteAllText("./default.task", Template);
            }
        }
        public bool CheckDependencies()
        {
            foreach (var item in Dependencies)
            {
                bool isFind = false;
                {
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
                                    }
                                    else
                                        return false;
                                }
                                else
                                {
                                    try
                                    {
                                        ProcessStartInfo processStartInfo = new ProcessStartInfo(deftool.Value.OriginalString);
                                        processStartInfo.CreateNoWindow = true;
                                        processStartInfo.RedirectStandardOutput = true;
                                        processStartInfo.RedirectStandardInput = true;
                                        var p = Process.Start(processStartInfo);
                                        p.Kill();
                                        isFind = true;
                                    }
                                    catch (Exception)
                                    {
                                        return false;
                                    }
                                }
                            }
                    }
                    foreach (var deftool in CustomedTools.ToolPair)
                    {
                        if (isFind == false)
                            if (deftool.Key.ToUpper() == item.ToUpper())
                            {
                                if (deftool.Value.isFile == true)
                                {
                                    if (deftool.Value.isExists == true)
                                    {
                                        isFind = true;
                                    }
                                    else
                                        return false;
                                }
                                else
                                {
                                    try
                                    {
                                        ProcessStartInfo processStartInfo = new ProcessStartInfo(deftool.Value.OriginalString);
                                        processStartInfo.CreateNoWindow = true;
                                        processStartInfo.RedirectStandardOutput = true;
                                        processStartInfo.RedirectStandardInput = true;
                                        var p = Process.Start(processStartInfo);
                                        p.Kill();
                                        isFind = true;
                                    }
                                    catch (Exception)
                                    {
                                        return false;
                                    }
                                }
                            }
                    }
                }
            }
            return true;
        }
        public void Compile(CompileConfiguration Config)
        {
            if (CheckDependencies())
            {
                foreach (var item in Tasks)
                {
                    Logger.CurrentLogger.Log("Task:"+item.ProjectManifest.Name, LogLevel.Normal);
                    item.Compile(Config);
                }
            }
            else
            {
                Logger.CurrentLogger.Log("Fetal: Did Not Match Dependencies!", LogLevel.Error);
            }

        }
    }
}
