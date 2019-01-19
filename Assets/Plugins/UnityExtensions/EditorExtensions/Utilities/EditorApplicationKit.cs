#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace UnityExtensions.Editor
{
    /// <summary>
    /// 编辑器 Application 工具箱
    /// </summary>
    public struct EditorApplicationKit
    {
        static float _deltaTime;
        static double _lastTimeSinceStartup;


        [InitializeOnLoadMethod]
        static void Init()
        {
            EditorApplication.update += () =>
            {
                _deltaTime = (float)(EditorApplication.timeSinceStartup - _lastTimeSinceStartup);
                _lastTimeSinceStartup = EditorApplication.timeSinceStartup;
            };
        }


        public static float deltaTime
        {
            get { return _deltaTime; }
        }

    } // struct EditorApplicationKit

} // namespace UnityExtensions.Editor

#endif // UNITY_EDITOR