using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MiniCleanerTool
{
    public static class LanguageManager
    {
        private static Dictionary<string, Dictionary<string, Dictionary<string, string>>> translations;
        public static string CurrentLanguage { get; private set; } = "en";

        public static void LoadTranslations()
        {
            string localAppDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "MiniCleanerTool", "Lingue");
            string filePath = Path.Combine(localAppDataPath, "FileTraduttore.json");

            if (!Directory.Exists(localAppDataPath))
            {
                Directory.CreateDirectory(localAppDataPath);
            }

            if (!File.Exists(filePath))
            {
                ExtractEmbeddedResource("MiniCleanerTool.Resources.FileTraduttore.json", filePath);
            }

            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    ReadCommentHandling = JsonCommentHandling.Skip,
                    AllowTrailingCommas = true
                };

                translations = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, Dictionary<string, string>>>>(json, options);
            }
            else
            {
                throw new FileNotFoundException($"File non presente {filePath}");
            }
        }

        private static void ExtractEmbeddedResource(string resourceName, string outputPath)
        {
            var assembly = Assembly.GetExecutingAssembly();
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                {
                    throw new FileNotFoundException($"Embedded resource {resourceName} not found.");
                }

                using (FileStream fileStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
                {
                    stream.CopyTo(fileStream);
                }
            }
        }

        public static void SetLanguage(string lang)
        {
            if (translations != null && translations.Values.Any(f => f.ContainsKey(lang)))
            {
                CurrentLanguage = lang;
            }
        }

        public static string GetTranslation(string formName, string key)
        {
            if (translations != null &&
                translations.ContainsKey(formName) &&
                translations[formName].ContainsKey(CurrentLanguage) &&
                translations[formName][CurrentLanguage].ContainsKey(key))
            {
                return translations[formName][CurrentLanguage][key];
            }
            return key;
        }
    }
}
