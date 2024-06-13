using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Configuration.Install;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace AccountsModuleService
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
            this.Installers.Add(GetConnecToBookingAPIService());
            this.Installers.Add(GetXMLFileReaderService());
          //  this.Installers.Add(GetMoveXMLToBookingService());
            this.Installers.Add(GetServiceProcessInstaller());
        }
        private ServiceInstaller GetConnecToBookingAPIService()
        {
            ServiceInstaller serviceInstaller = new ServiceInstaller();
            serviceInstaller.ServiceName = GetConfigurationValue("XMLBookingAPI");
            serviceInstaller.StartType = ServiceStartMode.Automatic;
            return serviceInstaller;
        }
        private ServiceInstaller GetXMLFileReaderService()
        {
            ServiceInstaller serviceInstaller = new ServiceInstaller();
            serviceInstaller.ServiceName = GetConfigurationValue("XMLServiceName");
            serviceInstaller.StartType = ServiceStartMode.Automatic;
            return serviceInstaller;
        }

        //private ServiceInstaller GetMoveXMLToBookingService()
        //{
        //    ServiceInstaller serviceInstaller = new ServiceInstaller();
        //    serviceInstaller.ServiceName = GetConfigurationValue("XMLToBooking");
        //    serviceInstaller.StartType = ServiceStartMode.Automatic;
        //    return serviceInstaller;
        //}

        private ServiceProcessInstaller GetServiceProcessInstaller()
        {
            ServiceProcessInstaller serviceProcessInstaller = new ServiceProcessInstaller();
            serviceProcessInstaller.Account = ServiceAccount.LocalSystem;
            return serviceProcessInstaller;
        }

        private string GetConfigurationValue(string key)
        {
            Assembly service = Assembly.GetAssembly(typeof(ProjectInstaller));
            Configuration config = ConfigurationManager.OpenExeConfiguration(service.Location);
            if (config.AppSettings.Settings[key] != null)
            {
                return config.AppSettings.Settings[key].Value;
            }
            else
            {
                throw new IndexOutOfRangeException("Settings collection does not contain the requested key:" + key);
            }
        }
    }
}
