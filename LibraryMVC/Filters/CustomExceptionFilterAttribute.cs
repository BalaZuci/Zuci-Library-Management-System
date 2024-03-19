using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace LibraryMVC.Filters
{
    public class CustomExceptionFilterAttribute : IExceptionFilter
    {
        private readonly ILogger<CustomExceptionFilterAttribute> _logger;
        private readonly IModelMetadataProvider _metadataProvider;
        public CustomExceptionFilterAttribute(ILogger<CustomExceptionFilterAttribute> logger, IModelMetadataProvider metadataProvider)
        {
            _logger = logger;
            _metadataProvider = metadataProvider;
        }
        public void OnException(ExceptionContext context)
        {
            var result = new ViewResult { ViewName = "Error" };
            result.ViewData = new ViewDataDictionary(_metadataProvider, context.ModelState);
            result.ViewData.Add("ErrorMessage", context.Exception.Message);
            _logger.LogError("Error: " + context.Exception.Message);
            context.ExceptionHandled = true;
            context.Result = result;
        }
    }
}
