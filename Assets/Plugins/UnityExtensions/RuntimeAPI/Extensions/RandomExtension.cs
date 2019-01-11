using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityExtensions
{
    /// <summary>
    /// Random 扩展
    /// </summary>
    public static partial class Extension
    {
        public static Vector2 OnUnitCircle(this ref Random random)
        {
            double a = random.Next01() * MathKit.TwoPi;
            return new Vector2((float)Math.Cos(a), (float)Math.Sin(a));
        }


        public static Vector3 OnUnitSphere(this ref Random random)
        {
            // http://mathworld.wolfram.com/SpherePointPicking.html

            double a = random.Next01() * MathKit.TwoPi;
            double cosB = random.Next01() * 2 - 1;
            double sinB = Math.Sqrt(1 - cosB * cosB);

            return new Vector3(
                (float)(Math.Cos(a) * sinB),
                (float)cosB,
                (float)(Math.Sin(a) * sinB));
        }


        public static Vector2 InsideUnitCircle(this ref Random random)
        {
            return random.OnUnitCircle() * (float)Math.Sqrt(random.Next01());
        }


        public static Vector3 InsideUnitSphere(this ref Random random)
        {
            return random.OnUnitSphere() * (float)Math.Pow(random.Next01(), 1.0 / 3.0);
        }


        public static Vector2 InsideEllipse(this ref Random random, Vector2 radius)
        {
            return Vector2.Scale(random.InsideUnitCircle(), radius);
        }


        public static Vector3 InsideEllipsoid(this ref Random random, Vector3 radius)
        {
            return Vector3.Scale(random.InsideUnitSphere(), radius);
        }


        public static float InsideRange(this ref Random random, Range range)
        {
            return random.Range(range.min, range.max);
        }


        public static Vector2 InsideRange2(this ref Random random, Range2 range2)
        {
            return new Vector2(
                random.Range(range2.x.min, range2.x.max),
                random.Range(range2.y.min, range2.y.max));
        }


        public static Vector3 InsideRange3(this ref Random random, Range3 range3)
        {
            return new Vector3(
                random.Range(range3.x.min, range3.x.max),
                random.Range(range3.y.min, range3.y.max),
                random.Range(range3.z.min, range3.z.max));
        }


        /// <summary>
        /// 从一组元素中选择一个. 如果所有元素被选中概率之和小于 1, 那么最后一个元素被选中概率相应增加
        /// </summary>
        /// <param name="getProbability"> 每个元素被选中的概率 </param>
        /// <param name="startIndex"> 开始遍历的索引 </param>
        /// <param name="count"> 遍历元素的数量 </param>
        /// <returns> 被选中的元素索引 </returns>
        public static int Choose(this ref Random random, Func<int, float> getProbability, int startIndex, int count)
        {
            int lastIndex = startIndex + count - 1;
            float rest = (float)random.Next01();
            float current;

            for (; startIndex < lastIndex; startIndex++)
            {
                current = getProbability(startIndex);
                if (rest < current) return startIndex;
                else rest -= current;
            }

            return lastIndex;
        }


        /// <summary>
        /// 从一组元素中选择一个. 如果所有元素被选中概率之和小于 1, 那么最后一个元素被选中概率相应增加
        /// </summary>
        /// <param name="probabilities"> 每个元素被选中的概率 </param>
        /// <param name="startIndex"> 开始遍历的索引 </param>
        /// <param name="count"> 遍历元素的数量. 如果这个值无效, 自动遍历到列表尾部 </param>
        /// <returns> 被选中的元素索引 </returns>
        public static int Choose(this ref Random random, IList<float> probabilities, int startIndex = 0, int count = 0)
        {
            if (count < 1 || count > probabilities.Count - startIndex)
            {
                count = probabilities.Count - startIndex;
            }

            int lastIndex = startIndex + count - 1;
            float rest = (float)random.Next01();
            float current;

            for (; startIndex < lastIndex; startIndex++)
            {
                current = probabilities[startIndex];
                if (rest < current) return startIndex;
                else rest -= current;
            }

            return lastIndex;
        }


        /// <summary>
        /// 将一组元素随机排序
        /// </summary>
        /// <typeparam name="T"> 元素类型 </typeparam>
        /// <param name="list"> 元素列表 </param>
        /// <param name="startIndex"> 开始排序的索引 </param>
        /// <param name="count"> 执行排序的元素总数. 如果这个值无效, 自动遍历到列表尾部 </param>
        public static void Sort<T>(this ref Random random, IList<T> list, int startIndex = 0, int count = 0)
        {
            int lastIndex = startIndex + count;
            if (lastIndex <= startIndex || lastIndex > list.Count)
            {
                lastIndex = list.Count;
            }

            lastIndex -= 1;

            T temp;
            int swapIndex;

            for (int i = startIndex; i < lastIndex; i++)
            {
                swapIndex = random.Range(i, lastIndex + 1);
                temp = list[i];
                list[i] = list[swapIndex];
                list[swapIndex] = temp;
            }
        }

    } // class Extension

} // namespace UnityExtensions