using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ExpressionExplorer
{
    /// <summary>
    /// Implementation that constrains max <see cref="Enumerable.Take"/>.
    /// </summary>
    /// <typeparam name="T">The type.</typeparam>
    public class TranslatingHost<T> : IOrderedQueryable<T>, IOrderedQueryable
    {
        private readonly IQueryable<T> query;

        public Type ElementType => typeof(T);

        private Expression TranslatedExpression { get; set; }

        public TranslatingHost(IQueryable<T> query, int maxTake)
        {
            this.query = query;
            var translator = new ExpressionTakeRestrainer();
            TranslatedExpression = translator
                .ParseAndConstrainTake(query.Expression, maxTake);
        }

        public Expression Expression => TranslatedExpression;

        public IQueryProvider Provider => query.Provider;

        public IEnumerator<T> GetEnumerator()
            => Provider.CreateQuery<T>(TranslatedExpression)
            .GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
