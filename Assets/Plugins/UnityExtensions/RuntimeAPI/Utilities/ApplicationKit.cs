using System;
using System.Collections;
using UnityEngine;

namespace UnityExtensions
{
    /// <summary>
    /// Unity Application 工具箱
    /// </summary>
    public struct ApplicationKit
    {
        static GameObject _globalGameObject;
        static GlobalComponent _globalComponent;


        [RuntimeInitializeOnLoadMethod]
#if UNITY_EDITOR
        [UnityEditor.InitializeOnLoadMethod]
#endif
        static void Initialize()
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

            if (!_globalComponent)
            {
                _globalComponent = _globalGameObject.AddComponent<GlobalComponent>();
            }
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


        /// <summary>
        /// 添加全局组件
        /// </summary>
        public static T AddGlobalComponent<T>() where T : Component
        {
            return _globalGameObject.AddComponent<T>();
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

    } // struct ApplicationKit

} // namespace UnityExtensions