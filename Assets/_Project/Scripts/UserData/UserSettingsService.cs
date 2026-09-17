using System.IO;
using UnityEngine;

namespace RiichiDojo
{
    public class UserSettingsService
    {
        private const string FileName = "UserSettings.json";

        private const int CurrentVersion = 2;
        
        private string FilePath =>
            Path.Combine(
                Application.persistentDataPath,
                FileName);

        public UserSettings Load()
        {
            if (!File.Exists(FilePath))
            {
                return new UserSettings();
            }

            string json =
                File.ReadAllText(FilePath);

            if (string.IsNullOrWhiteSpace(json))
            {
                return new UserSettings();
            }

            int version = 1;

            try
            {
                var versionData =
                    JsonUtility.FromJson<VersionData>(json);

                if (versionData != null)
                {
                    version = versionData.Version;
                }
            }
            catch
            {
                return new UserSettings();
            }

            if (version < CurrentVersion)
            {
                var legacySettings =
                    JsonUtility.FromJson<LegacyUserSettings>(json);

                UserSettings migratedSettings =
                    Migrate(legacySettings);

                Save(migratedSettings);

                return migratedSettings;
            }

            UserSettings settings =
                JsonUtility.FromJson<UserSettings>(json);

            if (settings == null)
            {
                return new UserSettings();
            }

            int originalVersion =
                settings.Version;

            string originalDifficulty =
                settings.TrainingDifficulty;

            settings =
                MigrateLegacyNames(settings);

            settings =
                Migrate(settings);

            if (settings.Version != originalVersion ||
                settings.TrainingDifficulty != originalDifficulty)
            {
                Save(settings);
            }

            return settings;
        }

        public void Save(UserSettings settings)
        {
            string json =
                JsonUtility.ToJson(settings, true);

            File.WriteAllText(FilePath, json);
        }
        
        private UserSettings Migrate(
            LegacyUserSettings legacySettings)
        {
            var settings = new UserSettings
            {
                Version = CurrentVersion,
                TrainingDifficulty =
                    GetDifficultyId(
                        legacySettings.TrainingDifficulty),
                TileTheme =
                    GetThemeName(
                        legacySettings.TileTheme)
            };

            return settings;
        }

        private UserSettings Migrate(UserSettings settings)
        {
            settings.Version = CurrentVersion;

            return settings;
        }
        
        private UserSettings MigrateLegacyNames(
            UserSettings settings)
        {
            switch (settings.TrainingDifficulty)
            {
                case "Easy":
                    settings.TrainingDifficulty = "1Suit";
                    break;

                case "Medium":
                    settings.TrainingDifficulty = "2Suits";
                    break;

                case "Hard":
                    settings.TrainingDifficulty = "3Suits";
                    break;
            }

            return settings;
        }
        
        private string GetDifficultyId(int index)
        {
            string[] difficultyIds =
            {
                "1Suit",
                "2Suits",
                "3Suits"
            };

            if (index < 0 || index >= difficultyIds.Length)
            {
                return "1Suit";
            }

            return difficultyIds[index];
        }

        private string GetThemeName(int index)
        {
            string[] themes =
            {
                "White",
                "Black"
            };

            if (index < 0 || index >= themes.Length)
            {
                return "White";
            }

            return themes[index];
        }
        
        [System.Serializable]
        private class LegacyUserSettings
        {
            public int Version = 1;

            public int TrainingDifficulty = 0;

            public int TileTheme = 0;
        }
        
        [System.Serializable]
        private class VersionData
        {
            public int Version = 1;
        }
    }
}