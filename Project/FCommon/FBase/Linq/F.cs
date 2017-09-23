using System;
using System.Collections.Generic;

namespace FFF.Base.Linq
{
    public static class F
    {

        /// <summary>
        /// 闭区间迭代
        /// </summary>
        /// <param name="begin">初始值</param>
        /// <param name="end">结束值</param>
        /// <param name="step">步长</param>
        /// <returns></returns>
        public static IEnumerable<int> For(int begin, int end, int step)
        {
            if (step > 0)
            {
                while (begin <= end)
                {
                    yield return begin;
                    begin += step;
                }
            }
            else if (step < 0)
            {
                while (begin >= end)
                {
                    yield return begin;
                    begin += step;
                }
            }
        }

        /// <summary>
        /// 闭区间迭代，根据begin和end决定递增或递减，步长为1
        /// </summary>
        /// <param name="begin">初始值</param>
        /// <param name="end">结束值</param>
        /// <returns></returns>
        public static IEnumerable<int> For(int begin, int end)
        {
            return For(begin, end, begin <= end ? 1 : -1);
        }

        /// <summary>
        /// [0, count)迭代
        /// </summary>
        /// <param name="count">迭代计数</param>
        /// <returns></returns>
        public static IEnumerable<int> For(int count)
        {
            var i = 0;
            while (i < count)
            {
                yield return i++;
            }
        }

        public static IEnumerable<int> For<T>(List<T> list)
        {
            var i = 0;
            while (i < list.Count)
            {
                yield return i++;
            }
        }

        public static IEnumerable<int> For<T>(T[] array)
        {
            var i = 0;
            while (i < array.Length)
            {
                yield return i++;
            }
        }

        public static IEnumerable<int> While()
        {
            var i = 0;
            while (true)
            {
                yield return i++;
            }
        }

        public static IEnumerable<int> LimitWhile(int maxCount)
        {
            var i = 0;
            while (i < maxCount)
            {
                yield return i++;
            }
            if (i == maxCount)
            {
                throw new WhileTooManyTimesException();
            }
        }

        public static void Swap<T>(ref T x, ref T y)
        {
            var tmp = x;
            x = y;
            y = tmp;
        }

        public static TArg Assign<TBase, TArg>(TArg obj, out TBase arg)
            where TArg : TBase
        {
            arg = obj;
            return obj;
        }

    }

    public class WhileTooManyTimesException : Exception
    {
    }

}
