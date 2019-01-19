using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UnityExtensions
{
    [TweenAnimation("Common/Behaviour Enabled", "Behaviour Enabled")]
    class TweenBehaviourEnabled : TweenFloat
    {
        public Behaviour targetBehaviour;
        public float criticalValue = 0.5f;


        public override float current
        {
            get
            {
                if (targetBehaviour)
                {
                    return targetBehaviour.enabled ? (criticalValue + 0.5f) : (criticalValue - 0.5f);
                }
                return criticalValue + 0.5f;
            }
            set
            {
                if (targetBehaviour)
                {
                    targetBehaviour.enabled = value > criticalValue;
                }
            }
        }


#if UNITY_EDITOR

        public override void Reset()
        {
            base.Reset();
            targetBehaviour = GetComponent<Behaviour>();
            criticalValue = 0.5f;
            from = current;
            to = current;
        }


        [CustomEditor(typeof(TweenBehaviourEnabled))]
        new class Editor : Editor<TweenBehaviourEnabled>
        {
            SerializedProperty _targetBehaviourProp;
            SerializedProperty _criticalValueProp;


            protected override void OnEnable()
            {
                base.OnEnable();
                _targetBehaviourProp = serializedObject.FindProperty("targetBehaviour");
                _criticalValueProp = serializedObject.FindProperty("criticalValue");
            }


            protected override void OnPropertiesGUI(Tween tween)
            {
                EditorGUILayout.Space();

                EditorGUILayout.PropertyField(_targetBehaviourProp);
                EditorGUILayout.PropertyField(_criticalValueProp);

                base.OnPropertiesGUI(tween);
            }
        }

#endif
    }

} // namespace UnityExtensions