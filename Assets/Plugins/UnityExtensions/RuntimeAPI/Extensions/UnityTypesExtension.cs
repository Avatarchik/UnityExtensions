using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UnityExtensions
{
    /// <summary>
    /// Unity 内置类型扩展
    /// </summary>
    public static partial class Extension
    {
        /// <summary>
        /// 安全获取组件. 如果物体上没有组件则自动添加
        /// </summary>
        public static T GetComponentSafely<T>(this GameObject target) where T : Component
        {
            T component = target.GetComponent<T>();
            if (!component) component = target.AddComponent<T>();
            return component;
        }


        /// <summary>
        /// 安全获取组件. 如果物体上没有组件则自动添加
        /// </summary>
        public static T GetComponentSafely<T>(this Component target) where T : Component
        {
            T component = target.GetComponent<T>();
            if (!component) component = target.gameObject.AddComponent<T>();
            return component;
        }


        /// <summary>
        /// 访问 UI 游戏对象的 RectTransform 组件
        /// </summary>
        public static RectTransform rectTransform(this Component target)
        {
            return target.transform as RectTransform;
        }


        /// <summary>
        /// 访问 UI 游戏对象的 RectTransform 组件
        /// </summary>
        public static RectTransform rectTransform(this GameObject target)
        {
            return target.transform as RectTransform;
        }


        /// <summary>
        /// 延时调用指定的方法
        /// </summary>
        /// <param name="behaviour"> 协程附着的脚本对象 </param>
        /// <param name="delay"> 延迟时间(秒) </param>
        /// <param name="action"> 延时结束调用的方法 </param>
        public static void DelayInvoking(this MonoBehaviour behaviour, float delay, Action action)
        {
            behaviour.StartCoroutine(DelayedCoroutine());

            IEnumerator DelayedCoroutine()
            {
                yield return new WaitForSeconds(delay);
                action();
            }
        }


        /// <summary>
        /// 重置 Transform 的 localPosition, localRotation 和 localScale
        /// </summary>
        public static void ResetLocal(this Transform transform)
        {
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;
        }


        /// <summary>
        /// 遍历 Transform 层级（根节点优先）, 对每一个节点执行一个自定义的操作
        /// </summary>
        /// <param name="root"> 遍历开始的根部 Transform 对象 </param>
        /// <param name="operate"> 遍历到每一个节点时将调用此方法 </param>
        /// <param name="depthLimit"> 访问深度限制, 负值表示不限制, 0 表示只访问 root 本身而不访问其子级, 正值表示最多访问的子级层数 </param>
        public static void TraverseHierarchy(this Transform root, Action<Transform> operate, int depthLimit = -1)
        {
            operate(root);

            if (depthLimit != 0)
            {
                int count = root.childCount;
                for (int i = 0; i < count; i++)
                {
                    TraverseHierarchy(root.GetChild(i), operate, depthLimit - 1);
                }
            }
        }


        /// <summary>
        /// 反向遍历 Transform 层级（叶子节点优先）, 对每一个节点执行一个自定义的操作
        /// </summary>
        /// <param name="root"> 树的根部 Transform 对象 </param>
        /// <param name="operate"> 遍历到每一个节点时将调用此方法 </param>
        /// <param name="depthLimit"> 访问深度限制, 负值表示不限制, 0 表示只访问 root 本身而不访问其子级, 正值表示最多访问的子级层数 </param>
        public static void InverseTraverseHierarchy(this Transform root, Action<Transform> operate, int depthLimit = -1)
        {
            if (depthLimit != 0)
            {
                int count = root.childCount;
                for (int i = 0; i < count; i++)
                {
                    InverseTraverseHierarchy(root.GetChild(i), operate, depthLimit - 1);
                }
            }

            operate(root);
        }


        /// <summary>
        /// 遍历 Transform 层级（根节点优先）, 判断每一个节点是否为查找目标, 发现查找目标则立即终止查找
        /// </summary>
        /// <param name="root"> 遍历开始的根部 Transform 对象 </param>
        /// <param name="match"> 判断当前节点是否为查找目标 </param>
        /// <param name="depthLimit"> 遍历深度限制, 负值表示不限制, 0 表示只访问 root 本身而不访问其子级, 正值表示最多访问的子级层数 </param>
        /// <returns> 如果查找到目标则返回此目标; 否则返回 null </returns>
        public static Transform SearchHierarchy(this Transform root, Predicate<Transform> match, int depthLimit = -1)
        {
            if (match(root)) return root;
            if (depthLimit == 0) return null;

            int count = root.childCount;
            Transform result = null;

            for (int i = 0; i < count; i++)
            {
                result = SearchHierarchy(root.GetChild(i), match, depthLimit - 1);
                if (result) break;
            }

            return result;
        }


        public static Rect GetWorldRect(this RectTransform rectTransform)
        {
            var rect = rectTransform.rect;
            rect.min = rectTransform.TransformPoint(rect.min);
            rect.max = rectTransform.TransformPoint(rect.max);
            return rect;
        }


        public static Vector2 xy(this Vector3 v)
        {
            return new Vector2(v.x, v.y);
        }


        public static Vector2 yz(this Vector3 v)
        {
            return new Vector2(v.y, v.z);
        }


        public static Vector2 xz(this Vector3 v)
        {
            return new Vector2(v.x, v.z);
        }


        /// <summary>
        /// 复制 AnimationCurve 实例
        /// </summary>
        public static AnimationCurve Clone(this AnimationCurve target)
        {
            var newCurve = new AnimationCurve(target.keys);
            newCurve.postWrapMode = target.postWrapMode;
            newCurve.preWrapMode = target.preWrapMode;

            return newCurve;
        }


        /// <summary>
        /// 复制 Gradient 实例
        /// </summary>
        public static Gradient Clone(this Gradient target)
        {
            var newGradient = new Gradient();
            newGradient.alphaKeys = target.alphaKeys;
            newGradient.colorKeys = target.colorKeys;
            newGradient.mode = target.mode;

            return newGradient;
        }


        /// <summary>
        /// 将屏幕尺寸转化为世界尺寸
        /// </summary>
        public static float ScreenToWorldSize(this Camera camera, float pixelSize, float clipPlane)
        {
            if (camera.orthographic)
            {
                return pixelSize * camera.orthographicSize * 2f / camera.pixelHeight;
            }
            else
            {
                return pixelSize * clipPlane * Mathf.Tan(camera.fieldOfView * 0.5f * Mathf.Deg2Rad) * 2f / camera.pixelHeight;
            }
        }


        /// <summary>
        /// 将世界尺寸转化为屏幕尺寸
        /// </summary>
        public static float WorldToScreenSize(this Camera camera, float worldSize, float clipPlane)
        {
            if (camera.orthographic)
            {
                return worldSize * camera.pixelHeight * 0.5f / camera.orthographicSize;
            }
            else
            {
                return worldSize * camera.pixelHeight * 0.5f / (clipPlane * Mathf.Tan(camera.fieldOfView * 0.5f * Mathf.Deg2Rad));
            }
        }


        /// <summary>
        /// 将世界平面转化为相机裁剪面
        /// </summary>
        public static Vector4 GetClipPlane(this Camera camera, Vector3 point, Vector3 normal)
        {
            Matrix4x4 wtoc = camera.worldToCameraMatrix;
            point = wtoc.MultiplyPoint(point);
            normal = wtoc.MultiplyVector(normal).normalized;

            return new Vector4(normal.x, normal.y, normal.z, -Vector3.Dot(point, normal));
        }


        /// <summary>
        /// 计算 ZBufferParams, 可用于 compute shader 
        /// </summary>
        public static Vector4 GetZBufferParams(this Camera camera)
        {
            double f = camera.farClipPlane;
            double n = camera.nearClipPlane;

            double rn = 1f / n;
            double rf = 1f / f;
            double fpn = f / n;

            return SystemInfo.usesReversedZBuffer
                ? new Vector4((float)(fpn - 1.0), 1f, (float)(rn - rf), (float)rf)
                : new Vector4((float)(1.0 - fpn), (float)fpn, (float)(rf - rn), (float)rn);
        }


        /// <summary>
        /// 为 EventTrigger 添加事件
        /// </summary>
        public static void AddListener(this EventTrigger eventTrigger, EventTriggerType type, UnityAction<BaseEventData> callback)
        {
            var triggers = eventTrigger.triggers;
            var index = triggers.FindIndex(entry => entry.eventID == type);
            if (index < 0)
            {
                var entry = new EventTrigger.Entry();
                entry.eventID = type;
                entry.callback.AddListener(callback);
                triggers.Add(entry);
            }
            else
            {
                triggers[index].callback.AddListener(callback);
            }
        }


        /// <summary>
        /// 为 EventTrigger 移除事件
        /// </summary>
        public static void RemoveListener(this EventTrigger eventTrigger, EventTriggerType type, UnityAction<BaseEventData> callback)
        {
            var triggers = eventTrigger.triggers;
            var index = triggers.FindIndex(entry => entry.eventID == type);
            if (index >= 0)
            {
                triggers[index].callback.RemoveListener(callback);
            }
        }

    } // class Extension

} // namespace UnityExtensions