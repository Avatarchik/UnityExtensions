using System;
using System.Collections;
using UnityEngine;

namespace UnityExtension
{
    /// <summary>
    /// Unity Application 实用工具
    /// </summary>
    public partial struct Utilities
    {
        static GameObject _globalGameObject;
        static GlobalComponent _globalComponent;


        /// <summary>
        /// 全局游戏对象. 不可见, 不会保存, 不可编辑, 不可卸载. 不要尝试 Destroy 它, 否则世界会坏掉
        /// </summary>
        static GameObject globalGameObject
        {
            get
            {
                if (!_globalGameObject)
                {
                    _globalGameObject = new GameObject("GlobalGameObject");
#if UNITY_EDITOR
                    if (Application.isPlaying)
#endif
                        GameObject.DontDestroyOnLoad(_globalGameObject);

                    _globalGameObject.hideFlags =
                        HideFlags.HideInHierarchy
                        | HideFlags.HideInInspector
                        | HideFlags.DontSaveInEditor
                        | HideFlags.DontSaveInBuild
                        | HideFlags.DontUnloadUnusedAsset;
                }
                return _globalGameObject;
            }
        }


        /// <summary>
        /// 全局组件，用以方便的添加/移除更新事件
        /// </summary>
        static GlobalComponent globalComponent
        {
            get
            {
                if (!_globalComponent)
                {
                    _globalComponent = globalGameObject.AddComponent<GlobalComponent>();
                }
                return _globalComponent;
            }
        }


        /// <summary>
        /// 添加全局组件
        /// </summary>
        public static T AddGlobalComponent<T>() where T : Component
        {
            return globalGameObject.AddComponent<T>();
        }


        public static event Action fixedUpdate;
        public static event Action waitForFixedUpdate;
        public static event Action update;
        public static event Action lateUpdate;
        public static event Action waitForEndOfFrame;


        public static void AddUpdate(UpdateMode mode, Action action)
        {
            switch (mode)
            {
                case UpdateMode.FixedUpdate: fixedUpdate += action; return;
                case UpdateMode.WaitForFixedUpdate: waitForFixedUpdate += action; return;
                case UpdateMode.Update: update += action; return;
                case UpdateMode.LateUpdate: lateUpdate += action; return;
                case UpdateMode.WaitForEndOfFrame: waitForEndOfFrame += action; return;
            }
        }


        public static void RemoveUpdate(UpdateMode mode, Action action)
        {
            switch (mode)
            {
                case UpdateMode.FixedUpdate: fixedUpdate -= action; return;
                case UpdateMode.WaitForFixedUpdate: waitForFixedUpdate -= action; return;
                case UpdateMode.Update: update -= action; return;
                case UpdateMode.LateUpdate: lateUpdate -= action; return;
                case UpdateMode.WaitForEndOfFrame: waitForEndOfFrame -= action; return;
            }
        }


        [ExecuteInEditMode]
        public class GlobalComponent : ScriptableComponent
        {
            WaitForFixedUpdate _waitForFixedUpdate;
            WaitForEndOfFrame _waitForEndOfFrame;


            void Start()
            {
                _waitForFixedUpdate = new WaitForFixedUpdate();
                StartCoroutine(WaitForFixedUpdate());

                _waitForEndOfFrame = new WaitForEndOfFrame();
                StartCoroutine(WaitForEndOfFrame());
            }


            void FixedUpdate()
            {
                fixedUpdate?.Invoke();
            }


            IEnumerator WaitForFixedUpdate()
            {
                while (true)
                {
                    yield return _waitForFixedUpdate;
                    waitForFixedUpdate?.Invoke();
                }
            }


            void Update()
            {
                update?.Invoke();
            }


            void LateUpdate()
            {
                lateUpdate?.Invoke();
            }


            IEnumerator WaitForEndOfFrame()
            {
                while (true)
                {
                    yield return _waitForEndOfFrame;
                    waitForEndOfFrame?.Invoke();
                }
            }

        } // class GlobalComponent

    } // struct Utilities

} // namespace UnityExtension