using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace MadPay724.Presentation.Helper.Filters
{
    public class UserCheckIdFilter : ActionFilterAttribute
    {
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserCheckIdFilter(ILoggerFactory loggerFactory, IHttpContextAccessor httpContextAccessor)
        {
            _logger = loggerFactory.CreateLogger("UserCheckIdFilter");
            _httpContextAccessor = httpContextAccessor;

        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.RouteData.Values["id"] != null && context.RouteData.Values["userId"] == null )
            {
                if (_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value == context.RouteData.Values["id"].ToString())
                {
                    base.OnActionExecuting(context);
                }
                else
                {
                    _logger.LogWarning($"impermissin action :user {_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value} attempted to Update or Get UserProfile of user {context.RouteData.Values["id"]}");
                    context.Result = new UnauthorizedResult();
                }
            }
            else if (context.RouteData.Values["userId"] != null)
            {
                if (_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value == context.RouteData.Values["userId"].ToString())
                {
                    base.OnActionExecuting(context);
                }
                else
                {
                    _logger.LogWarning($"impermissin action :user {_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value} attempted to Update or Get UserProfile of user {context.RouteData.Values["userId"]}");
                    context.Result = new UnauthorizedResult();
                }
            }

        }
    }
}
