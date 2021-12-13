using SWI.SoftStock.Client.Common;
using SWI.SoftStock.Client.Common.Helpers;
using SWI.SoftStock.Common.Dto;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Xml.Serialization;

namespace SWI.SoftStock.Client.Repositories
{
    public class FileRepository : IRepository
    {
        private const string MachineIdSection = "machineId";
        private const string CompanyIdSection = "companyId";
        private const string SoftwareInfoFileName = "softwareInfo.xml";
        private const string MachineInfoFileName = "machineInfo.xml";
        private const string OperationSystemFileName = "operationSystem.xml";
        private const string OperationSystemIdSection = "operationSystemId";
        private const string OperationModeFileName = "operationMode.xml";
        private const string UserFileName = "user.xml";

        #region IRepository Members

        public Guid GetMachineId()
        {
            return GetAppSetting(MachineIdSection);
        }

        public Guid GetCompanyId()
        {
            return GetAppSetting(CompanyIdSection);
        }

        public IList<SoftwareDto> GetSoftwareInfos()
        {
            var fullFileName = FileHelper.GetFullFileName(SoftwareInfoFileName);
            var serializer = new DataContractSerializer<IList<SoftwareDto>>();
            return serializer.DeSerializeObject(fullFileName) ?? new List<SoftwareDto>();
        }

        public Response SetModifiedSoftwareInfos(Guid machineId, IList<SoftwareStatusDto> softwareInfos)
        {
            throw new NotImplementedException();
        }

        public Response SetSoftwareInfos(IList<SoftwareDto> softwareInfos)
        {
            try
            {
                var fullFileName = FileHelper.GetFullFileName(SoftwareInfoFileName);
                var serializer = new DataContractSerializer<IList<SoftwareDto>>();
                serializer.SerializeObject(fullFileName, softwareInfos);
            }
            catch (Exception ex)
            {
                return new Response {Code = 1, Message = ex.Message};
            }
            return new Response {Code = 0};
        }

        public Response SetProcess(ProcessDto process)
        {
            throw new NotImplementedException();
        }

        public DataResponse GetData(Guid machineId)
        {
            throw new NotImplementedException();
        }

        public void RemoveAll()
        {
            var directory = new DirectoryInfo(FileHelper.GetCommonDataPath());

            foreach (var file in directory.GetFiles())
            {
                file.Delete();
            }
        }

        public Response SetOperationSystem(Guid machineId, OperationSystemDto operationSystem)
        {
            try
            {
                var fullFileName = FileHelper.GetFullFileName(OperationSystemFileName);
                var serializer = new DataContractSerializer<OperationSystemDto>();
                serializer.SerializeObject(fullFileName, operationSystem);
            }
            catch (Exception e)
            {
                return new Response {Code = 1, Message = e.Message};
            }
            return new Response {Code = 0};
        }

        public OperationSystemDto GetOperationSystem()
        {
            var fullFileName = FileHelper.GetFullFileName(OperationSystemFileName);
            var serializer = new DataContractSerializer<OperationSystemDto>();
            return serializer.DeSerializeObject(fullFileName);
        }

        public void SetOperationSystemId(Guid operationSystemId)
        {
            SetAppSetting(OperationSystemIdSection, operationSystemId.ToString());
        }

        public Guid GetOperationSystemId()
        {
            return GetAppSetting(OperationSystemIdSection);
        }

        public OperationModeDto GetOperationMode()
        {
            var fullFileName = FileHelper.GetFullFileName(OperationModeFileName);
            var serializer = new DataContractSerializer<OperationModeDto>();
            return serializer.DeSerializeObject(fullFileName);
        }

        public Response SetOperationMode(Guid machineId, Guid operationSystemId, OperationModeDto operationMode)
        {
            try
            {
                var fullFileName = FileHelper.GetFullFileName(OperationModeFileName);
                var serializer = new DataContractSerializer<OperationModeDto>();
                serializer.SerializeObject(fullFileName, operationMode);
            }
            catch (Exception e)
            {
                return new Response { Code = 1, Message = e.Message };
            }

            return new Response { Code = 0 };
        }

        public UserDto GetUser()
        {
            var fullFileName = FileHelper.GetFullFileName(UserFileName);
            var serializer = new DataContractSerializer<UserDto>();
            return serializer.DeSerializeObject(fullFileName);
        }

        public Response SetUser(Guid machineId, UserDto user)
        {
            try
            {
                var fullFileName = FileHelper.GetFullFileName(UserFileName);
                var serializer = new DataContractSerializer<UserDto>();
                serializer.SerializeObject(fullFileName, user);
            }
            catch (Exception e)
            {
                return new Response {Code = 1, Message = e.Message};
            }
            return new Response {Code = 0};
        }

        public Response SetActivity(Guid machineId)
        {
            throw new NotImplementedException();
        }

        public Response SetMachineInfo(MachineDto machineInfo)
        {
            try
            {
                var fullFileName = FileHelper.GetFullFileName(MachineInfoFileName);
                var serializer = new DataContractSerializer<MachineDto>();
                serializer.SerializeObject(fullFileName, machineInfo);
            }
            catch (Exception e)
            {
                return new Response { Code = 1, Message = e.Message };
            }

            return new Response { Code = 0, UniqueId = machineInfo.UniqueId };
        }

        public MachineDto GetMachineInfo()
        {
            var fullFileName = FileHelper.GetFullFileName(MachineInfoFileName);
            var serializer = new DataContractSerializer<MachineDto>();
            return serializer.DeSerializeObject(fullFileName);
        }

        public Response SetMachineId(Guid machineId)
        {
            try
            {
                SetAppSetting(MachineIdSection, machineId.ToString());
            }
            catch (Exception e)
            {
                return new Response { Code = 1, Message = e.Message };
            }

            return new Response { Code = 0 };
        }

        public void SetCompanyId(Guid companyId)
        {
            SetAppSetting(CompanyIdSection, companyId.ToString());
        }

        #endregion

        public Response SetProcess(Guid machineId, ProcessDto process)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Open custom shared config file
        /// </summary>
        /// <returns></returns>
        private static Configuration GetConfiguration()
        {
            var fullFileName = FileHelper.GetFullFileName("main.config");

            if (!File.Exists(fullFileName))
            {
                using (var fileStream = File.Create(fullFileName))
                {
                    var xs = new XmlSerializer(typeof (FakeConfiguration));
                    xs.Serialize(fileStream, new FakeConfiguration());
                }
            }
            var configFileMap = new ExeConfigurationFileMap {ExeConfigFilename = fullFileName};

            return ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);
        }

        private static Guid GetAppSetting(string settingKey)
        {
            var result = Guid.Empty;
            var config = GetConfiguration();
            var configSection = config.AppSettings.Settings[settingKey];
            if (configSection == null || !(Guid.TryParse(configSection.Value, out result)))
            {
                return result;
            }
            return result;
        }

        private static void SetAppSetting(string settingKey, string value)
        {
            var config = GetConfiguration();

            var configSection = config.AppSettings.Settings[settingKey];
            if (configSection == null)
            {
                config.AppSettings.Settings.Add(settingKey, value);
            }
            else
            {
                configSection.Value = value;
            }
            config.Save();
            ConfigurationManager.RefreshSection(config.AppSettings.SectionInformation.Name);
        }
    }

    [XmlRoot(ElementName = "configuration", Namespace = "")]
    public class FakeConfiguration
    {
    }
}