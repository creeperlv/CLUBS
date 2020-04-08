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
            if (!Directory.Exists(target))
                Directory.CreateDirectory(target);
            foreach (var item in oriD.EnumerateFiles())
            {
                File.Copy(item.FullName, Path.Combine(target, item.Name));
            }
            foreach (var item in oriD.EnumerateDirectories())
            {
                CopyRecursively(item.FullName, Path.Combine(target, item.Name));
            }
        }
        public static void DeleteRecursively(string target)
        {
            
            DirectoryInfo oriD = new DirectoryInfo(target);
            oriD.Delete(true);
            //foreach (var item in oriD.EnumerateFiles())
            //{
            //    item.Delete();
            //}
            //foreach (var item in oriD.EnumerateDirectories())
            //{
            //    DeleteRecursively(item.FullName);
            //}
        }
    }
}
