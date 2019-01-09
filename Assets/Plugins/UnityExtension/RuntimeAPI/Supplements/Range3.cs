using System;
using UnityEngine;

namespace UnityExtension
{
    /// <summary>
    /// 三维范围
    /// </summary>
    [Serializable]
    public struct Range3
    {
        public Range x;
        public Range y;
        public Range z;


        public Range3(Range x, Range y, Range z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }


        public Range3(Vector3 min, Vector3 max)
        {
            x.min = min.x;
            x.max = max.x;
            y.min = min.y;
            y.max = max.y;
            z.min = min.z;
            z.max = max.z;
        }


        /// <summary>
        /// min 顶点
        /// </summary>
        public Vector3 min
        {
            get { return new Vector3(x.min, y.min, z.min); }
            set
            {
                x.min = value.x;
                y.min = value.y;
                z.min = value.z;
            }
        }


        /// <summary>
        /// max 顶点
        /// </summary>
        public Vector3 max
        {
            get { return new Vector3(x.max, y.max, z.max); }
            set
            {
                x.max = value.x;
                y.max = value.y;
                z.max = value.z;
            }
        }


        /// <summary>
        /// 大小
        /// 修改大小时维持中心不变
        /// </summary>
        public Vector3 size
        {
            get { return new Vector3(x.size, y.size, z.size); }
            set
            {
                x.size = value.x;
                y.size = value.y;
                z.size = value.z;
            }
        }


        /// <summary>
        /// 中心
        /// 修改中心时维持大小不变
        /// </summary>
        public Vector3 center
        {
            get { return new Vector3(x.center, y.center, z.center); }
            set
            {
                x.center = value.x;
                y.center = value.y;
                z.center = value.z;
            }
        }


        /// <summary>
        /// 确保最小最大值关系正确
        /// </summary>
        public void SortMinMax()
        {
            x.SortMinMax();
            y.SortMinMax();
            z.SortMinMax();
        }


        /// <summary>
        /// 判断是否包含某个点
        /// </summary>
        public bool Contains(Vector3 point)
        {
            return x.Contains(point.x) && y.Contains(point.y) && z.Contains(point.z);
        }


        /// <summary>
        /// 获取范围内最接近给定点的点
        /// </summary>
        public Vector3 Closest(Vector3 point)
        {
            point.x = x.Closest(point.x);
            point.y = y.Closest(point.y);
            point.z = z.Closest(point.z);
            return point;
        }


        /// <summary>
        /// 判断是否与另一范围有交集
        /// </summary>
        public bool Intersects(Range3 other)
        {
            return x.Intersects(other.x) && y.Intersects(other.y) && z.Intersects(other.z);
        }


        /// <summary>
        /// 获取两个范围的交集
        /// </summary>
        public Range3 GetIntersection(Range3 other)
        {
            other.x = x.GetIntersection(other.x);
            other.y = y.GetIntersection(other.y);
            other.z = z.GetIntersection(other.z);
            return other;
        }


        /// <summary>
        /// 获取有符号的距离向量. 负值表示目标小于最小值, 正值表示目标大于最大值
        /// </summary>
        public Vector3 SignedDistance3(Vector3 point)
        {
            point.x = x.SignedDistance(point.x);
            point.y = y.SignedDistance(point.y);
            point.z = z.SignedDistance(point.z);
            return point;
        }


        /// <summary>
        /// 获取点相对于此范围两个轴向上的距离
        /// </summary>
        public Vector3 Distance3(Vector3 point)
        {
            point.x = x.Distance(point.x);
            point.y = y.Distance(point.y);
            point.z = z.Distance(point.z);
            return point;
        }


        /// <summary>
        /// 获取点相对于范围的距离的平方
        /// </summary>
        public float SqrDistance(Vector3 point)
        {
            return Distance3(point).sqrMagnitude;
        }


        /// <summary>
        /// 获取点相对于范围的距离
        /// </summary>
        public float Distance(Vector3 point)
        {
            return Distance3(point).magnitude;
        }


        /// <summary>
        /// 扩展以包含指定点
        /// </summary>
        public void Encapsulate(Vector3 point)
        {
            x.Encapsulate(point.x);
            y.Encapsulate(point.y);
            z.Encapsulate(point.z);
        }


        /// <summary>
        /// 扩展以包含指定范围
        /// </summary>
        public void Encapsulate(Range3 other)
        {
            x.Encapsulate(other.x);
            y.Encapsulate(other.y);
            z.Encapsulate(other.z);
        }


        /// <summary>
        /// 扩展范围, 负值代表收缩
        /// </summary>
        public void Expand(Vector3 delta)
        {
            x.Expand(delta.x);
            y.Expand(delta.y);
            z.Expand(delta.z);
        }


        /// <summary>
        /// 各方向等量扩展范围, 负值代表收缩
        /// </summary>
        public void Expand(float delta)
        {
            x.Expand(delta);
            y.Expand(delta);
            z.Expand(delta);
        }


        /// <summary>
        /// 移动范围
        /// </summary>
        public void Move(Vector3 delta)
        {
            x.Move(delta.x);
            y.Move(delta.y);
            z.Move(delta.z);
        }

    } // struct Range3

} // namespace UnityExtension