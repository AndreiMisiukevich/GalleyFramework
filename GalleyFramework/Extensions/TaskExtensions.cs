using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GalleyFramework.Extensions
{
    public static class TaskExtensions
    {
        public static async Task<TValue> Execute<TValue>(this Task<TValue> task, bool awaitInMainThread = false)
        {
            task = task.CheckNull();
            if (awaitInMainThread)
            {
                var completionSource = new TaskCompletionSource<TValue>();
                Device.BeginInvokeOnMainThread(async () =>
                {
                    completionSource.SetResult(await task);
                });
                return await completionSource.Task;
            }
            return await task;
        }

        public static async Task Execute(this Task task, bool awaitInMainThread = false)
        {
            task = task.CheckNull();
            if (awaitInMainThread)
            {
                var completionSource = new TaskCompletionSource<bool>();
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await task;
                    completionSource.SetResult(true);
                });
                await completionSource.Task;
                return;
            }
            await task;
        }

        public static Task<TValue> CheckNull<TValue>(this Task<TValue> task)
        => task ?? Task.FromResult(default(TValue));

        public static Task CheckNull(this Task task)
        => task ?? Task.FromResult(true);

        public static Task<TValue> Wrap<TValue>(this Task task)
        {
            task = task.CheckNull();
            return task.IsCompleted
            ? CompletedAsGeneric<TValue>(task)
            : task.ContinueWith(CompletedAsGeneric<TValue>, TaskContinuationOptions.ExecuteSynchronously).Unwrap();
        }

        private static Task<TValue> CompletedAsGeneric<TValue>(Task completedTask)
        {
            try
            {
                if (completedTask.Status != TaskStatus.RanToCompletion)
                {
                    completedTask.GetAwaiter().GetResult();
                }
                return Task.FromResult(default(TValue));
            }
            catch (OperationCanceledException ex)
            {
                if (completedTask.IsCanceled)
                {
                    return new Task<TValue>(() => default(TValue), ex.CancellationToken);
                }
                throw;
            }
        }
    }
}
