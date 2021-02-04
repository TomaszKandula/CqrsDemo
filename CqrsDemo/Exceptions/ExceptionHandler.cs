using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Newtonsoft.Json;
using CqrsDemo.Models;
using CqrsDemo.Shared.Resources;

namespace CqrsDemo.Exceptions
{
    public static class ExceptionHandler
    {
        public static void Handle(IApplicationBuilder AApplication)
        {
            AApplication.Run(async AHttpContext =>
            {
                var LExceptionHandlerPathFeature = AHttpContext.Features.Get<IExceptionHandlerPathFeature>();
                var LException = LExceptionHandlerPathFeature.Error;
                AHttpContext.Response.ContentType = "application/json";

                string LResult;
                switch (LException)
                {
                    case BusinessException AException:
                        {
                            var LAppError = new ApplicationError(AException.ErrorCode, AException.Message);
                            LResult = JsonConvert.SerializeObject(LAppError);
                            AHttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                            break;
                        }

                    default:
                        {
                            var LAppError = new ApplicationError(nameof(ErrorCodes.ERROR_UNEXPECTED), ErrorCodes.ERROR_UNEXPECTED);
                            LResult = JsonConvert.SerializeObject(LAppError);
                            AHttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            break;
                        }
                }
                await AHttpContext.Response.WriteAsync(LResult);
            });
        }
    }
}
