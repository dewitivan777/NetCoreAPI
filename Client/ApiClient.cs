using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Client.Models;
using System.Net.Http;
using System.Text;
using System.Collections.Generic;

namespace Client
{
    public class ApiClient : ApiClientBase, IApiClient
    {
        public ApiClient(
            HttpClient httpClient,
            IHttpContextAccessor contextAccessor) : base(httpClient, contextAccessor)
        {
        }

        public async Task<IApiResponse<T>> CreateAsync<T, TContent>(
            string baseUrl,
            TContent content,
            string pathWithQuery,
            string token = null,
            AuditInfo audit = null)
            where T : class
            where TContent : class
        {
            if (content is string) return new ApiResponse<T>(new ArgumentException("Content should not be a string"));

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, pathWithQuery)
            {
                Content = new StringContent(
                    content: content.ToJsonString(),
                    encoding: Encoding.UTF8,
                    mediaType: "application/json")
            };

            return await SendAsync<T>(baseUrl, httpRequestMessage, token, audit: audit);
        }

        public async Task<IApiResponse<T>> DeleteAsync<T>(
           string baseUrl,
            string pathWithQuery,
            string token = null,
            AuditInfo audit = null)
            where T : class
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, pathWithQuery);

            return await SendAsync<T>(baseUrl, httpRequestMessage, token, audit: audit);
        }

        public async Task<IApiResponse<T>> EditAsync<T, TContent>(
            string baseUrl,
            TContent content,
            string pathWithQuery,
            string token = null,
            AuditInfo audit = null)
            where T : class
            where TContent : class
        {
            if (content is string) return new ApiResponse<T>(new ArgumentException("content should not be a string"));

            var stringContent = content.ToJsonString();

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Put, pathWithQuery)
            {
                Content = new StringContent(content: stringContent, encoding: Encoding.UTF8, mediaType: "application/json")
            };

            return await SendAsync<T>(baseUrl, httpRequestMessage, token, audit: audit);
        }

        public async Task<IApiResponse<T>> GetAsync<T>(
            string baseUrl,
            string pathWithQuery,
            string token = null,
            string userClaim = null)
            where T : class
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, pathWithQuery);

            return await SendAsync<T>(baseUrl, httpRequestMessage, token, userClaim: userClaim);
        }

        public async Task<IApiResponse<T>> GetByQueryAsync<T>(
            string baseUrl,
            string pathWithQuery,
            string token = null,
            params KeyValuePair<string, string>[] queryValues)
            where T : class
        {
            using (var content = new FormUrlEncodedContent(queryValues))
            {
                var query = await content.ReadAsStringAsync();

                var requestWithQuery = string.Concat(pathWithQuery, "?", query);

                var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, requestWithQuery);

                return await SendAsync<T>(baseUrl, httpRequestMessage, token);
            }
        }
    }
}