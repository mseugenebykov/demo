using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using HR.Identity;
using HR.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace HR.Controllers
{
    [Authorize(AuthenticationSchemes = ApiAuthDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeFunctionsController : ControllerBase
    {
        public const string ControllerClientName = "employee-functions-controller";
        private const string EmployeeFunctionName = "Employees";
        private readonly string FunctionURI = "Employees";

        private static HttpClient httpClient;

        public EmployeeFunctionsController(IConfiguration config, IHttpClientFactory clientFactory)
        {
            FunctionURI = config.GetValue<string>("FunctionSettings:URI") + EmployeeFunctionName;
            httpClient = clientFactory.CreateClient(ControllerClientName);
        }

        // GET: api/EmployeeFunctions
        [AllowAnonymous]
        [HttpGet]
        public async Task<string> Get()
        {
            var response = await httpClient.GetAsync(FunctionURI);
            return await response.Content.ReadAsStringAsync();
        }

        // GET: api/EmployeeFunctions/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/EmployeeFunctions
        [HttpPost]
        public async Task Post([FromBody] Employee value)
        {
            var response = await httpClient.PostAsJsonAsync<ArmResourceBase<EmployeeResource>>(FunctionURI, ArmResourceBase<EmployeeResource>.Create(new EmployeeResource(value)));
        }

        // PUT: api/EmployeeFunctions/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
