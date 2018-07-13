using System;
using System.Collections.Generic;
using GalleyFramework.Helpers.Flow;

namespace GalleyFramework.Helpers.Interfaces
{
    public interface ILocalizationHelper
    {
        event Action LocaleChanged;
        GalleyBaseLocale CurrentLocale { get; }
        void SetLocale<TLocale>() where TLocale : GalleyBaseLocale;
        void SetLocale(string code);
        string Get(string key);
    }
}
