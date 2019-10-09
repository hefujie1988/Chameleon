﻿using System;
using Chameleon.Core.Models;
using Chameleon.Core.Resources;
using Chameleon.Services;
using MonkeyCache;
using Xamarin.Forms;

namespace Chameleon.Core.Helpers
{
    public class ThemeServiceBase : IThemeService
    {
        private readonly IBarrel _barrel;

        public ThemeServiceBase(IBarrel barrel)
        {
            _barrel = barrel ?? throw new ArgumentNullException(nameof(barrel));
        }

        public ThemeMode AppTheme
        {
            get => _barrel.Get<ThemeMode>(AppSettings.ThemeKey);
            set => _barrel.Add(AppSettings.ThemeKey, value, TimeSpan.MaxValue);
        }

        public ResourceDictionary CustomColors
        {
            get
            {
                var customColors = _barrel.Get<ResourceDictionary>(AppSettings.CustomColorsKey);
                if (customColors == null)
                    customColors = new LightColors();
                return customColors;
            }

            set => _barrel.Add(AppSettings.CustomColorsKey, value, TimeSpan.MaxValue);
        }

        public ThemeMode CurrentRuntimeTheme { get; private set; }

        public virtual void UpdateTheme(ThemeMode themeMode = ThemeMode.Auto)
        {
            switch (AppTheme)
            {
                case ThemeMode.Auto:
                    if (themeMode == ThemeMode.Dark)
                        ThemeDark();
                    else
                        ThemeLight();
                    break;
                case ThemeMode.Dark:
                    ThemeDark();
                    break;
                case ThemeMode.Light:
                    ThemeLight();
                    break;
                case ThemeMode.Custom:
                    ThemeCustom();
                    break;
                default:
                    break;
            }
        }

        public void ThemeDark()
        {
            if (CurrentRuntimeTheme == ThemeMode.Dark)
                return;

            Application.Current.Resources.Clear();
            var style = new Styles();
            style.MergedDictionaries.Add(new DarkColors());

            Application.Current.Resources = style;

            CurrentRuntimeTheme = ThemeMode.Dark;
        }

        public void ThemeLight()
        {
            if (CurrentRuntimeTheme == ThemeMode.Light)
                return;

            Application.Current.Resources.Clear();
            var style = new Styles();
            style.MergedDictionaries.Add(new LightColors());

            Application.Current.Resources = style;

            CurrentRuntimeTheme = ThemeMode.Light;
        }

        public void ThemeCustom()
        {
            if (CurrentRuntimeTheme == ThemeMode.Custom)
                return;

            Application.Current.Resources.Clear();
            var style = new Styles();
            style.MergedDictionaries.Add(CustomColors);

            Application.Current.Resources = style;

            CurrentRuntimeTheme = ThemeMode.Custom;
        }
    }
}