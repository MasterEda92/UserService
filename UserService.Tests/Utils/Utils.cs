using Microsoft.AspNetCore.Mvc;

namespace UserService.Tests.Utils;

public static class Utility
{
    public static T? GetObjectResultContent<T>(ActionResult<T> result)
    {
        return (T)(((ObjectResult)result.Result!)!).Value!;
    }

    public static IEnumerable<(T item, int index)> WithIndex<T>(this IEnumerable<T> source)
    {
        return source.Select((item, index) => (item, index));
    }
}