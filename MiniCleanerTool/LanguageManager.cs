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
        private static Dictionary<string, Dictionary<string, Dictionary<string, string>>>? translations;
        private static readonly HashSet<string> authoritativeTranslationKeys =
        [
            "Form1:en:eliminazioneLog",
            "Form1:it:eliminazioneLog",
            "Form1:en:rimozioneDefender",
            "Form1:it:rimozioneDefender"
        ];
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

            using Stream embeddedResource = Assembly.GetExecutingAssembly()
                .GetManifestResourceStream("MiniCleanerTool.Resources.FileTraduttore.json")
                ?? throw new FileNotFoundException("Risorsa traduzioni incorporata non trovata.");
            using var reader = new StreamReader(embeddedResource);
            var defaults = DeserializeTranslations(reader.ReadToEnd());
            var userTranslations = DeserializeTranslations(File.ReadAllText(filePath));
            translations = MergeTranslations(defaults, userTranslations);
        }

        private static Dictionary<string, Dictionary<string, Dictionary<string, string>>> DeserializeTranslations(string json)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                ReadCommentHandling = JsonCommentHandling.Skip,
                AllowTrailingCommas = true
            };

            return JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, Dictionary<string, string>>>>(json, options)
                ?? throw new JsonException("Il file delle traduzioni è vuoto o non valido.");
        }

        private static Dictionary<string, Dictionary<string, Dictionary<string, string>>> MergeTranslations(
            Dictionary<string, Dictionary<string, Dictionary<string, string>>> defaults,
            Dictionary<string, Dictionary<string, Dictionary<string, string>>> userTranslations)
        {
            foreach (var (formName, userLanguages) in userTranslations)
            {
                if (!defaults.TryGetValue(formName, out var defaultLanguages))
                {
                    defaults[formName] = userLanguages;
                    continue;
                }

                foreach (var (languageCode, userValues) in userLanguages)
                {
                    if (!defaultLanguages.TryGetValue(languageCode, out var defaultValues))
                    {
                        defaultLanguages[languageCode] = userValues;
                        continue;
                    }

                    foreach (var (key, value) in userValues)
                    {
                        if (!authoritativeTranslationKeys.Contains($"{formName}:{languageCode}:{key}"))
                            defaultValues[key] = value;
                    }
                }
            }

            return defaults;
        }

        private static void ExtractEmbeddedResource(string resourceName, string outputPath)
        {
            var assembly = Assembly.GetExecutingAssembly();
            using Stream stream = assembly.GetManifestResourceStream(resourceName)
                ?? throw new FileNotFoundException($"Embedded resource {resourceName} not found.");
            using FileStream fileStream = new(outputPath, FileMode.Create, FileAccess.Write);
            stream.CopyTo(fileStream);
        }

        public static void SetLanguage(string lang)
        {
            if (translations is not null && translations.Values.Any(f => f.ContainsKey(lang)))
            {
                CurrentLanguage = lang;
            }
        }

        public static string GetTranslation(string formName, string key)
        {
            if (translations is not null &&
                translations.TryGetValue(formName, out var form) &&
                form.TryGetValue(CurrentLanguage, out var language) &&
                language.TryGetValue(key, out var translation))
            {
                return translation;
            }
            return key;
        }
    }
}
