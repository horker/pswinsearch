using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Horker.WindowsSearch
{
    public class PropertyNameResolver
    {
        [DllImport(@"SearchQueryHelper.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private extern static int GetAllProperties(out IntPtr properties);

        private List<string> _canonicalNames;
        private List<string> _displayNames;
        private Dictionary<string, string> _nameMap;
        private Dictionary<string, List<string>> _displayNameMap;

        public List<string> CanonicalNames => _canonicalNames;
        public List<string> DisplayNames => _displayNames;

        public PropertyNameResolver()
        {
            PrepareMaps();
        }

        private void AddValue(Dictionary<string, List<string>> dic, string key, string value)
        {
            if (string.IsNullOrEmpty(key))
                return;

            if (dic.TryGetValue(key, out List<string> values))
            {
                values.Add(value);
                dic[key] = values;
            }
            else
            {
                dic[key] = new List<string>() { value };
            }
        }

        private void PrepareMaps()
        {
            var hr = GetAllProperties(out IntPtr p);
            var s = Marshal.PtrToStringUni(p);
            Marshal.FreeCoTaskMem(p);

            var names = s.Split('\t');

            _canonicalNames = new List<string>();
            _displayNames = new List<string>();

            _displayNameMap = new Dictionary<string, List<string>>();
            _nameMap = new Dictionary<string, string>();

            for (var i = 0; i < names.Length; i += 2)
            {
                _canonicalNames.Add(names[i]);
                _displayNames.Add(names[i + 1]);
                _nameMap.Add(names[i].ToLower(), names[i]);
                AddValue(_displayNameMap, names[i + 1].ToLower(), names[i]);
            }
        }

        private string GetMinimalLengthString(List<string> values)
        {
            if (values.Count == 1)
                return values[0];
            var min = values.Min(x => x.Length);
            foreach (var v in values)
            {
                if (v.Length == min)
                    return v;
            }
            throw new ApplicationException("unreachable");
        }

        public string GetCanonicalName(string name)
        {
            var n = name.ToLower();

            List<string> values;
            if (_displayNameMap.TryGetValue(n, out values))
            {
                if (values.Count == 1)
                    return values[0];

                foreach (var v in values)
                {
                    if (v.ToLower().IndexOf("system.") == 0 && v.IndexOf('.', "system.".Length) == -1)
                        return v;
                }

                throw new ApplicationException("For a display name '" + name + "', multiple properties matched: " + string.Join(" ", values));
            }

            string value;
            if (_nameMap.TryGetValue(n, out value))
                return value;

            if (_nameMap.TryGetValue("system." + n, out value))
                return value;

            throw new ApplicationException("Property not found: " + name);
        }

        public static PropertyNameResolver Instance => new PropertyNameResolver();
    }
}
