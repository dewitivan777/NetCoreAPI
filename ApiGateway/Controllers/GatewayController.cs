using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using ApiGateway.ApiGateway;
using ApiGateway.Extensions;
using Client;
using Client.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ApiGateway.Controllers
{
    [Route("service")]
    [ApiController]
    public class GatewayController : ControllerBase
    {
        private readonly IApiOrchestrator _apiOrchestrator;
        private readonly ILogger<ApiGatewayLog> _logger;
        private readonly IApiClient _apiClient;

        public GatewayController(IApiOrchestrator apiOrchestrator, ILogger<ApiGatewayLog> logger, IApiClient apiClient)
        {
            _apiOrchestrator = apiOrchestrator;
            _logger = logger;
            _apiClient = apiClient;
        }

        [HttpGet("{serviceName}/{*page}")]
        public async Task<IActionResult> Get(string serviceName, string page)
        {
            var parameters = Request.QueryString.Value;

            if (parameters != null)
                parameters = HttpUtility.UrlDecode(parameters);
            else
                parameters = string.Empty;

            _logger.LogApiInfo(serviceName, page, parameters);

            var apiInfo = _apiOrchestrator.GetApi(serviceName);
            var gwRouteInfo = apiInfo.Mediator.GetRoute(page.ToLower() + GatewayVerb.GET);
            var routeInfo = gwRouteInfo.Route;

            _logger.LogApiInfo($"{apiInfo.BaseUrl}{routeInfo.Path}{parameters}");

            var response = await _apiClient.GetAsync<object>(
            baseUrl: $"{apiInfo.BaseUrl}", pathWithQuery: $"{routeInfo.Path}?{parameters}");

            _logger.LogApiInfo($"{apiInfo.BaseUrl}{routeInfo.Path}{parameters}", false);

            if (response.IsError)
            {
                if (response.ResponseError == ResponseError.Http)
                {
                    if (string.IsNullOrWhiteSpace(response.Raw))
                    {
                        return StatusCode((int)response.HttpStatusCode);
                    }
                    else
                    {
                        return StatusCode((int)response.HttpStatusCode, response.Raw);
                    }
                }

                if (!string.IsNullOrWhiteSpace(response.Error))
                {
                    return StatusCode(500, response.Error);
                }

                return StatusCode(500, "unexpected response received while executing the request");
            }

            return Ok(routeInfo.ResponseType != null
                        ? JsonSerializer.Deserialize(response.Raw, routeInfo.ResponseType)
                        : response.Content);
        }

        [HttpPost]
        [Route("{serviceName}/{*page}")]
        public async Task<IActionResult> Post(string serviceName, string page, object request, string parameters = null)
        {
            if (parameters != null)
                parameters = HttpUtility.UrlDecode(parameters);
            else
                parameters = string.Empty;

            _logger.LogApiInfo(serviceName, page, parameters, request);

            var apiInfo = _apiOrchestrator.GetApi(serviceName);
            var gwRouteInfo = apiInfo.Mediator.GetRoute(page.ToLower() + GatewayVerb.POST);
            var routeInfo = gwRouteInfo.Route;

            var response = await _apiClient.CreateAsync<object, object>(
            baseUrl: $"{apiInfo.BaseUrl}", request, pathWithQuery: $"{routeInfo.Path}");

            _logger.LogApiInfo($"{apiInfo.BaseUrl}{routeInfo.Path}{parameters}", false);

            if (response.IsError)
            {
                if (response.ResponseError == ResponseError.Http)
                {
                    if (string.IsNullOrWhiteSpace(response.Raw))
                    {
                        return StatusCode((int)response.HttpStatusCode);
                    }
                    else
                    {
                        return StatusCode((int)response.HttpStatusCode, response.Raw);
                    }
                }

                if (!string.IsNullOrWhiteSpace(response.Error))
                {
                    return StatusCode(500, response.Error);
                }

                return StatusCode(500, "unexpected response received while executing the request");
            }

            return Ok(routeInfo.ResponseType != null
                        ? JsonSerializer.Deserialize(response.Raw, routeInfo.ResponseType)
                        : response.Content);

        }

        [HttpPut]
        [Route("{serviceName}/{*page}")]
        public async Task<IActionResult> Put(string serviceName, string page, object request, string parameters = null)
        {
            if (parameters != null)
                parameters = HttpUtility.UrlDecode(parameters);
            else
                parameters = string.Empty;

            _logger.LogApiInfo(serviceName, page, parameters, request);

            var apiInfo = _apiOrchestrator.GetApi(serviceName);
            var gwRouteInfo = apiInfo.Mediator.GetRoute(page.Split("/")[0].ToLower() + GatewayVerb.PUT);
            var routeInfo = gwRouteInfo.Route;

            var response = await _apiClient.EditAsync<object, object>(
            baseUrl: $"{apiInfo.BaseUrl}", request, pathWithQuery: $"{string.Format(routeInfo.Path, page.Split("/")[1] ?? "")}");

            _logger.LogApiInfo($"{apiInfo.BaseUrl}{routeInfo.Path}{parameters}", false);

            if (response.IsError)
            {
                if (response.ResponseError == ResponseError.Http)
                {
                    if (string.IsNullOrWhiteSpace(response.Raw))
                    {
                        return StatusCode((int)response.HttpStatusCode);
                    }
                    else
                    {
                        return StatusCode((int)response.HttpStatusCode, response.Raw);
                    }
                }

                if (!string.IsNullOrWhiteSpace(response.Error))
                {
                    return StatusCode(500, response.Error);
                }

                return StatusCode(500, "unexpected response received while executing the request");
            }

            return Ok(routeInfo.ResponseType != null
                        ? JsonSerializer.Deserialize(response.Raw, routeInfo.ResponseType)
                        : response.Content);
        }


        [HttpDelete]
        [Route("{serviceName}/{*page}")]
        public async Task<IActionResult> Delete(string serviceName, string page)
        {
            _logger.LogApiInfo(serviceName, page, null);

            var apiInfo = _apiOrchestrator.GetApi(serviceName);
            var gwRouteInfo = apiInfo.Mediator.GetRoute(page.Split("/")[0].ToLower() + GatewayVerb.DELETE);
            var routeInfo = gwRouteInfo.Route;

            var response = await _apiClient.DeleteAsync<string>(
            baseUrl: $"{apiInfo.BaseUrl}", pathWithQuery: $"{string.Format(routeInfo.Path, page.Split("/")[1] ?? "")}");

            _logger.LogApiInfo($"{apiInfo.BaseUrl}{routeInfo.Path}", false);

            if (response.IsError)
            {
                if (response.ResponseError == ResponseError.Http)
                {
                    if (string.IsNullOrWhiteSpace(response.Raw))
                    {
                        return StatusCode((int)response.HttpStatusCode);
                    }
                    else
                    {
                        return StatusCode((int)response.HttpStatusCode, response.Raw);
                    }
                }

                if (!string.IsNullOrWhiteSpace(response.Error))
                {
                    return StatusCode(500, response.Error);
                }

                return StatusCode(500, "unexpected response received while executing the request");
            }

            return Ok(routeInfo.ResponseType != null
                        ? JsonSerializer.Deserialize(response.Raw, routeInfo.ResponseType)
                        : response.Content);
        }

        [HttpGet]
        [Route("orchestration")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Orchestration))]
        public async Task<IActionResult> GetOrchestration(string serviceName = null, string page = null)
        {
            serviceName = serviceName?.ToLower();
            page = page?.ToLower();

            return Ok(await Task.FromResult(string.IsNullOrEmpty(serviceName) && string.IsNullOrEmpty(page)
                                            ? _apiOrchestrator.Orchestration
                                            : (!string.IsNullOrEmpty(serviceName) && string.IsNullOrEmpty(page)
                                            ? _apiOrchestrator.Orchestration?.Where(x => x.Api.Contains(serviceName.Trim()))
                                            : (string.IsNullOrEmpty(serviceName) && !string.IsNullOrEmpty(page)
                                            ? _apiOrchestrator.Orchestration?.Where(x => x.Routes.Any(y => y.Key.Contains(page.Trim())))
                                                                             .Select(x => x.FilterRoutes(page))
                                            : _apiOrchestrator.Orchestration?.Where(x => x.Api.Contains(serviceName.Trim()))
                                                                             .Select(x => x.FilterRoutes(page))))));
        }
    }
}