using System.Reflection;
using GalleyFramework.Extensions;

namespace GalleyFramework.ViewModels.Flow
{
    public sealed class GalleyArgsWrapper
    {
        private readonly object _args;

        public GalleyArgsWrapper(object args)
        {
            _args = args;
            IsEmpty = args.IsNull();
        }

        public bool IsEmpty { get; }

        public bool TryGetValue<TResult>(out TResult result, string id = null)
        {
            var value = id != null
                ? _args?.GetType().GetRuntimeProperty(id)?.GetValue(_args)
                : _args;
            if (value is TResult res)
            {
                result = res;
                return true;
            }
            result = default(TResult);
            return false;
        }

        public TResult GetValue<TResult>(string id = null)
        => TryGetValue(out TResult res, id) ? res : default(TResult);
    }
}