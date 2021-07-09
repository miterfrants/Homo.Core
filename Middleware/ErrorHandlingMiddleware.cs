using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Homo.Core.Helpers;
using Homo.Core.Constants;

namespace Homo.Core.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;
        private string envName;
        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context, Microsoft.AspNetCore.Hosting.IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                envName = env.EnvironmentName;
                IOptions<AppSettings> config = serviceProvider.GetService<IOptions<AppSettings>>();
                HandleExceptionAsync(context, ex, config.Value.Common.ErrorMappingPath);
            }
        }

        private ActionResult<dynamic> HandleExceptionAsync(HttpContext context, Exception ex, string errorCodeMappingPath)
        {
            var code = HttpStatusCode.InternalServerError;
            string errorKey = "";
            string internalErrorMessage = "";
            if (envName != "dev" && envName != "Development")
            {
                internalErrorMessage = System.Web.HttpUtility.JavaScriptStringEncode(ex.Message);
            }
            else
            {
                internalErrorMessage = System.Web.HttpUtility.JavaScriptStringEncode(ex.ToString());
            }

            Dictionary<string, dynamic> payload = null;
            if (ex.GetType() == typeof(CustomException))
            {
                var customEx = (CustomException)ex;
                if (customEx.code != HttpStatusCode.OK)
                {
                    code = customEx.code;
                }
                errorKey = customEx.errorCode.ToString();
                internalErrorMessage = ErrorHelper.GetErrorMessageByCode(customEx.errorCode, errorCodeMappingPath);
                if (customEx.option != null)
                {
                    foreach (string key in customEx.option.Keys)
                    {
                        string value = "";
                        customEx.option.TryGetValue(key, out value);
                        internalErrorMessage = internalErrorMessage.Replace("{" + key + "}", value);
                    }
                }
                payload = customEx.payload;
            }
            var result = new { message = internalErrorMessage, errorKey = errorKey, stackTrace = ex.StackTrace, payload = payload };
            try
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)code.GetHashCode();
            }
            catch (System.Exception exInner)
            {
                throw new Exception(ex.ToString() + exInner.ToString());
            }
            return context.Response.WriteAsync(JsonConvert.SerializeObject(result));
        }
    }
}
