using System.Collections;
using UnityEngine;

namespace UnityExtension
{
    /// <summary>
    /// Unity 一般性实用工具
    /// </summary>
    public partial struct Utilities
    {
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


        /// <summary>
        /// Hue 转换为颜色. 不同 hue 对应的颜色为:
        /// 0-red; 0.167-yellow; 0.333-green; 0.5-cyan; 0.667-blue; 0.833-magenta; 1-red
        /// </summary>
        public static Color HueToColor(float hue)
        {
            return new Color(
                HueToGreen(hue + 1f / 3f),
                HueToGreen(hue),
                HueToGreen(hue - 1f / 3f));

            float HueToGreen(float h)
            {
                h = ((h % 1f + 1f) % 1f) * 6f;

                if (h < 1f) return h;
                if (h < 3f) return 1f;
                if (h < 4f) return (4f - h);
                return 0f;
            }
        }


        /// <summary>
        /// 将 ARGB32 格式的颜色代码转化为 Color
        /// </summary>
        public static Color ARGB32ToColor(uint argb)
        {
            return new Color(
                ((argb >> 16) & 0xFF) / 255f,
                ((argb >> 8) & 0xFF) / 255f,
                ((argb) & 0xFF) / 255f,
                ((argb >> 24) & 0xFF) / 255f);
            
        }


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

    } // struct Utilities

} // namespace UnityExtension