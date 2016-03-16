
//===============================================================================

// CodedUIUtilities.cs
//
// This file contains the implementations of the ExcelUltilities class
// 
// Author : Sathya Salitha K

//==============================================================================



using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UITesting.HtmlControls;
using Microsoft.VisualStudio.TestTools.UITesting;
using System.Windows.Forms;

namespace ReusableCode
{
   public static class CodedUIUtilities
    {

        # region LaunchApplicationInBrowser
        /// <summary>
        /// Launches the application in the specified browser
        /// </summary>
        /// <param name="BrowserName">Name of the browser</param>
        /// <param name="ApplicationURL">Application URL </param>
        /// <returns></returns>
        
        public static Process LaunchApplicationInBrowser(string BrowserName,string ApplicationURL)
        {

            {
                //Start the process that will launch the browser
                Process p = new Process();

                if (BrowserName == "IE8" || BrowserName == "IE9")
                {
                     p = Process.Start("iexplore.exe",ApplicationURL);

                }

                else if (BrowserName == "FireFox")
                {
                     p = Process.Start("firefox.exe", ApplicationURL);


                }

                else if (BrowserName == "Safari")
                {
                     p = Process.Start("safari.exe", ApplicationURL);

                }

                  else if (BrowserName == "Chrome")
                {
                     p = Process.Start("chrome.exe", ApplicationURL);

                }
               
               return p;
            }

        }


    #endregion

      
        #region Descendants

        /// <summary>
        /// This method is used to fetch the list of descendants of a parent control into a collection
        /// It returns the decendants with the specified tagname
        /// </summary>
        /// <remarks>
        /// e.g.:
        /// List<HtmlControl> Textlst = ReusableCode.CodedUIUtilities.GetDescendantList(tbl, n => n.TagName == "INPUT");
        /// </remarks>
        /// <param name="control">the parent control object</param>
        /// <param name="predicate">the property for which the child controls need to be fetched</param>
        /// <returns>Collection</returns>
        
        public static List<HtmlControl> GetDescendantList(this HtmlControl control, Predicate<HtmlControl> predicate)
        {
            var collection = new List<HtmlControl>();

            foreach (var child in control.GetChildren())
            {
                var htmlChild = (HtmlControl)child;
                if (predicate(htmlChild))
                    collection.Add(htmlChild);

                foreach (var item in htmlChild.GetDescendantList(predicate))
                {
                    collection.Add(item);
                }
            }

            return collection;
        }
        public static List<HtmlControl> GetDescendantList(this HtmlControl control, string TagName)
        {
            return control.GetDescendantList(n => n.TagName.ToLower() == TagName.ToLower());
        }
        #endregion

        #region getSiblings
        /// <summary>
        /// This method is used to fetch the first sibling of a given control
        /// </summary>
        /// <remarks>
        /// e.g.:
        /// HtmlControl TextCtrl = ReusableCode.CodedUIUtilities.GetSibling(FNLabel, n => n.TagName == "INPUT");
        /// </remarks>
        /// <param name="control">the control object for which Sibling is to be fetched</param>
        /// <param name="predicate">the property for which the sibling control need to be fetched</param>
        /// <returns>first sibling of the control</returns>
       
       public static HtmlControl GetSibling(this HtmlControl control, Func<HtmlControl, bool> predicate)
        {
            var searchedSiblings = control.GetParent().GetChildren().Where(n => !n.Equals(control)).Cast<HtmlControl>().Where(predicate);

            return searchedSiblings.Count() == 0 ? null : searchedSiblings.First();
        }
        #endregion

        #region GetFirstDescendant
       /// <summary>
       /// This method is used to fetch the first descendant of a given control
       /// </summary>
       /// <remarks>
       /// e.g.:
       /// HtmlControl TextCtrl = ReusableCode.CodedUIUtilities.GetFirstDescendant(MyTable, n => n.TagName == "INPUT");
       /// </remarks>
       /// <param name="control">the control object for which the first descendant has to fetched</param>
       /// <param name="predicate">the property for which the sibling control need to be fetched</param>
       /// <returns>first descendant of the control</returns>
       
        public static HtmlControl GetFirstDescendant(this HtmlControl control, Predicate<HtmlControl> predicate)
        {
            foreach (var child in control.GetChildren())
            {
                if (predicate((HtmlControl)child))
                    return (HtmlControl)child;
            }

            HtmlControl candidate;
            foreach (var child in control.GetChildren())
            {
                candidate = ((HtmlControl)child).GetFirstDescendant(predicate);
                if (candidate != null)
                    return candidate;
            }

            return null;
        }
        #endregion

        #region Get The First Visible Required Cell
        /// <summary>
        /// This method is used to fetch the first Visible HTML CELL in a given HTML TABLE with provided search properties.
        /// </summary>
        /// <remarks>
        /// e.g.:
        /// HtmlCell testcell = ReusableCode.CodedUIUtilities.IdentifyParticularChildCell(itemtable, "InnerText", "Neudesic"); 
        /// </remarks>
        /// <param name="itemTable">the itemTable object is a HTML TABLE for which the visible HtmlCell has to fetched</param>
        /// <param name="searchProperty">the Property Name that should be used to identify the HtmlCell</param>
        /// <param name="propertyValue">the Property Value that should be possessed by the HtmlCell</param>
        /// <returns>first visible HTML CELL that belongs to the given HTML TABLE with the property specified</returns>
        /// <contributor>Pavan Gundapanthula <Pavan.Gundapanthula@neudesic.com></contributor>
        /// <project>Pulte Homes -QA Automated Test Go Faster</project>

        public static Dictionary<string, string> searchPropertiesDictionary = new Dictionary<string, string>();
        public static  HtmlCell IdentifyParticularChildCell(HtmlTable itemTable, string searchProperty, string propertyValue)
        {
            HtmlCell itemCell = new HtmlCell(itemTable);
            searchPropertiesDictionary.Add(searchProperty, propertyValue);
            foreach (KeyValuePair<string, string> searchProp in searchPropertiesDictionary)
            {
                itemCell.SearchProperties.Add(searchProp.Key, searchProp.Value, PropertyExpressionOperator.EqualTo);
            }

            if (itemCell.Exists)
            {
                foreach (HtmlCell requiredCell in itemCell.FindMatchingControls())
                {
                    if (requiredCell.BoundingRectangle.X > 0 && requiredCell.Height > 0 && requiredCell.Width > 0)
                    {
                        searchPropertiesDictionary.Clear();
                    }
                }
            }
            return itemCell;
        }

        #endregion

        


    }
}
