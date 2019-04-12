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

namespace HRFunction
{
    public static class GetEmployeeCollection
    {
        private const string ApiUri = "Employees";

        [FunctionName("GetEmployeeCollection")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            string result = null;
            string apiHost = Environment.GetEnvironmentVariable("ApiHostBaseUrl");

            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestPath = null;
            StringValues headerValue;
            if (req.Headers.TryGetValue("x-ms-requestpath", out headerValue) && (headerValue.Count > 0))
            {
                requestPath = headerValue[0];
            }

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(apiHost + ApiUri);

                if (response.IsSuccessStatusCode)
                {
                    var resultObjects = JsonConvert.DeserializeObject<Models.Employee[]>(await response.Content.ReadAsStringAsync());
                    var resultResources = new Models.EmployeeResource[resultObjects.Length];
                    for (int i = 0; i < resultObjects.Length; i++) resultResources[i] = new Models.EmployeeResource(requestPath, resultObjects[i]);
                    result = JsonConvert.SerializeObject(resultResources);
                }
            }

            return new OkObjectResult(result);
        }
    }
}
