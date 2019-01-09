using System;

namespace UnityExtension
{
    /// <summary>
    /// 一维范围
    /// </summary>
    [Serializable]
    public struct Range
    {
        /// <summary>
        /// 最小值
        /// </summary>
        public float min;


        /// <summary>
        /// 最大值
        /// </summary>
        public float max;


        /// <summary>
        /// 根据最小最大值构造范围
        /// </summary>
        public Range(float min, float max)
        {
            this.min = min;
            this.max = max;
        }


        /// <summary>
        /// 长度
        /// 修改长度时维持中心不变
        /// </summary>
        public float size
        {
            get { return max - min; }
            set
            {
                float center2 = min + max;
                min = (center2 - value) * 0.5f;
                max = (center2 + value) * 0.5f;
            }
        }


        /// <summary>
        /// 中心
        /// 修改中心时维持长度不变
        /// </summary>
        public float center
        {
            get { return (min + max) * 0.5f; }
            set
            {
                float half = (max - min) * 0.5f;
                min = value - half;
                max = value + half;
            }
        }


        /// <summary>
        /// 确保最小最大值关系正确
        /// </summary>
        public void SortMinMax()
        {
            if (min > max)
            {
                float tmp = max;
                max = min;
                min = tmp;
            }
        }


        /// <summary>
        /// 判断是否包含某个值
        /// </summary>
        public bool Contains(float value)
        {
            return value >= min && value <= max;
        }


        /// <summary>
        /// 获取范围内最接近给定值的值
        /// </summary>
        public float Closest(float value)
        {
            if (value <= min) return min;
            if (value >= max) return max;
            return value;
        }


        /// <summary>
        /// 判断是否与另一范围有交集
        /// </summary>
        public bool Intersects(Range other)
        {
            return min <= other.max && max >= other.min;
        }


        /// <summary>
        /// 获取两个范围的交集
        /// </summary>
        public Range GetIntersection(Range other)
        {
            if (min > other.min) other.min = min;
            if (max < other.max) other.max = max;
            return other;
        }


        /// <summary>
        /// 获取有符号的距离. 负值表示目标小于最小值, 正值表示目标大于最大值
        /// </summary>
        public float SignedDistance(float value)
        {
            if (value < min) return value - min;
            if (value > max) return value - max;
            return 0f;
        }


        /// <summary>
        /// 距离
        /// </summary>
        public float Distance(float value)
        {
            if (value < min) return min - value;
            if (value > max) return value - max;
            return 0f;
        }


        /// <summary>
        /// 扩展以包含指定值
        /// </summary>
        public void Encapsulate(float value)
        {
            if (value < min) min = value;
            else if (value > max) max = value;
        }


        /// <summary>
        /// 扩展以包含指定范围
        /// </summary>
        public void Encapsulate(Range other)
        {
            if (other.min < min) min = other.min;
            if (other.max > max) max = other.max;
        }


        /// <summary>
        /// 各方向等量扩展范围, 负值代表收缩
        /// </summary>
        public void Expand(float delta)
        {
            min -= delta;
            max += delta;
        }


        /// <summary>
        /// 移动范围
        /// </summary>
        public void Move(float delta)
        {
            min += delta;
            max += delta;
        }

    } // struct Range

} // namespace UnityExtension