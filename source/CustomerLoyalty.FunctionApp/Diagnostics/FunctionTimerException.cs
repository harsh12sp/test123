using System;

namespace BenjaminMoore.Api.Retail.Pos.Common.Diagnostics
{
    public class FunctionTimerException : Exception
    {
        public TimeSpan ExecutionTime { get; }

        public FunctionTimerException(TimeSpan executionTime, string message, Exception innerException) : base(message, innerException)
        {
            ExecutionTime = executionTime;
        }
    }
}