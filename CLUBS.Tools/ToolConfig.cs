using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLUBS.Tools
{
    [Serializable]
    public class ToolConfig
    {
        public static ToolConfig DefaultConfig { get; private set; } = GetDefaultConfig();
        static ToolConfig GetDefaultConfig()
        {
            var f = new FileInfo(
            Process.GetCurrentProcess().MainModule.FileName);
            return ResolveFromFile(Path.Combine(f.Directory.FullName, "Config", "Tools.cfg"));
        }
        public Dictionary<string, Tool> ToolPair = new Dictionary<string, Tool>();
        public static ToolConfig ResolveFromFile(string file)
        {
            ToolConfig toolConfig = new ToolConfig();
            var lines = File.ReadAllLines(file);
            foreach (var item in lines)
            {
                if (item.StartsWith("#"))
                {

                }
                else
                {
                    toolConfig.ToolPair.Add(item.Substring(0, item.IndexOf('=')), new Tool(item.Substring(item.IndexOf('=') + 1)));
                }

            }
            return toolConfig;
        }
    }
    [Serializable]
    public class Tool
    {
        public string OriginalString;
        public string RealPath;
        public bool isExists = true;
        public bool isFile = false;
        public Tool(string Original)
        {
            OriginalString = Original;
            RealPath = Original;
            if (Original.StartsWith("(BIN)"))
            {
                isFile = true;
                RealPath = RealPath.Substring("(BIN)".Length);
                RealPath = RealPath.Replace("[ProgFiles]", Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles));
                while (RealPath.IndexOf("[NewestFolder]") > 0)
                {
                    var Pre = RealPath.Substring(0, RealPath.IndexOf("[NewestFolder]"));
                    var D = new DirectoryInfo(Pre);
                    if (!D.Exists)
                    {
                        isExists = false;
                        return;
                    }
                    var Fs = D.EnumerateDirectories();
                    DirectoryInfo result = null;
                    foreach (var item in Fs)
                    {
                        if (result == null)
                        {
                            result = item;
                        }
                        else
                        {
                            if (item.LastWriteTime > result.LastWriteTime)
                            {
                                result = item;
                            }
                        }
                    }
                    RealPath = Pre + result.Name + RealPath.Substring(RealPath.IndexOf("[NewestFolder]") + "[NewestFolder]".Length);
                }
                if (!File.Exists(RealPath))
                {
                    isExists = false;
                }
            }
        }
    }

}
