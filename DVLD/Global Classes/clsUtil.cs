using DVLD_Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Contexts;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Global_Classes
{
    /// <summary>
    /// Provides utility methods for file and folder operations, including dynamic path resolution, file copying, and
    /// GUID-based renaming.
    /// </summary>
    /// <remarks>The <c>clsUtil</c> class contains static methods designed to assist with common file and
    /// folder management tasks, such as dynamically determining folder paths, ensuring folder existence, and copying
    /// files with unique names. These methods are particularly useful in scenarios where the application's deployment
    /// environment may vary, and consistent folder structures are required.</remarks>
    public class clsUtil
    {
        /// <summary>
        /// Generates a new globally unique identifier (GUID) as a string.
        /// </summary>
        /// <returns>A string representation of a newly generated GUID.</returns>
        private static string GenerateGUID()
        {
            // Generate a new GUID
            Guid newGuid = Guid.NewGuid();
            // convert the GUID to a string
            return newGuid.ToString();
        }
        /// <summary>
        /// Ensures that the specified folder exists by creating it if it does not already exist.
        /// </summary>
        /// <remarks>This method checks whether the folder specified by <paramref name="FolderPath"/>
        /// exists. If the folder does not exist, it attempts to create it. If an error occurs during folder creation,
        /// the method logs the error and displays a message to the user.</remarks>
        /// <param name="FolderPath">The full path of the folder to check and create if necessary.</param>
        /// <returns><see langword="true"/> if the folder exists or was successfully created;  otherwise, <see langword="false"/>
        /// if an error occurred during folder creation.</returns>
        private static bool CreateFolderIfDoesNotExist(string FolderPath)
        {
            // Check if the folder exists
            if (!Directory.Exists(FolderPath))
            {
                try
                {
                    // If it doesn't exist, create the folder
                    Directory.CreateDirectory(FolderPath);
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error creating folder: " + ex.Message);
                    clsEventLogger.SaveLog("Application", $"Error creating folder: {ex.Message}",
                        System.Diagnostics.EventLogEntryType.Error);
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// Replaces the file name of the specified file path with a newly generated GUID while preserving the file
        /// extension.
        /// </summary>
        /// <param name="sourceFile">The full path of the source file whose name will be replaced. Must include a valid file extension.</param>
        /// <returns>A string representing the file path with the original file name replaced by a GUID, retaining the original
        /// file extension.</returns>
        private static string ReplaceFileNameWithGUID(string sourceFile)
        {
            // Full file name. Change your file name   
            string fileName = sourceFile;
            FileInfo fi = new FileInfo(fileName);
            string extn = fi.Extension;
            return GenerateGUID() + extn;
        }
        /// <summary>
        /// Dynamically determines the file path to the "DVLD-People-Images" folder based on the application's startup
        /// directory.
        /// </summary>
        /// <remarks>This method calculates the path relative to the application's executable location,
        /// navigating up the directory hierarchy to the project root and then appending the "DVLD-People-Images" folder
        /// name. It is useful for scenarios where the folder structure is consistent but the application may be
        /// deployed in different environments.</remarks>
        /// <returns>The full file path to the "DVLD-People-Images" folder as a string.</returns>
        public static string GetDestinationImagesFolderDynamically()
        {
            // Geting path of running Exe
            string exePath = Application.StartupPath;
            // Go up to project root (usuall two levels : bin/Debug)
            string projectDir = Directory.GetParent(exePath).Parent.FullName; // = destination till DVLD folder
            // Get the parent directory and then build the DVLD-People-Images path
            string DrivingLicenseFileDir = Directory.GetParent(projectDir).FullName; // goes up to Driving License folder
            string imgsDir = Path.Combine(DrivingLicenseFileDir, "DVLD-People-Images");
            return imgsDir;
        }
        /// <summary>
        /// Copies an image file to the project's images folder, renaming it with a unique identifier.
        /// </summary>
        /// <remarks>This method copies the specified image file to a dynamically determined destination
        /// folder within the project. The file is renamed using a GUID while preserving its original extension. If the
        /// destination folder does not exist, it is created automatically. If the operation fails, the method returns
        /// <see langword="false"/> and logs the error.</remarks>
        /// <param name="sourceFile">The path of the source image file to copy. This parameter is updated to reflect the new file path in the
        /// destination folder after the operation completes.</param>
        /// <returns><see langword="true"/> if the image was successfully copied to the project's images folder;  otherwise, <see
        /// langword="false"/>.</returns>
        public static bool CopyImageToProjectImagesFolder(ref string sourceFile)
        {
            // this funciton will copy the image to the
            // project images foldr after renaming it
            // with GUID with the same extention, then it will update the sourceFileName with the new name.
            string DestinationFolder = GetDestinationImagesFolderDynamically();
            if (!CreateFolderIfDoesNotExist(DestinationFolder))
            {
                return false;
            }
            string destinationFile = Path.Combine(DestinationFolder, ReplaceFileNameWithGUID(sourceFile));
            try
            {
                File.Copy(sourceFile, destinationFile, true);
            }
            catch (IOException iox)
            {
                MessageBox.Show(iox.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                clsEventLogger.SaveLog("Application", $"Error copying image to project images folder: {iox.Message}",
                    System.Diagnostics.EventLogEntryType.Error);
                return false;
            }
            sourceFile = destinationFile;
            return true;
        }
    }
}