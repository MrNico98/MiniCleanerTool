using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MiniCleanerTool
{
    public partial class ProgressForm : Form
    {
        public ProgressForm()
        {
            InitializeComponent();
            SetMarquee();
        }

        public void SetMarquee()
        {
            progressBar.Style = ProgressBarStyle.Marquee;
            lblStatus.Text = "Operazione in corso...";
        }

        public void SetStatus(string status, int percentComplete)
        {
            lblStatus.Text = status;
            if (progressBar.Style == ProgressBarStyle.Blocks)
            {
                progressBar.Value = percentComplete;
            }
        }
        public void CompleteOperation()
        {
            progressBar.Style = ProgressBarStyle.Blocks;
            progressBar.Value = 100;
            lblStatus.Text = "Operazione completata";
        }
    }
}
