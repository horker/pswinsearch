using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Horker.WindowsSearch
{
    public class PropertyExpander
    {
        public static string Expand(string code, bool allowDisplayName)
        {
            var result = new StringBuilder();

            for (var p = 0; p < code.Length;)
            {
                if (code[p] == '\'')
                {
                    result.Append(code[p]);
                    ++p;
                    try
                    {
                        while (true)
                        {
                            for (; code[p] != '\''; ++p)
                                result.Append(code[p]);
                            result.Append('\'');
                            ++p;
                            if (p == code.Length || code[p] != '\'')
                                break;
                        }
                    }
                    catch (IndexOutOfRangeException)
                    {
                        throw new ApplicationException("String literal not terminated");
                    }
                }
                else if (code[p] == '"')
                {
                    result.Append(code[p]);
                    ++p;
                    while (true)
                    {
                        while (code[p] != '"')
                        {
                            result.Append(code[p]);
                            ++p;
                        }
                        if (code[p] != '"')
                            break;
                        result.Append(code[p]);
                        ++p;
                    }

                }
                else if (code[p] == '@')
                {
                    ++p;

                    string prop;
                    if (code[p] == '\'')
                    {
                        ++p;
                        var start = p;
                        while (p < code.Length && code[p] != '\'')
                            ++p;

                        if (p == code.Length)
                            throw new ApplicationException("Property expansion not terminated");

                        prop = code.Substring(start, p - start);
                        ++p;
                    }
                    else
                    {
                        var m = new Regex(@"[\w\.]+").Match(code, p);
                        if (!m.Success || m.Index != p || m.Value.Length == 0)
                            throw new ApplicationException("Invalid property specified after '@'");

                        prop = m.Value;
                        p += prop.Length;
                    }

                    result.Append(PropertyNameResolver.Instance.GetCanonicalName(prop, allowDisplayName));
                }
                else
                {
                    result.Append(code[p]);
                    ++p;
                }
            }

            return result.ToString();
        }
    }
}