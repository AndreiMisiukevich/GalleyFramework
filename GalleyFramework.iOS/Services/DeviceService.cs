﻿using System;
using GalleyFramework.iOS.Services;
using Xamarin.Forms;
using GalleyFramework.Helpers.Interfaces.Services;
using UIKit;
using System.Runtime.InteropServices;
using Xamarin.Forms.Internals;

[assembly: Dependency(typeof(DeviceService))]
namespace GalleyFramework.iOS.Services
{
    [Preserve(AllMembers = true)]
    public class DeviceService : IDeviceService
    {
        public GalleyDeviceInfo Info
        {
            get => new GalleyDeviceInfo
            {
                Model = UIDevice.CurrentDevice.GetModelName(),
                SystemVersion = UIDevice.CurrentDevice.SystemVersion,
                ScreenScale = UIScreen.MainScreen.Scale,
                ScreenWidth = UIScreen.MainScreen.Bounds.Width,
                ScreenHeight = UIScreen.MainScreen.Bounds.Height
            };
        }
    }

    public static class UIDeviceExtensions
    {
        private const string HardwareProperty = "hw.machine";

        [DllImport("libc", CallingConvention = CallingConvention.Cdecl)]
        static internal extern int sysctlbyname([MarshalAs(UnmanagedType.LPStr)] string property, IntPtr output, IntPtr oldLen, IntPtr newp, uint newlen);


        public static string GetModelName(this UIDevice device)
        {
            var pLen = Marshal.AllocHGlobal(sizeof(int));
            sysctlbyname(HardwareProperty, IntPtr.Zero, pLen, IntPtr.Zero, 0);
            var length = Marshal.ReadInt32(pLen);
            var pStr = Marshal.AllocHGlobal(length);
            sysctlbyname(HardwareProperty, pStr, pLen, IntPtr.Zero, 0);
            var hardwareStr = Marshal.PtrToStringAnsi(pStr);

            Marshal.FreeHGlobal(pLen);
            Marshal.FreeHGlobal(pStr);

            switch (hardwareStr)
            {
                case "iPod5,1":
                    return "iPod Touch 5";
                case "iPod7,1":
                    return "iPod Touch 6";
                case "iPhone3,1":
                case "iPhone3,2":
                case "iPhone3,3":
                    return "iPhone 4";
                case "iPhone4,1":
                    return "iPhone 4s";
                case "iPhone5,1":
                case "iPhone5,2":
                    return "iPhone 5";
                case "iPhone5,3":
                case "iPhone5,4":
                    return "iPhone 5c";
                case "iPhone6,1":
                case "iPhone6,2":
                    return "iPhone 5s";
                case "iPhone7,2":
                    return "iPhone 6";
                case "iPhone7,1":
                    return "iPhone 6 Plus";
                case "iPhone8,1":
                    return "iPhone 6s";
                case "iPhone8,2":
                    return "iPhone 6s Plus";
                case "iPhone9,1":
                case "iPhone9,3":
                    return "iPhone 7";
                case "iPhone9,2":
                case "iPhone9,4":
                    return "iPhone 7 Plus";
                case "iPhone8,4":
                    return "iPhone SE";
                case "iPhone10,1":
                case "iPhone10,4":
                    return "iPhone 8";
                case "iPhone10,2":
                case "iPhone10,5":
                    return "iPhone 8 Plus";
                case "iPhone10,3":
                case "iPhone10,6":
                    return "iPhone X";
                case "iPad2,1":
                case "iPad2,2":
                case "iPad2,3":
                case
                    "iPad2,4":
                    return "iPad 2";
                case "iPad3,1":
                case "iPad3,2":
                case
                    "iPad3,3":
                    return "iPad 3";
                case "iPad3,4":
                case "iPad3,5":
                case
                    "iPad3,6":
                    return "iPad 4";
                case "iPad4,1":
                case "iPad4,2":
                case
                    "iPad4,3":
                    return "iPad Air";
                case "iPad5,3":
                case
                    "iPad5,4":
                    return "iPad Air 2";
                case "iPad6,11":
                case "iPad6,12":
                    return "iPad 5";
                case "iPad2,5":
                case "iPad2,6":
                case "iPad2,7":
                    return "iPad Mini";
                case "iPad4,4":
                case "iPad4,5":
                case "iPad4,6":
                    return "iPad Mini 2";
                case "iPad4,7":
                case "iPad4,8":
                case "iPad4,9":
                    return "iPad Mini 3";
                case "iPad5,1":
                case "iPad5,2":
                    return "iPad Mini 4";
                case "iPad6,3":
                case "iPad6,4":
                    return "iPad Pro 9.7 Inch";
                case "iPad6,7":
                case "iPad6,8":
                    return "iPad Pro 12.9 Inch";
                case "iPad7,1":
                case "iPad7,2":
                    return "iPad Pro 12.9 Inch 2. Generation";
                case "iPad7,3":
                case "iPad7,4":
                    return "iPad Pro 10.5 Inch";
                case "AppleTV5,3":
                    return "Apple TV";
                case "AppleTV6,2":
                    return "Apple TV 4K";
                case "AudioAccessory1,1":
                    return "HomePod";
                case "i386":
                case "x86_64":
                    return "Simulator";
                default:
                    return hardwareStr;
            }
        }
    }
}
