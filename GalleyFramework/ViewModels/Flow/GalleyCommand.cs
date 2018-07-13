using System;
using System.Windows.Input;
using static System.Math;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GalleyFramework.Helpers.Flow.Exceptions;

namespace GalleyFramework.ViewModels.Flow
{
    public class GalleyCommand : ICommand
    {
        public static event Action<Exception> ExceptionSuppressed;

        public static List<Type> SuppressExceptionTypes { get; } = new List<Type>
        {
            typeof(GalleyHandledException)
        };

        public event EventHandler CanExecuteChanged;
        private readonly Action<object> _action;
        private readonly TimeSpan _actionFrequency;
        private readonly List<Type> _suppressExceptionTypes;
        private DateTime _lastActionTime;

        public GalleyCommand(Action<object> action, TimeSpan? actionFrequency = null, List<Type> suppressExceptionTypes = null)
        {
            _action = action;
            _actionFrequency = actionFrequency ?? TimeSpan.FromMilliseconds(270);
            _suppressExceptionTypes = suppressExceptionTypes ?? SuppressExceptionTypes;
        }

        public GalleyCommand(Action action, TimeSpan? actionFrequency = null, List<Type> suppressExceptionTypes = null) : this(p => action?.Invoke(), actionFrequency, suppressExceptionTypes)
        {
        }

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            var nowTime = DateTime.Now;
            if (_actionFrequency == TimeSpan.Zero
                || Abs((nowTime - _lastActionTime).TotalMilliseconds) >= _actionFrequency.TotalMilliseconds)
            {
                _lastActionTime = nowTime;
                try
                {
                    _action.Invoke(parameter);
                }
                catch (Exception ex)
                {
                    var exType = ex.GetType();
                    var exTypeInfo = exType.GetTypeInfo();
                    if (_suppressExceptionTypes.Any(t => t == exType || exTypeInfo.IsSubclassOf(t)))
                    {
                        ExceptionSuppressed?.Invoke(ex);
                        return;
                    }
                    throw ex;
                }
            }
        }

        public void ChangeCanExecute() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}