using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniCleanerTool.Browser
{
    public static class BrowserCleaner
    {
        public delegate void ProgressReport(int progress, string log);

        public static async Task CleanAllBrowsers(ProgressReport reportProgress)
        {
            await Task.Delay(3000);
            int totalBrowsers = 8;
            int currentProgress = 0;

            reportProgress?.Invoke(currentProgress, "Starting browser cache cleaning...\r\n");

            try
            {
                reportProgress?.Invoke(++currentProgress * 100 / totalBrowsers, "Cleaning Chrome cache...\r\n");
                CleanChromeCache(reportProgress, ref currentProgress, totalBrowsers);

                reportProgress?.Invoke(++currentProgress * 100 / totalBrowsers, "Cleaning Edge cache...\r\n");
                CleanEdgeCache(reportProgress, ref currentProgress, totalBrowsers);

                reportProgress?.Invoke(++currentProgress * 100 / totalBrowsers, "Cleaning Firefox cache...\r\n");
                CleanFirefoxCache(reportProgress, ref currentProgress, totalBrowsers);

                reportProgress?.Invoke(++currentProgress * 100 / totalBrowsers, "Cleaning Opera cache...\r\n");
                CleanOperaCache(reportProgress, ref currentProgress, totalBrowsers);

                reportProgress?.Invoke(++currentProgress * 100 / totalBrowsers, "Cleaning Brave cache...\r\n");
                CleanBraveCache(reportProgress, ref currentProgress, totalBrowsers);

                reportProgress?.Invoke(++currentProgress * 100 / totalBrowsers, "Cleaning Vivaldi cache...\r\n");
                CleanVivaldiCache(reportProgress, ref currentProgress, totalBrowsers);

                reportProgress?.Invoke(++currentProgress * 100 / totalBrowsers, "Cleaning Safari cache...\r\n");
                CleanSafariCache(reportProgress, ref currentProgress, totalBrowsers);

                reportProgress?.Invoke(++currentProgress * 100 / totalBrowsers, "Cleaning IE cache...\r\n");
                CleanIECache(reportProgress, ref currentProgress, totalBrowsers);

                reportProgress?.Invoke(100, "Browser cache cleaning completed successfully!\r\n");
            }
            catch (Exception ex)
            {
                reportProgress?.Invoke(100, $"Error during cleaning: {ex.Message}\r\n");
            }
        }

        private static void CleanChromeCache(ProgressReport reportProgress, ref int currentProgress, int totalBrowsers)
        {
            string[] chromePaths = {
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Google\\Chrome\\User Data\\Default\\Cache"),
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Google\\Chrome\\User Data\\Default\\Media Cache"),
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Google\\Chrome\\User Data\\Default\\Service Worker\\CacheStorage"),
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Google\\Chrome\\User Data\\Default\\Code Cache")
            };

            foreach (var path in chromePaths)
            {
                DeleteDirectoryContents(path, reportProgress);
            }
        }

        public static void CleanEdgeCache(ProgressReport reportProgress, ref int currentProgress, int totalBrowsers)
        {
            string[] edgePaths = {
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Microsoft\\Edge\\User Data\\Default\\Cache"),
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Microsoft\\Edge\\User Data\\Default\\Media Cache"),
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Microsoft\\Edge\\User Data\\Default\\Service Worker\\CacheStorage"),
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Microsoft\\Edge\\User Data\\Default\\Code Cache")
        };

            foreach (var path in edgePaths)
            {
                DeleteDirectoryContents(path, reportProgress);
            }
        }

        public static void CleanFirefoxCache(ProgressReport reportProgress, ref int currentProgress, int totalBrowsers)
        {
            string firefoxPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Mozilla\\Firefox\\Profiles");
            if (Directory.Exists(firefoxPath))
            {
                foreach (string profileDir in Directory.GetDirectories(firefoxPath))
                {
                    string cachePath = Path.Combine(profileDir, "cache2");
                    string storagePath = Path.Combine(profileDir, "storage");
                    string thumbnailsPath = Path.Combine(profileDir, "thumbnails");

                    DeleteDirectoryContents(cachePath, reportProgress);
                    DeleteDirectoryContents(storagePath, reportProgress);
                    DeleteDirectoryContents(thumbnailsPath, reportProgress);
                }
            }
        }

        public static void CleanOperaCache(ProgressReport reportProgress, ref int currentProgress, int totalBrowsers)
        {
            string[] operaPaths = {
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Opera Software\\Opera Stable\\Cache"),
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Opera Software\\Opera Stable\\Media Cache"),
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Opera Software\\Opera Stable\\Service Worker\\CacheStorage"),
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Opera Software\\Opera Stable\\Code Cache")
        };

            foreach (var path in operaPaths)
            {
                DeleteDirectoryContents(path, reportProgress);
            }
        }

        public static void CleanBraveCache(ProgressReport reportProgress, ref int currentProgress, int totalBrowsers)
        {
            string[] bravePaths = {
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "BraveSoftware\\Brave-Browser\\User Data\\Default\\Cache"),
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "BraveSoftware\\Brave-Browser\\User Data\\Default\\Media Cache"),
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "BraveSoftware\\Brave-Browser\\User Data\\Default\\Service Worker\\CacheStorage"),
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "BraveSoftware\\Brave-Browser\\User Data\\Default\\Code Cache")
        };

            foreach (var path in bravePaths)
            {
                DeleteDirectoryContents(path, reportProgress);
            }
        }

        public static void CleanVivaldiCache(ProgressReport reportProgress, ref int currentProgress, int totalBrowsers)
        {
            string[] vivaldiPaths = {
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Vivaldi\\User Data\\Default\\Cache"),
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Vivaldi\\User Data\\Default\\Media Cache"),
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Vivaldi\\User Data\\Default\\Service Worker\\CacheStorage"),
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Vivaldi\\User Data\\Default\\Code Cache")
        };

            foreach (var path in vivaldiPaths)
            {
                DeleteDirectoryContents(path, reportProgress);
            }
        }

        public static void CleanSafariCache(ProgressReport reportProgress, ref int currentProgress, int totalBrowsers)
        {
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                string safariPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Apple Computer\\Safari");
                DeleteDirectoryContents(safariPath);
            }
        }

        public static void CleanIECache(ProgressReport reportProgress, ref int currentProgress, int totalBrowsers)
        {
            try
            {
                string ieCachePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.InternetCache));
                DeleteDirectoryContents(ieCachePath, reportProgress);
                string tempInternetFiles = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Microsoft\\Windows\\INetCache");
                DeleteDirectoryContents(tempInternetFiles, reportProgress);
            }
            catch (Exception)
            {

            }
        }

        private static void DeleteDirectoryContents(string path, ProgressReport reportProgress = null)
        {
            if (!Directory.Exists(path))
            {
                reportProgress?.Invoke(0, $"Directory not found: {path}\r\n");
                return;
            }

            try
            {
                reportProgress?.Invoke(0, $"{DateTime.Now:dd/MM/yyyy - HH.mm} Pulizia percorso: {path}\r\n");
                var files = Directory.GetFiles(path, "*", SearchOption.AllDirectories);
                for (int i = 0; i < files.Length; i++)
                {
                    try
                    {
                        File.SetAttributes(files[i], FileAttributes.Normal);
                        File.Delete(files[i]);
                        if (i % 10 == 0)
                        {
                            reportProgress?.Invoke(0, $"Eliminato {i + 1}/{files.Length} files in {path}\r\n");
                        }
                    }
                    catch (UnauthorizedAccessException) { }
                    catch (IOException) { }
                }
                var dirs = Directory.GetDirectories(path);
                for (int i = 0; i < dirs.Length; i++)
                {
                    try
                    {
                        Directory.Delete(dirs[i], true);
                        if (i % 2 == 0)
                        {
                            reportProgress?.Invoke(0, $"Eliminato {i + 1}/{dirs.Length} percorso in {path}\r\n");
                        }
                    }
                    catch (UnauthorizedAccessException) { }
                    catch (IOException) { }
                }

                reportProgress?.Invoke(0, $"{DateTime.Now:dd/MM/yyyy - HH.mm} Pulizia percorso completata : {path}\r\n");
            }
            catch (Exception ex)
            {
                reportProgress?.Invoke(0, $"{DateTime.Now:dd/MM/yyyy - HH.mm} Errore pulizia {path}: {ex.Message}\r\n");
            }
        }
    }
}