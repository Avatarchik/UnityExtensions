using UnityEngine;
using UnityEditor;

namespace UnityExtension.Editor
{
    class MeasuringWindow : EditorWindow
    {
        static Transform _startTrans;
        static Vector3 _startPos;
        static Transform _endTrans;
        static Vector3 _endPos = new Vector3(1, 1, 1);

        static bool _showXYZ = true;
        static bool _showYZ;
        static bool _showXZ;
        static bool _showXY;

        static bool _showMoveTools = true;


        [MenuItem("Window/Unity Extension/Measuring")]
        static void ShowWindow()
        {
            var window = GetWindow<MeasuringWindow>("Measuring");
            window.autoRepaintOnSceneChange = true;

            window.ShowUtility();
        }


        void OnEnable()
        {
            SceneView.onSceneGUIDelegate += OnSceneGUI;
        }


        void OnDisable()
        {
            SceneView.onSceneGUIDelegate -= OnSceneGUI;
            Tools.hidden = false;
        }


        void OnGUI()
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Start", EditorStyles.boldLabel);

            var rect = EditorGUILayout.GetControlRect();
            rect.width -= 8 + rect.height * 3;
            _startTrans = EditorGUI.ObjectField(rect, GUIContent.none, _startTrans, typeof(Transform), true) as Transform;

            rect.x = rect.xMax + 8;
            rect.width = rect.height * 1.5f;
            if (GUI.Button(rect, EditorUtilities.TempContent("S", null, "Use selection"), EditorStyles.miniButtonLeft)) _startTrans = Selection.activeTransform;

            rect.x = rect.xMax;
            if (GUI.Button(rect, EditorUtilities.TempContent("C", null, "Clear reference"), EditorStyles.miniButtonRight)) _startTrans = null;

            if (_startTrans)
            {
                using (var scope = new ChangeCheckScope(_startTrans))
                {
                    _startPos = EditorGUILayout.Vector3Field(GUIContent.none, _startTrans.position);
                    if (scope.changed) _startTrans.position = _startPos;
                }
            }
            else
            {
                _startPos = EditorGUILayout.Vector3Field(GUIContent.none, _startPos);
            }

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("End", EditorStyles.boldLabel);

            rect = EditorGUILayout.GetControlRect();
            rect.width -= 8 + rect.height * 3;
            _endTrans = EditorGUI.ObjectField(rect, GUIContent.none, _endTrans, typeof(Transform), true) as Transform;

            rect.x = rect.xMax + 8;
            rect.width = rect.height * 1.5f;
            if (GUI.Button(rect, EditorUtilities.TempContent("S", null, "Use selection"), EditorStyles.miniButtonLeft)) _endTrans = Selection.activeTransform;

            rect.x = rect.xMax;
            if (GUI.Button(rect, EditorUtilities.TempContent("C", null, "Clear reference"), EditorStyles.miniButtonRight)) _endTrans = null;

            if (_endTrans)
            {
                using (var scope = new ChangeCheckScope(_endTrans))
                {
                    _endPos = EditorGUILayout.Vector3Field(GUIContent.none, _endTrans.position);
                    if (scope.changed) _endTrans.position = _endPos;
                }
            }
            else
            {
                _endPos = EditorGUILayout.Vector3Field(GUIContent.none, _endPos);
            }

            Vector3 distance = _endPos - _startPos;

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Distance", EditorStyles.boldLabel);
            EditorGUILayout.FloatField(GUIContent.none, distance.magnitude);

            using (new HorizontalLayoutScope(0))
            {
                using (new VerticalLayoutScope(0))
                {
                    using (new LabelWidthScope(20))
                    {
                        EditorGUILayout.FloatField("YZ", distance.yz().magnitude);
                        EditorGUILayout.FloatField("XZ", distance.xz().magnitude);
                        EditorGUILayout.FloatField("XY", distance.xy().magnitude);
                    }
                }

                EditorGUILayout.Space();

                using (new VerticalLayoutScope(0))
                {
                    using (new LabelWidthScope(14))
                    {
                        EditorGUILayout.FloatField("X", distance.x);
                        EditorGUILayout.FloatField("Y", distance.y);
                        EditorGUILayout.FloatField("Z", distance.z);
                    }
                }
            }

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Visualization", EditorStyles.boldLabel);
            _showXYZ = GUILayout.Toggle(_showXYZ, "Show XYZ 3D", EditorStyles.miniButton);
            _showYZ = GUILayout.Toggle(_showYZ, "Show YZ Plane", EditorStyles.miniButton);
            _showXZ = GUILayout.Toggle(_showXZ, "Show XZ Plane", EditorStyles.miniButton);
            _showXY = GUILayout.Toggle(_showXY, "Show XY Plane", EditorStyles.miniButton);
            EditorGUILayout.Space();
            _showMoveTools = GUILayout.Toggle(_showMoveTools, "Show Move Tools", EditorStyles.miniButton);
            Tools.hidden = !GUILayout.Toggle(!Tools.hidden, "Show Unity Tools", EditorStyles.miniButton);
        }


        void OnSceneGUI(SceneView scene)
        {
            if (_showMoveTools)
            {
                if (_startTrans)
                {
                    using (var scope = new ChangeCheckScope(_startTrans))
                    {
                        _startPos = Handles.PositionHandle(_startTrans.position, Tools.pivotRotation == PivotRotation.Local ? _startTrans.rotation : Quaternion.identity);
                        if (scope.changed) _startTrans.position = _startPos;
                    }
                }
                else
                {
                    using (var scope = new ChangeCheckScope(null))
                    {
                        _startPos = Handles.PositionHandle(_startPos, Quaternion.identity);
                        if (scope.changed) Repaint();
                    }
                }

                if (_endTrans)
                {
                    using (var scope = new ChangeCheckScope(_endTrans))
                    {
                        _endPos = Handles.PositionHandle(_endTrans.position, Tools.pivotRotation == PivotRotation.Local ? _endTrans.rotation : Quaternion.identity);
                        if (scope.changed) _endTrans.position = _endPos;
                    }
                }
                else
                {
                    using (var scope = new ChangeCheckScope(null))
                    {
                        _endPos = Handles.PositionHandle(_endPos, Quaternion.identity);
                        if (scope.changed) Repaint();
                    }
                }
            }

            Vector3 distance = _endPos - _startPos;
            Vector3 temp;

            if (_showYZ)
            {
                temp = new Vector3(_endPos.x, _startPos.y, _startPos.z);

                GUI.contentColor = Handles.color = new Color(1f, 0.4f, 0.4f);
                Handles.DrawLine(_startPos, temp);
                Handles.Label((_startPos + temp) * 0.5f, "X: " + Mathf.Abs(distance.x).ToString(), EditorStyles.whiteBoldLabel);

                GUI.contentColor = Handles.color = new Color(0.3f, 0.9f, 0.9f);
                Handles.DrawLine(_endPos, temp);
                Handles.Label((_endPos + temp) * 0.5f, "YZ: " + distance.yz().magnitude.ToString(), EditorStyles.whiteBoldLabel);
            }

            if (_showXZ)
            {
                temp = new Vector3(_startPos.x, _endPos.y, _startPos.z);

                GUI.contentColor = Handles.color = new Color(0.4f, 0.9f, 0.4f);
                Handles.DrawLine(_startPos, temp);
                Handles.Label((_startPos + temp) * 0.5f, "Y: " + Mathf.Abs(distance.y).ToString(), EditorStyles.whiteBoldLabel);

                GUI.contentColor = Handles.color = new Color(1f, 0.4f, 1f);
                Handles.DrawLine(_endPos, temp);
                Handles.Label((_endPos + temp) * 0.5f, "XZ: " + distance.xz().magnitude.ToString(), EditorStyles.whiteBoldLabel);
            }

            if (_showXY)
            {
                temp = new Vector3(_startPos.x, _startPos.y, _endPos.z);

                GUI.contentColor = Handles.color = new Color(0.4f, 0.7f, 1f);
                Handles.DrawLine(_startPos, temp);
                Handles.Label((_startPos + temp) * 0.5f, "Z: " + Mathf.Abs(distance.z).ToString(), EditorStyles.whiteBoldLabel);

                GUI.contentColor = Handles.color = new Color(0.85f, 0.85f, 0.3f);
                Handles.DrawLine(_endPos, temp);
                Handles.Label((_endPos + temp) * 0.5f, "XY: " + distance.xy().magnitude.ToString(), EditorStyles.whiteBoldLabel);
            }

            if (_showXYZ)
            {
                GUI.contentColor = Handles.color = Color.white;
                Handles.DrawLine(_startPos, _endPos);
                Handles.Label((_startPos + _endPos) * 0.5f, "XYZ: " + distance.magnitude.ToString(), EditorStyles.whiteBoldLabel);
            }
        }

    } // class MeasuringWindow

} // namespace UnityExtension.Editor
