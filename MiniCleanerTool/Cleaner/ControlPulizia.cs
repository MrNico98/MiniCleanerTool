using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Shell32;

namespace MiniCleanerTool
{
    public partial class ControlPulizia : UserControl
    {
        public ControlPulizia()
        {
            InitializeComponent();
            ThemeManager.ApplyThemeToControl(this, ThemeManager.IsDarkTheme);
            Verificaprofonda();
        }

        public void IniziaPulizia()
        {
            MiniCleanerTool.Cleaner.Cleaner.CleanSystem((progress, log) =>
            {
                SetProgress(progress);
                SetLog(log);
            });

        }

        private async void Verificaprofonda()
        {
            long recycleSize = GetRecycleBinSize();
            airCheckBox1.Text = $"{LanguageManager.GetTranslation("Cleaner", "cestino")}: {FormatSize(recycleSize)}";
            string downloadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
            long downloadSize = GetFolderSize(downloadPath);
            airCheckBox2.Text = $"Download: {FormatSize(downloadSize)}";
            string logFolderPath = Path.Combine(Environment.GetEnvironmentVariable("SystemRoot"), @"\System32\winevt\Logs");
            long totalLogSize = 0;

            if (Directory.Exists(logFolderPath))
            {
                DirectoryInfo logDirectory = new DirectoryInfo(logFolderPath);
                foreach (FileInfo file in logDirectory.GetFiles("*.evtx"))
                {
                    totalLogSize += file.Length;
                }
            }

            string localAppData = Environment.GetEnvironmentVariable("LOCALAPPDATA");
            if (Directory.Exists(localAppData))
            {
                DirectoryInfo localAppDataDir = new DirectoryInfo(localAppData);
                foreach (DirectoryInfo subDir in localAppDataDir.GetDirectories())
                {
                    if (subDir.Name.Contains("crash", StringComparison.OrdinalIgnoreCase) || subDir.Name.Contains("Cache", StringComparison.OrdinalIgnoreCase))
                    {
                        foreach (FileInfo file in subDir.GetFiles("*", SearchOption.AllDirectories))
                        {
                            totalLogSize += file.Length;
                        }
                    }
                }
            }

            airCheckBox3.Text = $"Crash Log: {FormatSize(totalLogSize)}";
            treeView1.Nodes.Clear();
            treeView1.Nodes.Add("Ricerca in corso...");

            var filesByDir = await Task.Run(() =>
            {
                string systemDrive = Path.GetPathRoot(Environment.SystemDirectory);
                return GetLargeFilesByDirectory(systemDrive, 2L * 1024 * 1024 * 1024);
            });

            treeView1.Nodes.Clear();

            foreach (var kvp in filesByDir)
            {
                string folderName = Path.GetFileName(kvp.Key);
                if (string.IsNullOrWhiteSpace(folderName))
                    folderName = kvp.Key.TrimEnd('\\');

                var dirNode = new TreeNode(folderName)
                {
                    ToolTipText = kvp.Key
                };

                foreach (var filePath in kvp.Value)
                {
                    var fileName = Path.GetFileName(filePath);
                    var fileNode = new TreeNode(fileName)
                    {
                        Tag = filePath,
                        ToolTipText = $"{filePath} - {new FileInfo(filePath).Length / (1024 * 1024)} MB"
                    };
                    dirNode.Nodes.Add(fileNode);
                }

                treeView1.Nodes.Add(dirNode);
            }

            treeView1.ExpandAll();
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
        private long GetFolderSize(string path)
        {
            try
            {
                return Directory.GetFiles(path, "*", SearchOption.AllDirectories)
                                .Sum(file => new FileInfo(file).Length);
            }
            catch
            {
                return 0;
            }
        }
        private Dictionary<string, List<string>> GetLargeFilesByDirectory(string rootPath, long minSize)
        {
            var result = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);
            var dirs = new Stack<string>();
            dirs.Push(rootPath);

            string[] excludedDirs = new string[]
            {
        Path.Combine(rootPath, "Windows"),
        Path.Combine(rootPath, "$Recycle.Bin"),
        Path.Combine(rootPath, "System Volume Information")
            };

            while (dirs.Count > 0)
            {
                string currentDir = dirs.Pop();

                if (excludedDirs.Any(ex => currentDir.StartsWith(ex, StringComparison.OrdinalIgnoreCase)))
                    continue;

                try
                {
                    foreach (string file in Directory.GetFiles(currentDir))
                    {
                        try
                        {
                            var fi = new FileInfo(file);
                            if (fi.Length >= minSize)
                            {
                                if (!result.ContainsKey(currentDir))
                                    result[currentDir] = new List<string>();

                                result[currentDir].Add(file);
                            }
                        }
                        catch { }
                    }

                    foreach (string dir in Directory.GetDirectories(currentDir))
                    {
                        dirs.Push(dir);
                    }
                }
                catch { }
            }

            return result;
        }

        struct SHQUERYRBINFO
        {
            public int cbSize;
            public long i64Size;
            public long i64NumItems;
        }

        [DllImport("Shell32.dll", CharSet = CharSet.Unicode)]
        static extern int SHQueryRecycleBin(string pszRootPath, ref SHQUERYRBINFO pSHQueryRBInfo);

        private long GetRecycleBinSize()
        {
            SHQUERYRBINFO rbInfo = new SHQUERYRBINFO();
            rbInfo.cbSize = Marshal.SizeOf(typeof(SHQUERYRBINFO));
            SHQueryRecycleBin(null, ref rbInfo);
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
                TreeNode selectedNode = treeView1.SelectedNode;

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

        private void materialButtonPulisci_Click(object sender, EventArgs e)
        {
            if (airCheckBox1.Checked)
            {
                try
                {
                    Shell shell = new Shell();
                    Folder folder = shell.NameSpace(10);
                    FolderItems items = folder.Items();

                    foreach (FolderItem item in items)
                    {
                        item.InvokeVerb("Empty Recycle Bin");
                    }
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
                    string downloadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
                    foreach (string file in Directory.GetFiles(downloadPath))
                    {
                        File.Delete(file);
                    }

                    foreach (string dir in Directory.GetDirectories(downloadPath))
                    {
                        Directory.Delete(dir, true);
                    }
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

    }
}
