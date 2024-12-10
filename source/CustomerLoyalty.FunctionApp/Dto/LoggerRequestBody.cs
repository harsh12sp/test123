using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace BenjaminMoore.Api.Retail.Pos.Common.Dto
{
    public class LoggerRequestBody
    {
        [JsonProperty("logType")]
        public string LogType { get; set; }

        [JsonProperty("logSource")]
        public string LogSource { get; set; }

        [JsonProperty("requestInfo")]
        public HttpRequestInfo RequestInfo { get; set; }

        [JsonProperty("config")]
        public LoggerConfig Config { get; set; }

        [JsonProperty("info")]
        public LoggerInfo Info { get; set; }

        [JsonProperty("debug")]
        public LoggerDebug Debug { get; set; }

        [JsonProperty("warning")]
        public LoggerWarning Warning { get; set; }

        [JsonProperty("error")]
        public LoggerError Erorr { get; set; }

        [JsonProperty("timestamp")]
        public string Timestamp { get; set; }
    }

    public class LoggerConfig
    {
        [JsonProperty("logDestination")]
        public string LogDestination { get; set; }

        [JsonProperty("containerName")]
        public string ContainerName { get; set; }

        [JsonProperty("directoryName")]
        public string DirectoryName { get; set; }
    }

    public class LoggerInfo
    {
        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }

    public class LoggerDebug
    {
        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }

    public class LoggerWarning
    {
        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }

    public class LoggerError
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("stack")]
        public string StackTrace { get; set; }
    }

    public enum LoggerLogType
    {
        [EnumMember(Value = "info")]
        Info,
        [EnumMember(Value = "debug")]
        Debug,
        [EnumMember(Value = "warning")]
        Warning,
        [EnumMember(Value = "error")]
        Error
    }
}
