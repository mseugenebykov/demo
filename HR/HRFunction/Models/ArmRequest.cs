using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRFunction.Models
{
    internal class ArmRequest
    {
        public HttpRequest Request { get; private set; }

        public ArmRequest(HttpRequest request)
        {
            this.Request = request ?? throw new ArgumentNullException("request");
        }

        public string GetResourceId(string name)
        {
            var requestPath = this.GetRequestPath();
            return (string.IsNullOrWhiteSpace(requestPath)) ? name : requestPath + "/" + name;
        }

        protected string GetRequestPath()
        {
            string requestPath = null;
            if (this.Request.Headers.TryGetValue("x-ms-customproviders-requestpath", out StringValues headerValue) && (headerValue.Count > 0))
            {
                requestPath = headerValue[0];
            }
            return requestPath;
        }
    }
}
