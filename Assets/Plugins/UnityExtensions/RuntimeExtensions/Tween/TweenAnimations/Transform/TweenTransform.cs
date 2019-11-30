using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using UnityExtensions.Editor;
#endif

namespace UnityExtensions
{
    [TweenAnimation("Transform/Transform", "Transform")]
    class TweenTransform : TweenFromTo<Transform>
    {
        public bool togglePosition = default;
        public bool toggleRotation = default;
        public bool toggleLocalScale = default;
        public Transform targetTransform = default;


        public Vector3 currentPosition
        {
            get => targetTransform ? targetTransform.position : default;
            set { if (targetTransform) targetTransform.position = value; }
        }


        public Quaternion currentRotation
        {
            get => targetTransform ? targetTransform.rotation : Quaternion.identity;
            set { if (targetTransform) targetTransform.rotation = value; }
        }


        public Vector3 currentLocalScale
        {
            get => targetTransform ? targetTransform.localScale : new Vector3(1, 1, 1);
            set { if (targetTransform) targetTransform.localScale = value; }
        }


        protected override void OnInterpolate(float factor)
        {
            if (from && to && targetTransform)
            {
                if (togglePosition)
                {
                    currentPosition = Vector3.LerpUnclamped(from.position, to.position, factor);
                }
                if (toggleRotation)
                {
                    currentRotation = Quaternion.SlerpUnclamped(from.rotation, to.rotation, factor);
                }
                if (toggleLocalScale)
                {
                    currentLocalScale = Vector3.LerpUnclamped(from.localScale, to.localScale, factor);
                }
            }
        }

#if UNITY_EDITOR

        Transform _originalTarget;
        Vector3 _tempPosition;
        Quaternion _tempRotation;
        Vector3 _tempLocalScale;


        public override void Record()
        {
            _originalTarget = targetTransform;
            _tempPosition = currentPosition;
            _tempRotation = currentRotation;
            _tempLocalScale = currentLocalScale;
        }


        public override void Restore()
        {
            var t = targetTransform;
            targetTransform = _originalTarget;
            currentPosition = _tempPosition;
            currentRotation = _tempRotation;
            currentLocalScale = _tempLocalScale;
            targetTransform = t;
        }


        public override void Reset()
        {
            base.Reset();
            togglePosition = default;
            toggleRotation = default;
            toggleLocalScale = default;
            targetTransform = transform;
        }


        [CustomEditor(typeof(TweenTransform))]
        new class Editor : Editor<TweenTransform>
        {
            SerializedProperty _togglePositionProp;
            SerializedProperty _toggleRotationProp;
            SerializedProperty _toggleLocalScaleProp;
            SerializedProperty _targetTransformProp;


            protected override void OnEnable()
            {
                base.OnEnable();
                _togglePositionProp = serializedObject.FindProperty("togglePosition");
                _toggleRotationProp = serializedObject.FindProperty("toggleRotation");
                _toggleLocalScaleProp = serializedObject.FindProperty("toggleLocalScale");
                _targetTransformProp = serializedObject.FindProperty("targetTransform");
            }


            protected override void OnPropertiesGUI(Tween tween)
            {
                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(_targetTransformProp);

                EditorGUILayout.Space();

                var rect = EditorGUILayout.GetControlRect();
                float labelWidth = EditorGUIUtility.labelWidth;

                var fromRect = new Rect(rect.x + labelWidth, rect.y, (rect.width - labelWidth) / 2 - 2, rect.height);
                var toRect = new Rect(rect.xMax - fromRect.width, fromRect.y, fromRect.width, fromRect.height);

                rect.width = rect.height + EditorStyles.label.CalcSize(EditorGUIKit.TempContent("P")).x;
                _togglePositionProp.boolValue = EditorGUI.ToggleLeft(rect, "P", _togglePositionProp.boolValue);

                rect.x = rect.xMax + rect.height * 0.5f;
                rect.width = rect.height + EditorStyles.label.CalcSize(EditorGUIKit.TempContent("R")).x;
                _toggleRotationProp.boolValue = EditorGUI.ToggleLeft(rect, "R", _toggleRotationProp.boolValue);

                rect.x = rect.xMax + rect.height * 0.5f;
                rect.width = rect.height + EditorStyles.label.CalcSize(EditorGUIKit.TempContent("S")).x;
                _toggleLocalScaleProp.boolValue = EditorGUI.ToggleLeft(rect, "S", _toggleLocalScaleProp.boolValue);

                using (new DisabledScope(!target.togglePosition && !target.toggleRotation && !target.toggleLocalScale))
                {
                    using (new LabelWidthScope(14))
                    {
                        EditorGUI.ObjectField(fromRect, _fromProp, EditorGUIKit.TempContent("F"));
                        EditorGUI.ObjectField(toRect, _toProp, EditorGUIKit.TempContent("T"));
                    }
                }
            }


            protected override void InitOptionsMenu(GenericMenu menu, Tween tween)
            {
                base.InitOptionsMenu(menu, tween);

                menu.AddItem(new GUIContent("current = from"), false, () =>
                {
                    if (target.targetTransform && target.from)
                    {
                        Undo.RecordObject(target.targetTransform, "current = from");
                        target.currentPosition = target.from.position;
                        target.currentRotation = target.from.rotation;
                        target.currentLocalScale = target.from.localScale;
                    }
                });

                menu.AddItem(new GUIContent("current = to"), false, () =>
                {
                    if (target.targetTransform && target.to)
                    {
                        Undo.RecordObject(target.targetTransform, "current = to");
                        target.currentPosition = target.to.position;
                        target.currentRotation = target.to.rotation;
                        target.currentLocalScale = target.to.localScale;
                    }
                });

                menu.AddItem(new GUIContent("from = current"), false, () =>
                {
                    if (target.from)
                    {
                        Undo.RecordObject(target.from, "from = current");
                        target.from.position = target.currentPosition;
                        target.from.rotation = target.currentRotation;
                        target.from.localScale = target.currentLocalScale;
                    }
                });

                menu.AddItem(new GUIContent("to = current"), false, () =>
                {
                    if (target.to)
                    {
                        Undo.RecordObject(target.to, "to = current");
                        target.to.position = target.currentPosition;
                        target.to.rotation = target.currentRotation;
                        target.to.localScale = target.currentLocalScale;
                    }
                });
            }
        }

#endif

    } // class TweenTransform

} // namespace UnityExtensions