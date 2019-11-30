﻿using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UnityExtensions
{
    [TweenAnimation("Rect Transform/Offset Max", "Rect Transform Offset Max")]
    class TweenRectTransformOffsetMax : TweenVector2
    {
        public RectTransform targetRectTransform = default;


        public override Vector2 current
        {
            get
            {
                if (targetRectTransform)
                {
                    return targetRectTransform.offsetMax;
                }
                return default;
            }
            set
            {
                if (targetRectTransform)
                {
                    targetRectTransform.offsetMax = value;
                }
            }
        }


#if UNITY_EDITOR

        RectTransform _originalTarget;


        public override void Record()
        {
            _originalTarget = targetRectTransform;
            base.Record();
        }


        public override void Restore()
        {
            var t = targetRectTransform;
            targetRectTransform = _originalTarget;
            base.Restore();
            targetRectTransform = t;
        }


        public override void Reset()
        {
            base.Reset();
            targetRectTransform = GetComponent<RectTransform>();
            from = current;
            to = current;
        }


        [CustomEditor(typeof(TweenRectTransformOffsetMax))]
        new class Editor : Editor<TweenRectTransformOffsetMax>
        {
            SerializedProperty _targetRectTransformProp;


            protected override void OnEnable()
            {
                base.OnEnable();
                _targetRectTransformProp = serializedObject.FindProperty("targetRectTransform");
            }


            protected override void OnPropertiesGUI(Tween tween)
            {
                EditorGUILayout.Space();

                EditorGUILayout.PropertyField(_targetRectTransformProp);

                base.OnPropertiesGUI(tween);
            }
        }

#endif
    }

} // namespace UnityExtensions