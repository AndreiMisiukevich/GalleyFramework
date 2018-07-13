using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ModernHttpClient;
using Plugin.Connectivity;
using GalleyFramework.Helpers.Interfaces;
using GalleyFramework.Helpers.Static;
using GalleyFramework.Helpers.Flow;
using GalleyFramework.Extensions;
using System.Text;
using System.Runtime.CompilerServices;
using GalleyFramework.Helpers.Flow.Exceptions;

namespace GalleyFramework.Helpers
{
    public class GalleyHttpHelper : IHttpHelper
    {
        public HttpClient Client { get; set; } = new HttpClient(new NativeMessageHandler());

        public Task<TWrapper> Post<TParameter, TWrapper>(string apiMethodName, TParameter body, params KeyValuePair<string, string>[] queryParameters) where TWrapper : GalleyResponse, new()
        {
            return ExecuteHttpMethod<TWrapper>(apiMethodName, queryParameters, (apiPath, contentBody) => Client.PostAsync(apiPath, contentBody), body);
        }

        public Task<TWrapper> Post<TWrapper>(string apiMethodName, params KeyValuePair<string, string>[] queryParameters) where TWrapper : GalleyResponse, new()
        {
            return Post<string, TWrapper>(apiMethodName, string.Empty, queryParameters);
        }

        public Task<TWrapper> Get<TWrapper>(string apiMethodName, params KeyValuePair<string, string>[] queryParameters) where TWrapper : GalleyResponse, new()
        {
            return ExecuteHttpMethod<TWrapper>(apiMethodName, queryParameters, (apiPath, contentBody) => Client.GetAsync(apiPath));
        }

		public Task<TWrapper> Put<TParameter, TWrapper>(string apiMethodName, TParameter body, params KeyValuePair<string, string>[] queryParameters) where TWrapper : GalleyResponse, new()
		{
            return ExecuteHttpMethod<TWrapper>(apiMethodName, queryParameters, (apiPath, contentBody) => Client.PutAsync(apiPath, contentBody), body);
		}

        public Task<TWrapper> Put<TWrapper>(string apiMethodName, params KeyValuePair<string, string>[] queryParameters) where TWrapper : GalleyResponse, new()
        {
            return Put<string, TWrapper>(apiMethodName, string.Empty, queryParameters);
        }

		public Task<TWrapper> Delete<TWrapper>(string apiMethodName, params KeyValuePair<string, string>[] queryParameters) where TWrapper : GalleyResponse, new()
		{
            return ExecuteHttpMethod<TWrapper>(apiMethodName, queryParameters, (apiPath, contentBody) => Client.DeleteAsync(apiPath));
		}

        protected virtual Task<TWrapper> OnRequest<TWrapper>(string apiMethodName, IEnumerable<KeyValuePair<string, string>> queryParameters, object body) where TWrapper : GalleyResponse, new()
        => Task.FromResult<TWrapper>(null);

        protected virtual Task<TWrapper> OnResponse<TWrapper>(TWrapper response) where TWrapper : GalleyResponse, new()
        => Task.FromResult<TWrapper>(null);

        protected virtual void OnException(Exception ex)
        {
        }

        protected virtual async Task<TWrapper> OnExecuteHttpMethod<TWrapper>(string apiMethodName, IEnumerable<KeyValuePair<string, string>> queryParameters, Func<string, StringContent, Task<HttpResponseMessage>> getResponse, object body = null) where TWrapper : GalleyResponse, new()
        {
            try
            {
                if (!CrossConnectivity.Current.IsConnected)
                {
                    return GetClientErrorRespone<TWrapper>(GalleyClientErrorCode.NoInternet);
                }
                var beforeReqResp = await OnRequest<TWrapper>(apiMethodName, queryParameters, body);
                if (beforeReqResp.NotNull())
                {
                    return beforeReqResp;
                }
                var httpResponse = await getResponse(CreateFullApiPath(apiMethodName, queryParameters), GalleyJsonHelper.SerializeIfNotNull(body));
                var respWrapper = new TWrapper
                {
                    StatusCode = httpResponse.StatusCode,
                    Data = httpResponse.Content.NotNull()
                                   ? await httpResponse.Content?.ReadAsStringAsync().Execute()
                                   : null
                };
                var afterResponseResp = await OnResponse(respWrapper);
                if (afterResponseResp.NotNull())
                {
                    return afterResponseResp;
                }
                return respWrapper;
            }
            catch (Exception ex) when (!(ex is GalleyHandledException))
            {
                OnException(ex);
                return GetClientErrorRespone<TWrapper>(GalleyClientErrorCode.SomethingWrong, ex);
            }
        }

        protected string CreateFullApiPath(string apiMethodName, IEnumerable<KeyValuePair<string, string>> queryParameters)
        {
            if (queryParameters?.Any() ?? false)
            {
                var apiMethodNameBuilder = new StringBuilder(apiMethodName);
                apiMethodNameBuilder.Append(apiMethodName.Contains("?") ? '&' : '?');
                queryParameters.Each(p => apiMethodNameBuilder.Append($"{p.Key}={p.Value}&"));
                return apiMethodNameBuilder.ToString();
            }
            return apiMethodName;
        }

        protected TWrapper GetClientErrorRespone<TWrapper>(GalleyClientErrorCode clientStatusCode, Exception exception = null) where TWrapper : GalleyResponse, new()
        {
            var resp = new TWrapper
            {
                Exception = exception
            };
            resp.SetClientError(clientStatusCode);
            return resp;
        }

        protected async Task<TWrapper> ExecuteHttpMethod<TWrapper>(string apiMethodName, KeyValuePair<string, string>[] queryParameters, Func<string, StringContent, Task<HttpResponseMessage>> getResponse, object body = null, [CallerMemberName] string methodName = null, int attemptCount = 1) where TWrapper : GalleyResponse, new()
        {
            var extendedQueryParams = OnExtendQueryParameters(queryParameters);
            var resp = await OnExecuteHttpMethod<TWrapper>(apiMethodName, extendedQueryParams, getResponse, body);

            if(resp.ShouldRetry)
            {
                return await ExecuteHttpMethod<TWrapper>(apiMethodName, queryParameters, getResponse, body, methodName, ++attemptCount);
            }

            resp.Request = new GalleyRequest
            {
                HttpClientBaseUrl = Client.BaseAddress?.AbsoluteUri,
                ApiUrl = apiMethodName,
                QueryParameters = extendedQueryParams,
                Body = body,
                HttpMethod = methodName,
                AttemptCount = attemptCount
            };
            return resp;
        }

        protected virtual IEnumerable<KeyValuePair<string, string>> OnExtendQueryParameters(KeyValuePair<string, string>[] queryParameters)
        => queryParameters;
    }
}