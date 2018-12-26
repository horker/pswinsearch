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
        public static string Expand(string code)
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
                            for (; p < code.Length && code[p] != '\''; ++p)
                                result.Append(code[p]);
                            if (p == code.Length || code[p] != '\'')
                                break;
                            result.Append(code[p]);
                            ++p;
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
                    var m = new Regex(@"[\w\.]+").Match(code, p);
                    if (!m.Success || m.Index != p)
                    {
                        result.Append('@');
                    }
                    else
                    {
                        var prop = m.Value;
                        p += prop.Length;
                        result.Append(PropertyNameResolver.Instance.GetCanonicalName(prop));
                    }
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