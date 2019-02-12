using UnityEngine;

namespace UnityExtensions.Test
{
    public class Player : ScriptableComponent
    {
        public int level;
        public float hp;

        public static Player instance { get; private set; }


        void Awake()
        {
            instance = this;
            SaveSystem.Init();
        }


        void OnDestroy()
        {
            SaveSystem.Dispose();
        }


        void OnGUI()
        {
            GUILayout.BeginVertical(GUILayout.Width(120));

            GUILayout.Label("Level: " + level);
            GUILayout.Label("HP: " + hp);

            if (GUILayout.Button("Load Game"))
            {
                SaveSystem.LoadGame();
            }

            if (GUILayout.Button("Save Game"))
            {
                SaveSystem.SaveGame();
            }

            GUILayout.Space(20);

            GUILayout.Label("Quality Level: " + QualitySettings.GetQualityLevel());
            GUILayout.Label("Audio Volume: " + AudioListener.volume);

            if (GUILayout.Button("Load Settings"))
            {
                SaveSystem.LoadSettings();
            }

            if (GUILayout.Button("Save Settings"))
            {
                SaveSystem.SaveSettings();
            }

#if UNITY_EDITOR

            GUILayout.Space(20);

            if (GUILayout.Button("Open Folder"))
            {
                Editor.EditorApplicationKit.OpenFolder(Application.persistentDataPath);
            }
#endif

            GUILayout.EndVertical();
        }
    }
}