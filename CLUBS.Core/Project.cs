using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
                if (mani[i].StartsWith("WorkingDirectory ")){
                    project.WorkingDirectory = mani[i].Substring("WorkingDirectory ".Length);
                }else if (mani[i].StartsWith("ImportFile ")){
                    var f = mani[i].Substring("ImportFile ".Length);
                    project.ImportFiles.Add(f.Substring(0,f.IndexOf('=')),f.Substring(f.IndexOf('=')));
                }else if (mani[i].StartsWith("CMD ")){
                    var f = mani[i].Substring("CMD ".Length);
                    project.CompileCommands.Add(f);
                }
            }
            return project;
        }
        public void Compile()
        {

        }
    }
}
