// 1211
using System;
namespace GalleyFramework.Helpers.Interfaces.Services
{
    public interface IDeviceService
    {
        GalleyDeviceInfo Info { get; }
    }

    public struct GalleyDeviceInfo
    {
        public string SystemVersion { get; set; }
        public string Model { get; set; }
        public double ScreenScale { get; set; }
        public double ScreenWidth { get; set; }
        public double ScreenHeight { get; set; }
    }
}
