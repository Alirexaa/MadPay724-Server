using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace MadPay724.Data.Dto.Common.ION
{
    public class Link
    {
        public const string GetMethod = "GET";

        public static Link To(string routeName, object routValues = null)
            => new Link
            {
                RouteName = routeName,
                RouteValues = routValues,
                Method = GetMethod,
                Relations = null
            };

        public static Link ToCollection(string routeName, object routValues = null)
            => new Link
            {
                RouteName = routeName,
                RouteValues = routValues,
                Method = GetMethod,
                Relations = new string[] { "collection" }
            };

        [JsonProperty(Order = -4)]
        public string Href { get; set; }


        [JsonProperty(Order = -3, NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        [DefaultValue(GetMethod)]
        public string Method { get; set; }


        [JsonProperty(Order = -2, PropertyName = "rel", NullValueHandling = NullValueHandling.Ignore)]
        public string[] Relations { get; set; }

        [JsonIgnore]
        public string RouteName { get; set; }
        [JsonIgnore]
        public object RouteValues { get; set; }

    }
}