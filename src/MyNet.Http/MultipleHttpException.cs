// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNet.Http
{
    public class HttpError
    {
        public string Message { get; }

        public string? Code { get; }

        public HttpError(string code, string message) => (Code, Message) = (code, message);

        public HttpError(string message) => Message = message;

        public override string ToString() => Message;
    }

    public class MultipleHttpException : Exception
    {
        private readonly List<HttpError> _errors = [];

        public IReadOnlyCollection<HttpError> Errors => _errors.AsReadOnly();

        public MultipleHttpException() { }

        public MultipleHttpException(IEnumerable<string> errors) : this(string.Empty, errors) { }

        public MultipleHttpException(IEnumerable<HttpError> errors) : this(string.Empty, errors) { }

        public MultipleHttpException(string message, IEnumerable<HttpError> errors) : base(message) => _errors.AddRange(errors);

        public MultipleHttpException(string message, IEnumerable<string> errors) : base(message) => _errors.AddRange(errors.Select(x => new HttpError(x)));

        public MultipleHttpException(string message, Exception exception) : base(message, exception) { }

        public override string ToString()
        {
            var errorMessage = new StringBuilder();

            if (!string.IsNullOrEmpty(Message))
            {
                _ = errorMessage.AppendLine(Message);
            }

            foreach (var error in Errors)
            {
                _ = errorMessage.AppendLine($" - {error.Message}");
            }

            return errorMessage.ToString();
        }
    }
}
