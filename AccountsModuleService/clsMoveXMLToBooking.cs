using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using log4net.Config;

namespace AccountsModuleService
{
    class clsMoveXMLToBooking
    {
        #region Member Declaration
        private static readonly ILog log = LogManager.GetLogger(typeof(clsXMLFileReader));
        public Timer dbPoolTimer = null;
        DBHandler dbHandler = null;
        #endregion Member Declaration

        #region Constructor
        public clsMoveXMLToBooking()
        {
            log4net.Config.XmlConfigurator.Configure();
        }
        #endregion Constructor

        #region Public Methods

        public void MoveXMLToBooking()
        {
            try
            {
                DataSet dataSet = new DataSet();
                dbHandler = new DBHandler();
                dataSet = dbHandler.GetMovetoBookingData();
                if(dataSet != null )
                {
                    if(dataSet.Tables.Count > 0)
                    {
                        DataTable dt = new DataTable();
                        dt = dataSet.Tables[0];
                        for(int i = 0; i < dt.Rows.Count; i++)
                        {
                            string Number = dt.Rows[i]["NUMBER"].ToString();
                            string XmlFileName = dt.Rows[i]["XMLFILENAME"].ToString();
                            string XmlFilePath = dt.Rows[i]["XMLFILEPATH"].ToString();
                            dbHandler = new DBHandler();
                            DataSet ds = new DataSet();
                            ds = dbHandler.MoveXmlToBooking(Number, XmlFileName, XmlFilePath);
                            if(ds != null)
                            {
                                if(ds.Tables.Count > 0)
                                {
                                    DataTable dtResult = new DataTable();
                                    dtResult = ds.Tables[0];
                                    int resultcode = Convert.ToInt32(dtResult.Rows[0]["Resultcode"]);
                                    //if(resultcode == 0)
                                    //{
                                        
                                    //}
                                    dbHandler = new DBHandler();
                                    dbHandler.UpdateXmlDataFlag(Number, resultcode);
                                    log.Info("--------------XML response saved successfully--------------");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        #endregion Public Methods

    }
}
