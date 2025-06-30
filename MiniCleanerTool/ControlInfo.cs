using ReaLTaiizor.Forms;
using System.Diagnostics;
using System.Globalization;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MiniCleanerTool
{
    public partial class ControlInfo : UserControl
    {
        private static readonly HttpClient client = new HttpClient();
        private string currentVersion = "0.0.0.0";
        private string updateUrl;
        private string latestVersion;
        private UpdateInfo _updateInfo;
        public event Action OnUpdateAvailable;
        public bool isloading = true;

        public ControlInfo(bool dark)
        {
            InitializeComponent();
            ThemeManager.ApplyThemeToControl(this, ThemeManager.IsDarkTheme);
            metroSwitch1.Switched = dark;
            dungeonLabel4.Text = dark ? "Tema scuro" : "Tema chiaro";
            lblClient.Text = $"Versione Client: {currentVersion}";
            CheckForUpdatesOnStartup();
            string languageCode = Properties.Settings.Default.Language ?? "en";
            if (isloading == true)
            {
                aloneComboBox1.SelectedIndexChanged -= aloneComboBox1_SelectedIndexChanged;

                if (languageCode == "it")
                {
                    aloneComboBox1.SelectedItem = "IT";
                    pictureBox3.Image = Properties.Resources.italy;
                }
                else if (languageCode == "en")
                {
                    aloneComboBox1.SelectedItem = "EN";
                    pictureBox3.Image = Properties.Resources.united_kingdom;
                }

                aloneComboBox1.SelectedIndexChanged += aloneComboBox1_SelectedIndexChanged;
            }

        }

        private void metroSwitch1_SwitchedChanged(object sender)
        {
            bool dark = metroSwitch1.Switched;
            ThemeManager.SetTheme(dark);
            dungeonLabel4.Text = dark ? "Tema scuro" : "Tema chiaro";
            Properties.Settings.Default.DarkTheme = dark;
            Properties.Settings.Default.Save();
        }
        public async void CheckForUpdatesOnStartup() => await CheckForUpdatesAsync();

        private async Task CheckForUpdatesAsync()
        {
            string configUrl = "https://raw.githubusercontent.com/MrNico98/MiniCleanerTool/refs/heads/main/MiniCleanerToolUpdateFile.json";

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string json = await client.GetStringAsync(configUrl);
                    _updateInfo = JsonSerializer.Deserialize<UpdateInfo>(json);
                    lblServer.Text = $"Versione Server: {_updateInfo.version}";
                    if (_updateInfo.version != currentVersion)
                    {
                        btnAggiorna.Visible = true;
                        OnUpdateAvailable?.Invoke();
                    }
                    else
                    {
                        btnAggiorna.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "MiniCleanerTool");
            }
        }

        private async Task DownloadAndUpdate(string updateUrl, string version)
        {
            string updateFilePath = Path.Combine(Path.GetTempPath(), $"MiniCreationTool{version}.exe");
            using (var progressForm = new ProgressForm())
            {
                progressForm.Show();
                progressForm.SetMarquee();
                try
                {
                    await DownloadFileWithProgress(updateUrl, updateFilePath, progressForm);
                    string currentExecutablePath = Application.ExecutablePath;
                    File.Move(currentExecutablePath, Path.ChangeExtension(currentExecutablePath, ".old"), true);
                    File.Move(updateFilePath, currentExecutablePath);
                    Process.Start(currentExecutablePath);
                    Application.Exit();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Errore durante l'aggiornamento: {ex.Message}", "Errore");
                }
                finally
                {
                    progressForm.CompleteOperation();
                }
            }
        }

        private async Task DownloadFileWithProgress(string url, string filePath, ProgressForm progressForm)
        {
            using var response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();
            var totalBytes = response.Content.Headers.ContentLength.GetValueOrDefault();
            using var contentStream = await response.Content.ReadAsStreamAsync();
            using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, 8192, true);
            var buffer = new byte[8192];
            long bytesRead = 0;
            int read;
            while ((read = await contentStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
            {
                await fileStream.WriteAsync(buffer, 0, read);
                bytesRead += read;
                progressForm.Invoke(new Action(() => progressForm.SetStatus("Download in corso...", (int)((bytesRead * 100) / totalBytes))));
            }
        }

        private async void btnAggiorna_Click(object sender, EventArgs e)
        {
            string releaseNotes = string.Join("\n", _updateInfo.releaseNotes["IT"]);
            var dialogResult = MessageBox.Show(
                $"Nuova versione disponibile: {_updateInfo.version}\n\nNote di rilascio:\n{releaseNotes}\n\nVuoi aggiornare ora?",
                "Aggiornamento Disponibile",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (dialogResult == DialogResult.Yes)
            {
                await DownloadAndUpdate(_updateInfo.updateUrl, _updateInfo.version);
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "https://ko-fi.com/mrnico98",
                    UseShellExecute = true
                };
                Process.Start(psi);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Errore nell'aprire l'URL: {ex.Message}");
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "https://github.com/MrNico98/MiniCleanerTool",
                    UseShellExecute = true
                };
                Process.Start(psi);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            string selectedLanguage = aloneComboBox1.SelectedItem?.ToString().ToLower();
            ChangeLanguageAndPromptRestart(selectedLanguage);
        }

        private void aloneComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedLanguage = aloneComboBox1.SelectedItem?.ToString().ToLower();
            ChangeLanguageAndPromptRestart(selectedLanguage);
        }


        private void ChangeLanguageAndPromptRestart(string selectedLanguage)
        {
            if (string.IsNullOrEmpty(selectedLanguage)) return;

            LanguageManager.SetLanguage(selectedLanguage);
            Properties.Settings.Default.Language = selectedLanguage;
            Properties.Settings.Default.Save();

            Thread.CurrentThread.CurrentUICulture = new CultureInfo(selectedLanguage);
            Thread.CurrentThread.CurrentCulture = new CultureInfo(selectedLanguage);

            DialogResult result = MessageBox.Show(
                LanguageManager.GetTranslation("ControlInfo", "restart_required_message"),
                LanguageManager.GetTranslation("ControlInfo", "restart_required_title"),
                MessageBoxButtons.YesNo, MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                Application.Restart();
                Environment.Exit(0);
            }
        }
    }

    public class UpdateInfo
    {
        public string version { get; set; }
        public Dictionary<string, string[]> releaseNotes { get; set; }
        public string updateUrl { get; set; }
    }
}
