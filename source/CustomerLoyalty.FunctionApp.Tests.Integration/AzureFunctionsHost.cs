using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace BenjaminMoore.Api.Retail.Pos.CustomerLoyalty.FunctionApp.Tests.Integration
{
    [ExcludeFromCodeCoverage]
    public class AzureFunctionsHost : IDisposable
    {
        private static SemaphoreSlim _PortMutex = new SemaphoreSlim(1);

        private readonly AzureFunctionsHostSettings _settings;
        private readonly HttpClient _httpClient = new HttpClient();
        private Process _funcHostProcess;
        private int _port;

        public AzureFunctionsHost(AzureFunctionsHostSettings settings)
        {
            _settings = settings;
        }

        public virtual void Dispose()
        {
            ShutDownHostProcess();
        }

        public AzureFunctionsHost Start()
        {
            ShutDownHostProcess();

            string functionHostPath = Environment.ExpandEnvironmentVariables(_settings.FunctionHostPath);
            string functionAppFolder = Path.GetFullPath(_settings.FunctionAppPath);
            int availablePort = GetAvailablePort();

            _funcHostProcess = new Process
            {
                StartInfo =
                {
                    FileName = functionHostPath,
                    Arguments = $"host start --port {availablePort}",
                    WorkingDirectory = functionAppFolder
                }
            };

            if (!_funcHostProcess.Start())
            {
                throw new InvalidOperationException("Could not start Azure Functions host.");
            }

            Task.Delay(2000); //Give process a chance to get started.

            _port = availablePort;

            return this;
        }

        public HttpClient GetClient()
        {
            _httpClient.BaseAddress = new Uri($"http://localhost:{_port}");

            return _httpClient;
        }

        private void ShutDownHostProcess()
        {
            if (!_funcHostProcess?.HasExited ?? false)
            {
                _funcHostProcess.Kill();
            }

            _funcHostProcess?.Dispose();
        }

        private int GetAvailablePort()
        {
            try
            {
                _PortMutex.Wait();

                TcpListener tcpListener = new TcpListener(IPAddress.Loopback, 0);

                tcpListener.Start();

                int availablePort = ((IPEndPoint)tcpListener.LocalEndpoint).Port;

                tcpListener.Stop();

                return availablePort;
            }
            finally
            {
                _PortMutex.Release();
            }
        }
    }
}