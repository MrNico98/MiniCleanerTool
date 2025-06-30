using System.Diagnostics;
using System.Drawing;

namespace MiniCleanerTool
{
    public partial class Form1 : Form
    {
        private enum AzioneConferma { Nessuna, Pulizia, Registro, Browser, Rete }
        private bool pulisciconfermato = false;
        private bool registroconfermato = false;
        private AzioneConferma azioneCorrente = AzioneConferma.Nessuna;
        private readonly Dictionary<Control, bool> _isAnimating = new Dictionary<Control, bool>();
        private bool aggiornamento = false;
        public Form1()
        {
            InitializeComponent();
            LanguageManager.LoadTranslations();
            string savedLanguage = Properties.Settings.Default.Language ?? "it";
            LanguageManager.SetLanguage(savedLanguage);
            ThemeManager.ApplyThemeToControl(this, ThemeManager.IsDarkTheme);
            rtbDescription.ReadOnly = true;
            rtbDescription.TabStop = false;
            rtbDescription.Cursor = Cursors.Default;
            string userName = Environment.UserName;
            string greeting;
            int hour = DateTime.Now.Hour;

            if (hour < 12)
                greeting = LanguageManager.GetTranslation("Form1", "buongiorno");
            else if (hour < 18)
                greeting = LanguageManager.GetTranslation("Form1", "buon_pomeriggio");
            else
                greeting = LanguageManager.GetTranslation("Form1", "buonasera");

            string helpText = LanguageManager.GetTranslation("Form1", "come_posso_aiutarti");

            dangeonHeardelabel.Text = $"{greeting} {userName}, {helpText}";
            Aggiornamento();
        }

        public async Task Aggiornamento()
        {
            ControlInfo controlInfo = new ControlInfo(ThemeManager.IsDarkTheme);
            controlInfo.OnUpdateAvailable += async () =>
            {
                bool confermato = await AnimatePanelSlideUpAsyncUpdate(panel4, LanguageManager.GetTranslation("Form1", "aggiornamentotrovato"), btnHome, 100, 1000);
                if (confermato)
                {
                    AttivaBottone(btnInfo);
                    panel2.Controls.Clear();
                    bool dark = Properties.Settings.Default.DarkTheme;
                    ControlInfo infoControl = new ControlInfo(dark);
                    infoControl.Dock = DockStyle.Fill;
                    panel2.Controls.Add(infoControl);
                }
                else
                {
                    await Task.Delay(9000);
                    aggiornamento = true;
                    await AnimatePanelSlideDownAsync(panel4);
                    aggiornamento = false;
                }
            };

            controlInfo.CheckForUpdatesOnStartup();
        }


        private async Task<bool> AnimatePanelSlideUpAsyncUpdate(Control control, string message, Button bottoneFocus, int distance = 100, int durationMs = 1000, params Button[] buttons)
        {
            smallLabel1.Text = message;
            panel4.Visible = true;
            this.ActiveControl = panel2;
            if (!TryStartAnimation(control)) return false;
            if (control == null || control.IsDisposed) return false;
            var tcs = new TaskCompletionSource<bool>();
            var btnConferma = control.Controls.OfType<Button>().FirstOrDefault(b => b.Name == "btnAggiorna");

            if (btnConferma != null)
            {
                bool handled = false;

                btnConferma.Click += (s, e) =>
                {
                    if (!handled)
                    {
                        handled = true;
                        tcs.SetResult(true);
                    }
                };
            }
            else
            {
                tcs.SetResult(false);
            }
            var originalLocation = control.Location;
            control.Visible = false;
            control.Location = new Point(originalLocation.X, originalLocation.Y + distance);
            control.Visible = true;
            control.BringToFront();
            const int targetFps = 60;
            int totalFrames = (int)Math.Max(10, (durationMs / 1000f) * targetFps);
            int delay = 1000 / targetFps;
            float CubicEaseOut(float t) => 1 - (float)Math.Pow(1 - t, 3);

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            for (int frame = 0; frame <= totalFrames; frame++)
            {
                float elapsed = Math.Min(stopwatch.ElapsedMilliseconds, durationMs);
                float progress = elapsed / durationMs;
                float easedProgress = CubicEaseOut(progress);
                int newY = originalLocation.Y + (int)(distance * (1 - easedProgress));
                if (control.InvokeRequired)
                {
                    control.Invoke(() =>
                    {
                        if (!control.IsDisposed)
                            control.Location = new Point(originalLocation.X, newY);
                    });
                }
                else if (!control.IsDisposed)
                {
                    control.Location = new Point(originalLocation.X, newY);
                }
                int targetTime = frame * delay;
                int actualTime = (int)stopwatch.ElapsedMilliseconds;
                int sleepTime = targetTime - actualTime;

                if (sleepTime > 0)
                {
                    await Task.Delay(sleepTime);
                }
            }
            if (!control.IsDisposed)
            {
                if (control.InvokeRequired)
                {
                    control.Invoke(() => control.Location = originalLocation);
                }
                else
                {
                    control.Location = originalLocation;
                }
            }
            EndAnimation(control);
            if (bottoneFocus != null && bottoneFocus.CanFocus)
            {
                bottoneFocus.Focus();
            }
            return await tcs.Task;
        }

        private bool TryStartAnimation(Control control)
        {
            lock (_isAnimating)
            {
                if (_isAnimating.TryGetValue(control, out bool isRunning) && isRunning)
                    return false;

                _isAnimating[control] = true;
                return true;
            }
        }

        private void EndAnimation(Control control)
        {
            lock (_isAnimating)
            {
                if (_isAnimating.ContainsKey(control))
                    _isAnimating[control] = false;
            }
        }

        private async Task<bool> AnimatePanelSlideUpAsync(Control control, string message, Button bottoneFocus, int distance = 100, int durationMs = 1000, params Button[] buttonsToDisable)
        {
            materialLabel2.Text = message;
            panel3.Visible = true;
            foreach (var btn in buttonsToDisable)
                if (btn != null) btn.Enabled = false;
            this.ActiveControl = panel2;
            if (!TryStartAnimation(control)) return false;
            if (control == null || control.IsDisposed) return false;
            var tcs = new TaskCompletionSource<bool>();
            var btnConferma = control.Controls.OfType<Button>().FirstOrDefault(b => b.Name == "btnConferma");
            var btnAnnulla = control.Controls.OfType<Button>().FirstOrDefault(b => b.Name == "btnAnnulla");

            if (btnConferma != null && btnAnnulla != null)
            {
                bool handled = false;

                btnConferma.Click += (s, e) =>
                {
                    if (!handled)
                    {
                        handled = true;
                        tcs.SetResult(true);
                    }
                };

                btnAnnulla.Click += (s, e) =>
                {
                    if (!handled)
                    {
                        handled = true;
                        tcs.SetResult(false);
                    }
                };
            }
            else
            {
                tcs.SetResult(false);
            }
            var originalLocation = control.Location;
            control.Visible = false;
            control.Location = new Point(originalLocation.X, originalLocation.Y + distance);
            control.Visible = true;
            control.BringToFront();
            const int targetFps = 60;
            int totalFrames = (int)Math.Max(10, (durationMs / 1000f) * targetFps);
            int delay = 1000 / targetFps;
            float CubicEaseOut(float t) => 1 - (float)Math.Pow(1 - t, 3);

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            for (int frame = 0; frame <= totalFrames; frame++)
            {
                float elapsed = Math.Min(stopwatch.ElapsedMilliseconds, durationMs);
                float progress = elapsed / durationMs;
                float easedProgress = CubicEaseOut(progress);
                int newY = originalLocation.Y + (int)(distance * (1 - easedProgress));
                if (control.InvokeRequired)
                {
                    control.Invoke(() =>
                    {
                        if (!control.IsDisposed)
                            control.Location = new Point(originalLocation.X, newY);
                    });
                }
                else if (!control.IsDisposed)
                {
                    control.Location = new Point(originalLocation.X, newY);
                }
                int targetTime = frame * delay;
                int actualTime = (int)stopwatch.ElapsedMilliseconds;
                int sleepTime = targetTime - actualTime;

                if (sleepTime > 0)
                {
                    await Task.Delay(sleepTime);
                }
            }
            if (!control.IsDisposed)
            {
                if (control.InvokeRequired)
                {
                    control.Invoke(() => control.Location = originalLocation);
                }
                else
                {
                    control.Location = originalLocation;
                }
            }
            foreach (var btn in buttonsToDisable)
                if (btn != null) btn.Enabled = true;
            EndAnimation(control);
            if (bottoneFocus != null && bottoneFocus.CanFocus)
            {
                bottoneFocus.Focus();
            }
            return await tcs.Task;
        }

        private async Task AnimatePanelSlideDownAsync(Control control, int distance = 90, int durationMs = 1000)
        {
            if (!aggiornamento)
            {
                btnRegistro.Enabled = false;
                btnPulisci.Enabled = false;
                btnBrowser.Enabled = false;
            }
            if (!TryStartAnimation(control)) return;
            if (control == null || control.IsDisposed) return;
            var originalLocation = control.Location;
            const int targetFps = 60;
            int totalFrames = (int)Math.Max(10, (durationMs / 1000f) * targetFps);
            int delay = 1000 / targetFps;
            float CubicEaseIn(float t) => t * t * t;

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            for (int frame = 0; frame <= totalFrames; frame++)
            {
                float elapsed = Math.Min(stopwatch.ElapsedMilliseconds, durationMs);
                float progress = elapsed / durationMs;
                float easedProgress = CubicEaseIn(progress);
                int newY = originalLocation.Y + (int)(distance * easedProgress);
                if (control.InvokeRequired)
                {
                    control.Invoke(() =>
                    {
                        if (!control.IsDisposed)
                        {
                            control.Location = new Point(originalLocation.X, newY);
                            control.Refresh();
                        }
                    });
                }
                else if (!control.IsDisposed)
                {
                    control.Location = new Point(originalLocation.X, newY);
                    control.Refresh();
                }
                int targetTime = frame * delay;
                int actualTime = (int)stopwatch.ElapsedMilliseconds;
                int sleepTime = targetTime - actualTime;

                if (sleepTime > 0)
                {
                    await Task.Delay(sleepTime);
                }
            }
            if (!control.IsDisposed)
            {
                if (control.InvokeRequired)
                {
                    control.Invoke(() =>
                    {
                        control.Visible = false;
                        control.Location = originalLocation;
                    });
                }
                else
                {
                    control.Visible = false;
                    control.Location = originalLocation;
                }
            }
            if (!aggiornamento)
            {
                btnRegistro.Enabled = true;
                btnPulisci.Enabled = true;
                btnBrowser.Enabled = true;
            }
            EndAnimation(control);
        }

        private Button bottoneAttivo = null;
        private void AttivaBottone(Button btn)
        {
            if (bottoneAttivo != null)
            {
                bottoneAttivo.BackColor = SystemColors.Control;
                bottoneAttivo.Refresh();
            }

            bottoneAttivo = btn;
            bottoneAttivo.BackColor = Color.Gray;
        }

        private async void btnPulisci_Click(object sender, EventArgs e)
        {
            AttivaBottone(btnPulisci);
            panel2.Controls.Clear();
            panel2.Controls.Add(panel3);
            panel2.Controls.Add(rtbDescription);
            ThemeManager.SetTheme(Properties.Settings.Default.DarkTheme);
            azioneCorrente = AzioneConferma.Pulizia;
            bool confermato = await AnimatePanelSlideUpAsync(panel3, LanguageManager.GetTranslation("Form1", "puliziapc"), btnPulisci, 100, 1000, btnPulisci, btnRegistro, btnBrowser);
            AttivaBottone(btnPulisci);
            if (confermato)
            {
                await AnimatePanelSlideDownAsync(panel3);
                panel2.Controls.Clear();
                AttivaBottone(btnPulisci);
                this.ActiveControl = btnPulisci;
                ControlPulizia puliziaControl = new ControlPulizia();
                puliziaControl.Dock = DockStyle.Fill;
                panel2.Controls.Add(puliziaControl);
                await Task.Run(() =>
                {
                    puliziaControl.IniziaPulizia();
                    string folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "MiniCleanerTool");
                    Directory.CreateDirectory(folderPath);
                    string filePath = Path.Combine(folderPath, "pulizia.dat");
                    File.WriteAllText(filePath, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                });
            }
            else
            {
                await AnimatePanelSlideDownAsync(panel3);
            }

            azioneCorrente = AzioneConferma.Nessuna;
        }

        private void btnPulisci_MouseHover(object sender, EventArgs e)
        {
            string folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "MiniCleanerTool");
            string filePath = Path.Combine(folderPath, "pulizia.dat");
            string ultimaPulizia = LanguageManager.GetTranslation("Form1", "noneseguita");
            if (File.Exists(filePath))
            {
                ultimaPulizia = File.ReadAllText(filePath);
            }
            dangeonHeardelabel.Text = LanguageManager.GetTranslation("Form1", "puliziaTitolo");
            rtbDescription.Text =
                "\n\n" +
                $"✅ {LanguageManager.GetTranslation("Form1", "rimozioneTemporanei")}\n" +
                $"✅ {LanguageManager.GetTranslation("Form1", "eliminazioneLog")}\n" +
                $"✅ {LanguageManager.GetTranslation("Form1", "puliziaPrefetch")}\n" +
                $"✅ {LanguageManager.GetTranslation("Form1", "rimozioneDefender")}\n" +
                $"✅ {LanguageManager.GetTranslation("Form1", "puliziaDirectX")}\n" +
                $"✅ {LanguageManager.GetTranslation("Form1", "rimozioneCacheIE")}\n\n\n" +
                $"{LanguageManager.GetTranslation("Form1", "ultimaPulizia")}: {ultimaPulizia}";
        }

        private void btnPulisci_MouseLeave(object sender, EventArgs e)
        {
            string userName = Environment.UserName;
            string greeting;
            int hour = DateTime.Now.Hour;
            if (hour < 12)
                greeting = LanguageManager.GetTranslation("Form1", "buongiorno");
            else if (hour < 18)
                greeting = LanguageManager.GetTranslation("Form1", "buon_pomeriggio");
            else
                greeting = LanguageManager.GetTranslation("Form1", "buonasera");

            string helpText = LanguageManager.GetTranslation("Form1", "come_posso_aiutarti");

            dangeonHeardelabel.Text = $"{greeting} {userName}, {helpText}";
            rtbDescription.Clear();
        }

        private async void btnRegistro_Click(object sender, EventArgs e)
        {
            AttivaBottone(btnRegistro);
            panel2.Controls.Clear();
            panel2.Controls.Add(panel3);
            panel2.Controls.Add(rtbDescription);
            ThemeManager.SetTheme(Properties.Settings.Default.DarkTheme);
            azioneCorrente = AzioneConferma.Registro;
            bool confermato = await AnimatePanelSlideUpAsync(panel3, LanguageManager.GetTranslation("Form1", "registrohome"), btnRegistro, 100, 1000, btnPulisci, btnRegistro, btnBrowser);
            AttivaBottone(btnRegistro);
            if (confermato)
            {
                await AnimatePanelSlideDownAsync(panel3);
                panel2.Controls.Clear();
                AttivaBottone(btnRegistro);
                this.ActiveControl = btnRegistro;
                ControlRegistro registroControl = new ControlRegistro();
                registroControl.Dock = DockStyle.Fill;
                panel2.Controls.Add(registroControl);
                Task.Run(() =>
                {
                    registroControl.IniziaPulizia();
                    string folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "MiniCleanerTool");
                    Directory.CreateDirectory(folderPath);
                    string filePath = Path.Combine(folderPath, "registro.dat");
                    File.WriteAllText(filePath, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                });
            }
            else
            {
                await AnimatePanelSlideDownAsync(panel3);
            }
            azioneCorrente = AzioneConferma.Nessuna;
        }

        private void btnRegistro_MouseHover(object sender, EventArgs e)
        {
            string folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "MiniCleanerTool");
            string filePath = Path.Combine(folderPath, "registro.dat");
            string ultimaPulizia = LanguageManager.GetTranslation("Form1", "noneseguita");
            if (File.Exists(filePath))
            {
                ultimaPulizia = File.ReadAllText(filePath);
            }
            dangeonHeardelabel.Text = LanguageManager.GetTranslation("Form1", "puliziaRegistroTitolo");
            rtbDescription.Text =
                "\n\n" +
                $"✅ {LanguageManager.GetTranslation("Form1", "puliziaDllMancanti")}\n" +
                $"✅ {LanguageManager.GetTranslation("Form1", "puliziaKeyVuote")}\n" +
                $"✅ {LanguageManager.GetTranslation("Form1", "puliziaAvvioNonValido")}\n" +
                $"✅ {LanguageManager.GetTranslation("Form1", "puliziaDisinstallazioniNonValide")}\n" +
                $"✅ {LanguageManager.GetTranslation("Form1", "puliziaUserAssist")}\n" +
                $"✅ {LanguageManager.GetTranslation("Form1", "puliziaRecentDocs")}\n\n\n" +
                $"{LanguageManager.GetTranslation("Form1", "ultimaPulizia")}: {ultimaPulizia}";
        }

        private void btnRegistro_MouseLeave(object sender, EventArgs e)
        {
            string userName = Environment.UserName;
            string greeting;
            int hour = DateTime.Now.Hour;
            if (hour < 12)
                greeting = LanguageManager.GetTranslation("Form1", "buongiorno");
            else if (hour < 18)
                greeting = LanguageManager.GetTranslation("Form1", "buon_pomeriggio");
            else
                greeting = LanguageManager.GetTranslation("Form1", "buonasera");

            string helpText = LanguageManager.GetTranslation("Form1", "come_posso_aiutarti");

            dangeonHeardelabel.Text = $"{greeting} {userName}, {helpText}";
            rtbDescription.Clear();
        }

        private async void btnBrowser_Click(object sender, EventArgs e)
        {
            AttivaBottone(btnBrowser);
            panel2.Controls.Clear();
            panel2.Controls.Add(panel3);
            panel2.Controls.Add(rtbDescription);
            ThemeManager.SetTheme(Properties.Settings.Default.DarkTheme);
            azioneCorrente = AzioneConferma.Browser;
            bool confermato = await AnimatePanelSlideUpAsync(panel3, LanguageManager.GetTranslation("Form1", "browserhome"), btnBrowser, 100, 1000, btnPulisci, btnRegistro, btnBrowser);
            AttivaBottone(btnBrowser);
            if (confermato)
            {
                panel2.Controls.Clear();
                AttivaBottone(btnBrowser);
                ControlBrowser browserControl = new ControlBrowser();
                browserControl.Dock = DockStyle.Fill;
                panel2.Controls.Add(browserControl);
                Task.Run(() =>
                {
                    browserControl.IniziaPulizia();
                    string folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "MiniCleanerTool");
                    Directory.CreateDirectory(folderPath);
                    string filePath = Path.Combine(folderPath, "browser.dat");
                    File.WriteAllText(filePath, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                });
            }
            else
            {
                await AnimatePanelSlideDownAsync(panel3);
            }
            azioneCorrente = AzioneConferma.Nessuna;
        }

        private void btnBrowser_MouseHover(object sender, EventArgs e)
        {
            string folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "MiniCleanerTool");
            string filePath = Path.Combine(folderPath, "browser.dat");
            string ultimaPulizia = LanguageManager.GetTranslation("Form1", "noneseguita");
            if (File.Exists(filePath))
            {
                ultimaPulizia = File.ReadAllText(filePath);
            }
            dangeonHeardelabel.Text = LanguageManager.GetTranslation("Form1", "puliziaBrowserTitolo");
            rtbDescription.Text =
                "\n\n" +
                $"✅ {LanguageManager.GetTranslation("Form1", "puliziaEdge")}\n" +
                $"✅ {LanguageManager.GetTranslation("Form1", "puliziaChrome")}\n" +
                $"✅ {LanguageManager.GetTranslation("Form1", "puliziaFirefox")}\n" +
                $"✅ {LanguageManager.GetTranslation("Form1", "puliziaIE")}\n" +
                $"✅ {LanguageManager.GetTranslation("Form1", "puliziaOpera")}\n" +
                $"✅ {LanguageManager.GetTranslation("Form1", "puliziaBrave")}\n" +
                $"✅ {LanguageManager.GetTranslation("Form1", "puliziaVivaldi")}\n" +
                $"✅ {LanguageManager.GetTranslation("Form1", "puliziaSafari")}\n\n\n" +
                $"{LanguageManager.GetTranslation("Form1", "ultimaPulizia")}: {ultimaPulizia}";
        }

        private void btnBrowser_MouseLeave(object sender, EventArgs e)
        {
            string userName = Environment.UserName;
            string greeting;
            int hour = DateTime.Now.Hour;

            if (hour < 12)
                greeting = LanguageManager.GetTranslation("Form1", "buongiorno");
            else if (hour < 18)
                greeting = LanguageManager.GetTranslation("Form1", "buon_pomeriggio");
            else
                greeting = LanguageManager.GetTranslation("Form1", "buonasera");

            string helpText = LanguageManager.GetTranslation("Form1", "come_posso_aiutarti");

            dangeonHeardelabel.Text = $"{greeting} {userName}, {helpText}";
            rtbDescription.Clear();
        }

        private void btnConferma_Click(object sender, EventArgs e)
        {
            switch (azioneCorrente)
            {
                case AzioneConferma.Pulizia:
                    pulisciconfermato = true;
                    break;
                case AzioneConferma.Registro:
                    registroconfermato = true;
                    break;
                case AzioneConferma.Browser:
                    registroconfermato = true;
                    break;
            }
        }

        private void btnAnnulla_Click(object sender, EventArgs e)
        {
            switch (azioneCorrente)
            {
                case AzioneConferma.Pulizia:
                    pulisciconfermato = false;
                    break;
                case AzioneConferma.Registro:
                    registroconfermato = false;
                    break;
                case AzioneConferma.Browser:
                    registroconfermato = false;
                    break;
            }
        }

        private void btnInfo_Click(object sender, EventArgs e)
        {
            AttivaBottone(btnInfo);
            panel2.Controls.Clear();
            bool dark = Properties.Settings.Default.DarkTheme;
            ControlInfo infoControl = new ControlInfo(dark);
            infoControl.Dock = DockStyle.Fill;
            panel2.Controls.Add(infoControl);
        }
        private void btnInfo_MouseHover(object sender, EventArgs e)
        {
            dangeonHeardelabel.Text = LanguageManager.GetTranslation("Form1", "infoApplicazioneTitolo");
            rtbDescription.Text = "\n\n" + LanguageManager.GetTranslation("Form1", "infoApplicazioneDescrizione");
        }


        private void btnInfo_MouseLeave(object sender, EventArgs e)
        {
            string userName = Environment.UserName;
            string greeting;
            int hour = DateTime.Now.Hour;

            if (hour < 12)
                greeting = LanguageManager.GetTranslation("Form1", "buongiorno");
            else if (hour < 18)
                greeting = LanguageManager.GetTranslation("Form1", "buon_pomeriggio");
            else
                greeting = LanguageManager.GetTranslation("Form1", "buonasera");

            string helpText = LanguageManager.GetTranslation("Form1", "come_posso_aiutarti");

            dangeonHeardelabel.Text = $"{greeting} {userName}, {helpText}";
            rtbDescription.Clear();
        }
        private void btnHome_Click(object sender, EventArgs e)
        {
            AttivaBottone(btnInfo);
            panel3.Visible = false;
            panel2.Controls.Clear();
            panel2.Controls.Add(panel3);
            panel2.Controls.Add(rtbDescription);
            panel2.Controls.Add(dangeonHeardelabel);
            ThemeManager.SetTheme(Properties.Settings.Default.DarkTheme);
        }
        private void btnHome_MouseHover(object sender, EventArgs e)
        {
            dangeonHeardelabel.Text = LanguageManager.GetTranslation("Form1", "homeTitolo");
            rtbDescription.Text = "\n\n" + LanguageManager.GetTranslation("Form1", "homeDescrizione");
        }

        private void btnHome_MouseLeave(object sender, EventArgs e)
        {
            string userName = Environment.UserName;
            string greeting;

            int hour = DateTime.Now.Hour;

            if (hour < 12)
                greeting = LanguageManager.GetTranslation("Form1", "buongiorno");
            else if (hour < 18)
                greeting = LanguageManager.GetTranslation("Form1", "buon_pomeriggio");
            else
                greeting = LanguageManager.GetTranslation("Form1", "buonasera");

            string helpText = LanguageManager.GetTranslation("Form1", "come_posso_aiutarti");

            dangeonHeardelabel.Text = $"{greeting} {userName}, {helpText}";
            rtbDescription.Clear();
        }
        private void rtbDescription_Enter(object sender, EventArgs e)
        {
            this.ActiveControl = null;
        }
    }
}
