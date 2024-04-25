// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using MyNet.Utilities;

namespace MyNet.Http
{
    public sealed class WebApiService : IDisposable
    {
        private readonly HttpClient _client;
        private readonly TimeSpan _timeout;
        private readonly Func<object?, Exception>? _toException;

        /// <summary>
        /// Initalise a new instance of <see cref="WebApiService"/>.
        /// </summary>
        /// <param name="serverUrl"></param>
        /// <param name="timeout"></param>
        /// <param name="headers"></param>
        /// <param name="toException"></param>
        public WebApiService(Uri? serverUrl = null, TimeSpan timeout = default, Dictionary<string, string>? headers = null, Func<object?, Exception>? toException = null)
        {
            _toException = toException;
            _timeout = timeout != default ? timeout : TimeSpan.FromMilliseconds(Timeout.Infinite);
            var handler = new TimeoutHandler
            {
                InnerHandler = new HttpClientHandler() { AutomaticDecompression = System.Net.DecompressionMethods.GZip }
            };

            _client = new HttpClient(handler)
            {
                BaseAddress = serverUrl,
                Timeout = _timeout
            };
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/problem+json"));
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));
            _client.DefaultRequestHeaders.AcceptEncoding.Add(StringWithQualityHeaderValue.Parse("gzip"));

            headers?.ForEach(x => _client.DefaultRequestHeaders.Add(x.Key, x.Value));
        }

        public async Task<Stream> GetStreamAsync(string str) => await _client.GetStreamAsync(str).ConfigureAwait(false);

        public Stream GetStream(string str) => _client.GetStreamAsync(str).GetAwaiter().GetResult();

        /// <summary>
        /// Get Data from web service.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="str"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<T> GetDataAsync<T>(string str, CancellationToken cancellationToken, params (string key, string value)[] parameters) => await GetDataAsync<T>(str.ToWebUri(parameters), cancellationToken).ConfigureAwait(false);

        /// <summary>
        /// Get Data from web service.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uri"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<T> GetDataAsync<T>(Uri uri, CancellationToken cancellationToken = default) => await SendRequestAsync<T>(HttpMethod.Get, uri, cancellationToken: cancellationToken).ConfigureAwait(false);

        /// <summary>
        /// Get Data from web service.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="value"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<TReturn> PostDataAsync<TParam, TReturn>(string str, TParam value, CancellationToken cancellationToken, params (string key, string value)[] parameters) => await PostDataAsync<TParam, TReturn>(str.ToWebUri(parameters), value, cancellationToken).ConfigureAwait(false);

        /// <summary>
        /// Post Data from web service.
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="value"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<T> PostDataAsync<TParam, T>(Uri uri, TParam value, CancellationToken cancellationToken = default) => await SendRequestAsync<T>(HttpMethod.Post, uri, CreateContent(value), cancellationToken).ConfigureAwait(false);

        /// <summary>
        /// Get Data from web service.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task PostDataAsync(string str, CancellationToken cancellationToken, params (string key, string value)[] parameters) => await PostDataAsync(str.ToWebUri(parameters), cancellationToken).ConfigureAwait(false);

        /// <summary>
        /// Delete Data from web service.
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task PostDataAsync(Uri uri, CancellationToken cancellationToken = default) => await SendRequestAsync(HttpMethod.Post, uri, cancellationToken: cancellationToken).ConfigureAwait(false);

        /// <summary>
        /// Get Data from web service.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task DeleteDataAsync(string str, CancellationToken cancellationToken, params (string key, string value)[] parameters) => await DeleteDataAsync(str.ToWebUri(parameters), cancellationToken: cancellationToken).ConfigureAwait(false);

        /// <summary>
        /// Delete Data from web service.
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task DeleteDataAsync(Uri uri, CancellationToken cancellationToken = default) => await SendRequestAsync(HttpMethod.Delete, uri, cancellationToken: cancellationToken).ConfigureAwait(false);

        /// <summary>
        /// Get Data from web service.
        /// </summary>
        /// <returns></returns>
        public async Task<TReturn> DeleteDataAsync<TParam, TReturn>(string str, TParam value, CancellationToken cancellationToken, params (string key, string value)[] parameters) => await DeleteDataAsync<TParam, TReturn>(str.ToWebUri(parameters), value, cancellationToken).ConfigureAwait(false);

        /// <summary>
        /// Delete Data from web service.
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="value"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<T> DeleteDataAsync<TParam, T>(Uri uri, TParam value, CancellationToken cancellationToken = default) => await SendRequestAsync<T>(HttpMethod.Delete, uri, CreateContent(value), cancellationToken).ConfigureAwait(false);

        /// <summary>
        /// Get Data from web service.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="str"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public T GetData<T>(string str, params (string key, string value)[] parameters) => GetData<T>(str.ToWebUri(parameters));

        /// <summary>
        /// Get Data from web service.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uri"></param>
        /// <returns></returns>
        public T GetData<T>(Uri uri) => GetDataAsync<T>(uri).GetAwaiter().GetResult();

        /// <summary>
        /// Get Data from web service.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="value"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public TReturn PostData<TParam, TReturn>(string str, TParam value, params (string key, string value)[] parameters) => PostData<TParam, TReturn>(str.ToWebUri(parameters), value);

        /// <summary>
        /// Get Data from web service.
        /// </summary>
        /// <typeparam name="TParam"></typeparam>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="uri"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public TReturn PostData<TParam, TReturn>(Uri uri, TParam value) => PostDataAsync<TParam, TReturn>(uri, value).GetAwaiter().GetResult();

        /// <summary>
        /// Get Data from web service.
        /// </summary>
        /// <typeparam name="TParam"></typeparam>
        /// <param name="str"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public void PostData<TParam>(string str, TParam value) => PostDataAsync<TParam, bool>(str.ToWebUri(), value).GetAwaiter().GetResult();

        /// <summary>
        /// Get Data from web service.
        /// </summary>
        /// <typeparam name="TParam"></typeparam>
        /// <param name="uri"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public void PostData<TParam>(Uri uri, TParam value) => PostDataAsync<TParam, bool>(uri, value).GetAwaiter().GetResult();

        /// <summary>
        /// Get Data from web service.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public void PostData(string str, params (string key, string value)[] parameters) => PostData(str.ToWebUri(parameters));

        /// <summary>
        /// Get Data from web service.
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public void PostData(Uri uri) => PostDataAsync(uri).GetAwaiter().GetResult();

        /// <summary>
        /// Get Data from web service.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public void DeleteData(string str, params (string key, string value)[] parameters) => DeleteData(str.ToWebUri(parameters));

        /// <summary>
        /// Get Data from web service.
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public void DeleteData(Uri uri) => DeleteDataAsync(uri).GetAwaiter().GetResult();

        /// <summary>
        /// Get Data from web service.
        /// </summary>
        /// <typeparam name="TParam"></typeparam>
        /// <param name="str"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public void DeleteDataWithParam<TParam>(string str, TParam value) => DeleteDataAsync<TParam, bool>(str.ToWebUri(), value).GetAwaiter().GetResult();

        /// <summary>
        /// Get Data from web service.
        /// </summary>
        /// <typeparam name="TParam"></typeparam>
        /// <param name="uri"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public void DeleteDataWithParam<TParam>(Uri uri, TParam value) => DeleteDataAsync<TParam, bool>(uri, value).GetAwaiter().GetResult();

        public void Dispose() => _client.Dispose();

        public async Task SendRequestAsync(HttpMethod method, Uri uri, HttpContent? content = null, CancellationToken cancellationToken = default)
        {
            using var tokenSource = new CancellationTokenSource();
            using var linkedCancellationToken = CancellationTokenSource.CreateLinkedTokenSource(tokenSource.Token, cancellationToken);
            tokenSource.CancelAfter(_timeout);

            using var request = new HttpRequestMessage(method, uri) { Content = content };
            request.Headers.AcceptLanguage.Add(new StringWithQualityHeaderValue(CultureInfo.CurrentCulture.Name));
            using var response = await _client.SendAsync(request, HttpCompletionOption.ResponseContentRead, linkedCancellationToken.Token).ConfigureAwait(false);

            if (response.IsSuccessStatusCode) return;

            throw await GetExceptionAsync(response).ConfigureAwait(false);
        }

        public async Task<T> SendRequestAsync<T>(HttpMethod method, Uri uri, HttpContent? content = null, CancellationToken cancellationToken = default)
        {
            using var tokenSource = new CancellationTokenSource();
            using var linkedCancellationToken = CancellationTokenSource.CreateLinkedTokenSource(tokenSource.Token, cancellationToken);
            tokenSource.CancelAfter(_timeout);

            using var request = new HttpRequestMessage(method, uri)
            {
                Content = content
            };

            request.Headers.AcceptLanguage.Add(new StringWithQualityHeaderValue(CultureInfo.CurrentCulture.Name));
            using var response = await _client.SendAsync(request, HttpCompletionOption.ResponseContentRead, linkedCancellationToken.Token).ConfigureAwait(false);
            return response.IsSuccessStatusCode
                ? await response.Content.ReadAsAsync<T>().ConfigureAwait(false)
                : throw await GetExceptionAsync(response).ConfigureAwait(false);
        }

        private async Task<Exception> GetExceptionAsync(HttpResponseMessage response)
        {
            var result = await response.Content.ReadAsAsync<object>([new JsonProblemMediaTypeFormatter()]).ConfigureAwait(false);
            return _toException?.Invoke(result) ?? new WebApiException(result);
        }

        private static ObjectContent<TParam> CreateContent<TParam>(TParam value) => new(value, new JsonMediaTypeFormatter());
    }

    internal class JsonProblemMediaTypeFormatter : JsonMediaTypeFormatter
    {
        public JsonProblemMediaTypeFormatter()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/problem+json"));
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
        }
    }

    internal class TimeoutHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {
                return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
            }
            catch (OperationCanceledException) when (!cancellationToken.IsCancellationRequested)
            {
                throw new TimeoutException();
            }
        }
    }
}
