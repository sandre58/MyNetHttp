// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;

namespace MyNet.Http
{
    public class WebApiException : Exception
    {
        public object? Exception { get; }

        public WebApiException(object? exception) : base(exception?.ToString()) => Exception = exception;

        protected WebApiException(string message) : base(message) { }

        protected WebApiException(string message, Exception? exception) : base(message, exception) { }
    }
}
