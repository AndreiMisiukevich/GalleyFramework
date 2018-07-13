using System;
using GalleyFramework.Helpers.Interfaces;
using System.Globalization;
using System.Collections.Generic;
using GalleyFramework.Helpers.Flow;
using System.Linq;
using GalleyFramework.Extensions;

namespace GalleyFramework.Helpers
{
    public class GalleyLocalizationHelper : ILocalizationHelper
    {
        public event Action LocaleChanged;

        public GalleyLocalizationHelper(params GalleyBaseLocale[] locales) : this(selectedLocale: null, locales: locales)
        {
        }

        public GalleyLocalizationHelper(string selectedLocaleCode, params GalleyBaseLocale[] locales)
            : this(selectedLocale: locales?.FirstOrDefault(l => l.Code == selectedLocaleCode), locales: locales)
		{
		}

        public GalleyLocalizationHelper(Type selectedLocaleType, params GalleyBaseLocale[] locales)
            : this(selectedLocale: locales?.FirstOrDefault(l => l.GetType() == selectedLocaleType), locales: locales)
        {
        }

        private GalleyLocalizationHelper(GalleyBaseLocale selectedLocale, params GalleyBaseLocale[] locales)
		{
            Locales = new List<GalleyBaseLocale>(locales?.Length > 0 
                                                 ? locales 
                                                 : new[] { new GalleyNoLocale()});

            var currentLocaleCode = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
            CurrentLocale = selectedLocale
                ?? Locales.FirstOrDefault(l => l.Code == currentLocaleCode)
                          ?? Locales.FirstOrDefault();
		}

		public List<GalleyBaseLocale> Locales { get; }

		private GalleyBaseLocale _currentLocale;
        public GalleyBaseLocale CurrentLocale
		{
            get => _currentLocale;
            private set
			{
                if (value.NotNull() && Locales.Contains(value))
				{
                    if (_currentLocale != value)
                    {
                        _currentLocale?.OnDeclined();
                        _currentLocale = value;
                        _currentLocale.OnAccepted();
                    }
                    else
                    {
                        _currentLocale.OnRefreshed();
                    }
                    LocaleChanged?.Invoke();
				}
			}
		}

		public void SetLocale<TLocale>() where TLocale : GalleyBaseLocale
        {
            var type = typeof(TLocale);
            CurrentLocale = Locales.FirstOrDefault(l => l.GetType() == type);
        }

        public void SetLocale(string code)
        {
            CurrentLocale = Locales.FirstOrDefault(l => l.Code == code);
        }

        public string Get(string key)
        {
            return CurrentLocale.Get(key);
        }
	}
}

