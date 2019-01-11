using UnityEngine;

namespace UnityExtensions
{
    [AddComponentMenu("Unity Extensions/Show FPS")]
    [DisallowMultipleComponent]
    public class ShowFPS : ScriptableComponent
    {
#if DEBUG

        [Range(0, 1)]
        public float refreshInterval = 0.2f;

        float _duration;
        int _frames;
        string _fps;


        void OnEnable()
        {
            _duration = 0;
            _frames = 0;
            _fps = string.Format("FPS: {0:F1}", 1f / Time.unscaledDeltaTime);
        }


        void Update()
        {
            _duration += Time.unscaledDeltaTime;
            _frames++;

            if (_duration >= refreshInterval)
            {
                _fps = string.Format("FPS: {0:F1}", _frames / _duration);

                _duration = 0;
                _frames = 0;
            }
        }


        void OnGUI()
        {
            GUI.DrawTexture(new Rect(5, Screen.height - 25, 78, 20), RenderingKit.blackTexture);

            GUI.color = Color.white;

            GUI.Label(new Rect(10, Screen.height - 25, 68, 20), _fps);
        }

#endif // DEBUG

    } // ShowFPS

} // UnityExtensions