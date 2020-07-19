using System.Linq;
using System.Linq.Expressions;

namespace ExpressionExplorer
{
    /// <summary>
    /// Parses a tree and constraints any <see cref="Enumerable.Take"/>
    /// calls to a max value. Also sets a flag indicating whether a 
    /// "take" was encountered.
    /// </summary>
    public class ExpressionTakeRestrainer : ExpressionVisitor
    {
        /// <summary>
        /// What is the most we can take?
        /// </summary>
        private int maxTake;

        /// <summary>
        /// Sets to <c>true</c> when "take" is encountered.
        /// </summary>
        public bool ExpressionHasTake { get; private set; }

        /// <summary>
        /// Parses the expression.
        /// </summary>
        /// <param name="expression">The <see cref="Expression"/> to parse.</param>
        /// <param name="maxTake">Max take.</param>
        /// <returns>The new expression tree with modifications.</returns>
        public Expression ParseAndConstrainTake(
            Expression expression, int maxTake)
        {
            this.maxTake = maxTake;
            ExpressionHasTake = false;
            return Visit(expression);
        }
    
        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            // it's a take
            if (node.Method.Name == nameof(Enumerable.Take))
            {
                ExpressionHasTake = true;

                // should have two arguments: the list and the take count
                if (node.Arguments.Count == 2 && 
                    node.Arguments[1] is ConstantExpression constant)
                {
                    var takeCount = (int)constant.Value;
                    // exceeds limits, will rewrite
                    if (takeCount > maxTake)
                    {
                        // just visit the first argument to resolve
                        var arg1 = Visit(node.Arguments[0]);
                        // make our own second argument
                        var arg2 = Expression.Constant(maxTake);
                        // rebuild the call node
                        var methodCall = Expression.Call(
                            node.Object, 
                            node.Method, 
                            new[] { arg1, arg2 } );
                        return methodCall;
                    }
                }
            }
            return base.VisitMethodCall(node);
        }
    }
}
