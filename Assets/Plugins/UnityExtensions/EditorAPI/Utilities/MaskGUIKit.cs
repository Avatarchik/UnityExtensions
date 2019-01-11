#if UNITY_EDITOR

using System;
using UnityEngine;
using UnityEditor;

namespace UnityExtensions.Editor
{
    /// <summary>
    /// Mask 工具箱
    /// </summary>
    public partial struct GUIKit
    {
        enum PopupMode { Bit, Mask }


        class MaskWindow : PopupWindowContent
        {
            public static int controlID;
            public static int value;

            static MaskWindow _instance;

            static int _count;
            static string[] _names = new string[32];
            static int[] _bits = new int[32];

            static PopupMode _mode;
            static int _hoverLine;
            static float _buttonWidth;
            static EditorWindow _focusedWindow;

            static Color _windowBackground = new Color(1f, 1f, 1f, 0.2f);
            static Color _headerBackground = new Color(0f, 0f, 0f, 0.15f);
            static Color _mouseHoverColor = new Color(0, 0.5f, 1f, 0.4f);


            public static void Show(PopupMode mode, Rect buttonRect, int controlID, int value, Func<int, string> bitToName)
            {
                MaskWindow.controlID = controlID;
                MaskWindow.value = value;

                _count = 0;

                for (int i = 0; i < 32; i++)
                {
                    var name = bitToName(i);
                    if (!string.IsNullOrEmpty(name))
                    {
                        _names[_count] = name;
                        _bits[_count] = i;
                        _count++;
                    }
                }

                _mode = mode;
                _hoverLine = 0;
                _buttonWidth = buttonRect.width;
                _focusedWindow = EditorWindow.focusedWindow;

                _instance = new MaskWindow();
                Undo.undoRedoPerformed += Close;
                PopupWindow.Show(buttonRect, _instance);
            }


            public static void Close()
            {
                if (_instance != null)
                {
                    _instance.editorWindow.Close();
                    _instance = null;
                }
            }


            public override Vector2 GetWindowSize()
            {
                editorWindow.wantsMouseMove = true;
                return new Vector2(_buttonWidth, EditorGUIUtility.singleLineHeight * (_mode == PopupMode.Bit ? _count : (_count + 2)));
            }


            public override void OnGUI(Rect rect)
            {
                switch (Event.current.type)
                {
                    case EventType.MouseMove:
                        int newLine = Mathf.Clamp((int)((Event.current.mousePosition.y - rect.y) / EditorGUIUtility.singleLineHeight), 0, _mode == PopupMode.Bit ? (_count-1) : (_count + 1));
                        if (_hoverLine != newLine)
                        {
                            _hoverLine = newLine;
                            editorWindow.Repaint();
                        }
                        return;

                    case EventType.MouseDown:
                        if (_mode == PopupMode.Bit)
                        {
                            value = _bits[_hoverLine];
                        }
                        else
                        {
                            if (_hoverLine == 0) value = 0;
                            else if (_hoverLine == 1) value = ~0;
                            else value = BitwiseKit.ReverseBit(value, _bits[_hoverLine - 2]);
                        }
                        if (_focusedWindow) _focusedWindow.Repaint();
                        editorWindow.Repaint();
                        return;

                    case EventType.Repaint:
                        OnPaint(rect);
                        return;
                }
            }


            static void OnPaint(Rect rect)
            {
                EditorGUI.DrawRect(rect, _windowBackground);
                if (_mode == PopupMode.Mask)
                {
                    rect.height = EditorGUIUtility.singleLineHeight * 2;
                    EditorGUI.DrawRect(rect, _headerBackground);
                }
                rect.height = EditorGUIUtility.singleLineHeight;

                float checkSize = Mathf.Ceil(rect.height * 0.3f);
                float checkOffset = Mathf.Floor((rect.height - checkSize) * 0.5f);
                var checkRect = new Rect(rect.x + checkOffset, rect.y + checkOffset, checkSize, checkSize);
                var textRect = rect;
                textRect.xMin = checkRect.xMax + checkOffset;

                if (_mode == PopupMode.Mask)
                {
                    // Nothing

                    if (_hoverLine == 0) EditorGUI.DrawRect(rect, _mouseHoverColor);
                    if (value == 0)
                    {
                        EditorGUI.DrawRect(checkRect, defaultContentColor);
                        EditorGUI.LabelField(textRect, "Nothing", EditorStyles.boldLabel);
                    }
                    else EditorGUI.LabelField(textRect, "Nothing");

                    rect.y += rect.height;
                    checkRect.y += rect.height;
                    textRect.y += rect.height;

                    // Everything

                    if (_hoverLine == 1) EditorGUI.DrawRect(rect, _mouseHoverColor);
                    if (value == ~0)
                    {
                        EditorGUI.DrawRect(checkRect, defaultContentColor);
                        EditorGUI.LabelField(textRect, "Everything", EditorStyles.boldLabel);
                    }
                    else EditorGUI.LabelField(textRect, "Everything");

                    rect.y += rect.height;
                    checkRect.y += rect.height;
                    textRect.y += rect.height;
                }

                // Layers

                for (int i=0; i<_count; i++)
                {
                    if (_mode == PopupMode.Mask)
                    {
                        if ((_hoverLine - 2) == i) EditorGUI.DrawRect(rect, _mouseHoverColor);
                        if (BitwiseKit.GetBit(value, _bits[i]))
                        {
                            EditorGUI.DrawRect(checkRect, defaultContentColor);
                            EditorGUI.LabelField(textRect, _names[i], EditorStyles.boldLabel);
                        }
                        else EditorGUI.LabelField(textRect, _names[i]);
                    }
                    else
                    {
                        if (_hoverLine == i) EditorGUI.DrawRect(rect, _mouseHoverColor);
                        if (value == _bits[i])
                        {
                            EditorGUI.DrawRect(checkRect, defaultContentColor);
                            EditorGUI.LabelField(textRect, _names[i], EditorStyles.boldLabel);
                        }
                        else EditorGUI.LabelField(textRect, _names[i]);
                    }

                    rect.y += rect.height;
                    checkRect.y += rect.height;
                    textRect.y += rect.height;
                }
            }


            public override void OnClose()
            {
                controlID = 0;
                Undo.undoRedoPerformed -= Close;
            }
        }


        /// <summary>
        /// 绘制 32 位选择工具
        /// </summary>
        public static int BitField(Rect rect, string label, int bit, Func<int, string> bitToName)
        {
            int controlID = GUIUtility.GetControlID(FocusType.Passive);

            if (controlID == MaskWindow.controlID && MaskWindow.value != bit)
            {
                bit = MaskWindow.value;
                MaskWindow.Close();
                GUI.changed = true;
            }

            EditorGUI.PrefixLabel(rect, controlID, TempContent(label));

            rect.xMin += EditorGUIUtility.labelWidth;
            if (GUI.Button(rect, bitToName(bit), EditorStyles.popup))
            {
                GUIUtility.hotControl = 0;
                MaskWindow.Show(PopupMode.Bit, rect, controlID, bit, bitToName);
            }

            return bit;
        }


        /// <summary>
        /// 绘制 32 位选择工具
        /// </summary>
        public static int BitFieldLayout(string label, int bit, Func<int, string> bitToName, params GUILayoutOption[] options)
        {
            return BitField(EditorGUILayout.GetControlRect(options), label, bit, bitToName);
        }


        /// <summary>
        /// 绘制 32 位掩码工具
        /// </summary>
        public static int MaskField(Rect rect, string label, int mask, Func<int, string> bitToName)
        {
            int controlID = GUIUtility.GetControlID(FocusType.Passive);

            if (controlID == MaskWindow.controlID && MaskWindow.value != mask)
            {
                mask = MaskWindow.value;
                GUI.changed = true;
            }

            EditorGUI.PrefixLabel(rect, controlID, TempContent(label));

            rect.xMin += EditorGUIUtility.labelWidth;
            if (GUI.Button(rect, GetMaskButtonText(), EditorStyles.popup))
            {
                GUIUtility.hotControl = 0;
                MaskWindow.Show(PopupMode.Mask, rect, controlID, mask, bitToName);
            }

            return mask;

            string GetMaskButtonText()
            {
                if (mask == 0) return "Nothing";
                if (mask == ~0) return "Everything";

                string name1 = null;
                string name2 = null;
                int count = 0;

                for (int i = 0; i < 32; i++)
                {
                    if ((mask & (1 << i)) != 0)
                    {
                        string name = bitToName(i);
                        if (!string.IsNullOrEmpty(name))
                        {
                            count++;
                            if (count == 1) name1 = name;
                            else if (count == 2) name2 = name;
                            else break;
                        }
                    }
                }

                switch (count)
                {
                    case 0: return "(Nameless)";
                    case 1: return name1;
                    case 2: return string.Format("{0}, {1}", name1, name2);
                    default: return string.Format("{0}, {1}, ...", name1, name2);
                }
            }
        }


        /// <summary>
        /// 绘制 32 位掩码工具
        /// </summary>
        public static int MaskFieldLayout(string label, int mask, Func<int, string> bitToName, params GUILayoutOption[] options)
        {
            return MaskField(EditorGUILayout.GetControlRect(options), label, mask, bitToName);
        }


        /// <summary>
        /// 自动布局绘制 LayerMask
        /// </summary>
        public static LayerMask LayerMaskFieldLayout(string label, LayerMask mask, params GUILayoutOption[] options)
        {
            return MaskField(EditorGUILayout.GetControlRect(options), label, mask, LayerMask.LayerToName);
        }

    } // struct GUIKit

} // namespace UnityExtensions.Editor

#endif // UNITY_EDITOR