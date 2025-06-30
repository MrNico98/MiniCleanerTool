using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MiniCleanerTool.Cleaner
{
    public static class Cleaner
    {
        public static async Task CleanSystem(Action<int, string> updateProgress)
        {
            await Task.Delay(3000);

            List<string> directoriesToClean = new List<string>
        {
            Path.GetTempPath(),
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "Temp"),
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"Microsoft\Windows\Recent"),
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CrashDumps"),
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"Microsoft\Windows\WebCache"),
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"Microsoft\Windows\INetCache"),
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"Microsoft\Windows\INetCookies"),
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), @"SoftwareDistribution\Download"),
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), @"Prefetch"),
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), @"System32\config\systemprofile\AppData\Local\Microsoft\Windows\DeliveryOptimization"),
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), @"Microsoft\Windows Defender\Scans"),
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), @"AppData\Local\D3DSCache"),
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), @"AppData\Local\Microsoft\Windows\History")
        };

            var fileTasks = directoriesToClean
                .Where(Directory.Exists)
                .Select(dir => Task.Run(() => CleanDirectory(dir, updateProgress)))
                .ToArray();

            await Task.WhenAll(fileTasks);

            updateProgress(100, $"✔️ {LanguageManager.GetTranslation("Cleaner", "cleaning_complete_all")}\n");
        }

        private static void CleanDirectory(string dir, Action<int, string> updateProgress)
        {
            List<string> filesToDelete = new List<string>();
            List<string> dirsToDelete = new List<string>();

            try
            {
                filesToDelete.AddRange(Directory.GetFiles(dir, "*", SearchOption.AllDirectories));
                dirsToDelete.AddRange(Directory.GetDirectories(dir, "*", SearchOption.AllDirectories));

                int totalItems = filesToDelete.Count + dirsToDelete.Count;
                int processedItems = 0;

                Parallel.ForEach(filesToDelete, (file) =>
                {
                    try
                    {
                        File.Delete(file);
                        Interlocked.Increment(ref processedItems);
                        int progress = (int)((double)processedItems / totalItems * 100);
                        updateProgress(progress, $"{DateTime.Now:dd/MM/yyyy - HH.mm} 🗑️ {LanguageManager.GetTranslation("Cleaner", "file_deleted")}: {file}\n");
                    }
                    catch
                    {
                        Interlocked.Increment(ref processedItems);
                    }
                });

                Parallel.ForEach(dirsToDelete.OrderByDescending(d => d.Length), (dirToDelete) =>
                {
                    try
                    {
                        Directory.Delete(dirToDelete, true);
                        Interlocked.Increment(ref processedItems);
                        int progress = (int)((double)processedItems / totalItems * 100);
                        updateProgress(progress, $"{DateTime.Now:dd/MM/yyyy - HH.mm} 🗑️ {LanguageManager.GetTranslation("Cleaner", "folder_deleted")}: {dirToDelete}\n");
                    }
                    catch
                    {
                        Interlocked.Increment(ref processedItems);
                    }
                });
            }
            catch
            {

            }
        }
    }
}
