using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using Microsoft.Extensions.Primitives;
using System.Net;
using System.Text;
using HRFunction.Models;

namespace HRFunction
{
    public static class Employees
    {
        private const string ApiUri = "Employees";

        [FunctionName("Employees")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Employee request. Method: " + req.Method);

            string requestPath = null;
            StringValues headerValue;
            if (req.Headers.TryGetValue("x-ms-customproviders-requestpath", out headerValue) && (headerValue.Count > 0))
            {
                requestPath = headerValue[0];
            }

            switch (req.Method.ToUpperInvariant())
            {
                case "GET": return await Employees.Get(requestPath, log);
            }

            return new HttpResponseMessage(HttpStatusCode.BadRequest);
        }

        private static async Task<HttpResponseMessage> Get(string requestPath, ILogger log)
        {
            string result = null;
            string apiHost = Environment.GetEnvironmentVariable("ApiHostBaseUrl");

            log.LogInformation("Get employee collection request.");

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(apiHost + ApiUri);

                if (response.IsSuccessStatusCode)
                {
                    var resultObjects = JsonConvert.DeserializeObject<Employee[]>(await response.Content.ReadAsStringAsync());
                    result = JsonConvert.SerializeObject(new EmployeeResourceCollection(requestPath, resultObjects));
                }
            }

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(result, Encoding.UTF8, "application/json")
            };
        }
    }
}
