// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;

namespace MyNet.Http
{
    public class HttpException : Exception
    {
        public HttpException() { }

        public HttpException(string message)
            : base(message) { }

        public HttpException(string message, Exception? exception)
            : base(message, exception) { }
    }
}
