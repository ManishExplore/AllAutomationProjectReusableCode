
//===============================================================================

// XMLUltilities.cs
//
// This file contains the implementations of the XMLUltilities class
//

//==============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace ReusableCode
{


    /// <summary>
    /// The XMLUtilities class is intended to encapsulate high performance, scalable best practices for 
    /// common uses of XML file data.
    /// </summary>
    public sealed class XMLUitlities
    {
        
        //Since this class provides only static methods, make the default constructor private to prevent 
        //instances from being created with "new XMLUitlities()".
        private XMLUitlities() { }

        #region ReadAllConfigXmlData
        /// <summary>
        /// This method is used to Load Config data from a XML file into a dictionary
        /// It will return the dictionary containing the config data as key,value pairs
        /// </summary>
        /// <remarks>
        /// e.g:
        /// dsConfigData = xmlUtilities.ReadAllConfigXmlData(@"..\..\ConfigData\ConfigFile.xml", "AutomationSettings");          
        /// </remarks>        
        /// <param name="fileName">This denotes the relative path of the XML file</param>
        /// <param name="nodename">This is the node in the config xml under which config Data is present. e.g:AutomationSettings</param>
        /// <returns>Dictionary containing the required config data as a name and value pair</returns>
        

        
        public static Dictionary<String,string> ReadAllConfigXmlData(string fileName, string nodename)
        {
            Dictionary<String, string> ConfigData = new Dictionary<string, string>();
            System.IO.FileInfo fileInfo = new System.IO.FileInfo(fileName);
            
            //check if xml file exists and throw exception if not found
            if (!fileInfo.Exists)
            {
                throw new System.IO.FileNotFoundException(fileName);
            }
            
            XmlDocument doc = new XmlDocument();
            //load the xml config file
            doc.Load(fileName);
            
            //Retrieve the Data from AutomationSettings node
            XmlNode n = doc.SelectSingleNode(nodename);

            //Loop through the nodes in the xml document
            foreach (XmlNode x in n)
            {
               // add the node and the its inner text value into dictionay
                ConfigData.Add(x.Name, x.InnerXml);

            }
            //return dictionary object
            return ConfigData;
        }

        #endregion

        #region ReadSpecificNodeinConfigXmlData

        /// <summary>
        /// This methodLoads Config data from specified nodes of the XML file into a dictionary
        /// It can be used when you want to load only some specific config data and not the entire data
        /// it returns a dictionary object
        /// </summary>
        /// <remarks>
        /// e.g.:
        /// dsConfigData1 = xmlUtil.ReadSpecificNodeinConfigXmlData(@"..\..\ConfigData\ConfigFile.xml", "Browser","PublicUrl");
        /// </remarks>
        /// <param name="fileName">This denotes the path of the XML file </param>
        /// <param name="nodename">string array of nodes seperated by ","</param>
        /// <returns>Dictionary containing the required config data as a name and value pair</returns>

    
        public static Dictionary<String, string> ReadSpecificNodeinConfigXmlData(string fileName, params string[] nodenames)
        {

            Dictionary<String, string> ConfigData = new Dictionary<string, string>();
            System.IO.FileInfo fileInfo = new System.IO.FileInfo(fileName);

            //check for existance of file and throw exception if not found
            if (!fileInfo.Exists)
            {
                throw new System.IO.FileNotFoundException(fileName);
            }
           
            //instantiate an xml reader
            System.Xml.XmlTextReader reader = new System.Xml.XmlTextReader(fileName);

            string element = "";

            //Loop through the nodes in the xml document
            while (reader.Read())
            {
                reader.MoveToContent();
            
                //Check if the node type is element
                if (reader.NodeType == System.Xml.XmlNodeType.Element)
                    
                    //Get the element
                    element = reader.Name;
                
                foreach (string value in nodenames)
                {
                     //Check if the node element matches with  specified  node
                    if (element.Equals(value))
                    {
                        if (reader.Value != "")
                        {
                            // Add the node and the its inner text value into dictionay
                            ConfigData.Add(element, reader.Value);

                        }
                    }

                }

            }
            return ConfigData;
        }
        #endregion

        #region LoadTestCaseData
        /// <summary>
        /// This method is used to Load test data associated with the test case from a XML file
        /// A test case can have mulitple input parameters, this can be provided as Parameter name and value 
        /// Refer to sample xml  below
        /// </summary>
        /// <remarks>
        /// e.g.:
        /// dsTestData=uitilities.LoadTestData(@"..\..\ConfigData\ConfigFile.xml","TestCase_234")
        /// </remarks>
        /// <samplexml>
        ///   <TestCase name="TestCase_234"><Parameter name="Server">hyd2bifmstst2</Parameter>
        ///   <Parameter name="tagKey">SampleSmartTag</Parameter> </TestCase>
        /// </samplexml>
        /// <param name="dataXMLPath">This is the relative path of the XML file path</param>
        /// <returns>Dictionary containing the test case data as key, value pairs</returns>
        
        public static Dictionary<string, string> LoadTestData(string dataXMLPath, string CaseID)
        {
            Dictionary<string, string> paramsTable = new Dictionary<string, string>();
            System.IO.FileInfo fileInfo = new System.IO.FileInfo(dataXMLPath);
        
            //check for file exisitance and throw exception if file not found
            if (!fileInfo.Exists)
            {
                throw new System.IO.FileNotFoundException(dataXMLPath);
            }


            XmlDocument doc = new XmlDocument();
            //Load xml file
            doc.Load(dataXMLPath);
            
            // Retrieve data from "TestCase" nodes into an  xmlnodelist
            XmlNodeList nodes = doc.GetElementsByTagName("TestCase");
            
            //loop through the node list
            foreach (XmlNode n in nodes)
            {
                XmlElement el = (XmlElement)n;
                
                // Find specific CaseInfo by name attribute(e.g.: TestCase name)
                if (el.GetAttribute("name").ToUpper() == CaseID.ToUpper())
                {
                    //loop through each sub node under the test case node
                    foreach (XmlNode sub in el)
                    {
                        UpdateParameters(paramsTable, sub);
                    }
                    // If encounter the node second time, then break
                    break;
                }
            }
            return paramsTable;
        }

        /// <summary>
        /// This method is called by LoadTestData method, it add parameters from XML file into a dictionary
        /// </summary>
        /// <remarks>
        /// e.g.:
        /// UpdateParameters(paramsTable,node)
        /// </remarks>
        /// <param name="paramsTable">Parameters dictionary table</param>
        /// <param name="sub">Node in XML file </param>
        private static void UpdateParameters(Dictionary<string, string> paramsTable, XmlNode sub)
        {
        
            if (sub.Name.Equals("Parameter"))
            {
                XmlElement e = (XmlElement)sub;
            
                //check the dictionary if already has the parameter name as a Key
                if (paramsTable.ContainsKey(e.GetAttribute("name")))
                {
                    //set the value with xmlelements inner text
                    paramsTable[e.GetAttribute("name")] = e.InnerText;
                }
                else
                {
                    //add a key value pair for the name and value
                    paramsTable.Add(e.GetAttribute("name"), e.InnerText);
                }
            }
        }
        #endregion
    }
       
}
