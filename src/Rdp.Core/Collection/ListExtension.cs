// Copyright (c) .CHOW TAI FOOK. All rights reserved.
// Author Tim  
// Build on 12/07/2017

using System;
using System.Collections.Generic;
using System.Text;

namespace Rdp.Core.Collection
{
    public static class ListExtension
    {
        /// <summary>
        /// 将含有多个相同Key的List转换成排序的SortedList，适合一对多的快速查找
        /// </summary>
        /// <typeparam name="KeyT"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">需要排序的列表</param>
        /// <param name="keyFun">设置主键的方法</param>
        /// <returns></returns>
        public static SortedList<KeyT, List<T>> ToSortedListWithMultipleKey<KeyT, T>(this List<T> list, Func<T, KeyT> keyFun, bool bSorted)
        {
            if (list.Count <= 0)
                return new SortedList<KeyT, List<T>>();

            if (!bSorted)
                throw new NotImplementedException("请先进行排序");//todo

            var sortedList = new SortedList<KeyT, List<T>>();
            var preKey = keyFun(list[0]);
            var tempList = new List<T>();
            KeyT nowKey = default(KeyT);

            foreach (var e in list)
            {
                nowKey = keyFun(e);
                if (!EqualityComparer<KeyT>.Default.Equals(nowKey,preKey))
                {
                    sortedList.Add(preKey, new List<T>(tempList));
                    tempList.Clear();
                }

                tempList.Add(e);
                preKey = nowKey;
            }

            sortedList.Add(preKey, new List<T>(tempList));

            return sortedList;
        }

        /// <summary>
        /// 将含有多个相同Key的List转换成排序的SortedList，适合一对多的快速查找
        /// </summary>
        /// <typeparam name="KeyT"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="list">需要排序的列表</param>
        /// <param name="keyFun">设置主键的方法</param>
        /// <returns></returns>
        public static Dictionary<KeyT, List<T>> ToDictionaryWithMultipleKey<KeyT, T>(this List<T> list, Func<T, KeyT> keyFun, bool bSorted)
        {
            if (list.Count <= 0)
                return new Dictionary<KeyT, List<T>>();

            if (!bSorted)
                throw new NotImplementedException("请先进行排序"); //todo

            var sortedList = new Dictionary<KeyT, List<T>>();
            var preKey = keyFun(list[0]);
            var tempList = new List<T>();
            KeyT nowKey = default(KeyT);

            foreach (var e in list)
            {
                nowKey = keyFun(e);
                if (!EqualityComparer<KeyT>.Default.Equals(nowKey, preKey))
                {
                    sortedList.Add(preKey, new List<T>(tempList));
                    tempList.Clear();
                }

                tempList.Add(e);
                preKey = nowKey;
            }

            sortedList.Add(preKey, new List<T>(tempList));

            return sortedList;
        }
    }
}
