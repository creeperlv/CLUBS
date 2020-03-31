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
        public ToolConfig CustomedTools;
        public string DefaultConfiguration = "Debug";
        public List<Project> Projects = new List<Project>();
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
                if (content_Lines[i].StartsWith("Dep:"))
                {
                    Dependencies.Add(content_Lines[i].Substring("Dep:".Length));
                }
                else if (content_Lines[i].StartsWith("Project:"))
                {
                    Projects.Add(Project.Load(new FileInfo(Path.Combine(RepoDirectory.FullName, content_Lines[i].Substring("Project:".Length))), this));
                }
                else if (content_Lines[i].StartsWith("CustomedTools:"))
                {
                    CustomedTools = ToolConfig.ResolveFromFile(new FileInfo(Path.Combine(RepoDirectory.FullName, content_Lines[i].Substring("Project:".Length))).FullName);
                }
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
        public void Compile(string Config)
        {
            if (CheckDependencies())
            {
                foreach (var item in Projects)
                {
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
