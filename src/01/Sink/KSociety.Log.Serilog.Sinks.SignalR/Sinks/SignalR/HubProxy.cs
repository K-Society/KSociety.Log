namespace KSociety.Log.Serilog.Sinks.SignalR.Sinks.SignalR
{
    using System;
    using System.Threading.Tasks;
    using KSociety.Log.Srv.Dto;
    using Microsoft.AspNetCore.SignalR.Client;

    public class HubProxy
    {
        public string Uri { get; set; }

        private HubConnection _connection;

        public HubProxy()
        {
            this.Uri = "http://localhost:61000/LoggingHub";
        }

        public HubProxy(string uri)
        {
            this.Uri = uri;
        }

        public async ValueTask Log(LogEvent logEvent)
        {

            this.EnsureProxyExists();

            try
            {
                await this._connection.InvokeAsync("SendLog", logEvent).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void EnsureProxyExists()
        {
            if (this._connection is null)
            {
                this.BeginNewConnection();
            }
            else if (this._connection.State == HubConnectionState.Disconnected)
            {
                this.StartExistingConnection();
            }
        }

        private void BeginNewConnection()
        {
            try
            {
                this._connection = new HubConnectionBuilder().WithUrl(this.Uri).Build();

                this._connection.StartAsync().Wait();

                this._connection.InvokeAsync("Notify", this._connection.ConnectionId);
            }
            catch (Exception)
            {
                this._connection.DisposeAsync();
            }
        }

        private void StartExistingConnection()
        {
            try
            {
                this._connection.StartAsync().Wait();
            }
            catch (Exception)
            {
                this._connection.DisposeAsync();
            }
        }
    }
}