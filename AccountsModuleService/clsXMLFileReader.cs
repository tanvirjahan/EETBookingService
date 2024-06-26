﻿using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Timers;
using log4net.Config;

namespace AccountsModuleService
{
    class clsXMLFileReader
    {
        #region Member Declaration
        private static readonly ILog log = LogManager.GetLogger(typeof(clsXMLFileReader));
        public System.Timers.Timer dbPoolTimer = null;
        public Thread _thread;
        #endregion Member Declaration

        #region Constructor
        public clsXMLFileReader()
        {
            log4net.Config.XmlConfigurator.Configure();
        }
        #endregion Constructor

        #region Public Methods

        public void Thread_Start()
        {
            this._thread = new Thread(() => this.ValidateXML());
            this._thread.Start();

            //this.Reader_Ips(LocationName);
        }

        public void Thread_Stop()
        {
            this._thread.Abort();
        }
         
        public void ValidateXML()
        {
            try
            {
                XMLComparator comparator = new XMLComparator();
                comparator.ValidateXMLDocument();


                //(new clsXMLFileReader()).ValidateXML();
            }
            catch (Exception ex)
            {
                log.Error(ex);
              
                //(new clsXMLFileReader()).Thread_Start();
            }
        }

        #endregion Public Methods
    }
}
