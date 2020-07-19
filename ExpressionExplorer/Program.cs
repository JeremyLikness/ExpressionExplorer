using System;
using System.Linq;
using System.Linq.Expressions;

namespace ExpressionExplorer
{
    class Program
    {
        static void Main(string[] _)
        {
            Parse();
        }

        static void Divider()
        {
            Console.WriteLine("\r\n---Hit ENTER to Run Next Section---\r\n");
            Console.ReadLine();
        }

        static void Parse()
        {
            void RunSection(Expression<Action> action)
            {
                var method = (MethodCallExpression)action.Body;
                Console.WriteLine($"{method.Method.Name}:");
                action.Compile()();
            }

            var query = Thing.Things(500).AsQueryable()
                .Where(t => t.Name.Contains("a",
                StringComparison.InvariantCultureIgnoreCase) &&
                t.Created > DateTimeOffset.Now.AddDays(-1))
                .Skip(2)
                .Take(50)
                .OrderBy(t => t.Created);

            RunSection(() => PrintQuery(query));

            Divider();

            RunSection(() => BasicVisitor(query));

            Divider();

            RunSection(() => SavvyVisitor(query));

            Divider();

            RunSection(() => Constrainer(query));

            Divider();

            RunSection(() => RunQuery(query));

            Divider();

            RunSection(() => RunTransformed(query));
        }

        static void PrintQuery(IOrderedQueryable<Thing> query)
        {
            Console.WriteLine(query.ToString());
        }

        static void BasicVisitor(IOrderedQueryable<Thing> query)
        {
            new BasicExpressionConsoleWriter().Visit(query.Expression);
        }

        static void SavvyVisitor(IOrderedQueryable<Thing> query)
        {
            new ExpressionConsoleWriter().Parse(query.Expression);
        }

        static void Constrainer(IOrderedQueryable<Thing> query)
        {
            new ExpressionConsoleWriter().Parse(
                new ExpressionTakeRestrainer()
                .ParseAndConstrainTake(query.Expression, 5));
        }

        static void RunQuery(IOrderedQueryable<Thing> query)
        {
            var list = query.ToList();
            Console.WriteLine($"Query results: {list.Count}");
        }

        static void RunTransformed(IOrderedQueryable<Thing> query)
        {
            var transformedQuery =
                new TranslatingHost<Thing>(query, 5);
            var list2 = transformedQuery.ToList();

            Console.WriteLine($"Modified query results: {list2.Count}");
        }
    }
}
