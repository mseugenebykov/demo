using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net;
using System.Text;
using HRFunction.Models;
using System.Net.Http.Headers;
using System.IO;

namespace HRFunction
{
    public static class Employees
    {
        private static readonly string ApiUri = Environment.GetEnvironmentVariable("ApiHostBaseUrl") + "Employees";

        private static HttpClient httpClient = new HttpClient();

        [FunctionName("Employees")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", "put", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Employee request. Method: " + req.Method);

            var request = new ArmRequest(req);

            switch (req.Method.ToUpperInvariant())
            {
                case "GET": return await Employees.Get(request, log);
                case "POST": return await Employees.Put(request, log);
                case "PUT": return await Employees.Put(request, log);
            }

            return new HttpResponseMessage(HttpStatusCode.BadRequest);
        }

        private static HttpClient GetHttpClient()
        {
            return GetHttpClient("demo","password");
        }

        private static HttpClient GetHttpClient(string username, string password)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}")));
            return httpClient;
        }

        private static async Task<HttpResponseMessage> Get(ArmRequest request, ILogger log)
        {
            log.LogInformation("Get employee collection.");

            var response = await GetHttpClient().GetAsync(ApiUri);

            if (response.IsSuccessStatusCode)
            {
                var employees = JsonConvert.DeserializeObject<Employee[]>(await response.Content.ReadAsStringAsync());
                var result = ArmResourceCollection<ArmEmployeeResource>.Create<Employee>(request, employees, e => (new ArmEmployeeResource(e)));

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

        private static async Task<HttpResponseMessage> Put(ArmRequest request, ILogger log)
        {
            log.LogInformation("Put new employee." );

            //var body = await request.Request.ReadAsStringAsync();
            string body;
            using (StreamReader reader = new StreamReader(request.Request.Body))
            {
                body = await reader.ReadToEndAsync();
            }

            var resource = JsonConvert.DeserializeObject<ArmResourceBase<EmployeeResource>>(body);

            var response = await GetHttpClient().PostAsJsonAsync<Employee>(ApiUri, resource.properties.GetValue());
            return (response.IsSuccessStatusCode) ?
                new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(body, Encoding.UTF8, "application/json")
                }:
                new HttpResponseMessage(response.StatusCode);
        }
    }
}
