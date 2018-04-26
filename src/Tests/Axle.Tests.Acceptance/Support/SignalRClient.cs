namespace Axle.Tests.Acceptance.Support
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.SignalR.Client;

    public sealed class SignalRClient
    {
        private readonly Uri axleUri;
        private HubConnection connection;

        public SignalRClient(Uri axleUri)
        {
            this.axleUri = axleUri ?? throw new ArgumentNullException(nameof(axleUri));
            this.InitializeConnection();
        }

        public Task StartConnection() => this.connection.StartAsync();

        public async Task CloseConnection()
        {
            await this.connection.DisposeAsync();
            this.InitializeConnection();
        }

        public Task StartSession(string userId) => this.connection.InvokeAsync("startSession", userId);

        public Task TerminateSession() => this.connection.InvokeAsync("terminateSession");

        private void InitializeConnection()
        {
            this.connection = new HubConnectionBuilder()
                             .WithUrl(new Uri(this.axleUri, "session"))
                             .WithConsoleLogger()
                             .Build();
        }
    }
}
