using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using log4net;
using log4net.Config;
using System.Configuration;

namespace AccountsModuleService
{
    class DBHandler
    {
        #region Member Declartion
        private static readonly ILog log = LogManager.GetLogger(typeof(DBHandler));
        private string conStr = ConfigurationManager.ConnectionStrings["strDBConnection"].ToString();
        #endregion Member Declaration

        #region Constructor
        public DBHandler()
        {
            log4net.Config.XmlConfigurator.Configure();
        }
        #endregion Constructor

        #region Public Members
        /// <summary>
        /// </summary>
        public string GetAutoDocNo(string optionname)
        {
            string newno = "";

            if (!string.IsNullOrEmpty(optionname))
            {




                //// dtran.Commit(); // Commit transaction if needed                SqlParameter[] sqlParameters = new SqlParameter[11];
                //SqlParameter[] sqlParameters = new SqlParameter[2];

                //    sqlParameters[0] = new SqlParameter("@optionname", optionname);
                //    sqlParameters[1] = new SqlParameter("@newno", newno);
                //    new SqlParameter("@newno", SqlDbType.Int) { Direction = ParameterDirection.Output }


                //    newno = (string) SqlHelper.ExecuteScalar(conStr, CommandType.StoredProcedure, "sp_getnumber", sqlParameters);
                SqlParameter[] sqlParameters = new SqlParameter[]
    {
    new SqlParameter("@optionname", SqlDbType.NVarChar,50) { Value = "MAPBOOKING" },
    new SqlParameter("@newno", SqlDbType.NVarChar ,50) { Direction = ParameterDirection.Output }
    };

                SqlHelper.ExecuteNonQuery(conStr, CommandType.StoredProcedure, "sp_getnumber", sqlParameters);

                // Retrieve the output parameter value
                  newno = (String) sqlParameters[1].Value;


            }

            return newno;
        }

        
        public DataSet SaveXmlErrorData(DataTable dt, string xmlFilePath, string xmlFileName)
        {
            DataSet ds = new DataSet();
            try
            {
                string xml = "<ROOT>";
                for(int i=0; i < dt.Rows.Count; i++)
                {
                    xml += "<ERRORXML>" + Environment.NewLine;
                    xml += "<BOOKINGCODE>" + dt.Rows[i]["BookingCode"].ToString() + "</BOOKINGCODE>" + Environment.NewLine;
                    xml += "<IMPORT_ID>" + dt.Rows[i]["IMPORT_ID"].ToString() + "</IMPORT_ID>" + Environment.NewLine;
                    //xml += "<XMLFILEPATH>" + dt.Rows[i]["XmlFilePath"].ToString() + "</XMLFILEPATH>" + Environment.NewLine;
                    //xml += "<XMLFILENAME>" + dt.Rows[i]["XmlFileName"].ToString() + "</XMLFILENAME>" + Environment.NewLine;
                    xml += "<ELEMENT>" + dt.Rows[i]["Element"].ToString() + "</ELEMENT>" + Environment.NewLine;
                    xml += "<ATTRIBUTE>" + dt.Rows[i]["Attribute"].ToString() + "</ATTRIBUTE>" + Environment.NewLine;
                    xml += "<ERRORDESC>" + dt.Rows[i]["ErrorDesc"].ToString() + "</ERRORDESC>" + Environment.NewLine;
                    xml += "<TYPE>" + dt.Rows[i]["Type"].ToString() + "</TYPE>" + Environment.NewLine;
                    xml += "<UNIQUEATTRIBUTE>" + dt.Rows[i]["UniqueAttribute"].ToString() + "</UNIQUEATTRIBUTE>" + Environment.NewLine;
                    xml += "<UNIQUEATTRIBUTEVALUE>" + dt.Rows[i]["UniqueAttributeValue"].ToString() + "</UNIQUEATTRIBUTEVALUE>" + Environment.NewLine;
                    xml += "</ERRORXML>" + Environment.NewLine;
                }
                xml += "</ROOT>";
                
                SqlParameter[] sqlParameters = new SqlParameter[3];
                sqlParameters[0] = new SqlParameter("@ERRORXML", xml);
                sqlParameters[1] = new SqlParameter("@XMLFILEPATH", xmlFilePath);
                sqlParameters[2] = new SqlParameter("@XMLFILENAME", xmlFileName);
                ds = SqlHelper.ExecuteDataset(conStr, CommandType.StoredProcedure, "sp_add_import_xml_ErrorXml", sqlParameters);
            }
            catch(Exception ex) 
            {
                ds = new DataSet();
                log.Fatal(ex);
            }
            return ds;
        }

        public DataSet SaveXmlSuccessData(string Import_id,string RequestId, string HeaderXml, string GuestXml, string HotelXml, string HotelClientXml, string ServiceXml, string ServiceClientXml, string TransportXml, string TransportClientXml,string xmlFilePath, string xmlFileName)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[12];
                sqlParameters[0] = new SqlParameter("@Import_id", Import_id);
                sqlParameters[1] = new SqlParameter("@NUMBER", RequestId);
                sqlParameters[2] = new SqlParameter("@HEADERXML", HeaderXml);
                sqlParameters[3] = new SqlParameter("@GUESTXML", GuestXml);
                sqlParameters[4] = new SqlParameter("@HOTELXML", HotelXml);
                sqlParameters[5] = new SqlParameter("@HOTELCLIENTXML", HotelClientXml);
                sqlParameters[6] = new SqlParameter("@SERVICEXML", ServiceXml);
                sqlParameters[7] = new SqlParameter("@SERVICECLIENTXML", ServiceClientXml);
                sqlParameters[8] = new SqlParameter("@TRANSPORTXML", TransportXml);//Tanvir
                sqlParameters[9] = new SqlParameter("@TRANSPORTCLIENTXML", TransportClientXml);
                sqlParameters[10] = new SqlParameter("@XMLFILEPATH", xmlFilePath);
                sqlParameters[11] = new SqlParameter("@XMLFILENAME", xmlFileName);
                ds = SqlHelper.ExecuteDataset(conStr, CommandType.StoredProcedure, "sp_add_import_xml_SuccessXml", sqlParameters);
            }
            catch (Exception ex)
            {
                ds = new DataSet();
                log.Fatal(ex);
            }
            return ds;
        }

        public DataSet GetMovetoBookingData()
        {
            DataSet ds = new DataSet();
            try
            {
                ds = SqlHelper.ExecuteDataset(conStr, CommandType.StoredProcedure, "sp_get_import_xml_data");
            }
            catch(Exception ex) 
            {
                ds = new DataSet();
                log.Fatal(ex);
            }
            return ds;
        }

        public DataSet MoveXmlToBooking(string Number, string XmlFileName, string XmlFilePath)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[3];
                sqlParameters[0] = new SqlParameter("@Number", Number);
                sqlParameters[1] = new SqlParameter("@FileName", XmlFileName);
                sqlParameters[2] = new SqlParameter("@filePath", XmlFilePath);
                ds = SqlHelper.ExecuteDataset(conStr, CommandType.StoredProcedure, "sp_importBooking_validate_XML", sqlParameters);
            }
            catch(Exception ex)
            {
                ds = new DataSet();
                log.Fatal(ex);
            }
            return ds;
        }


        public void UpdateXmlDataFlag(string Number, int ResultCode)
        {
            int retValue = 0;

            try
            {

                SqlParameter[] sqlParameters = new SqlParameter[2];
                sqlParameters[0] = new SqlParameter("@NUMBER", Number);
                sqlParameters[1] = new SqlParameter("@RCODE", ResultCode);

                retValue = SqlHelper.ExecuteNonQuery(conStr, CommandType.StoredProcedure, "sp_update_import_xml_flag", sqlParameters);

            }
            catch (Exception ex)
            {
                log.Fatal(ex);
                //return retValue;
            }
        }


        /// Method to update the calculated cost for the particular record based on the Unique Id
        /// </summary>
        public void SetCallCost(string uniqueId, string callDate, string callTime, string duration_s, string callerNo, string calledNo, string trunkId)
        {
            //string fileData = string.Empty;
            //AlertService alertService = null;
            //try
            //{
            //    SqlParameter[] sqlParameters = new SqlParameter[7];
            //    sqlParameters[0] = new SqlParameter("@i_unique_id", uniqueId);
            //    sqlParameters[1] = new SqlParameter("@i_call_date", callDate);
            //    sqlParameters[2] = new SqlParameter("@i_call_time", callTime);
            //    sqlParameters[3] = new SqlParameter("@i_duration_s", duration_s);
            //    sqlParameters[4] = new SqlParameter("@i_caller_no", callerNo);
            //    sqlParameters[5] = new SqlParameter("@i_called_no", calledNo);
            //    sqlParameters[6] = new SqlParameter("@i_trunk_id", trunkId);

            //    int retValue = 0;
            //    if (InitialContext.configDetails.PBXId == "1" || InitialContext.configDetails.PBXId == "2" || InitialContext.configDetails.PBXId == "7"
            //        || InitialContext.configDetails.PBXId == "8" || InitialContext.configDetails.PBXId == "9")
            //        retValue = SqlHelper.ExecuteNonQuery(InitialContext.ConnectionString, CommandType.StoredProcedure, "SetCallCost", sqlParameters);
            //    else if (InitialContext.configDetails.PBXId == "3" || InitialContext.configDetails.PBXId == "5")
            //        retValue = SqlHelper.ExecuteNonQuery(InitialContext.ConnectionString, CommandType.StoredProcedure, "SetCallCost1", sqlParameters);

            //    log.DebugFormat("retValue : {0}", retValue);

            //    log.InfoFormat("Call cost updated Successfully for the Unique Id : {0}", uniqueId);
            //    log.Info("Process Data Ends----------------------------------------------------------------");
          //  }
            //catch (SqlException sqlex)
            //{
            //    log.Fatal(sqlex);
            //    if (Convert.ToInt32(InitialContext.configDetails.AlertLevel) >= 1)
            //    {
            //        if (InitialContext.CheckAlertTimeDifference("DBF1"))
            //        {
            //            alertService = new AlertService();
            //            alertService.SendAlert("FATAL! - " + InitialContext.configDetails.ServiceName, "Method - SetCallCost() \n\nSqlException : Unable to calculate call cost\n\n" + sqlex.Message);
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    log.Error(ex);
            //}
            //finally
            //{
            //    alertService = null;
            //}

        

}

    


        #endregion Public Members
    }
    
}
