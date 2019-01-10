using UnityEngine;
using UnityEditor;

namespace UnityExtension.Editor
{
    class MeasuringWindow : EditorWindow
    {
        class Data : EditorScriptableSingleton<Data>
        {
            public Transform startTrans;
            public Vector3 startPos;
            public Transform endTrans;
            public Vector3 endPos = new Vector3(2, 3, 4);

            public bool showXYZ = true;
            public bool showYZ;
            public bool showXZ;
            public bool showXY;
            
            public bool showMoveTools = true;
        }

        Data d;


        [MenuItem("Window/Unity Extension/Measuring")]
        static void ShowWindow()
        {
            var window = GetWindow<MeasuringWindow>("Measuring");
            window.autoRepaintOnSceneChange = true;

            window.ShowUtility();
        }


        void OnEnable()
        {
            d = Data.instance;
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

            using (var scope = new ChangeCheckScope(d))
            {
                var rect = EditorGUILayout.GetControlRect();
                rect.width -= 8 + rect.height * 3;
                var startTrans = EditorGUI.ObjectField(rect, GUIContent.none, d.startTrans, typeof(Transform), true) as Transform;

                rect.x = rect.xMax + 8;
                rect.width = rect.height * 1.5f;
                if (GUI.Button(rect, EditorUtilities.TempContent("S", null, "Use selection"), EditorStyles.miniButtonLeft)) startTrans = Selection.activeTransform;

                rect.x = rect.xMax;
                if (GUI.Button(rect, EditorUtilities.TempContent("C", null, "Clear reference"), EditorStyles.miniButtonRight)) startTrans = null;

                if (scope.changed) d.startTrans = startTrans;
            }

            if (d.startTrans)
            {
                using (var scope = new ChangeCheckScope(d.startTrans))
                {
                    d.startPos = EditorGUILayout.Vector3Field(GUIContent.none, d.startTrans.position);
                    if (scope.changed) d.startTrans.position = d.startPos;
                }
            }
            else
            {
                using (var scope = new ChangeCheckScope(d))
                {
                    var startPos = EditorGUILayout.Vector3Field(GUIContent.none, d.startPos);
                    if (scope.changed) d.startPos = startPos;
                }
            }

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("End", EditorStyles.boldLabel);

            using (var scope = new ChangeCheckScope(d))
            {
                var rect = EditorGUILayout.GetControlRect();
                rect.width -= 8 + rect.height * 3;
                var endTrans = EditorGUI.ObjectField(rect, GUIContent.none, d.endTrans, typeof(Transform), true) as Transform;

                rect.x = rect.xMax + 8;
                rect.width = rect.height * 1.5f;
                if (GUI.Button(rect, EditorUtilities.TempContent("S", null, "Use selection"), EditorStyles.miniButtonLeft)) endTrans = Selection.activeTransform;

                rect.x = rect.xMax;
                if (GUI.Button(rect, EditorUtilities.TempContent("C", null, "Clear reference"), EditorStyles.miniButtonRight)) endTrans = null;

                if (scope.changed) d.endTrans = endTrans;
            }

            if (d.endTrans)
            {
                using (var scope = new ChangeCheckScope(d.endTrans))
                {
                    d.endPos = EditorGUILayout.Vector3Field(GUIContent.none, d.endTrans.position);
                    if (scope.changed) d.endTrans.position = d.endPos;
                }
            }
            else
            {
                using (var scope = new ChangeCheckScope(d))
                {
                    var endPos = EditorGUILayout.Vector3Field(GUIContent.none, d.endPos);
                    if (scope.changed) d.endPos = endPos;
                }
            }

            Vector3 distance = d.endPos - d.startPos;

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
            d.showXYZ = GUILayout.Toggle(d.showXYZ, "Show XYZ 3D", EditorStyles.miniButton);
            d.showYZ = GUILayout.Toggle(d.showYZ, "Show YZ Plane", EditorStyles.miniButton);
            d.showXZ = GUILayout.Toggle(d.showXZ, "Show XZ Plane", EditorStyles.miniButton);
            d.showXY = GUILayout.Toggle(d.showXY, "Show XY Plane", EditorStyles.miniButton);
            EditorGUILayout.Space();
            d.showMoveTools = GUILayout.Toggle(d.showMoveTools, "Show Move Tools", EditorStyles.miniButton);
            Tools.hidden = !GUILayout.Toggle(!Tools.hidden, "Show Unity Tools", EditorStyles.miniButton);
        }


        void OnSceneGUI(SceneView scene)
        {
            if (d.showMoveTools)
            {
                if (d.startTrans)
                {
                    using (var scope = new ChangeCheckScope(d.startTrans))
                    {
                        d.startPos = Handles.PositionHandle(d.startTrans.position, Tools.pivotRotation == PivotRotation.Local ? d.startTrans.rotation : Quaternion.identity);
                        if (scope.changed) d.startTrans.position = d.startPos;
                    }
                }
                else
                {
                    using (var scope = new ChangeCheckScope(d))
                    {
                        var startPos = Handles.PositionHandle(d.startPos, Quaternion.identity);
                        if (scope.changed)
                        {
                            d.startPos = startPos;
                            Repaint();
                        }
                    }
                }

                if (d.endTrans)
                {
                    using (var scope = new ChangeCheckScope(d.endTrans))
                    {
                        d.endPos = Handles.PositionHandle(d.endTrans.position, Tools.pivotRotation == PivotRotation.Local ? d.endTrans.rotation : Quaternion.identity);
                        if (scope.changed) d.endTrans.position = d.endPos;
                    }
                }
                else
                {
                    using (var scope = new ChangeCheckScope(d))
                    {
                        var endPos = Handles.PositionHandle(d.endPos, Quaternion.identity);
                        if (scope.changed)
                        {
                            d.endPos = endPos;
                            Repaint();
                        }
                    }
                }
            }

            Vector3 distance = d.endPos - d.startPos;
            Vector3 temp;
            float length;

            if (d.showYZ)
            {
                temp = new Vector3(d.endPos.x, d.startPos.y, d.startPos.z);

                length = Mathf.Abs(distance.x);
                if (length > Mathf.Epsilon)
                {
                    GUI.contentColor = Handles.color = new Color(1f, 0.4f, 0.4f);
                    Handles.DrawLine(d.startPos, temp);
                    Handles.Label((d.startPos + temp) * 0.5f, "X: " + length, EditorStyles.whiteBoldLabel);
                }

                length = distance.yz().magnitude;
                if (length > Mathf.Epsilon)
                {
                    GUI.contentColor = Handles.color = new Color(0.3f, 0.9f, 0.9f);
                    Handles.DrawLine(d.endPos, temp);
                    Handles.Label((d.endPos + temp) * 0.5f, "YZ: " + length, EditorStyles.whiteBoldLabel);
                }
            }

            if (d.showXZ)
            {
                temp = new Vector3(d.startPos.x, d.endPos.y, d.startPos.z);

                length = Mathf.Abs(distance.y);
                if (length > Mathf.Epsilon)
                {
                    GUI.contentColor = Handles.color = new Color(0.4f, 0.9f, 0.4f);
                    Handles.DrawLine(d.startPos, temp);
                    Handles.Label((d.startPos + temp) * 0.5f, "Y: " + length, EditorStyles.whiteBoldLabel);
                }

                length = distance.xz().magnitude;
                if (length > Mathf.Epsilon)
                {
                    GUI.contentColor = Handles.color = new Color(1f, 0.4f, 1f);
                    Handles.DrawLine(d.endPos, temp);
                    Handles.Label((d.endPos + temp) * 0.5f, "XZ: " + length, EditorStyles.whiteBoldLabel);
                }
            }

            if (d.showXY)
            {
                temp = new Vector3(d.startPos.x, d.startPos.y, d.endPos.z);

                length = Mathf.Abs(distance.z);
                if (length > Mathf.Epsilon)
                {
                    GUI.contentColor = Handles.color = new Color(0.4f, 0.7f, 1f);
                    Handles.DrawLine(d.startPos, temp);
                    Handles.Label((d.startPos + temp) * 0.5f, "Z: " + length, EditorStyles.whiteBoldLabel);
                }

                length = distance.xy().magnitude;
                if (length > Mathf.Epsilon)
                {
                    GUI.contentColor = Handles.color = new Color(0.85f, 0.85f, 0.3f);
                    Handles.DrawLine(d.endPos, temp);
                    Handles.Label((d.endPos + temp) * 0.5f, "XY: " + length, EditorStyles.whiteBoldLabel);
                }
            }

            if (d.showXYZ)
            {
                length = distance.magnitude;
                if (length > Mathf.Epsilon)
                {
                    GUI.contentColor = Handles.color = Color.white;
                    Handles.DrawLine(d.startPos, d.endPos);
                    Handles.Label((d.startPos + d.endPos) * 0.5f, "XYZ: " + length, EditorStyles.whiteBoldLabel);
                }
            }
        }

    } // class MeasuringWindow

} // namespace UnityExtension.Editor
