using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Horker.WindowsSearch
{
    public class AstNode
    {
        public TokenType Type;
        public AstNode Lhs;
        public AstNode Rhs;
        public string Value;

        public AstNode(TokenType type, AstNode lhs, AstNode rhs = null)
        {
            Type = type;
            Lhs = lhs;
            Rhs = rhs;
        }

        public AstNode(TokenType type, string value)
        {
            Type = type;
            Value = value;
        }

        public override string ToString()
        {
            var type = Type.ToString();
            var lhs = Lhs == null ? "-" : Lhs.ToString();
            var rhs = Rhs == null ? "-" : Rhs.ToString();

            return string.Format("({0} {1} {2} '{3}')", type, lhs, rhs, Value);
        }
    }
}
