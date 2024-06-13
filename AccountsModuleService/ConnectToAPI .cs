using log4net;
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
using System.Timers;

namespace AccountsModuleService
{
    partial class MoveXMLToBooking : ServiceBase
    {
        #region Private Members
        private static readonly ILog log = LogManager.GetLogger(typeof(XMLFileReader));
        private Timer timer;
        clsMoveXMLToBooking clsMoveXMLToBooking = null;
        #endregion Private Members
        public MoveXMLToBooking()
        {
            InitializeComponent();
            this.ServiceName = ConfigurationManager.AppSettings.Get("XMLToBooking");
        }

        protected override void OnStart(string[] args)
        {
            // TODO: Add code here to start your service.
            try
            {
                timer = new Timer();
                timer.Interval = 5 * 60 * 1000; // 5 minutes in milliseconds
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
                if (clsMoveXMLToBooking != null)
                {
                    //if (clsXMLFileReader.dbPoolTimer != null)
                    //{
                    //    csvdownload.dbPoolTimer.Stop();
                    //    csvdownload.dbPoolTimer.Close();
                    //    log.Info("Timer Object disposed successfully......");
                    //}
                    clsMoveXMLToBooking = null;
                }

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
                    clsMoveXMLToBooking = new clsMoveXMLToBooking();
                    clsMoveXMLToBooking.MoveXMLToBooking();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }
    }
}
