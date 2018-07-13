using System;
using GalleyFramework.Infrastructure;
using GalleyFramework.Helpers.Flow;
using System.Runtime.CompilerServices;

namespace GalleyFramework.Sample.Locales
{
    public abstract class BaseLocale : GalleyBaseLocale
    {
        protected BaseLocale(string code) : base(code)
        {
        }

        public string LocaleName => Get();
        public string PressMe => Get();
    }
}
