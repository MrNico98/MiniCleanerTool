using ReaLTaiizor.Forms;
using System.Diagnostics;
using System.Globalization;
using System.Text.Json;

namespace MiniCleanerTool
{
    public partial class ControlInfo : UserControl
    {
        private static readonly HttpClient client = new() { Timeout = TimeSpan.FromSeconds(15) };
        private const string CurrentVersion = "0.0.0.3";
        private UpdateInfo? _updateInfo;
        public event Func<Task>? OnUpdateAvailable;
        public bool isloading = true;

        public ControlInfo(bool dark)
        {
            InitializeComponent();
            ThemeManager.ApplyThemeToControl(this, ThemeManager.IsDarkTheme);
            metroSwitch1.Switched = dark;
            dungeonLabel4.Text = dark ? "Tema scuro" : "Tema chiaro";
            lblClient.Text = $"{LanguageManager.GetTranslation("Info", "versioneclient")} {CurrentVersion}";
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
        public Task CheckForUpdatesOnStartup() => CheckForUpdatesAsync();

        private async Task CheckForUpdatesAsync()
        {
            string configUrl = "https://raw.githubusercontent.com/MrNico98/MiniCleanerTool/refs/heads/main/MiniCleanerToolUpdateFile.json";

            try
            {
                string json = await client.GetStringAsync(configUrl);
                _updateInfo = JsonSerializer.Deserialize<UpdateInfo>(json)
                    ?? throw new JsonException("Il manifest di aggiornamento è vuoto.");
                if (!Version.TryParse(_updateInfo.version, out Version? remoteVersion) ||
                    !Version.TryParse(CurrentVersion, out Version? installedVersion) ||
                    !Uri.TryCreate(_updateInfo.updateUrl, UriKind.Absolute, out Uri? releaseUri) ||
                    releaseUri.Scheme != Uri.UriSchemeHttps ||
                    !releaseUri.Host.Equals("github.com", StringComparison.OrdinalIgnoreCase) ||
                    !releaseUri.AbsolutePath.StartsWith("/MrNico98/MiniCleanerTool/releases/", StringComparison.OrdinalIgnoreCase) ||
                    _updateInfo.releaseNotes is null)
                {
                    throw new JsonException("Il manifest contiene versione, note o URL di release non validi.");
                }

                lblServer.Text = $"{LanguageManager.GetTranslation("Info", "versioneserver")} {_updateInfo.version}";
                if (remoteVersion > installedVersion)
                {
                    btnAggiorna.Visible = true;
                    if (OnUpdateAvailable is { } updateHandler)
                    {
                        await updateHandler();
                    }
                }
                else
                {
                    btnAggiorna.Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{LanguageManager.GetTranslation("Global", "errore")} {ex.Message}", "MiniCleanerTool");
            }
        }

        private async void btnAggiorna_Click(object sender, EventArgs e)
        {
            if (_updateInfo is null)
            {
                return;
            }

            string culture = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName.ToUpper();
            string releaseNotes = _updateInfo.releaseNotes.TryGetValue(culture, out string[]? localizedNotes)
                ? string.Join("\n", localizedNotes)
                : string.Join("\n", _updateInfo.releaseNotes.GetValueOrDefault("EN", []));

            var dialogResult = MessageBox.Show(
                $"{LanguageManager.GetTranslation("Info", "versioneserverup")}: {_updateInfo.version}\n\n" +
                $"{LanguageManager.GetTranslation("Info", "releasednotes")}\n{releaseNotes}\n\n" +
                $"{LanguageManager.GetTranslation("Info", "updatequestion")}\n\n" +
                "L'app aprirà la pagina della release; il file non viene installato automaticamente. Verifica provenienza e firma/hash prima di avviarlo.",
                LanguageManager.GetTranslation("Info", "updateavailable"),
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (dialogResult == DialogResult.Yes)
            {
                try
                {
                    Process.Start(new ProcessStartInfo(_updateInfo.updateUrl) { UseShellExecute = true });
                }
                catch (Exception exception)
                {
                    MessageBox.Show($"Impossibile aprire la pagina della release: {exception.Message}", "MiniCleanerTool");
                }
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
            string currentLanguage = Properties.Settings.Default.Language;
            if (currentLanguage == "en")
            {
                ChangeLanguageAndPromptRestart("it");
                pictureBox3.Image = Properties.Resources.italy;
            }
            else
            {
                ChangeLanguageAndPromptRestart("en");
                pictureBox3.Image = Properties.Resources.united_kingdom;
            }
        }

        private void aloneComboBox1_SelectedIndexChanged(object? sender, EventArgs e)
        {
            string? selectedLanguage = aloneComboBox1.SelectedItem?.ToString()?.ToLowerInvariant();
            ChangeLanguageAndPromptRestart(selectedLanguage);
        }


        private void ChangeLanguageAndPromptRestart(string? selectedLanguage)
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
        public string version { get; set; } = string.Empty;
        public Dictionary<string, string[]> releaseNotes { get; set; } = new(StringComparer.OrdinalIgnoreCase);
        public string updateUrl { get; set; } = string.Empty;
    }
}
