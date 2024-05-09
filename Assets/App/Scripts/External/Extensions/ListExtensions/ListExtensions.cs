using System;
using System.Collections.Generic;

namespace App.Scripts.External.Extensions.ListExtensions
{
    public static class ListExtensions
    {
        public static TResult GetRandomValue<TResult>(this List<TResult> list)
        {
            if (list.Count == 0)
            {
                return default;
            }

            return list[new Random().Next(0, list.Count)];
        }

        public static List<TResult> CreateClone<TResult>(this List<TResult> list)
        {
            List<TResult> newList = new();

            foreach (TResult result in list)
            {
                newList.Add(result);
            }

            return newList;
        }
    }
}