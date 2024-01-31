using System;
using System.Collections.Generic;

public static class ListExtensions
{
    private static Random random = new Random();

    // Extension-Methode, um eine List<T> in-place zu mischen
    public static void ListShuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = random.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}
