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
            pictureBox1 = new PictureBox();
            lblServer = new ReaLTaiizor.Controls.DungeonLabel();
            lblClient = new ReaLTaiizor.Controls.DungeonLabel();
            metroSwitch1 = new ReaLTaiizor.Controls.MetroSwitch();
            dungeonLabel4 = new ReaLTaiizor.Controls.DungeonLabel();
            btnAggiorna = new ReaLTaiizor.Controls.MaterialButton();
            pictureBox2 = new PictureBox();
            pictureBox3 = new PictureBox();
            aloneComboBox1 = new ReaLTaiizor.Controls.AloneComboBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).BeginInit();
            SuspendLayout();
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
            // lblServer
            // 
            resources.ApplyResources(lblServer, "lblServer");
            lblServer.BackColor = Color.Transparent;
            lblServer.ForeColor = Color.FromArgb(76, 76, 77);
            lblServer.Name = "lblServer";
            // 
            // lblClient
            // 
            resources.ApplyResources(lblClient, "lblClient");
            lblClient.BackColor = Color.Transparent;
            lblClient.ForeColor = Color.FromArgb(76, 76, 77);
            lblClient.Name = "lblClient";
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
            // dungeonLabel4
            // 
            resources.ApplyResources(dungeonLabel4, "dungeonLabel4");
            dungeonLabel4.BackColor = Color.Transparent;
            dungeonLabel4.ForeColor = Color.FromArgb(76, 76, 77);
            dungeonLabel4.Name = "dungeonLabel4";
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
            // pictureBox2
            // 
            resources.ApplyResources(pictureBox2, "pictureBox2");
            pictureBox2.Cursor = Cursors.Hand;
            pictureBox2.Image = Properties.Resources.pngKoFi;
            pictureBox2.Name = "pictureBox2";
            pictureBox2.TabStop = false;
            pictureBox2.Click += pictureBox2_Click;
            // 
            // pictureBox3
            // 
            resources.ApplyResources(pictureBox3, "pictureBox3");
            pictureBox3.Image = Properties.Resources.italy;
            pictureBox3.Name = "pictureBox3";
            pictureBox3.TabStop = false;
            pictureBox3.Click += pictureBox3_Click;
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
            // ControlInfo
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            Controls.Add(aloneComboBox1);
            Controls.Add(pictureBox3);
            Controls.Add(pictureBox2);
            Controls.Add(btnAggiorna);
            Controls.Add(dungeonLabel4);
            Controls.Add(metroSwitch1);
            Controls.Add(lblClient);
            Controls.Add(lblServer);
            Controls.Add(pictureBox1);
            Name = "ControlInfo";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pictureBox1;
        private ReaLTaiizor.Controls.DungeonLabel lblServer;
        private ReaLTaiizor.Controls.DungeonLabel lblClient;
        private ReaLTaiizor.Controls.MetroSwitch metroSwitch1;
        private ReaLTaiizor.Controls.DungeonLabel dungeonLabel4;
        private ReaLTaiizor.Controls.MaterialButton btnAggiorna;
        private PictureBox pictureBox2;
        private PictureBox pictureBox3;
        private ReaLTaiizor.Controls.AloneComboBox aloneComboBox1;
    }
}
