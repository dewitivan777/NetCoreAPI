using Client.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Client
{
    public abstract class ApiClientBase
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _contextAccessor;

        public ApiClientBase(
            HttpClient httpClient,
            IHttpContextAccessor contextAccessor)
        {
            _httpClient = httpClient;
            _contextAccessor = contextAccessor;
        }

        protected async Task<IApiResponse<T>> SendAsync<T>(
            string path,
            HttpRequestMessage httpRequestMessage,
            string token,
            CancellationToken cancellationToken = default,
            AuditInfo audit = null,
            string userClaim = null) where T : class
        {
            HttpResponseMessage httpResponseMessage;
            try
            {
                httpRequestMessage.RequestUri = new Uri(path + httpRequestMessage.RequestUri.ToString());
      
                httpResponseMessage = await _httpClient.SendAsync(httpRequestMessage, cancellationToken);
            }
            catch (Exception ex)
            {
                return new ApiResponse<T>(ex);
            }

            string raw = null;
            if (httpResponseMessage.Content != null)
            {
                raw = await httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
            }

            return new ApiResponse<T>(httpResponseMessage.StatusCode, raw, httpResponseMessage.ReasonPhrase);
        }
    }
}