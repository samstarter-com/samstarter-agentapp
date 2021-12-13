using System;
using System.IO;

namespace SWI.SoftStock.Client.Common.Helpers
{
    public static class FileHelper
    {
        private const string Filefolder = "SoftStock";

        public static string GetCommonDataPath()
        {
            string dir = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            dir = System.IO.Path.Combine(dir, Filefolder);
            return dir;
        }

        public static string GetFullFileName(string filename)
        {
            var userDataPath = GetCommonDataPath();
            if (!Directory.Exists(userDataPath))
                Directory.CreateDirectory(userDataPath);
            return System.IO.Path.Combine(userDataPath, filename);
        } 
    }
}
