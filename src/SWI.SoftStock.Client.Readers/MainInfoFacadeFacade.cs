using Microsoft.Win32;
using SWI.SoftStock.Client.Common;
using SWI.SoftStock.Common.Dto;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Windows.Forms;

namespace SWI.SoftStock.Client.Facades
{
    public class MainInfoFacade : IMainInfoFacade
    {
        #region IMainInfoFacade Members

        /// <summary>
        /// http://forum.sysinternals.com/finding-all-installed-programs-from-the-registry_topic21312.html
        /// </summary>
        /// <returns></returns>
        public IList<SoftwareDto> GetSoftwareInfos()
        {
            var result = new List<SoftwareDto>();
            const string registryName = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
            const string registryName1 = @"Software\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall";
            result = GetSoftwareInfos(Registry.LocalMachine, registryName).ToList();
            IEnumerable<SoftwareDto> result2 =
                GetSoftwareInfos(Registry.LocalMachine, registryName1);
            result = result.Union(result2).ToList();
            IEnumerable<SoftwareDto> result3 =
                GetSoftwareInfos(Registry.CurrentUser, registryName);
            result = result.Union(result3).ToList();
            IEnumerable<SoftwareDto> result4 =
                GetSoftwareInfos(Registry.CurrentUser, registryName1);
            result = result.Union(result4).Distinct().ToList();          
            return result;
        }

        public MachineDto GetMachine()
        {
            var result = new MachineDto
                             {
                                 UniqueId = LocalStorage.GetMachineId(),
                                 CompanyUniqueId = LocalStorage.GetCompanyId(),
                                 Name = SystemInformation.ComputerName,
                                 MonitorCount = SystemInformation.MonitorCount,
                                 MonitorsSameDisplayFormat = SystemInformation.MonitorsSameDisplayFormat,
                                 MouseButtons = SystemInformation.MouseButtons,
                                 ScreenOrientation = SystemInformation.ScreenOrientation.ToString(),
                                 ProcessorCount = Environment.ProcessorCount,
                                 MemoryTotalCapacity = GetMemoryTotalCapacity(),
                                 Processor = GetProcessor(),
                                 NetworkAdapters = GetNetworkAdapters()
                             };

            return result;
        }

        public IStorage LocalStorage { get; set; }

        public UserDto GetUser(Action<Exception> raiseError, Action<string> info)
        {
            ManagementObject wmi;
            var result = new UserDto() { IsEmpty = true };
            try
            {
                var mos = new ManagementObjectSearcher("SELECT UserName FROM Win32_ComputerSystem").Get();
                if (mos.Count == 0)
                {
                    info("ManagementObjectSearcher: SELECT UserName FROM Win32_ComputerSystem return 0 items");
                    return result;
                }
                wmi = mos.Cast<ManagementObject>().FirstOrDefault();
                if (wmi == null)
                {
                    info("Cast of: SELECT UserName FROM Win32_ComputerSystem return 0 items");
                    return result;
                }
            }
            catch (Exception e)
            {
                raiseError(e);
                throw;
            }
            string username = GetManagementObjectByName<string>(wmi, "UserName", raiseError);
            var user = username.Split('\\');
            result = new UserDto();
            result.UserDomainName = user.Count() == 2 ? user[0] : string.Empty;
            result.UserName = user.Count() == 2 ? user[1] : username;
            result.IsEmpty = false;
            wmi.Dispose();
            return result;
        }

        public OperationSystemDto GetOperationSystem(Action<Exception> raiseError)
        {
            ManagementObject wmi = null;
            try
            {
               wmi =
               new ManagementObjectSearcher("select * from Win32_OperatingSystem")
                   .Get()
                   .Cast<ManagementObject>()
                   .First();
            }
            catch (Exception e)
            {
                raiseError(e);
                throw;
            }

            var result = new OperationSystemDto();
            result.UniqueId = LocalStorage.GetOperationSystemId();
            result.Name = GetManagementObjectByName<string>(wmi, "Caption", raiseError).Trim();
            result.Version = GetManagementObjectByName<string>(wmi, "Version", raiseError);
            result.MaxNumberOfProcesses = GetManagementObjectByName<UInt32>(wmi, "MaxNumberOfProcesses", raiseError);
            result.MaxProcessMemorySize = GetManagementObjectByName<ulong>(wmi, "MaxProcessMemorySize", raiseError);
            result.Architecture = GetManagementObjectByName<string>(wmi, "OSArchitecture", raiseError) ?? String.Empty;
            result.BuildNumber = GetManagementObjectByName<string>(wmi, "BuildNumber", raiseError);
            wmi.Dispose();

            return result;
        }

        private T GetManagementObjectByName<T>(ManagementObject wmi, string name, Action<Exception> raiseError)
        {
            var result = default(T);
            try
            {
                result = (T) wmi[name];
            }
            catch (Exception e)
            {
                raiseError(e);
            }
            return result;
        }

        public OperationModeDto GetOperationMode()
        {
            var result = new OperationModeDto();
            ManagementObject wmi =
                new ManagementObjectSearcher("select * from Win32_OperatingSystem")
                    .Get()
                    .Cast<ManagementObject>()
                    .First();
            result.SerialNumber = (string) wmi["SerialNumber"];
            result.BootMode = SystemInformation.BootMode.ToString();
            result.Secure = SystemInformation.Secure;
            result.SystemDirectory = Environment.SystemDirectory;
            result.LogicalDrives = String.Join(" ", Environment.GetLogicalDrives());

            foreach (DictionaryEntry envVar in Environment.GetEnvironmentVariables())
            {
                result.EnvironmentVariables += string.Format("{0}:{1}; ", envVar.Key, envVar.Value);
            }
            wmi.Dispose();
            return result;
        }

        #endregion

        private IEnumerable<SoftwareDto> GetSoftwareInfos(RegistryKey registryKey, string name)
        {
            var result = new List<SoftwareDto>();
            using (RegistryKey key = registryKey.OpenSubKey(name))
            {
                if (key != null)
                    foreach (string subkeyName in key.GetSubKeyNames())
                    {
                        using (RegistryKey subkey = key.OpenSubKey(subkeyName))
                        {
                            if (subkey != null)
                            {
                                var softwareInfo = new SoftwareDto();
                                if (subkey.GetValue("DisplayName") != null)
                                {
                                    softwareInfo.Name = subkey.GetValue("DisplayName").ToString();
                                }
                                if (subkey.GetValue("DisplayVersion") != null)
                                {
                                    softwareInfo.Version = subkey.GetValue("DisplayVersion").ToString();
                                }
                                if (subkey.GetValue("InstallDate") != null)
                                {
                                    softwareInfo.InstallDate = subkey.GetValue("InstallDate").ToString();
                                }
                                if (subkey.GetValue("SystemComponent") != null)
                                {
                                    softwareInfo.SystemComponent = subkey.GetValue("SystemComponent").ToString();
                                }
                                if (subkey.GetValue("WindowsInstaller") != null)
                                {
                                    softwareInfo.WindowsInstaller = subkey.GetValue("WindowsInstaller").ToString();
                                }
                                if (subkey.GetValue("ReleaseType") != null)
                                {
                                    softwareInfo.ReleaseType = subkey.GetValue("ReleaseType").ToString();
                                }
                                if (subkey.GetValue("Publisher") != null)
                                {
                                    softwareInfo.Publisher = new PublisherDto
                                                                 {Name = subkey.GetValue("Publisher").ToString()};
                                }
                                if (!IsNullOrEmpty(softwareInfo))
                                {
                                    result.Add(softwareInfo);
                                }
                            }
                        }
                    }
            }
            return result;
        }

        private static bool IsNullOrEmpty(SoftwareDto software)
        {
            if (software == null)
            {
                return true;
            }

            return (String.IsNullOrEmpty(software.Name))
                   && (String.IsNullOrEmpty(software.Version));
        }

        /// <summary>
        /// MemoryTotalCapacity in bytes
        /// </summary>       
        private static double GetMemoryTotalCapacity()
        {
            var objectQuery = new ObjectQuery("select * from Win32_PhysicalMemory");
            var searcher = new
                ManagementObjectSearcher(objectQuery);
            ManagementObjectCollection vals = searcher.Get();

            return vals.Cast<ManagementObject>().Sum(val => Convert.ToDouble(val.GetPropertyValue("Capacity")));
        }

        private static ProcessorDto GetProcessor()
        {
            var result = new ProcessorDto
                             {Manufacturer = new ManufacturerDto(), Is64BitProcess = Environment.Is64BitProcess};
            //create an instance of the Managemnet class with the
            //Win32_Processor class
            var mgmt = new ManagementClass("Win32_Processor");
            //create a ManagementObjectCollection to loop through
            ManagementObjectCollection objCol = mgmt.GetInstances();
            //start our loop for all processors found
            foreach (ManagementObject obj in objCol)
            {
                // only return cpuInfo from first CPU             
                result.ProcessorId = obj.Properties["ProcessorId"].Value.ToString();
                result.DeviceID = obj.Properties["DeviceID"].Value.ToString();
                result.SocketDesignation = obj.Properties["SocketDesignation"].Value.ToString();
                result.Manufacturer.Name = obj.Properties["Manufacturer"].Value.ToString();
                //dispose of our object
                obj.Dispose();
            }
            //return to calling method
            return result;
        }

        private static IList<NetworkAdapterDto> GetNetworkAdapters()
        {
            var oMClass = new ManagementClass("Win32_NetworkAdapterConfiguration");

            ManagementObjectCollection colMObj = oMClass.GetInstances();

            return (from ManagementObject objMo in colMObj
                    let macAdress = objMo["MacAddress"]
                    where macAdress != null
                    select new NetworkAdapterDto
                               {
                                   Caption = objMo["Caption"].ToString(),
                                   MacAdress = macAdress.ToString()
                               }).ToList();
        }
    }
}