using System;
using System.Runtime.CompilerServices;
using GalleyFramework.Helpers.Flow;
namespace GalleyFramework.Sample.Locales
{
    public class EnLocale : BaseLocale
    {
        public EnLocale() : base("en")
        {
        }

        public override string Get([CallerMemberName] string key = null)
        {
			//you can read it from anywhere (dictionary, file, class, network etc.)
			//You don't have to write hard code

			if(key == nameof(LocaleName))
            {
                return "English";
            }
            if(key == nameof(PressMe))
            {
                return "Press me";
            }
            return null;
        }
    }
}
