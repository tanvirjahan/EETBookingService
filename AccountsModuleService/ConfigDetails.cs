using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace AccountsModuleService
{
    class ConfigDetails
    {
        #region Private DataMembers

        private string siteName = string.Empty;
        private string serviceName = string.Empty;
        private string pbxId = string.Empty;
        private string licenseKey = string.Empty;
        private string dbSeekIntvl = string.Empty;
        private string alertLevel = string.Empty;
        private string senderEmail = string.Empty;
        private string alertEmail = string.Empty;
        private string smtpName = string.Empty;
        private string smtpPort = string.Empty;
        private string smtpUser = string.Empty;
        private string smtpPwd = string.Empty;
        private string mailPriority = string.Empty;
        private string mailEncoding = string.Empty;
        private string alertIntvl = string.Empty;
        private int columnCount = 0;

        private ArrayList alertEmailList = new ArrayList();


        #endregion Private DataMembers

        #region Public Properties

        public string SiteName
        {
            get { return siteName; }
            set { siteName = value; }
        }


        /// <summary>
        /// Gets or Sets ServiceName.
        /// </summary>
        public string ServiceName
        {
            get { return serviceName; }
            set { serviceName = value; }
        }

        /// <summary>
        /// Gets or Sets PBX Id.
        /// </summary>
        public string PBXId
        {
            get { return pbxId; }
            set { pbxId = value; }
        }

        /// <summary>
        /// Gets or Sets the License
        /// </summary>
        public string LicenseKey
        {
            get { return licenseKey; }
            set { licenseKey = value; }
        }

        /// <summary>
        /// Gets or Sets the Database Seek Inteval.
        /// </summary>
        public string DbSeekIntvl
        {
            get { return dbSeekIntvl; }
            set { dbSeekIntvl = value; }
        }

        
        /// <summary>
        /// Gets or Sets Alert Level.
        /// </summary>
        public string AlertLevel
        {
            get { return alertLevel; }
            set { alertLevel = value; }
        }

        /// <summary>
        /// Gets or Sets Sender Email.
        /// </summary>
        public string SenderEmail
        {
            get { return senderEmail; }
            set { senderEmail = value; }
        }

        /// <summary>
        /// Gets or Sets Alert Email.
        /// </summary>
        public string AlertEmail
        {
            get { return alertEmail; }
            set { alertEmail = value; }
        }

        /// <summary>
        /// Gets or Sets SMTP Server Name/IP.
        /// </summary>
        public string SMTPName
        {
            get { return smtpName; }
            set { smtpName = value; }
        }

        /// <summary>
        /// Gets or Sets SMTP Port No.
        /// </summary>
        public string SMTPPort
        {
            get { return smtpPort; }
            set { smtpPort = value; }
        }

        /// <summary>
        /// Gets or Sets SMTP User Name.
        /// </summary>
        public string SMTPUser
        {
            get { return smtpUser; }
            set { smtpUser = value; }
        }

        /// <summary>
        /// Gets or Sets SMTP Password.
        /// </summary>
        public string SMTPPwd
        {
            get { return smtpPwd; }
            set { smtpPwd = value; }
        }

        /// <summary>
        /// Gets or Sets Mail Priority
        /// </summary>
        public string MailPriority
        {
            get { return mailPriority; }
            set { mailPriority = value; }
        }

        /// <summary>
        /// Gets or Sets Mail Endoing format
        /// </summary>
        public string MailEncoding
        {
            get { return mailEncoding; }
            set { mailEncoding = value; }
        }

        /// <summary>
        /// Gets or Sets the Alert Send Interval.
        /// </summary>
        public string AlertInvl
        {
            get { return alertIntvl; }
            set { alertIntvl = value; }
        }
        /// <summary>
        /// Gets or Sets Column count.
        /// </summary>
        public int ColumnCount
        {
            get { return columnCount; }
            set { columnCount = value; }
        }

        /// <summary>
        /// Gets or Sets the Alert Email List.
        /// </summary>
        public ArrayList AlertEmailList
        {
            get { return alertEmailList; }
            set { alertEmailList = value; }
        }
        #endregion Public Properties

    }
 
}
