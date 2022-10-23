using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Invoices.Common
{
    public static class OrderingHelpers
    {
        public static IOrderedQueryable<T> OrderingHelper<T>(this IQueryable<T> source, string propertyName, bool ascending, bool firstSort)
        {
            var param = Expression.Parameter(typeof(T), "prop");
            var property = Expression.Property(param, propertyName);
            var sort = Expression.Lambda(property, param);

            var call = Expression.Call(
                typeof(Queryable),
                (firstSort ? "OrderBy" : "ThenBy") + (ascending ? string.Empty : "Descending"),
                new[] { typeof(T), property.Type },
                source.Expression,
                Expression.Quote(sort));

            return (IOrderedQueryable<T>)source.Provider.CreateQuery<T>(call);
        }
    }
}
