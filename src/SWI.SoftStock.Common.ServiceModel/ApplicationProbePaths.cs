using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SWI.SoftStock.Common
{
    /// <summary>
    /// Stores probe paths for searching asssemblies, configs etc.
    /// </summary>
    public static class ApplicationProbePaths
    {
        /// <summary>
        /// Gets list of paths, where application specific files and assemblies could be found
        /// </summary>
        public static IList<string> ProbePaths { get; private set; }

        static ApplicationProbePaths()
        {
            ProbePaths = new List<string> { AppDomain.CurrentDomain.BaseDirectory };
        }

        /// <summary>
        /// Looking for specified file in all <see cref="ProbePaths"/>
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string FindFile(string fileName)
        {
            return ProbePaths
                .Select(searchFolder => FindConfigFile(searchFolder, fileName))
                .FirstOrDefault(foundConfigFile => foundConfigFile != null);
        }

        private static string FindConfigFile(string inFolder, string fileName)
        {
            if (Directory.Exists(inFolder) == false)
                return null;

            var filePath = Path.Combine(inFolder, fileName);
            if (File.Exists(filePath))
                return filePath;

            return Directory.GetDirectories(inFolder)
                .Select(directory => FindConfigFile(directory, fileName))
                .FirstOrDefault(foundConfigFile => foundConfigFile != null);
        }
    }
}
