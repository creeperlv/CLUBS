using CLUBS.Core;
using System;
using System.IO;

namespace CLUBS
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("CLUBS - Creeper Lv's Universal Build System");
            Repo repo;
            repo = new Repo(new DirectoryInfo(new DirectoryInfo(".").FullName));
            bool willCompile = false;
            for (int i = 0; i < args.Length; i++)
            {
                if (i == 0)
                {
                    if (Directory.Exists(args[i]))
                    {
                        repo = new Repo(new DirectoryInfo(args[i]));
                    }else
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
                        
                        }
                        break;
                    case "CONFIG":
                        {
                            //clubs config -g xxxxx xxxx
                            var scope=args[i + 1];
                        }
                        break;
                    case "VERSION":
                        {

                        }
                        break;
                    default:
                        {

                        }
                        break;
                }
            }
        }
    }
}
