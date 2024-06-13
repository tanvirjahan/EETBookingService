using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using log4net;
using log4net.Config;

namespace AccountsModuleService
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            //#if DEBUG
            //            //While debugging this section is used.
            //XMLFileReader myService = new XMLFileReader();
            //myService.onDebug();
            //System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite);

            //#else
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                              new ConnecToBookingAPI(),
                             new XMLFileReader()
                           // new MoveXMLToBooking()
            };
            ServiceBase.Run(ServicesToRun);
//#endif
      //      Service

            //clsConnectToAPI clsConnectAPI = new clsConnectToAPI();
            //clsConnectAPI.GetBookingXML();

            //clsXMLFileReader clsXMLFileReader = new clsXMLFileReader();
            //clsXMLFileReader.ValidateXML();
            //Service

            //clsMoveXMLToBooking clsMoveXMLToBooking = new clsMoveXMLToBooking();
            //clsMoveXMLToBooking.MoveXMLToBooking();
        }

    }
}
