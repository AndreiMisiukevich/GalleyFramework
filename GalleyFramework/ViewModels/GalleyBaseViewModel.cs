using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using GalleyFramework.ViewModels.Flow;
using GalleyFramework.Helpers.Flow;
using GalleyFramework.Infrastructure;
using GalleyFramework.Extensions;
using System.Threading;

namespace GalleyFramework.ViewModels
{
    public static class GalleyBaseViewModelExtensions
    {
        public static TModel Init<TModel>(this TModel model, object args = null) where TModel : GalleyBaseViewModel
        {
            model.Initialize(new GalleyArgsWrapper(args));
            return model;
        }
    }

    public abstract class GalleyBaseViewModel : GalleyCommandsHolder, IGalleySignalHandler, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event Action<GalleyBaseViewModel, GalleyArgsWrapper> Initialized;
        public event Action<GalleyBaseViewModel> Destroyed;
        public event Action<GalleyBaseViewModel, bool> Shown;
        public event Action<GalleyBaseViewModel, bool> Hided;

        private bool _isBusy;
        private bool _isDestroyed;


        protected GalleyBaseViewModel() : this(GalleyCreateType.ViewEveryAppearing)
        {
        }

        protected GalleyBaseViewModel(GalleyCreateType createType)
        {
            CreateViewType = createType;
        }

        public virtual bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsFree));
            }
        }

        public bool IsFree => !IsBusy;
        public GalleyCreateType CreateViewType { get; }
        public GalleyBaseLocale Locale => Galley.Loc.CurrentLocale;

        public bool IsDestroyed
        {
            get => _isDestroyed;
            private set
            {
                _isDestroyed = value;
                OnPropertyChanged();
            }
        }

        public ICommand BackCommand => CheckCommand() ?? RegCommand(BackAction);

        public virtual void HandleSignal(GalleyBaseSignal signal)
        {
        }

        public void Initialize(GalleyArgsWrapper args)
        {
            Galley.Loc.LocaleChanged += OnLocaleChanged;
            OnInitialized(args);
            Initialized?.Invoke(this, args);
        }

        public void Destroy()
        {
            Galley.Loc.LocaleChanged -= OnLocaleChanged;
            IsDestroyed = true;
            this.ClearSignalHandlers();
            OnDestroyed();
            Destroyed?.Invoke(this);

            Initialized = null;
            Shown = null;
            Hided = null;
            Destroyed = null;
        }

        public void Show(bool fromBackgroundMode = false, GalleyArgsWrapper args = null)
        {
            OnShown(fromBackgroundMode, args ?? new GalleyArgsWrapper(null));
            Shown?.Invoke(this, fromBackgroundMode);
        }

		public void Hide(bool toBackgroundMode = false)
		{
			OnHided(toBackgroundMode);
			Hided?.Invoke(this, toBackgroundMode);
		}

        protected virtual void OnInitialized(GalleyArgsWrapper args)
		{
		}

        protected virtual void OnDestroyed()
		{
		}

        protected virtual void OnShown(bool fromBackgroundMode, GalleyArgsWrapper args)
		{
		}

        protected virtual void OnHided(bool toBackgroundMode)
		{
		}

        protected virtual async void BackAction() => await Galley.Navigation.Pop();

        protected async Task<TResult> InvokeWithIsBusy<TResult>(Func<Task<TResult>> action, int delay = int.MaxValue)
        {
            IsBusy = true;
            var cts = new CancellationTokenSource();
            try
            {
                var task = action?.Invoke();
                await Task.WhenAny(
                    task.Execute(),
                    Task.Delay(delay, cts.Token));

                if(task.IsCompleted)
                {
                    return task.Result;
                }
                return default(TResult);
            }
            finally
            {
                cts.Cancel();
                IsBusy = false;
            }
        }

        protected async Task InvokeWithIsBusy(Func<Task> action, int delay = int.MaxValue)
        => await InvokeWithIsBusy(() => action?.Invoke()?.Wrap<bool>(), delay);

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        private void OnLocaleChanged() => OnPropertyChanged(nameof(Locale));
    }
}
