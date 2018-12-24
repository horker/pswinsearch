using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Horker.WindowsSearch
{
    public class Parser
    {
        private Tokenizer _tokenizer;
        private IEnumerator<Token> _reader;
        private Token _t;

        public Parser(Tokenizer tokenizer)
        {
            _tokenizer = tokenizer;
            _reader = _tokenizer.GetReader();
        }

        private Token ReadNextToken()
        {
            if (!_reader.MoveNext())
                _t = new Token(TokenType.EOL, null, 0, 0);
            else
                _t = _reader.Current;
            return _t;
        }

        private static void TestToken(Token token, TokenType type)
        {
            if (token.Type != type)
                throw new ParserException(type.ToString(), token);
        }

        private static void TestLiteral(Token token)
        {
            if (token.Type != TokenType.Number && token.Type != TokenType.String && token.Type != TokenType.Word && token.Type != TokenType.Any)
                throw new ParserException("property name or literal", token);
        }

        private static void TestBinaryOperator(Token token)
        {
            if (token.Type != TokenType.And && token.Type != TokenType.Or && token.Type != TokenType.Like &&
                token.Type != TokenType.Eq && token.Type != TokenType.Ne &&
                token.Type != TokenType.Lt && token.Type != TokenType.Le &&
                token.Type != TokenType.Gt && token.Type != TokenType.Ge)
                throw new ParserException("binary operator", token);
        }

        public AstNode Parse()
        {
            ReadNextToken();
            return ParseExpression();
        }

        private AstNode ParseExpression()
        {
            return ParseOrOperation();
        }

        private AstNode ParseNot()
        {
            var expr = ParseExpression();
            return new AstNode(TokenType.Not, expr);
        }

        private AstNode ParseOrOperation()
        {
            var node = ParseAndOperation();
            while (_t.Type == TokenType.Or)
            {
                ReadNextToken();
                var rhs = ParseAndOperation();
                node = new AstNode(TokenType.Or, node, rhs);
            }
            return node;
        }

        private AstNode ParseAndOperation()
        {
            var node = ParseNotOperation();
            while (_t.Type == TokenType.And)
            {
                ReadNextToken();
                var rhs = ParseNotOperation();
                node = new AstNode(TokenType.And, node, rhs);
            }
            return node;
        }

        private AstNode ParseNotOperation()
        {
            if (_t.Type == TokenType.Not)
            {
                ReadNextToken();
                var expr = ParseCompareOperation();
                return new AstNode(TokenType.Not, expr);
            }

            return ParseCompareOperation();
        }

        private AstNode ParseCompareOperation()
        {
            var node = ParseTerm();
            while (_t.Type == TokenType.Like ||
                _t.Type == TokenType.Eq || _t.Type == TokenType.Ne ||
                _t.Type == TokenType.Lt || _t.Type == TokenType.Le ||
                _t.Type == TokenType.Gt || _t.Type == TokenType.Ge)
            {
                var type = _t.Type;
                ReadNextToken();
                var rhs = ParseTerm();
                node = new AstNode(type, node, rhs);
            }
            return node;
        }

        private AstNode ParseTerm()
        {
            if (_t.Type == TokenType.LeftParen)
            {
                ReadNextToken();
                return ParseSubexpression();
            }

            TestLiteral(_t);
            var node = new AstNode(_t.Type, _t.Value);
            ReadNextToken();
            return node;
        }

        private AstNode ParseSubexpression()
        {
            var node = ParseExpression();
            TestToken(_t, TokenType.RightParen);
            ReadNextToken();
            return node;
        }
    }
}
