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

            // Gestione specifica per DungeonHeaderLabel
            if (control is ReaLTaiizor.Controls.DungeonHeaderLabel headerLabel)
            {
                if (headerLabel.Visible)
                {
                    headerLabel.ForeColor = foreColor;
                }
            }

            // Gestione specifica per AirCheckBox
            if (control is ReaLTaiizor.Controls.AirCheckBox airCheckBox)
            {
                if (airCheckBox.Visible)
                {
                    // Cloniamo i colori attuali per modificarli
                    var colors = airCheckBox.Colors;

                    for (int i = 0; i < colors.Length; i++)
                    {
                        if (colors[i].Name == "GradientBottomNormal")
                        {
                            colors[i].Value = darkTheme ? Color.FromArgb(45, 45, 45) : Color.FromArgb(240, 240, 240);
                        }
                        else if (colors[i].Name == "Text")
                        {
                            colors[i].Value = darkTheme ? Color.White : Color.Black;
                        }
                    }

                    airCheckBox.Colors = colors;
                    airCheckBox.Refresh(); // Applica i nuovi colori
                }
            }

            // Ricorsione sui figli
            foreach (Control child in control.Controls)
            {
                ApplyThemeToControl(child, darkTheme);
            }
        }
    }
}
