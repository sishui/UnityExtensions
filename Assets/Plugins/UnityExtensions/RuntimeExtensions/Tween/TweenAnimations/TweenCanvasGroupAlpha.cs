using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UnityExtensions
{
    [TweenAnimation("2D and UI/Canvas Group Alpha", "Canvas Group Alpha")]
    class TweenCanvasGroupAlpha : TweenFloat
    {
        public CanvasGroup targetCanvasGroup;


        public override float current
        {
            get
            {
                if (targetCanvasGroup)
                {
                    return targetCanvasGroup.alpha;
                }
                return 1f;
            }
            set
            {
                if (targetCanvasGroup)
                {
                    targetCanvasGroup.alpha = value;
                }
            }
        }


#if UNITY_EDITOR

        public override void Reset()
        {
            base.Reset();
            targetCanvasGroup = GetComponent<CanvasGroup>();
            from = current;
            to = current;
        }


        [CustomEditor(typeof(TweenCanvasGroupAlpha))]
        new class Editor : Editor<TweenCanvasGroupAlpha>
        {
            SerializedProperty _targetCanvasGroupProp;


            protected override void OnEnable()
            {
                base.OnEnable();
                _targetCanvasGroupProp = serializedObject.FindProperty("targetCanvasGroup");
            }


            protected override void OnPropertiesGUI(Tween tween)
            {
                EditorGUILayout.Space();

                EditorGUILayout.PropertyField(_targetCanvasGroupProp);

                base.OnPropertiesGUI(tween);
            }
        }

#endif
    }

} // namespace UnityExtensions