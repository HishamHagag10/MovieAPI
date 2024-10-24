﻿using System.Net;
using System.Text.Json;

namespace MovieAPI.Meddlewares
{
    public class ExceptionMeddleware
    {
        private RequestDelegate _next;

        private ILogger<ExceptionMeddleware> _logger;
        public ExceptionMeddleware(ILogger<ExceptionMeddleware>logger,RequestDelegate next)
        {
            _logger = logger;
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
                _logger.LogError($"Something went wrong: {ex}");
                await HandleExceptionAsync(context,ex);
            }
        }
        private async Task HandleExceptionAsync(HttpContext context, Exception exception) 
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await context.Response.WriteAsync(new ErrorDetails
            {
                StatusCode=context.Response.StatusCode,
                Message= "An unexpected error occurred. Please try again later.",

            }.ToString());;
            return;
        }
    }
    public class ErrorDetails 
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }
        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}