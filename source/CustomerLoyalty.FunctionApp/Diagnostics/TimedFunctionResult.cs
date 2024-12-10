using System;

namespace BenjaminMoore.Api.Retail.Pos.Common.Diagnostics
{
    public class TimedFunctionResult<T>
    {
        public TimeSpan ExecutionTime { get; }
        public T Result { get; }

        public TimedFunctionResult(TimeSpan executionTime, T result)
        {
            ExecutionTime = executionTime;
            Result = result;
        }
    }
}