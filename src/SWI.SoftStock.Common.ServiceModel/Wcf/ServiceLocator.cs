using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Configuration;
using System.ServiceModel.Description;
using SWI.SoftStock.Common.ServiceModel;
using log4net;

namespace SWI.SoftStock.Common.Wcf
{
    /// <summary>
    /// Responsible for dynamic creation of proxies for wcf services
    /// </summary>
    public class ServiceLocator : IServiceLocator
    {
        private readonly Credentials _credentials;
        private readonly ILog _logger;

        #region defs

        private const string ConfigFileNameFormat = "{0}.config";
        private static readonly Dictionary<string, IChannelFactory> Cache = new Dictionary<string, IChannelFactory>();

        #endregion

        #region ctors

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceLocator"/> class.
        /// </summary>
        /// <param name="credentials">
        /// The credentials.
        /// </param>
        /// <param name="logger">Logger</param>
        public ServiceLocator(Credentials credentials, ILog logger = null)
        {
            _credentials = credentials;
            _logger = logger;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceLocator"/> class.
        /// </summary>
        /// <param name="logger">
        /// The logger.
        /// </param>
        public ServiceLocator(ILog logger = null)
        {
            _logger = logger;
        }

        /// <summary>
        /// Gets CurrentWindowsUserServiceLocator.
        /// </summary>
        /// <value>
        /// The current windows user service locator.
        /// </value>
        public static ServiceLocator GetCurrentWindowsUserServiceLocator(ILog logger = null)
        {
            return new ServiceLocator(new Credentials(WindowsIdentity.GetCurrent().Name), logger);
        }

        #endregion

        #region IServiceLocator implementation

        /// <summary>
        /// Creates proxy for service with provided contract
        /// </summary>
        /// <typeparam name="TService">
        /// Service contract
        /// </typeparam>
        public TService GetServiceProxy<TService>()
        {
            return GetServiceProxy<TService>(null);
        }

        /// <summary>
        /// Creates proxy for service with provided contract
        /// </summary>
        /// <typeparam name="TService">
        /// Service contract
        /// </typeparam>
        public TService GetServiceProxy<TService>(string endpointAddress)
        {
            try
            {
                string contractName = typeof(TService).Name;
                string endpointConfigurationName = GetEndpointConfigurationName(contractName);

                ChannelFactory<TService> factory;

                if (Cache.ContainsKey(endpointConfigurationName))
                {
                    LogDebug(string.Format("Retrieving channel factory for {0} service from cache", endpointConfigurationName));
                    factory = Cache[endpointConfigurationName] as ChannelFactory<TService>;
                    if (_credentials != null && _credentials.Type == AuthenticationType.UserName && (factory.Credentials.UserName.UserName != _credentials.Login
                                                                                                   || factory.Credentials.UserName.Password != _credentials.Password.SecureStringToString()))
                    {
                        factory = new ChannelFactory<TService>(endpointConfigurationName);
                        factory.Credentials.UserName.UserName = _credentials.Login;
                        factory.Credentials.UserName.Password = _credentials.Password.SecureStringToString();
                        Cache[endpointConfigurationName] = factory;
                    }
                }
                else
                {
                    string configPath = ApplicationProbePaths.FindFile(string.Format(ConfigFileNameFormat, contractName));

                    if (string.IsNullOrEmpty(configPath) == false && File.Exists(configPath))
                    {
                        LogDebug(string.Format(
                            "Creating channel factory for {0} service from custom config '{1}'",
                            endpointConfigurationName,
                            configPath));

                        factory = GetConfigurationFactory<TService>(
                            endpointConfigurationName, configPath, endpointAddress);
                    }
                    else
                    {
                        LogDebug(string.Format("Creating channel factory for {0} service from app config.", endpointConfigurationName));
                        factory = new ChannelFactory<TService>(endpointConfigurationName);
                    }

                    if (_credentials != null && _credentials.Type == AuthenticationType.UserName)
                    {
                        factory.Credentials.UserName.UserName = _credentials.Login;
                        factory.Credentials.UserName.Password = _credentials.Password.SecureStringToString();
                    }
                    LogDebug(string.Format("Saving {0} channel factory to cache.", endpointConfigurationName));
                    Cache[endpointConfigurationName] = factory;
                }

                if (factory == null)
                {
                    LogError("Failed to retrieve from cache or create service factory.");
                    throw new InvalidOperationException("Failed to retrieve from cache or create service factory.");
                }

                TService result = string.IsNullOrEmpty(endpointAddress)
                                      ? factory.CreateChannel()
                                      : factory.CreateChannel(new EndpointAddress(endpointAddress));
                
                return result;
            }
            catch (Exception e)
            {
                LogError(e, "Attempt to construct proxy for service with contract {0} failed with exception.", typeof(TService).Name);
                throw;
            }
        }

        private void LogError(Exception ex, string message, params object[] parameters)
        {
            if (_logger != null)
            {
                _logger.Error(string.Format("{0} {1} {2}", ex, message, parameters));
            }
            else
            {
                Debug.WriteLine(string.Format(ex + message, parameters));
            }
        }

        private void LogError(string message, params object[] parameters)
        {
            if (_logger != null)
            {
                _logger.Error(string.Format("{0} {1}", message, parameters));
            }
            else
            {
                Debug.WriteLine(string.Format(message, parameters));
            }
        }

        private void LogDebug(string message, params object[] parameters)
        {
            if (_logger != null)
            {
                _logger.Debug(string.Format("{0} {1}", message, parameters));
            }
            else
            {
                Debug.WriteLine(string.Format(message, parameters));
            }
        }
        
        private string GetEndpointConfigurationName(string name)
        {
            if (_credentials == null || _credentials.Type == AuthenticationType.Windows)
                return name + AuthenticationType.Windows;

            return name + AuthenticationType.UserName;
        }

        /// <summary>
        /// Creates ConfigurationChannelFactory
        /// </summary>
        /// <typeparam name="TService">Type of service</typeparam>
        /// <param name="endpointConfigurationName">Name of configuration in .config</param>
        /// <param name="configPath">Path to config file</param>
        /// <param name="endpointAddress">Endpoint address</param>
        /// <returns>Returns ConfigurationChannelFactory</returns>
        private static ConfigurationChannelFactory<TService> GetConfigurationFactory<TService>(
            string endpointConfigurationName, string configPath, string endpointAddress)
        {
            var config = ConfigurationManager.OpenMappedExeConfiguration(
                new ExeConfigurationFileMap
                {
                    ExeConfigFilename = configPath
                },
                ConfigurationUserLevel.None
                );
            var address = string.IsNullOrEmpty(endpointAddress)
                              ? null
                              : new EndpointAddress(endpointAddress);

            return new ConfigurationChannelFactory<TService>(endpointConfigurationName, config, address);
        }

        /// <summary>
        /// Gets endpoint description for provided service contract <see cref="TService"/>.
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <remarks>see http://stackoverflow.com/questions/211122/wcf-service-custom-configuration </remarks>
        /// <returns></returns>
        public ServiceEndpoint GetServiceEndpoint<TService>()
        {
            var endpointName = typeof(TService).Name;
            var configFilePath = ApplicationProbePaths.FindFile(string.Format(ConfigFileNameFormat, endpointName));

            if (string.IsNullOrEmpty(configFilePath))
                throw new FileNotFoundException("Configuration file not found", configFilePath);

            var config = ConfigurationManager.OpenMappedExeConfiguration(
                new ExeConfigurationFileMap
                {
                    ExeConfigFilename = configFilePath
                },
                ConfigurationUserLevel.None
                );

            var serviceModelSection = ServiceModelSectionGroup.GetSectionGroup(config);
            if (serviceModelSection == null)
                return null;

            var endpointElement = serviceModelSection.Client.Endpoints
                .OfType<ChannelEndpointElement>()
                .FirstOrDefault(ep => ep.Name == endpointName);
            if (endpointElement == null)
                return null;

            var contract = new ContractDescription(endpointElement.Contract);
            var binding = GetBinding(endpointElement.Binding, endpointElement.BindingConfiguration, serviceModelSection);
            var address = new EndpointAddress(
                endpointElement.Address,
                GetIdentity(endpointElement.Identity),
                endpointElement.Headers.Headers
                );

            var result = new ServiceEndpoint(contract, binding, address);
            if (string.IsNullOrEmpty(endpointElement.BehaviorConfiguration) == false)
            {
                AddBehaviors(endpointElement.BehaviorConfiguration, result, serviceModelSection);
            }

            return result;
        }

        #endregion

        #region private stuff

        private static Binding GetBinding(string bindingType, string bindingName,
                                          ServiceModelSectionGroup serviceModelSection)
        {
            if (string.IsNullOrEmpty(bindingName))
                throw new ArgumentNullException("bindingName");
            if (serviceModelSection == null)
                throw new ArgumentNullException("serviceModelSection");

            var bindingElementCollection = serviceModelSection.Bindings[bindingType];
            if (bindingElementCollection.ConfiguredBindings.Count > 0)
            {
                var be = bindingElementCollection.ConfiguredBindings
                    .OfType<IBindingConfigurationElement>()
                    .FirstOrDefault(b => b.Name == bindingName);
                var binding = CreateBinding(be);
                if (be != null)
                {
                    be.ApplyConfiguration(binding);
                }
                return binding;
            }
            return null;
        }

        private static Binding CreateBinding(IBindingConfigurationElement configurationElement)
        {
            if (configurationElement is CustomBindingElement)
                return new CustomBinding();
            if (configurationElement is BasicHttpBindingElement)
                return new BasicHttpBinding();
            if (configurationElement is NetMsmqBindingElement)
                return new NetMsmqBinding();
            if (configurationElement is NetNamedPipeBindingElement)
                return new NetNamedPipeBinding();
            if (configurationElement is NetPeerTcpBindingElement)
                return new NetPeerTcpBinding();
            if (configurationElement is NetTcpBindingElement)
                return new NetTcpBinding();
            if (configurationElement is WSDualHttpBindingElement)
                return new WSDualHttpBinding();
            if (configurationElement is WSHttpBindingElement)
                return new WSHttpBinding();
            if (configurationElement is WSFederationHttpBindingElement)
                return new WSFederationHttpBinding();

            return null;
        }

        private static EndpointIdentity GetIdentity(IdentityElement element)
        {
            EndpointIdentity identity = null;
            var properties = element.ElementInformation.Properties;
            var userPrincipalName = properties["userPrincipalName"];
            if (userPrincipalName != null && userPrincipalName.ValueOrigin != PropertyValueOrigin.Default)
            {
                return EndpointIdentity.CreateUpnIdentity(element.UserPrincipalName.Value);
            }
            var servicePrincipalName = properties["servicePrincipalName"];
            if (servicePrincipalName != null && servicePrincipalName.ValueOrigin != PropertyValueOrigin.Default)
            {
                return EndpointIdentity.CreateSpnIdentity(element.ServicePrincipalName.Value);
            }
            var dns = properties["dns"];
            if (dns != null && dns.ValueOrigin != PropertyValueOrigin.Default)
            {
                return EndpointIdentity.CreateDnsIdentity(element.Dns.Value);
            }
            var rsa = properties["rsa"];
            if (rsa != null && rsa.ValueOrigin != PropertyValueOrigin.Default)
            {
                return EndpointIdentity.CreateRsaIdentity(element.Rsa.Value);
            }
            var certificate = properties["certificate"];
            if (certificate != null && certificate.ValueOrigin != PropertyValueOrigin.Default)
            {
                var supportingCertificates = new X509Certificate2Collection();
                supportingCertificates.Import(Convert.FromBase64String(element.Certificate.EncodedValue));

                if (supportingCertificates.Count == 0)
                {
                    throw new InvalidOperationException("UnableToLoadCertificateIdentity");
                }

                var primaryCertificate = supportingCertificates[0];
                supportingCertificates.RemoveAt(0);
                return EndpointIdentity.CreateX509CertificateIdentity(primaryCertificate, supportingCertificates);
            }
            return identity;
        }

        private static void AddBehaviors(string behaviorConfiguration, ServiceEndpoint serviceEndpoint,
                                         ServiceModelSectionGroup group)
        {
            var behaviorElement = group.Behaviors.EndpointBehaviors[behaviorConfiguration];
            foreach (var behaviorExtension in behaviorElement)
            {
                var extension = behaviorExtension.GetType().InvokeMember("CreateBehavior",
                                                                         BindingFlags.InvokeMethod |
                                                                         BindingFlags.NonPublic | BindingFlags.Instance,
                                                                         null, behaviorExtension, null);
                if (extension != null)
                {
                    serviceEndpoint.Behaviors.Add((IEndpointBehavior)extension);
                }
            }
        }

        #endregion
    }
}
