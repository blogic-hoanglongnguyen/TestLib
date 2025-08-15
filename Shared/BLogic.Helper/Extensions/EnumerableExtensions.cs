namespace BLogic.Helper.Extensions;

public static class EnumerableExtensions
{
    public static bool IsNullOrEmpty<T>(this IEnumerable<T> source)
    {
        return source == null || !source.Any();
    }
    
    public static bool IsNotNullAndEmpty<T>(this IEnumerable<T> source)
    {
        return !source.IsNullOrEmpty();
    }
}