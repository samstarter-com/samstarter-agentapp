using System.Collections.Generic;
using System.IO;
using SWI.SoftStock.Client.Common.Helpers;

namespace SWI.SoftStock.Client.Common.Processes
{
    public static class MonitoredProcesses
    {
        private const string Filename="mp.bin";

        /// <summary>
        /// Запись в файл списка процессов, которые необходимо отслеживать
        /// </summary>
        /// <param name="processes">список процессов, которые необходимо отслеживать</param>
        public static void WriteToFileMonitoredProcesses(string[] processes)
        {
            var fullFileName = FileHelper.GetFullFileName(Filename);

            var serializer = new BinarySerializer<string[]>();
            serializer.SerializeObject(fullFileName, processes);
        }
        /// <summary>
        /// Чтение из файла процессов, которые необходимо отслеживать
        /// </summary>
        /// <returns>список процессов, которые необходимо отслеживать</returns>
        public static IEnumerable<string> ReadFromFileMonitoredProcesses()
        {
            var serializer = new BinarySerializer<IEnumerable<string>>();
            var fullFileName = FileHelper.GetFullFileName(Filename);
            return File.Exists(fullFileName) ? serializer.DeSerializeObject(fullFileName) : new string[0];         
        }    

    }
}
