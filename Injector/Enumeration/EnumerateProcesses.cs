using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Management;

namespace Injector.Enumeration
{
    class EnumerateProcesses
    {

        public static int GetExplorerProcceses(string user)
        {
            Process[] Processes = Process.GetProcessesByName("explorer");
            foreach (Process process in Processes)
            {
               if (GetProcessUser(process.Id) == user)
               {
                    return process.Id;
               }
            }
            return 0;
        }

        public static List<int> GetSystemProcesses()
        {
            Process[] Processes = Process.GetProcesses();
            List<int> SystemList = new List<int>();

            foreach (Process process in Processes)
            {
                if (GetProcessUser(process.Id) == "SYSTEM")
                {
                  SystemList.Add(process.Id);
                }
            }
            return SystemList;
        }
        static string GetProcessUser(int ProcessID)
        {
            string query = "Select * From Win32_Process Where ProcessID = " + ProcessID;
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
            ManagementObjectCollection processList = searcher.Get();

            foreach (ManagementObject obj in processList)
            {
                string[] argList = new string[] { string.Empty, string.Empty };
                int returnVal = Convert.ToInt32(obj.InvokeMethod("GetOwner", argList));
                if (returnVal == 0)
                {
                    // return DOMAIN\user
                    return argList[0];
                }
            }

            return "NO OWNER";
        }
    }
}
