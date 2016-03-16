

//===============================================================================

//ZipUltilities.cs
//
// This file contains the implementations of the ZipUltilities class
// 
// Author : Sathya Salitha K

//==============================================================================


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Core;
using System.Security.Cryptography;
namespace ReusableCode
{
    public sealed class ZipUtilities
    {


        /// <summary>
        /// The ZipUtilities class is intended to encapsulate high performance, scalable best practices for 
        /// gzip/zip compression.
        /// </summary>
        /// 

        //Since this class provides only static methods, make the default constructor private to prevent 
        //instances from being created with "new ZipUtilities()".
        
        private ZipUtilities() { }

        # region ZipFile
        /// <summary>
        /// This method is used to zip a folder and place the zip file in output path
        /// </summary>
        /// <remarks>
        /// e.g.:
        /// utilities.ZipFile(@"..\..\Zipfilefolder\zippedfile.zip","pwd",@"..\..\folderToZip\")
        /// </remarks>
        /// <param name="outpath">the relative path where the zipped file needs to be placed</param>
        /// <param name="password">password to be used while zipping</param>
        /// <param name="foldername">the folder to be zipped</param>
        /// <returns></returns>
        


        public void ZipFile(string outPathname, string password, string folderName)
        {

            FileStream fsOut = File.Create(outPathname);
            ZipOutputStream zipStream = new ZipOutputStream(fsOut);

            //0-9, 9 being the highest level of compression
            zipStream.SetLevel(3);

            // optional. Null is the same as not setting. Required if using AES.
            zipStream.Password = password; 

            // This setting will strip the leading part of the folder path in the entries, to
            // make the entries relative to the starting folder.
            // To include the full path for each entry up to the drive root, assign folderOffset = 0.
            int folderOffset = folderName.Length + (folderName.EndsWith("\\") ? 0 : 1);

            CompressFolder(folderName, zipStream, folderOffset);

            // Closes the underlying stream
            zipStream.IsStreamOwner = true; 
            zipStream.Close();
        }


        /// <summary>
        /// This method is called within the Zipfile method above and performs the actual action of zipping the file
        /// </summary>
        /// <remarks>
        /// e.g.:
        /// utilities.CompressFolder(@"..\..\folderToZip\",zipstream,folderoffset)
        /// </remarks>
        /// <param name="path">the folder to be zipped</param>
        /// <param name="zipStream">zip stream object to be zipped</param>
        /// <param name="folderoffset">length of the folder</param>
        /// <returns></returns>

        private void CompressFolder(string path, ZipOutputStream zipStream, int folderOffset)
        {

            string[] files = Directory.GetFiles(path);

            foreach (string filename in files)
            {

                FileInfo fi = new FileInfo(filename);

                // Makes the name in zip based on the folder
                string entryName = filename.Substring(folderOffset);

                // Removes drive from name and fixes slash direction
                entryName = ZipEntry.CleanName(entryName); 
                ZipEntry newEntry = new ZipEntry(entryName);

                // Note the zip format stores 2 second granularity
                newEntry.DateTime = fi.LastWriteTime; 

                // Specifying the AESKeySize triggers AES encryption. Allowable values are 0 (off), 128 or 256.
                // A password on the ZipOutputStream is required if using AES.
                //   newEntry.AESKeySize = 256;

                // To permit the zip to be unpacked by built-in extractor in WinXP and Server2003, WinZip 8, Java, and other older code,
                // you need to do one of the following: Specify UseZip64.Off, or set the Size.
                // If the file may be bigger than 4GB, or you do not need WinXP built-in compatibility, you do not need either,
                // but the zip will be in Zip64 format which not all utilities can understand.
                //   zipStream.UseZip64 = UseZip64.Off;
                newEntry.Size = fi.Length;

                zipStream.PutNextEntry(newEntry);

                // Zip the file in buffered chunks
                // the "using" will close the stream even if an exception occurs
                byte[] buffer = new byte[4096];
                using (FileStream streamReader = File.OpenRead(filename))
                {
                    StreamUtils.Copy(streamReader, zipStream, buffer);
                }
                zipStream.CloseEntry();
            }
            string[] folders = Directory.GetDirectories(path);
            foreach (string folder in folders)
            {
                CompressFolder(folder, zipStream, folderOffset);
            }
        }

        #endregion


        #region ExtractZipFile

        /// <summary>
        /// This method is used to extract the zipped file and place the unzipped file in output path
        /// </summary>
        /// <remarks>
        /// e.g.:
        /// utilities.ExtractZipFile(@"..\..\Zipfilefolder\zippedfile.zip","pwd",@"..\..\UnzippedFilefolder\")
        /// </remarks>
        /// <param name="archiveFilenameIn">the relative path where the zipped file is present</param>
        /// <param name="password">password to be used while zipping</param>
        /// <param name="outFolder">the folder to where extracted file needs to be placed</param>
        /// <returns></returns>
        


        public void ExtractZipFile(string archiveFilenameIn, string password, string outFolder)
        {
            ZipFile zf = null;
            try
            {
                FileStream fs = File.OpenRead(archiveFilenameIn);
                zf = new ZipFile(fs);
                if (!String.IsNullOrEmpty(password))
                {
                    // AES encrypted entries are handled automatically
                    zf.Password = password;   
                }
                foreach (ZipEntry zipEntry in zf)
                {
                    if (!zipEntry.IsFile)
                    {
                        // Ignore directories
                        continue;           
                    }
                    String entryFileName = zipEntry.Name;

                    // to remove the folder from the entry:- entryFileName = Path.GetFileName(entryFileName);
                    // Optionally match entrynames against a selection list here to skip as desired.
                    // The unpacked length is available in the zipEntry.Size property.

                    byte[] buffer = new byte[4096];     // 4K is optimum
                    Stream zipStream = zf.GetInputStream(zipEntry);

                    // Manipulate the output filename here as desired.
                    String fullZipToPath = Path.Combine(outFolder, entryFileName);
                    string directoryName = Path.GetDirectoryName(fullZipToPath);
                    if (directoryName.Length > 0)
                    Directory.CreateDirectory(directoryName);

                    // Unzip file in buffered chunks. This is just as fast as unpacking to a buffer the full size
                    // of the file, but does not waste memory.
                    // The "using" will close the stream even if an exception occurs.
                    using (FileStream streamWriter = File.Create(fullZipToPath))
                    {
                        StreamUtils.Copy(zipStream, streamWriter, buffer);
                    }
                }
            }
            finally
            {
                if (zf != null)
                {
                    // Makes close also shut the underlying stream
                    zf.IsStreamOwner = true;
                    // Ensure we release resources
                    zf.Close(); 
                }
            }
        }

        #endregion

    }
}

