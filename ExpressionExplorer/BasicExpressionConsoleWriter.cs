using System;
using System.Linq.Expressions;

namespace ExpressionExplorer
{
    /// <summary>
    /// A basic implementation that just prints some information
    /// and passes through to the base class.
    /// </summary>
    public class BasicExpressionConsoleWriter : ExpressionVisitor
    {
        protected override Expression VisitBinary(BinaryExpression node)
        {
            Console.Write($" binary:{node.NodeType} ");
            return base.VisitBinary(node);
        }

        protected override Expression VisitUnary(UnaryExpression node)
        {
            if (node.Method != null)
            {
                Console.Write($" unary:{node.Method.Name} ");
            }
            Console.Write($" unary:{node.Operand.NodeType} ");
            return base.VisitUnary(node);
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            Console.Write($" constant:{node.Value} ");
            return base.VisitConstant(node);
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            Console.Write($" member:{node.Member.Name} ");
            return base.VisitMember(node);
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            Console.Write($" call:{node.Method.Name} ");
            return base.VisitMethodCall(node);
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            Console.Write($" p:{node.Name} ");
            return base.VisitParameter(node);
        }
    }
}
