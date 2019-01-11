using UnityEngine;

namespace UnityExtensions.Test
{
    /// <summary>
    /// 栈状态机使用示例
    /// </summary>
    public class UIManager : StackStateMachineComponent<BaseStackStateComponent>
    {
        [SerializeField]
        BaseStackStateComponent _defaultState;

        [SerializeField]
        RectTransform _border;

        [SerializeField]
        float _tweenDuration;


        // Tween
        Rect _start;
        Rect _target;
        float _progress;


        void Awake()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                var stateTrans = transform.GetChild(i);
                if (stateTrans.GetComponent<BaseStackStateComponent>())
                {
                    stateTrans.gameObject.SetActive(false);
                }
            }

            PushState(_defaultState);
            SetBorderWorldRect(_defaultState.rectTransform().GetWorldRect());
            _progress = 1f;
        }


        void Update()
        {
            OnUpdate(Time.unscaledDeltaTime);

            if (_progress < 1f)
            {
                _progress = Mathf.Clamp01(_progress + Time.unscaledDeltaTime / _tweenDuration);
                SetBorderWorldRect(MathKit.Lerp(_start, _target, Mathf.SmoothStep(0,1,_progress)));
            }
        }


        void SetBorderWorldRect(Rect rect)
        {
            rect.min = RectTransformUtility.WorldToScreenPoint(null, rect.min);
            rect.max = RectTransformUtility.WorldToScreenPoint(null, rect.max);

            _border.offsetMin = rect.min;
            _border.offsetMax = rect.min + rect.size / _border.lossyScale;
        }


        void StartTween(BaseStackStateComponent targetState)
        {
            _start = _border.GetWorldRect();
            _target = targetState.rectTransform().GetWorldRect();
            _progress = 0f;
        }


        protected override void OnStatePopped(BaseStackStateComponent poppedState)
        {
            StartTween(currentState);
        }


        protected override void OnStatePushed(BaseStackStateComponent pushedState)
        {
            StartTween(pushedState);
        }
    }
}