using UnityEngine;

namespace UnityExtension.Test
{
    /// <summary>
    /// 栈状态机使用示例
    /// </summary>
    public class UIManager : StackStateMachineComponent<BaseStackStateComponent>
    {
        [SerializeField]
        BaseStackStateComponent _defaultState;


        void Awake()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }

            PushState(_defaultState);

            //statePushed += PlayPushSound;
            //statePopped += PlayPopSound;
        }


        void Update()
        {
            OnUpdate(Time.unscaledDeltaTime);
        }
    }
}