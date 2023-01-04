using System.Net;
using Microsoft.AspNetCore.Http;
using Serilog.Context;

using Serilog;
using SH.Bookstore.Shared.Identity.RequestCurrentUser;
using SH.Bookstore.Shared.Common.Services.Serialize;
using SH.Bookstore.Shared.Wrapper;
using SH.Bookstore.Shared.Exceptions;

namespace SH.Bookstore.Shared.Middlewares;
internal class ExceptionMiddleware : IMiddleware
{
    private readonly ICurrentUser _currentUser;
    private readonly ISerializerService _jsonSerializer;

    public ExceptionMiddleware(
        ICurrentUser currentUser,
        ISerializerService jsonSerializer)
    {
        _currentUser = currentUser;
        _jsonSerializer = jsonSerializer;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            string email = _currentUser.GetUserEmail() is string userEmail ? userEmail : "Anonymous";
            var userId = _currentUser.GetUserId();
            if (userId != Guid.Empty) LogContext.PushProperty("UserId", userId);
            LogContext.PushProperty("UserEmail", email);
            string errorId = Guid.NewGuid().ToString();
            LogContext.PushProperty("ErrorId", errorId);
            LogContext.PushProperty("StackTrace", exception.StackTrace);

            var errorResult = await Result.FailAsync($"Source: {exception.TargetSite?.DeclaringType?.FullName}");
            errorResult.Messages!.Add($"Exception: {exception.Message}");
            var response = context.Response;
            response.ContentType = "application/json";
            if (exception is not CustomException && exception.InnerException != null)
                while (exception.InnerException != null)
                    exception = exception.InnerException;

            switch (exception)
            {
                case CustomException e:
                    response.StatusCode = (int)e.StatusCode;
                    if (e.ErrorMessages is not null)
                        errorResult.Messages = e.ErrorMessages;

                    break;

                case KeyNotFoundException:
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;

                default:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            Log.Error($"{errorResult.Messages.Last()} Request failed with Status Code {context.Response.StatusCode} and Error Id {errorId}.");
            await response.WriteAsync(_jsonSerializer.Serialize(errorResult));
        }
    }
}
