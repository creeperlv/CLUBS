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
        public string Platform = "any";
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

        public List<string> CompileCommands = new List<string>();
        public Dictionary<string, string> ExportFiles = new Dictionary<string, string>();
        public static Project Load(FileInfo ProjectManifest)
        {
            Project project = new Project();

            return project;
        }
        public void Compile()
        {

        }
    }
}
