using UnityEngine;
using UnityEngine.UI;

namespace UnityExtension.Test
{
    /// <summary>
    /// 栈状态机使用示例
    /// </summary>
    public class UIState : BaseStackStateComponent
    {
        public CanvasGroup canvasGroup;
        public Image background;


        public override void OnEnter(StackAction stackAction)
        {
            if (stackAction == StackAction.Push)
            {
                gameObject.SetActive(true);
            }
            else
            {
                canvasGroup.interactable = true;
                background.color = GameSettings.instance.enabledUIColor;
            }
        }


        public override void OnExit(StackAction stackAction)
        {
            if (stackAction == StackAction.Pop)
            {
                gameObject.SetActive(false);
            }
            else
            {
                canvasGroup.interactable = false;
                background.color = GameSettings.instance.disabledUIColor;
            }
        }


        public override void OnUpdate(float deltaTime)
        {
        }
    }
}
