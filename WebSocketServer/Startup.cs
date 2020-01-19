using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.WebSockets;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace WebSocketServer
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseWebSockets();

            app.Use(async (context, next) =>
            {
                WriteRequestParam(context);

                if (!context.WebSockets.IsWebSocketRequest)
                {
                    System.Console.WriteLine("Hello from the context type validation part of the middleware.");
                    await next();
                }
                else
                {
                    WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
                    System.Console.WriteLine("WebSocket connected.");
                }
            });

            app.Run(async context =>
            {
                string helloMessage = "Hello from the last part of the middleware.";
                System.Console.WriteLine(helloMessage);
                await context.Response.WriteAsync(helloMessage);
            });
        }

        public void WriteRequestParam(HttpContext context)
        {
            System.Console.WriteLine($"Request Method: { context.Request.Method }.");
            System.Console.WriteLine($"Request Protocol: { context.Request.Protocol }.");

            if (context.Request.Headers != null)
            {
                foreach (var header in context.Request.Headers)
                {
                    System.Console.WriteLine($"--> { header.Key } : { header.Value }");
                }
            }
        }
    }
}
