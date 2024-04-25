// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using System.Net.Http;

namespace MyNet.Http
{
    public static class HttpRequestExtensions
    {
        private const string TimeoutPropertyKey = "RequestTimeout";

        public static void SetTimeout(this HttpRequestMessage request, TimeSpan? timeout)
        {
            ArgumentNullException.ThrowIfNull(request);

            request.Options.Set(new HttpRequestOptionsKey<TimeSpan?>(TimeoutPropertyKey), timeout);
        }

        public static TimeSpan? GetTimeout(this HttpRequestMessage request)
        {
            ArgumentNullException.ThrowIfNull(request);

            return request.Options.TryGetValue(new HttpRequestOptionsKey<TimeSpan?>(TimeoutPropertyKey), out var value) && value is TimeSpan timeout ? (TimeSpan?)timeout : null;
        }
    }
}
