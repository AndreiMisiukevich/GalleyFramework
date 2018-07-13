// 1211
using System;
namespace GalleyFramework.Helpers.Interfaces
{
    public interface IDeviceHelper
    {
        string Os { get; }
        string LocaleName { get; }
        string SystemVersion { get;}
        string Model { get; }
        double ScreenScale { get; }
        double ScreenWidth { get; }
        double ScreenHeight { get; }
        bool OpenDialer(string number);
        bool OpenSms(string number);
        bool OpenEmail(string email);
        bool OpenUrl(string url);
    }
}
