#if UNITY_EDITOR

using System;
using UnityEngine;
using UnityEditor;

namespace UnityExtensions.Editor
{
    public struct LabelWidthScope : IDisposable
    {
        float _orginal;

        public LabelWidthScope(float value)
        {
            _orginal = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = value;
        }

        void IDisposable.Dispose()
        {
            EditorGUIUtility.labelWidth = _orginal;
        }
    }


    public struct WideModeScope : IDisposable
    {
        bool _orginal;

        public WideModeScope(bool value)
        {
            _orginal = EditorGUIUtility.wideMode;
            EditorGUIUtility.wideMode = value;
        }

        void IDisposable.Dispose()
        {
            EditorGUIUtility.wideMode = _orginal;
        }
    }


    public struct IndentScope : IDisposable
    {
        int _indent;

        public IndentScope(int indent, bool relative = true)
        {
            _indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = relative ? (_indent + indent) : indent;
        }

        void IDisposable.Dispose()
        {
            EditorGUI.indentLevel = _indent;
        }
    }


    public struct GUIContentColorScope : IDisposable
    {
        Color _orginal;

        public GUIContentColorScope(Color value)
        {
            _orginal = GUI.contentColor;
            GUI.contentColor = value;
        }

        void IDisposable.Dispose()
        {
            GUI.contentColor = _orginal;
        }
    }


    public struct GUIBackgroundColorScope : IDisposable
    {
        Color _orginal;

        public GUIBackgroundColorScope(Color value)
        {
            _orginal = GUI.backgroundColor;
            GUI.backgroundColor = value;
        }

        void IDisposable.Dispose()
        {
            GUI.backgroundColor = _orginal;
        }
    }


    public struct GUIColorScope : IDisposable
    {
        Color _orginal;

        public GUIColorScope(Color value)
        {
            _orginal = GUI.color;
            GUI.color = value;
        }

        void IDisposable.Dispose()
        {
            GUI.color = _orginal;
        }
    }


    public struct HandlesColorScope : IDisposable
    {
        Color _orginal;

        public HandlesColorScope(Color value)
        {
            _orginal = Handles.color;
            Handles.color = value;
        }

        void IDisposable.Dispose()
        {
            Handles.color = _orginal;
        }
    }


    public struct HandlesMatrixScope : IDisposable
    {
        Matrix4x4 _orginal;

        public HandlesMatrixScope(Matrix4x4 value)
        {
            _orginal = Handles.matrix;
            Handles.matrix = value;
        }

        public HandlesMatrixScope(ref Matrix4x4 value)
        {
            _orginal = Handles.matrix;
            Handles.matrix = value;
        }

        void IDisposable.Dispose()
        {
            Handles.matrix = _orginal;
        }
    }


    public struct GizmosColorScope : IDisposable
    {
        Color _orginal;

        public GizmosColorScope(Color value)
        {
            _orginal = Gizmos.color;
            Gizmos.color = value;
        }

        void IDisposable.Dispose()
        {
            Gizmos.color = _orginal;
        }
    }


    public struct GizmosMatrixScope : IDisposable
    {
        Matrix4x4 _orginal;

        public GizmosMatrixScope(Matrix4x4 value)
        {
            _orginal = Gizmos.matrix;
            Gizmos.matrix = value;
        }

        public GizmosMatrixScope(ref Matrix4x4 value)
        {
            _orginal = Gizmos.matrix;
            Gizmos.matrix = value;
        }

        void IDisposable.Dispose()
        {
            Gizmos.matrix = _orginal;
        }
    }


    public struct DisabledScope : IDisposable
    {
        public DisabledScope(bool disabled)
        {
            EditorGUI.BeginDisabledGroup(disabled);
        }

        public void Dispose()
        {
            EditorGUI.EndDisabledGroup();
        }
    }


    public struct ChangeCheckScope : IDisposable
    {
        bool _end;
        bool _changed;
        UnityEngine.Object _undoRecordTarget;

        public bool changed
        {
            get
            {
                if (!_end)
                {
                    _end = true;
                    _changed = EditorGUI.EndChangeCheck();
                    if (_changed && _undoRecordTarget)
                    {
                        Undo.RecordObject(_undoRecordTarget, _undoRecordTarget.name);
                    }
                }
                return _changed;
            }
        }

        public ChangeCheckScope(UnityEngine.Object undoRecordTarget)
        {
            _end = false;
            _changed = false;
            _undoRecordTarget = undoRecordTarget;
            EditorGUI.BeginChangeCheck();
        }

        void IDisposable.Dispose()
        {
            if (!_end)
            {
                _end = true;
                _changed = EditorGUI.EndChangeCheck();
            }
        }
    }


    public struct HandlesGUIScope : IDisposable
    {
        // 因为 C# 暂时不能自定义或移除 struct 的无参构造，导致我们必须添加一个无用的参数 :/
        public HandlesGUIScope(int useless)
        {
            Handles.BeginGUI();
        }

        void IDisposable.Dispose()
        {
            Handles.EndGUI();
        }
    }


    public struct HorizontalLayoutScope : IDisposable
    {
        // 因为 C# 暂时不能自定义或移除 struct 的无参构造，导致我们必须添加一个无用的参数 :/
        public HorizontalLayoutScope(int useless)
        {
            EditorGUILayout.BeginHorizontal();
        }

        public HorizontalLayoutScope(params GUILayoutOption[] options)
        {
            EditorGUILayout.BeginHorizontal(options);
        }

        public HorizontalLayoutScope(GUIStyle style, params GUILayoutOption[] options)
        {
            EditorGUILayout.BeginHorizontal(style, options);
        }

        void IDisposable.Dispose()
        {
            EditorGUILayout.EndHorizontal();
        }
    }


    public struct VerticalLayoutScope : IDisposable
    {
        // 因为 C# 暂时不能自定义或移除 struct 的无参构造，导致我们必须添加一个无用的参数 :/
        public VerticalLayoutScope(int useless)
        {
            EditorGUILayout.BeginVertical();
        }

        public VerticalLayoutScope(params GUILayoutOption[] options)
        {
            EditorGUILayout.BeginVertical(options);
        }

        public VerticalLayoutScope(GUIStyle style, params GUILayoutOption[] options)
        {
            EditorGUILayout.BeginVertical(style, options);
        }

        void IDisposable.Dispose()
        {
            EditorGUILayout.EndVertical();
        }
    }

} // namespace UnityExtensions.Editor

#endif // UNITY_EDITOR