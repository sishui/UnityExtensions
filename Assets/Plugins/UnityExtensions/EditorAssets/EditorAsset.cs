#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

namespace UnityExtensions.Editor
{
    public class EditorAsset : ScriptableAssetSingleton<EditorAsset>
    {
        public Texture2D play;
        public Texture2D rightArrow;
        public Texture2D leftArrow;


        [MenuItem("Assets/Create/Unity Extensions/Editor or Test/Editor Asset")]
        static void CreateAsset()
        {
            CreateOrSelectAsset();
        }

    } // EditorAsset

} // UnityExtensions.Editor

#endif