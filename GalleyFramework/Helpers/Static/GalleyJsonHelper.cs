using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using GalleyFramework.Extensions;
using System.Linq;

namespace GalleyFramework.Helpers.Static
{
    public static class GalleyJsonHelper
    {
        private const string MediaType = "application/json";

        public static StringContent Serialize(object entity)
        {
            try
            {
                return CreateContent(entity != null ? JsonConvert.SerializeObject(entity) : string.Empty);
            }
            catch
            {
                return CreateContent(string.Empty);
            }
        }

        public static StringContent SerializeIfNotNull(object entity)
        {
            try
            {
                return entity != null ? CreateContent(JsonConvert.SerializeObject(entity)) : null;
            }
            catch
            {
                return CreateContent(string.Empty);
            }
        }

        public static TResult Deserialize<TResult>(string content, params string[] valueKeys)
        {
            try
            {
                var value = (valueKeys?.Any() ?? false)
                    ? GetValue(content, valueKeys)
                    : content;
                if (value.IsNull()) return default(TResult);
                return JsonConvert.DeserializeObject<TResult>(value);
            }
            catch
            {
                return default(TResult);
            }
        }

        public static string GetValue(string content, params string[] valueKeys)
        {
            if (content.IsNull()) return null;
            try
            {
                var jsonToken = JToken.Parse(content);
                if (valueKeys.NotNull())
                {
                    foreach(var key in valueKeys)
                    {
                        if (jsonToken.IsNull()) break;
                        jsonToken = jsonToken[key];
                    }
                }
                return jsonToken?.ToString();
            }
            catch
            {
                return null;
            }
        }

        private static StringContent CreateContent(string jsonString) => new StringContent(jsonString, Encoding.UTF8, MediaType);
    }
}