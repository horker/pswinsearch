using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Horker.WindowsSearch;
using System.Runtime.InteropServices;

namespace UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            IntPtr searchQueryHelper;
            var hr = SearchQueryHelper.GetSearchQueryHelper(out searchQueryHelper);
            if (hr != 0)
                Marshal.ThrowExceptionForHR(hr);

            string sql;
            hr = SearchQueryHelper.GenerateSQLFromUserQuery(searchQueryHelper, "document", out sql);
            if (hr != 0)
                Marshal.ThrowExceptionForHR(hr);

            Assert.IsTrue(true);
        }
    }
}
