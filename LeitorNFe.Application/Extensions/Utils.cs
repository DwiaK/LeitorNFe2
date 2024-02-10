using System.Collections.Generic;

namespace LeitorNFe.Application.Extensions;

public class Utils
{
    public static bool IsNullOrEmpty<T>(List<T> list)
    {
        return list == null || list.Count == 0;
    }
}
