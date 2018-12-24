using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Horker.WindowsSearch
{
    public class Tokenizer
    {
        private string _code;

        private readonly static Regex REGEX = new Regex(@"(?<and>and)|(?<or>or)|(?<not>not)|(?<like>like)|(?<contains>contains)|(?<freetext>freetext)|(?<any>\*)|(?<number>[.\d][.\d\w]+)|(?<word>\w+)|(?<space>\s+)|(?<leftparen>\()|(?<rightparen>\))|(?<eq>=)|(?<ne>(!=|<>))|(?<lt><)|(?<le><=)|(?<gt>>)|(?<ge>>=)|(?<str>')", RegexOptions.IgnoreCase);

        private readonly static TokenType[] TOKEN_TYPES = new TokenType[] {
            TokenType.And,
            TokenType.Or,
            TokenType.Not,
            TokenType.Like,
            TokenType.Contains,
            TokenType.Freetext,
            TokenType.Any,
            TokenType.Number,
            TokenType.Word,
            TokenType.Invalid,
            TokenType.LeftParen,
            TokenType.RightParen,
            TokenType.Eq,
            TokenType.Ne,
            TokenType.Lt,
            TokenType.Le,
            TokenType.Gt,
            TokenType.Ge,
            TokenType.String
        };

        public Tokenizer(string code)
        {
            _code = code;
        }

        public IEnumerator<Token> GetReader()
        {
            for (var p = 0; p < _code.Length;)
            {
                var start = p;
                var m = REGEX.Match(_code, p);
                var index = 1;
                Group g = null;
                for (; index < m.Groups.Count; ++index)
                {
                    if (m.Groups[index].Success)
                    {
                        g = m.Groups[index];
                        break;
                    }
                }

                if (g == null)
                    throw new ApplicationException("Unexpected token found at 1:" + (p + 1));

                TokenType t = TOKEN_TYPES[index - 1];
                string value = g.Value;
                if (t == TokenType.String)
                {
                    ++p;
                    while (_code[p] != '\'')
                        ++p;
                    value = _code.Substring(start + 1, p - start - 1);
                    ++p;
                }
                else
                {
                    p += g.Length;
                }

                if (t != TokenType.Invalid)
                    yield return new Token(t, value, 1, start + 1);
            }
        }
    }
}
