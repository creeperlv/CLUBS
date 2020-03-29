using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLUBS.Core
{
    [Serializable]
    public class Repo
    {

        public List<string> Dependencies = new List<string>();
        public List<Project> Projects = new List<Project>();
        public Repo(DirectoryInfo RepoDir)
        {
        }
        public Repo(FileInfo Manifest)
        {
        
        }
        public void Compile()
        {
        
        }
    }
}
