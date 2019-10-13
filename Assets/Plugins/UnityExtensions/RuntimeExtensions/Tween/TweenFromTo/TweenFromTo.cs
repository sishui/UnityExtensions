using UnityEngine;

namespace UnityExtensions
{
    public abstract class TweenFromTo<T> : TweenAnimation
    {
        public T from;
        public T to;

        public override void Reset()
        {
            base.Reset();
            from = default;
            to = default;
        }


#if UNITY_EDITOR

        protected new abstract class Editor<U> : TweenAnimation.Editor<U> where U : TweenFromTo<T>
        {
            protected UnityEditor.SerializedProperty _fromProp;
            protected UnityEditor.SerializedProperty _toProp;


            protected override void OnEnable()
            {
                base.OnEnable();
                _fromProp = serializedObject.FindProperty("from");
                _toProp = serializedObject.FindProperty("to");
            }


            protected override void InitOptionsMenu(UnityEditor.GenericMenu menu, Tween tween)
            {
                base.InitOptionsMenu(menu, tween);

                menu.AddSeparator(string.Empty);

                menu.AddItem(new GUIContent("from <=> to"), false, () =>
                {
                    UnityEditor.Undo.RecordObject(target, "from <=> to");
                    GeneralKit.Swap(ref target.from, ref target.to);
                });
            }

        }

#endif

    }


    public abstract class TweenFromToStruct<T> : TweenFromTo<T> where T : struct
    {
        /// <summary>
        /// 当前状态
        /// </summary>
        public abstract T current
        {
            get;
            set;
        }


#if UNITY_EDITOR

        T _temp;


        public override void Record()
        {
            _temp = current;
        }


        public override void Restore()
        {
            current = _temp;
        }


        protected new abstract class Editor<U> : TweenFromTo<T>.Editor<U> where U : TweenFromToStruct<T>
        {
            protected override void InitOptionsMenu(UnityEditor.GenericMenu menu, Tween tween)
            {
                base.InitOptionsMenu(menu, tween);

                menu.AddItem(new GUIContent("current = from"), false, () =>
                {
                    target.OnInterpolate(0);
                    UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(target.gameObject.scene);
                });

                menu.AddItem(new GUIContent("current = to"), false, () =>
                {
                    target.OnInterpolate(1);
                    UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(target.gameObject.scene);
                });

                menu.AddItem(new GUIContent("from = current"), false, () => 
                {
                    UnityEditor.Undo.RecordObject(target, "from = current");
                    target.from = target.current;
                });

                menu.AddItem(new GUIContent("to = current"), false, () =>
                {
                    UnityEditor.Undo.RecordObject(target, "to = current");
                    target.to = target.current;
                });
            }
        }

#endif

    } // class TweenLeftRight<T>

} // namespace UnityExtensions