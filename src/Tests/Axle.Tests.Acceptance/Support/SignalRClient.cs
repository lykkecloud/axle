// Copyright (c) Lykke Corp.
// See the LICENSE file in the project root for more information.

namespace Axle.Tests.Acceptance.Support
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.SignalR.Client;

    //public sealed class SignalRClient
    //{
    //    private readonly Uri axleUri;
    //    private HubConnection connection;
    //    private bool isConnected = false;

    //    public SignalRClient(Uri axleUri)
    //    {
    //        this.axleUri = axleUri ?? throw new ArgumentNullException(nameof(axleUri));
    //        this.InitializeConnection();
    //    }

    //    public bool IsConnected => this.isConnected;

    //    public Task StartConnection() => this.connection.StartAsync();

    //    public async Task CloseConnection()
    //    {
    //        await this.connection.DisposeAsync();
    //        this.InitializeConnection();
    //    }

    //    public Task StartSession(string userId, string sessionId) => this.connection.InvokeAsync("startSession", userId, sessionId);

    //    public Task TerminateSession() => this.connection.InvokeAsync("terminateSession");

    //    public async Task Teardown()
    //    {
    //        this.connection.Closed -= this.Connection_Closed;
    //        this.connection.Connected -= this.Connection_Connected;
    //        await this.connection.DisposeAsync();
    //    }

    //    private void InitializeConnection()
    //    {
    //        this.connection = new HubConnectionBuilder()
    //                         .WithUrl(new Uri(this.axleUri, "session"))
    //                         .WithConsoleLogger()
    //                         .Build();

    //        this.connection.Connected += this.Connection_Connected;
    //        this.connection.Closed += this.Connection_Closed;
    //    }

    //    private Task Connection_Closed(Exception arg)
    //    {
    //        this.isConnected = false;
    //        return Task.CompletedTask;
    //    }

    //    private Task Connection_Connected()
    //    {
    //        this.isConnected = true;
    //        return Task.CompletedTask;
    //    }
    //}
}
