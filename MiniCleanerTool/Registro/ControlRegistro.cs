using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MiniCleanerTool
{
    public partial class ControlRegistro : UserControl
    {
        public ControlRegistro()
        {
            InitializeComponent();
            ThemeManager.ApplyThemeToControl(this, ThemeManager.IsDarkTheme);
        }

        public void IniziaPulizia()
        {
            MiniCleanerTool.Registro.RegistryCleaner.StartRegedit((p, log) =>
            {
                SetProgress(p);
                SetLog(log);
            });
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
    }
}
