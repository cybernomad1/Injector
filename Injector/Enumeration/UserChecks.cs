using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Principal;
using System.Management;
using System.Runtime.InteropServices;
namespace Injector.Enumeration
{
    class UserChecks
    {
        //https://stackoverflow.com/questions/33935825/pinvoke-netlocalgroupgetmembers-runs-into-fatalexecutionengineerror/33939889#33939889
        public static class NetworkAPI
        {
            // from boboes' code at https://stackoverflow.com/questions/33935825/pinvoke-netlocalgroupgetmembers-runs-into-fatalexecutionengineerror/33939889#33939889

            [DllImport("Netapi32.dll")]
            public extern static uint NetLocalGroupGetMembers([MarshalAs(UnmanagedType.LPWStr)] string servername, [MarshalAs(UnmanagedType.LPWStr)] string localgroupname, int level, out IntPtr bufptr, int prefmaxlen, out int entriesread, out int totalentries, out IntPtr resumehandle);

            [DllImport("Netapi32.dll")]
            public extern static int NetApiBufferFree(IntPtr Buffer);

            // LOCALGROUP_MEMBERS_INFO_2 - Structure for holding members details
            [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
            public struct LOCALGROUP_MEMBERS_INFO_2
            {
                public IntPtr lgrmi2_sid;
                public int lgrmi2_sidusage;
                public string lgrmi2_domainandname;
            }

            // documented in MSDN
            public const uint ERROR_ACCESS_DENIED = 0x0000005;
            public const uint ERROR_MORE_DATA = 0x00000EA;
            public const uint ERROR_NO_SUCH_ALIAS = 0x0000560;
            public const uint NERR_InvalidComputer = 0x000092F;

            // found by testing
            public const uint NERR_GroupNotFound = 0x00008AC;
            public const uint SERVER_UNAVAILABLE = 0x0006BA;
        }

        public static string[] GetLocalGroupMembers()
        {
            // returns the "DOMAIN\user" members for a specified local group name
            // adapted from boboes' code at https://stackoverflow.com/questions/33935825/pinvoke-netlocalgroupgetmembers-runs-into-fatalexecutionengineerror/33939889#33939889

            string computerName = null; // null for the local machine
            string groupName = "Administrators";
            int EntriesRead;
            int TotalEntries;
            IntPtr Resume;
            IntPtr bufPtr;

            uint retVal = NetworkAPI.NetLocalGroupGetMembers(computerName, groupName, 2, out bufPtr, -1, out EntriesRead, out TotalEntries, out Resume);

            if (retVal != 0)
            {
                if (retVal == NetworkAPI.ERROR_ACCESS_DENIED) { Console.WriteLine("Access denied"); return null; }
                if (retVal == NetworkAPI.ERROR_MORE_DATA) { Console.WriteLine("ERROR_MORE_DATA"); return null; }
                if (retVal == NetworkAPI.ERROR_NO_SUCH_ALIAS) { Console.WriteLine("Group not found"); return null; }
                if (retVal == NetworkAPI.NERR_InvalidComputer) { Console.WriteLine("Invalid computer name"); return null; }
                if (retVal == NetworkAPI.NERR_GroupNotFound) { Console.WriteLine("Group not found"); return null; }
                if (retVal == NetworkAPI.SERVER_UNAVAILABLE) { Console.WriteLine("Server unavailable"); return null; }
                Console.WriteLine("Unexpected NET_API_STATUS: " + retVal.ToString());
                return null;
            }

            if (EntriesRead > 0)
            {
                string[] names = new string[EntriesRead];
                NetworkAPI.LOCALGROUP_MEMBERS_INFO_2[] Members = new NetworkAPI.LOCALGROUP_MEMBERS_INFO_2[EntriesRead];
                IntPtr iter = bufPtr;

                for (int i = 0; i < EntriesRead; i++)
                {
                    Members[i] = (NetworkAPI.LOCALGROUP_MEMBERS_INFO_2)Marshal.PtrToStructure(iter, typeof(NetworkAPI.LOCALGROUP_MEMBERS_INFO_2));

                    //x64 safe
                    iter = new IntPtr(iter.ToInt64() + Marshal.SizeOf(typeof(NetworkAPI.LOCALGROUP_MEMBERS_INFO_2)));

                    names[i] = Members[i].lgrmi2_domainandname;
                }
                NetworkAPI.NetApiBufferFree(bufPtr);

                return names;
            }
            else
            {
                return null;
            }

        }
            
            
        public static bool IsAdministrator()
        {
            string[] LocalAdmins = GetLocalGroupMembers();

            foreach (string LocalAdmin in LocalAdmins)
            {
                if(LocalAdmin == System.Security.Principal.WindowsIdentity.GetCurrent().Name)
                {
                    return true;
                }
            }

            return false;
        } 
    }
}
