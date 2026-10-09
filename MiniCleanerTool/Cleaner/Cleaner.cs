using System.Security;

namespace MiniCleanerTool.Cleaner;

public static class Cleaner
{
    private const FileAttributes ReparsePoint = FileAttributes.ReparsePoint;
    private const FileAttributes DirectoryAttribute = FileAttributes.Directory;

    public static Task CleanSystem(Action<int, string> updateProgress)
    {
        ArgumentNullException.ThrowIfNull(updateProgress);

        string windows = Environment.GetFolderPath(Environment.SpecialFolder.Windows);
        string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        string userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        string commonData = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);

        string[] directories =
        [
            Path.GetTempPath(),
            Path.Combine(windows, "Temp"),
            Path.Combine(appData, "Microsoft", "Windows", "Recent"),
            Path.Combine(localAppData, "CrashDumps"),
            Path.Combine(localAppData, "Microsoft", "Windows", "WebCache"),
            Path.Combine(localAppData, "Microsoft", "Windows", "INetCache"),
            Path.Combine(localAppData, "Microsoft", "Windows", "INetCookies"),
            Path.Combine(windows, "SoftwareDistribution", "Download"),
            Path.Combine(windows, "Prefetch"),
            Path.Combine(windows, "System32", "config", "systemprofile", "AppData", "Local", "Microsoft", "Windows", "DeliveryOptimization"),
            Path.Combine(userProfile, "AppData", "Local", "D3DSCache"),
            Path.Combine(userProfile, "AppData", "Local", "Microsoft", "Windows", "History")
        ];

        foreach (string directory in directories.Distinct(StringComparer.OrdinalIgnoreCase))
        {
            CleanDirectory(directory, updateProgress);
        }

        updateProgress(100, $"✔️ {LanguageManager.GetTranslation("Cleaner", "cleaning_complete_all")}\n");
        return Task.CompletedTask;
    }

    private static void CleanDirectory(string directory, Action<int, string> updateProgress)
    {
        if (!Directory.Exists(directory)) return;

        string root = Path.TrimEndingDirectorySeparator(Path.GetFullPath(directory));
        var pending = new Stack<(string Path, bool RemoveAfterChildren)>();
        pending.Push((root, false));

        while (pending.TryPop(out var item))
        {
            if (item.RemoveAfterChildren)
            {
                try
                {
                    Directory.Delete(item.Path, recursive: false);
                }
                catch (Exception exception) when (exception is IOException or UnauthorizedAccessException or SecurityException)
                {
                    ReportFailure(updateProgress, item.Path, exception);
                }

                continue;
            }

            FileAttributes directoryAttributes;
            try
            {
                directoryAttributes = File.GetAttributes(item.Path);
            }
            catch (Exception exception) when (exception is IOException or UnauthorizedAccessException or SecurityException)
            {
                ReportFailure(updateProgress, item.Path, exception);
                continue;
            }

            if ((directoryAttributes & DirectoryAttribute) == 0 || (directoryAttributes & ReparsePoint) != 0)
            {
                ReportFailure(updateProgress, item.Path, new IOException("Percorso non directory o reparse point: elemento saltato."));
                continue;
            }

            if (!item.Path.Equals(root, StringComparison.OrdinalIgnoreCase))
            {
                pending.Push((item.Path, true));
            }

            IEnumerable<string> entries;
            try
            {
                entries = Directory.EnumerateFileSystemEntries(item.Path).ToArray();
            }
            catch (Exception exception) when (exception is IOException or UnauthorizedAccessException or SecurityException)
            {
                ReportFailure(updateProgress, item.Path, exception);
                continue;
            }

            foreach (string entry in entries)
            {
                try
                {
                    FileAttributes attributes = File.GetAttributes(entry);
                    if ((attributes & ReparsePoint) != 0)
                    {
                        continue;
                    }

                    if ((attributes & DirectoryAttribute) != 0)
                    {
                        pending.Push((entry, false));
                        continue;
                    }

                    File.Delete(entry);
                    updateProgress(0, $"{DateTime.Now:dd/MM/yyyy - HH.mm} 🗑️ {LanguageManager.GetTranslation("Cleaner", "file_deleted")}: {entry}\n");
                }
                catch (Exception exception) when (exception is IOException or UnauthorizedAccessException or SecurityException)
                {
                    ReportFailure(updateProgress, entry, exception);
                }
            }
        }
    }

    private static void ReportFailure(Action<int, string> updateProgress, string path, Exception exception) =>
        updateProgress(0, $"{DateTime.Now:dd/MM/yyyy - HH.mm} ⚠️ Elemento non pulito: {path}: {exception.Message}\n");
}
