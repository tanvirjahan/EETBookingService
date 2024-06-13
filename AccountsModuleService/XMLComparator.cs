using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using log4net;
using log4net.Config;
using static log4net.Appender.RollingFileAppender;


//using Microsoft.VisualBasic.Logging;
//using Microsoft.VisualBasic.Logging;

namespace AccountsModuleService
{
    public class XMLComparator
    {
        #region Private Members
        private string xmlFilePath = ConfigurationManager.AppSettings.Get("XMLFilePath");
        private string ErrorxmlFilePath = ConfigurationManager.AppSettings.Get("ErrorXMLFilePath");
        private string SuccessxmlFilePath = ConfigurationManager.AppSettings.Get("SuccessXMLFilePath");
        private string NoDataxmlFilePath = ConfigurationManager.AppSettings.Get("NoDataxmlFilePath");
        private string LogxmlFilePath = ConfigurationManager.AppSettings.Get("LogxmlFilePath");
        #endregion Private Members

        public DataTable dtErrorRecord = new DataTable();

        public XMLComparator() 
        {
            dtErrorRecord.Columns.Add("Import_id", typeof(string));//Tanvir
            dtErrorRecord.Columns.Add("BookingCode", typeof(string));
            dtErrorRecord.Columns.Add("XmlFileName", typeof(string));
            dtErrorRecord.Columns.Add("Element", typeof(string));
            dtErrorRecord.Columns.Add("Attribute", typeof(string));
            dtErrorRecord.Columns.Add("UniqueAttribute", typeof(string));
            dtErrorRecord.Columns.Add("UniqueAttributeValue", typeof(string));
            dtErrorRecord.Columns.Add("ErrorDesc", typeof(string));
            dtErrorRecord.Columns.Add("Type", typeof(string));
            dtErrorRecord.AcceptChanges();
        }

        #region enum declaration

        enum ReservationAttributes
        {
            NUMBER,
            DATE,
            BEG,
            END,
            PARTNER_INC,
            PARTNER,
            PARTNER_CONTRACT,
            PARTNER_INN,
            TOUR_INC,
            TOUR,
            NOTE,
            DEPTSUM,
            DEPTCURRENCY,
            PROFITSUM,
            PROFITCURRENCY,
            STATUS_INC,
            STATUS,
            PARTNER_TOWN_INC,
            PARTNER_TOWN,
            PARTNER_STATE_INC,
            PARTNER_STATE,
            OWNER,
            OWNER_NAME

        }

        enum PeopleAttributes
        {
            ID,
            HUMAN,
            NAME,
            LNAME,
            PNUMBER,
            PSER,
            GENDER,
            BORN
        }

        enum HotelAttributes
        {
            ID,
            BEG,
            END,
            HOTEL_INC,
            HOTEL,
            ROOM_INC,
            ROOM,
            PLACE_INC,
            PLACE,
            MEAL_INC,
            MEAL,
            CNT,
            PARTNER_INC,
            PARTNER,
            PARTNER_INN,
            NETPRICE,
            NETPRICEDETAIL,
            NETCURRENCY_INC,
            NETCURRENCY,
            SALEPRICE,
            SALEPRICEDETAIL,
            SALECURRENCY_INC,
            SALECURRENCY,
            ACTSTATUS_INC,
            ACTSTATUS,
            HOTEL_TOWN_INC,
            HOTEL_TOWN,
            HOTEL_STATE_INC,
            HOTEL_STATE,
            PARTNER_TOWN_INC,
            PARTNER_TOWN,
            PARTNER_STATE_INC,
            PARTNER_STATE,
            PARTNER_CONTRACT
        }

        enum HotelClientAttributes
        {
            ID, 
            PEOPLEID, 
            COMMON
        }

        enum ServiceAttributes
        {
            ID,
            BEG,
            END,
            SERVICE_INC,
            SERVICE,
            SERVICETYPE_INC,
            SERVICETYPE,
            CNT,
            PARTNER_INC,
            PARTNER,
            PARTNER_INN,
            SALEPRICE,
            SALEPRICEDETAIL,
            SALECURRENCY_INC,
            SALECURRENCY,
            ACTSTATUS_INC,
            ACTSTATUS,
            PARTNER_TOWN_INC,
            PARTNER_TOWN,
            PARTNER_STATE_INC,
            PARTNER_STATE,
            PARTNER_CONTRACT,
            NETPRICEDETAIL,
            NETCURRENCY_INC,
            NETCURRENCY,
            NETPRICE
          

        }

        enum ServiceClientAttributes
        {
            ID,
            PEOPLEID
        }
        //Tanvir 
        enum TransportAttributes        {
            ID,
            BEG,
            END,
            TRANSPORT_INC,
            TRANSPORT,
            CLASS_INC,
            CLASS,
            SERVICETYPE,
            CNT,
            PARTNER_INC,
            PARTNER,
            PARTNER_INN,
            SALEPRICE,
            SALEPRICEDETAIL,
            SALECURRENCY_INC,
            SALECURRENCY,
            ACTSTATUS_INC,
            ACTSTATUS,
            PARTNER_TOWN_INC,
            PARTNER_TOWN,
            PARTNER_STATE_INC,
            PARTNER_STATE,
            PARTNER_CONTRACT 
    }
        enum TransportClientAttributes
        {
            ID,
            PEOPLEID,
            COMMON
        }
//Tanvir
        enum MandatoryReservationAttributes
        {
            NUMBER,
            DATE,
            BEG,
            END,
            PARTNER_INC
            //PARTNER,
            //TOUR,
            //DEPTSUM,
            //DEPTCURRENCY,
            //STATUS_INC,
            //STATUS
            //PARTNER_TOWN_INC,
            //PARTNER_TOWN
        }

        #endregion enum declaration
        public void WriteLog(string strMethodName, string userMessage)
        {

            try
            {
                StreamWriter SW;
                string StrErrTime = "";
                string StrFileName = "";
                var dToday = DateTime.Now;
                string DailyDatefolder = DateTime.Now.ToString("dd-MM-yyyy");
                string Basefolder = "";
                // string filePath2 = @"d:\WebQnex\EET\AccountsModule\Log" + "\\" + DailyDatefolder;
                string filePath2 = LogxmlFilePath + "\\" + DailyDatefolder;
                if (!Directory.Exists(filePath2))
                {
                    Directory.CreateDirectory(filePath2);
                }
                Basefolder = filePath2;

                string Todaysdate = DateTime.Now.ToString("ddMMyyyy");
                StrFileName = Basefolder + "\\" + "Log_" + Todaysdate + ".txt";

                StrErrTime = DateTime.Now.ToString("dd-MM-yyyy-hh:mm:ss");

                // Check if the log file for today exists, if not create a new one
                if (!File.Exists(StrFileName))
                {
                    SW = File.CreateText(StrFileName);
                }
                else
                {
                    // Append to existing log file
                    SW = File.AppendText(StrFileName);
                }

                // Writing user-defined message
                SW.WriteLine(StrErrTime + " : " + userMessage);

                SW.Close();
            }
            catch (Exception Ex)
            {

                // Handle any exceptions occurred during logging, if needed
            }
        }

        public void WriteLog(Exception ex, string strMethodName)
        {
            try
            {
                StreamWriter SW;
                string StrErrTime = "";
                string StrFileName = "";
                var dToday = DateTime.Now;
                string DailyDatefolder = DateTime.Now.ToString("dd-MM-yyyy");
                string Basefolder = "";
               // string filePath2 = @"d:\WebQnex\EET\AccountsModule\\Log" + "\\" + DailyDatefolder;
                string filePath2 = LogxmlFilePath + "\\" + DailyDatefolder;
                if (!Directory.Exists(filePath2))
                {
                    Directory.CreateDirectory(filePath2);
                }
                Basefolder = filePath2;

                string Todaysdate = DateTime.Now.ToString("ddMMyyyy");
                // StrFileName = Basefolder + "\\" + "Log_" + strMethodName + "_" + Todaysdate;
                StrFileName = Basefolder + "\\" + "Log_" + Todaysdate + ".txt";
                StrErrTime = DateTime.Now.ToString("dd-MM-yyyy-hh:mm:ss");

                if (!File.Exists(StrFileName))
                {
                    SW = File.CreateText(StrFileName);
                }
                else
                {
                    // Append to existing log file
                    SW = File.AppendText(StrFileName);
                }

                SW.WriteLine(StrErrTime + " : " + ex.ToString());

                SW.Close();
            }
            catch (Exception Ex)
            {

            }
        }
        public void ConnectAndSaveResponse()
        {

            using (HttpClient client = new HttpClient())
            {
                string webServiceUrl = ConfigurationManager.AppSettings.Get("webServiceUrl");
              //  string webServiceUrl = "https://online.eet.ae/xYz85g36a47P9/?test=1"; Tanvir 17052024
                HttpResponseMessage response = client.GetAsync(webServiceUrl).Result;
                string currentTime = DateTime.Now.ToString("HH:mm:ss");
                if (response.IsSuccessStatusCode)
                {

                    string xmlResponse = response.Content.ReadAsStringAsync().Result;
                     // string directoryPath = @"D:\WebQnex\EET\AccountsModule\reservation_new";
                    string directoryPath = ConfigurationManager.AppSettings.Get("XMLFilePath");
                    string fileName = $"reservation_{DateTime.Now:yyyyMMdd_HHmmss}.xml";
                    string filePath = Path.Combine(directoryPath, fileName);
                    string  xmlContent = RemoveXmlCommentsBeforeReservations(xmlResponse);
                    File.WriteAllText(filePath, xmlContent);

                   


                    WriteLog ("["+ DateTime.Now.ToString("HH:mm:ss") + "]"+  "ConnectAndSaveResponse", "XML response saved successfully From API.");
                }
                else
                {
                    WriteLog("[" + DateTime.Now.ToString("HH:mm:ss") + "]" + "ConnectAndSaveResponse", $"XML Error From API: {response.StatusCode}");
                }
            }
        }
        // Function to remove comments before <RESERVATIONS> root node
        private string RemoveXmlCommentsBeforeReservations(string xmlContent)
        {
            // Find the position of the <RESERVATIONS> root node
            int reservationsIndex = xmlContent.IndexOf("<RESERVATIONS>");

            // If <RESERVATIONS> root node exists
            if (reservationsIndex >= 0)
            {
                // Get the substring before <RESERVATIONS>
                string substringBeforeReservations = xmlContent.Substring(0, reservationsIndex);

                // If there is any text or elements before <RESERVATIONS>
                if (!string.IsNullOrWhiteSpace(substringBeforeReservations))
                {
                    // Remove any text or elements before <RESERVATIONS>
                    xmlContent = xmlContent.Substring(reservationsIndex);
                    // Replace '&' with '&amp;'
                    xmlContent = xmlContent.Replace("&", "&amp;");
                }

              
            }

            return xmlContent;
        }
        
        public void ValidateXMLDocument()
        {
            int ERRORCNT = 0;
            int CURRENTNODE = 0;
            try
            {
                string xmlFileName = "";
                //string xmlFileLocation = xmlFilePath + xmlFileName;
                List<string> attributes = new List<string>();
                List<string> attributesValue = new List<string>();
                string RequestId = "", HotelId = "", ServiceId= "", UniqueAttr = "", UniqueAttrValue = "",TransportId="";
                string HeaderXml = "", GuestXml = "", HotelXml = "", HotelClientXml = "", ServiceXml = "", ServiceClientXml = "",TransportXml="",TransportClientXml="";
                string[] xmlFiles = null;
                IEnumerable<string> fileEnumerable = Directory.EnumerateFiles(xmlFilePath, "*.xml");

                if(fileEnumerable.Any())
                {
                    xmlFiles = fileEnumerable.ToArray();
                }
                //if (Directory.Exists(xmlFilePath))
                if(xmlFiles != null)
                {
                    if (xmlFiles.Length > 0)
                    {
                        
                        foreach (string xmlFile in xmlFiles)
                        {
                            HeaderXml = ""; GuestXml = ""; HotelXml = ""; HotelClientXml = ""; ServiceXml = ""; ServiceClientXml = "";
                            TransportClientXml = ""; TransportXml = "";
                            dtErrorRecord.Rows.Clear();
                            //dtErrorRecord = null;
                        
                            xmlFileName = xmlFile;
                            string fileContent = File.ReadAllText(xmlFile);
                            if (fileContent.Contains("There is no data to change."))
                            {
                                // Save to log
                               Console.WriteLine($"File '{xmlFile}' contains 'There is no data to change.'. Saving to log...");
                                string noData = "NoData";
                                MoveXmlFiles(xmlFile, "NoData");
                                // return noData; 
                              

                            }
                            else
                            {

                                dtErrorRecord.Rows.Clear();
                                //dtErrorRecord = null;
                                XmlDocument xmlDocument1 = new XmlDocument();
                                xmlDocument1.Load(xmlFile);

                                String Import_id;

                                //XmlNodeList nodes1 = xmlDocument1.SelectNodes("//*");
                                XmlNodeList reservationsNodes = xmlDocument1.SelectNodes("//RESERVATIONS");
                                if (reservationsNodes != null)
                                {
                                    if (reservationsNodes.Count > 0)
                                    {
                                        foreach (XmlNode reservationNode in reservationsNodes)
                                        {
                                            Import_id = GetImportId();
                                            CURRENTNODE = 0;
                                            HeaderXml = ""; GuestXml = ""; HotelXml = ""; HotelClientXml = ""; ServiceXml = ""; ServiceClientXml = "";
                                            TransportClientXml = ""; TransportXml = "";
                                            //Console.WriteLine("Parent Node: " + reservationNode.Name);

                                            XmlNodeList reservationNodes = reservationNode.ChildNodes;
                                            if (reservationNodes.Count > 0)
                                            {
                                                foreach (XmlNode xmlNode in reservationNodes)
                                                {
                                                    UniqueAttr = "";
                                                    UniqueAttrValue = "";
                                                    HeaderXml = ""; 
                                                    //Tanvir 2105
                                                     GuestXml = ""; HotelXml = ""; HotelClientXml = ""; ServiceXml = ""; ServiceClientXml = "";
                                                    TransportClientXml = ""; TransportXml = "";
                                                    //tanvir 2105
                                                    // Reservation Attributes checking start
                                                    int lastReservationNode = 0;
                                                    dtErrorRecord.Clear();
                                                    lastReservationNode = reservationNodes.Count ;
                                                    CURRENTNODE++;

                                                    if (xmlNode.Attributes != null)
                                                    {
                                                        
                                                       attributes = new List<string>();
                                                        attributesValue = new List<string>();
                                                        HeaderXml = "<ROOT><HEADER>" + Environment.NewLine;
                                                        foreach (XmlAttribute attr in xmlNode.Attributes)
                                                        {
                                                            attributes.Add(attr.Name.ToString());
                                                            if (attr.Value != null && attr.Value != "")
                                                            {
                                                                attributesValue.Add(attr.Name.ToString());

                                                                if (attr.Name.ToString() == "NUMBER")
                                                                {
                                                                    RequestId = attr.Value;
                                                                    UniqueAttr = attr.Name.ToString();
                                                                    UniqueAttrValue = attr.Value;
                                                                }
                                                            }

                                                            HeaderXml += "<" + attr.Name.ToString() + ">" + attr.Value.ToString() + "</" + attr.Name.ToString() + ">" + Environment.NewLine;

                                                        }
                                                        HeaderXml += "</HEADER></ROOT>" + Environment.NewLine;

                                                        List<string> MissingReservationAttributes = FindMissingReservationAttr(attributes);
                                                        if (MissingReservationAttributes != null)
                                                        {
                                                            if (MissingReservationAttributes.Count > 0)
                                                            {
                                                                //Error table record
                                                                AddErrorData(Import_id,RequestId, xmlFileName, xmlNode.Name.ToUpper(), MissingReservationAttributes, "Missing", UniqueAttr, UniqueAttrValue);
                                                            }
                                                        }

                                                        List<string> NewReservationAttributes = FindNewReservationAttr(attributes);
                                                        if (NewReservationAttributes != null)
                                                        {
                                                            if (NewReservationAttributes.Count > 0)
                                                            {
                                                                //Error table record
                                                                AddErrorData(Import_id,RequestId, xmlFileName, xmlNode.Name.ToUpper(), NewReservationAttributes, "New", UniqueAttr, UniqueAttrValue);
                                                            }
                                                        }

                                                        List<string> mandatoryAttributes = FindMissingMandatoryReservationAttr(attributesValue);
                                                        if (mandatoryAttributes != null)
                                                        {
                                                            if (mandatoryAttributes.Count > 0)
                                                            {
                                                                //Error table record
                                                                AddErrorData(Import_id,RequestId, xmlFileName, xmlNode.Name.ToUpper(), mandatoryAttributes, "Mandatory", UniqueAttr, UniqueAttrValue);
                                                            }
                                                        }
                                                    }

                                                    else
                                                    {
                                                        //Error table record
                                                    }
                                                    // Reservation Attributes checking end

                                                    // Reservation Child Element checking start
                                                    XmlNodeList reservationChildNodeList = xmlNode.ChildNodes;
                                                    if (reservationChildNodeList != null && reservationChildNodeList.Count > 0)
                                                    {
                                                        foreach (XmlNode xmlreservationChildNode in reservationChildNodeList)
                                                        {
                                                            if (xmlreservationChildNode.Name.ToUpper() == "PEOPLES")
                                                            {
                                                                XmlNodeList peopleNodeList = xmlreservationChildNode.ChildNodes; //.SelectNodes("//PEOPLES");

                                                                if (peopleNodeList.Count > 0)
                                                                {
                                                                    GuestXml = "<ROOT><GUESTS>" + Environment.NewLine;
                                                                    foreach (XmlNode peopleNode in peopleNodeList)
                                                                    {
                                                                        UniqueAttr = "";
                                                                        UniqueAttrValue = "";
                                                                        if (peopleNode.Attributes != null)
                                                                        {
                                                                            attributes = new List<string>();
                                                                            GuestXml += "<GUEST>" + Environment.NewLine;
                                                                            GuestXml += "<NUMBER>" + RequestId + "</NUMBER>" + Environment.NewLine;
                                                                            foreach (XmlAttribute attr in peopleNode.Attributes)
                                                                            {
                                                                                attributes.Add(attr.Name.ToString());

                                                                                if (attr.Name.ToString() == "ID")
                                                                                {
                                                                                    UniqueAttr = attr.Name.ToString();
                                                                                    UniqueAttrValue = attr.Value;
                                                                                }

                                                                                GuestXml += "<" + attr.Name.ToString() + ">" + attr.Value.ToString() + "</" + attr.Name.ToString() + ">" + Environment.NewLine;
                                                                            }
                                                                            GuestXml += "</GUEST>" + Environment.NewLine;
                                                                            List<string> MissingPeopleAttributes = FindMissingPeopleAttr(attributes);
                                                                            if (MissingPeopleAttributes != null)
                                                                            {
                                                                                if (MissingPeopleAttributes.Count > 0)
                                                                                {
                                                                                    //Error table record
                                                                                    AddErrorData(Import_id,RequestId, xmlFileName, peopleNode.Name.ToUpper(), MissingPeopleAttributes, "Missing", UniqueAttr, UniqueAttrValue);
                                                                                }
                                                                            }

                                                                            List<string> NewPeopleAttributes = FindNewPeopleAttr(attributes);
                                                                            if (NewPeopleAttributes != null)
                                                                            {
                                                                                if (NewPeopleAttributes.Count > 0)
                                                                                {
                                                                                    //Error table record
                                                                                    AddErrorData(Import_id,RequestId, xmlFileName, peopleNode.Name.ToUpper(), NewPeopleAttributes, "New", UniqueAttr, UniqueAttrValue);
                                                                                }
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            //Error table record
                                                                        }
                                                                    }
                                                                    GuestXml += "</GUESTS></ROOT>";
                                                                }
                                                                else
                                                                {
                                                                    //Error table record
                                                                    attributes = new List<string>();
                                                                    AddErrorData(Import_id,RequestId, xmlFileName, "PEOPLE", attributes, "Mandatory", UniqueAttr, UniqueAttrValue);
                                                                }
                                                            }

                                                            if (xmlreservationChildNode.Name.ToUpper() == "HOTELS")
                                                            {
                                                                XmlNodeList hotelNodeList = xmlreservationChildNode.ChildNodes; //.SelectNodes("//HOTELS");

                                                                if (hotelNodeList.Count > 0)
                                                                {
                                                                    HotelXml = "<ROOT><HOTELS>" + Environment.NewLine;
                                                                    HotelClientXml = "<ROOT><CLIENTS>" + Environment.NewLine;
                                                                    foreach (XmlNode hotelNode in hotelNodeList)
                                                                    {
                                                                        UniqueAttr = "";
                                                                        UniqueAttrValue = "";
                                                                        if (hotelNode.Attributes != null)
                                                                        {
                                                                            attributes = new List<string>();
                                                                            HotelXml += "<HOTEL>" + Environment.NewLine;
                                                                            HotelXml += "<NUMBER>" + RequestId + "</NUMBER>" + Environment.NewLine;
                                                                            foreach (XmlAttribute attr in hotelNode.Attributes)
                                                                            {
                                                                                attributes.Add(attr.Name.ToString());

                                                                                if (attr.Name.ToString() == "ID")
                                                                                {
                                                                                    UniqueAttr = attr.Name.ToString();
                                                                                    UniqueAttrValue = attr.Value;
                                                                                    HotelId = attr.Value;
                                                                                }

                                                                                HotelXml += "<" + attr.Name.ToString() + ">" + attr.Value.ToString() + "</" + attr.Name.ToString() + ">" + Environment.NewLine;
                                                                            }
                                                                            HotelXml += "</HOTEL>" + Environment.NewLine;
                                                                            List<string> MissingHotelAttributes = FindMissingHotelAttr(attributes);
                                                                            if (MissingHotelAttributes != null)
                                                                            {
                                                                                if (MissingHotelAttributes.Count > 0)
                                                                                {
                                                                                    //Error table record
                                                                                    AddErrorData(Import_id,RequestId, xmlFileName, hotelNode.Name.ToUpper(), MissingHotelAttributes, "Missing", UniqueAttr, UniqueAttrValue);
                                                                                }
                                                                            }

                                                                            List<string> NewHotelAttributes = FindNewHotelAttr(attributes);
                                                                            if (NewHotelAttributes != null)
                                                                            {
                                                                                if (NewHotelAttributes.Count > 0)
                                                                                {
                                                                                    //Error table record
                                                                                    AddErrorData(Import_id,RequestId, xmlFileName, hotelNode.Name.ToUpper(), NewHotelAttributes, "New", UniqueAttr, UniqueAttrValue);
                                                                                }
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            //Error table record
                                                                        }

                                                                        UniqueAttr = "";
                                                                        UniqueAttrValue = "";

                                                                        XmlNodeList clientsNodeList = hotelNode.ChildNodes; //.SelectNodes("//CLIENTS");

                                                                        if (clientsNodeList.Count > 0)
                                                                        {
                                                                            foreach (XmlNode clientsNode in clientsNodeList)
                                                                            {
                                                                                XmlNodeList clientNodeList = clientsNode.ChildNodes;

                                                                                foreach (XmlNode clientNode in clientNodeList)
                                                                                {
                                                                                    if (clientNode.Attributes != null)
                                                                                    {
                                                                                        attributes = new List<string>();
                                                                                        HotelClientXml += "<CLIENT>" + Environment.NewLine;
                                                                                        HotelClientXml += "<NUMBER>" + RequestId + "</NUMBER>" + Environment.NewLine;
                                                                                        HotelClientXml += "<HOTELID>" + HotelId + "</HOTELID>" + Environment.NewLine;
                                                                                        foreach (XmlAttribute attr in clientNode.Attributes)
                                                                                        {
                                                                                            attributes.Add(attr.Name.ToString());

                                                                                            if (attr.Name.ToString() == "ID")
                                                                                            {
                                                                                                UniqueAttr = attr.Name.ToString();
                                                                                                UniqueAttrValue = attr.Value;
                                                                                            }

                                                                                            HotelClientXml += "<" + attr.Name.ToString() + ">" + attr.Value.ToString() + "</" + attr.Name.ToString() + ">" + Environment.NewLine;
                                                                                        }
                                                                                        HotelClientXml += "</CLIENT>" + Environment.NewLine;
                                                                                        List<string> MissingClientAttributes = FindMissingClientAttr(attributes);
                                                                                        if (MissingClientAttributes != null)
                                                                                        {
                                                                                            if (MissingClientAttributes.Count > 0)
                                                                                            {
                                                                                                //Error table record
                                                                                                AddErrorData(Import_id,RequestId, xmlFileName, clientNode.Name.ToUpper(), MissingClientAttributes, "Missing", UniqueAttr, UniqueAttrValue);
                                                                                            }
                                                                                        }

                                                                                        List<string> NewClientAttributes = FindNewClientAttr(attributes);
                                                                                        if (NewClientAttributes != null)
                                                                                        {
                                                                                            if (NewClientAttributes.Count > 0)
                                                                                            {
                                                                                                //Error table record
                                                                                                AddErrorData(Import_id,RequestId, xmlFileName, clientNode.Name.ToUpper(), NewClientAttributes, "New", UniqueAttr, UniqueAttrValue);
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                    HotelXml += "</HOTELS></ROOT>";
                                                                    HotelClientXml += "</CLIENTS></ROOT>";
                                                                }
                                                                else
                                                                {
                                                                    //Error table record
                                                                }
                                                            }
                                                            //Tanvir
                                                            if (xmlreservationChildNode.Name.ToUpper() == "SERVICES")
                                                            {
                                                                XmlNodeList serviceNodeList = xmlreservationChildNode.ChildNodes;

                                                                if (serviceNodeList.Count > 0)
                                                                {
                                                                    ServiceXml = "<ROOT><SERVICES>" + Environment.NewLine;
                                                                    ServiceClientXml = "<ROOT><CLIENTS>" + Environment.NewLine;
                                                                    foreach (XmlNode serviceNode in serviceNodeList)
                                                                    {
                                                                        UniqueAttr = "";
                                                                        UniqueAttrValue = "";
                                                                        if (serviceNode.Attributes != null)
                                                                        {
                                                                            attributes = new List<string>();
                                                                            ServiceXml += "<SERVICE>" + Environment.NewLine;
                                                                            ServiceXml += "<NUMBER>" + RequestId + "</NUMBER>" + Environment.NewLine;
                                                                            foreach (XmlAttribute attr in serviceNode.Attributes)
                                                                            {
                                                                                attributes.Add(attr.Name.ToString());

                                                                                if (attr.Name.ToString() == "ID")
                                                                                {
                                                                                    UniqueAttr = attr.Name.ToString();
                                                                                    UniqueAttrValue = attr.Value;
                                                                                    ServiceId = attr.Value;
                                                                                }

                                                                                ServiceXml += "<" + attr.Name.ToString() + ">" + attr.Value.ToString() + "</" + attr.Name.ToString() + ">" + Environment.NewLine;
                                                                            }
                                                                            ServiceXml += "</SERVICE>" + Environment.NewLine;
                                                                            List<string> MissingServiceAttributes = FindMissingServiceAttr(attributes);
                                                                            if (MissingServiceAttributes != null)
                                                                            {
                                                                                if (MissingServiceAttributes.Count > 0)
                                                                                {
                                                                                    //Error table record
                                                                                    AddErrorData(Import_id,RequestId, xmlFileName, serviceNode.Name.ToUpper(), MissingServiceAttributes, "Missing", UniqueAttr, UniqueAttrValue);
                                                                                }
                                                                            }

                                                                            List<string> NewServiceAttributes = FindNewServiceAttr(attributes);
                                                                            if (NewServiceAttributes != null)
                                                                            {
                                                                                if (NewServiceAttributes.Count > 0)
                                                                                {
                                                                                    //Error table record
                                                                                    AddErrorData(Import_id,RequestId, xmlFileName, serviceNode.Name.ToUpper(), NewServiceAttributes, "New", UniqueAttr, UniqueAttrValue);
                                                                                }
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            //Error table record
                                                                        }

                                                                        UniqueAttr = "";
                                                                        UniqueAttrValue = "";

                                                                        XmlNodeList clientsNodeList = serviceNode.ChildNodes; //.SelectNodes("//CLIENTS");

                                                                        if (clientsNodeList.Count > 0)
                                                                        {
                                                                            foreach (XmlNode clientsNode in clientsNodeList)
                                                                            {
                                                                                XmlNodeList clientNodeList = clientsNode.ChildNodes;

                                                                                foreach (XmlNode clientNode in clientNodeList)
                                                                                {
                                                                                    ServiceClientXml += "<CLIENT>" + Environment.NewLine;
                                                                                    ServiceClientXml += "<NUMBER>" + RequestId + "</NUMBER>" + Environment.NewLine;
                                                                                    ServiceClientXml += "<SERVICEID>" + ServiceId + "</SERVICEID>" + Environment.NewLine;
                                                                                    if (clientNode.Attributes != null)
                                                                                    {
                                                                                        attributes = new List<string>();
                                                                                        foreach (XmlAttribute attr in clientNode.Attributes)
                                                                                        {
                                                                                            attributes.Add(attr.Name.ToString());
                                                                                            if (attr.Name.ToString() == "ID")
                                                                                            {
                                                                                                UniqueAttr = attr.Name.ToString();
                                                                                                UniqueAttrValue = attr.Value;
                                                                                            }
                                                                                            ServiceClientXml += "<" + attr.Name.ToString() + ">" + attr.Value.ToString() + "</" + attr.Name.ToString() + ">" + Environment.NewLine;
                                                                                        }
                                                                                        ServiceClientXml += "</CLIENT>" + Environment.NewLine;
                                                                                        List<string> MissingServiceClientAttributes = FindMissingServiceClientAttr(attributes);
                                                                                        if (MissingServiceClientAttributes != null)
                                                                                        {
                                                                                            if (MissingServiceClientAttributes.Count > 0)
                                                                                            {
                                                                                                //Error table record
                                                                                                AddErrorData(Import_id,RequestId, xmlFileName, clientNode.Name.ToUpper(), MissingServiceClientAttributes, "Missing", UniqueAttr, UniqueAttrValue);
                                                                                            }
                                                                                        }

                                                                                        List<string> NewServiceClientAttributes = FindNewServiceClientAttr(attributes);
                                                                                        if (NewServiceClientAttributes != null)
                                                                                        {
                                                                                            if (NewServiceClientAttributes.Count > 0)
                                                                                            {
                                                                                                //Error table record
                                                                                                AddErrorData(Import_id,RequestId, xmlFileName, clientNode.Name.ToUpper(), NewServiceClientAttributes, "New", UniqueAttr, UniqueAttrValue);
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                    ServiceXml += "</SERVICES></ROOT>";
                                                                    ServiceClientXml += "</CLIENTS></ROOT>" + Environment.NewLine;
                                                                }
                                                            }
                                                            //Tanvir added transports
                                                            if (xmlreservationChildNode.Name.ToUpper() == "TRANSPORTS")
                                                            {
                                                                XmlNodeList serviceNodeList = xmlreservationChildNode.ChildNodes;

                                                                if (serviceNodeList.Count > 0)
                                                                {
                                                                   TransportXml = "<ROOT><TRANSPORTS>" + Environment.NewLine;
                                                                    TransportClientXml = "<ROOT><CLIENTS>" + Environment.NewLine;
                                                                    foreach (XmlNode serviceNode in serviceNodeList)
                                                                    {
                                                                        UniqueAttr = "";
                                                                        UniqueAttrValue = "";
                                                                        if (serviceNode.Attributes != null)
                                                                        {
                                                                            attributes = new List<string>();
                                                                            TransportXml += "<TRANSPORT>" + Environment.NewLine;
                                                                            TransportXml += "<NUMBER>" + RequestId + "</NUMBER>" + Environment.NewLine;
                                                                            foreach (XmlAttribute attr in serviceNode.Attributes)
                                                                            {
                                                                                attributes.Add(attr.Name.ToString());

                                                                                if (attr.Name.ToString() == "ID")
                                                                                {
                                                                                    UniqueAttr = attr.Name.ToString();
                                                                                    UniqueAttrValue = attr.Value;
                                                                                    TransportId = attr.Value;
                                                                                }

                                                                                TransportXml += "<" + attr.Name.ToString() + ">" + attr.Value.ToString() + "</" + attr.Name.ToString() + ">" + Environment.NewLine;
                                                                            }
                                                                            TransportXml += "</TRANSPORT>" + Environment.NewLine;
                                                                            List<string> MissingTransportAttributes = FindMissingTransportAttr(attributes);
                                                                            if (MissingTransportAttributes != null)
                                                                            {
                                                                                if (MissingTransportAttributes.Count > 0)
                                                                                {
                                                                                    //Error table record
                                                                                    AddErrorData(Import_id,RequestId, xmlFileName, serviceNode.Name.ToUpper(), MissingTransportAttributes, "Missing", UniqueAttr, UniqueAttrValue);
                                                                                }
                                                                            }

                                                                            List<string> NewTransportAttributes = FindNewTransportAttr(attributes);
                                                                            if (NewTransportAttributes != null)
                                                                            {
                                                                                if (NewTransportAttributes.Count > 0)
                                                                                {
                                                                                    //Error table record
                                                                                    AddErrorData(Import_id,RequestId, xmlFileName, serviceNode.Name.ToUpper(), NewTransportAttributes, "New", UniqueAttr, UniqueAttrValue);
                                                                                }
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            //Error table record
                                                                        }

                                                                        UniqueAttr = "";
                                                                        UniqueAttrValue = "";

                                                                        XmlNodeList clientsNodeList = serviceNode.ChildNodes; //.SelectNodes("//CLIENTS");

                                                                        if (clientsNodeList.Count > 0)
                                                                        {
                                                                            foreach (XmlNode clientsNode in clientsNodeList)
                                                                            {
                                                                                XmlNodeList clientNodeList = clientsNode.ChildNodes;

                                                                                foreach (XmlNode clientNode in clientNodeList)
                                                                                {
                                                                                    TransportClientXml += "<CLIENT>" + Environment.NewLine;
                                                                                    TransportClientXml += "<NUMBER>" + RequestId + "</NUMBER>" + Environment.NewLine;
                                                                                    TransportClientXml += "<TRANSPORTID>" + TransportId + "</TRANSPORTID>" + Environment.NewLine;
                                                                                    if (clientNode.Attributes != null)
                                                                                    {
                                                                                        attributes = new List<string>();
                                                                                        foreach (XmlAttribute attr in clientNode.Attributes)
                                                                                        {
                                                                                            attributes.Add(attr.Name.ToString());
                                                                                            if (attr.Name.ToString() == "ID")
                                                                                            {
                                                                                                UniqueAttr = attr.Name.ToString();
                                                                                                UniqueAttrValue = attr.Value;
                                                                                            }
                                                                                            TransportClientXml += "<" + attr.Name.ToString() + ">" + attr.Value.ToString() + "</" + attr.Name.ToString() + ">" + Environment.NewLine;
                                                                                        }
                                                                                        TransportClientXml += "</CLIENT>" + Environment.NewLine;
                                                                                        List<string> MissingTransportClientAttributes = FindMissingTransportClientAttr(attributes);
                                                                                        if (MissingTransportClientAttributes != null)
                                                                                        {
                                                                                            if (MissingTransportClientAttributes.Count > 0)
                                                                                            {
                                                                                                //Error table record
                                                                                                AddErrorData(Import_id,RequestId, xmlFileName, clientNode.Name.ToUpper(), MissingTransportClientAttributes, "Missing", UniqueAttr, UniqueAttrValue);
                                                                                            }
                                                                                        }

                                                                                        List<string> NewTransportClientAttributes = FindNewTransportClientAttr(attributes);
                                                                                        if (NewTransportClientAttributes != null)
                                                                                        {
                                                                                            if (NewTransportClientAttributes.Count > 0)
                                                                                            {
                                                                                                //Error table record
                                                                                                AddErrorData(Import_id,RequestId, xmlFileName, clientNode.Name.ToUpper(), NewTransportClientAttributes, "New", UniqueAttr, UniqueAttrValue);
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                    TransportXml += "</TRANSPORTS></ROOT>";
                                                                    TransportClientXml += "</CLIENTS></ROOT>" + Environment.NewLine;
                                                                }
                                                            }



                                                        }

                                                    }
                                               
                                                    if (dtErrorRecord != null && dtErrorRecord.Rows.Count > 0)
                                                    {

                                                        ////  DataSet result = SaveErrorData(dtErrorRecord, xmlFile);
                                                        //  foreach (DataRow row in result.Tables[0].Rows)
                                                        //  {
                                                        //      // Assuming "request" is the column name
                                                        //      string RequestId1 = row["requestid"].ToString();
                                                        //      //  if (result == 0)
                                                        //      if (row["errormsg"].ToString() == "0")
                                                        //      {
                                                        //          SaveXmlData(Import_id, RequestId1, HeaderXml, GuestXml, HotelXml, HotelClientXml, ServiceXml, ServiceClientXml, TransportXml, TransportClientXml, xmlFile);
                                                        //      }
                                                        //  }
                                                        
                                                        int result = SaveErrorData(dtErrorRecord, xmlFile);
                                                        if (result == 0)

                                                        {
                                                            SaveXmlData(Import_id, RequestId, HeaderXml, GuestXml, HotelXml, HotelClientXml, ServiceXml, ServiceClientXml, TransportXml, TransportClientXml, xmlFile);

                                                                                                                 }
                                                        else
                                                        {
                                                            ERRORCNT++;
                                                        }




                                                    }
                                                    if (CURRENTNODE == lastReservationNode)
                                                    {
                                                    if( ERRORCNT >0)  
                                                                {
                                                            WriteLog("[" + DateTime.Now.ToString("HH:mm:ss") + "]" + "ValidateXMLDocument", "Booking moved to  : "+ xmlFile);
                                                            MoveXmlFiles(xmlFile, "Error");
                                                                                                                    }
                                                    else
                                                        {
                                                            WriteLog("[" + DateTime.Now.ToString("HH:mm:ss") + "]" + "ValidateXMLDocument", "Booking moved to  : " + xmlFile);
                                                            MoveXmlFiles(xmlFile, "Success");
                                                        }

                                                    }


                                                }
                                                }





                                        }
                                    }
                                    else
                                    {
                                        //Error table record
                                    }
                                }
                                else
                                {
                                    //Error table record
                                }
                            }
                       
                            ////            if (dtErrorRecord != null && dtErrorRecord.Rows.Count > 0  )
                            ////{
                                 
                            ////    DataSet  result  = SaveErrorData(dtErrorRecord, xmlFile);
                            ////    foreach (DataRow row in result.Tables[0].Rows )
                            ////    {
                            ////        // Assuming "request" is the column name
                            ////        string RequestId1 = row["requestid"].ToString();
                            ////        //if (result == 0)
                                   
                            ////        SaveXmlData(RequestId1, HeaderXml, GuestXml, HotelXml, HotelClientXml, ServiceXml, ServiceClientXml,TransportXml,TransportClientXml , xmlFile);
                            ////    }
                              
                            ////}
                                        //Tanvir
                            //else
                            //{
                            //    SaveXmlData(RequestId, HeaderXml, GuestXml, HotelXml, HotelClientXml, ServiceXml, ServiceClientXml, xmlFile);
                            //}
                        }
                    }
                }
                
            }
            catch (Exception ex)
            {
                WriteLog(ex, "ValidateXMLDocument");
                //   log.Error(ex);
                //(new clsXMLFileReader()).Thread_Stop();
                //(new clsXMLFileReader()).Thread_Start();
            }
        }

        private void AddErrorData(string Import_id,string requestId, string xmlFileName, string Element, List<string> attributes, string Type, string uniqueAttr, string uniqueAttrValue)
        {
            try
            {
                if(attributes != null)
                {
                    if(attributes.Count > 0)
                    {
                        for (int i = 0; i < attributes.Count; i++)
                        {
                            DataRow dataRow = dtErrorRecord.NewRow();
                            dataRow["Import_id"] = Import_id;//Tanvir
                            dataRow["BookingCode"] = requestId;
                            dataRow["XmlFileName"] = xmlFileName;
                            dataRow["Element"] = Element;
                            dataRow["Attribute"] = attributes[i].ToString();

                            if (Type == "Missing")
                            {
                                dataRow["ErrorDesc"] = string.Concat("Attribute ", attributes[i].ToString(), " is missing in ", Element, " element ");
                            }
                            else if (Type == "New")
                            {
                                dataRow["ErrorDesc"] = string.Concat("New Attribute ", attributes[i].ToString(), " found in ", Element, " element ");
                            }
                            else if (Type == "Mandatory")
                            {
                                dataRow["ErrorDesc"] = string.Concat("Attribute ", attributes[i].ToString(), " is mandatory in ", Element, " element ");
                            }
                            else
                            {
                                dataRow["ErrorDesc"] = "";
                            }
                            dataRow["Type"] = Type;
                            dataRow["UniqueAttribute"] = uniqueAttr;
                            dataRow["UniqueAttributeValue"] = uniqueAttrValue;

                            dtErrorRecord.Rows.Add(dataRow);
                        }
                    }
                    else
                    {
                        goto ElementMissingIns;
                    }
                }
                else
                {
                    goto ElementMissingIns;
                }

                ElementMissingIns:
                DataRow dr = dtErrorRecord.NewRow();
                dr["BookingCode"] = requestId;
                dr["XmlFileName"] = xmlFileName;
                dr["Element"] = Element;
                dr["Attribute"] = "";
                if (Type == "Missing")
                {
                    dr["ErrorDesc"] = string.Concat("Element ", Element, " is Missing");
                }
                else if (Type == "Mandatory")
                {
                    dr["ErrorDesc"] = string.Concat("Element ", Element, " is Mandatory");
                }
                else
                {
                    dr["ErrorDesc"] = "";
                }
                dr["Type"] = Type;
                dr["UniqueAttribute"] = uniqueAttr;
                dr["UniqueAttributeValue"] = uniqueAttrValue;
                dtErrorRecord.Rows.Add(dr);

            }
            catch(Exception ex)
            {
                WriteLog(ex, "ValidateXMLDocument");
            }
        }

        private String  GetImportId()
        {
            String import_id;
            DBHandler dbHandler = null;
            dbHandler = new DBHandler();
            import_id = dbHandler.GetAutoDocNo("MAPBOOKING");
            return import_id;

        }
        //private DataSet SaveErrorData(DataTable dtErrorRecord, string xmlFile)
        //{
        //    DataSet dataSet = null;
        //    int ErrId = 0;
        //    try
        //    {

        //     //   DataSet dataSet = new DataSet();
        //        string xmlFilePath = "", xmlFileName = "";
        //        xmlFilePath = Path.GetDirectoryName(xmlFile);
        //        xmlFileName = Path.GetFileName(xmlFile);
        //        DBHandler dbHandler = null;
        //        dbHandler = new DBHandler();
        //        dataSet = dbHandler.SaveXmlErrorData(dtErrorRecord, xmlFilePath, xmlFileName);


        //        if (dataSet != null)
        //        {
        //            if (dataSet.Tables[1].Rows.Count > 0)
        //            {
        //                  ErrId = Convert.ToInt32(dataSet.Tables[1].Rows[0]["ErrorId"]);
        //                //string ErrMsg = dataSet.Tables[0].Rows[0]["ErrorMessage"].ToString();
        //                string ErrMsg = dataSet.Tables[1].Rows[0]["ErrorMessage"].ToString();
        //                if (ErrId == 1)
        //                {
        //                    WriteLog("[" + DateTime.Now.ToString("HH:mm:ss") + "]" + "SaveErrorData", "Booking Cannot be saved in Booking Tables : ");
        //                    MoveXmlFiles(xmlFile, "Error");
        //                }

        //            }


        //                // return ErrId;
        //                return dataSet;
        //                // MoveXmlFiles(xmlFile, "Error");

        //        }
        //    }
        //    catch(Exception ex)
        //    {

        //    }
        //    return dataSet;
        //    //return dataSet;
        //}
        private int SaveErrorData(DataTable dtErrorRecord, string xmlFile)
        {
            int ErrId = 0;
            try
            {
                DataSet dataSet = new DataSet();
                string xmlFilePath = "", xmlFileName = "";
                xmlFilePath = Path.GetDirectoryName(xmlFile);
                xmlFileName = Path.GetFileName(xmlFile);
                DBHandler dbHandler = null;
                dbHandler = new DBHandler();
                dataSet = dbHandler.SaveXmlErrorData(dtErrorRecord, xmlFilePath, xmlFileName);
                if (dataSet != null)
                {
                    if (dataSet.Tables[0].Rows.Count > 0)
                    {
                          ErrId = Convert.ToInt32(dataSet.Tables[0].Rows[0]["ErrorId"]);
                        string ErrMsg = dataSet.Tables[0].Rows[0]["ErrorMessage"].ToString();
                        if (ErrId == 1)
                        {
                            WriteLog("[" + DateTime.Now.ToString("HH:mm:ss") + "]" + "SaveErrorData", "Booking Cannot be saved in Booking Tables : ");
                            MoveXmlFiles(xmlFile, "Error");
                        }
                        else
                        {
                          //  MoveXmlFiles(xmlFile, "Error");
                        }
                        return ErrId;
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLog(ex, "SaveErrorData");
            }
            return ErrId;


        }
        private void SaveXmlData(string Import_id,string RequestId,string HeaderXml,string GuestXml, string HotelXml,string HotelClientXml,string ServiceXml, string ServiceClientXml, string TransportXml, string TransportClientXml, string xmlFile)
        {
            try
            {
                DataSet dataSet = new DataSet();
                DBHandler dbHandler = null;
                dbHandler = new DBHandler();

                string xmlFilePath = "", xmlFileName = "";
                xmlFilePath = Path.GetDirectoryName(xmlFile);
                xmlFileName = Path.GetFileName(xmlFile);

                HeaderXml = HeaderXml.Replace("&", "&amp;");
                GuestXml = GuestXml.Replace("&", "&amp;");
                HotelXml = HotelXml.Replace("&", "&amp;");
                HotelClientXml = HotelClientXml.Replace("&", "&amp;");
                ServiceXml = ServiceXml.Replace("&", "&amp;");
                ServiceClientXml = ServiceClientXml.Replace("&", "&amp;");
                //Tanvir

                TransportXml = TransportXml.Replace("&", "&amp;");
                TransportClientXml = TransportClientXml.Replace("&", "&amp;");

                if (GuestXml.ToString().Trim() == "")
                {
                    GuestXml = "<ROOT><GUESTS>" + Environment.NewLine;
                    GuestXml += "</GUESTS></ROOT>";
                }

                if (HotelXml.ToString().Trim() == "")
                {
                    HotelXml = "<ROOT><HOTELS>" + Environment.NewLine;
                    HotelXml += "</HOTELS></ROOT>";
                }

                if (HotelClientXml.ToString().Trim() == "")
                {
                    HotelClientXml = "<ROOT><CLIENTS>" + Environment.NewLine;
                    HotelClientXml += "</CLIENTS></ROOT>";
                }

                if (ServiceXml.ToString().Trim() == "")
                {
                    ServiceXml = "<ROOT><SERVICES>" + Environment.NewLine;
                    ServiceXml += "</SERVICES></ROOT>";
                }

                if (ServiceClientXml.ToString().Trim() == "")
                {
                    ServiceClientXml = "<ROOT><CLIENTS>" + Environment.NewLine;
                    ServiceClientXml += "</CLIENTS></ROOT>";
                }
                //Tanvir
                if (TransportXml.ToString().Trim() == "")
                {
                    TransportXml = "<ROOT><TRANSPORTS>" + Environment.NewLine;
                    TransportXml += "</TRANSPORTS></ROOT>";
                }

                if (TransportClientXml.ToString().Trim() == "")
                {
                    TransportClientXml = "<ROOT><CLIENTS>" + Environment.NewLine;
                    TransportClientXml += "</CLIENTS></ROOT>";
                }

                dataSet = dbHandler.SaveXmlSuccessData(Import_id,RequestId, HeaderXml, GuestXml, HotelXml, HotelClientXml, ServiceXml, ServiceClientXml, TransportXml, TransportClientXml, xmlFilePath, xmlFileName);

                if(dataSet != null) 
                {
                    if (dataSet.Tables[0].Rows.Count > 0)
                    {
                        int ErrId = Convert.ToInt32(dataSet.Tables[0].Rows[0]["ErrorId"]);
                        string ErrMsg = dataSet.Tables[0].Rows[0]["ErrorMessage"].ToString();
                        if (ErrId == 1) 
                        {
                            WriteLog("[" + DateTime.Now.ToString("HH:mm:ss") + "]" + " SaveXMLDAtA", "Booking Imported : " + RequestId);
                            // MoveXmlFiles(xmlFile, "Success");
                        }
                        else
                        {
                            MoveXmlFiles(xmlFile, "Error");
                        }
                    }
                    
                }
            }
            catch (Exception ex)
            {
                WriteLog(ex, "SaveXmlData");
            }
        }

        private void MoveXmlFiles(string xmlFile, string Type)
        {
            try
            {
                string sourceFilePath = Path.GetDirectoryName(xmlFile);
                // string FilePath = (Type == "Error" || Type == "NoData") ? ErrorxmlFilePath : SuccessxmlFilePath;
                string FilePath;
                if (Type == "Error")
                {
                    FilePath = ErrorxmlFilePath;
                }
                else if (Type == "NoData")
                {
                    FilePath = NoDataxmlFilePath;
                }
                else
                {
                    FilePath = SuccessxmlFilePath;
                }

                if (!Directory.Exists(FilePath))
                { 
                    Directory.CreateDirectory(FilePath);
                }

                // Construct the destination file path
                string destinationFilePath = Path.Combine(FilePath, Path.GetFileName(xmlFile));

                if(File.Exists(destinationFilePath))
                {
                    File.Delete(destinationFilePath);
                }

                File.Move(xmlFile, destinationFilePath);

                WriteLog("[" + DateTime.Now.ToString("HH:mm:ss") + "]" + "MoveXMLFiles", "XML response Moved to Folder : " +FilePath );
            }
            catch( Exception ex )
            {
                WriteLog(ex, "MoveXMLFiles");
            }
        }

        private List<string> FindNewReservationAttr(List<string> attributes)
        {
            return attributes
                .Except(Enum.GetNames(typeof(ReservationAttributes)))
                .ToList();
        }

        private List<string> FindMissingReservationAttr(List<string> inputAttr)
        {
            return Enum.GetNames(typeof(ReservationAttributes))
                .Except(inputAttr)
                .ToList();
        }

        private List<string> FindMissingMandatoryReservationAttr(List<string> inputAttr)
        {
            return Enum.GetNames(typeof(MandatoryReservationAttributes))
                .Except(inputAttr)
                .ToList();
        }

        private List<string> FindNewPeopleAttr(List<string> attributes)
        {
            return attributes
                .Except(Enum.GetNames(typeof(PeopleAttributes)))
                .ToList();
        }

        private List<string> FindMissingPeopleAttr(List<string> inputAttr)
        {
            return Enum.GetNames(typeof(PeopleAttributes))
                .Except(inputAttr)
                .ToList();
        }

        private List<string> FindNewHotelAttr(List<string> attributes)
        {
            return attributes
                .Except(Enum.GetNames(typeof(HotelAttributes)))
                .ToList();
        }

        private List<string> FindMissingHotelAttr(List<string> inputAttr)
        {
            return Enum.GetNames(typeof(HotelAttributes))
                .Except(inputAttr)
                .ToList();
        }

        private List<string> FindNewClientAttr(List<string> attributes)
        {
            return attributes
                .Except(Enum.GetNames(typeof(HotelClientAttributes)))
                .ToList();
        }

        private List<string> FindMissingClientAttr(List<string> inputAttr)
        {
            return Enum.GetNames(typeof(HotelClientAttributes))
                .Except(inputAttr)
                .ToList();
        }

        private List<string> FindNewServiceAttr(List<string> attributes)
        {
            return attributes
                .Except(Enum.GetNames(typeof(ServiceAttributes)))
                .ToList();
        }
        //Tanvir
        private List<string> FindNewTransportAttr(List<string> attributes)
        {
            return attributes
                .Except(Enum.GetNames(typeof(TransportAttributes)))
                .ToList();
        }
        private List<string> FindMissingServiceAttr(List<string> inputAttr)
        {
            return Enum.GetNames(typeof(ServiceAttributes))
                .Except(inputAttr)
                .ToList();
        }
        //Tanvir
        private List<string> FindMissingTransportAttr(List<string> inputAttr)
        {
            return Enum.GetNames(typeof(TransportAttributes))
                .Except(inputAttr)
                .ToList();
        }


        private List<string> FindNewServiceClientAttr(List<string> attributes)
        {
            return attributes
                .Except(Enum.GetNames(typeof(ServiceClientAttributes)))
                .ToList();
        }
        private List<string> FindNewTransportClientAttr(List<string> attributes)
        {
            return attributes
                .Except(Enum.GetNames(typeof(TransportClientAttributes)))
                .ToList();
        }
        private List<string> FindMissingServiceClientAttr(List<string> inputAttr)
        {
            return Enum.GetNames(typeof(ServiceClientAttributes))
                .Except(inputAttr)
                .ToList();
        }
        //Tanvir
        private List<string> FindMissingTransportClientAttr(List<string> inputAttr)
        {
            return Enum.GetNames(typeof(TransportClientAttributes))
                .Except(inputAttr)
                .ToList();
        }

        private void findChildNodes(XmlNode parentNode)
        {
            XmlNodeList childNodes = parentNode.ChildNodes;
            foreach (XmlNode childNode in childNodes)
            {
                if (childNode.NodeType == XmlNodeType.Element)
                {
                    Console.WriteLine("  Child Node: " + childNode.Name + " - " + childNode.InnerText);
                    findChildNodes((XmlNode)childNode);
                }
            }
        }
    }
}
