namespace MiniCleanerTool
{
    partial class ControlPulizia
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ControlPulizia));
            materialProgressBar1 = new ReaLTaiizor.Controls.MaterialProgressBar();
            pictureBox1 = new PictureBox();
            tableLayoutPanel1 = new TableLayoutPanel();
            panel1 = new Panel();
            richTextBox1 = new RichTextBox();
            panel2 = new Panel();
            airCheckBox3 = new ReaLTaiizor.Controls.AirCheckBox();
            airCheckBox2 = new ReaLTaiizor.Controls.AirCheckBox();
            airCheckBox1 = new ReaLTaiizor.Controls.AirCheckBox();
            materialButtonPulisci = new ReaLTaiizor.Controls.MaterialButton();
            treeView1 = new TreeView();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            tableLayoutPanel1.SuspendLayout();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // materialProgressBar1
            // 
            resources.ApplyResources(materialProgressBar1, "materialProgressBar1");
            materialProgressBar1.Depth = 0;
            materialProgressBar1.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            materialProgressBar1.Name = "materialProgressBar1";
            materialProgressBar1.UseAccentColor = false;
            // 
            // pictureBox1
            // 
            resources.ApplyResources(pictureBox1, "pictureBox1");
            pictureBox1.Image = Properties.Resources.gifPuliziaPCPulizia;
            pictureBox1.Name = "pictureBox1";
            pictureBox1.TabStop = false;
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(tableLayoutPanel1, "tableLayoutPanel1");
            tableLayoutPanel1.Controls.Add(panel1, 0, 0);
            tableLayoutPanel1.Controls.Add(panel2, 0, 1);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // panel1
            // 
            resources.ApplyResources(panel1, "panel1");
            panel1.Controls.Add(richTextBox1);
            panel1.Name = "panel1";
            // 
            // richTextBox1
            // 
            resources.ApplyResources(richTextBox1, "richTextBox1");
            richTextBox1.Name = "richTextBox1";
            // 
            // panel2
            // 
            resources.ApplyResources(panel2, "panel2");
            panel2.Controls.Add(airCheckBox3);
            panel2.Controls.Add(airCheckBox2);
            panel2.Controls.Add(airCheckBox1);
            panel2.Controls.Add(materialButtonPulisci);
            panel2.Controls.Add(treeView1);
            panel2.Name = "panel2";
            // 
            // airCheckBox3
            // 
            resources.ApplyResources(airCheckBox3, "airCheckBox3");
            airCheckBox3.Checked = false;
            airCheckBox3.Customization = "7e3t//Ly8v/r6+v/5ubm/+vr6//f39//p6en/zw8PP8=";
            airCheckBox3.Image = null;
            airCheckBox3.Name = "airCheckBox3";
            airCheckBox3.NoRounding = false;
            airCheckBox3.Transparent = false;
            // 
            // airCheckBox2
            // 
            resources.ApplyResources(airCheckBox2, "airCheckBox2");
            airCheckBox2.Checked = false;
            airCheckBox2.Customization = "7e3t//Ly8v/r6+v/5ubm/+vr6//f39//p6en/zw8PP8=";
            airCheckBox2.Image = null;
            airCheckBox2.Name = "airCheckBox2";
            airCheckBox2.NoRounding = false;
            airCheckBox2.Transparent = false;
            // 
            // airCheckBox1
            // 
            resources.ApplyResources(airCheckBox1, "airCheckBox1");
            airCheckBox1.Checked = false;
            airCheckBox1.Customization = "7e3t//Ly8v/r6+v/5ubm/+vr6//f39//p6en/zw8PP8=";
            airCheckBox1.Image = null;
            airCheckBox1.Name = "airCheckBox1";
            airCheckBox1.NoRounding = false;
            airCheckBox1.Transparent = false;
            // 
            // materialButtonPulisci
            // 
            resources.ApplyResources(materialButtonPulisci, "materialButtonPulisci");
            materialButtonPulisci.Cursor = Cursors.Hand;
            materialButtonPulisci.Density = ReaLTaiizor.Controls.MaterialButton.MaterialButtonDensity.Dense;
            materialButtonPulisci.Depth = 0;
            materialButtonPulisci.HighEmphasis = true;
            materialButtonPulisci.Icon = null;
            materialButtonPulisci.IconType = ReaLTaiizor.Controls.MaterialButton.MaterialIconType.Rebase;
            materialButtonPulisci.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            materialButtonPulisci.Name = "materialButtonPulisci";
            materialButtonPulisci.NoAccentTextColor = Color.Empty;
            materialButtonPulisci.Type = ReaLTaiizor.Controls.MaterialButton.MaterialButtonType.Contained;
            materialButtonPulisci.UseAccentColor = false;
            materialButtonPulisci.UseVisualStyleBackColor = true;
            materialButtonPulisci.Click += materialButtonPulisci_Click;
            // 
            // treeView1
            // 
            resources.ApplyResources(treeView1, "treeView1");
            treeView1.Name = "treeView1";
            treeView1.KeyDown += treeView1_KeyDown;
            // 
            // ControlPulizia
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            Controls.Add(pictureBox1);
            Controls.Add(materialProgressBar1);
            Controls.Add(tableLayoutPanel1);
            Name = "ControlPulizia";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            tableLayoutPanel1.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private ReaLTaiizor.Controls.MaterialProgressBar materialProgressBar1;
        private PictureBox pictureBox1;
        private TableLayoutPanel tableLayoutPanel1;
        private Panel panel1;
        private RichTextBox richTextBox1;
        private Panel panel2;
        private ReaLTaiizor.Controls.AirCheckBox airCheckBox3;
        private ReaLTaiizor.Controls.AirCheckBox airCheckBox2;
        private ReaLTaiizor.Controls.AirCheckBox airCheckBox1;
        private ReaLTaiizor.Controls.MaterialButton materialButtonPulisci;
        private TreeView treeView1;
    }
}
