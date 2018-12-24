using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Horker.WindowsSearch
{
    public enum TokenType
    {
        Word,
        Number,
        String,
        Any,
        And,
        Or,
        Not,
        Like,
        Contains,
        Freetext,
        LeftParen,
        RightParen,
        Eq,
        Ne,
        Lt,
        Le,
        Gt,
        Ge,
        EOL,
        Invalid
    }
}
