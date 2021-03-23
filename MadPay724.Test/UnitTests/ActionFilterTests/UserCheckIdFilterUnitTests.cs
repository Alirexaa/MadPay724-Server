using MadPay724.Presentation.Helper.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MadPay724.Test.UnitTests.ActionFilterTests
{
    public class UserCheckIdFilterUnitTests
    {
        private readonly Mock<ILoggerFactory> _moqLoggerFactory;
        private readonly Mock<IHttpContextAccessor> _moqHttpContextAccessor;
        private readonly Mock<ILogger<UserCheckIdFilter>> _moqLogger;

        public UserCheckIdFilterUnitTests()
        {
            _moqLoggerFactory = new Mock<ILoggerFactory>();
            _moqHttpContextAccessor = new Mock<IHttpContextAccessor>();
            _moqLogger = new Mock<ILogger<UserCheckIdFilter>>();

        }
        [Fact]
        public async Task UserCheckIdFilter_Success_UserIdHimSelf()
        {
            //Arrange
            var userId = "5a3a2a02-7bbf-41f1-b401-25e7be899d24";
            var httpContext = new DefaultHttpContext();
            var route = new RouteData();
            route.Values.Add("id", userId);
            var context = new ActionExecutingContext(
              new ActionContext
              {
                  HttpContext = httpContext,
                  RouteData = route,
                  ActionDescriptor = new ActionDescriptor()
              },
            new List<IFilterMetadata>(),
            new Dictionary<string, object>(),
            new Mock<Controller>().Object);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,userId)
            };

            var identity = new ClaimsIdentity(claims);
            var claimsPrincipal = new ClaimsPrincipal(identity);



            _moqHttpContextAccessor.Setup(o => o.HttpContext).Returns(httpContext);
            _moqHttpContextAccessor.Setup(o => o.HttpContext.User.Identity.IsAuthenticated).Returns(true);
            _moqHttpContextAccessor.Setup(o => o.HttpContext.User).Returns(claimsPrincipal);
            _moqLoggerFactory.Setup(o => o.CreateLogger(It.IsAny<string>())).Returns(_moqLogger.Object);

            var filter = new UserCheckIdFilter(_moqLoggerFactory.Object, _moqHttpContextAccessor.Object);
            //Act
            filter.OnActionExecuting(context);
            //Assert
            Assert.Null(context.Result);
        }
        [Fact]
        public async Task UserCheckIdFilter_Fail_UserIdAnother()
        {
            //Arrange
            var userId = "5a3a2a02-7bbf-41f1-b401-25e7be899d24";
            var UserIdAnother = "5a3a2a02-7bbf-41f1-b401-299d24asdadas";
            var httpContext = new DefaultHttpContext();
            var route = new RouteData();
            route.Values.Add("id", UserIdAnother);
            var context = new ActionExecutingContext(
              new ActionContext
              {
                  HttpContext = httpContext,
                  RouteData = route,
                  ActionDescriptor = new ActionDescriptor()
              },
            new List<IFilterMetadata>(),
            new Dictionary<string, object>(),
            new Mock<Controller>().Object);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,userId)
            };

            var identity = new ClaimsIdentity(claims);
            var claimsPrincipal = new ClaimsPrincipal(identity);



            _moqHttpContextAccessor.Setup(o => o.HttpContext).Returns(httpContext);
            _moqHttpContextAccessor.Setup(o => o.HttpContext.User.Identity.IsAuthenticated).Returns(true);
            _moqHttpContextAccessor.Setup(o => o.HttpContext.User).Returns(claimsPrincipal);
            _moqLoggerFactory.Setup(o => o.CreateLogger(It.IsAny<string>())).Returns(_moqLogger.Object);

            var filter = new UserCheckIdFilter(_moqLoggerFactory.Object, _moqHttpContextAccessor.Object);
            //Act
            filter.OnActionExecuting(context);
            //Assert
            Assert.NotNull(context.Result);
            Assert.IsType<UnauthorizedResult>(context.Result);
        }
    }
}
