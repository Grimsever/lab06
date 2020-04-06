using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Lab06.MVC.BL.Filter
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<GlobalExceptionFilter> _logger;

        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            var actionName = context.ActionDescriptor.DisplayName;

            var exceptionStack = context.Exception.StackTrace;

            var exceptionMessage = context.Exception.Message;

            context.Result = new RedirectToActionResult("InfoPage", "Home",
                new InformationViewModel {Message = exceptionMessage});

            context.ExceptionHandled = true;

            _logger.LogError($"{actionName} throw exception: \n {exceptionMessage} \n {exceptionStack}");
        }
    }
}