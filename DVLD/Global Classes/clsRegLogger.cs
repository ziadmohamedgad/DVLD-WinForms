using DVLD_BusinessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using Microsoft.Win32;
using DVLD_Shared;
using System.Diagnostics;
namespace DVLD.Global_Classes
{
    public class clsRegLogger
    {
        public static clsUser CurrentUser;
        private static string _Registry_KeyPath = @"HKEY_CURRENT_USER\SOFTWARE\DVLD";
        private static string _UsernameValueName = "username";
        private static string _PasswordValueName = "password";
        public static bool DeleteSavedCredentials()
        {
            try
            {
                string SubKeyPath = @"SOFTWARE\DVLD";
                using (RegistryKey BaseKey = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64))
                {
                    using(RegistryKey DVLD = BaseKey.OpenSubKey(SubKeyPath, true))
                    {
                        if (DVLD != null)
                        {
                            DVLD.DeleteValue(_UsernameValueName, false);
                            DVLD.DeleteValue(_PasswordValueName, false);
                        }
                    }
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                MessageBox.Show("UnauthorizedAccessException: Run the program with administrative privileges.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                clsEventLogger.SaveLog("Application", $"{ex.Message}: Run the program with administrative privileges.", EventLogEntryType.Error);
                return false;
            }
            return true;
        }
        public static bool RememberUsernameAndPassword(string Username, string Password)
        {
            try
            {
                Registry.SetValue(_Registry_KeyPath, _UsernameValueName, Username, RegistryValueKind.String);
                Registry.SetValue(_Registry_KeyPath, _PasswordValueName, Password, RegistryValueKind.String);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}: failed during remembring username and password!",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                clsEventLogger.SaveLog("Application", $"failed saving username and password to registry.", EventLogEntryType.Error);
                return false;
            }
            return true;
        }
        public static bool GetStoredCredentilas(ref string Username, ref string Password)
        {
            try
            {
                Username = Registry.GetValue(_Registry_KeyPath, _UsernameValueName, null) as string;
                Password = Registry.GetValue(_Registry_KeyPath, _PasswordValueName, null) as string;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occured during getting stored credentails: {ex.Message}");
                clsEventLogger.SaveLog("Application", $"failed fetching username and password from registry.", EventLogEntryType.Error);
                return false;
            }
            if (string.IsNullOrEmpty(Username)) return false;
            return true;
        }
    }
}