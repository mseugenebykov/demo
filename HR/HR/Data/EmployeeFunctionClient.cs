using HR.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace HR.Data
{
    public class EmployeeFunctionClient
    {
        public const string DefaultClientName = "employee-functions";

        private const string FunctionURI = "/api/EmployeeFunctions";

        private static HttpClient client = new HttpClient();

        public EmployeeFunctionClient(HttpClient httpClient)
        {
            client = httpClient;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes("demo:password")));
        }

        protected static string FormatUrl(HttpContext context, string uri)
        {
            return string.Format("{0}://{1}{2}", context.Request.Scheme, context.Request.Host, uri);
        }

        public async Task<HttpResponseMessage> PostAsync(HttpContext context, Employee data)
        {
            return await client.PostAsJsonAsync<Employee>(FormatUrl(context, FunctionURI), data);
        }
    }
}
