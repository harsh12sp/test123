using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace BenjaminMoore.Api.Retail.Pos.Common.Diagnostics
{
    public static class FunctionTimer
    {
        public static async Task<TimedFunctionResult<T>> ExecuteWithTimer<T>(Func<Task<T>> action)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            try
            {
                T result = await action();
                stopwatch.Stop();

                return new TimedFunctionResult<T>(stopwatch.Elapsed, result);
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                throw new FunctionTimerException(stopwatch.Elapsed,
                    "Async execution failed, see inner exception for details", ex);
            }
        }
    }
}