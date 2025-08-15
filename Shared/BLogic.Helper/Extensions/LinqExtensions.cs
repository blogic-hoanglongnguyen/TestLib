using System.Linq.Expressions;

namespace BLogic.Helper.Extensions;

public static class LinqExtensions
{
    public static IQueryable<T> OrderByDynamic<T>(this IQueryable<T> source, string sortBy, string sortDirection)
    {
        var param = Expression.Parameter(typeof(T), "x");
        var property = Expression.PropertyOrField(param, sortBy);
        var lambda = Expression.Lambda(property, param);

        string method = sortDirection?.ToLower() == "desc" ? "OrderByDescending" : "OrderBy";

        var result = typeof(Queryable).GetMethods()
            .First(m => m.Name == method && m.GetParameters().Length == 2)
            .MakeGenericMethod(typeof(T), property.Type)
            .Invoke(null, new object[] { source, lambda });

        return (IQueryable<T>)result!;
    }
}