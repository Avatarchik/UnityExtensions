using UnityEngine;

namespace UnityExtensions.Test
{
    /// <summary>
    /// ScriptableAssetSingleton 与 Attributes 示例
    /// </summary>
    public class GameSettings : ScriptableAssetSingleton<GameSettings>
    {
        [MinMax(0, 1024)]
        public int textureSize;

        [Direction]
        public Vector3 direction;

        [EulerAngles]
        public Quaternion rotation;

        [GetSet("degrees")]
        public float radians;

        [AxisUsage(AxisUsage.Direction6)]
        public Axis defaultGravity;

        public Color enabledUIColor;
        public Color disabledUIColor;


        public float degrees
        {
            get { return radians * Mathf.Rad2Deg; }
            set { radians = value * Mathf.Deg2Rad; }
        }


#if UNITY_EDITOR

        [UnityEditor.MenuItem("Assets/Create/Unity Extensions/Test/Game Settings")]
        static void CreateAsset()
        {
            CreateOrSelectAsset();
        }

#endif
    }
}