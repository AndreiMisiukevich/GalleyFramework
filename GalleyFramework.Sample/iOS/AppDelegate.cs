using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;
using GalleyFramework.iOS;

namespace GalleyFramework.Sample.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();
            GF.Init();
            LoadApplication(new SampleApp());

            return base.FinishedLaunching(app, options);
        }
    }
}
