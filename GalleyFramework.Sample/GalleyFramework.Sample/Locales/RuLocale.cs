using System;
using Xamarin.Forms;
using GalleyFramework.Helpers.Flow;
using System.Runtime.CompilerServices;

namespace GalleyFramework.Sample.Locales
{
    public class RuLocale : BaseLocale
    {
        public RuLocale() : base("ru")
        {
        }

        public override string ErrorTitle => Get();

        public override string Get([CallerMemberName] string key = null)
        {
			//you can read it from anywhere (dictionary, file, class, network etc.)
            //You don't have to write hard code
			if (key == nameof(LocaleName))
			{
				return "Русский";
			}
            if (key == nameof(ErrorTitle))
            {
                return "Ошибка";
            }
			if (key == nameof(PressMe))
			{
				return "Нажми меня нежно";
			}
			return null;
        }
    }
}
