// 1410
using System;
using System.Collections.Generic;
namespace GalleyFramework.Helpers.Flow
{
    public sealed class GalleyRequest
    {
        public string HttpClientBaseUrl { get; internal set; }
        public string ApiUrl { get; internal set; }
        public IEnumerable<KeyValuePair<string, string>> QueryParameters { get; internal set; }
        public object Body { get; internal set; }
        public string HttpMethod { get; internal set; }
        public int AttemptCount { get; internal set; }
    }
}
