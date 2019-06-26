using System;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace RaspPiTest.Middleware
{
    public class ApiException : Exception
    {
        private static string Serialize(ErrorResult result) => JsonConvert.SerializeObject(result, new JsonSerializerSettings {ContractResolver = new CamelCasePropertyNamesContractResolver()});

        public int StatusCode { get; }
        public string ContentType { get; } = @"text/plain";

        public ApiException(int statusCode, string message = null, Exception innerException = null)
            : base(message, innerException)
        {
            StatusCode = statusCode;
        }

        public ApiException(int statusCode, ErrorResult result) : this(statusCode, Serialize(result))
        {
            ContentType = @"application/json";
        }

        public ApiException(ErrorResult result) : this(400, Serialize(result))
        {
            ContentType = @"application/json";
        }

        public static ApiException Create(Exception exception)
        {
            IfNull(exception, new ErrorResult(1, "Fehler bei der Übergabe einer Ausnahme."));

            return exception is ApiException apiException
                ? apiException
                : new ApiException((int) HttpStatusCode.InternalServerError, new ErrorResult(0, exception.Message));
        }

        public static void If(bool condition, ErrorResult errorResult, string message = null)
        {
            if (condition)
            {
                if (!string.IsNullOrEmpty(message)) errorResult.ErrorDescription = message;
                throw new ApiException(errorResult);
            }
        }

        public static void IfNull(object value, ErrorResult errorResult, string message = null) => If(value == null, errorResult, message);
    }
}