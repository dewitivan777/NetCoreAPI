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
                        Path = "ClassificationService/category"
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
                        Path = "ClassificationService/Supplier"
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
                    .AddRoute("Supplier", GatewayVerb.POST,
                    new RouteInfo
                    {
                        Path = "ProductService/product"
                    })
                    .AddRoute("Supplier", GatewayVerb.DELETE,
                    new RouteInfo
                    {
                        Path = "ProductService/product"
                    })
                    .AddRoute("Supplier", GatewayVerb.PUT,
                    new RouteInfo
                    {
                        Path = "ProductService/product"
                    });

        }
    }
}
