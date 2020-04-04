using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CLUBS.Core.Utilities
{
    public class DirectoryHelper
    {
        public static void CopyRecursively(string ori, string target)
        {
            DirectoryInfo oriD = new DirectoryInfo(ori);
            foreach (var item in oriD.EnumerateFiles())
            {
                File.Copy(item.FullName, Path.Combine(target, item.Name));
            }
            foreach (var item in oriD.EnumerateDirectories())
            {
                CopyRecursively(item.FullName, Path.Combine(target, item.Name));
            }
        }
    }
}
