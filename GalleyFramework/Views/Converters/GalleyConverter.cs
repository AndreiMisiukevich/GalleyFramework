using System;
using System.Globalization;
using Xamarin.Forms;
using System.Collections.Generic;
using GalleyFramework.Extensions;

namespace GalleyFramework.Views.Converters
{
    public static class GConverters
    {
        private static readonly object _locker;
        private static readonly Dictionary<string, Func<IValueConverter>> _bindings;
        private static readonly Dictionary<string, IValueConverter> _converters;

        static GConverters()
        {
            _locker = new object();
            _bindings = new Dictionary<string, Func<IValueConverter>>();
            _converters = new Dictionary<string, IValueConverter>();
        }

        public static void Reg<TConverter>(string key) where TConverter : class, IValueConverter, new()
        {
            lock (_locker)
            {
                _converters.Remove(key);
                _bindings[key] = () => new TConverter();
            }
        }

        public static void UnReg(string key)
        {
            lock (_locker)
            {
                _bindings.Remove(key);
                _converters.Remove(key);
            }
        }

        public static IValueConverter Get(string key)
        {
            lock (_locker)
            {
                _converters.ContainsKey(key).Else(() => _converters.Add(key, _bindings[key].Invoke()));
                return _converters[key];
            }
        }
    }

    public abstract class GalleyConverter : IValueConverter
    {
        public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
