// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Net.Http;
using Microsoft.AspNetCore.Sockets;
using Microsoft.AspNetCore.Sockets.Client;
using Xunit;

namespace Microsoft.AspNetCore.SignalR.Tests
{
    public class DefaultTransportFactoryTests
    {
        [Theory]
        [InlineData(0)]
        [InlineData(TransportType.All + 1)]
        public void DefaultTransportFactoryCannotBeCreatedWithInvalidTransportType(TransportType transportType)
        {
            Assert.Throws<ArgumentOutOfRangeException>(
                () => new DefaultTransportFactory(transportType));
        }

        [Theory]
        [InlineData(TransportType.WebSockets, typeof(WebSocketsTransport))]
        [InlineData(TransportType.LongPolling, typeof(LongPollingTransport))]
        public void DefaultTransportFactoryCreatesRequestedTransportIfAvailable(TransportType requestedTransport, Type expectedTransportType)
        {
            var transportFactory = new DefaultTransportFactory(requestedTransport);
            Assert.IsType(expectedTransportType,
                transportFactory.CreateTransport(TransportType.All, loggerFactory: null, httpClient: null));
        }

        [Theory]
        [InlineData(TransportType.WebSockets)]
        [InlineData(TransportType.ServerSentEvents)]
        [InlineData(TransportType.LongPolling)]
        [InlineData(TransportType.All)]
        public void DefaultTransportFactoryThrowsIfItCannotCreateRequestedTransport(TransportType requestedTransport)
        {
            var transportFactory = new DefaultTransportFactory(requestedTransport);
            var ex = Assert.Throws<InvalidOperationException>(
                () => transportFactory.CreateTransport(~requestedTransport, loggerFactory: null, httpClient: null));

            Assert.Equal("No requested transports available on the server.", ex.Message);
        }

        [Fact]
        public void DefaultTransportFactoryCreatesWebSocketsTransportIfAvailable()
        {
            Assert.IsType<WebSocketsTransport>(
                new DefaultTransportFactory(TransportType.All)
                    .CreateTransport(TransportType.All, loggerFactory: null, httpClient: null));
        }
    }
}
