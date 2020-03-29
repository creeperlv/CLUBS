using CLUBS.Core;
using CLUBS.Tools;
using CLUBS.Tools.Windows;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace CLUBS
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("CLUBS - Creeper Lv's Universal Build System");
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Console.WriteLine("Running on Windows.");
            }
            Repo repo = null;
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
                            Console.WriteLine($"{item.Key}:{item.Value.OriginalString} -> {item.Value.RealPath}");
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
                                Console.WriteLine($"{item.Key}:{item.Value.OriginalString} -> {item.Value.RealPath}");
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
