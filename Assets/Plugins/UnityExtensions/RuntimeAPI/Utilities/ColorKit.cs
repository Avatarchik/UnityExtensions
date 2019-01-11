using UnityEngine;

namespace UnityExtensions
{
    /// <summary>
    /// Color 工具箱
    /// </summary>
    public struct ColorKit
    {
        /// <summary>
        /// 计算颜色的感观亮度
        /// 参考: http://alienryderflex.com/hsp.html
        /// </summary>
        /// <param name="color"> 要计算感观亮度的颜色, alpha 通道被忽略 </param>
        /// <returns> 颜色的感观亮度, [0..1] </returns>
        public static float GetPerceivedBrightness(Color color)
        {
            return Mathf.Sqrt(
                0.241f * color.r * color.r +
                0.691f * color.g * color.g +
                0.068f * color.b * color.b);
        }


        /// <summary>
        /// 将颜色转化为 ARGB32 格式的颜色代码
        /// </summary>
        public static uint ToARGB32(Color c)
        {
            return ((uint)(c.a * 255) << 24)
                 | ((uint)(c.r * 255) << 16)
                 | ((uint)(c.g * 255) << 8)
                 | ((uint)(c.b * 255));
        }


        /// <summary>
        /// 将 ARGB32 格式的颜色代码转化为 Color
        /// </summary>
        public static Color FromARGB32(uint argb)
        {
            return new Color(
                ((argb >> 16) & 0xFF) / 255f,
                ((argb >> 8) & 0xFF) / 255f,
                ((argb) & 0xFF) / 255f,
                ((argb >> 24) & 0xFF) / 255f);

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

    } // struct ColorKit

} // namespace UnityExtensions