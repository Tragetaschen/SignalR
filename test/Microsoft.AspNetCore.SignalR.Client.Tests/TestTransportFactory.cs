﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.Extensions.Logging;
using System.Net.Http;

namespace Microsoft.AspNetCore.Sockets.Client.Tests
{
    public class TestTransportFactory : ITransportFactory
    {
        private readonly ITransport _transport;

        public TestTransportFactory(ITransport transport)
        {
            _transport = transport;
        }

        public ITransport CreateTransport(TransportType availableServerTransports, ILoggerFactory loggerFactory, HttpClient httpClient)
        {
            return _transport;
        }
    }
}