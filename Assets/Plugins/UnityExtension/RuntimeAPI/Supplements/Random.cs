using System;

namespace UnityExtension
{
    /// <summary>
    /// 随机数（使用 Create 方法创建对象）
    /// </summary>
    [Serializable]
    public struct Random
    {
        /// <summary>
        /// 种子
        /// </summary>
        public uint seed;


        // 静态 Random 实例, 用以自动分配随机种子
        static Random _static = Create((uint)(DateTime.Now.Ticks & 0x_FFFF_FFFF));


        // 更新种子，每次使用前都会在内部调用
        void Next()
        {
            //
            // https://en.wikipedia.org/wiki/Lehmer_random_number_generator
            //
            // 1.在原始实现中，seed 值域为 [1, 2147483646]，为使用方便此处使用取余运算并最后加 1 将 UInt32 的 seed 转化到此范围
            // 2.将数值转换为 UInt64 是为了防止乘法运算溢出
            // 3.原始实现中最后取余运算的结果直接存储到了 seed 中（运算结果最小为 1 而不会是 0），为了与步骤 1 兼容最后需要减去 1
            //
            seed = (uint)((seed % 2147483646U + 1U) * 48271UL % 2147483647UL) - 1U;
        }


        /// <summary>
        /// 返回一个 [0, 1) 范围内的随机浮点数（double）
        /// <summary>
        public double Next01()
        {
            Next();
            //
            // seed 此时值域为 [0, 2147483645]
            //
            return seed / 2147483646.0;
        }


        /// <summary>
        /// 创建一个随机对象，使用指定的种子初始化此对象
        /// </summary>
        public static Random Create(uint seed)
        {
            return new Random { seed = seed };
        }


        /// <summary>
        /// 创建一个随机对象，使用自动分配的随机种子初始化
        /// 自动分配的随机种子与时间无关，因此连续调用不会产生相同的结果
        /// </summary>
        public static Random Create()
        {
            _static.Next();
            return new Random { seed = ~_static.seed };
        }


        /// <summary>
        /// 返回一个 [0, 1) 范围内的随机浮点数
        /// </summary>
        public float Range01()
        {
            return (float)Next01();
        }


        /// <summary>
        /// 返回一个指定范围内的随机浮点数
        /// </summary>
        /// <param name="minValue"> 返回的随机数的下界(包含) </param>
        /// <param name="maxValue"> 返回的随机数的上界(不包含) </param>
        /// <returns> [minValue, maxValue) 范围的均匀分布随机数 </returns>
        public float Range(float minValue, float maxValue)
        {
            return minValue + (maxValue - minValue) * (float)Next01();
        }


        /// <summary>
        /// 返回一个指定范围内的随机整数
        /// </summary>
        /// <param name="minValue"> 返回的随机数的下界(包含) </param>
        /// <param name="maxValue"> 返回的随机数的上界(不包含) </param>
        /// <returns> [minValue, maxValue) 范围的均匀分布随机数 </returns>
        public int Range(int minValue, int maxValue)
        {
            return minValue + (int)((maxValue - minValue) * Next01());
        }


        /// <summary>
        /// 测试随机事件在一次独立实验中是否发生
        /// </summary>
        /// <param name="probability"> [0f, 1f] 范围的概率 </param>
        /// <returns> 如果事件发生返回 true, 否则返回 false </returns>
        public bool Test(float probability)
        {
            return Next01() < probability;
        }


        /// <summary>
        /// 产生正态分布的随机数
        /// 正态分布随机数落在 μ±σ, μ±2σ, μ±3σ 的概率依次为 68.26%, 95.44%, 99.74%
        /// </summary>
        /// <param name="averageValue"> 正态分布的平均值, 即 N(μ, σ^2) 中的 μ </param>
        /// <param name="standardDeviation"> 正态分布的标准差, 即 N(μ, σ^2) 中的 σ </param>
        /// <returns> 返回正态分布的随机数. 理论值域是 μ±∞ </returns>
        public float Normal(float averageValue, float standardDeviation)
        {
            //
            // https://en.wikipedia.org/wiki/Box-Muller_transform
            //
            return averageValue + standardDeviation * (float)
                (
                    Math.Sqrt(-2 * Math.Log(1 - Next01())) * Math.Sin(Utilities.TwoPi * Next01())
                );
        }

    } // struct Random

} // namespace UnityExtension