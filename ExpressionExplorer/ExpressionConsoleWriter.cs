using System;
using System.Linq.Expressions;

namespace ExpressionExplorer
{
    /// <summary>
    /// A more savvy visitor that prints with indentation and lists
    /// method parameters and chains together binary expressions.
    /// </summary>
    public class ExpressionConsoleWriter
        : ExpressionVisitor
    {
        /// <summary>
        /// Current indentation level
        /// </summary>
        int indent;

        /// <summary>
        /// Returns appropriate tabs and newline
        /// </summary>
        private string Indent => 
            $"\r\n{new string('\t', indent)}";

        /// <summary>
        /// Custom entry point to reset the indentation.
        /// </summary>
        /// <param name="expression">The <see cref="Expression"/> to parse.</param>
        public void Parse(Expression expression)
        {
            indent = 0;
            Visit(expression);
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            if (node.Value is Expression value)
            {
                Visit(value);
            }
            else
            {
                Console.Write($"{node.Value}");
            }
            return node;
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            Console.Write(node.Name);
            return node;
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            if (node.Expression != null)
            {
                Visit(node.Expression);
            }
            Console.Write($".{node.Member?.Name}.");
            return node;
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (node.Object != null)
            {
                Visit(node.Object);
            }
            Console.Write($"{Indent}{node.Method.Name}( ");
            var first = true;
            indent++;
            foreach (var arg in node.Arguments)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    indent--;
                    Console.Write($"{Indent},");
                    indent++;
                }
                Visit(arg);
            }
            indent--;
            Console.Write(") ");
            return node;
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            Console.Write($"{Indent}<");
            indent++;
            Visit(node.Left);
            indent--;
            Console.Write($"{Indent}{node.NodeType}");
            indent++;
            Visit(node.Right);
            indent--;
            Console.Write(">");
            return node;
        }
    }
}
