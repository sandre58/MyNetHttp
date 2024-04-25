// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using MyNet.Utilities.Helpers;
using Newtonsoft.Json.Linq;

namespace MyNet.Http
{
    /// <summary>
    /// Manage Exception for Business Layers.
    /// </summary>
    [Serializable]
    public sealed class ExceptionResult : ISerializable
    {
        public Exception Exception { get; private set; } = null!;

        public ExceptionResult() { }

        public ExceptionResult(Exception exception) => Exception = exception;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S3267:Loops should be simplified with \"LINQ\" expressions", Justification = "Impossible on JsonObject")]
        [System.Security.SecuritySafeCritical]  // auto-generated
        private ExceptionResult(SerializationInfo info, StreamingContext context)
        {
            try
            {
                var exception = info.GetValue(nameof(Exception), typeof(Exception));

                var assemblyStr = (string?)info.GetValue("assembly", typeof(string))!;
                var assembly = TypeHelper.GetAssemblyByName(assemblyStr);

                if (assembly is not null)
                {
                    var typeStr = (string?)info.GetValue("type", typeof(string))!;
                    var type = assembly.GetType(typeStr)!;
                    Exception = (Exception)Convert.ChangeType(exception, type)!;
                }
                else
                {
                    var e = (Exception)exception!;
                    Exception = new HttpException(e.Message, e.InnerException);
                }

            }
            catch (Exception)
            {
                var jsonErrors = (JObject?)info.GetValue("errors", typeof(JObject));
                var title = (JValue?)info.GetValue("title", typeof(JValue));

                if (jsonErrors != null)
                {
                    var errors = new List<HttpError>();
                    foreach (var item in jsonErrors)
                    {
                        if (item.Value?.ToString() is not null)
                        {
                            var str = item.Value.ToString();
                            switch (item.Value)
                            {
                                case JArray array:
                                    str = array.First?.ToString();
                                    break;
                            }

                            errors.Add(new HttpError(str ?? string.Empty));
                        }
                    }
                    Exception = new MultipleHttpException(title?.Value?.ToString() ?? string.Empty, errors);
                }
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("assembly", Exception.GetType().Assembly.FullName, typeof(string));
            info.AddValue("type", Exception.GetType().Name, typeof(string));
            info.AddValue(nameof(Exception), Exception, Exception.GetType());
        }
    }
}
