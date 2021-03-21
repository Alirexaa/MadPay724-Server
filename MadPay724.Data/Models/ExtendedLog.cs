using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using ZNetCS.AspNetCore.Logging.EntityFrameworkCore;

namespace MadPay724.Data.Models
{
    public class ExtendedLog : Log
    {
        public string Browser { get; set; }
        public string Host { get; set; }
        public string User { get; set; }
        public string Path { get; set; }
        public ExtendedLog(IHttpContextAccessor httpContextAccessor)
        {
            string browser = httpContextAccessor.HttpContext?.Request.Headers["User-Agent"];
            if (!string.IsNullOrEmpty(browser) && browser.Length > 255)
            {
                browser.Substring(0, 255);
            }
            this.Browser = browser;
            this.Host = httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress.ToString();
            this.User = httpContextAccessor.HttpContext?.User?.Identity.Name;
            this.Path = httpContextAccessor.HttpContext?.Request.Path;

        }
        public ExtendedLog()
        {

        }
    }
}
