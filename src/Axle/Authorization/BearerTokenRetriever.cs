// Copyright (c) Lykke Corp.
// See the LICENSE file in the project root for more information.

namespace Axle.Authorization
{
    using System;
    using IdentityModel.AspNetCore.OAuth2Introspection;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Primitives;

    /// <summary>
    /// Get the access token jwt or reference
    /// either from
    ///  authorization header
    ///  access_token query param
    /// This is to support web socket connection, because javascript client cannot pass header for web socket
    /// </summary>
    public static class BearerTokenRetriever
    {
        internal const string TokenItemsKey = "idsrv4:tokenvalidation:token";

        // custom token key change it to the one you use for sending the access_token to the server
        // during websocket handshake
        internal const string SignalRTokenKey = "access_token";

        static BearerTokenRetriever()
        {
            AuthHeaderTokenRetriever = TokenRetrieval.FromAuthorizationHeader();
            QueryStringTokenRetriever = TokenRetrieval.FromQueryString();
        }

        internal static Func<HttpRequest, string> AuthHeaderTokenRetriever { get; set; }

        internal static Func<HttpRequest, string> QueryStringTokenRetriever { get; set; }

        public static string FromHeaderAndQueryString(HttpRequest request)
        {
            var token = AuthHeaderTokenRetriever(request);

            if (string.IsNullOrEmpty(token))
            {
                token = QueryStringTokenRetriever(request);
            }

            if (string.IsNullOrEmpty(token))
            {
                token = request.HttpContext.Items[TokenItemsKey] as string;
            }

            if (string.IsNullOrEmpty(token) && request.Query.TryGetValue(SignalRTokenKey, out StringValues extract))
            {
                token = extract.ToString();
            }

            return token;
        }
    }
}
