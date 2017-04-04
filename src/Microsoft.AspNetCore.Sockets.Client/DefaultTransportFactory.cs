// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;

namespace Microsoft.AspNetCore.Sockets.Client
{
    public class DefaultTransportFactory : ITransportFactory
    {
        private readonly TransportType _requestedTransportType;

        public DefaultTransportFactory(TransportType requestedTransportType)
        {
            if (requestedTransportType <= 0 || requestedTransportType > TransportType.All)
            {
                throw new ArgumentOutOfRangeException(nameof(requestedTransportType));
            }

            _requestedTransportType = requestedTransportType;
        }

        public ITransport CreateTransport(TransportType availableServerTransports, ILoggerFactory loggerFactory, HttpClient httpClient)
        {
            if ((availableServerTransports & TransportType.WebSockets & _requestedTransportType) == TransportType.WebSockets)
            {
                return new WebSocketsTransport(loggerFactory);
            }

            if ((availableServerTransports & TransportType.ServerSentEvents & _requestedTransportType) == TransportType.ServerSentEvents)
            {
                throw new NotImplementedException();
            }

            if ((availableServerTransports & TransportType.LongPolling & _requestedTransportType) == TransportType.LongPolling)
            {
                return new LongPollingTransport(httpClient, loggerFactory);
            }

            throw new InvalidOperationException("No requested transports available on the server.");
        }
    }
}
