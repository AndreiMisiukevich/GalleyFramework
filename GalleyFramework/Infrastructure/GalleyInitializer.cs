using GalleyFramework.Helpers.Interfaces;
using GalleyFramework.Helpers;
using GalleyFramework.Services;

namespace GalleyFramework.Infrastructure
{
    public class GalleyInitializer
    {
        public virtual INavigationHelper GetNavigationHelper() => new GalleyNavigationHelper();
        public virtual IDialogHelper GetDialogHelper() => new GalleyDialogHelper();
        public virtual IHttpHelper GetHttpHelper() => new GalleyHttpHelper();
        public virtual ILocalizationHelper GetLocalizationHelper() => new GalleyLocalizationHelper();
        public virtual IDeviceHelper GetDeviceHelper() => new GalleyDeviceHelper();
    } 
}
