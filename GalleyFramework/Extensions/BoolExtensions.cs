using System;
using System.Threading.Tasks;
using GalleyFramework.Extensions.Flow;
using Xamarin.Forms;

namespace GalleyFramework.Extensions
{
    public static class BoolExtensions
    {
		public static bool Then(this bool res, Action action)
		{
			if (res)
			{
				action?.Invoke();
			}
			return res;
		}

		public static bool Else(this bool res, Action action)
		{
			return (!res).Then(action);
		}

        public static bool Then(this bool res, Action action, ExecuteStrategy strategy)
        {
            if (res)
            {
                switch (strategy)
                {
                    case ExecuteStrategy.Default:
                        action?.Invoke();
                        break;
                    case ExecuteStrategy.SeparateTask:
                        Task.Run(action);
                        break;
                    case ExecuteStrategy.MainThread:
                        Device.BeginInvokeOnMainThread(action);
                        break;
                }
            }
            return res;
        }

        public static bool Else(this bool res, Action action, ExecuteStrategy strategy)
        {
            return (!res).Then(action, strategy);
        }

        public static bool And(this bool res, bool other)
        {
            return res && other;
        }

		public static bool Or(this bool res, bool other)
		{
			return res || other;
		}

		public static bool Not(this bool res)
		{
			return !res;
		}
    }
}

