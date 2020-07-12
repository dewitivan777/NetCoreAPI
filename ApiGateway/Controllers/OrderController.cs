using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using ApiGateway.ApiGateway;
using ApiGateway.Extensions;
using Client;
using Client.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Services.Domain.OrderService.Models;
using Services.Domain.ProductService.Models;

namespace ApiGateway.Controllers
{
    [ApiController]
    [Route("service")]
    public class OrderController : ControllerBase
    {
        private readonly IApiOrchestrator _apiOrchestrator;
        private readonly ILogger<ApiGatewayLog> _logger;
        private readonly IApiClient _apiClient;

        public OrderController(IApiOrchestrator apiOrchestrator, ILogger<ApiGatewayLog> logger, IApiClient apiClient)
        {
            _apiOrchestrator = apiOrchestrator;
            _logger = logger;
            _apiClient = apiClient;
        }

        [HttpPost]
        [Route("OrderService/order")]
        public async Task<IActionResult> Post(OrderEntity request)
        {
            //Get Product
            var apiInfo = _apiOrchestrator.GetApi("ProductService");
            var gwRouteInfo = apiInfo.Mediator.GetRoute("product" + GatewayVerb.GET);
            var routeInfo = gwRouteInfo.Route;

            _logger.LogApiInfo($"{apiInfo.BaseUrl}{routeInfo.Path}");

            var productResponse = await _apiClient.GetAsync<ProductEntity>(
            baseUrl: $"{apiInfo.BaseUrl}", pathWithQuery: $"{routeInfo.Path}/{request.ProductId}");

            _logger.LogApiInfo($"{apiInfo.BaseUrl}{routeInfo.Path}/{request.ProductId}", false);

            if (productResponse.IsError)
            {
                if (productResponse.ResponseError == ResponseError.Http)
                {
                    if (string.IsNullOrWhiteSpace(productResponse.Raw))
                    {
                        return StatusCode((int)productResponse.HttpStatusCode);
                    }
                    else
                    {
                        return StatusCode((int)productResponse.HttpStatusCode, productResponse.Raw);
                    }
                }

                if (!string.IsNullOrWhiteSpace(productResponse.Error))
                {
                    return StatusCode(500, productResponse.Error);
                }

                return StatusCode(500, "unexpected response received while executing the request");
            }

            //Check stock
            if (request.Quantity > productResponse.Content.UnitsInStock)
            {
                ModelState.AddModelError("Quantity", "Not Enough Units Available");

                return BadRequest(ModelState);
            }

            //Create Order
            apiInfo = _apiOrchestrator.GetApi("OrderService");
            gwRouteInfo = apiInfo.Mediator.GetRoute("order" + GatewayVerb.POST);
            routeInfo = gwRouteInfo.Route;

            _logger.LogApiInfo("OrderService", "order", null, request);

            var orderResponse = await _apiClient.CreateAsync<OrderEntity, OrderEntity>(
            baseUrl: $"{apiInfo.BaseUrl}", request, pathWithQuery: $"{routeInfo.Path}");

            _logger.LogApiInfo($"{apiInfo.BaseUrl}{routeInfo.Path}", false);

            if (orderResponse.IsError)
            {
                if (orderResponse.ResponseError == ResponseError.Http)
                {
                    if (string.IsNullOrWhiteSpace(orderResponse.Raw))
                    {
                        return StatusCode((int)orderResponse.HttpStatusCode);
                    }
                    else
                    {
                        return StatusCode((int)orderResponse.HttpStatusCode, orderResponse.Raw);
                    }
                }

                if (!string.IsNullOrWhiteSpace(orderResponse.Error))
                {
                    return StatusCode(500, orderResponse.Error);
                }

                return StatusCode(500, "unexpected response received while executing the request");
            }

            //Update Product
            apiInfo = _apiOrchestrator.GetApi("ProductService");
            gwRouteInfo = apiInfo.Mediator.GetRoute("product" + GatewayVerb.PUT);
            routeInfo = gwRouteInfo.Route;

            _logger.LogApiInfo($"{apiInfo.BaseUrl}{routeInfo.Path}");

            productResponse.Content.UnitsInStock = productResponse.Content.UnitsInStock - orderResponse.Content.Quantity;

            var response = await _apiClient.EditAsync<ProductEntity, ProductEntity>(
               baseUrl: $"{apiInfo.BaseUrl}", productResponse.Content, pathWithQuery: $"{string.Format(routeInfo.Path, productResponse.Content.Id)}");

            _logger.LogApiInfo($"{apiInfo.BaseUrl}{routeInfo.Path}/{request.ProductId}", false);

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

            return Ok(orderResponse.Content);
        }

        [HttpPut]
        [Route("OrderService/order/{id}")]
        public async Task<IActionResult> Put(OrderEntity request, string id)
        {
         //Get Product
           var apiInfo = _apiOrchestrator.GetApi("ProductService");
           var gwRouteInfo = apiInfo.Mediator.GetRoute("product" + GatewayVerb.GET);
           var routeInfo = gwRouteInfo.Route;

            _logger.LogApiInfo($"{apiInfo.BaseUrl}{routeInfo.Path}");

            var productResponse = await _apiClient.GetAsync<ProductEntity>(
            baseUrl: $"{apiInfo.BaseUrl}", pathWithQuery: $"{routeInfo.Path}/{request.ProductId}");

            _logger.LogApiInfo($"{apiInfo.BaseUrl}{routeInfo.Path}/{request.ProductId}", false);

            if (productResponse.IsError)
            {
                if (productResponse.ResponseError == ResponseError.Http)
                {
                    if (string.IsNullOrWhiteSpace(productResponse.Raw))
                    {
                        return StatusCode((int)productResponse.HttpStatusCode);
                    }
                    else
                    {
                        return StatusCode((int)productResponse.HttpStatusCode, productResponse.Raw);
                    }
                }

                if (!string.IsNullOrWhiteSpace(productResponse.Error))
                {
                    return StatusCode(500, productResponse.Error);
                }

                return StatusCode(500, "unexpected response received while executing the request");
            }

            //Get Current Order
            apiInfo = _apiOrchestrator.GetApi("OrderService");
            gwRouteInfo = apiInfo.Mediator.GetRoute("order" + GatewayVerb.GET);
            routeInfo = gwRouteInfo.Route;

            _logger.LogApiInfo($"{apiInfo.BaseUrl}{routeInfo.Path}");

            var orderResponse = await _apiClient.GetAsync<OrderEntity>(
            baseUrl: $"{apiInfo.BaseUrl}", pathWithQuery: $"{routeInfo.Path}/{id}");

            _logger.LogApiInfo($"{apiInfo.BaseUrl}{routeInfo.Path}/{request.ProductId}", false);

            if (orderResponse.IsError)
            {
                if (orderResponse.ResponseError == ResponseError.Http)
                {
                    if (string.IsNullOrWhiteSpace(orderResponse.Raw))
                    {
                        return StatusCode((int)orderResponse.HttpStatusCode);
                    }
                    else
                    {
                        return StatusCode((int)orderResponse.HttpStatusCode, orderResponse.Raw);
                    }
                }

                if (!string.IsNullOrWhiteSpace(orderResponse.Error))
                {
                    return StatusCode(500, orderResponse.Error);
                }

                return StatusCode(500, "unexpected response received while executing the request");
            }

            //Completed and Canceled orders cannot be updated
            if (new[] { "Completed", "Canceled" }.Any(x => x.Contains(orderResponse.Content.State)))
            {
                ModelState.AddModelError("", "Order is " + orderResponse.Content.State);

                return BadRequest(ModelState);
            }

            //State Change
            if (request.State =="Canceled")
            {
                //Update Product
                apiInfo = _apiOrchestrator.GetApi("ProductService");
                gwRouteInfo = apiInfo.Mediator.GetRoute("product" + GatewayVerb.PUT);
                routeInfo = gwRouteInfo.Route;

                _logger.LogApiInfo($"{apiInfo.BaseUrl}{routeInfo.Path}");

                productResponse.Content.UnitsInStock = productResponse.Content.UnitsInStock + orderResponse.Content.Quantity;

                var responseProductCancel = await _apiClient.EditAsync<ProductEntity, ProductEntity>(
                   baseUrl: $"{apiInfo.BaseUrl}", productResponse.Content, pathWithQuery: $"{string.Format(routeInfo.Path, productResponse.Content.Id)}");

                _logger.LogApiInfo($"{apiInfo.BaseUrl}{routeInfo.Path}/{request.ProductId}", false);

                if (responseProductCancel.IsError)
                {
                    if (responseProductCancel.ResponseError == ResponseError.Http)
                    {
                        if (string.IsNullOrWhiteSpace(responseProductCancel.Raw))
                        {
                            return StatusCode((int)responseProductCancel.HttpStatusCode);
                        }
                        else
                        {
                            return StatusCode((int)responseProductCancel.HttpStatusCode, responseProductCancel.Raw);
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(responseProductCancel.Error))
                    {
                        return StatusCode(500, responseProductCancel.Error);
                    }

                    return StatusCode(500, "unexpected response received while executing the request");
                }

                //Update Order
                apiInfo = _apiOrchestrator.GetApi("OrderService");
                gwRouteInfo = apiInfo.Mediator.GetRoute("order" + GatewayVerb.PUT);
                routeInfo = gwRouteInfo.Route;

                _logger.LogApiInfo("OrderService", "order", null, request);

               var orderCancelResponse = await _apiClient.EditAsync<OrderEntity, OrderEntity>(
                baseUrl: $"{apiInfo.BaseUrl}", request, pathWithQuery: $"{string.Format(routeInfo.Path, id)}");

                _logger.LogApiInfo($"{apiInfo.BaseUrl}{routeInfo.Path}", false);

                if (orderCancelResponse.IsError)
                {
                    if (orderCancelResponse.ResponseError == ResponseError.Http)
                    {
                        if (string.IsNullOrWhiteSpace(orderCancelResponse.Raw))
                        {
                            return StatusCode((int)orderCancelResponse.HttpStatusCode);
                        }
                        else
                        {
                            return StatusCode((int)orderCancelResponse.HttpStatusCode, orderCancelResponse.Raw);
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(orderCancelResponse.Error))
                    {
                        return StatusCode(500, orderCancelResponse.Error);
                    }

                    return StatusCode(500, "unexpected response received while executing the request");
                }

                return Ok( orderCancelResponse.Content);
            }
            else if(request.State == "Completed")
            {
                //Update Order
                apiInfo = _apiOrchestrator.GetApi("OrderService");
                gwRouteInfo = apiInfo.Mediator.GetRoute("order" + GatewayVerb.PUT);
                routeInfo = gwRouteInfo.Route;

                _logger.LogApiInfo("OrderService", "order", null, request);

                var orderCompleteResponse = await _apiClient.EditAsync<OrderEntity, OrderEntity>(
                 baseUrl: $"{apiInfo.BaseUrl}", request, pathWithQuery: $"{string.Format(routeInfo.Path, id)}");

                _logger.LogApiInfo($"{apiInfo.BaseUrl}{routeInfo.Path}", false);

                if (orderCompleteResponse.IsError)
                {
                    if (orderCompleteResponse.ResponseError == ResponseError.Http)
                    {
                        if (string.IsNullOrWhiteSpace(orderCompleteResponse.Raw))
                        {
                            return StatusCode((int)orderCompleteResponse.HttpStatusCode);
                        }
                        else
                        {
                            return StatusCode((int)orderCompleteResponse.HttpStatusCode, orderCompleteResponse.Raw);
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(orderCompleteResponse.Error))
                    {
                        return StatusCode(500, orderCompleteResponse.Error);
                    }

                    return StatusCode(500, "unexpected response received while executing the request");
                }

                return Ok(orderCompleteResponse.Content);
            }
     
            //Product changed
            if (orderResponse.Content.ProductId != request.ProductId)
            {
                //Get Product
                apiInfo = _apiOrchestrator.GetApi("ProductService");
                gwRouteInfo = apiInfo.Mediator.GetRoute("product" + GatewayVerb.GET);
                routeInfo = gwRouteInfo.Route;

                _logger.LogApiInfo($"{apiInfo.BaseUrl}{routeInfo.Path}");

                var oldProductResponse = await _apiClient.GetAsync<ProductEntity>(
                baseUrl: $"{apiInfo.BaseUrl}", pathWithQuery: $"{routeInfo.Path}/{orderResponse.Content.ProductId}");

                _logger.LogApiInfo($"{apiInfo.BaseUrl}{routeInfo.Path}/{orderResponse.Content.ProductId}", false);

                //Check stock
                if (request.Quantity > productResponse.Content.UnitsInStock)
                {
                    ModelState.AddModelError("Quantity", "Not Enough Units Available");

                    return BadRequest(ModelState);
                }

                //Update Order
                apiInfo = _apiOrchestrator.GetApi("OrderService");
                gwRouteInfo = apiInfo.Mediator.GetRoute("order" + GatewayVerb.PUT);
                routeInfo = gwRouteInfo.Route;

                _logger.LogApiInfo("OrderService", "order", null, request);

                orderResponse = await _apiClient.EditAsync<OrderEntity, OrderEntity>(
                baseUrl: $"{apiInfo.BaseUrl}", request, pathWithQuery: $"{string.Format(routeInfo.Path, id)}");

                _logger.LogApiInfo($"{apiInfo.BaseUrl}{routeInfo.Path}", false);

                if (orderResponse.IsError)
                {
                    if (orderResponse.ResponseError == ResponseError.Http)
                    {
                        if (string.IsNullOrWhiteSpace(orderResponse.Raw))
                        {
                            return StatusCode((int)orderResponse.HttpStatusCode);
                        }
                        else
                        {
                            return StatusCode((int)orderResponse.HttpStatusCode, orderResponse.Raw);
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(orderResponse.Error))
                    {
                        return StatusCode(500, orderResponse.Error);
                    }

                    return StatusCode(500, "unexpected response received while executing the request");
                }

                //Update old Product
                apiInfo = _apiOrchestrator.GetApi("ProductService");
                gwRouteInfo = apiInfo.Mediator.GetRoute("product" + GatewayVerb.PUT);
                routeInfo = gwRouteInfo.Route;

                _logger.LogApiInfo($"{apiInfo.BaseUrl}{routeInfo.Path}");

                oldProductResponse.Content.UnitsInStock = (oldProductResponse.Content.UnitsInStock + orderResponse.Content.Quantity);

                var updateOldProductResponse = await _apiClient.EditAsync<ProductEntity, ProductEntity>(
                   baseUrl: $"{apiInfo.BaseUrl}", oldProductResponse.Content, pathWithQuery: $"{string.Format(routeInfo.Path, orderResponse.Content.ProductId)}");

                _logger.LogApiInfo($"{apiInfo.BaseUrl}{routeInfo.Path}/{request.ProductId}", false);

                if (updateOldProductResponse.IsError)
                {
                    if (updateOldProductResponse.ResponseError == ResponseError.Http)
                    {
                        if (string.IsNullOrWhiteSpace(updateOldProductResponse.Raw))
                        {
                            return StatusCode((int)updateOldProductResponse.HttpStatusCode);
                        }
                        else
                        {
                            return StatusCode((int)updateOldProductResponse.HttpStatusCode, updateOldProductResponse.Raw);
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(updateOldProductResponse.Error))
                    {
                        return StatusCode(500, updateOldProductResponse.Error);
                    }

                    return StatusCode(500, "unexpected response received while executing the request");
                }

                //Update new Product
                productResponse.Content.UnitsInStock = productResponse.Content.UnitsInStock - request.Quantity;

                _logger.LogApiInfo($"{apiInfo.BaseUrl}{routeInfo.Path}");

                var updateNewProductResponse = await _apiClient.EditAsync<ProductEntity, ProductEntity>(
                   baseUrl: $"{apiInfo.BaseUrl}", productResponse.Content, pathWithQuery: $"{string.Format(routeInfo.Path, request.ProductId)}");

                _logger.LogApiInfo($"{apiInfo.BaseUrl}{routeInfo.Path}/{request.ProductId}", false);

                if (updateNewProductResponse.IsError)
                {
                    if (updateNewProductResponse.ResponseError == ResponseError.Http)
                    {
                        if (string.IsNullOrWhiteSpace(updateNewProductResponse.Raw))
                        {
                            return StatusCode((int)updateNewProductResponse.HttpStatusCode);
                        }
                        else
                        {
                            return StatusCode((int)updateNewProductResponse.HttpStatusCode, updateNewProductResponse.Raw);
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(updateNewProductResponse.Error))
                    {
                        return StatusCode(500, updateNewProductResponse.Error);
                    }

                    return StatusCode(500, "unexpected response received while executing the request");
                }

                return Ok(orderResponse.Content);
            }

            var differenceInOrder = request.Quantity - orderResponse.Content.Quantity;

            //Check stock
            if (differenceInOrder > productResponse.Content.UnitsInStock)
            {
                ModelState.AddModelError("Quantity", "Not Enough Units Available");

                return BadRequest(ModelState);
            }

            //Update Order
            apiInfo = _apiOrchestrator.GetApi("OrderService");
            gwRouteInfo = apiInfo.Mediator.GetRoute("order" + GatewayVerb.PUT);
            routeInfo = gwRouteInfo.Route;

            _logger.LogApiInfo("OrderService", "order", null, request);

            orderResponse = await _apiClient.EditAsync<OrderEntity, OrderEntity>(
            baseUrl: $"{apiInfo.BaseUrl}", request, pathWithQuery: $"{string.Format(routeInfo.Path, id)}");

            _logger.LogApiInfo($"{apiInfo.BaseUrl}{routeInfo.Path}", false);

            if (orderResponse.IsError)
            {
                if (orderResponse.ResponseError == ResponseError.Http)
                {
                    if (string.IsNullOrWhiteSpace(orderResponse.Raw))
                    {
                        return StatusCode((int)orderResponse.HttpStatusCode);
                    }
                    else
                    {
                        return StatusCode((int)orderResponse.HttpStatusCode, orderResponse.Raw);
                    }
                }

                if (!string.IsNullOrWhiteSpace(orderResponse.Error))
                {
                    return StatusCode(500, orderResponse.Error);
                }

                return StatusCode(500, "unexpected response received while executing the request");
            }

            //Update Product
            apiInfo = _apiOrchestrator.GetApi("ProductService");
            gwRouteInfo = apiInfo.Mediator.GetRoute("product" + GatewayVerb.PUT);
            routeInfo = gwRouteInfo.Route;

            _logger.LogApiInfo($"{apiInfo.BaseUrl}{routeInfo.Path}");

            productResponse.Content.UnitsInStock = productResponse.Content.UnitsInStock - differenceInOrder;

            var response = await _apiClient.EditAsync<ProductEntity, ProductEntity>(
               baseUrl: $"{apiInfo.BaseUrl}", productResponse.Content, pathWithQuery: $"{string.Format(routeInfo.Path, productResponse.Content.Id)}");

            _logger.LogApiInfo($"{apiInfo.BaseUrl}{routeInfo.Path}/{request.ProductId}", false);

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

            return Ok(orderResponse.Content);
        }
    }
}