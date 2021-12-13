using log4net;
using Newtonsoft.Json;
using SWI.SoftStock.Client.Common;
using SWI.SoftStock.Client.Common.Options;
using SWI.SoftStock.Common.Dto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace SWI.SoftStock.Client.Repositories
{
    public class RestRepository : IRepository
    {
        private readonly RestRepositoryOptions options;
        private readonly ILog log;

        public RestRepository(ILog log, RestRepositoryOptions options)
        {
            this.options = options ?? throw new ArgumentNullException(nameof(options));
            this.log = log;
        }

        #region IRepository Members

        public Guid GetMachineId()
        {
            throw new NotImplementedException();
        }

        public Guid GetCompanyId()
        {
            throw new NotImplementedException();
        }

        public IList<SoftwareDto> GetSoftwareInfos()
        {
            throw new NotImplementedException();
        }

        public Response SetSoftwareInfos(IList<SoftwareDto> softwareInfos)
        {
            throw new NotImplementedException();
        }

        public Response SetModifiedSoftwareInfos(Guid machineId, IList<SoftwareStatusDto> softwareInfos)
        {
            try
            {
                var requestUrl = $"{options.BaseAddress}/softwares?api-version={options.ApiVersion}";
                var http = (HttpWebRequest)WebRequest.Create(requestUrl);
                http.ContentType = "application/json";
                http.Accept = "application/json";
                http.Method = "PUT";
                var request = new SoftwareRequest { Softwares = softwareInfos.ToArray(), MachineUniqueId = machineId };
                var body = JsonConvert.SerializeObject(request);
                http.ContentLength = body.Length;

                var requestWriter = new StreamWriter(http.GetRequestStream(), System.Text.Encoding.ASCII);
                requestWriter.Write(body);
                requestWriter.Close();

                using (var response = http.GetResponse())
                {
                    var responseStream = response.GetResponseStream();
                    if (responseStream != null)
                    {
                        var reader = new StreamReader(responseStream);
                        var receiveContent = reader.ReadToEnd();
                        reader.Close();
                        return JsonConvert.DeserializeObject<Response>(receiveContent);
                    }
                }
            }
            catch (Exception e)
            {
                log.Error(e);
                return new Response() { Code = 1 };
            }

            return new Response() { Code = 1 };
        }

        public Response SetProcess(ProcessDto process)
        {
            try
            {
                var requestUrl = $"{options.BaseAddress}/processes?api-version={options.ApiVersion}";
                var http = (HttpWebRequest)WebRequest.Create(requestUrl);
                http.ContentType = "application/json";
                http.Accept = "application/json";
                http.Method = "PUT";
                var request = new ProcessRequest { Process = process };
                var body = JsonConvert.SerializeObject(request);
                http.ContentLength = body.Length;

                var requestWriter = new StreamWriter(http.GetRequestStream(), System.Text.Encoding.ASCII);
                requestWriter.Write(body);
                requestWriter.Close();

                using (var response = http.GetResponse())
                {
                    var responseStream = response.GetResponseStream();
                    if (responseStream != null)
                    {
                        var reader = new StreamReader(responseStream);
                        var receiveContent = reader.ReadToEnd();
                        reader.Close();
                        return JsonConvert.DeserializeObject<Response>(receiveContent);
                    }
                }
            }
            catch (Exception e)
            {
                log.Error(e);
                return new Response() { Code = 1 };
            }

            return new Response() { Code = 1 };
        }

        public DataResponse GetData(Guid machineId)
        {
            try
            {
                var requestUrl = $"{options.BaseAddress}/machines/{machineId}?api-version={options.ApiVersion}";
                var http = (HttpWebRequest)WebRequest.Create(requestUrl);
                http.Accept = "application/json";
                http.Method = "GET";

                using (var response = http.GetResponse())
                {
                    var responseStream = response.GetResponseStream();
                    if (responseStream != null)
                    {
                        var reader = new StreamReader(responseStream);
                        var receiveContent = reader.ReadToEnd();
                        reader.Close();
                        return JsonConvert.DeserializeObject<DataResponse>(receiveContent);
                    }
                }
            }
            catch (Exception e)
            {
                log.Error(e);
                return new DataResponse() { Code = 1 };
            }

            return new DataResponse() { Code = 1 };
        }

        public void RemoveAll()
        {
            throw new NotImplementedException();
        }

        public Response SetOperationSystem(Guid machineId, OperationSystemDto operationSystem)
        {
            try
            {
                var requestUrl = $"{options.BaseAddress}/operationSystems?api-version={options.ApiVersion}";
                var http = (HttpWebRequest)WebRequest.Create(requestUrl);
                http.ContentType = "application/json";
                http.Accept = "application/json";
                http.Method = "PUT";
                var request = new OperationSystemRequest { MachineUniqueId = machineId, OperationSystem = operationSystem };
                var body = JsonConvert.SerializeObject(request, Formatting.Indented);
                http.ContentLength = body.Length;

                var requestWriter = new StreamWriter(http.GetRequestStream(), System.Text.Encoding.ASCII);
                requestWriter.Write(body);
                requestWriter.Close();

                using (var response = http.GetResponse())
                {
                    var responseStream = response.GetResponseStream();
                    if (responseStream != null)
                    {
                        var reader = new StreamReader(responseStream);
                        var receiveContent = reader.ReadToEnd();
                        reader.Close();
                        return JsonConvert.DeserializeObject<Response>(receiveContent);
                    }
                }
            }
            catch (Exception e)
            {
                log.Error(e);
                return new Response() { Code = 1 };
            }

            return new Response() { Code = 1 };
        }

        public OperationSystemDto GetOperationSystem()
        {
            throw new NotImplementedException();
        }

        public void SetOperationSystemId(Guid operationSystemId)
        {
            throw new NotImplementedException();
        }

        public Guid GetOperationSystemId()
        {
            throw new NotImplementedException();
        }

        public OperationModeDto GetOperationMode()
        {
            throw new NotImplementedException();
        }

        public Response SetOperationMode(Guid machineId, Guid operationSystemId, OperationModeDto operationMode)
        {
            try
            {
                var requestUrl = $"{options.BaseAddress}/operationSystems/operationMode?api-version={options.ApiVersion}";
                var http = (HttpWebRequest)WebRequest.Create(requestUrl);
                http.ContentType = "application/json";
                http.Accept = "application/json";
                http.Method = "PUT";
                var request = new OperationModeRequest
                {
                    MachineUniqueId = machineId,
                    OperationSystemUniqueId = operationSystemId,
                    OperationMode = operationMode
                };
                var body = JsonConvert.SerializeObject(request);
                http.ContentLength = body.Length;

                var requestWriter = new StreamWriter(http.GetRequestStream(), System.Text.Encoding.ASCII);
                requestWriter.Write(body);
                requestWriter.Close();

                using (var response = http.GetResponse())
                {
                    var responseStream = response.GetResponseStream();
                    if (responseStream != null)
                    {
                        var reader = new StreamReader(responseStream);
                        var receiveContent = reader.ReadToEnd();
                        reader.Close();
                        return JsonConvert.DeserializeObject<Response>(receiveContent);
                    }
                }
            }
            catch (Exception e)
            {
                log.Error(e);
                return new Response() { Code = 1 };
            }

            return new Response() { Code = 1 };
        }

        public UserDto GetUser()
        {
            throw new NotImplementedException();
        }

        public Response SetUser(Guid machineId, UserDto user)
        {
            try
            {
                var requestUrl = $"{options.BaseAddress}/users?api-version={options.ApiVersion}";
                var http = (HttpWebRequest)WebRequest.Create(requestUrl);
                http.ContentType = "application/json";
                http.Accept = "application/json";
                http.Method = "PUT";
                var request = new UserRequest { MachineUniqueId = machineId, User = user };
                var body = JsonConvert.SerializeObject(request);
                http.ContentLength = body.Length;

                var requestWriter = new StreamWriter(http.GetRequestStream(), System.Text.Encoding.ASCII);
                requestWriter.Write(body);
                requestWriter.Close();

                using (var response = http.GetResponse())
                {
                    var responseStream = response.GetResponseStream();
                    if (responseStream != null)
                    {
                        var reader = new StreamReader(responseStream);
                        var receiveContent = reader.ReadToEnd();
                        reader.Close();
                        return JsonConvert.DeserializeObject<Response>(receiveContent);
                    }
                }
            }
            catch (Exception e)
            {
                log.Error(e);
                return new Response() { Code = 1 };
            }

            return new Response() { Code = 1 };
        }

        public Response SetActivity(Guid machineId)
        {
            try
            {
                var requestUrl = $"{options.BaseAddress}/machines/activity/{machineId}?api-version={options.ApiVersion}";
                var http = (HttpWebRequest)WebRequest.Create(requestUrl);
                http.Accept = "application/json";
                http.Method = "POST";
                http.ContentLength = 0;
                using (var response = http.GetResponse()) 
                {
                    var responseStream = response.GetResponseStream();
                    if (responseStream != null)
                    {
                        var reader = new StreamReader(responseStream);
                        var receiveContent = reader.ReadToEnd();
                        reader.Close();
                        return JsonConvert.DeserializeObject<Response>(receiveContent);
                    }
                }
            }
            catch (Exception e)
            {
                log.Error(e);
                return new Response() { Code = 1 };
            }

            return new Response() { Code = 1 };
        }

        public Response SetMachineInfo(MachineDto machineInfo)
        {
            try
            {
                var requestUrl = $"{options.BaseAddress}/machines?api-version={options.ApiVersion}";
                var http = (HttpWebRequest)WebRequest.Create(requestUrl);
                http.ContentType = "application/json";
                http.Accept = "application/json";
                http.Method = "PUT";
                var request = new MachineRequest() { Machine = machineInfo };
                var body = JsonConvert.SerializeObject(request);
                http.ContentLength = body.Length;

                var requestWriter = new StreamWriter(http.GetRequestStream(), System.Text.Encoding.ASCII);
                requestWriter.Write(body);
                requestWriter.Close();

                using (var response = http.GetResponse())
                {
                    var responseStream = response.GetResponseStream();
                    if (responseStream != null)
                    {
                        var reader = new StreamReader(responseStream);
                        var receiveContent = reader.ReadToEnd();
                        reader.Close();
                        return JsonConvert.DeserializeObject<Response>(receiveContent);
                    }
                }
            }
            catch (Exception e)
            {
                log.Error(e);
                return new Response() { Code = 1 };
            }
          
            return new Response() { Code = 1 };
        }

        public Response SetMachineId(Guid machineId)
        {
            throw new NotImplementedException();
        }

        public void SetCompanyId(Guid machineId)
        {
            throw new NotImplementedException();
        }

        public MachineDto GetMachineInfo()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}