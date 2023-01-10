using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MealsOrderAPI.Models;
namespace MealsOrderAPI.Extensions
{

    public static class HttpContextExtension
    {
        private const string _timestamp = "timestamp";
        private const string _mosErrorKeyInException = "mosError";
        private const int _queryTimeLimitSecond = 30;

        private static string ExceptionTitleMessage(HttpContext context)
        {
            string requestMethod = !string.IsNullOrEmpty(context?.Request.Method)
                ? $"{context.Request.Method}"
                : string.Empty;

            string requestPath = context?.Request.Path.ToString();

            return ("Exception occur due to : " +
                    $"{requestMethod}" +
                    $"{(!string.IsNullOrEmpty(requestMethod) && !string.IsNullOrEmpty(requestPath) ? " " : string.Empty)}" +
                    $"{requestPath}").AddPeriodAtEnd();
        }

        private static ProblemDetails GetProblemDetails(HttpContext context, int statusCode, string title, string detail)
        {
            return new ProblemDetails
            {
                Type = "https://tools.ietf.org/html/rfc7807",
                Title = title,
                Detail = detail,
                Status = statusCode,
                Instance = context?.Request.Path + context?.Request.QueryString,
            };
        }

        private static string GetDBExceptionPrefixMessage(Exception ex, int queryTimeLimitSecond)
        {
            return ex switch
            {
                TimeoutException when ex.StackTrace.Contains("Database") =>
                    "Requests has no response from Database.",
                _ => string.Empty
            };
        }

        public static ObjectResult ProblemDetailsException(this HttpContext context, Exception ex)
        {
            string _dbExceptionMessage = GetDBExceptionPrefixMessage(ex, _queryTimeLimitSecond);
            string message = string.Empty;

            if (!string.IsNullOrEmpty(_dbExceptionMessage))
                message = $"{_dbExceptionMessage}";
            else
            {
                message = ex switch
                {
                    TimeoutException => "Requests have no response.",
                    _ => message
                };
            }

            if (!string.IsNullOrEmpty(message) && !string.IsNullOrEmpty(ex.Message))
                message += " ";

            var problemDetails = new ProblemDetails
            {
                Type = "https://tools.ietf.org/html/rfc7807",
                Title = ExceptionTitleMessage(context),
                Detail = $"{(message + ex.Message).FirstCharToUpper().AddPeriodAtEnd()}\nStackTrace: \n{ex.StackTrace}",
                Status = StatusCodes.Status500InternalServerError,
                Instance = context?.Request.Path + context?.Request.QueryString
            };

            problemDetails.Extensions.Add(_timestamp, DateTime.UtcNow);
            problemDetails.Extensions.Add(_mosErrorKeyInException, string.Empty);

            return new ObjectResult(problemDetails);
        }

        public static ObjectResult ProblemDetailsError(this HttpContext context, int statusCode, string title)
        {
            var problemDetails = GetProblemDetails(
                context, statusCode, $"Error: {title.FirstCharToUpper()}".AddPeriodAtEnd(), string.Empty);
            return AddProblemDetailExtensions(problemDetails);
        }

        private static ObjectResult AddProblemDetailExtensions(ProblemDetails problemDetails)
        {
            problemDetails.Extensions.Add(_timestamp, DateTime.UtcNow);

            return new ObjectResult(problemDetails);
        }
    }
}
