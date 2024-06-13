using System;
using System.Xml;
using System.IO;
using System.Configuration;


namespace AccountsModuleService
{
    /// <summary>
    /// Summary description for ConfigFileEncryption.
    /// </summary>
    public class ConfigFileEncryption
    {
        #region Private Declarations
        private readonly string m_HashConst = "$$##COGNICX##$$";	// Hash Code Constant for the Encryption Algorithm
        private ConfigFileType m_FileType;
        private XmlDocument m_newConfigFile;							// Stores the Xml Document
        private XmlNodeList m_nodeList;									// Stores the NodeList from the XML File
        private XmlNode m_node;											// Stores Node from the Nodelist
        #endregion Private Declarations

        #region Constructors
        /// <summary>
        /// Creates a New Instance of the ConfigFileEncryptionClass
        /// </summary>
        public ConfigFileEncryption()
        {
            m_newConfigFile = new XmlDocument();
        }
        /// <summary>
        /// Creates a New instance of the ConfigFileEncryption Class 
        /// </summary>
        /// <param name="configFileType"></param>
        public ConfigFileEncryption(ConfigFileType configFileType)
        {
            m_newConfigFile = new XmlDocument();
            m_FileType = configFileType;
        }


        #endregion Constructors

        #region Properties
        /// <summary>
        /// Gets or Sets the Type of Configuration File Used for Encryption
        /// </summary>
        public ConfigFileType FileType
        {
            get
            {
                return this.m_FileType;
            }
            set
            {
                this.m_FileType = value;
            }
        }
        #endregion Properties

        #region Public Methods
        /// <summary>
        /// This method is used to get the connection string in plain text which in web.config file and Encrypt the string and save it.
        /// </summary>
        /// <example>if the connection string in web.config file is like "Encrypt(Password=sti;Persist Security Info=True;User ID=sti;Data Source=RAP)" 
        /// then it encrypt the given connection string to "Decrypt(jfBC6AqFLugOOo1MPQQUt3wXN5o7eX7VUD6ZUolv7kWMds08gtqWXFPxNOMC6RvHQTXPqKtYx0lqLog7OqGlJatZ6LxEHOtyTnxr3bkxVR8=)"</example>
        public void WriteConfigFile()
        {
            CheckFiletype();

            bool fileChanged = false;				// Initially Setting the FileChanged Property to False
            string configFilePath = string.Empty;		// Stores the Config File Path - Setting Initially to Empty String
            string configFile = string.Empty;		// Stores the Config File Name - Setting Initially to Empty String 

            //Initialise the XmlDocument 
            m_newConfigFile = new XmlDocument();

            // If the Specified File type is App.Config 
            if (m_FileType == ConfigFileType.APP_CONFIG)
            {
                configFilePath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
                configFile = configFilePath + "App.config";


                if (File.Exists(configFile) == false)
                {
                    configFile = AppDomain.CurrentDomain.GetData("APP_CONFIG_FILE").ToString();

                }

            }
            else if (m_FileType == ConfigFileType.WEB_CONFIG)
            {
                configFilePath = AppDomain.CurrentDomain.GetData("APP_CONFIG_FILE").ToString();
                configFile = configFilePath;
            }

            m_newConfigFile.Load(configFile);
            m_nodeList = m_newConfigFile.GetElementsByTagName("add");

            for (int nodeCount = 0; nodeCount < m_nodeList.Count; nodeCount++)
            {
                m_node = m_nodeList.Item(nodeCount);
                string keyValue = string.Empty;
                if (m_node.Attributes.Count > 0)
                {
                    if (m_node.Attributes.Item(1) != null)
                    {
                        if (m_node.Attributes.Item(1).Value.StartsWith("Encrypt("))
                        {
                            keyValue = m_node.Attributes.Item(1).Value.ToString();
                            keyValue = keyValue.Substring(8, m_node.Attributes.Item(1).Value.Length - 9);
                            //keyValue		=	m_node.Attributes.Item(1).Value.Substring(8,m_node.Attributes.Item(1).Value.Length - 9);  
                            Encryption EncryptClass = new Encryption();
                            string encoded = EncryptClass.EncryptUsingRijndael(keyValue, m_HashConst);
                            m_node.Attributes.Item(1).Value = "Decrypt(" + encoded + ")";
                            EncryptClass = null;
                            fileChanged = true;
                        }
                    }
                }
            }

            if (fileChanged == true)
            {
                if (m_FileType == ConfigFileType.APP_CONFIG)
                {
                    if (File.Exists(configFile) == true)
                    {
                        m_newConfigFile.Save(configFile);
                    }
                    m_newConfigFile.Save(AppDomain.CurrentDomain.GetData("APP_CONFIG_FILE").ToString());
                }
                else
                {
                    m_newConfigFile.Save(AppDomain.CurrentDomain.GetData("APP_CONFIG_FILE").ToString());
                }

            }

        }
        /// <summary>
        /// Reads  the value of the connection string which are in decrypted text format and starts with Decrypt from web.config file
        /// </summary>
        /// <example>"Decrypt(jfBC6AqFLugOOo1MPQQUt3wXN5o7eX7VUD6ZUolv7kWMds08gtqWXFPxNOMC6RvHQTXPqKtYx0lqLog7OqGlJatZ6LxEHOtyTnxr3bkxVR8=)"</example>
        /// <param name="keyName">Name of the key in web.config file</param>
        /// <returns>Returns the connection string in form of string</returns>
        public string ReadFromFile(string keyName)
        {
            string configFilePath = string.Empty;
            string configFile = string.Empty;
            string decryptedValue = string.Empty;

            //Initialising the NewConfigFile 
            m_newConfigFile = new XmlDocument();

            if (m_FileType == ConfigFileType.APP_CONFIG)
            {
                configFilePath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
                configFile = configFilePath + "App.config";

                if (File.Exists(configFile) == false)
                {
                    configFile = AppDomain.CurrentDomain.GetData("APP_CONFIG_FILE").ToString();
                }

            }
            else if (m_FileType == ConfigFileType.WEB_CONFIG)
            {
                configFilePath = AppDomain.CurrentDomain.GetData("APP_CONFIG_FILE").ToString();
                configFile = configFilePath;
            }

            m_newConfigFile.Load(configFile);
            m_nodeList = m_newConfigFile.GetElementsByTagName("add");

            for (int nodeCount = 0; nodeCount < m_nodeList.Count; nodeCount++)
            {
                m_node = m_nodeList.Item(nodeCount);
                string keyValue = string.Empty;
                if (m_node.Attributes.Count > 0)
                {
                    if (m_node.Attributes.Item(1) != null)
                    {
                        if ((m_node.Attributes.Item(1).Value.StartsWith("Decrypt(") && (m_node.Attributes.Item(0).Value == keyName)))
                        {
                            keyValue = m_node.Attributes.Item(1).Value.Substring(8, m_node.Attributes.Item(1).Value.Length - 9);
                            Encryption EncryptClass = new Encryption();
                            decryptedValue = EncryptClass.DecryptUsingRijndael(keyValue, m_HashConst);
                            EncryptClass = null;
                        }
                    }
                }
            }
            return decryptedValue;
        }


        #endregion Public Methods

        #region Private Methods
        /// <summary>
        /// Checks if the File Type has been Set to either Web.Config or App.Config
        /// If the File Type has not been set then throws up an exception
        /// </summary>
        private void CheckFiletype()
        {
            if (m_FileType != ConfigFileType.APP_CONFIG && m_FileType != ConfigFileType.WEB_CONFIG)
            { throw new Exception("File Type Not Specified "); }
        }
        #endregion Private Methods

    }
}
