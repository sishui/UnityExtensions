using System.Collections.Generic;
using UnityEngine;

namespace UnityExtensions.Test
{
    public struct SaveSystem
    {
        static GameSave gameSave;
        static IStorageTarget gameSaveTarget;
        static SaveManager gameSaveManager;

        static Settings settings;
        static IStorageTarget settingsTarget;
        static SaveManager settingsManager;


        public static void Init()
        {
            gameSave = new GameSave();
            settings = new Settings();

#if UNITY_PS4

            var saveManager = new PS4SaveManager();

            gameSaveTarget = new PS4StorageTarget("Test GameSave.bin");
            gameSaveManager = saveManager;

            settingsTarget = new PS4StorageTarget("Test Settings.txt");
            settingsManager = saveManager;

#else // Standalone

    #if STEAM

            gameSaveTarget = new SteamStorageTarget("Test GameSave.bin");
            gameSaveManager = new SteamSaveManager();

            settingsTarget = new FileStorageTarget("Test Settings.txt");
            settingsManager = new BackgroundThreadSaveManager();

    #else // File System

            var saveManager = new BackgroundThreadSaveManager();

            gameSaveTarget = new FileStorageTarget("Test GameSave.bin");
            gameSaveManager = saveManager;

            settingsTarget = new FileStorageTarget("Test Settings.txt");
            settingsManager = saveManager;

    #endif

#endif
        }


        public static void Dispose()
        {
            gameSaveManager.Dispose();
            settingsManager.Dispose();
        }


        public static void SaveGame()
        {
            gameSaveManager.NewSaveTask(gameSave, gameSaveTarget);
        }


        public static void LoadGame()
        {
            gameSaveManager.NewLoadTask(gameSave, gameSaveTarget);
        }


        public static void SaveSettings()
        {
            settingsManager.NewSaveTask(settings, settingsTarget);
        }


        public static void LoadSettings()
        {
            settingsManager.NewLoadTask(settings, settingsTarget);
        }
    }


    public class GameSave : ListBinarySave
    {
        protected override IList<BinarySaveField> CreateFields()
        {
            return new BinarySaveField[]
            {
                new BinarySaveField
                (
                    r => Player.instance.level = r.ReadInt32(),
                    w => w.Write(Player.instance.level)
                ),
                new BinarySaveField
                (
                    r => Player.instance.hp = r.ReadSingle(),
                    w => w.Write(Player.instance.hp)
                ),
            };
        }


        public override void Reset()
        {
            Player.instance.level = 1;
            Player.instance.hp = 1;
        }
    }


    public class Settings : DictionaryTextSave
    {
        protected override IDictionary<string, TextSaveField> CreateFields()
        {
            var fields = new Dictionary<string, TextSaveField>();

            fields.Add(
                "Quality Level", new TextSaveField
                (
                    s => QualitySettings.SetQualityLevel(int.Parse(s)),
                    w => w.Write(QualitySettings.GetQualityLevel())
                ));

            fields.Add(
                "Audio Volume", new TextSaveField
                (
                    s => AudioListener.volume = float.Parse(s),
                    w => w.Write(AudioListener.volume)
                ));

            return fields;
        }


        public override void Reset()
        {
            QualitySettings.SetQualityLevel(0);
            AudioListener.volume = 1;
        }
    }
}