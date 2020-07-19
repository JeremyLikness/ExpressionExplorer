using System;
using System.Collections.Generic;

namespace ExpressionExplorer
{
    public class Thing
    {
        public Thing()
        {
            Id = Guid.NewGuid().ToString();
            Created = DateTimeOffset.Now;
            Name = Guid.NewGuid().ToString().Split("-")[0];
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public DateTimeOffset Created { get; private set; }

        public string GetId() => Id;

        public static IList<Thing> Things(int count)
        {
            var things = new List<Thing>();
            while (count-- > 0)
            {
                things.Add(new Thing());
            }
            return things;
        }

        public override string ToString() =>
            $"({Id}: {Name}@{Created})";
    }
}
