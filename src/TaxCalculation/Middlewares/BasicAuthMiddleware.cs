using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaxCalculation.Middlewares
{
    public class BasicAuthMiddleware
    {
        private readonly RequestDelegate _next;

        public BasicAuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            string authHeader = context.Request.Headers["Authorization"];

            // Call the next delegate/middleware in the pipeline

            if (authHeader != null && authHeader.StartsWith("Bearer "))
            {
                string token = authHeader.Substring("Bearer ".Length).Trim();

                if (token == "5da2f821eee4035db4771edab942a4cc")
                    await _next(context);
                else
                {
                    context.Response.StatusCode = 401;
                    return;
                }
            }
            else
            {
                string authQuery = context.Request.Query["Authorization"];

                if (authQuery == "5da2f821eee4035db4771edab942a4cc")
                    await _next(context);
                else
                {
                    context.Response.StatusCode = 401;
                    return;
                }
            }
        }
    }
}
