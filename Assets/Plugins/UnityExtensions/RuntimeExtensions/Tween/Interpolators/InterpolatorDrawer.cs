#if UNITY_EDITOR

using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace UnityExtensions.Editor
{
    /// <summary>
    /// Interpolator Drawer
    /// </summary>
    [CustomPropertyDrawer(typeof(Interpolator))]
    [CustomPropertyDrawer(typeof(CustomizableInterpolator))]
    class InterpolatorDrawer : BasePropertyDrawer
    {
        // 最后采样时的数据缓存
        int _lastType;
        float _lastStrength;

        float _minValue, _maxValue;
        List<Vector3> _samples = new List<Vector3>(64);


        // 采样
        void Sample(int type, float strength, int maxSegments, float maxError)
        {
            if (_samples.Count == 0
                    || type != _lastType
                    || strength != _lastStrength)
            {
                _lastType = type;
                _lastStrength = strength;
                _samples.Clear();

                var interpolator = new Interpolator((Interpolator.Type)type, strength);

                // 添加第一个点

                Vector3 point = new Vector3(0, interpolator[0]);
                _samples.Add(point);

                // 添加其他点

                Vector3 lastSample = point, lastEvaluate = point;
                _minValue = _maxValue = point.y;

                float minSlope = float.MinValue;
                float maxSlope = float.MaxValue;

                for (int i = 1; i <= maxSegments; i++)
                {
                    point.x = i / (float)maxSegments;
                    point.y = interpolator[point.x];

                    if (_minValue > point.y) _minValue = point.y;
                    if (_maxValue < point.y) _maxValue = point.y;

                    maxSlope = Mathf.Min((point.y - lastSample.y + maxError) / (point.x - lastSample.x), maxSlope);
                    minSlope = Mathf.Max((point.y - lastSample.y - maxError) / (point.x - lastSample.x), minSlope);

                    if (minSlope >= maxSlope)
                    {
                        _samples.Add(lastSample = lastEvaluate);
                        maxSlope = (point.y - lastSample.y + maxError) / (point.x - lastSample.x);
                        minSlope = (point.y - lastSample.y - maxError) / (point.x - lastSample.x);
                    }

                    lastEvaluate = point;
                }

                // 添加最后一个点

                _samples.Add(point);
                if (_minValue > point.y) _minValue = point.y;
                if (_maxValue < point.y) _maxValue = point.y;

                // 计算绘制的边界值

                if (_maxValue - _minValue < 1f)
                {
                    if (_minValue < 0f)
                    {
                        _maxValue = _minValue + 1f;
                    }
                    else if (_maxValue > 1f)
                    {
                        _minValue = _maxValue - 1f;
                    }
                    else
                    {
                        _minValue = 0f;
                        _maxValue = 1f;
                    }
                }
            }
        }


        // 绘制曲线
        void DrawCurve(Rect rect)
        {
            EditorGUI.DrawRect(rect, new Color(0.3f, 0.3f, 0.3f));

            Vector2 origin = new Vector2(rect.x + 1, rect.y + 1);
            Vector2 scale = new Vector2(rect.width - 2, (rect.height - 2) / (_maxValue - _minValue));

            //if (_maxValue > 0f && _minValue < 1f)
            //{
            //    float yMin = origin.y + (_maxValue - Mathf.Min(_maxValue, 1f)) * scale.y;
            //    float yMax = origin.y + (_maxValue - Mathf.Max(_minValue, 0f)) * scale.y;
            //    Rect rect01 = new Rect(rect.x, yMin, rect.width, yMax - yMin);
            //    EditorGUI.DrawRect(rect01, new Color(0.4f, 0.4f, 0.4f));
            //}

            Vector3 last = _samples[0];
            last.x = origin.x + last.x * scale.x;
            last.y = origin.y + (_maxValue - last.y) * scale.y;

            using (new HandlesColorScope(new Color(1f, 1f, 1f, 0.8f)))
            {
                Vector3 point;

                for (int i = 1; i < _samples.Count; i++)
                {
                    point = _samples[i];
                    point.x = origin.x + point.x * scale.x;
                    point.y = origin.y + (_maxValue - point.y) * scale.y;

                    HandlesKit.DrawAALine(last, point);
                    last = point;
                }
            }

            EditorGUIKit.DrawWireRect(rect, new Color(0, 0, 0, 0.5f));
        }


        public override bool CanCacheInspectorGUI(SerializedProperty property)
        {
            return false;
        }


        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight * 2 + 2;
        }


        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            position = EditorGUI.PrefixLabel(position, label);

            var curveRect = position;
            curveRect.xMin = curveRect.xMax - curveRect.height * 2;
            position.xMax = curveRect.xMin - 4;
            position.height = EditorGUIUtility.singleLineHeight;

            var prop = property.FindPropertyRelative("type");
            using (var scope = new ChangeCheckScope(null))
            {
                EditorGUI.PropertyField(position, prop, GUIContent.none);
                if (scope.changed) property.FindPropertyRelative("strength").floatValue = 0.5f;
            }
            int type = prop.intValue;
            float strength = 0;

            switch ((CustomizableInterpolator.Type)prop.intValue)
            {
                case CustomizableInterpolator.Type.Linear:
                case CustomizableInterpolator.Type.Parabolic:
                case CustomizableInterpolator.Type.Sine:
                case CustomizableInterpolator.Type.CustomCurve:
                    break;

                default:
                    position.y = position.yMax + 2;
                    prop = property.FindPropertyRelative("strength");
                    if (position.width < 110)
                    {
                        position.xMin -= 24;
                        using (new LabelWidthScope(24))
                        {
                            prop.floatValue = strength = Mathf.Clamp01(EditorGUI.FloatField(position, "    ", prop.floatValue));
                        }
                    }
                    else
                    {
                        EditorGUI.PropertyField(position, prop, GUIContent.none);
                        strength = prop.floatValue;
                    }
                    break;
            }

            if (type < 0)
            {
                prop = property.FindPropertyRelative("customCurve");
                EditorGUI.PropertyField(curveRect, prop, GUIContent.none);
            }
            else
            {
                if (Event.current.type == EventType.Repaint)
                {
                    Sample(type, strength, Mathf.Min((int)curveRect.width, 256), 0.002f);
                    DrawCurve(curveRect);
                }
            }
        }

    } // class InterpolatorDrawer

} // namespace UnityExtensions

#endif // UNITY_EDITOR