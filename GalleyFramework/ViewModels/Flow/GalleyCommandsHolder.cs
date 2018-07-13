using System;
using System.Windows.Input;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GalleyFramework.Extensions;

namespace GalleyFramework.ViewModels.Flow
{
    public abstract class GalleyCommandsHolder
    {
        private readonly Dictionary<string, ICommand> _commandsMapping = new Dictionary<string, ICommand>();

        protected ICommand GetCommand(Action<object> action, TimeSpan? actionFrequency = null, List<Type> suppressExceptionTypes = null, [CallerMemberName] string name = null)
        => CheckCommand(name) ?? RegCommand(action, actionFrequency, suppressExceptionTypes, name);

        protected ICommand GetCommand(Action action, TimeSpan? actionFrequency = null, List<Type> suppressExceptionTypes = null, [CallerMemberName]  string name = null)
        => GetCommand(p => action?.Invoke(), actionFrequency, suppressExceptionTypes, name);

        protected ICommand CheckCommand([CallerMemberName]  string name = null)
        => _commandsMapping.TryGetValue(name, out ICommand command)
            ? command
            : null;

        protected ICommand RegCommand(Action<object> action, TimeSpan? actionFrequency = null, List<Type> suppressExceptionTypes = null, [CallerMemberName] string name = null)
        => _commandsMapping[name] = new GalleyCommand(p => action?.Invoke(p), actionFrequency, suppressExceptionTypes);

        protected ICommand RegCommand(Action action, TimeSpan? actionFrequency = null, List<Type> suppressExceptionTypes = null, [CallerMemberName] string name = null)
        => RegCommand(p => action?.Invoke(), actionFrequency, suppressExceptionTypes, name);
    }
}
