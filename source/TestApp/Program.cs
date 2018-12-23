using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Horker.WindowsSearch;

namespace TestApp
{
    class Program
    {
        [DllImport(@"SearchQueryHelper.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private extern static int GetAllProperties(out IntPtr properties);

        static void Main(string[] args)
        {
            var hr = GetAllProperties(out IntPtr p);
            if (hr != 0)
                Marshal.ThrowExceptionForHR(hr);
            var s = Marshal.PtrToStringUni(p);
            Console.WriteLine(s);
            Console.ReadLine();
        }
    }
}
