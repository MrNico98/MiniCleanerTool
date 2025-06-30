namespace MiniCleanerTool
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            tableLayoutPanel1 = new TableLayoutPanel();
            panel1 = new Panel();
            dungeonLabel3 = new ReaLTaiizor.Controls.DungeonLabel();
            btnInfo = new Button();
            btnHome = new Button();
            btnBrowser = new Button();
            pictureBox1 = new PictureBox();
            btnRegistro = new Button();
            btnPulisci = new Button();
            panel2 = new Panel();
            panel4 = new Panel();
            smallLabel1 = new ReaLTaiizor.Controls.SmallLabel();
            btnAggiorna = new ReaLTaiizor.Controls.MaterialButton();
            panel3 = new Panel();
            materialLabel2 = new ReaLTaiizor.Controls.SmallLabel();
            btnAnnulla = new ReaLTaiizor.Controls.MaterialButton();
            btnConferma = new ReaLTaiizor.Controls.MaterialButton();
            dangeonHeardelabel = new ReaLTaiizor.Controls.DungeonHeaderLabel();
            rtbDescription = new RichTextBox();
            tableLayoutPanel1.SuspendLayout();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            panel2.SuspendLayout();
            panel4.SuspendLayout();
            panel3.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(tableLayoutPanel1, "tableLayoutPanel1");
            tableLayoutPanel1.BackColor = Color.White;
            tableLayoutPanel1.Controls.Add(panel1, 0, 0);
            tableLayoutPanel1.Controls.Add(panel2, 1, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // panel1
            // 
            panel1.BackColor = Color.White;
            panel1.Controls.Add(dungeonLabel3);
            panel1.Controls.Add(btnInfo);
            panel1.Controls.Add(btnHome);
            panel1.Controls.Add(btnBrowser);
            panel1.Controls.Add(pictureBox1);
            panel1.Controls.Add(btnRegistro);
            panel1.Controls.Add(btnPulisci);
            resources.ApplyResources(panel1, "panel1");
            panel1.Name = "panel1";
            // 
            // dungeonLabel3
            // 
            resources.ApplyResources(dungeonLabel3, "dungeonLabel3");
            dungeonLabel3.BackColor = Color.Transparent;
            dungeonLabel3.ForeColor = Color.FromArgb(76, 76, 77);
            dungeonLabel3.Name = "dungeonLabel3";
            // 
            // btnInfo
            // 
            resources.ApplyResources(btnInfo, "btnInfo");
            btnInfo.Cursor = Cursors.Hand;
            btnInfo.Image = Properties.Resources.gifHomeSettings;
            btnInfo.Name = "btnInfo";
            btnInfo.UseVisualStyleBackColor = true;
            btnInfo.Click += btnInfo_Click;
            btnInfo.MouseLeave += btnInfo_MouseLeave;
            btnInfo.MouseHover += btnInfo_MouseHover;
            // 
            // btnHome
            // 
            resources.ApplyResources(btnHome, "btnHome");
            btnHome.Cursor = Cursors.Hand;
            btnHome.Image = Properties.Resources.gifHomeHome;
            btnHome.Name = "btnHome";
            btnHome.UseVisualStyleBackColor = true;
            btnHome.Click += btnHome_Click;
            btnHome.MouseLeave += btnHome_MouseLeave;
            btnHome.MouseHover += btnHome_MouseHover;
            // 
            // btnBrowser
            // 
            resources.ApplyResources(btnBrowser, "btnBrowser");
            btnBrowser.Cursor = Cursors.Hand;
            btnBrowser.Image = Properties.Resources.gifHomeBrowser;
            btnBrowser.Name = "btnBrowser";
            btnBrowser.UseVisualStyleBackColor = true;
            btnBrowser.Click += btnBrowser_Click;
            btnBrowser.MouseLeave += btnBrowser_MouseLeave;
            btnBrowser.MouseHover += btnBrowser_MouseHover;
            // 
            // pictureBox1
            // 
            resources.ApplyResources(pictureBox1, "pictureBox1");
            pictureBox1.Image = Properties.Resources.gifAppLogo;
            pictureBox1.Name = "pictureBox1";
            pictureBox1.TabStop = false;
            // 
            // btnRegistro
            // 
            resources.ApplyResources(btnRegistro, "btnRegistro");
            btnRegistro.Cursor = Cursors.Hand;
            btnRegistro.Image = Properties.Resources.gifHomeRegistro;
            btnRegistro.Name = "btnRegistro";
            btnRegistro.UseVisualStyleBackColor = true;
            btnRegistro.Click += btnRegistro_Click;
            btnRegistro.MouseLeave += btnRegistro_MouseLeave;
            btnRegistro.MouseHover += btnRegistro_MouseHover;
            // 
            // btnPulisci
            // 
            resources.ApplyResources(btnPulisci, "btnPulisci");
            btnPulisci.Cursor = Cursors.Hand;
            btnPulisci.Image = Properties.Resources.gifHomePuliziaPC;
            btnPulisci.Name = "btnPulisci";
            btnPulisci.UseVisualStyleBackColor = true;
            btnPulisci.Click += btnPulisci_Click;
            btnPulisci.MouseLeave += btnPulisci_MouseLeave;
            btnPulisci.MouseHover += btnPulisci_MouseHover;
            // 
            // panel2
            // 
            panel2.BackColor = Color.White;
            panel2.Controls.Add(panel4);
            panel2.Controls.Add(panel3);
            panel2.Controls.Add(dangeonHeardelabel);
            panel2.Controls.Add(rtbDescription);
            resources.ApplyResources(panel2, "panel2");
            panel2.Name = "panel2";
            // 
            // panel4
            // 
            resources.ApplyResources(panel4, "panel4");
            panel4.BackColor = Color.WhiteSmoke;
            panel4.Controls.Add(smallLabel1);
            panel4.Controls.Add(btnAggiorna);
            panel4.Name = "panel4";
            // 
            // smallLabel1
            // 
            resources.ApplyResources(smallLabel1, "smallLabel1");
            smallLabel1.BackColor = Color.Transparent;
            smallLabel1.ForeColor = Color.Black;
            smallLabel1.Name = "smallLabel1";
            // 
            // btnAggiorna
            // 
            resources.ApplyResources(btnAggiorna, "btnAggiorna");
            btnAggiorna.Cursor = Cursors.Hand;
            btnAggiorna.Density = ReaLTaiizor.Controls.MaterialButton.MaterialButtonDensity.Default;
            btnAggiorna.Depth = 0;
            btnAggiorna.HighEmphasis = true;
            btnAggiorna.Icon = null;
            btnAggiorna.IconType = ReaLTaiizor.Controls.MaterialButton.MaterialIconType.Rebase;
            btnAggiorna.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            btnAggiorna.Name = "btnAggiorna";
            btnAggiorna.NoAccentTextColor = Color.Empty;
            btnAggiorna.Type = ReaLTaiizor.Controls.MaterialButton.MaterialButtonType.Contained;
            btnAggiorna.UseAccentColor = false;
            btnAggiorna.UseVisualStyleBackColor = true;
            // 
            // panel3
            // 
            resources.ApplyResources(panel3, "panel3");
            panel3.BackColor = Color.WhiteSmoke;
            panel3.Controls.Add(materialLabel2);
            panel3.Controls.Add(btnAnnulla);
            panel3.Controls.Add(btnConferma);
            panel3.Name = "panel3";
            // 
            // materialLabel2
            // 
            resources.ApplyResources(materialLabel2, "materialLabel2");
            materialLabel2.BackColor = Color.Transparent;
            materialLabel2.ForeColor = Color.Black;
            materialLabel2.Name = "materialLabel2";
            // 
            // btnAnnulla
            // 
            resources.ApplyResources(btnAnnulla, "btnAnnulla");
            btnAnnulla.Cursor = Cursors.Hand;
            btnAnnulla.Density = ReaLTaiizor.Controls.MaterialButton.MaterialButtonDensity.Default;
            btnAnnulla.Depth = 0;
            btnAnnulla.HighEmphasis = true;
            btnAnnulla.Icon = null;
            btnAnnulla.IconType = ReaLTaiizor.Controls.MaterialButton.MaterialIconType.Rebase;
            btnAnnulla.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            btnAnnulla.Name = "btnAnnulla";
            btnAnnulla.NoAccentTextColor = Color.Empty;
            btnAnnulla.Type = ReaLTaiizor.Controls.MaterialButton.MaterialButtonType.Contained;
            btnAnnulla.UseAccentColor = false;
            btnAnnulla.UseVisualStyleBackColor = true;
            btnAnnulla.Click += btnAnnulla_Click;
            // 
            // btnConferma
            // 
            resources.ApplyResources(btnConferma, "btnConferma");
            btnConferma.Cursor = Cursors.Hand;
            btnConferma.Density = ReaLTaiizor.Controls.MaterialButton.MaterialButtonDensity.Default;
            btnConferma.Depth = 0;
            btnConferma.HighEmphasis = true;
            btnConferma.Icon = null;
            btnConferma.IconType = ReaLTaiizor.Controls.MaterialButton.MaterialIconType.Rebase;
            btnConferma.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            btnConferma.Name = "btnConferma";
            btnConferma.NoAccentTextColor = Color.Empty;
            btnConferma.Type = ReaLTaiizor.Controls.MaterialButton.MaterialButtonType.Contained;
            btnConferma.UseAccentColor = false;
            btnConferma.UseVisualStyleBackColor = true;
            btnConferma.Click += btnConferma_Click;
            // 
            // dangeonHeardelabel
            // 
            resources.ApplyResources(dangeonHeardelabel, "dangeonHeardelabel");
            dangeonHeardelabel.BackColor = Color.Transparent;
            dangeonHeardelabel.ForeColor = Color.FromArgb(76, 76, 77);
            dangeonHeardelabel.Name = "dangeonHeardelabel";
            // 
            // rtbDescription
            // 
            resources.ApplyResources(rtbDescription, "rtbDescription");
            rtbDescription.BackColor = Color.White;
            rtbDescription.BorderStyle = BorderStyle.None;
            rtbDescription.DetectUrls = false;
            rtbDescription.ForeColor = Color.Black;
            rtbDescription.Name = "rtbDescription";
            rtbDescription.ReadOnly = true;
            rtbDescription.TabStop = false;
            rtbDescription.Enter += rtbDescription_Enter;
            // 
            // Form1
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Dpi;
            Controls.Add(tableLayoutPanel1);
            Name = "Form1";
            tableLayoutPanel1.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            panel4.ResumeLayout(false);
            panel4.PerformLayout();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            ResumeLayout(false);
        }
        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private Panel panel1;
        private Button btnRegistro;
        private Button btnPulisci;
        private Panel panel2;
        private PictureBox pictureBox1;
        public RichTextBox rtbDescription;
        private Button btnBrowser;
        private Button btnInfo;
        private Button btnHome;
        private Panel panel3;
        private ReaLTaiizor.Controls.MaterialButton btnAnnulla;
        private ReaLTaiizor.Controls.MaterialButton btnConferma;
        private ReaLTaiizor.Controls.SmallLabel materialLabel2;
        private ReaLTaiizor.Controls.DungeonHeaderLabel dangeonHeardelabel;
        private ReaLTaiizor.Controls.DungeonLabel dungeonLabel3;
        private Panel panel4;
        private ReaLTaiizor.Controls.SmallLabel smallLabel1;
        private ReaLTaiizor.Controls.MaterialButton materialButton1;
        private ReaLTaiizor.Controls.MaterialButton btnAggiorna;
    }
}
