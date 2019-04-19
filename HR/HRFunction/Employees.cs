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
        private static readonly string ApiUri = Environment.GetEnvironmentVariable("ApiHostBaseUrl") + "Employees";

        private static HttpClient httpClient = new HttpClient();

        [FunctionName("Employees")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Employee request. Method: " + req.Method);

            var request = new ArmRequest(req);

            switch (req.Method.ToUpperInvariant())
            {
                case "GET": return await Employees.Get(request, log);
                case "POST": return await Employees.Post(request, log);
            }

            return new HttpResponseMessage(HttpStatusCode.BadRequest);
        }

        private static async Task<HttpResponseMessage> Get(ArmRequest request, ILogger log)
        {
            log.LogInformation("Get employee collection.");

            var response = await httpClient.GetAsync(ApiUri);

            if (response.IsSuccessStatusCode)
            {
                var employees = JsonConvert.DeserializeObject<Employee[]>(await response.Content.ReadAsStringAsync());
                var result = ArmResourceCollection<EmployeeResource>.Create<Employee>(request, employees, e => (new EmployeeResource(e)));

                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(JsonConvert.SerializeObject(result), Encoding.UTF8, "application/json")
                };
            }
            else
            {
                return new HttpResponseMessage(response.StatusCode);
            }
        }

        private static async Task<HttpResponseMessage> Post(ArmRequest request, ILogger log)
        {
            return null;
        }
    }
}
