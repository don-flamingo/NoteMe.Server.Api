using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using NLog;
using NoteMe.Common.Exceptions;
using NoteMe.Common.Services.Json;

namespace NoteMe.Server.Api.Middlewares
{
    public class ExceptionMiddleware : IMiddleware
    {
        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next.Invoke(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }
        
        public Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var statusCode = HttpStatusCode.BadRequest;
            var errorCode = "internal_server_error";
            var details = exception.Message as object;
            var stack = exception.StackTrace;

            switch (exception)
            {
                case ServerException serverException:
                    errorCode = serverException.Code;
                    break;
                default:
                    var errorMessage = $"Unexpected exception {exception} with stack: {exception.StackTrace} with source: {exception.Source}";
                    _logger.Fatal(errorMessage);
                    break;
            }

            var response = new
            {
                ErrorCode = errorCode,
                Details = details,
                Stack = stack
            };

            var payload = JsonSerializeService.Serialize(response);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int) statusCode;

            return context.Response.WriteAsync(payload);
        }
    }
}