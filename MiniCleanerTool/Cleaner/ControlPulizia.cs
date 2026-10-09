using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MiniCleanerTool
{
    public partial class ControlPulizia : UserControl
    {
        private readonly CancellationTokenSource _analysisCancellation = new();

        public ControlPulizia()
        {
            InitializeComponent();
            ThemeManager.ApplyThemeToControl(this, ThemeManager.IsDarkTheme);
            airCheckBox3.Enabled = false;
            Disposed += (_, _) => _analysisCancellation.Cancel();
            Verificaprofonda();
        }

        public async Task IniziaPulizia()
        {
            await Task.Run(() => MiniCleanerTool.Cleaner.Cleaner.CleanSystem((progress, log) =>
            {
                SetProgress(progress);
                SetLog(log);
            }));

        }

        private async void Verificaprofonda()
        {
            treeView1.Nodes.Clear();
            treeView1.Nodes.Add("Analisi spazio e ricerca file > 2 GiB in corso...");
            try
            {
                var analysis = await Task.Run(() => AnalyzeStorage(_analysisCancellation.Token), _analysisCancellation.Token);
                if (IsDisposed) return;

                airCheckBox1.Text = $"{LanguageManager.GetTranslation("Cleaner", "cestino")}: {FormatSize(analysis.RecycleBinBytes)}";
                airCheckBox2.Text = $"Download: {FormatSize(analysis.DownloadBytes)}";
                airCheckBox3.Text = $"Crash Log (sola analisi): {FormatSize(analysis.LogAndCacheBytes)}";

                treeView1.Nodes.Clear();
                foreach (var kvp in analysis.LargeFilesByDirectory)
                {
                    string folderName = Path.GetFileName(kvp.Key);
                    if (string.IsNullOrWhiteSpace(folderName))
                        folderName = kvp.Key.TrimEnd(Path.DirectorySeparatorChar);

                    var dirNode = new TreeNode(folderName) { ToolTipText = kvp.Key };
                    foreach (var file in kvp.Value)
                    {
                        var fileNode = new TreeNode(Path.GetFileName(file.Path))
                        {
                            Tag = file.Path,
                            ToolTipText = $"{file.Path} - {file.SizeBytes / (1024 * 1024)} MB"
                        };
                        dirNode.Nodes.Add(fileNode);
                    }

                    treeView1.Nodes.Add(dirNode);
                }

                if (treeView1.Nodes.Count == 0)
                    treeView1.Nodes.Add("Nessun file da 2 GiB o più trovato nelle aree accessibili.");
                treeView1.ExpandAll();
            }
            catch (OperationCanceledException) { }
            catch (Exception exception)
            {
                if (!IsDisposed)
                {
                    treeView1.Nodes.Clear();
                    treeView1.Nodes.Add($"Analisi non completata: {exception.Message}");
                }
            }
        }

        private string FormatSize(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            double len = bytes;
            int order = 0;
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len /= 1024;
            }
            return $"{len:0.##} {sizes[order]}";
        }
        private sealed record StorageAnalysis(
            long RecycleBinBytes,
            long DownloadBytes,
            long LogAndCacheBytes,
            Dictionary<string, List<LargeFile>> LargeFilesByDirectory);

        private sealed record LargeFile(string Path, long SizeBytes);

        private StorageAnalysis AnalyzeStorage(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            string downloads = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
            string eventLogs = Path.Combine(Environment.SystemDirectory, "winevt", "Logs");
            string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            long logAndCacheBytes = GetDirectorySize(eventLogs, cancellationToken, "*.evtx");

            foreach (string directory in Directory.EnumerateDirectories(localAppData))
            {
                cancellationToken.ThrowIfCancellationRequested();
                string name = Path.GetFileName(directory);
                if (name.Contains("crash", StringComparison.OrdinalIgnoreCase) || name.Contains("cache", StringComparison.OrdinalIgnoreCase))
                    logAndCacheBytes += GetDirectorySize(directory, cancellationToken);
            }

            string systemDrive = Path.GetPathRoot(Environment.SystemDirectory)
                ?? throw new DirectoryNotFoundException("Impossibile individuare l'unità di sistema.");
            return new StorageAnalysis(
                GetRecycleBinSize(),
                GetDirectorySize(downloads, cancellationToken),
                logAndCacheBytes,
                GetLargeFilesByDirectory(systemDrive, 2L * 1024 * 1024 * 1024, cancellationToken));
        }

        private static long GetDirectorySize(string rootPath, CancellationToken cancellationToken, string pattern = "*")
        {
            if (!Directory.Exists(rootPath)) return 0;

            long total = 0;
            var pending = new Stack<string>();
            pending.Push(rootPath);
            while (pending.TryPop(out string? directory))
            {
                cancellationToken.ThrowIfCancellationRequested();
                try
                {
                    if ((File.GetAttributes(directory) & FileAttributes.ReparsePoint) != 0) continue;
                    foreach (string file in Directory.EnumerateFiles(directory, pattern, SearchOption.TopDirectoryOnly))
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                        try
                        {
                            if ((File.GetAttributes(file) & FileAttributes.ReparsePoint) == 0)
                                total += new FileInfo(file).Length;
                        }
                        catch (Exception exception) when (exception is IOException or UnauthorizedAccessException or System.Security.SecurityException) { }
                    }

                    foreach (string child in Directory.EnumerateDirectories(directory))
                        pending.Push(child);
                }
                catch (Exception exception) when (exception is IOException or UnauthorizedAccessException or System.Security.SecurityException) { }
            }

            return total;
        }

        private Dictionary<string, List<LargeFile>> GetLargeFilesByDirectory(string rootPath, long minSize, CancellationToken cancellationToken)
        {
            var result = new Dictionary<string, List<LargeFile>>(StringComparer.OrdinalIgnoreCase);
            var pending = new Stack<string>();
            string root = Path.TrimEndingDirectorySeparator(Path.GetFullPath(rootPath));
            string[] excludedDirectories = new[] { "Windows", "$Recycle.Bin", "System Volume Information" }
                .Select(name => Path.TrimEndingDirectorySeparator(Path.GetFullPath(Path.Combine(root, name))))
                .ToArray();
            pending.Push(root);

            while (pending.TryPop(out string? currentDirectory))
            {
                cancellationToken.ThrowIfCancellationRequested();
                string normalizedDirectory = Path.TrimEndingDirectorySeparator(Path.GetFullPath(currentDirectory));
                if (excludedDirectories.Any(excluded => normalizedDirectory.Equals(excluded, StringComparison.OrdinalIgnoreCase) ||
                    normalizedDirectory.StartsWith(excluded + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase))) continue;

                try
                {
                    if ((File.GetAttributes(normalizedDirectory) & FileAttributes.ReparsePoint) != 0) continue;
                    foreach (string file in Directory.EnumerateFiles(normalizedDirectory))
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                        try
                        {
                            if ((File.GetAttributes(file) & FileAttributes.ReparsePoint) == 0)
                            {
                                long length = new FileInfo(file).Length;
                                if (length >= minSize)
                                {
                                    if (!result.TryGetValue(normalizedDirectory, out List<LargeFile>? files))
                                        result[normalizedDirectory] = files = [];
                                    files.Add(new LargeFile(file, length));
                                }
                            }
                        }
                        catch (Exception exception) when (exception is IOException or UnauthorizedAccessException or System.Security.SecurityException) { }
                    }

                    foreach (string directory in Directory.EnumerateDirectories(normalizedDirectory))
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                        pending.Push(directory);
                    }
                }
                catch (Exception exception) when (exception is IOException or UnauthorizedAccessException or System.Security.SecurityException) { }
            }

            return result;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct SHQUERYRBINFO
        {
            public int cbSize;
            public long i64Size;
            public long i64NumItems;
        }

        [LibraryImport("Shell32.dll", EntryPoint = "SHEmptyRecycleBinW", StringMarshalling = StringMarshalling.Utf16)]
        private static partial int SHEmptyRecycleBin(IntPtr hwnd, string? rootPath, uint flags);

        [LibraryImport("Shell32.dll", EntryPoint = "SHQueryRecycleBinW", StringMarshalling = StringMarshalling.Utf16)]
        private static partial int SHQueryRecycleBin(string? rootPath, ref SHQUERYRBINFO info);

        private long GetRecycleBinSize()
        {
            SHQUERYRBINFO rbInfo = new() { cbSize = Marshal.SizeOf<SHQUERYRBINFO>() };
            int result = SHQueryRecycleBin(null, ref rbInfo);
            if (result < 0)
            {
                Marshal.ThrowExceptionForHR(result);
            }

            return rbInfo.i64Size;
        }

        private void SetProgress(int value)
        {
            if (materialProgressBar1.InvokeRequired)
            {
                materialProgressBar1.Invoke(new Action(() => materialProgressBar1.Value = value));
            }
            else
            {
                materialProgressBar1.Value = value;
            }
        }

        private void SetLog(string log)
        {
            if (richTextBox1.InvokeRequired)
            {
                richTextBox1.Invoke(new Action(() =>
                {
                    richTextBox1.AppendText(log);
                    richTextBox1.SelectionStart = richTextBox1.Text.Length;
                    richTextBox1.ScrollToCaret();
                }));
            }
            else
            {
                richTextBox1.AppendText(log);
                richTextBox1.SelectionStart = richTextBox1.Text.Length;
                richTextBox1.ScrollToCaret();
            }
        }

        private void treeView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                TreeNode? selectedNode = treeView1.SelectedNode;

                if (selectedNode?.Tag is string filePath && File.Exists(filePath))
                {
                    var result = MessageBox.Show($"Vuoi eliminare il file?\n\n{filePath}", "Conferma eliminazione",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (result == DialogResult.Yes)
                    {
                        try
                        {
                            File.Delete(filePath);
                            selectedNode.Remove();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error:\n{ex.Message}");
                        }
                    }
                }
            }
        }

        private async void materialButtonPulisci_Click(object sender, EventArgs e)
        {
            if (!airCheckBox1.Checked && !airCheckBox2.Checked)
            {
                MessageBox.Show("Seleziona almeno un elemento da pulire.", "Pulizia", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string selectedItems = string.Join(Environment.NewLine, new[]
            {
                airCheckBox1.Checked ? "• Cestino su tutte le unità" : null,
                airCheckBox2.Checked ? "• Tutti i file e le cartelle in Download" : null,
                null
            }.Where(item => item is not null));
            if (MessageBox.Show(
                    $"L'operazione può eliminare definitivamente i dati selezionati:{Environment.NewLine}{Environment.NewLine}{selectedItems}{Environment.NewLine}{Environment.NewLine}Vuoi continuare?",
                    "Conferma pulizia",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button2) != DialogResult.Yes)
            {
                return;
            }

            if (airCheckBox1.Checked)
            {
                try
                {
                    const uint flags = 0x00000001 | 0x00000002 | 0x00000004;
                    int result = SHEmptyRecycleBin(Handle, null, flags);
                    if (result < 0) Marshal.ThrowExceptionForHR(result);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }

            if (airCheckBox2.Checked)
            {
                try
                {
                    await Task.Run(() => CleanUserDownloads(SetLog));
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
            if (airCheckBox3.Checked)
            {

            }

            MessageBox.Show("Clean complete.");
        }

        private static void CleanUserDownloads(Action<string> report)
        {
            string root = Path.TrimEndingDirectorySeparator(Path.GetFullPath(
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads")));
            if (!Directory.Exists(root)) return;
            if ((File.GetAttributes(root) & FileAttributes.ReparsePoint) != 0)
                throw new IOException("La cartella Download è un reparse point: pulizia annullata.");

            var pending = new Stack<(string Path, bool RemoveAfterChildren)>();
            pending.Push((root, false));
            while (pending.TryPop(out var item))
            {
                if (item.RemoveAfterChildren)
                {
                    Directory.Delete(item.Path, recursive: false);
                    continue;
                }

                if ((File.GetAttributes(item.Path) & FileAttributes.ReparsePoint) != 0)
                {
                    report($"Percorso collegato saltato: {item.Path}\r\n");
                    continue;
                }

                if (!item.Path.Equals(root, StringComparison.OrdinalIgnoreCase))
                    pending.Push((item.Path, true));

                foreach (string entry in Directory.EnumerateFileSystemEntries(item.Path))
                {
                    FileAttributes attributes = File.GetAttributes(entry);
                    if ((attributes & FileAttributes.ReparsePoint) != 0)
                    {
                        report($"Percorso collegato saltato: {entry}\r\n");
                    }
                    else if ((attributes & FileAttributes.Directory) != 0)
                    {
                        pending.Push((entry, false));
                    }
                    else
                    {
                        File.Delete(entry);
                    }
                }
            }
        }

    }
}
