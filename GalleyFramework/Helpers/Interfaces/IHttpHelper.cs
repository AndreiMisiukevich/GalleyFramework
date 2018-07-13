using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using GalleyFramework.Helpers.Flow;

namespace GalleyFramework.Helpers.Interfaces
{
    public interface IHttpHelper
    {
        HttpClient Client { get; set; }

        Task<TWrapper> Post<TParameter, TWrapper>(string apiMethodName, TParameter body, params KeyValuePair<string, string>[] queryParams) where TWrapper : GalleyResponse, new();
		Task<TWrapper> Post<TWrapper>(string apiMethodName, params KeyValuePair<string, string>[] queryParams) where TWrapper : GalleyResponse, new();
        Task<TWrapper> Get<TWrapper>(string apiMethodName, params KeyValuePair<string, string>[] queryParams) where TWrapper : GalleyResponse, new();
		Task<TWrapper> Put<TParameter, TWrapper>(string apiMethodName, TParameter body, params KeyValuePair<string, string>[] queryParams) where TWrapper : GalleyResponse, new();
		Task<TWrapper> Put<TWrapper>(string apiMethodName, params KeyValuePair<string, string>[] queryParams) where TWrapper : GalleyResponse, new();
		Task<TWrapper> Delete<TWrapper>(string apiMethodName, params KeyValuePair<string, string>[] queryParams) where TWrapper : GalleyResponse, new();
    }
}
