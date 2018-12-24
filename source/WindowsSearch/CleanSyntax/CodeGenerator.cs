using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Horker.WindowsSearch
{
    public class CodeGenerator
    {
        private readonly static Dictionary<TokenType, string> BINARY_OP_REPR = new Dictionary<TokenType, string>() {
            { TokenType.And, "AND" },
            { TokenType.Or, "OR" },
            { TokenType.Like, "LIKE" },
            { TokenType.Eq, "=" },
            { TokenType.Ne, "!=" },
            { TokenType.Lt, "<" },
            { TokenType.Le, "<=" },
            { TokenType.Gt, ">" },
            { TokenType.Ge, ">=" }
        };

        private StringBuilder _out;
        private AstNode _root;

        public CodeGenerator(AstNode root)
        {
            _out = new StringBuilder();
            _root = root;
        }

        public string Generate()
        {
            Generate(_root);
            return _out.ToString();
        }

        private void GenerateBinaryOperator(AstNode node)
        {
            var repr = BINARY_OP_REPR[node.Type];
            _out.Append('(');
            Generate(node.Lhs);
            _out.Append(' ');
            _out.Append(repr);
            _out.Append(' ');
            Generate(node.Rhs);
            _out.Append(')');
        }

        private void Generate(AstNode node)
        {
            switch (node.Type)
            {
                case TokenType.Not:
                    _out.Append("NOT ");
                    Generate(node.Lhs);
                    break;

                case TokenType.Like:
                    if (node.Lhs.Type == TokenType.Any)
                    {
                        _out.Append("CONTAINS(*, ");
                        Generate(node.Rhs);
                        _out.Append(")");
                    }
                    else
                    {
                        GenerateBinaryOperator(node);
                    }
                    break;

                case TokenType.String:
                    _out.Append('\'');
                    _out.Append(node.Value);
                    _out.Append('\'');
                    break;

                case TokenType.Number:
                    _out.Append(node.Value);
                    break;

                case TokenType.Word:
                    _out.Append(PropertyNameResolver.Instance.GetCanonicalName(node.Value));
                    break;

                default:
                    if (BINARY_OP_REPR.ContainsKey(node.Type))
                    {
                        GenerateBinaryOperator(node);
                        return;
                    }
                    throw new ApplicationException(string.Format("Syntax error: {0}", _out.ToString()));
            }
        }
    }
}
