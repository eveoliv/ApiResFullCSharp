using Alura.WebAPI.Api.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Alura.WebAPI.Api.Filtros
{
    public class ErrorResponseFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            context.Result = 
                new ObjectResult(ErrorResponse.From(context.Exception))
                { StatusCode = 500 };
        }
    }
}
