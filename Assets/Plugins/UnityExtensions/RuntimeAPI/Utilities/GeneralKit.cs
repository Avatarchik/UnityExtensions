using System.Collections;
using UnityEngine;

namespace UnityExtensions
{
    /// <summary>
    /// 普通工具箱
    /// </summary>
    public struct GeneralKit
    {
        /// <summary>
        /// 交换两个变量的值
        /// </summary>
        public static void Swap<T>(ref T a, ref T b)
        {
            T c = a;
            a = b;
            b = c;
        }


        /// <summary>
        /// 判断集合是否为 null 或元素个数是否为 0
        /// </summary>
        public static bool IsNullOrEmpty(ICollection collection)
        {
            return collection == null || collection.Count == 0;
        }


        /// <summary>
        /// 同时设置 Unity 时间缩放和 FixedUpdate 频率
        /// </summary>
        /// <param name="timeScale"> 要设置的时间缩放 </param>
        /// <param name="fixedFrequency"> 要设置的 FixedUpdate 频率 </param>
        public static void SetTimeScaleAndFixedFrequency(float timeScale, float fixedFrequency)
        {
            Time.timeScale = timeScale;
            Time.fixedDeltaTime = timeScale / fixedFrequency;
        }


        /// <summary>
        /// 使用恰当的方式 Destroy Object
        /// </summary>
        public static void DestroySafely(Object obj)
        {
            if (obj != null)
            {
#if UNITY_EDITOR
                if (Application.isPlaying) Object.Destroy(obj);
                else Object.DestroyImmediate(obj);
#else
                Object.Destroy(obj);
#endif
            }
        }

    } // struct GeneralKit

} // namespace UnityExtensions