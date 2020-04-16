using System;
using System.Collections.Generic;
using System.Linq;

namespace Asperand.IrcBallistic.InversionOfControl.Extenstions
{
    public static class IEnumerableExtensions
    {
        public static (IEnumerable<T> True, IEnumerable<T> False) SplitBy<T>(this IEnumerable<T> types, Func<T, bool> predicate)
        {
            var typeGrouping = types
                .GroupBy(predicate);

            return (ReduceGrouping(typeGrouping, true), ReduceGrouping(typeGrouping, false));
        }
        
        private static IEnumerable<T> ReduceGrouping<T>(IEnumerable<IGrouping<bool, T>> e, bool filter) =>
            e.Where(g => g.Key == filter)
                .SelectMany(g => g).ToList();
    }
}