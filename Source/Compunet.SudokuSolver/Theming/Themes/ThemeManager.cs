using Compunet.SudokuSolver.Services;
using Compunet.SudokuSolver.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Compunet.SudokuSolver.Theming.Themes
{
    public class ThemeManager : IThemeService
    {
        private const int ThemeResourceIndex = 0;
        private const string ThemesDirectory = ".\\themes\\";
        private const string ThemesSearchPattern = "*-theme.ctheme";
        private Collection<ResourceDictionary> Resources => Application.Current.Resources.MergedDictionaries;

        private readonly BrushConverter converter = new();
        private readonly List<ResourceDictionary> themes = new();

        public int CurrentThemeIndex { get; private set; }


        public ThemeManager(ISettingsService settings,
                             IExitService exit)
        {
            CurrentThemeIndex = settings.ActivatedTheme;
            exit.Exiting += () => settings.ActivatedTheme = CurrentThemeIndex;
        }

        public Task Load()
        {
            themes.Clear();
            LoadDefault();
            LoadFiles();

            SetTheme();

            return Tasks.Empty;
        }

        public async Task NextTheme()
        {
            CurrentThemeIndex++;
            CurrentThemeIndex %= themes.Count;

            await Task.Run(SetTheme);
        }

        private void LoadDefault()
        {
            var light = new ResourceDictionary
            {
                Source = new Uri("/Theming/Themes/LightTheme.xaml", UriKind.RelativeOrAbsolute)
            };

            var dark = new ResourceDictionary
            {
                Source = new Uri("/Theming/Themes/DarkTheme.xaml", UriKind.RelativeOrAbsolute)
            };

            themes.Add(light);
            themes.Add(dark);
        }

        private void LoadFiles()
        {
            if (Directory.Exists(ThemesDirectory))
            {
                foreach (var file in Directory.GetFiles(ThemesDirectory, ThemesSearchPattern).Reverse())
                {
                    var theme = BuildFromFile(file);
                    themes.Add(theme);
                }
            }
        }

        private void SetTheme()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Resources[ThemeResourceIndex] = themes[CurrentThemeIndex];
            });
        }

        private ResourceDictionary BuildFromFile(string filename)
        {
            const char separator = ':';

            ResourceDictionary result = new();

            foreach (var line in File.ReadLines(filename))
            {
                string[] parts = line.Split(separator);

                if (parts.Length != 2)
                    continue;

                string key = parts[0];
                string value = parts[1];

                Brush? brush = converter.ConvertFrom(value) as Brush;

                if (brush is null)
                    continue;

                result.Add(key, brush);
            }

            return result;
        }
    }
}
