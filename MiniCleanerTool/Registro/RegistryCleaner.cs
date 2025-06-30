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
                await Task.Delay(3000);
                callback(0, $"{DateTime.Now:dd/MM/yyyy - HH.mm} Inizio pulizia del registro...\n");
                await Task.Delay(3000);
                CheckMissingDlls(callback);
                await Task.Delay(5000);
                CleanInvalidUninstallEntries(callback);
                await Task.Delay(5000);
                CleanInvalidStartupEntries(callback);
                await Task.Delay(5000);
                CleanUserAssistHistory(callback);
                await Task.Delay(5000);
                CleanRecentDocsHistory(callback);
                await Task.Delay(5000);
                CleanRegistry(callback);
                await Task.Delay(2000);
                callback(100, $"{DateTime.Now:dd/MM/yyyy - HH.mm} Pulizia del registro completata con successo!\n");
            }
            catch (Exception ex)
            {
                callback(-1, $"{DateTime.Now:dd/MM/yyyy - HH.mm} ERRORE durante la pulizia: {ex.Message}\n");
            }
        }
        private static void CleanRegistry(ProgressLogCallback callback)
        {
            try
            {
                callback(0, $"{DateTime.Now:dd/MM/yyyy - HH.mm} Inizio pulizia delle chiavi di registro vuote...\n");
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

                callback(100, $"{DateTime.Now:dd/MM/yyyy - HH.mm} Pulizia delle chiavi vuote completata con successo!\n");
            }
            catch (Exception ex)
            {
                callback(-1, $"{DateTime.Now:dd/MM/yyyy - HH.mm} ERRORE durante la pulizia delle chiavi vuote: {ex.Message}\n");
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
                                            callback(15, $"Rimossa chiave vuota: {hiveRoot.Name}\\{registryPath}\\{subKeyName}\n");
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
                                callback(20, $"Rimossa chiave principale vuota: {hiveRoot.Name}\\{registryPath}\n");
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
            callback(10, $"{DateTime.Now:dd/MM/yyyy - HH.mm} Verifica delle DLL mancanti nelle voci di registro...\n");
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
                                callback(15, $"Trovata DLL mancante: {dllPath}\n");
                                key.DeleteValue(valueName);
                                callback(15, $"Rimossa voce per DLL mancante: {dllPath}\n");
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
                                    callback(20, $"Trovata voce di esecuzione con file mancante: {filePath}\n");
                                    key.DeleteValue(valueName);
                                    callback(20, $"Rimossa voce di esecuzione per file mancante: {filePath}\n");
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
                                    callback(20, $"Trovata voce di esecuzione con file mancante (HKLM): {filePath}\n");
                                    key.DeleteValue(valueName);
                                    callback(20, $"Rimossa voce di esecuzione per file mancante (HKLM): {filePath}\n");
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
                    callback(20, $"Errore durante l'accesso a {runKey}: {ex.Message}\n");
                }
            }
        }

        private static void CleanInvalidUninstallEntries(ProgressLogCallback callback)
        {
            callback(30, $"{DateTime.Now:dd/MM/yyyy - HH.mm} Pulizia delle voci di disinstallazione non valide\n");

            try
            {
                using (var uninstallKey = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Uninstall", false))
                {
                    if (uninstallKey == null) return;

                    var subKeyNames = uninstallKey.GetSubKeyNames();
                    foreach (var subKeyName in subKeyNames)
                    {
                        try
                        {
                            using (var appKey = uninstallKey.OpenSubKey(subKeyName, true))
                            {
                                if (appKey == null) continue;
                                var displayName = appKey.GetValue("DisplayName")?.ToString();
                                var installLocation = appKey.GetValue("InstallLocation")?.ToString();
                                var uninstallString = appKey.GetValue("UninstallString")?.ToString();

                                if (string.IsNullOrEmpty(displayName)) continue;

                                bool isValid = true;
                                if (!string.IsNullOrEmpty(installLocation) && !Directory.Exists(installLocation))
                                {
                                    isValid = false;
                                    callback(35, $"Trovata voce di disinstallazione con percorso non valido: {displayName}\n");
                                }

                                if (!string.IsNullOrEmpty(uninstallString))
                                {
                                    var uninstallExe = uninstallString.Split(' ')[0].Trim('"');
                                    if (!File.Exists(uninstallExe))
                                    {
                                        isValid = false;
                                        callback(35, $"Trovata voce di disinstallazione con comando non valido: {displayName}\n");
                                    }
                                }

                                if (!isValid)
                                {
                                    try
                                    {
                                        Registry.LocalMachine.DeleteSubKeyTree($@"Software\Microsoft\Windows\CurrentVersion\Uninstall\{subKeyName}");
                                        callback(35, $"Rimossa voce di disinstallazione non valida: {displayName}\n");
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
            }
            catch (Exception ex)
            {

            }
        }

        private static void CleanInvalidStartupEntries(ProgressLogCallback callback)
        {
            callback(40, $"{DateTime.Now:dd/MM/yyyy - HH.mm} Pulizia delle voci di avvio non valide\n");

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
                                    callback(45, $"Trovata voce di avvio con file mancante: {valueName}\n");
                                    key.DeleteValue(valueName);
                                    callback(45, $"Rimossa voce di avvio non valida: {valueName}\n");
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
                                    callback(45, $"Trovata voce di avvio con file mancante (HKLM): {valueName}\n");
                                    key.DeleteValue(valueName);
                                    callback(45, $"Rimossa voce di avvio non valida (HKLM): {valueName}\n");
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
            callback(50, $"{DateTime.Now:dd/MM/yyyy - HH.mm} Pulizia della cronologia UserAssist\n");

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

                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
                callback(55, $"{DateTime.Now:dd/MM/yyyy - HH.mm} Cronologia UserAssist pulita.\n");
            }
            catch (Exception ex)
            {
                callback(55, $"Errore durante la pulizia della cronologia UserAssist: {ex.Message}\n");
            }
        }

        private static void CleanRecentDocsHistory(ProgressLogCallback callback)
        {
            callback(60, $"{DateTime.Now:dd/MM/yyyy - HH.mm} Pulizia della cronologia RecentDocs\n");

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
 
                        }
                    }
                }
                callback(65, $"{DateTime.Now:dd/MM/yyyy - HH.mm} Cronologia RecentDocs pulita.\n");
            }
            catch (Exception ex)
            {

            }
        }
    }
}