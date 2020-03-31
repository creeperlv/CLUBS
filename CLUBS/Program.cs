using CLUBS.Core;
using CLUBS.Tools;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace CLUBS
{
    class Program
    {
        public static Version ShellVerison = new Version(1, 0, 0, 0);
        static void Main(string[] args)
        {
            {
                //try
                //{
                    //var p = Process.Start("dotnet", "--list-sdks");
                //    p.Close();
                //}
                //catch (Exception e)
                //{
                //    Console.WriteLine("javac does not exist");
                //}
                //Use this to judge weather a command exist on Windows(Also availiable on Linux).
                //On Linux, use "command -v CMD |wc -l"
            }
            Console.WriteLine("CLUBS - Creeper Lv's Universal Build System");

            Console.OutputEncoding = Encoding.Unicode;
            string ConfigurationOverride = "";
            Repo repo = null;
            bool willCompile = false;
            Operations operation = Operations.Repo;
            if (args.Length == 0)
            {
                operation = Operations.Help;
            }
            for (int i = 0; i < args.Length; i++)
            {
                if (i == 0)
                {
                    if (Directory.Exists(args[i]))
                    {
                        repo = new Repo(new DirectoryInfo(args[i]));
                    }
                    else
                    if (File.Exists(args[i]))
                    {
                        repo = new Repo(new FileInfo(args[i]));
                    }
                }
                switch (args[i].ToUpper())
                {
                    case "COMPILE":
                        {
                            willCompile = true;
                        }
                        break;
                    case "HELP":
                        {

                            operation = Operations.Help;
                        }
                        break;
                    case "CONFIG":
                        {
                            //clubs config -g xxxxx xxxx
                            var scope = args[i + 1];
                        }
                        break;
                    case "VERSION":
                        {
                            operation = Operations.Version;

                        }
                        break;
                    case "TOOLS":
                        {
                            operation = Operations.Tools;

                        }
                        break;
                    default:
                        {
                            if (args[i].StartsWith("-c:"))
                            {
                                ConfigurationOverride = args[i].Substring("-c:".Length);
                            }
                        }
                        break;
                }
            }
            switch (operation)
            {
                case Operations.Repo:
                    {
                        if (repo == null)
                        {
                            
                            repo = new Repo(new DirectoryInfo(new DirectoryInfo(".").FullName));
                            if (willCompile == true)
                            {
                                repo.Compile(ConfigurationOverride==""? repo.DefaultConfiguration:ConfigurationOverride);
                            }
                        }
                    }
                    break;
                case Operations.Tools:
                    {
                        Console.WriteLine("");
                        Console.WriteLine("Recorded Tools:");
                        Console.WriteLine("");
                        Console.WriteLine("---------");
                        foreach (var item in ToolConfig.DefaultConfig.ToolPair)
                        {
                            Console.WriteLine($"{(item.Value.isFile ? "[❓]" : item.Value.isExists ? "[✔]" : "[❌]")}{item.Key}:{item.Value.OriginalString} -> {item.Value.RealPath}");
                        }
                        try
                        {
                            repo = new Repo(new DirectoryInfo(new DirectoryInfo(".").FullName));

                            Console.WriteLine("");
                            Console.WriteLine("Customed Tools:");
                            Console.WriteLine("");
                            Console.WriteLine("---------");
                            foreach (var item in repo.CustomedTools.ToolPair)
                            {
                                Console.WriteLine($"{(item.Value.isFile?"[❓]": item.Value.isExists ? "[✔]" : "[❌]")}{item.Key}:{item.Value.OriginalString} -> {item.Value.RealPath}");
                            }
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("---------");
                            Console.WriteLine("");
                            Console.WriteLine("\tNo Customed Tools!");
                            Console.WriteLine("");
                            Console.WriteLine("---------");
                        }
                    }
                    break;
                case Operations.Version:
                    {
                        Console.WriteLine("Shell:"+ShellVerison);
                        Console.WriteLine("Core:"+CoreLib.LibVersion);
                        Console.WriteLine("ToolsLib:"+ToolsLib.LibVersion);
                        Console.WriteLine("Default Tools Definition:"+ToolsInfo.ToolsInfoVer);
                        Console.WriteLine("Default Tools Definition:"+ToolsInfo.CFG_PLATFORM);
                        Console.WriteLine("Runtime:"+RuntimeInformation.FrameworkDescription);
                    }
                    break;
                case Operations.Help:
                    break;
                default:
                    break;
            }
        }
        enum Operations
        {
            Repo, Tools, Version, Help
        }
    }
}
