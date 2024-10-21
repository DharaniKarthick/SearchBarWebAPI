using SearchBarWebAPI.Search.Core.Model;
using SearchBarWebAPI.Search.Core.Utility;
using Serilog;
using System.Diagnostics;
using System.Net;
using System.Text.Json;

namespace SearchBarWebAPI.Search.API.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
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

        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            // Create an instance of ErrorHandling to store error details
            var errorHandling = new ErrorHandling();
            StackTrace st = new StackTrace(ex, true);

            // Get the first stack frame
            StackFrame frame = st.GetFrame(0);

            // Get the class name
            string className = frame.GetMethod().ReflectedType.FullName;

            // Get the method name
            string methodName = frame.GetMethod().Name;

            // Get the line number from the stack frame
            int line = frame.GetFileLineNumber();

            // Handle Exception
            errorHandling.Id = Guid.NewGuid().ToString();
            errorHandling.Message = ex.Message;
            errorHandling.SystemMessage = ex.InnerException != null ? ex.InnerException.Message + Environment.NewLine : string.Empty;
            errorHandling.Type = $"{className}:{methodName}";
            errorHandling.Line = line;

            // Create a formatted log message string
            string logMessage = $"An exception occurred: {errorHandling.Message}, Type: {errorHandling.Type}, Line: {errorHandling.Line}";

            // Log the exception details for internal tracking
            Log.Error(logMessage);
            var statusCode = ex switch
            {
                UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized, // 401
                KeyNotFoundException => (int)HttpStatusCode.NotFound,            // 404
                ArgumentException => (int)HttpStatusCode.BadRequest,             // 400
                NotFoundException => (int)HttpStatusCode.NotFound,            // 404
                BadRequestException => (int)HttpStatusCode.BadRequest,
                _ => (int)HttpStatusCode.InternalServerError                     // 500 for all other exceptions
            };
            // Create a response object with a formatted message
            var response = new
            {
                StatusCode = statusCode,
                Message = "An error occurred while processing your request. Please try again later.",
                Detailed = logMessage,
                errorHandling.Id
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
