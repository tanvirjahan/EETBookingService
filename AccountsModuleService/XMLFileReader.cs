using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
//using System.Threading;
using System.Threading.Tasks;
using log4net;
using System.Timers;

namespace AccountsModuleService
{
    partial class XMLFileReader : ServiceBase
    {
        #region Private Members
        private static readonly ILog log = LogManager.GetLogger(typeof(XMLFileReader));
        private Timer timer;
        clsXMLFileReader clsXMLFileReader = null;
        #endregion Private Members
        public XMLFileReader()
        {
            InitializeComponent();
            this.ServiceName = ConfigurationManager.AppSettings.Get("XMLServiceName");
        }

        protected override void OnStart(string[] args)
        {
            // TODO: Add code here to start your service.
            try
            {
                //clsXMLFileReader = new clsXMLFileReader();
                //clsXMLFileReader.Thread_Start();

                // Create and configure the timer
                timer = new Timer();
                timer.Interval = 60 * 60 * 1000;//4 * 60 * 60 * 1000;// 5 * 60 * 1000; // 5 minutes in milliseconds
                timer.Elapsed += OnElapsedTime;
                timer.AutoReset = true;
                timer.Start();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        protected override void OnStop()
        {
            // TODO: Add code here to perform any tear-down necessary to stop your service.
            try
            {
                log.InfoFormat("Stop Service - {0}", this.ServiceName);
                //clsXMLFileReader = new clsXMLFileReader();
                //clsXMLFileReader.Thread_Stop();

                timer.Stop();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        private void OnElapsedTime(object sender, ElapsedEventArgs e)
        {
            try
            {
                if (InitialContext.InitializeConfigSettings(this.ServiceName))
                {
                    //if (stateTimer != null)
                    //{
                    //    stateTimer.Dispose();
                    //    timerDelegate = null;
                    //}

                    clsXMLFileReader= new clsXMLFileReader();
                    clsXMLFileReader.ValidateXML();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        public void onDebug()
        {
            OnStart(null);
        }
    }
}
