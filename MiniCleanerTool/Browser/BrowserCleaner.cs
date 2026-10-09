using System.Security;

namespace MiniCleanerTool.Browser;

public static class BrowserCleaner
{
    public delegate void ProgressReport(int progress, string log);

    public static void CleanAllBrowsers(ProgressReport reportProgress)
    {
        ArgumentNullException.ThrowIfNull(reportProgress);
        var local = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        var roaming = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var browserTargets = new (string TranslationKey, IEnumerable<string> Paths)[]
        {
            ("chrome", ChromiumPaths(Path.Combine(local, "Google", "Chrome", "User Data", "Default"))),
            ("edge", ChromiumPaths(Path.Combine(local, "Microsoft", "Edge", "User Data", "Default"))),
            ("firefox", FirefoxPaths(Path.Combine(roaming, "Mozilla", "Firefox", "Profiles"), reportProgress)),
            ("opera", ChromiumPaths(Path.Combine(roaming, "Opera Software", "Opera Stable"))),
            ("brave", ChromiumPaths(Path.Combine(local, "BraveSoftware", "Brave-Browser", "User Data", "Default"))),
            ("vivaldi", ChromiumPaths(Path.Combine(local, "Vivaldi", "User Data", "Default"))),
            ("safari", [Path.Combine(local, "Apple Computer", "Safari", "Cache")]),
            ("ie", [Environment.GetFolderPath(Environment.SpecialFolder.InternetCache), Path.Combine(local, "Microsoft", "Windows", "INetCache")])
        };

        reportProgress(0, $"{LanguageManager.GetTranslation("Browser", "startingclean")}\r\n");
        for (int index = 0; index < browserTargets.Length; index++)
        {
            var browser = browserTargets[index];
            reportProgress((index + 1) * 100 / browserTargets.Length,
                $"{LanguageManager.GetTranslation("Browser", browser.TranslationKey)}\r\n");
            foreach (string path in browser.Paths)
            {
                DeleteDirectoryContents(path, reportProgress);
            }
        }

        reportProgress(100, $"{LanguageManager.GetTranslation("Browser", "completed")}\r\n");
    }

    private static IEnumerable<string> ChromiumPaths(string profileRoot) =>
    [
        Path.Combine(profileRoot, "Cache"),
        Path.Combine(profileRoot, "Media Cache"),
        Path.Combine(profileRoot, "Service Worker", "CacheStorage"),
        Path.Combine(profileRoot, "Code Cache")
    ];

    private static IEnumerable<string> FirefoxPaths(string profilesRoot, ProgressReport reportProgress)
    {
        var paths = new List<string>();
        if (!Directory.Exists(profilesRoot)) return paths;

        foreach (string profile in Directory.EnumerateDirectories(profilesRoot))
        {
            try
            {
                if ((File.GetAttributes(profile) & FileAttributes.ReparsePoint) != 0) continue;
                paths.Add(Path.Combine(profile, "cache2"));
                paths.Add(Path.Combine(profile, "thumbnails"));
            }
            catch (Exception exception) when (exception is IOException or UnauthorizedAccessException or SecurityException)
            {
                ReportFailure(reportProgress, profile, exception);
            }
        }

        return paths;
    }

    private static void DeleteDirectoryContents(string path, ProgressReport reportProgress)
    {
        if (!Directory.Exists(path))
        {
            reportProgress(0, $"{LanguageManager.GetTranslation("Browser", "dir_not_found")}: {path}\r\n");
            return;
        }

        string root = Path.TrimEndingDirectorySeparator(Path.GetFullPath(path));
        var pending = new Stack<(string Path, bool RemoveAfterChildren)>();
        pending.Push((root, false));
        int deletedFiles = 0;
        int deletedDirectories = 0;
        reportProgress(0, $"{DateTime.Now:dd/MM/yyyy - HH.mm} {LanguageManager.GetTranslation("Browser", "starting_cleaning")}: {root}\r\n");

        while (pending.TryPop(out var item))
        {
            if (item.RemoveAfterChildren)
            {
                try
                {
                    Directory.Delete(item.Path, recursive: false);
                    deletedDirectories++;
                }
                catch (Exception exception) when (exception is IOException or UnauthorizedAccessException or SecurityException)
                {
                    ReportFailure(reportProgress, item.Path, exception);
                }

                continue;
            }

            try
            {
                if ((File.GetAttributes(item.Path) & FileAttributes.ReparsePoint) != 0)
                {
                    reportProgress(0, $"Percorso collegato saltato: {item.Path}\r\n");
                    continue;
                }

                if (!item.Path.Equals(root, StringComparison.OrdinalIgnoreCase))
                    pending.Push((item.Path, true));

                foreach (string entry in Directory.EnumerateFileSystemEntries(item.Path))
                {
                    try
                    {
                        FileAttributes attributes = File.GetAttributes(entry);
                        if ((attributes & FileAttributes.ReparsePoint) != 0) continue;
                        if ((attributes & FileAttributes.Directory) != 0)
                        {
                            pending.Push((entry, false));
                            continue;
                        }

                        File.SetAttributes(entry, FileAttributes.Normal);
                        File.Delete(entry);
                        deletedFiles++;
                    }
                    catch (Exception exception) when (exception is IOException or UnauthorizedAccessException or SecurityException)
                    {
                        ReportFailure(reportProgress, entry, exception);
                    }
                }
            }
            catch (Exception exception) when (exception is IOException or UnauthorizedAccessException or SecurityException)
            {
                ReportFailure(reportProgress, item.Path, exception);
            }
        }

        reportProgress(0, $"{DateTime.Now:dd/MM/yyyy - HH.mm} {LanguageManager.GetTranslation("Browser", "cleaning_complete")}: {root} ({deletedFiles} file, {deletedDirectories} cartelle)\r\n");
    }

    private static void ReportFailure(ProgressReport reportProgress, string path, Exception exception) =>
        reportProgress(0, $"{LanguageManager.GetTranslation("Browser", "cleaning_error")}: {path}: {exception.Message}\r\n");
}
