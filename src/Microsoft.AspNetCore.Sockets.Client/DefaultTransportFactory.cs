// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Net.Http;
using Microsoft.Extensions.Logging;

namespace Microsoft.AspNetCore.Sockets.Client
{
    public class DefaultTransportFactory : ITransportFactory
    {
        private readonly TransportType _requestedTransportType;
        private readonly ILoggerFactory _loggerFactory;


        public DefaultTransportFactory(TransportType requestedTransportType, ILoggerFactory loggerFactory)
        {
            if (requestedTransportType <= 0 || requestedTransportType > TransportType.All)
            {
                throw new ArgumentOutOfRangeException(nameof(requestedTransportType));
            }

            _requestedTransportType = requestedTransportType;
            _loggerFactory = loggerFactory;
        }

        public ITransport CreateTransport(TransportType availableServerTransports, HttpClient httpClient)
        {
            if ((availableServerTransports & TransportType.WebSockets & _requestedTransportType) == TransportType.WebSockets)
            {
                return new WebSocketsTransport(_loggerFactory);
            }

            if ((availableServerTransports & TransportType.ServerSentEvents & _requestedTransportType) == TransportType.ServerSentEvents)
            {
                throw new NotImplementedException();
            }

            if ((availableServerTransports & TransportType.LongPolling & _requestedTransportType) == TransportType.LongPolling)
            {
                return new LongPollingTransport(httpClient, _loggerFactory);
            }

            throw new InvalidOperationException("No requested transports available on the server.");
        }
    }
}
