using ApiGateway.ApiGateway;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace ApiGateway.API
{
    public static class ApiOrchestration
    {
        public static void Create(IApiOrchestrator orchestrator, IApplicationBuilder app)
        {
            orchestrator
                //ClassificationService
                .AddApi("ClassificationService", "http://localhost:5002/")
                   //category
                   .AddRoute("category", GatewayVerb.GET,
                    new RouteInfo
                    {
                        Path = "ClassificationService/category"
                    })
                   .AddRoute("category", GatewayVerb.POST,
                    new RouteInfo
                    {
                        Path = "ClassificationService/category"
                    })
                   .AddRoute("category", GatewayVerb.DELETE,
                    new RouteInfo
                    {
                        Path = "ClassificationService/category/{0}"
                    })
                    .AddRoute("category", GatewayVerb.PUT,
                    new RouteInfo
                    {
                        Path = "ClassificationService/category/{0}"
                    })
                   //supplier
                   .AddRoute("Supplier", GatewayVerb.GET,
                    new RouteInfo
                    {
                        Path = "ClassificationService/Supplier"
                    })
                    .AddRoute("Supplier", GatewayVerb.POST,
                    new RouteInfo
                    {
                        Path = "ClassificationService/Supplier"
                    })
                    .AddRoute("Supplier", GatewayVerb.DELETE,
                    new RouteInfo
                    {
                        Path = "ClassificationService/Supplier/{0}"
                    })
                    .AddRoute("Supplier", GatewayVerb.PUT,
                    new RouteInfo
                    {
                        Path = "ClassificationService/Supplier/{0}"
                    })
                    //ProductService
                    .AddApi("ProductService", "http://localhost:5003/")
                    //product
                    .AddRoute("product", GatewayVerb.GET,
                    new RouteInfo
                    {
                        Path = "ProductService/product"
                    })
                    .AddRoute("product", GatewayVerb.POST,
                    new RouteInfo
                    {
                        Path = "ProductService/product"
                    })
                    .AddRoute("product", GatewayVerb.DELETE,
                    new RouteInfo
                    {
                        Path = "ProductService/product/{0}"
                    })
                    .AddRoute("product", GatewayVerb.PUT,
                    new RouteInfo
                    {
                        Path = "ProductService/product/{0}"
                    })
                    //ProductService
                    .AddApi("OrderService", "http://localhost:5004/")
                    //order
                    .AddRoute("order", GatewayVerb.GET,
                    new RouteInfo
                    {
                        Path = "OrderService/order"
                    })
                    .AddRoute("order", GatewayVerb.POST,
                    new RouteInfo
                    {
                        Path = "OrderService/order"
                    })
                    .AddRoute("order", GatewayVerb.DELETE,
                    new RouteInfo
                    {
                        Path = "OrderService/order"
                    })
                    .AddRoute("order", GatewayVerb.PUT,
                    new RouteInfo
                    {
                        Path = "OrderService/order/{0}"
                    });
        }
    }
}
