using FluentValidation.Results;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace CoreFaces.Helper
{
    public class ResponseWrapper
    {
        private readonly RequestDelegate _next;

        public ResponseWrapper(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var currentBody = context.Response.Body;

            using (var memoryStream = new MemoryStream())
            {
                //set the current response to the memorystream.
                context.Response.Body = memoryStream;

                await _next(context);

                //reset the body 
                context.Response.Body = currentBody;
                memoryStream.Seek(0, SeekOrigin.Begin);

                var readToEnd = new StreamReader(memoryStream).ReadToEnd();
                var objResult = JsonConvert.DeserializeObject(readToEnd);
                var result = CommonApiResponse<dynamic>.Create(context.Response, (HttpStatusCode)context.Response.StatusCode, false, objResult, "");
                await context.Response.WriteAsync(JsonConvert.SerializeObject(result));
            }
        }

    }

    public static class ResponseWrapperExtensions
    {
        public static IApplicationBuilder UseResponseWrapper(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ResponseWrapper>();
        }
    }


    public class CommonApiResponse<TEntity>
    {
        public bool Status = false;
        public string Version => "1.0";
        public int StatusCode { get; set; }
        public string RequestId { get; }
        public List<ValidationFailure> ErrorMessage { get; set; }
        public dynamic Result { get; set; }
        public CommonApiResponse()
        { }

        public static CommonApiResponse<TEntity> Create(HttpResponse response, HttpStatusCode statusCode, bool status, TEntity result, object errorMessage)
        {
            List<ValidationFailure> errList = new List<ValidationFailure>();
            ValidationFailure err = new ValidationFailure("", errorMessage.ToString());
            errList.Add(err);

            return new CommonApiResponse<TEntity>(response, statusCode, status, result, errList);
        }


        public static CommonApiResponse<TEntity> Create(HttpResponse response, HttpStatusCode statusCode, bool status, TEntity result, List<ValidationFailure> errorMessage)
        {
            return new CommonApiResponse<TEntity>(response, statusCode, status, result, errorMessage);
        }


        protected CommonApiResponse(HttpStatusCode statusCode, bool status, TEntity result, List<ValidationFailure> errorMessage)
        {
            RequestId = Guid.NewGuid().ToString();
            StatusCode = (int)statusCode;
            Result = result;
            ErrorMessage = errorMessage;
            Status = status;
        }

        protected CommonApiResponse(HttpResponse response, HttpStatusCode statusCode, bool status, TEntity result, List<ValidationFailure> errorMessage)
        {
            RequestId = Guid.NewGuid().ToString();
            StatusCode = (int)statusCode;
            Result = result;
            ErrorMessage = errorMessage;
            Status = status;
            try
            {
                response.StatusCode = (int)statusCode;
            }
            catch (Exception)
            {

            }
        }

    }
}
