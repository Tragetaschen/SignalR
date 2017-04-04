﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Net.Http;
using Microsoft.Extensions.Logging;

namespace Microsoft.AspNetCore.Sockets.Client
{
    public interface ITransportFactory
    {
        ITransport CreateTransport(TransportType availableServerTransports, ILoggerFactory loggerFactory, HttpClient httpClient);
    }
}
