


//===============================================================================

//ExcelUltilities.cs
//
// This file contains the implementations of the ExcelUltilities class
// 
// Author : Sathya Salitha K

//==============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;
using Excel;



namespace ReusableCode
{
    public sealed class ExcelUtilities
    {

           /// <summary>
        /// The ExcelUtilities class is intended to encapsulate high performance, scalable best practices for 
        /// reading excel data.
        /// </summary>
        /// 

        //Since this class provides only static methods, make the default constructor private to prevent 
        //instances from being created with "new ExcelUtilities()".

        private ExcelUtilities() { }


        #region ConvertExcelToTable
        /// <summary>
        /// This method is used to Load test data associated with the test case from an excel file
        /// This method will treat the first row as column names for the data table 
        
        /// </summary>
        /// <remarks>
        /// e.g.:
        /// dsTestData=ExcelUtilities.ConvertExcelToTable(@"..\..\ConfigData\TestData.xlsx","TestData")
        /// </remarks>
        /// <param name="filePath">This is the relative path of the excel file path</param>
        /// <param name="workSheetName">This is the name of worksheet in the excel file path</param>
        /// <returns>Datatable containing the test case data</returns>
        
        public static DataTable ConvertExcelToTable(string filePath, string workSheetName)
        {           
            IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(GetDataStream(filePath));
            excelReader.IsFirstRowAsColumnNames = true;
            DataTable excelTable = excelReader.AsDataSet().Tables[workSheetName];
            return excelTable;
        }

        #endregion
        # region GetDataStream
        /// <summary>
        /// This method is used to read the file specified and return the filestream object
        /// This method is called in the convertExceltoTable method
        /// </summary>
        /// <remarks>
        /// e.g.:
        /// GetDataStream(filePath)
        /// </remarks>
        /// <param name="filePath">This is the relative path of the excel file path</param>
        /// <returns>filestream object containing the data read from the file</returns>
        private static Stream GetDataStream(string fileName)
        {       
            return new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        }

        #endregion

        #region ConvertExcelToDataSet
        /// <summary>
        /// This method is used to Load test data associated with the test case from an excel file into a dataset
        /// This method will treat the first row as column names  
        /// </summary>
        /// <remarks>
        /// e.g.:
        /// dsTestData=ExcelUtilities.ConvertExcelToDataSet(@"..\..\ConfigData\TestData.xlsx")
        /// </remarks>
        /// <param name="filePath">This is the relative path of the excel file path</param>
        /// <returns>Dataset containing the test case data</returns>
        public static DataSet ConvertExcelToDataset(string filePath)
        {            
            IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(GetDataStream(filePath));
            excelReader.IsFirstRowAsColumnNames = true;
            DataSet excelDataSet = excelReader.AsDataSet();
            return excelDataSet;
        }

        #endregion


        #region ConvertExcelColumnToList
        /// <summary>
        /// This method is used to convert the excel columns in a worksheet into a list
        /// This method will treat the first row as column names  
        /// </summary>
        /// <remarks>
        /// e.g.:
        /// dsTestData=ExcelUtilities.ConvertExcelColumnToList(@"..\..\ConfigData\TestData.xlsx","Emp")
        /// </remarks>
        /// <param name="filePath">This is the relative path of the excel file path</param>
        /// <returns>Dataset containing the test case data</returns>
        public List<string> ConvertExcelColumnToList(string filePath, string workSheetName, string excelColumnName)
        {

            List<string> excelColumnData = new List<string>();
            IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(GetDataStream(filePath));
            excelReader.IsFirstRowAsColumnNames = true;
            DataTable excelDataTable = excelReader.AsDataSet().Tables[workSheetName];
            foreach (DataRow dRow in excelDataTable.Rows)
            {
                excelColumnData.Add(dRow[excelColumnName].ToString());
            }
            return excelColumnData;
        }
        #endregion
    }
}














