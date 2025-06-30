using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ReaLTaiizor.Controls;

namespace MiniCleanerTool
{
    public static class ThemeManager
    {
        public static bool IsDarkTheme { get; private set; } = false;

        public static void SetTheme(bool darkTheme)
        {
            IsDarkTheme = darkTheme;
            foreach (Form form in Application.OpenForms)
            {
                ApplyThemeToControl(form, darkTheme);
            }
        }

        public static void ApplyThemeToControl(Control control, bool darkTheme)
        {
            Color backColor = darkTheme ? Color.FromArgb(32, 32, 32) : Color.White;
            Color foreColor = darkTheme ? Color.White : Color.Black;
            control.BackColor = backColor;
            control.ForeColor = foreColor;
            if (control is ReaLTaiizor.Controls.DungeonHeaderLabel materialLabel)
            {
                if (materialLabel.Visible)
                {
                    materialLabel.ForeColor = darkTheme ? Color.White : Color.Black;
                }
                else
                {

                }
            }
            foreach (Control child in control.Controls)
            {
                ApplyThemeToControl(child, darkTheme);
            }
        }
    }

}
