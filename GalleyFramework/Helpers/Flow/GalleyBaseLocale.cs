using System;
using System.Runtime.CompilerServices;
namespace GalleyFramework.Helpers.Flow
{
    public abstract class GalleyBaseLocale
    {
        protected GalleyBaseLocale(string localeCode)
        {
            Code = localeCode;
        }

        public string Code { get; }

        public virtual string ErrorTitle => "Error";
        public virtual string OkText => "Ok";
        public virtual string YesText => "Yes";
        public virtual string NoText => "No";
        public virtual string NoInternetError => "No internet connection";
        public virtual string SomethingWrongError => "Something went wrong";

        public abstract string Get([CallerMemberName] string key = null);

        public virtual void OnAccepted()
        {
        }

        public virtual void OnDeclined()
        {
        }

        public virtual void OnRefreshed()
        {
        }
    }

    public sealed class GalleyNoLocale : GalleyBaseLocale
    {
        public GalleyNoLocale() : base(null)
        {
            
        }

        public override string Get([CallerMemberName] string key = null)
        {
            throw new NotImplementedException("There is no localization");
        }
    }
}
