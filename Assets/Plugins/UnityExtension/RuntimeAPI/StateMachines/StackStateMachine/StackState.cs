using System;
using UnityEngine;
using UnityEngine.Events;

namespace UnityExtension
{
    [Serializable]
    public class StackStateEvent : UnityEvent<StackAction>
    {
    }


    /// <summary>
    /// 可序列化栈状态. 状态的 Enter 和 Exit 事件可序列化
    /// </summary>
    [Serializable]
    public class StackState : IStackState
    {
        [SerializeField]
        StackStateEvent _onEnter;

        [SerializeField]
        StackStateEvent _onExit;


        /// <summary>
        /// 添加或移除更新状态触发的事件
        /// </summary>
        public event Action<float> onUpdate;


        /// <summary>
        /// 添加或移除进入状态触发的事件
        /// </summary>
        public event UnityAction<StackAction> onEnter
        {
            add
            {
                if (_onEnter == null)
                {
                    _onEnter = new StackStateEvent();
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
        public event UnityAction<StackAction> onExit
        {
            add
            {
                if (_onExit == null)
                {
                    _onExit = new StackStateEvent();
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


        public void OnEnter(StackAction stackAction)
        {
            _onEnter?.Invoke(stackAction);
        }


        public void OnExit(StackAction stackAction)
        {
            _onExit?.Invoke(stackAction);
        }


        public void OnUpdate(float deltaTime)
        {
            onUpdate?.Invoke(deltaTime);
        }

    } // class StackState

} // namespace UnityExtension
