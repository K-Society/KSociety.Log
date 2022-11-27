using System;
using System.Threading.Tasks;
using KSociety.Log.Srv.Dto;
using Microsoft.AspNetCore.SignalR.Client;

namespace KSociety.Log.Serilog.Sinks.SignalR.Sinks.SignalR
{
    public class HubProxy
    {
        public string Uri { get; set; }

        private HubConnection _connection;

        public HubProxy()
        {
            Uri = "http://localhost:61000/LoggingHub";
        }

        public HubProxy(string uri)
        {
            Uri = uri;
        }

        public async ValueTask Log(LogEvent logEvent)
        {

            EnsureProxyExists();

            try
            {
                await _connection.InvokeAsync("SendLog", logEvent).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void EnsureProxyExists()
        {
            if (_connection is null)
            {
                BeginNewConnection();
            }
            else if (_connection.State == HubConnectionState.Disconnected)
            {
                StartExistingConnection();
            }
        }

        private void BeginNewConnection()
        {
            try
            {
                _connection = new HubConnectionBuilder().WithUrl(Uri).Build();

                _connection.StartAsync().Wait();

                _connection.InvokeAsync("Notify", _connection.ConnectionId);
            }
            catch (Exception)
            {
                _connection.DisposeAsync();
            }
        }

        private void StartExistingConnection()
        {
            try
            {
                _connection.StartAsync().Wait();
            }
            catch (Exception)
            {
                _connection.DisposeAsync();
            }
        }
    }
}