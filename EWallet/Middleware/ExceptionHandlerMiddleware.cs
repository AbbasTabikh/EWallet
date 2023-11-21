using EWallet.Models;
using System.Net;

namespace EWallet.Middleware
{
    public class ExceptionHandlerMiddleware 
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }
        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;

            return context.Response.WriteAsJsonAsync(new ErrorResponse()
            {
                ErrorMessage = "An error occured while processing your request"
            });

        }
    }
}
