using UnityEngine;

namespace UnityExtensions.Test
{
    public class Save : ScriptableComponent
    {
        // GameSave
        BinarySavableCollection gameSave;
        IStorageTarget gameSaveTarget;
        SaveManager gameSaveManager;

        // Settings
        TextSavableCollection settings;
        IStorageTarget settingsTarget;
        SaveManager settingsManager;

        public static Save instance { get; private set; }


        void Start()
        {
            instance = this;

            // GameSave
            var gameSavables = new IBinarySavable[]
            {
                Player.instance,
            };
            gameSave = new BinarySavableCollection(gameSavables);

            // Settings
            var settingsSavables = new ITextSavable[]
            {
                new TextSavableField
                (
                    "Quality Level",
                    t => QualitySettings.SetQualityLevel(int.Parse(t)),
                    w => w.Write(QualitySettings.GetQualityLevel()),
                    () => QualitySettings.SetQualityLevel(0)
                ),
                new TextSavableField
                (
                    "Audio Volume",
                    t => AudioListener.volume = float.Parse(t),
                    w => w.Write(AudioListener.volume),
                    () => AudioListener.volume = 1
                ),
            };
            settings = new TextSavableCollection(settingsSavables);

            // platform-dependent
#if UNITY_PS4

            var saveManager = new PS4SaveManager();

            gameSaveTarget = new PS4StorageTarget("TestGameSave.bin");
            gameSaveManager = saveManager;

            settingsTarget = new PS4StorageTarget("TestSettings.txt");
            settingsManager = saveManager;

#else // Standalone

    #if STEAM

            gameSaveTarget = new SteamStorageTarget("TestGameSave.bin");
            gameSaveManager = new SteamSaveManager();

            settingsTarget = new FileStorageTarget("TestSettings.txt");
            settingsManager = new BackgroundThreadSaveManager();

    #else // File System

            var saveManager = new BackgroundThreadSaveManager();

            gameSaveTarget = new FileStorageTarget("TestGameSave.bin");
            gameSaveManager = saveManager;

            settingsTarget = new FileStorageTarget("TestSettings.txt");
            settingsManager = saveManager;

    #endif

#endif
        }


        void OnDestroy()
        {
            gameSaveManager.Dispose();
            settingsManager.Dispose();
        }


        public void SaveGame()
        {
            gameSaveManager.NewSaveTask(gameSave, gameSaveTarget);
        }


        public void LoadGame()
        {
            gameSaveManager.NewLoadTask(gameSave, gameSaveTarget);
        }


        public void SaveSettings()
        {
            settingsManager.NewSaveTask(settings, settingsTarget);
        }


        public void LoadSettings()
        {
            settingsManager.NewLoadTask(settings, settingsTarget);
        }


        void OnGUI()
        {
            GUILayout.BeginVertical(GUILayout.Width(120));

            GUILayout.Label("Level: " + Player.instance.level);
            GUILayout.Label("HP: " + Player.instance.hp);

            if (GUILayout.Button("Load Game")) LoadGame();
            if (GUILayout.Button("Save Game")) SaveGame();

            GUILayout.Space(20);

            GUILayout.Label("Quality Level: " + QualitySettings.GetQualityLevel());
            GUILayout.Label("Audio Volume: " + AudioListener.volume);

            if (GUILayout.Button("Load Settings")) LoadSettings();
            if (GUILayout.Button("Save Settings")) SaveSettings();

#if UNITY_EDITOR
            GUILayout.Space(20);
            if (GUILayout.Button("Open Folder"))
                Editor.EditorApplicationKit.OpenFolder(Application.persistentDataPath);
#endif

            GUILayout.EndVertical();
        }
    }
}