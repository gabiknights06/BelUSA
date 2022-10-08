using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TaxCalculation.Persistent.Exceptions;

namespace TaxCalculation.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);

                var c = httpContext;
                await HandleAuthenticationError(httpContext);
            }
            catch (Exception ex)
            {
                await HandleException(httpContext, ex);
            }
        }
        private Task HandleAuthenticationError(HttpContext context)
        {
            ErrorResponse errorResponse = null;
            if (context.Response.StatusCode == 403)
            {
                errorResponse = ErrorResponse.PermissionError(context);
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                Log.Warning("Forbidden request information {RequestMethod} {RequestPath} {statusCode}", context.Request.Method, context.Request.Path, context.Response.StatusCode);
            }
            else if (context.Response.StatusCode == 401)
            {
                errorResponse = ErrorResponse.AuthenticationError(context);
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                Log.Warning("Unauthorized request information {RequestMethod} {RequestPath} {statusCode}", context.Request.Method, context.Request.Path, context.Response.StatusCode);
            }

            if (errorResponse != null)
            {
                context.Response.ContentType = "application/problem+json";

                var result = JsonConvert.SerializeObject(errorResponse, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    ContractResolver = new DefaultContractResolver
                    {
                        NamingStrategy = new SnakeCaseNamingStrategy(true, true)
                    }
                });

                return context.Response.WriteAsync(result);
            }
            else
            {
                return Task.CompletedTask;
            }
        }
        private Task HandleException(HttpContext context, Exception exception)
        {
            ErrorResponse errorResponse = null;
                       
            if (exception is TaxValidatorException)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;

                errorResponse = ErrorResponse.ValidationError(context, exception.Message);

                Log.Error(exception, "One or more validation errors occured {message} {innerException}", exception.Message, exception.InnerException);
            }
            else if (exception is BadGateWayException)
            {
                context.Response.StatusCode = StatusCodes.Status502BadGateway;

                errorResponse = ErrorResponse.BadGateway(context);

                Log.Error(exception, "BadGateway request {message} {innerException}", exception.Message, exception.InnerException);
            }

            else if (exception is BadRequestException)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;

                if (exception.Message != null || exception.Message != string.Empty)
                {
                    errorResponse = ErrorResponse.BadRequestError(context, exception.Message);
                }
                else
                {
                    errorResponse = ErrorResponse.BadRequestError(context);
                }

                Log.Error(exception, "Bad request occured {message} {innerException}", exception.Message, exception.InnerException);
            }

            else if (exception is Exception)
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                errorResponse = ErrorResponse.InternalServerError(context);

                Log.Error(exception, "Exception occured {message} {innerException}", exception.Message, exception.InnerException);
            }

            context.Response.ContentType = "application/problem+json";

            var result = JsonConvert.SerializeObject(errorResponse, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new SnakeCaseNamingStrategy(true, true)
                }
            });

            return context.Response.WriteAsync(result);
        }
    }

    public class ErrorResponse
    {
        private ErrorResponse(string errorCode, string errorDescription, string traceId)
        {
            ErrorCode = errorCode;
            ErrorDescription = errorDescription;
            TraceId = traceId;
        }
        public string ErrorCode { get; private set; }
        public string ErrorDescription { get; private set; }
        public string TraceId { get; private set; }

        public static ErrorResponse AuthenticationError(HttpContext context)
        {
            string errorCode = "authentication_error";
            string description = "You don't have a valid api key to access the API. You may not supplied an api key or your api key has expired.";
            var traceId = Activity.Current?.RootId ?? context?.TraceIdentifier;
            return new ErrorResponse(errorCode, description, traceId);
        }
        public static ErrorResponse PermissionError(HttpContext context)
        {
            string errorCode = "access_denied";
            string description = "You don't have permission to access the Resource.";
            var traceId = Activity.Current?.RootId ?? context?.TraceIdentifier;
            return new ErrorResponse(errorCode, description, traceId);
        }
        public static ErrorResponse BadGateway(HttpContext context)
        {
            string errorCode = "bad_gate_way";
            string description = "An unhandled exception has occurred while executing the request.";
            var traceId = Activity.Current?.RootId ?? context?.TraceIdentifier;
            return new ErrorResponse(errorCode, description, traceId);
        }
        public static ErrorResponse BadGateway(HttpContext context, string service)
        {
            string errorCode = "bad_gate_way";
            string description = $@"An unhandled exception has occurred while requesting in {service}.";
            var traceId = Activity.Current?.RootId ?? context?.TraceIdentifier;
            return new ErrorResponse(errorCode, description, traceId);
        }      
        public static ErrorResponse ValidationError(HttpContext context, string message)
        {
            string errorCode = "validation_error";
            string description = message;
            var traceId = Activity.Current?.RootId ?? context?.TraceIdentifier;
            return new ErrorResponse(errorCode, description, traceId);
        }
        public static ErrorResponse InternalServerError(HttpContext context)
        {
            string errorCode = "internal_server_error";
            string description = "An unhandled exception has occurred while executing the request.";
            var traceId = Activity.Current?.RootId ?? context?.TraceIdentifier;
            return new ErrorResponse(errorCode, description, traceId);
        }
        public static ErrorResponse BadRequestError(HttpContext context)
        {
            string errorCode = "bad_request_error";
            string description = "An unhandled exception has occurred while executing the request.";
            var traceId = Activity.Current?.RootId ?? context?.TraceIdentifier;
            return new ErrorResponse(errorCode, description, traceId);
        }
        public static ErrorResponse BadRequestError(HttpContext context, string message)
        {
            string errorCode = "bad_request_error";
            string description = message;
            var traceId = Activity.Current?.RootId ?? context?.TraceIdentifier;
            return new ErrorResponse(errorCode, description, traceId);
        }

    }



}
