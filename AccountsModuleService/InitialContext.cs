using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Data;
using System.Reflection;
using System.Management;
using log4net;

namespace AccountsModuleService
{
    class InitialContext
    {
        #region Internal Static Members
        
        internal static ILog log = LogManager.GetLogger(typeof(InitialContext));
        internal static string ConnectionString = string.Empty;
        internal static ConfigDetails configDetails = new ConfigDetails();
        internal static Hashtable alertHash = new Hashtable();



        #endregion

        #region Internal static Methods
        /// <summary>
        /// Function to initialize configuration settings
        /// </summary>
        /// <returns></returns>
        internal static bool InitializeConfigSettings(string serviceName)
        {
            //DBHandler  dbHandler = null;
            DataSet dsPBXInfo = null;
          
            try
            {

                dsPBXInfo = new DataSet();
                //dbHandler = new DBHandler();
                log.InfoFormat("File Version : {0}", Assembly.GetExecutingAssembly().GetName().Version.ToString());
                log.InfoFormat("File FullName : {0}", Assembly.GetExecutingAssembly().GetName().FullName.ToString());

                log.Info("Host Name                 : " + System.Net.Dns.GetHostName());
                log.Info("Host IP Address           : " + System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList[0].ToString());

                // Create Config file Encryptor object to Read the encrypted contents from config file
                ConfigFileEncryption confEncrypt = new ConfigFileEncryption(ConfigFileType.APP_CONFIG);
                confEncrypt.WriteConfigFile();
                ConnectionString = confEncrypt.ReadFromFile("DBConn");

                if (string.IsNullOrEmpty(InitialContext.ConnectionString))
                {
                    log.Fatal("Database connection string configuration setting is missing. Before Starting the application configure the application");
                    return false;
                }

                if (string.IsNullOrEmpty(serviceName))
                {
                    log.Fatal("Service Name not available, Configuration Error.");
                    return false;
                }

                configDetails.ServiceName = serviceName;
                log.InfoFormat("Service Name : {0}", configDetails.ServiceName);

                return true;
            }
            catch (Exception ex)
            {
                log.Fatal(ex.Message, ex);
                return false;
            }
        }

        /// <summary>
        /// Method to find the time difference between the generated alert based on alertId which is being sent as email, only if the alert is enabled.
        /// </summary>
        /// <returns></returns>
        internal static bool CheckAlertTimeDifference(string alertId)
        {
            try
            {
                if (InitialContext.alertHash.ContainsKey(alertId))
                {

                    string tempDateTime = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss", System.Globalization.CultureInfo.GetCultureInfo("en-us"));
                    log.DebugFormat("Current Date : {0}", tempDateTime);

                    DateTime endTime = DateTime.ParseExact(tempDateTime, "MM/dd/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                    log.DebugFormat("EndTime : {0}", endTime);


                    DateTime startTime = DateTime.ParseExact(InitialContext.alertHash[alertId].ToString(), "MM/dd/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                    log.DebugFormat("StartTime : {0}", startTime);

                    TimeSpan timeSpan = endTime.Subtract(startTime);

                    log.DebugFormat("TotalSeconds : {0}", timeSpan.TotalSeconds);
                    if (timeSpan.TotalSeconds < Convert.ToDouble(InitialContext.configDetails.AlertInvl))
                        return false;

                    InitialContext.alertHash[alertId] = endTime.ToString("MM/dd/yyyy HH:mm:ss", System.Globalization.CultureInfo.GetCultureInfo("en-us"));
                }
                else
                {
                    InitialContext.alertHash.Add(alertId, DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss", System.Globalization.CultureInfo.GetCultureInfo("en-us")));
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return false;
            }
            return true;

        }

        /// <summary>
        /// Check for valid license
        /// </summary>
        internal static bool CheckForLicense()
        {
            string licenseKey = string.Empty;
            string macAddress = string.Empty;
            Encryption encryption = null;
            try
            {
                macAddress = FindMACAddress();
                encryption = new Encryption();
                if (macAddress != string.Empty)
                {
                    licenseKey = encryption.DecryptUsingRijndael(configDetails.LicenseKey);
                    string[] splitKey = licenseKey.Split(',');
                    if (splitKey.Length == 2)
                    {
                        if (macAddress == splitKey[0].ToString())
                        {
                            DateTime licenseValid = DateTime.ParseExact(splitKey[1], "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                            string licenseValidTill = licenseValid.ToString("MM/dd/yyyy", System.Globalization.CultureInfo.GetCultureInfo("en-us"));

                            string tempDateTime = DateTime.Now.ToString("MM/dd/yyyy", System.Globalization.CultureInfo.GetCultureInfo("en-us"));
                            log.InfoFormat("License Valid Till : {0}", licenseValidTill);

                            DateTime dateToday = DateTime.ParseExact(tempDateTime, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                            TimeSpan datediff = licenseValid.Subtract(dateToday);

                            if (datediff.Days < 0)
                            {
                                log.Fatal("************License Expired************");
                                return false;
                            }
                            log.InfoFormat("License is valid for next {0} day(s)", datediff.Days);
                        }
                        else
                        {
                            log.Fatal("************Invalid License************");
                            return false;
                        }
                    }
                    else
                    {
                        log.Fatal("************InValid License Key************");
                        return false;
                    }
                }
                else
                {
                    log.Fatal("************Error in retreving MAC Address************");
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.Fatal("*****************Not a Valid License**********************");
                return false;
            }
            finally
            {
                encryption = null;
            }
        }

        /// <summary>
        /// Method to find the Macaddress of the machine
        /// </summary>
        internal static string FindMACAddress()
        {
            string address = string.Empty;
            try
            {
                //create out management class object using the
                //Win32_NetworkAdapterConfiguration class to get the attributes
                //af the network adapter
                ManagementClass mgmt = new ManagementClass("Win32_NetworkAdapterConfiguration");
                //create our ManagementObjectCollection to get the attributes with
                ManagementObjectCollection objCol = mgmt.GetInstances();

                //loop through all the objects we find
                foreach (ManagementObject obj in objCol)
                {
                    if (address == String.Empty)  // only return MAC Address from first card
                    {
                        //grab the value from the first network adapter we find
                        //you can change the string to an array and get all
                        //network adapters found as well
                        if ((bool)obj["IPEnabled"] == true) address = obj["MacAddress"].ToString();
                    }
                    //dispose of our object
                    obj.Dispose();
                }
                //replace the ":" with an empty space, this could also
                //be removed if you wish
                //address = address.Replace(":", "");
                //return the mac address
                address = address.Replace(":", "-");
                log.DebugFormat("Received MAC Address : {0}", address);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            return address;
        } 

        #endregion Internal static Methods
    }
}
