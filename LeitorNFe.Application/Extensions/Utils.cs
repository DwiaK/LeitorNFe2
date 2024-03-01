using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LeitorNFe.Application.Extensions;

public static class Utils
{
    public static bool IsNullOrEmpty<T>(List<T> list)
    {
        return list == null || list.Count == 0;
    }

    public static async Task ForEachAsync<T>(this List<T> list, Func<T, Task> func)
    {
        foreach (var value in list)
        {
            await func(value);
        }
    }
}
