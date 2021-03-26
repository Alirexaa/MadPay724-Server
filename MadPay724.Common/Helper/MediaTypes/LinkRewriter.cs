using MadPay724.Data.Dto.Common.ION;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace MadPay724.Common.Helper.MediaTypes
{
    public class LinkRewriter
    {
        private readonly IUrlHelper _urlHelper;
        public LinkRewriter(IUrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
        }
        public Link Rewrite(Link orginal)
        {
            if (orginal == null)
                return null;
            return new Link
            {
                Href = _urlHelper.Link(orginal.RouteName, orginal.RouteValues),
                Method = orginal.Method,
                Relations = orginal.Relations
            };
        }
    }
}
