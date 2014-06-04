using Disseminate.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public static class DisseminateExtensions
{
    public static void FastRemove<T>(this List<T> list, T val)
    {
        var index = list.IndexOf(val);
        list.FastRemoveAt(index);
    }

    public static void FastRemoveAt<T>(this List<T> list, int index)
    {
        list[index] = list[list.Count - 1];
        list.RemoveAt(list.Count - 1);
    }
}
