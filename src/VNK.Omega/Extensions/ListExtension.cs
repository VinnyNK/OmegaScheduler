namespace VNK.Omega.Extensions;

internal static class ListExtension
{
    internal static T Pop<T>(this IList<T> source) where T : class
    {
        if(!source.Any()) throw new StackOverflowException();
        var item = source[0];
        source.RemoveAt(0);
        return item;
    }

    internal static T Peek<T>(this IList<T> source) where T : class
    {
        if(!source.Any()) throw new StackOverflowException();
        return source[0];
    }
}