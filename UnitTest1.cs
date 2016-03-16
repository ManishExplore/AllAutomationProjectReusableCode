using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;

namespace ReusableCode
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class UnitTest1
    {

       // XMLUitlities xmlUtil = new XMLUitlities();
        Dictionary<string, string> dsConfigData = new Dictionary<string, string>();

        public UnitTest1()
        {
            //


            dsConfigData = XMLUitlities.ReadAllConfigXmlData(@"..\..\ConfigData\ConfigFile.xml", "AutomationSettings");          
            // TODO: Add constructor logic here
           
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext) 
        
        {

        }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        [TestInitialize()]
        public void MyTestInitialize()
        
        {

          
        
        }
        
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void TestMethod1()
        {
           
           //// int s = dsConfigData.Count();
            Dictionary<string, string> dsConfigData1 = new Dictionary<string, string>();
            dsConfigData1 = XMLUitlities.ReadSpecificNodeinConfigXmlData(@"..\..\ConfigData\ConfigFile.xml", "Browser", "PublicUrl");
            int l = dsConfigData1.Count();
            // TODO: Add test logic here
            //
        }


        [TestMethod]
        public void TestMethod2()
        {

            //// int s = dsConfigData.Count();
            //DataTable dsTestData = new DataTable();
            //dsTestData = ExcelUtilities.ConvertExcelToTable(@"..\..\ConfigData\EmpData.xlsx", "Emp");
            //int l = dsTestData.Rows.Count;

            DataSet ds = new DataSet();
            ds = ExcelUtilities.ConvertExcelToDataset(@"..\..\ConfigData\EmpData.xlsx");
            int l = ds.Tables.Count;
            // TODO: Add test logic here
            //
        }
    }
}
