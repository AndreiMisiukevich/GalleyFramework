using System;
using System.Collections.Generic;
using GalleyFramework.Extensions;
using GalleyFramework.Extensions.Flow;
using System.Linq;

namespace GalleyFramework.ViewModels.Flow
{
    public static class GalleySignalDispatcher
    {
        private static readonly Dictionary<Type, Dictionary<IGalleySignalHandler, object>> _signalsMapping = new Dictionary<Type, Dictionary<IGalleySignalHandler, object>>();

        public static TSignal SendSignal<TSignal>(TSignal signal, bool isSync = true) where TSignal : GalleyBaseSignal
        {
            _signalsMapping.TryGetValue(signal.GetType(), out Dictionary<IGalleySignalHandler, object> listeners)
                .Then(() => listeners.ToArray().Each(l => (l.Value.As<Action<TSignal>>() ?? l.Key.HandleSignal).Invoke(signal), isSync ? ExecuteStrategy.Default : ExecuteStrategy.SeparateTask));
            return signal;
        }

        public static TSignal SendSignal<TSignal>(bool isSync = true) where TSignal : GalleyBaseSignal, new()
        => SendSignal(new TSignal(), isSync);

        public static void RemoveSignalHandler<TSignal>(this IGalleySignalHandler element) where TSignal : GalleyBaseSignal
        => _signalsMapping.TryGetValue(typeof(TSignal), out Dictionary<IGalleySignalHandler, object> listeners)
                  .Then(() => listeners.Remove(element));

        public static void ClearSignalHandlers(this IGalleySignalHandler element)
        => _signalsMapping.Values.Each(listeners => listeners.Remove(element));

        public static void AddSignalHandler<TSignal>(this IGalleySignalHandler element, Action<TSignal> action = null) where TSignal : GalleyBaseSignal
        => _signalsMapping.TryGetValue(typeof(TSignal), out Dictionary<IGalleySignalHandler, object> listeners)
               .Then(() => listeners[element] = action)
               .Else(() => _signalsMapping.Add(typeof(TSignal), new Dictionary<IGalleySignalHandler, object>
               {
                   [element] = action
               }));
    }
}
