﻿using Xamarin.Forms;
using GalleyFramework.Infrastructure;
using GalleyFramework.Extensions;

namespace GalleyFramework
{
    public class GalleyApp : Application
    {
        public static new GalleyApp Current => Application.Current.As<GalleyApp>();

        public GalleyViewsFactory ViewFactory { get; }

        public new GalleySuperPage MainPage
        {
            get => base.MainPage.As<GalleySuperPage>();
            set => base.MainPage = value;
        }

        protected GalleyApp(GalleyInitializer initializer = null, GalleyViewsFactory viewFactory = null)
        {
            ViewFactory = viewFactory ?? new GalleyViewsFactory();
            var superView = ViewFactory.CreateSuperView();
            MainPage = ViewFactory.CreateSuperPage(superView);
            Galley.SetUp(initializer ?? new GalleyInitializer());
        }

		protected override void OnSleep()
		{
			base.OnSleep();
			Galley.Navigation.CurrentModel?.Hide(true);
		}

        protected override void OnResume()
        {
            base.OnResume();
            Galley.Navigation.CurrentModel?.Show(true);
        }
    }
}
