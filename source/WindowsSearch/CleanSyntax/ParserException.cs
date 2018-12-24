using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Horker.WindowsSearch
{
    public class ParserException : Exception
    {
        public ParserException(string message)
            : base(message)
        {}

        public ParserException(string expected, string actual)
            : base(string.Format("{0} expected, but {1} found", expected, actual))
        {}

        public ParserException(string expected, Token actual)
            : base(string.Format("{0} expected, but {1} found at {2}:{3}", expected, actual.Type.ToString(), actual.Line, actual.Column))
        {}
    }
}
