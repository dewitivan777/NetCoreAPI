﻿using ApiGateway.ApiGateway;
using ApiGateway.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ApiGateway
{
    public static class ApiExtensions
    {

        public static void AddApiGateway(this IServiceCollection services)
        {
            var apis = new ApiOrchestrator();

            services.AddTransient<IApiOrchestrator>(x => apis);
            services.AddHttpClient<IHttpService, HttpService>();

            services.AddControllers();
        }

        public static void UseApiGateway(this IApplicationBuilder app, Action<IApiOrchestrator> setApis)
        {
            var serviceProvider = app.ApplicationServices;
            setApis(serviceProvider.GetService<IApiOrchestrator>());
            app.UseMiddleware<GatewayMiddleware>();
        }

        internal static void AddRequestHeaders(this IHeaderDictionary requestHeaders, HttpRequestHeaders headers)
        {
            foreach (var item in requestHeaders)
            {
                try
                {
                    if (!headers.Contains(item.Key))
                        headers.Add(item.Key, item.Value.ToString());
                }
                catch (Exception)
                { }
            }
        }

        internal static Orchestration FilterRoutes(this Orchestration orchestration, string key)
        {
            orchestration.Routes = orchestration.Routes.Where(y => y.Key.Contains(key.Trim()));
            return orchestration;
        }

        internal static string ToUtcLongDateTime(this DateTime dateTime)
        {
            return dateTime.ToString("MM/dd/yyyy hh:mm:ss.fff tt");
        }

        internal static void LogApiInfo(this ILogger<ApiGatewayLog> logger, string api, string key, string parameters, object request = null)
        {
            if (request != null)
                logger.LogInformation($"ApiGateway: Incoming POST request. api: {api}, key: {key}, object: {JsonConvert.SerializeObject(request)}, parameters: {parameters}, UtcTime: { DateTime.UtcNow.ToUtcLongDateTime() }");
            else
                logger.LogInformation($"ApiGateway: Incoming POST request. api: {api}, key: {key}, UtcTime: { DateTime.UtcNow.ToUtcLongDateTime() }");
        }

        internal static void LogApiInfo(this ILogger<ApiGatewayLog> logger, string url, bool beforeBackendCall = true)
        {
            if (beforeBackendCall)
                logger.LogInformation($"ApiGateway: Calling back end. Url: {url}, UtcTime: { DateTime.UtcNow.ToUtcLongDateTime() }");
            else
                logger.LogInformation($"ApiGateway: Finished calling back end. Url: {url}, UtcTime: { DateTime.UtcNow.ToUtcLongDateTime() }");
        }
    }
}
