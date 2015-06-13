using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnergenieLibv2.Helper
{
    public static class RunAsyncHelper
    {
        internal static async Task<T> RunAsyncWithRetries<T>(Func<Task<T>> work, int retries)
        {
            var nextDelay = TimeSpan.FromSeconds(1);
            for (int i = 0; i != retries; ++i)
            {
                try
                {
                    return await work();
                }
                catch (Exception)
                {
                }
                await Task.Delay(nextDelay);
                nextDelay = nextDelay + nextDelay;
            }
            return await work();
        }
        internal static async Task<T> RunAsyncWithTimeOut<T>(Func<T> work, int timeout)
        {
            var task = Task.Run(delegate
            {
                return work();
            });

            if (task == await Task.WhenAny(task, Task.Delay(TimeSpan.FromSeconds(timeout))))
            {
                return await task;
            }
            else
            {
                throw new TimeoutException("Socket Connecting timed out");
            }
        }
    }
}
