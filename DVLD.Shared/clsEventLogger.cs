using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
namespace DVLD_Shared
{
    public class clsEventLogger
    {
        private static string LogSourceName = "DVLD";
        public static void SaveLog(string Category, string Message, EventLogEntryType LogType)
        {
            if (!EventLog.SourceExists(LogSourceName))
            {
                EventLog.CreateEventSource(LogSourceName, Category);
            }
            EventLog.WriteEntry(LogSourceName, Message, LogType);
        }
    }
}