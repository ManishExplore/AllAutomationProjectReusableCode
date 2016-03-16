using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReusableCode;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data;

namespace ReusableCode
{
    /// <summary>
    /// Summary description for UnitTest2
    /// </summary>
    /// 

   
     
    [TestClass]
    public class UnitTest2
    {



        string strconn = string.Format("Data Source={0};Initial Catalog=EmployeeInfo.Models.EmployeeInfoDBContext;Integrated Security=SSPI;", "SSALITHA01\\SQLEXPRESS");
           
        //SqlCommand Sqlcmd = new SqlCommand();
        //Sqlcmd.CommandType=CommandType.Text;
        public UnitTest2()
        {
            //
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
         [ClassInitialize()]
         public static void MyClassInitialize(TestContext testContext) 
         {
         
        
         
         }
        
         //Use ClassCleanup to run code after all tests in a class have run
         [ClassCleanup()]
         public static void MyClassCleanup() { }
        
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void ExSqlSelectQuery()
        {
           // int result;
            DataSet ds = new DataSet();
            //string UpdateQuery;
            //UpdateQuery = "update Emp set EmpName='Test14' where EmpId=1";
            ds = SqlUtilities.ExecuteDataset(strconn, "GetEmpName", (1));

            //result = SqlUtilities.ExecuteNonQuery(strconn, CommandType.StoredProcedure, "GetEmpName", new SqlParameter("@Emplid", 1));
      


        }
    }
}
