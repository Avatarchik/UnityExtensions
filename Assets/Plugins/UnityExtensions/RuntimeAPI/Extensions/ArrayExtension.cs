using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityExtensions
{
    /// <summary>
    /// 数组扩展方法
    /// </summary>
    public static partial class Extension
    {
        /// <summary>
        /// 设置一维数组中一个区段的元素的值
        /// </summary>
        /// <param name="index"> 开始设置值的下标 </param>
        /// <param name="count"> 连续设置值的元素数, 非正值表示直到数组尾部 </param>
        public static void SetValues<T>(
            this T[] array,
            T value = default,
            int index = 0,
            int count = 0)
        {
            int lastIndex = count > 0 ? (index + count) : array.Length;
            while(index < lastIndex) array[index++] = value;
        }


        /// <summary>
        /// 设置二维数组中一个区域的元素的值
        /// </summary>
        /// <param name="beginRowIndex"> 区域开始的行下标 </param>
        /// <param name="beginColIndex"> 区域开始的列下标 </param>
        /// <param name="endRowIndex"> 区域结束的行下标, 非正值表示直到数组边界 </param>
        /// <param name="endColIndex"> 区域结束的列下标, 非正值表示直到数组边界 </param>
        public static void SetValues<T>(
            this T[,] array,
            T value = default,
            int beginRowIndex = 0,
            int beginColIndex = 0,
            int endRowIndex = 0,
            int endColIndex = 0)
        {
            if (endRowIndex <= 0) endRowIndex = array.GetLength(0)-1;
            if (endColIndex <= 0) endColIndex = array.GetLength(1)-1;

            for (int i = beginRowIndex; i <= endRowIndex; i++)
            {
                for (int j = beginColIndex; j <= endColIndex; j++)
                {
                    array[i,j] = value;
                }
            }
        }


        /// <summary>
        /// 找到一组元素中最接近的一个
        /// </summary>
        /// <returns> 最接近的元素下标 </returns>
        public static int FindNearest(this float[] items, float value)
        {
            if (GeneralKit.IsNullOrEmpty(items)) return -1;

            int result = 0;
            float minError = Mathf.Abs(value - items[0]);
            float error;

            for (int i = 1; i < items.Length; i++)
            {
                error = Mathf.Abs(value - items[i]);
                if (error < minError)
                {
                    minError = error;
                    result = i;
                }
            }

            return result;
        }


        /// <summary>
        /// 修改 List<> 的元素数量, newValue 指定新添加的元素
        /// </summary>
        public static void Resize<T>(this List<T> list, int newSize, T newValue = default)
        {
            if (list.Count != newSize)
            {
                if (list.Count > newSize)
                {
                    list.RemoveRange(newSize, list.Count - newSize);
                }
                else
                {
                    int addCount = newSize - list.Count;

                    while (addCount > 0)
                    {
                        list.Add(newValue);
                        addCount--;
                    }
                }
            }
        }


        /// <summary>
        /// 修改 IList 的元素数量, newValue 指定新添加的元素
        /// </summary>
        public static void Resize(this IList list, int newSize, object newValue = null)
        {
            if (list.Count != newSize)
            {
                if (list.Count > newSize)
                {
                    for (int i = list.Count - 1; i >= newSize; i--)
                    {
                        list.RemoveAt(i);
                    }
                }
                else
                {
                    int addCount = newSize - list.Count;

                    while (addCount > 0)
                    {
                        list.Add(newValue);
                        addCount--;
                    }
                }
            }
        }


        /// <summary>
        /// 对列表中一段元素排序
        /// </summary>
        /// <param name="index"> 参与排序元素的开始下标 </param>
        /// <param name="count"> 参与排序的元素数量, 非正值表示直到列表尾部 </param>
        public static void Sort<T>(this IList<T> list, Comparison<T> compare, int index = 0, int count = 0)
        {
            if (count <= 0) count = list.Count - index;
            int lastIndex = index + count - 1;
            T temp;

            for (int i = 0; i < count - 1; i++)
            {
                for (int j = index; j < lastIndex; j++)
                {
                    if (compare(list[j], list[j+1]) > 0)
                    {
                        temp = list[j];
                        list[j] = list[j+1];
                        list[j+1] = temp;
                    }
                }

                lastIndex--;
            }
        }


        /// <summary>
        /// 遍历任意维度的数组
        /// </summary>
        /// <param name="array"> 执行遍历的数组 </param>
        /// <param name="onElement"> 遍历到每一个数组元素时执行此方法, 参数 1 是当前元素从 0 开始的维度, 参数 2 是此元素在每个维度的下标组成的数组 </param>
        /// <param name="beginDimension"> 遍历每个维度开始时执行此方法, 参数 1 是从 0 开始的当前维度值, 参数 2 是此维度之前每个维度的下标组成的数组 </param>
        /// <param name="endDimension"> 遍历每个维度结束时执行此方法, 参数 1 是从 0 开始的当前维度值, 参数 2 是此维度之前每个维度的下标组成的数组 </param>
        public static void Traverse(
            this Array array,
            Action<int, int[]> onElement,
            Action<int, int[]> beginDimension = null,
            Action<int, int[]> endDimension = null)
        {
            if (array.Length != 0)
            {
                TraverseArrayDimension(0, new int[array.Rank]);
            }

            void TraverseArrayDimension(int dimension, int[] indices)
            {
                int size = array.GetLength(dimension);
                bool isFinal = (dimension + 1 == array.Rank);

                beginDimension?.Invoke(dimension, indices);

                for (int i = 0; i < size; i++)
                {
                    indices[dimension] = i;
                    if (isFinal)
                    {
                        onElement?.Invoke(dimension, indices);
                    }
                    else TraverseArrayDimension(dimension + 1, indices);
                }

                endDimension?.Invoke(dimension, indices);
            }
        }


        /// <summary>
        /// 获取数组内容的字符串描述, 该字符串与 C# 代码书写格式相似
        /// 如果 elementToString 为 null 将使用默认方法, 即：
        ///        空引用使用 “null” 表示, string 类型的两边会添加“"”, 其他类型通过 ToString() 获得描述
        /// </summary>
        public static string ToCodeString(this Array array, Func<object, string> elementToString = null)
        {
            if (array == null) return "Null Array";

            if (elementToString == null)
            {
                elementToString = obj =>
                {
                    if (ReferenceEquals(obj, null))
                    {
                        return "null";
                    }
                    if (obj.GetType() == typeof(string))
                    {
                        return string.Format("\"{0}\"", obj);
                    }
                    return obj.ToString();
                };
            }

            var builder = new System.Text.StringBuilder(array.Length * 4);

            Traverse(array,
                (d, i) =>
                {
                    if (i[d] != 0) builder.Append(',');
                    builder.Append(' ');
                    object obj = array.GetValue(i);
                    builder.Append(elementToString(obj));
                },

                (d, i) =>
                {
                    if (d != 0)
                    {
                        if(i[d - 1] != 0) builder.Append(',');
                        builder.Append('\n');
                        while(d != 0)
                        {
                            builder.Append('\t');
                            d--;
                        }
                    }
                    builder.Append('{');
                },

                (d, i) =>
                {
                    if (d + 1 == array.Rank) builder.Append(" }");
                    else
                    {
                        builder.Append('\n');
                        while (d != 0)
                        {
                            builder.Append('\t');
                            d--;
                        }
                        builder.Append('}');
                    }
                });

            return builder.ToString();
        }

    } // class Extension

} // namespace UnityExtensions