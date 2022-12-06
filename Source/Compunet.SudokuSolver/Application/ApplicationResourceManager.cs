using Compunet.SudokuSolver.Extensions.Generic;
using Compunet.SudokuSolver.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Compunet.SudokuSolver.Application
{
    public class ApplicationResourceManager : IApplicationResourceManager
    {
        private const int ThemeResourcePositionIndex = 0;
        private const int LangResourcePositionIndex = 1;

        private const string DefaultLightTheme = "[Default] Light Theme";
        private const string DefaultDarkTheme = "[Default] Dark Theme";
        private const string DefaultHebrewLanguage = "[Default] Hebrew Language";
        private const string DefaultEnglishLanguage = "[Default] English Language";

        private readonly ISettingsService mSettings;
        private readonly System.Windows.Application mApp = System.Windows.Application.Current;

        private readonly List<IApplicationResource> mAllThemes = new();
        private readonly List<IApplicationResource> mAllLangs = new();

        private string mCurrentTheme = DefaultLightTheme;
        private string mCurrentLanguage = DefaultEnglishLanguage;

        public event Action<string> ThemeChanged = (theme) => { };
        public event Action<string> LanguageChanged = (lang) => { };

        private System.Windows.Application CurrentApplication => System.Windows.Application.Current;
        private Collection<ResourceDictionary> ApplicationResources => CurrentApplication.Resources.MergedDictionaries;

        public ApplicationResourceManager(ISettingsService settings)
        {
            mSettings = settings;
        }

        public Task Load()
        {
            InitThemes();
            InitLanguages();

            return Task.CompletedTask;
        }

        private void InitThemes()
        {
            var light = new ResourceDictionary
            {
                Source = new Uri("/Resources/Themes/LightTheme.xaml", UriKind.RelativeOrAbsolute)
            };

            var dark = new ResourceDictionary
            {
                Source = new Uri("/Resources/Themes/DarkTheme.xaml", UriKind.RelativeOrAbsolute)
            };

            var light_theme = new ThemeApplicationResource(DefaultLightTheme, ApplicationResourceCategory.ThemeAsset, new Lazy<ResourceDictionary>(light));
            var dark_theme = new ThemeApplicationResource(DefaultDarkTheme, ApplicationResourceCategory.ThemeAsset, new Lazy<ResourceDictionary>(dark));

            mAllThemes.Add(light_theme);
            mAllThemes.Add(dark_theme);

            mCurrentTheme = mSettings.CurrentTheme ?? mCurrentTheme;
            SetTheme(mCurrentTheme);
        }

        private void InitLanguages()
        {
            var he = new ResourceDictionary
            {
                Source = new Uri("/Resources/Localization/StringResources.he.xaml", UriKind.RelativeOrAbsolute)
            };

            var en = new ResourceDictionary
            {
                Source = new Uri("/Resources/Localization/StringResources.en.xaml", UriKind.RelativeOrAbsolute)
            };

            var en_lang = new ThemeApplicationResource(DefaultEnglishLanguage, ApplicationResourceCategory.LanguageAsset, new Lazy<ResourceDictionary>(en));
            var he_lang = new ThemeApplicationResource(DefaultHebrewLanguage, ApplicationResourceCategory.LanguageAsset, new Lazy<ResourceDictionary>(he));

            mAllLangs.Add(en_lang);
            mAllLangs.Add(he_lang);

            mCurrentLanguage = mSettings.CurrentLanguage ?? mCurrentLanguage;
            SetLanguage(mCurrentLanguage);
        }

        public Task SetTheme(string themeName)
        {
            var theme = mAllThemes.SingleOrDefault(t => t?.Name == themeName, null);

            if (theme is not null)
            {
                ApplicationResources[ThemeResourcePositionIndex] = theme.CreateResourceDictionary();

                mCurrentTheme = theme.Name;
                mSettings.CurrentTheme = mCurrentTheme;
                ThemeChanged(mCurrentTheme);
            }

            return Task.CompletedTask;
        }

        public Task NextTheme()
        {
            // get index of current theme
            int index = mAllThemes.FirstIndex(item => item.Name == mCurrentTheme);

            // compute the next theme index
            index = (index + 1) % mAllThemes.Count;

            var nextThemeName = mAllThemes[index].Name;

            SetTheme(nextThemeName);

            return Task.CompletedTask;
        }

        public Task SetLanguage(string langName)
        {
            var lang = mAllLangs.SingleOrDefault(t => t?.Name == langName, null);

            if (lang is not null)
            {
                ApplicationResources[LangResourcePositionIndex] = lang.CreateResourceDictionary();

                mCurrentLanguage = lang.Name;
                mSettings.CurrentLanguage = mCurrentLanguage;
                LanguageChanged(mCurrentLanguage);
            }

            return Task.CompletedTask;
        }

        public Task NextLanguage()
        {
            // get index of current language
            int index = mAllLangs.FirstIndex(item => item.Name == mCurrentLanguage);

            // compute the next language index
            index = (index + 1) % mAllLangs.Count;

            var nextLangName = mAllLangs[index].Name;

            SetLanguage(nextLangName);

            return Task.CompletedTask;
        }

        public object GetResource(string resourceKey)
        {
            return mApp.FindResource(resourceKey);
        }

        public string GetStringResource(string resourceKey)
        {
            string? value = GetResource(resourceKey) as string;
            return value ?? throw new ResourceReferenceKeyNotFoundException();
        }
    }
}
