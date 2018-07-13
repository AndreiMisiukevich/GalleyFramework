using System;
using System.Threading.Tasks;
using GalleyFramework.Helpers.Static;
using GalleyFramework.Infrastructure;
using System.Net;
using GalleyFramework.Extensions;

namespace GalleyFramework.Helpers.Flow
{
    public enum GalleyClientErrorCode { Ok = 0, NoInternet, SomethingWrong }

    public class GalleyResponse
    {
        private string _data;
        public string Data
        {
            get => _data;
            set
            {
                _data = value;
                SetDataError();
            }
        }

        public GalleyRequest Request { get; internal set; }

        public HttpStatusCode StatusCode { get; set; }

        public GalleyClientErrorCode ClientErrorCode { get; protected set; }

        public bool IsHttpOkStatus => ((int)StatusCode >= 200) && ((int)StatusCode <= 299);

        public bool HasError { get; protected set; }

        public Exception Exception { get; set; }

        public bool IsOk => IsHttpOkStatus && !HasError;

        public bool ShouldRetry { get; set; }

        public virtual string ErrorMessage { get; protected set; }

        public bool ContainsKey(params string[] keys) => GalleyJsonHelper.GetValue(Data, keys).NotNull();

        public TValue GetValue<TValue>(params string[] keys)
        {
            var str = GalleyJsonHelper.GetValue(Data, keys);
            return !string.IsNullOrEmpty(str)
                          ? (TValue)Convert.ChangeType(str, typeof(TValue))
                          : default(TValue);
        }

        public TValue CreateObject<TValue>(params string[] keys)
        => GalleyJsonHelper.Deserialize<TValue>(Data, keys);


        public virtual void SetDataError() => SetErrorMessage(GetValue<string>("error"));

        public virtual void SetClientError(GalleyClientErrorCode clientErrorCode)
        {
            SetErrorMessage(GetClientErrorMessage(clientErrorCode));
            ClientErrorCode = clientErrorCode;
        }

        protected string GetClientErrorMessage(GalleyClientErrorCode clientErrorCode)
        {
            switch (clientErrorCode)
            {
                case GalleyClientErrorCode.NoInternet:
                    return Galley.Loc.CurrentLocale.NoInternetError;
                case GalleyClientErrorCode.SomethingWrong:
                    return Galley.Loc.CurrentLocale.SomethingWrongError;
                default: return null;
            }
        }

        private void SetErrorMessage(string errorMessage)
        {
			ErrorMessage = errorMessage;
			HasError = errorMessage.NotNull();
        }
    }

    public class GalleyResponse<TEntityData> : GalleyResponse where TEntityData : class
    {
        private TEntityData _entity;
        public TEntityData EntityData
        {
            get => IsOk
                    ? _entity ?? (_entity = CreateObject<TEntityData>())
                    : default(TEntityData);
            set => _entity = value;
        }
    }

    public static class GalleyResponseExtensions
    {
        public static async Task ExecuteOrShowError<TWrapper>(this TWrapper resp, Action<TWrapper> action) where TWrapper : GalleyResponse
		{
			if (resp.IsOk)
			{
				action?.Invoke(resp);
			}
			else if (resp.HasError)
			{
                await Galley.Dialog.ShowAlert(Galley.Loc.CurrentLocale.ErrorTitle, resp.ErrorMessage);
			}
		}

		public static async Task ExecuteOrShowError<TWrapper>(this TWrapper resp, Action action) where TWrapper : GalleyResponse
		=>  await resp.ExecuteOrShowError((r) => action?.Invoke());
		
		public static TWrapper OnSuccess<TWrapper>(this TWrapper resp, Action<TWrapper> action) where TWrapper : GalleyResponse
		{
			resp.IsOk.Then(() => action.Invoke(resp));
			return resp;
		}

		public static TWrapper OnSuccess<TWrapper>(this TWrapper resp, Action action) where TWrapper : GalleyResponse
		=> resp.OnSuccess(r => action.Invoke());

		public static TWrapper OnError<TWrapper>(this TWrapper resp, Action<TWrapper> action) where TWrapper : GalleyResponse
		{
            resp.IsOk.Else(() => action.Invoke(resp));
			return resp;
		}

		public static TWrapper OnError<TWrapper>(this TWrapper resp, Action action) where TWrapper : GalleyResponse
        => resp.OnError(r => action.Invoke());
    }
}