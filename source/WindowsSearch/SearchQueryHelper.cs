using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Horker.WindowsSearch
{
    public class SearchQueryHelper : IDisposable
    {
        public enum SEARCH_QUERY_SYNTAX
        {
            SEARCH_NO_QUERY_SYNTAX,
            SEARCH_ADVANCED_QUERY_SYNTAX,
            SEARCH_NATURAL_QUERY_SYNTAX,
            NoSyntax = SEARCH_NO_QUERY_SYNTAX,
            Advanced = SEARCH_ADVANCED_QUERY_SYNTAX,
            Natural = SEARCH_NATURAL_QUERY_SYNTAX
        }

        public enum SEARCH_TERM_EXPANSION
        {
            SEARCH_TERM_NO_EXPANSION,
            SEARCH_TERM_PREFIX_ALL,
            SEARCH_TERM_STEM_ALL
        }

        [DllImport(@"SearchQueryHelper.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private extern static int GetSearchQueryHelper(out IntPtr searchQueryHelper);

        [DllImport(@"SearchQueryHelper.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private extern static int ReleaseSearchQueryHelper(IntPtr searchQueryHelper);

        [DllImport(@"SearchQueryHelper.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private extern static int GenerateSQLFromUserQuery(IntPtr searchQueryHelper, string userQuery, out IntPtr sql);

        [DllImport(@"SearchQueryHelper.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private extern static int get_ConnectionString(IntPtr pSearchQueryHelper, out IntPtr pszConnectionString);

        [DllImport(@"SearchQueryHelper.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private extern static int get_QueryContentLocale(IntPtr pSearchQueryHelper, out int pLcid);

        [DllImport(@"SearchQueryHelper.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private extern static int put_QueryContentLocale(IntPtr pSearchQueryHelper, int lcid);

        [DllImport(@"SearchQueryHelper.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private extern static int get_QueryContentProperties(IntPtr pSearchQueryHelper, out IntPtr ppszContentProperties);

        [DllImport(@"SearchQueryHelper.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private extern static int put_QueryContentProperties(IntPtr pSearchQueryHelper, string pszContentProperties);

        [DllImport(@"SearchQueryHelper.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private extern static int get_QueryKeywordLocale(IntPtr pSearchQueryHelper, out int pLcid);

        [DllImport(@"SearchQueryHelper.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private extern static int put_QueryKeywordLocale(IntPtr pSearchQueryHelper, int lcid);

        [DllImport(@"SearchQueryHelper.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private extern static int get_QueryMaxResults(IntPtr pSearchQueryHelper, out int pcMaxResults);

        [DllImport(@"SearchQueryHelper.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private extern static int put_QueryMaxResults(IntPtr pSearchQueryHelper, int cMaxResults);

        [DllImport(@"SearchQueryHelper.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private extern static int get_QuerySelectColumns(IntPtr pSearchQueryHelper, out IntPtr ppszSelectColumns);

        [DllImport(@"SearchQueryHelper.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private extern static int put_QuerySelectColumns(IntPtr pSearchQueryHelper, string pszSelectColumns);

        [DllImport(@"SearchQueryHelper.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private extern static int get_QuerySorting(IntPtr pSearchQueryHelper, out IntPtr ppszSorting);

        [DllImport(@"SearchQueryHelper.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private extern static int put_QuerySorting(IntPtr pSearchQueryHelper, string pszSorting);

        [DllImport(@"SearchQueryHelper.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private extern static int get_QuerySyntax(IntPtr pSearchQueryHelper, out SEARCH_QUERY_SYNTAX pQuerySyntax);

        [DllImport(@"SearchQueryHelper.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private extern static int put_QuerySyntax(IntPtr pSearchQueryHelper, SEARCH_QUERY_SYNTAX querySyntax);

        [DllImport(@"SearchQueryHelper.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private extern static int get_QueryTermExpansion(IntPtr pSearchQueryHelper, out SEARCH_TERM_EXPANSION pExpandTerms);

        [DllImport(@"SearchQueryHelper.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private extern static int put_QueryTermExpansion(IntPtr pSearchQueryHelper, SEARCH_TERM_EXPANSION expandTerms);

        [DllImport(@"SearchQueryHelper.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private extern static int get_QueryWhereRestrictions(IntPtr pSearchQueryHelper, out IntPtr ppszRestrictions);

        [DllImport(@"SearchQueryHelper.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private extern static int put_QueryWhereRestrictions(IntPtr pSearchQueryHelper, string pszRestrictions);

        IntPtr _instance;

        private void ValidateHResult(int hResult)
        {
            if (hResult != 0)
                Marshal.ThrowExceptionForHR(hResult);
        }

        private string GetStringResult(int hResult, IntPtr result)
        {
            ValidateHResult(hResult);
            var s = Marshal.PtrToStringUni(result);
            Marshal.FreeCoTaskMem(result);
            return s;
        }

        /*
        private string PrependPrefixAndJoin(string[] props)
        {
            return String.Join(",", props.Select(x => {
                if (!x.ToLower().StartsWith("system."))
                    return "System." + x;
                return x;
            }));
        }
        */

        public SearchQueryHelper()
        {
            var hr = GetSearchQueryHelper(out _instance);
            ValidateHResult(hr);
        }

        public string GenerateSQLFromUserQuery(string userQuery)
        {
            var hr = GenerateSQLFromUserQuery(_instance, userQuery, out IntPtr sql);
            return GetStringResult(hr, sql);
        }

        public string ConnectionString
        {
            get
            {
                var hr = get_ConnectionString(_instance, out var s);
                return GetStringResult(hr, s);
            }
        }

        public CultureInfo ContentLocale
        {
            get
            {
                var hr = get_QueryContentLocale(_instance, out int lcid);
                ValidateHResult(hr);
                return new CultureInfo(lcid);
            }

            set
            {
                var hr = put_QueryContentLocale(_instance, value.LCID);
                ValidateHResult(hr);
            }
        }

        public string[] ContentProperties
        {
            get
            {
                var hr = get_QueryContentProperties(_instance, out IntPtr columns);
                var s = GetStringResult(hr, columns);
                return s.Split(',');
            }

            set
            {
                var hr = put_QueryContentProperties(_instance, String.Join(",", value));
                ValidateHResult(hr);
            }
        }

        public CultureInfo KeywordLocale
        {
            get
            {
                var hr = get_QueryKeywordLocale(_instance, out int lcid);
                ValidateHResult(hr);
                return new CultureInfo(lcid);
            }

            set
            {
                var hr = put_QueryKeywordLocale(_instance, value.LCID);
                ValidateHResult(hr);
            }
        }

        public int MaxResults
        {
            get
            {
                var hr = get_QueryMaxResults(_instance, out int maxResults);
                ValidateHResult(hr);
                return maxResults;
            }

            set
            {
                var hr = put_QueryMaxResults(_instance, value);
                ValidateHResult(hr);
            }
        }

        public SEARCH_QUERY_SYNTAX QuerySyntax
        {
            get
            {
                var hr = get_QuerySyntax(_instance, out SEARCH_QUERY_SYNTAX querySyntax);
                ValidateHResult(hr);
                return querySyntax;
            }

            set
            {
                var hr = put_QuerySyntax(_instance, value);
                ValidateHResult(hr);
            }
        }

        public string[] SelectColumns
        {
            get
            {
                var hr = get_QuerySelectColumns(_instance, out IntPtr columns);
                var s = GetStringResult(hr, columns);
                return s.Split(',');
            }

            set
            {
                var hr = put_QuerySelectColumns(_instance, String.Join(",", value));
                ValidateHResult(hr);
            }
        }

        public string[] Sorting
        {
            get
            {
                var hr = get_QuerySorting(_instance, out IntPtr columns);
                var s = GetStringResult(hr, columns);
                return s.Split(',');
            }

            set
            {
                var hr = put_QuerySorting(_instance, String.Join(",", value));
                ValidateHResult(hr);
            }
        }

        public string WhereRestrictions
        {
            get
            {
                var hr = get_QueryWhereRestrictions(_instance, out IntPtr where);
                return GetStringResult(hr, where);
            }

            set
            {
                var hr = put_QueryWhereRestrictions(_instance, value);
                ValidateHResult(hr);
            }
        }

        #region IDisposable Support

        private bool _disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Dispose managed objects
                }

                if (_instance != null)
                    ReleaseSearchQueryHelper(_instance);
                _disposed = true;
            }
        }

        ~SearchQueryHelper()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
