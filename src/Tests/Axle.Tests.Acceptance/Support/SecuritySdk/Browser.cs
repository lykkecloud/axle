// Copyright (c) Lykke Corp.
// See the LICENSE file in the project root for more information.

#pragma warning disable CA1724

namespace Axle.Tests.Acceptance.Support.SecuritySdk
{
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading.Tasks;
    using IdentityModel.OidcClient.Browser;

    public class Browser : IBrowser
    {
        private readonly BrowserAutomation automation;
        private readonly string path;

        public Browser(BrowserAutomation automation, int? port = null, string path = null)
        {
            this.automation = automation;
            this.path = path;
            this.Port = port ?? GetRandomUnusedPort();
        }

        public int Port { get; }

        public async Task<BrowserResult> InvokeAsync(BrowserOptions options)
        {
            using (var listener = new LoopbackHttpListener(this.Port, this.path))
            {
                await this.automation.NavigateToLoginAsync(options.StartUrl).ConfigureAwait(false);
                await this.automation.LoginToAuthorizationServerAsync().ConfigureAwait(false);

                try
                {
                    var result = await listener.WaitForCallbackAsync(5).ConfigureAwait(false);
                    if (string.IsNullOrWhiteSpace(result))
                    {
                        return new BrowserResult { ResultType = BrowserResultType.UnknownError, Error = "Empty response." };
                    }

                    return new BrowserResult { Response = result, ResultType = BrowserResultType.Success };
                }
                catch (TaskCanceledException ex)
                {
                    return new BrowserResult { ResultType = BrowserResultType.Timeout, Error = ex.Message };
                }
                catch (Exception ex)
                {
                    return new BrowserResult { ResultType = BrowserResultType.UnknownError, Error = ex.Message };
                }
            }
        }

        private static int GetRandomUnusedPort()
        {
            var listener = new TcpListener(IPAddress.Loopback, 0);
            listener.Start();
            var port = ((IPEndPoint)listener.LocalEndpoint).Port;
            listener.Stop();
            return port;
        }
    }
}