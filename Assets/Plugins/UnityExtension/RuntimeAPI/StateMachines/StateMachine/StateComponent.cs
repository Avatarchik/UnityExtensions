using System;
using UnityEngine;
using UnityEngine.Events;

namespace UnityExtension
{
    /// <summary>
    /// 状态组件. 状态的 Enter 和 Exit 事件可序列化
    /// </summary>
    public class StateComponent : ScriptableComponent, IState
    {
        [SerializeField]
        UnityEvent _onEnter;

        [SerializeField]
        UnityEvent _onExit;


        /// <summary>
        /// 添加或移除更新状态触发的事件
        /// </summary>
        public event Action<float> onUpdate;


        /// <summary>
        /// 添加或移除进入状态触发的事件
        /// </summary>
        public event UnityAction onEnter
        {
            add
            {
                if (_onEnter == null)
                {
                    _onEnter = new UnityEvent();
                }
                _onEnter.AddListener(value);
            }
            remove
            {
                if (_onEnter != null)
                {
                    _onEnter.RemoveListener(value);
                }
            }
        }


        /// <summary>
        /// 添加或移除离开状态触发的事件
        /// </summary>
        public event UnityAction onExit
        {
            add
            {
                if (_onExit == null)
                {
                    _onExit = new UnityEvent();
                }
                _onExit.AddListener(value);
            }
            remove
            {
                if (_onExit != null)
                {
                    _onExit.RemoveListener(value);
                }
            }
        }


        public void OnEnter()
        {
            _onEnter?.Invoke();
        }


        public void OnExit()
        {
            _onExit?.Invoke();
        }


        public void OnUpdate(float deltaTime)
        {
            onUpdate?.Invoke(deltaTime);
        }

    } // class StateComponent

} // namespace UnityExtension
