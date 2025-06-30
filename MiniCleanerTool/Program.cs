using System.Globalization;

namespace MiniCleanerTool
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            bool dark = Properties.Settings.Default.DarkTheme;
            ThemeManager.SetTheme(dark);
            string languageCode = Properties.Settings.Default.Language ?? "it";
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(languageCode);
            Thread.CurrentThread.CurrentCulture = new CultureInfo(languageCode);
            LanguageManager.SetLanguage(languageCode);
            Application.Run(new Form1());
        }
    }
}