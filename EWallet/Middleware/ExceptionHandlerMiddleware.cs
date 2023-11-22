using EWallet.Models;
using System.ComponentModel.DataAnnotations;
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
            catch(FluentValidation.ValidationException ex)
            {
                await HandleValidationException(context, ex);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleValidationException(HttpContext context, FluentValidation.ValidationException ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int) HttpStatusCode.BadRequest;

            return context.Response.WriteAsJsonAsync(new ErrorResponse
            {
                FieldErrors = ex.Errors.ToDictionary(validationFailure => validationFailure.PropertyName,
                                                                                                        validationFailure => validationFailure.ErrorMessage)
            }); ;
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
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
