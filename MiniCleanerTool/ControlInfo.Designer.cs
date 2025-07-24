namespace MiniCleanerTool
{
    partial class ControlInfo
    {
        /// <summary> 
        /// Variabile di progettazione necessaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Pulire le risorse in uso.
        /// </summary>
        /// <param name="disposing">ha valore true se le risorse gestite devono essere eliminate, false in caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Codice generato da Progettazione componenti

        /// <summary> 
        /// Metodo necessario per il supporto della finestra di progettazione. Non modificare 
        /// il contenuto del metodo con l'editor di codice.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ControlInfo));
            panel17 = new Panel();
            pictureBox2 = new PictureBox();
            lblClient = new ReaLTaiizor.Controls.DungeonLabel();
            lblServer = new ReaLTaiizor.Controls.DungeonLabel();
            btnAggiorna = new ReaLTaiizor.Controls.MaterialButton();
            metroSwitch1 = new ReaLTaiizor.Controls.MetroSwitch();
            aloneComboBox1 = new ReaLTaiizor.Controls.AloneComboBox();
            pictureBox3 = new PictureBox();
            pictureBox1 = new PictureBox();
            dungeonLabel4 = new ReaLTaiizor.Controls.DungeonLabel();
            panel17.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // panel17
            // 
            resources.ApplyResources(panel17, "panel17");
            panel17.Controls.Add(pictureBox2);
            panel17.Controls.Add(lblClient);
            panel17.Controls.Add(lblServer);
            panel17.Controls.Add(btnAggiorna);
            panel17.Controls.Add(metroSwitch1);
            panel17.Controls.Add(aloneComboBox1);
            panel17.Controls.Add(pictureBox3);
            panel17.Controls.Add(pictureBox1);
            panel17.Controls.Add(dungeonLabel4);
            panel17.Name = "panel17";
            // 
            // pictureBox2
            // 
            resources.ApplyResources(pictureBox2, "pictureBox2");
            pictureBox2.Cursor = Cursors.Hand;
            pictureBox2.Image = Properties.Resources.pngKoFi;
            pictureBox2.Name = "pictureBox2";
            pictureBox2.TabStop = false;
            pictureBox2.Click += pictureBox2_Click;
            // 
            // lblClient
            // 
            resources.ApplyResources(lblClient, "lblClient");
            lblClient.BackColor = Color.Transparent;
            lblClient.ForeColor = Color.FromArgb(76, 76, 77);
            lblClient.Name = "lblClient";
            // 
            // lblServer
            // 
            resources.ApplyResources(lblServer, "lblServer");
            lblServer.BackColor = Color.Transparent;
            lblServer.ForeColor = Color.FromArgb(76, 76, 77);
            lblServer.Name = "lblServer";
            // 
            // btnAggiorna
            // 
            resources.ApplyResources(btnAggiorna, "btnAggiorna");
            btnAggiorna.Cursor = Cursors.Hand;
            btnAggiorna.Density = ReaLTaiizor.Controls.MaterialButton.MaterialButtonDensity.Default;
            btnAggiorna.Depth = 0;
            btnAggiorna.DrawShadows = false;
            btnAggiorna.HighEmphasis = true;
            btnAggiorna.Icon = null;
            btnAggiorna.IconType = ReaLTaiizor.Controls.MaterialButton.MaterialIconType.Rebase;
            btnAggiorna.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            btnAggiorna.Name = "btnAggiorna";
            btnAggiorna.NoAccentTextColor = Color.Empty;
            btnAggiorna.Type = ReaLTaiizor.Controls.MaterialButton.MaterialButtonType.Contained;
            btnAggiorna.UseAccentColor = false;
            btnAggiorna.UseVisualStyleBackColor = true;
            btnAggiorna.Click += btnAggiorna_Click;
            // 
            // metroSwitch1
            // 
            resources.ApplyResources(metroSwitch1, "metroSwitch1");
            metroSwitch1.BackColor = Color.Transparent;
            metroSwitch1.BackgroundColor = Color.Empty;
            metroSwitch1.BorderColor = Color.FromArgb(165, 159, 147);
            metroSwitch1.CheckColor = Color.FromArgb(65, 177, 225);
            metroSwitch1.CheckState = ReaLTaiizor.Enum.Metro.CheckState.Unchecked;
            metroSwitch1.DisabledBorderColor = Color.FromArgb(205, 205, 205);
            metroSwitch1.DisabledCheckColor = Color.FromArgb(100, 65, 177, 225);
            metroSwitch1.DisabledUnCheckColor = Color.FromArgb(200, 205, 205, 205);
            metroSwitch1.IsDerivedStyle = true;
            metroSwitch1.Name = "metroSwitch1";
            metroSwitch1.Style = ReaLTaiizor.Enum.Metro.Style.Light;
            metroSwitch1.StyleManager = null;
            metroSwitch1.Switched = false;
            metroSwitch1.SymbolColor = Color.FromArgb(92, 92, 92);
            metroSwitch1.ThemeAuthor = "Taiizor";
            metroSwitch1.ThemeName = "MetroLight";
            metroSwitch1.UnCheckColor = Color.FromArgb(155, 155, 155);
            metroSwitch1.SwitchedChanged += metroSwitch1_SwitchedChanged;
            // 
            // aloneComboBox1
            // 
            resources.ApplyResources(aloneComboBox1, "aloneComboBox1");
            aloneComboBox1.DrawMode = DrawMode.OwnerDrawFixed;
            aloneComboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            aloneComboBox1.EnabledCalc = true;
            aloneComboBox1.FormattingEnabled = true;
            aloneComboBox1.Items.AddRange(new object[] { resources.GetString("aloneComboBox1.Items"), resources.GetString("aloneComboBox1.Items1") });
            aloneComboBox1.Name = "aloneComboBox1";
            aloneComboBox1.SelectedIndexChanged += aloneComboBox1_SelectedIndexChanged;
            // 
            // pictureBox3
            // 
            resources.ApplyResources(pictureBox3, "pictureBox3");
            pictureBox3.Cursor = Cursors.Hand;
            pictureBox3.Image = Properties.Resources.italy;
            pictureBox3.Name = "pictureBox3";
            pictureBox3.TabStop = false;
            pictureBox3.Click += pictureBox3_Click;
            // 
            // pictureBox1
            // 
            resources.ApplyResources(pictureBox1, "pictureBox1");
            pictureBox1.Cursor = Cursors.Hand;
            pictureBox1.Image = Properties.Resources.pngGithub;
            pictureBox1.Name = "pictureBox1";
            pictureBox1.TabStop = false;
            pictureBox1.Click += pictureBox1_Click;
            // 
            // dungeonLabel4
            // 
            resources.ApplyResources(dungeonLabel4, "dungeonLabel4");
            dungeonLabel4.BackColor = Color.Transparent;
            dungeonLabel4.ForeColor = Color.FromArgb(76, 76, 77);
            dungeonLabel4.Name = "dungeonLabel4";
            // 
            // ControlInfo
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            Controls.Add(panel17);
            Name = "ControlInfo";
            panel17.ResumeLayout(false);
            panel17.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel17;
        private ReaLTaiizor.Controls.DungeonLabel lblClient;
        private ReaLTaiizor.Controls.DungeonLabel lblServer;
        private ReaLTaiizor.Controls.MaterialButton btnAggiorna;
        private ReaLTaiizor.Controls.MetroSwitch metroSwitch1;
        private ReaLTaiizor.Controls.AloneComboBox aloneComboBox1;
        private PictureBox pictureBox3;
        private PictureBox pictureBox1;
        private ReaLTaiizor.Controls.DungeonLabel dungeonLabel4;
        private PictureBox pictureBox2;
    }
}
