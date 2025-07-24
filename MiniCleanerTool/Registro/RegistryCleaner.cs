using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace MiniCleanerTool.Registro
{
    public static class RegistryCleaner
    {
        public delegate void ProgressLogCallback(int progress, string log);
        [DllImport("advapi32.dll", CharSet = CharSet.Auto)]
        public static extern int RegDeleteKey(IntPtr hKey, string subKey);

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern int RegLoadKey(IntPtr hKey, string lpSubKey, string lpFile);

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern int RegUnLoadKey(IntPtr hKey, string lpSubKey);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr LoadLibrary(string dllToLoad);

        public static async Task StartRegedit(ProgressLogCallback callback)
        {
            try
            {
                await Task.Delay(2000);
                callback(0, $"{DateTime.Now:dd/MM/yyyy - HH.mm} {LanguageManager.GetTranslation("Regedit", "start_cleaning_registry")}\n");
                await Task.Delay(2000);
                CheckMissingDlls(callback);
                await Task.Delay(3000);
                CleanInvalidStartupEntries(callback);
                await Task.Delay(3000);
                CleanUserAssistHistory(callback);
                await Task.Delay(3000);
                CleanRecentDocsHistory(callback);
                await Task.Delay(3000);
                CleanOrphanedServices(callback);
                await Task.Delay(3000);
                CleanActiveXControls(callback);
                await Task.Delay(3000);
                CleanInvalidCLSID(callback);
                await Task.Delay(3000);
                CleanRegistry(callback);
                await Task.Delay(2000);
                callback(100, $"{DateTime.Now:dd/MM/yyyy - HH.mm} {LanguageManager.GetTranslation("Regedit", "cleaning_completed_successfully")}\n");
            }
            catch (Exception ex)
            {
                callback(-1, $"{DateTime.Now:dd/MM/yyyy - HH.mm} {LanguageManager.GetTranslation("Regedit", "error_during_cleaning")}: {ex.Message}\n");
            }
        }

        private static void CleanOrphanedServices(ProgressLogCallback callback)
        {
            try
            {
                using RegistryKey servicesKey = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Services", writable: true);
                foreach (string subKey in servicesKey.GetSubKeyNames())
                {
                    using RegistryKey service = servicesKey.OpenSubKey(subKey);
                    if (service?.GetValue("ImagePath") == null)
                    {
                        servicesKey.DeleteSubKeyTree(subKey, false);
                        callback(0, $"✔ {LanguageManager.GetTranslation("Regedit", "orphaned_service_removed")}: {subKey}\n");
                    }
                }
            }
            catch (Exception ex)
            {
                callback(0, $"❌ {LanguageManager.GetTranslation("Regedit", "error_cleaning_services")}: {ex.Message}\n");
            }
        }

        private static void CleanInvalidCLSID(ProgressLogCallback callback)
        {
            try
            {
                using RegistryKey clsidKey = Registry.ClassesRoot.OpenSubKey(@"CLSID", writable: true);
                foreach (string subKey in clsidKey.GetSubKeyNames())
                {
                    using RegistryKey currentKey = clsidKey.OpenSubKey(subKey);
                    if (currentKey == null || currentKey.GetValue("") == null)
                    {
                        clsidKey.DeleteSubKeyTree(subKey, false);
                        callback(0, $"✔ {LanguageManager.GetTranslation("Regedit", "orphaned_clsid_removed")}: {subKey}\n");
                    }
                }
            }
            catch (Exception ex)
            {
                callback(0, $"❌ {LanguageManager.GetTranslation("Regedit", "error_cleaning_clsid")}: {ex.Message}\n");
            }
        }

        private static void CleanActiveXControls(ProgressLogCallback callback)
        {
            try
            {
                using RegistryKey activeXKey = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\SharedDLLs", writable: true);
                if (activeXKey != null)
                {
                    foreach (string valueName in activeXKey.GetValueNames())
                    {
                        int usageCount = Convert.ToInt32(activeXKey.GetValue(valueName));
                        if (usageCount == 0)
                        {
                            activeXKey.DeleteValue(valueName, false);
                            callback(0, $"✔ {LanguageManager.GetTranslation("Regedit", "unused_dll_removed")}: {valueName}\n");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                callback(0, $"❌ {LanguageManager.GetTranslation("Regedit", "error_cleaning_activex")}: {ex.Message}\n");
            }
        }

        private static void CleanRegistry(ProgressLogCallback callback)
        {
            try
            {
                callback(0, $"{DateTime.Now:dd/MM/yyyy - HH.mm} {LanguageManager.GetTranslation("Regedit", "start_cleaning_empty_keys")}\n");

                string[] registryPathsToClean = {
            @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall",
            @"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\RunMRU",
            @"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\TypedPaths",
            @"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Shell Folders",
            @"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\RecentDocs",
            @"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\ComDlg32\OpenSaveMRU",
            @"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\ComDlg32\LastVisitedMRU"
        };

                CleanEmptyKeysInHive(Registry.LocalMachine, registryPathsToClean, callback);
                CleanEmptyKeysInHive(Registry.CurrentUser, registryPathsToClean, callback);

                callback(100, $"{DateTime.Now:dd/MM/yyyy - HH.mm} {LanguageManager.GetTranslation("Regedit", "empty_keys_cleaning_completed")}\n");
            }
            catch (Exception ex)
            {
                callback(-1, $"{DateTime.Now:dd/MM/yyyy - HH.mm} {LanguageManager.GetTranslation("Regedit", "error_cleaning_empty_keys")}: {ex.Message}\n");
            }
        }


        private static void CleanEmptyKeysInHive(RegistryKey hiveRoot, string[] pathsToClean, ProgressLogCallback callback)
        {
            foreach (var registryPath in pathsToClean)
            {
                try
                {
                    using (var key = hiveRoot.OpenSubKey(registryPath, true))
                    {
                        if (key == null)
                        {
                            continue;
                        }
                        var subKeyNames = key.GetSubKeyNames();
                        foreach (var subKeyName in subKeyNames)
                        {
                            try
                            {
                                using (var subKey = key.OpenSubKey(subKeyName, true))
                                {
                                    if (subKey == null) continue;
                                    if (IsKeyEmpty(subKey))
                                    {
                                        try
                                        {
                                            key.DeleteSubKey(subKeyName, false);
                                            callback(15, $"{DateTime.Now:dd/MM/yyyy - HH.mm} {LanguageManager.GetTranslation("Regedit", "removed_empty_key")}: {hiveRoot.Name}\\{registryPath}\\{subKeyName}\n");
                                        }
                                        catch (Exception ex)
                                        {

                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {

                            }
                        }

                        if (IsKeyEmpty(key))
                        {
                            try
                            {
                                hiveRoot.DeleteSubKeyTree(registryPath);
                                callback(20, $"{DateTime.Now:dd/MM/yyyy - HH.mm} {LanguageManager.GetTranslation("Regedit", "removed_empty_main_key")}: {hiveRoot.Name}\\{registryPath}\n");
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                    }
                }
                catch (Exception ex)
                {

                }
            }
        }

        private static bool IsKeyEmpty(RegistryKey key)
        {
            if (key.GetValueNames().Length > 0)
                return false;

            var subKeyNames = key.GetSubKeyNames();
            foreach (var subKeyName in subKeyNames)
            {
                using (var subKey = key.OpenSubKey(subKeyName))
                {
                    if (subKey != null && !IsKeyEmpty(subKey))
                        return false;
                }
            }

            return true;
        }


        private static void CheckMissingDlls(ProgressLogCallback callback)
        {
            callback(10, $"{DateTime.Now:dd/MM/yyyy - HH.mm} {LanguageManager.GetTranslation("Regedit", "verificadllmancanti")}\n");
            CheckMissingDllsInKey(Registry.LocalMachine, @"Software\Microsoft\Windows\CurrentVersion\SharedDLLs", callback);
            CheckMissingDllsInKey(Registry.CurrentUser, @"Software\Microsoft\Windows\CurrentVersion\SharedDLLs", callback);
            CheckMissingDllsInRunEntries(callback);
        }

        private static void CheckMissingDllsInKey(RegistryKey rootKey, string subKeyPath, ProgressLogCallback callback)
        {
            try
            {
                using (var key = rootKey.OpenSubKey(subKeyPath, true))
                {
                    if (key == null) return;

                    var valueNames = key.GetValueNames();
                    foreach (var valueName in valueNames)
                    {
                        try
                        {
                            var dllPath = valueName;
                            if (!File.Exists(dllPath))
                            {
                                key.DeleteValue(valueName);
                                callback(15, $"{LanguageManager.GetTranslation("Regedit", "rimossavoceperdll")}: {dllPath}\n");
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private static void CheckMissingDllsInRunEntries(ProgressLogCallback callback)
        {
            var runKeys = new[]
            {
                @"Software\Microsoft\Windows\CurrentVersion\Run",
                @"Software\Microsoft\Windows\CurrentVersion\RunOnce",
                @"Software\Microsoft\Windows\CurrentVersion\RunServices",
                @"Software\Microsoft\Windows\CurrentVersion\RunServicesOnce"
            };

            foreach (var runKey in runKeys)
            {
                try
                {
                    using (var key = Registry.CurrentUser.OpenSubKey(runKey, true))
                    {
                        if (key == null) continue;

                        var valueNames = key.GetValueNames();
                        foreach (var valueName in valueNames)
                        {
                            try
                            {
                                var value = key.GetValue(valueName)?.ToString();
                                if (string.IsNullOrEmpty(value)) continue;
                                var filePath = value.Split(' ')[0].Trim('"');
                                if (!File.Exists(filePath))
                                {
                                    callback(20, $"{LanguageManager.GetTranslation("Regedit", "trovatoesecuzionemancante")}: {filePath}\n");
                                    key.DeleteValue(valueName);
                                    callback(20, $"{LanguageManager.GetTranslation("Regedit", "rimossavocesecuzionemancante")}: {filePath}\n");
                                }
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                    }
                    using (var key = Registry.LocalMachine.OpenSubKey(runKey, true))
                    {
                        if (key == null) continue;

                        var valueNames = key.GetValueNames();
                        foreach (var valueName in valueNames)
                        {
                            try
                            {
                                var value = key.GetValue(valueName)?.ToString();
                                if (string.IsNullOrEmpty(value)) continue;

                                var filePath = value.Split(' ')[0].Trim('"');
                                if (!File.Exists(filePath))
                                {
                                    callback(20, $"{LanguageManager.GetTranslation("Regedit", "trovatoesecuzionemancantehkml")}: {filePath}\n");
                                    key.DeleteValue(valueName);
                                    callback(20, $"{LanguageManager.GetTranslation("Regedit", "rimossavocesecuzionemancantehkml")}: {filePath}\n");
                                }
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                    }
                }
                catch (Exception ex)
                {

                }
            }
        }

        private static void CleanInvalidStartupEntries(ProgressLogCallback callback)
        {
            callback(40, $"{DateTime.Now:dd/MM/yyyy - HH.mm} {LanguageManager.GetTranslation("Regedit", "puliziavocidiavviononvalide")}\n");

            var startupKeys = new[]
            {
        @"Software\Microsoft\Windows\CurrentVersion\Run",
        @"Software\Microsoft\Windows\CurrentVersion\RunOnce",
        @"Software\Microsoft\Windows\CurrentVersion\RunServices",
        @"Software\Microsoft\Windows\CurrentVersion\RunServicesOnce"
    };

            foreach (var startupKey in startupKeys)
            {
                try
                {
                    using (var key = Registry.CurrentUser.OpenSubKey(startupKey, true))
                    {
                        if (key == null) continue;

                        var valueNames = key.GetValueNames();
                        foreach (var valueName in valueNames)
                        {
                            try
                            {
                                var value = key.GetValue(valueName)?.ToString();
                                if (string.IsNullOrEmpty(value)) continue;

                                var filePath = value.Split(' ')[0].Trim('"');
                                if (!File.Exists(filePath))
                                {
                                    callback(45, $"{LanguageManager.GetTranslation("Regedit", "trovatoesecuzionemancante")}: {valueName}\n");
                                    key.DeleteValue(valueName);
                                    callback(45, $"{LanguageManager.GetTranslation("Regedit", "rimossavocesecuzionemancante")}: {valueName}\n");
                                }
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                    }
                    using (var key = Registry.LocalMachine.OpenSubKey(startupKey, true))
                    {
                        if (key == null) continue;

                        var valueNames = key.GetValueNames();
                        foreach (var valueName in valueNames)
                        {
                            try
                            {
                                var value = key.GetValue(valueName)?.ToString();
                                if (string.IsNullOrEmpty(value)) continue;

                                var filePath = value.Split(' ')[0].Trim('"');
                                if (!File.Exists(filePath))
                                {
                                    callback(45, $"{LanguageManager.GetTranslation("Regedit", "trovavocidiavviomancantehkml")}: {valueName}\n");
                                    key.DeleteValue(valueName);
                                    callback(45, $"{LanguageManager.GetTranslation("Regedit", "rimossavocesecuzionemancantehkml")}: {valueName}\n");
                                }
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                    }
                }
                catch (Exception ex)
                {

                }
            }
        }

        private static void CleanUserAssistHistory(ProgressLogCallback callback)
        {
            callback(50, $"{DateTime.Now:dd/MM/yyyy - HH.mm} {LanguageManager.GetTranslation("Regedit", "puliziacronologiauserassist")}\n");

            try
            {
                var userAssistKeyPath = @"Software\Microsoft\Windows\CurrentVersion\Explorer\UserAssist";
                using (var userAssistKey = Registry.CurrentUser.OpenSubKey(userAssistKeyPath, true))
                {
                    if (userAssistKey == null) return;

                    var subKeyNames = userAssistKey.GetSubKeyNames();
                    foreach (var subKeyName in subKeyNames)
                    {
                        try
                        {
                            using (var subKey = userAssistKey.OpenSubKey(subKeyName, true))
                            {
                                if (subKey == null) continue;

                                var valueNames = subKey.GetValueNames();
                                foreach (var valueName in valueNames)
                                {
                                    try
                                    {
                                        subKey.DeleteValue(valueName);
                                    }
                                    catch (Exception ex)
                                    {
                                        callback(55, $"{LanguageManager.GetTranslation("Regedit", "errorerimozionevalore")} '{valueName}' ({subKeyName}): {ex.Message}\n");
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            callback(55, $"{LanguageManager.GetTranslation("Regedit", "erroreaccessosottochiave")} {subKeyName}: {ex.Message}\n");
                        }
                    }
                }

                callback(55, $"{DateTime.Now:dd/MM/yyyy - HH.mm} {LanguageManager.GetTranslation("Regedit", "cronologiauserassistpulita")}\n");
            }
            catch (Exception ex)
            {
                callback(55, $"{LanguageManager.GetTranslation("Regedit", "errorepuliziacronologiauserassist")}: {ex.Message}\n");
            }
        }

        private static void CleanRecentDocsHistory(ProgressLogCallback callback)
        {
            callback(60, $"{DateTime.Now:dd/MM/yyyy - HH.mm} {LanguageManager.GetTranslation("Regedit", "puliziacronologiarecentdocs")}\n");

            try
            {
                var recentDocsKeyPath = @"Software\Microsoft\Windows\CurrentVersion\Explorer\RecentDocs";
                using (var recentDocsKey = Registry.CurrentUser.OpenSubKey(recentDocsKeyPath, true))
                {
                    if (recentDocsKey == null) return;

                    var valueNames = recentDocsKey.GetValueNames();
                    foreach (var valueName in valueNames)
                    {
                        try
                        {
                            recentDocsKey.DeleteValue(valueName);
                        }
                        catch (Exception ex)
                        {
                            callback(65, $"{LanguageManager.GetTranslation("Regedit", "errorerimozionevalore")} '{valueName}': {ex.Message}\n");
                        }
                    }

                    var subKeyNames = recentDocsKey.GetSubKeyNames();
                    foreach (var subKeyName in subKeyNames)
                    {
                        try
                        {
                            Registry.CurrentUser.DeleteSubKeyTree($@"{recentDocsKeyPath}\{subKeyName}");
                        }
                        catch (Exception ex)
                        {
                            callback(65, $"{LanguageManager.GetTranslation("Regedit", "erroreaccessosottochiave")} {subKeyName}: {ex.Message}\n");
                        }
                    }
                }

                callback(65, $"{DateTime.Now:dd/MM/yyyy - HH.mm} {LanguageManager.GetTranslation("Regedit", "cronologiarecentdocspulita")}\n");
            }
            catch (Exception ex)
            {
                callback(65, $"{LanguageManager.GetTranslation("Regedit", "errorepuliziacronologiarecentdocs")}: {ex.Message}\n");
            }
        }
    }
}